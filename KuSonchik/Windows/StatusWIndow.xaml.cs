using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Логика взаимодействия для StatusWIndow.xaml
    /// </summary>
    public partial class StatusWIndow : Window
    {
        public StatusWIndow(DateTime? addInBasket, DateTime? paid, DateTime? delievery, DateTime? received, DateTime? received2week, DateTime? returned)
        {
            InitializeComponent();
            AddInBasket.Text = addInBasket.ToString();
            Paid.Text = paid?.ToString();
            Delievery.Text = delievery?.ToString();
            Received.Text = received?.ToString();
            if (returned != null) Returned.Text = returned.ToString();
            else Received2Week.Text = received2week.ToString();
        }
    }
}
