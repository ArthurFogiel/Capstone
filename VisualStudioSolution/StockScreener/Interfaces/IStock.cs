
using System.Xml.Serialization;

namespace StockScreener.Interfaces
{
    /// <summary>
    /// Representation of a stock
    /// </summary>
    public interface IStock: IXmlSerializable
    {
        /// <summary>
        /// Ticker symbol
        /// </summary>
        string Ticker { get;}
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
        /// Passin in a stock, update this stocks values from the input
        /// </summary>
        /// <param name="stock"></param>
        void UpdateFromStock(IStock stock);
    }
}
