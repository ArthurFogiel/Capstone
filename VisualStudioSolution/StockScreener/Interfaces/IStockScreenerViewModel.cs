using System.Collections.ObjectModel;
using System.Windows.Input;

namespace StockScreener.Interfaces
{
    public interface IStockScreenerViewModel
    {
        IUserInfoService UserInfoService { get; }

        IStockService StockService { get; }

        /// <summary>
        /// Property for the view to see a collection of stocks that are generated from the filter
        /// An observable collection automatically sends events when items are added or removed
        /// </summary>
        ObservableCollection<IStock> FilteredStocks { get; }

        /// <summary>
        /// Property for the view to see a collection of stocks that are being watched
        /// An observable collection automatically sends events when items are added or removed
        /// </summary>
        ObservableCollection<IStock> WatchedStocks { get; }

        /// <summary>
        /// Button for saving the user settings to file
        /// </summary>
        ICommand SaveFavorite { get; }

        /// <summary>
        /// Button for loading a saved favorite file
        /// </summary>

        ICommand LoadFavorite { get; }

        /// <summary>
        /// Button for Applying settings
        /// </summary>

        ICommand Apply { get; }

        ICommand LogOut { get; }

        /// <summary>
        /// Button in the data grid to add to watch list
        /// </summary>
        ICommand WatchCommand { get; }

        /// <summary>
        /// Button in the data grid to Remove from watch list
        /// </summary>
        ICommand RemoveWatchCommand { get; }


}
}
