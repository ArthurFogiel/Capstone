
namespace StockScreener.Interfaces
{
    public interface IQuote
    {

        string Symbol { get; set; }

        string CompanyName { get; set; }

        string PrimaryExchange { get; set; }

        string Sector { get; set; }

        string CalculationPrice { get; set; }

        float Open { get; set; }

        float OpenTime { get; set; }

        float Close { get; set; }

        float CloseTime { get; set; }

        float High { get; set; }

        float Low { get; set; }

        float LatestPrice { get; set; }

        string LatestSource { get; set; }

        string LatestTime { get; set; }

        float LatestUpdate { get; set; }

        float LatestVolume { get; set; }

        float IEXRealTimePrice { get; set; }

        float IEXRealTimeSize { get; set; }

        float IEXLastUpdated { get; set; }

        float DelayedPrice { get; set; }

        float DelayedPriceTime { get; set; }

        float PreviousClose { get; set; }

        float Change { get; set; }

        float ChangePercent { get; set; }

        float IEXMarketPercent { get; set; }

        float IEXVolume { get; set; }

        float AvgTotalVolume { get; set; }

        float IEXBidPrice { get; set; }

        float IEXBidSize { get; set; }

        float IEXAskPrice { get; set; }

        float IEXAskSize { get; set; }

        float MarketCap { get; set; }

        float PERatio { get; set; }

        float Week52High { get; set; }

        float Week52Low { get; set; }

        float YtdChange { get; set; }
    }
}
