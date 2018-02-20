
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
    }

    /// <summary>
    ///     This is so the datagrid can access properties inside the view model that are outside of the inherited datagrid
    ///     source.
    /// </summary>
    public class BindingProxyStocksView : Freezable
    {
        #region Overrides of Freezable

        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxyStocksView();
        }

        #endregion

        // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(BindingProxyStocksView),
                new UIPropertyMetadata(null));

        public object Data
        {
            get { return GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }
    }
    }
