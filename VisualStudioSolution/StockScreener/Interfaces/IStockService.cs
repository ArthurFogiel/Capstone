
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace StockScreener.Interfaces
{
    public interface IStockService: INotifyPropertyChanged
    {
        /// <summary>
        /// List of all stocks that are constantly updated
        /// </summary>
        ObservableCollection<IStock> Stocks {get;}

        /// <summary>
        /// Are we successfully online?
        /// </summary>
        bool IsOnline { get; }
        /// <summary>
        /// Is it done initializing
        /// </summary>
        bool IsInitialized { get; }
        /// <summary>
        /// Did we initialize successfully?
        /// </summary>
        bool InitializedSuccessfully { get;
        }
    }
}
