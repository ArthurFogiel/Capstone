
using StockScreener.Interfaces;

namespace StockScreener.Model
{
    /// <summary>
    /// Implementation of a stock service
    /// Manages getting and updating stocks
    /// </summary>
    public class StockService : Notifyable, IStockService
    {
        //TODO
        public IStock GetStock(string ticker)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateStock(IStock stock)
        {
            throw new System.NotImplementedException();
        }
    }
}