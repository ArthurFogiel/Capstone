using StockScreener.Interfaces;

namespace StockScreener.Model
{
    /// <summary>
    /// Immplementaion of a stock
    /// </summary>
    public class Stock : IStock
    {
        //TODO
        public string ticker { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public float MarketCap { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public float CurrentPrice { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public float LastClosePrice { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public float CurrentVolume { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }
}
