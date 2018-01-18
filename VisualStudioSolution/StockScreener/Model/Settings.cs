
using StockScreener.Interfaces;

namespace StockScreener.Model
{
    class Settings : Notifyable, ISettings
    {
        //todo
        public float PriceMin { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public float PriceMax { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public float MarketCapMin { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public float MarketCapMax { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        private MarketCapUnitsEnum _marketCapUnitsEnum = MarketCapUnitsEnum.Millions;
        public MarketCapUnitsEnum MarketCapUnits
        {
            get { return _marketCapUnitsEnum; }
            set
            {
                _marketCapUnitsEnum = value;
                RaisePropertyChanged();
            }
        }
        public float VolumeMin { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public float VolumeMax { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }
}
