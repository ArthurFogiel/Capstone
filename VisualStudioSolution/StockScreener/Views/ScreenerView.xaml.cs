
using System.Windows;
using System.Windows.Controls;

namespace StockScreener.Views
{
    /// <summary>
    /// Interaction logic for ScreenerView.xaml
    /// </summary>
    public partial class ScreenerView : UserControl
    {
        public ScreenerView()
        {
            InitializeComponent();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ((Interfaces.ILoginViewModel)DataContext).UserInfoService.LogOffUser();
        }
    }
}
