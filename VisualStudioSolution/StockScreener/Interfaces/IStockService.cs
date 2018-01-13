
namespace StockScreener.Interfaces
{
    public interface IStockService
    {
        IStock GetStock(string ticker);

        /// <summary>
        /// Refresh the values of a stock
        /// </summary>
        /// <param name="stock"></param>
        void UpdateStock(IStock stock);

    }
}
