using KuSonchik.Models;
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
    /// Логика взаимодействия для ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        public ReportWindow()
        {
            InitializeComponent();
            using (CRMContext db = new())
            {
                TopClientsBySum = db.Orders.GroupBy(p => p.IdClient).Select(g => new
                {
                    IdClient = g.Key,
                    Sum = g.Sum(p => p.Cost)
                }).OrderByDescending(p => p.Sum).Take(5).ToList();

                TopProductsByCount = db.Orders.GroupBy(p => p.IdProduct).Select(g => new
                {
                    IdProduct = g.Key,
                    Count = g.Count()
                }).OrderByDescending(p => p.Count).Take(5).ToList();

                TopCategoryByCount = db.Orders.Join(db.Products, o => o.IdProduct, p => p.Id, (o, p) => new
                {
                    IdProduct = p.Id,
                    Category = p.Category
                }).GroupBy(p => p.Category).Select(g => new
                {
                    Category = g.Key,
                    Count = g.Count()
                }).OrderByDescending(p => p.Count).First().Category;

                AllSum = db.Orders.Sum(o => o.Cost);

                CountClientsStatus = db.Clients.GroupBy(p => p.Status).Select(g => new
                {
                    Status = g.Key as string,
                    Count = g.Count()
                }).OrderBy(p => p.Status.Length).ToList();
            }
        }


        public static readonly DependencyProperty CountClientsStatusProperty = DependencyProperty.Register(nameof(CountClientsStatus), typeof(IList), typeof(ReportWindow));
        public IList CountClientsStatus
        {
            get => (IList)GetValue(CountClientsStatusProperty);
            set => SetValue(CountClientsStatusProperty, value);
        }


        public static readonly DependencyProperty TopClientsBySumProperty = DependencyProperty.Register(nameof(TopClientsBySum), typeof(IList), typeof(ReportWindow));
        public IList TopClientsBySum
        {
            get => (IList)GetValue(TopClientsBySumProperty);
            set => SetValue(TopClientsBySumProperty, value);
        }


        public static readonly DependencyProperty TopProductsByCountProperty = DependencyProperty.Register(nameof(TopProductsByCount), typeof(IList), typeof(ReportWindow));
        public IList TopProductsByCount
        {
            get => (IList)GetValue(TopProductsByCountProperty);
            set => SetValue(TopProductsByCountProperty, value);
        }


        public static readonly DependencyProperty TopCategoryByCountProperty = DependencyProperty.Register(nameof(TopCategoryByCount), typeof(string), typeof(ReportWindow));
        public string TopCategoryByCount
        {
            get => (string)GetValue(TopCategoryByCountProperty);
            set => SetValue(TopCategoryByCountProperty, value);
        }


        public static readonly DependencyProperty AllSumProperty = DependencyProperty.Register(nameof(AllSum), typeof(long), typeof(ReportWindow));
        public long AllSum
        {
            get => (long)GetValue(AllSumProperty);
            set => SetValue(AllSumProperty, value);
        }

    }
}
