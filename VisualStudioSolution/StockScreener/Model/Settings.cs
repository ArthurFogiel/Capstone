
using StockScreener.Interfaces;

namespace StockScreener.Model
{
    class Settings : Notifyable, ISettings
    {
        //Default value is 5
        private float _priceMin = 5;
        public float PriceMin
        {
            get
            {
                return _priceMin;
            }
            set
            {
                _priceMin = value;
                RaisePropertyChanged();
            }
        }
        //Default to 10
        private float _priceMax = 10;
        public float PriceMax
        {
            get
            {
                return _priceMax;
            }
            set
            {
                _priceMax = value;
                RaisePropertyChanged();
            }
        }


        public float _marketCapMin = 500;
        public float MarketCapMin
        {
            get
            {
                return _marketCapMin;
            }
            set
            {
                _marketCapMin = value;
                RaisePropertyChanged();
            }
        }

        private float _marketCapMax = 1000;
        public float MarketCapMax
        {
            get
            {
                return _marketCapMax;
            }
            set
            {
                _marketCapMax = value;
                RaisePropertyChanged();
            }
        }

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

        private float _volumeMin = 500;
        public float VolumeMin
        {
            get
            {
                return _volumeMin;
            }
            set
            {
                _volumeMin = value;
                RaisePropertyChanged();
            }
        }

        private float _volumeMax = 1000; 
        public float VolumeMax
        {
            get
            {
                return _volumeMax;
            }
            set
            {
                _volumeMax = value;
                RaisePropertyChanged();
            }
        }
    }
}
