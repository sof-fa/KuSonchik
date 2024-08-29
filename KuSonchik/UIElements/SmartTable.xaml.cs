using KuSonchik.Models;
using KuSonchik.Utilities;
using KuSonchik.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KuSonchik.UIElements
{
	/// <summary>
	/// Логика взаимодействия для SmartTable.xaml
	/// </summary>
	public partial class SmartTable : UserControl
	{
		public SmartTable()
		{
			InitializeComponent();
			Init();
		}

		private Type CurrentType { get; set; }
		private Dictionary<PropertyInfo, int> PosProperty { get; set; } = new();
		private Dictionary<string, int> StatusToSequenceNumber { get; set; } = new Dictionary<string, int>
		{
			{ "Добавлен в корзину", 0 },
            { "Оплачен", 1 },
            { "Доставка", 2 },
            { "Получен", 3 },
            { "Получен (2 недели)", 4 },
            { "Возврат", 5 },
        };

		private void Init()
		{
			Table.ItemsSource = ItemsSource;
		}

		public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(IList), typeof(SmartTable),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(ItemsSourcePropertyChangedCallback)));
		public IList ItemsSource
		{
			get => (IList)GetValue(ItemsSourceProperty);
			set => SetValue(ItemsSourceProperty, value);
		}
		private static void ItemsSourcePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var table = (SmartTable)d;
			table.Table.ItemsSource = e.NewValue as IList;
			if (table.Table.Items.Count > 0)
				table.CurrentType = table.Table.Items[0].GetType();
			table.GenerateSelectorPanel();
		}


		public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(SmartTable));
		public object SelectedItem
		{
			get => GetValue(SelectedItemProperty);
			set => SetValue(SelectedItemProperty, value);
		}


		private void GenerateSelectorPanel()
		{
			if (CurrentType == null) return;
			SelectorPanel.ColumnDefinitions.Clear();
			SelectorPanel.Children.Clear();
			PosProperty.Clear();
			var properties = CurrentType.GetProperties();
			var styleCheckBox = Resources["CheckBoxStyle"] as Style;
			for (int i = 0, row = 0; i < properties.Length; ++i)
			{
				var navigAttr = properties[i].GetCustomAttribute<IsNavigationAttribute>();
				if (navigAttr != null && navigAttr.IsNavigation) continue;
				SelectorPanel.ColumnDefinitions.Add(new ColumnDefinition());
				var nameAttr = properties[i].GetCustomAttribute<ColumnPropertiesAttribute>();
				var checkBox = new CheckBox
				{
					Name = properties[i].Name + "CheckBox",
					Content = nameAttr.Name,
					Style = styleCheckBox,
					IsChecked = true
				};
                checkBox.Click += CheckBox_Click;
				Grid.SetColumn(checkBox, row);
				SelectorPanel.Children.Add(checkBox);
				PosProperty[properties[i]] = row;
				++row;
			}
		}

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            var nameProperty = checkbox.Name.Substring(0, checkbox.Name.Length - "CheckBox".Length);
            var posProperty = PosProperty[CurrentType.GetProperty(nameProperty)];
            if (checkbox.IsChecked == true)
                Table.Columns[posProperty].Visibility = Visibility.Visible;
            else
                Table.Columns[posProperty].Visibility = Visibility.Hidden;
        }

        private void Table_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
		{
			var desc = e.PropertyDescriptor as PropertyDescriptor;
			if (desc == null) return;
			var attProps = desc.Attributes[typeof(ColumnPropertiesAttribute)] as ColumnPropertiesAttribute;
			if (attProps != null)
			{
                e.Column.Header = attProps.Name;
                e.Column.Width = new DataGridLength(attProps.WidthInStars, DataGridLengthUnitType.Star);
            }
			
			var attVisibility = desc.Attributes[typeof(VisibilityColumnAttribute)] as VisibilityColumnAttribute;
			if (attVisibility != null)
				e.Column.Visibility = attVisibility.Visibility;

			
		}

		private void Table_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
		{
			var datagrid = (DataGrid)sender;
			SelectedItem = datagrid.SelectedItem;
		}

        private void Table_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
			var selectedItem = (sender as DataGrid).SelectedItem;
			if (selectedItem is not Order order) return;
			var rand = new Random();
			DateTime? paidDate = null;
            DateTime? delieveryDate = null;
            DateTime? receivedDate = null;
            DateTime? receivedDate2Week = null;
            DateTime? returnedDate = null;

			int currentStatusNumber = StatusToSequenceNumber[order.Status];

            if (StatusToSequenceNumber["Оплачен"] <= currentStatusNumber)
				paidDate = (DateTime)order.Date?.AddMinutes(rand.Next(5, 7200));
			if (StatusToSequenceNumber["Доставка"] <= currentStatusNumber)
				delieveryDate = paidDate?.AddDays(rand.Next(1, 7));
			if (StatusToSequenceNumber["Получен"] <= currentStatusNumber)
				receivedDate = delieveryDate?.AddDays(rand.Next(0, 3));
			if (StatusToSequenceNumber["Получен (2 недели)"] <= currentStatusNumber)
				receivedDate2Week = receivedDate?.AddDays(14);
			if (StatusToSequenceNumber["Возврат"] <= currentStatusNumber)
				returnedDate = receivedDate?.AddDays(rand.Next(0, 13));

			var window = new StatusWIndow(order.Date, paidDate, delieveryDate, receivedDate, receivedDate2Week, returnedDate);
			window.Show();
        }
    }
}
