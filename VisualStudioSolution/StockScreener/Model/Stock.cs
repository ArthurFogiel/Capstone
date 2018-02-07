using StockScreener.Interfaces;
using System.Diagnostics;

namespace StockScreener.Model
{
    /// <summary>
    /// Immplementaion of a stock
    /// </summary>
    public class Stock : Notifyable, IStock
    {
        /// <summary>
        /// Constructor to create the stock from the alpha vantage return string
        /// To be determined if json or csv
        /// </summary>
        /// <param name="response"></param>
        public Stock(string response)
        {
            //todo generate a stock from the json or csv response
        }

        /// <summary>
        /// Constructor to create from the comma seperated default file
        /// </summary>
        /// <param name="ticker"></param>
        /// <param name="marketCapFromFile"></param>
        public Stock(string ticker, string marketCapFromFile)
        {
            //TODO none of this is tested yet
            Ticker = ticker;
            //parse market cap.
            //unkown case
            if (marketCapFromFile == "n/a")
                return;
            //strip out $ sign 
            string marketCapStripped = marketCapFromFile.Replace("$", "");
            //get the units
            var isBillions = marketCapStripped[marketCapStripped.Length - 1] == 'B';

            //strip out units
            marketCapStripped = marketCapStripped.Substring(0,marketCapStripped.Length - 1);
            //parse the float value out
            float value;
            if (!float.TryParse(marketCapStripped, out value))
                return;
            //If units are millions multiply by 1000 to make it billions (MarketCap is stored in billions)
            if (!isBillions)
                value = value/1000;
            MarketCap = value;

        }

        #region IStock Properties
        private string _ticker;
        public string Ticker
        {
            get
            {
                return _ticker;
            }

            private set
            {
                _ticker = value;
                RaisePropertyChanged();
            }
        }

        private float _marketCap;
        /// <summary>
        /// Current market cap
        /// </summary>
        public float MarketCap
        {
            get
            {
                return _marketCap;
            }
            set
            {
                //do nothing if equal
                if (_marketCap == value) return;
                _marketCap = value;
                RaisePropertyChanged();
            }
        }

        private float _currentPrice;
        /// <summary>
        /// Current stock price
        /// </summary>
        public float CurrentPrice
        {
            get
            {
                return _currentPrice;
            }
            set
            {
                //do nothing if equal
                if (_currentPrice == value) return;
                _currentPrice = value;
                RaisePropertyChanged();
            }
        }

        private float _lastClosePrice;

        public float LastClosePrice
        {
            get
            {
                return _lastClosePrice;
            }
            set
            {
                //do nothing if equal
                if (_lastClosePrice == value) return;
                _lastClosePrice = value;
                RaisePropertyChanged();
            }
        }

        private float _currentVolume;
        public float CurrentVolume
        {
            get
            {
                return _currentVolume;
            }
            set
            {
                //do nothing if equal
                if (_currentVolume == value) return;
                _currentVolume = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        /// <summary>
        /// Take an input stock and update our values
        /// </summary>
        /// <param name="stock"></param>
        public void UpdateFromStock(IStock stock)
        {
            //If our stock ticker is not equal to input just return out
            if (Ticker != stock.Ticker)
            {
                //This is bad so assert in debug mode to let developer know.  In release mose its ok just bail out.
                var logMessage = "Passed in stock ticker does not match current stock in UpdateFromStock.  Current ticker = " + Ticker + " passed in ticker = " + stock.Ticker;
                Debug.Assert(false, logMessage);
                //In release mode just add a trace message so when listening to traces we will see it for debugging
                Trace.WriteLine(logMessage);
                return;
            }
            CurrentPrice = stock.CurrentPrice;
            LastClosePrice = stock.LastClosePrice;
            CurrentVolume = stock.CurrentVolume;
        }
    }
}
