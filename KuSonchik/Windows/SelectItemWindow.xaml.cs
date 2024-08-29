using KuSonchik.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
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

namespace KuSonchik.Windows
{
    /// <summary>
    /// Логика взаимодействия для SelectItemWindow.xaml
    /// </summary>
    public partial class SelectItemWindow : Window
    {
        public SelectItemWindow(IList items, PropertyInfo propertyInfo)
        {
            InitializeComponent();
            Table.ItemsSource = items;
            PropertyInfo = propertyInfo;
        }

        private PropertyInfo PropertyInfo { get; }

        public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register(nameof(SelectedValue), typeof(object), typeof(SelectItemWindow));
        public object SelectedValue
        {
            get => GetValue(SelectedValueProperty);
            set => SetValue(SelectedValueProperty, value);
        }


        private static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(SelectItemWindow),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSelectedItemPropertyChanged)));
        private object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }
        private static void OnSelectedItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = (SelectItemWindow)d;
            var propertyInfo = window.PropertyInfo;
            window.SelectedValue = propertyInfo.GetValue(e.NewValue);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
