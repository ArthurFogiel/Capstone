
using System.Collections.Generic;
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
    }
}
