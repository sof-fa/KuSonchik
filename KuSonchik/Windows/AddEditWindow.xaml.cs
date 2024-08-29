using KuSonchik.Models;
using KuSonchik.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static KuSonchik.Windows.AddEditWindow;

namespace KuSonchik.Windows
{
	/// <summary>
	/// Логика взаимодействия для DeleteEditWindow.xaml
	/// </summary>
	public partial class AddEditWindow : Window
	{
		public AddEditWindow(TypeAction typeAction, Type typeObject, object item = null)
		{
			InitializeComponent();
			CurrentAction = typeAction;
			Item = item;
			CurrentType = typeObject;
			Init();
		}


		public Type CurrentType { get; }
		public enum TypeAction { Add, Edit }
		private TypeAction CurrentAction { get; }
		private object Item { get; set; }
		private Button SelectedButton { get; set; }
		private Dictionary<Type, string> ContentDict { get; } = new() 
			{ { typeof(Order), "заказа" }, { typeof(Delievery), "доставки" }, { typeof(Product), "товара" }, { typeof(Client), "клиента" }, { typeof(Supplier), "сотрудника" } };
		private Dictionary<PropertyInfo, UIElement> PropertyElements { get; } = new();


		public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(nameof(Items), typeof(IList), typeof(AddEditWindow));
		public IList Items
		{
			get => (IList)GetValue(ItemsProperty);
			set => SetValue(ItemsProperty, value);
		}


