using StockScreener.Interfaces;
using System.Diagnostics;

namespace StockScreener.Model
{
    /// <summary>
    /// Immplementaion of a stock
    /// </summary>
    public class Stock : Notifyable, IStock
    {
        //TEMPORARY FOR TESTING
        public static float price = 0;
        public static float volume = 0.01f;

        /// <summary>
        /// Constructor to create the stock from a csv split line
        /// THROWS EXCEPTION ON BAD PARSE
        /// </summary>
        /// <param name="response"></param>
        public Stock(string ticker, string name)
        {
            //create an instance of a stock from the items
            //Directly assume array items, ok to throw exception on failure
            Ticker = ticker;
            Name = name;
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

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }

            private set
            {
                _name = value;
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

        private float _percentChange;
        public float PercentChange
        {
            get { return _percentChange; }
            set
            {
                if (PercentChange == value) return;
                _percentChange = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        /// <summary>
        /// Take an input stock and update our values
        /// </summary>
        /// <param name="stock"></param>
        public void UpdateFromQuote(IQuote quote)
        {
            //If our stock ticker is not equal to input just return out
            if (Ticker != quote.Symbol)
            {
                //This is bad so assert in debug mode to let developer know.  In release mose its ok just bail out.
                var logMessage = "Passed in stock ticker does not match current stock in UpdateFromStock.  Current ticker = " + Ticker + " passed in ticker = " + quote.Symbol;
                Debug.Assert(false, logMessage);
                //In release mode just add a trace message so when listening to traces we will see it for debugging
                Trace.WriteLine(logMessage);
                return;
            }
            //note: If equal the setter will throw away
            CurrentPrice = quote.LatestPrice;
            PercentChange = quote.ChangePercent * 100;
            //store in millions
            CurrentVolume = quote.LatestVolume/1000000;
            //store in millions
            MarketCap = quote.MarketCap/1000000;
            LastClosePrice = quote.PreviousClose;
        }
    }
}
