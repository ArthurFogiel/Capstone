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
        public Stock(string[] splitItems, ExchangeEnums exchange)
        {
            Exchange = exchange;
            PopulateFromNasdaqDotCom(splitItems);
        }

        /// <summary>
        /// Pass in return of alphavantage
        /// </summary>
        /// <param name="alphaReturn"></param>
        /// <param name="exchange"></param>
        public Stock(string alphaReturn)
        {
            PopulateFromAlphaVantage(alphaReturn);
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

        private ExchangeEnums _exchange;
        public ExchangeEnums Exchange
        {
            get { return _exchange; }
            private set
            {
                _exchange = value;
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
            //Below are only values we get real time updates for
            CurrentPrice = stock.CurrentPrice;
            //possible we don't get the volume, in that case don't update
            if(stock.CurrentVolume != float.MinValue)
                CurrentVolume = stock.CurrentVolume;
        }


        /// <summary>
        /// Format is:
        /// Symbol,Name,LastSale,MarketCap,IPOyear,Sector,industry,Summary Quote,"
        /// Will THROW EXCEPTION if parse is bad
        /// </summary>
        /// <param name="csvLine"></param>
        private void PopulateFromNasdaqDotCom(string[] splitItems)
        {
            //create an instance of a stock from the items
            //Directly assume array items, ok to throw exception on failure
            Ticker = splitItems[0];
            LastClosePrice = float.Parse(splitItems[2]);
            CurrentPrice = LastClosePrice;
            //market cap
            //strip out $ sign 
            string marketCapStripped = splitItems[3].Replace("$", "");
            //get the units
            var isBillions = marketCapStripped[marketCapStripped.Length - 1] == 'B';

            //strip out units
            marketCapStripped = marketCapStripped.Substring(0, marketCapStripped.Length - 1);
            //parse the float value out
            float value;
            if (!float.TryParse(marketCapStripped, out value))
                return;
            //If units are millions multiply by 1000 to make it billions (MarketCap is stored in billions)
            if (!isBillions)
                value = value / 1000;
            MarketCap = value;

        }

        private void PopulateFromAlphaVantage(string alphaReturn)
        {
            //todo...parse the alphvantage

            //Here is a normal output by uncommenting  //GetUpdatedStock(Stocks.FirstOrDefault(x => x.Ticker == "MSFT")); in the stock service constructor
            //"timestamp,open,high,low,close,volume\r\n
        }
    }
}
