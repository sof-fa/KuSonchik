using KuSonchik.Models;
using KuSonchik.Utilities;
using KuSonchik.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using static KuSonchik.Windows.AddEditWindow;

namespace KuSonchik
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        public static readonly DependencyProperty TableItemsProperty = DependencyProperty.Register(nameof(TableItems), typeof(IList), typeof(MainWindow));
        public IList TableItems
        {
            get => (IList)GetValue(TableItemsProperty);
            set => SetValue(TableItemsProperty, value);
        }


        public enum TabTypes { Orders, Delieveries, Products, Clients, Suppliers }
        private TabTypes CurrentTabType { get; set; }
        private Type CurrentType { get; set; }
        private object SelectedItem { get; set; }

        private Dictionary<string, TabTypes> TypeTabByName { get; } = new()
            { { "Продажи", TabTypes.Orders }, { "Доставка", TabTypes.Delieveries }, { "Товары", TabTypes.Products }, { "Клиенты", TabTypes.Clients }, { "Поставщики", TabTypes.Suppliers } };

        private async void LoadOrders()
        {
            var table = new List<Order>();
            await App.DBAPI.LoadTable<Order>(table);
            TableItems = table;
        }
        private async void LoadDelieveries()
        {
            var table = new List<Delievery>();
            await App.DBAPI.LoadTable<Delievery>(table);
            TableItems = table;
        }
        private async void LoadProducts()
        {
            var table = new List<Product>();
            await App.DBAPI.LoadTable<Product>(table);
            TableItems = table;
        }
        private async void LoadClients()
        {
            var table = new List<Client>();
            await App.DBAPI.LoadTable<Client>(table);
            TableItems = table;
        }
        private async void LoadEmployees()
        {
            var table = new List<Supplier>();
            await App.DBAPI.LoadTable<Supplier>(table);
            TableItems = table;
        }

        private void UpdateInfo(TabTypes type)
        {
            if (type == TabTypes.Orders) LoadOrders();
            else if (type == TabTypes.Delieveries) LoadDelieveries();
            else if (type == TabTypes.Products) LoadProducts();
            else if (type == TabTypes.Clients) LoadClients();
            else if (type == TabTypes.Suppliers) LoadEmployees();
        }

        private void MainTabs_Clicked(object sender, RoutedEventArgs e)
        {
            var rb = (RadioButton)sender;
            CurrentTabType = TypeTabByName[(string)rb.Content];
            if (CurrentTabType == TabTypes.Orders)
                CurrentType = typeof(Order);
            else if (CurrentTabType == TabTypes.Delieveries)
                CurrentType = typeof(Delievery);
            else if (CurrentTabType == TabTypes.Products)
                CurrentType = typeof(Product);
            else if (CurrentTabType == TabTypes.Clients)
                CurrentType = typeof(Client);
            else if (CurrentTabType == TabTypes.Suppliers)
                CurrentType = typeof(Supplier);
            UpdateInfo(CurrentTabType);
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddEditWindow(TypeAction.Add, CurrentType);
            if (window.ShowDialog() == true) UpdateInfo(CurrentTabType);
        }

        private void EditItemButton_Click(object sender, RoutedEventArgs e)
        {

            if (CurrentTabType == TabTypes.Orders)
            {
                SelectedItem = SmartTable.SelectedItem as Order;
            }
            else if (CurrentTabType == TabTypes.Delieveries)
            {
                SelectedItem = SmartTable.SelectedItem as Delievery;
            }
            else if (CurrentTabType == TabTypes.Products)
            {
                SelectedItem = SmartTable.SelectedItem as Product;
            }
            else if (CurrentTabType == TabTypes.Clients)
            {
                SelectedItem = SmartTable.SelectedItem as Client;
            }
            else if (CurrentTabType == TabTypes.Suppliers)
            {
                SelectedItem = SmartTable.SelectedItem as Supplier;
            }
            var window = new AddEditWindow(TypeAction.Edit, CurrentType, SelectedItem);
            if (window.ShowDialog() == true)
            {
                UpdateInfo(CurrentTabType);
            }
        }

        private void DeleteItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentTabType == TabTypes.Orders)
            {
                SelectedItem = SmartTable.SelectedItem as Order;
            }
            else if (CurrentTabType == TabTypes.Delieveries)
            {
                SelectedItem = SmartTable.SelectedItem as Delievery;
            }
            else if (CurrentTabType == TabTypes.Products)
            {
                SelectedItem = SmartTable.SelectedItem as Product;
            }
            else if (CurrentTabType == TabTypes.Clients)
            {
                SelectedItem = SmartTable.SelectedItem as Client;
            }
            else if (CurrentTabType == TabTypes.Suppliers)
            {
                SelectedItem = SmartTable.SelectedItem as Supplier;
            }
            ConfirmWindow window = new();
            if (window.ShowDialog() == true)
            {
                if (CurrentTabType == TabTypes.Orders)
                    App.DBAPI.DeleteOrder(SelectedItem as Order);
                else if (CurrentTabType == TabTypes.Delieveries)
                    App.DBAPI.DeleteDelievery(SelectedItem as Delievery);
                else if (CurrentTabType == TabTypes.Products)
                    App.DBAPI.DeleteProduct(SelectedItem as Product);
                else if (CurrentTabType == TabTypes.Clients)
                    App.DBAPI.DeleteClient(SelectedItem as Client);
                else if (CurrentTabType == TabTypes.Suppliers)
                    App.DBAPI.DeleteEmployee(SelectedItem as Supplier);

                UpdateInfo(CurrentTabType);
            }
        }

        private void MakeReportButton_Click(object sender, RoutedEventArgs e)
        {
            ReportWindow window = new();
            window.Show();
        }

        private void SalesFunnelButton_Click(object sender, RoutedEventArgs e)
        {
            using CRMContext db = new();
            int countVisitors = int.Parse(CountVisitors.Text);
            int countAdd = db.Orders.Count();
            int countPaid = db.Orders.Count(p => p.Status == "Оплачен");
            int countReceived = db.Orders.Count(p => p.Status == "Получен");
            int countReceived2Week = db.Orders.Count(p => p.Status == "Получен (2 недели)");
            int countReturned = db.Orders.Count(p => p.Status == "Возврат");
            var window = new SalesFunnelWindow(countVisitors, countAdd, countPaid, countReceived, countReceived2Week, countReturned);
            window.Show();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
