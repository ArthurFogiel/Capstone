
namespace StockScreener.Interfaces
{
    /// <summary>
    /// Enum for selecting a units for the market cap
    /// </summary>
    public enum MarketCapUnitsEnum
    {
        Millions,
        Billions
    }
    public interface ISettings
    {
        /// <summary>
        /// Price min in dollars to filter by
        /// </summary>
        float PriceMin { get; set; }
        /// <summary>
        /// Price Max in dollars for filtering
        /// </summary>
        float PriceMax { get; set; }
        /// <summary>
        /// Market Cap Min
        /// </summary>
        float MarketCapMin { get; set; }
        /// <summary>
        /// Market Cap Max
        /// </summary>
        float MarketCapMax { get; set; }
        /// <summary>
        /// Units for market cap to filter by
        /// </summary>
        MarketCapUnitsEnum MarketCapUnits { get;set;}
        /// <summary>
        /// Min Volume shares for the current day in millions
        /// </summary>
        float VolumeMin { get; set; }
        /// <summary>
        /// Maximum Volume for today to filter in Millions
        /// </summary>
        float VolumeMax { get; set; }
    }
}
