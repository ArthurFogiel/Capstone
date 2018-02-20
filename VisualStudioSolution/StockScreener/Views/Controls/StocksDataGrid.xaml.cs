

using System.Windows;
using System.Windows.Controls;

namespace StockScreener.Views.Controls
{
    /// <summary>
    /// Interaction logic for ScreenerParmaters.xaml
    /// </summary>
    public partial class StocksDataGrid : UserControl
    {
        public StocksDataGrid()
        {
            InitializeComponent();
        }
    }

    /// <summary>
    ///     This is so the datagrid can access properties inside the view model that are outside of the inherited datagrid
    ///     source.
    /// </summary>
    public class BindingProxyStocks : Freezable
    {
        #region Overrides of Freezable

        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxyStocks();
        }

        #endregion

        // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(BindingProxyStocks),
                new UIPropertyMetadata(null));

        public object Data
        {
            get { return GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }
    }
}
