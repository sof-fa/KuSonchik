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
    /// Логика взаимодействия для SalesFunnelWindow.xaml
    /// </summary>
    public partial class SalesFunnelWindow : Window
    {
        public SalesFunnelWindow(int countVisitors, int countAdd, int countPaid, int countReceived, int countReceived2Week, int countReturned)
        {
            InitializeComponent();
            CountVisitorsInfo.Text = countVisitors.ToString();
            CountAddInBasketInfo.Text = countAdd.ToString();
            CountPaidInfo.Text = countPaid.ToString();
            CountReceivedInfo.Text = countReceived.ToString();
            CountReceived2Week.Text = countReceived2Week.ToString();

            CountReturnedInfo.Text = countReturned.ToString();
            PercentReturnedInfo.Text = ((double)countReturned / countAdd).ToString("P2");
            PercentAddInBasketInfo.Text = ((double)countAdd / countVisitors).ToString("P2");
            PercentPaidInfo.Text = ((double)countPaid / countVisitors).ToString("P2");
            PercentReceivedInfo.Text = ((double)countReceived2Week / countVisitors).ToString("P2");
        }
    }
}
