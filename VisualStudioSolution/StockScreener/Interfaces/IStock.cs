
using System.Xml.Serialization;

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
        string Ticker { get;}
        /// <summary>
        /// Name
        /// </summary>
        string Name { get; }
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
        /// Percent change
        /// </summary>
        float PercentChange { get; }
        /// <summary>
        /// Current volume of shares traded in Millions
        /// </summary>
        float CurrentVolume { get; set; }

        void UpdateFromQuote(IQuote quote);
    }
}
