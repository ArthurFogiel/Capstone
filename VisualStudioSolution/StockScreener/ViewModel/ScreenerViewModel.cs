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

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public ScreenerViewModel(IUserInfoService userService, IStockService stockService)
        {
            _userService = userService;
            _stockService = stockService;
        }

        public IUserInfoService UserInfoService
        {
            get { return _userService; }
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}