		private static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(AddEditWindow),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSelectedItemPropertyChanged)));
		private object SelectedItem
		{
			get => GetValue(SelectedItemProperty);
			set => SetValue(SelectedItemProperty, value);
		}
		private static void OnSelectedItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var window = (AddEditWindow)d;
			window.SelectedButton.Content = e.NewValue.GetType().GetProperty("Id").GetValue(e.NewValue) as string;
		}


		private static readonly DependencyProperty VisibilityForeignTableProperty = DependencyProperty.Register(nameof(VisibilityForeignTable), typeof(Visibility), typeof(AddEditWindow),
			new PropertyMetadata(Visibility.Collapsed));
		private Visibility VisibilityForeignTable
		{
			get => (Visibility)GetValue(VisibilityForeignTableProperty);
			set => SetValue(VisibilityForeignTableProperty, value);
		}


		private static readonly DependencyProperty CountItemProperty = DependencyProperty.Register(nameof(CountItem), typeof(int), typeof(AddEditWindow),
			new PropertyMetadata(1));
		private int CountItem
		{
			get => (int)GetValue(CountItemProperty);
			set => SetValue(CountItemProperty, value);
		}


		private void Init()
		{
			if (CurrentAction == TypeAction.Add)
			{
				NameAction.Content = "Добавление " + ContentDict[CurrentType];
				DeleteEditButton.Content = "Добавить";
			}
			else
			{
                NameAction.Content = "Редактирование " + ContentDict[CurrentType];
				DeleteEditButton.Content = "Сохранить";
			}

			var allProps = CurrentType.GetProperties();
			Style styleTextBlock = Resources["DialogWindow_TextBlock_Style"] as Style;
			Style styleTextBox = Resources["DialogWindow_TextBox_Style"] as Style;
			Style styleBorder = Resources["DialogWindow_Border_TextBox_Style"] as Style;
			int row = 0;
			for (int i = 0; i < allProps.Length; ++i)
			{
				if (!IsAcceptedProperty(allProps[i])) continue;
				MainGrid.RowDefinitions.Add(new RowDefinition());
				var textBlock = new TextBlock
				{
					Text = allProps[i].GetCustomAttribute<ColumnPropertiesAttribute>().Name,
					Style = styleTextBlock
				};
				Grid.SetRow(textBlock, row);
				Grid.SetColumn(textBlock, 0);
				MainGrid.Children.Add(textBlock);

				var border = new Border { Style = styleBorder };
				var isForeignKeyAttr = allProps[i].GetCustomAttribute<IsForeignKeyAttribute>();
				if (isForeignKeyAttr != null && isForeignKeyAttr.IsForeignKey)
				{
                    var button = new Button() { Name = allProps[i].Name, Content = "Выбрать" };
                    button.Click += Button_Click;
					border.Child = button;
					PropertyElements[allProps[i]] = button;
                }
				else
				{
					var textBox = new TextBox { Name = allProps[i].Name, Style = styleTextBox };
					border.Child = textBox;
					PropertyElements[allProps[i]] = textBox;
				}

                Grid.SetRow(border, row);
				Grid.SetColumn(border, 1);
				MainGrid.Children.Add(border);
				++row;
			}
			CountItem = row;
		}

		private async void Button_Click(object sender, RoutedEventArgs e)
		{
			var button = (Button)sender;
			SelectedButton = button;
			var propertyInfoInternal = CurrentType.GetProperty(button.Name);
			var foreignKeyAttr = propertyInfoInternal.GetCustomAttribute<IsForeignKeyAttribute>();
			SelectItemWindow window = null;
			IList list = null;
			if (foreignKeyAttr.TypeForeignKey == typeof(Order))
			{
				list = new List<Order>();
                await App.DBAPI.LoadTable<Order>(list);
            }
            else if (foreignKeyAttr.TypeForeignKey == typeof(Delievery))
			{
                list = new List<Delievery>();
                await App.DBAPI.LoadTable<Delievery>(list);
            }
            else if (foreignKeyAttr.TypeForeignKey == typeof(Product))
            {
                list = new List<Product>();
                await App.DBAPI.LoadTable<Product>(list);
            }
            else if (foreignKeyAttr.TypeForeignKey == typeof(Client))
            {
                list = new List<Client>();
                await App.DBAPI.LoadTable<Client>(list);
            }
            else if (foreignKeyAttr.TypeForeignKey == typeof(Supplier))
            {
                list = new List<Supplier>();
                await App.DBAPI.LoadTable<Supplier>(list);
            }
			if (list == null) return;
            window = new(list, foreignKeyAttr.PropertyInfo);
            if (window.ShowDialog() == true)
			{
				SelectedButton.Content = window.SelectedValue;
			}
        }

		private async void TextBox_ForeignKey_MouseDown(object sender, MouseButtonEventArgs e)
		{
			var textBox = sender as TextBox;
			if (textBox is null) return;
			var property = CurrentType.GetProperty(textBox.Name);
			if (property is null) return;
			var typeForeignKey = property.GetCustomAttribute<IsForeignKeyAttribute>().TypeForeignKey;
			if (typeForeignKey is null) return;
			IList list = null;
			if (typeForeignKey == typeof(Order))
			{
				list = new List<Order>();
				await App.DBAPI.LoadTable<Order>(list);
			}
			else if (typeForeignKey == typeof(Product))
			{
                list = new List<Product>();
                await App.DBAPI.LoadTable<Product>(list);
            }
			else if (typeForeignKey == typeof(Client))
			{
                list = new List<Client>();
                await App.DBAPI.LoadTable<Client>(list);
            }
			else if (typeForeignKey == typeof(Supplier))
			{
                list = new List<Supplier>();
                await App.DBAPI.LoadTable<Supplier>(list);
            }
			if (list != null) Items = list;
        }


        private void MakeItem()
		{
			Item = Activator.CreateInstance(CurrentType);

			var allProps = CurrentType.GetProperties();
			for (int i = 0; i < allProps.Length; ++i)
			{
				if (!IsAcceptedProperty(allProps[i])) continue;
				var elem = PropertyElements[allProps[i]];
                string str = string.Empty;
				if (PropertyElements[allProps[i]].GetType() == typeof(Button))
					str = (PropertyElements[allProps[i]] as Button).Content as string;
				else if (PropertyElements[allProps[i]].GetType() == typeof(TextBox))
					str = (PropertyElements[allProps[i]] as TextBox).Text;

                if (allProps[i].PropertyType == typeof(int)
					|| allProps[i].PropertyType == typeof(int?))
				{
					allProps[i].SetValue(Item, Convert.ToInt32(str));
				}
				else if (allProps[i].PropertyType == typeof(string))
				{
					allProps[i].SetValue(Item, str);
				}
				else if (allProps[i].PropertyType == typeof(DateTime))
				{
					allProps[i].SetValue(Item, DateTime.Parse(str));
				}
				else if (allProps[i].PropertyType == typeof(DateOnly))
				{
					allProps[i].SetValue(Item, DateOnly.Parse(str));
				}
				else if (allProps[i].PropertyType == typeof(decimal))
				{
					allProps[i].SetValue(Item, Convert.ToDecimal(str));
				}
			}
		}

		private bool IsAcceptedProperty(PropertyInfo property)
		{
			bool flag = true;
			if (property.GetCustomAttribute<IsNavigationAttribute>() != null) flag = false;
			var visibilityColumnAttribute = property.GetCustomAttribute<VisibilityColumnAttribute>();
			if (visibilityColumnAttribute != null
				&& visibilityColumnAttribute.ID_Visibility != Visibility.Visible) flag = false;
			return flag;
		}        
		

		private void ButtonSave_Click(object sender, RoutedEventArgs e)
		{
			if (CurrentType == typeof(Order))
			{
				if (CurrentAction == TypeAction.Edit) App.DBAPI.EditOrder(Item as Order);
				else
				{
					MakeItem();
					App.DBAPI.AddOrder(Item as Order);
				}
			}
			else if (CurrentType == typeof(Delievery))
			{
				if (CurrentAction == TypeAction.Edit) App.DBAPI.EditDelievery(Item as Delievery);
				else
				{
					MakeItem();
					App.DBAPI.AddDelievery(Item as Delievery);
				}
			}
			else if (CurrentType == typeof(Product))
			{
				if (CurrentAction == TypeAction.Edit) App.DBAPI.EditProduct(Item as Product);
				else
				{
					MakeItem();
					App.DBAPI.AddProduct(Item as Product);
				}
			}
			else if (CurrentType == typeof(Client))
			{
				if (CurrentAction == TypeAction.Edit) App.DBAPI.EditClient(Item as Client);
				else
				{
					MakeItem();
					App.DBAPI.AddClient(Item as Client);
				}
			}
			else if (CurrentType == typeof(Supplier))
			{
				if (CurrentAction == TypeAction.Edit) App.DBAPI.EditEmployee(Item as Supplier);
				else
				{
					MakeItem();
					App.DBAPI.AddEmployee(Item as Supplier);
				}
			}
			Close();
		}

		private void ButtonCancell_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
