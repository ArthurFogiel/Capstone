using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using StockScreener.Interfaces;

namespace StockScreener.ViewModel
{
    /// <summary>
    /// This class contains properties that the Screener View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net for base class
    /// </para>
    /// </summary>
    public class ScreenerViewModel : ViewModelBase, IStockScreenerViewModel
    {
        private readonly IUserInfoService _userService;
        private readonly IStockService _stockService;

        [GalaSoft.MvvmLight.Ioc.PreferredConstructor]
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public ScreenerViewModel(IUserInfoService userService, IStockService stockService)
        {
            _userService = userService;
            _stockService = stockService;
        }

        /// <summary>
        /// This constructor is Just for designer for showing the UI elements
        /// </summary>
        public ScreenerViewModel()
        {
        }

        public IUserInfoService UserInfoService
        {
            get { return _userService; }
        }

        /// <summary>
        /// The filtered list of stocks using the logged in users settings and the full list of stocks from the model
        /// </summary>
        public ObservableCollection<IStock> FilteredStocks
        {
            get
            {
                //TODO
                return null;
            }
        }
        /// <summary>
        /// The list of stocks from the users watch list
        /// </summary>
        public ObservableCollection<IStock> WatchedStocks
        {
            get
            {
                //TODO
                return null;
            }
        }

        //todo
        public ICommand SaveFavorite => throw new System.NotImplementedException();

        public ICommand LoadFavorite => throw new System.NotImplementedException();

        public ICommand Apply => throw new System.NotImplementedException();

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}