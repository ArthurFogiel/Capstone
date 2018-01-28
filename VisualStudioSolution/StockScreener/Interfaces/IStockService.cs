
using System.Collections.Generic;
using System.ComponentModel;

namespace StockScreener.Interfaces
{
    public interface IStockService: INotifyPropertyChanged
    {
        /// <summary>
        /// List of all stocks that are constantly updated
        /// </summary>
        List<IStock> Stocks {get;}
    }
}
