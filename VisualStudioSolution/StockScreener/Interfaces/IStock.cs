
namespace StockScreener.Interfaces
{
    /// <summary>
    /// Representation of a stock
    /// </summary>
    public interface IStock
    {
        /// <summary>
        /// Ticker symbol
        /// </summary>
        string Ticker { get; set; }
        /// <summary>
        /// Market Cap in millions
        /// </summary>
        float MarketCap { get; set; }
        /// <summary>
        /// Current price
        /// </summary>
        float CurrentPrice { get; set; }
        /// <summary>
        /// Last trading days close price
        /// </summary>
        float LastClosePrice { get; set; }
        /// <summary>
        /// Current volume of shares traded in Millions
        /// </summary>
        float CurrentVolume { get; set; }
        /// <summary>
        /// Is the stock marked to be watched?
        /// </summary>
        bool IsWatching { get; set; }
    }
}
