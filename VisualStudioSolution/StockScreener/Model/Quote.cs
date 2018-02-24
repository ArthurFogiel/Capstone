
using Newtonsoft.Json;
using StockScreener.Interfaces;

namespace StockScreener.Model
{
    /// <summary>
    /// Skeleton Class for JSON response of a quote from API IEX
    /// https://iextrading.com/developer/docs/#quote
    /// </summary>
    public class Quote: IQuote
    {
        public Quote()
        {

        }

        public string Symbol { get; set; }

        public string CompanyName { get; set; }

        public string PrimaryExchange { get; set; }

        public string Sector { get; set; }

        public string CalculationPrice { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float Open { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float OpenTime { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float Close { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float CloseTime { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float High { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float Low { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float LatestPrice { get; set; }


        public string LatestSource { get; set; }

        public string LatestTime { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float LatestUpdate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float LatestVolume { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float IEXRealTimePrice { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float IEXRealTimeSize { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float IEXLastUpdated { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float DelayedPrice { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float DelayedPriceTime { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float PreviousClose { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float Change { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float ChangePercent { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float IEXMarketPercent { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float IEXVolume { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float AvgTotalVolume { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float IEXBidPrice { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float IEXBidSize { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float IEXAskPrice { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float IEXAskSize { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float MarketCap { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float PERatio { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float Week52High { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float Week52Low { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float YtdChange { get; set; }
    }
}
