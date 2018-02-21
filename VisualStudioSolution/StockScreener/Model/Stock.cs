using StockScreener.Interfaces;
using System.Diagnostics;
using System.Xml;
using System.Xml.Schema;

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
        public Stock(string alphaReturn, ExchangeEnums exchange)
        {
            Exchange = exchange;
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
            LastClosePrice = stock.LastClosePrice;
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
            //"timestamp,open,high,low,close,volume\r\n2018-02-20 16:00:00,92.7950,92.8400,92.6801,92.7200,5070967\r\n2018-02-20 15:59:00,92.8650,92.8700,92.7100,92.8000,162181\r\n2018-02-20 15:58:00,92.8100,92.8800,92.8100,92.8600,126393\r\n2018-02-20 15:57:00,92.8850,92.8905,92.7800,92.8100,150894\r\n2018-02-20 15:56:00,92.8300,92.9100,92.8300,92.8800,125741\r\n2018-02-20 15:55:00,92.7900,92.8500,92.7900,92.8400,74013\r\n2018-02-20 15:54:00,92.7450,92.7900,92.7400,92.7800,63106\r\n2018-02-20 15:53:00,92.7700,92.8100,92.7100,92.7500,72263\r\n2018-02-20 15:52:00,92.8000,92.8300,92.7550,92.7800,82768\r\n2018-02-20 15:51:00,92.7750,92.8200,92.7450,92.8050,100614\r\n2018-02-20 15:50:00,92.6600,92.7800,92.6600,92.7750,95601\r\n2018-02-20 15:49:00,92.7100,92.7450,92.6250,92.6500,77399\r\n2018-02-20 15:48:00,92.6200,92.7300,92.5800,92.7100,123421\r\n2018-02-20 15:47:00,92.4600,92.6400,92.4600,92.6239,123377\r\n2018-02-20 15:46:00,92.3300,92.4600,92.2700,92.4600,141199\r\n2018-02-20 15:45:00,92.3900,92.4200,92.3300,92.3300,74391\r\n2018-02-20 15:44:00,92.4100,92.4400,92.3350,92.3900,46101\r\n2018-02-20 15:43:00,92.4000,92.4800,92.4000,92.4200,53988\r\n2018-02-20 15:42:00,92.4250,92.4800,92.3600,92.4100,72222\r\n2018-02-20 15:41:00,92.4200,92.4600,92.3650,92.4300,79487\r\n2018-02-20 15:40:00,92.4452,92.5085,92.4100,92.4100,114537\r\n2018-02-20 15:39:00,92.4200,92.5000,92.4200,92.4450,39314\r\n2018-02-20 15:38:00,92.4200,92.4700,92.4000,92.4200,39029\r\n2018-02-20 15:37:00,92.4200,92.4900,92.3900,92.4200,44279\r\n2018-02-20 15:36:00,92.4600,92.5000,92.3500,92.4100,61748\r\n2018-02-20 15:35:00,92.3800,92.4700,92.3700,92.4700,39467\r\n2018-02-20 15:34:00,92.3700,92.4460,92.3550,92.3850,39339\r\n2018-02-20 15:33:00,92.4300,92.4400,92.3700,92.3750,49136\r\n2018-02-20 15:32:00,92.4650,92.4750,92.3900,92.4300,54997\r\n2018-02-20 15:31:00,92.4100,92.5200,92.4000,92.4700,61369\r\n2018-02-20 15:30:00,92.3000,92.4600,92.3000,92.4100,51011\r\n2018-02-20 15:29:00,92.3350,92.4300,92.2900,92.3000,68734\r\n2018-02-20 15:28:00,92.2500,92.3650,92.2450,92.3350,40002\r\n2018-02-20 15:27:00,92.3200,92.3240,92.2600,92.2600,48464\r\n2018-02-20 15:26:00,92.3400,92.3800,92.3000,92.3250,66871\r\n2018-02-20 15:25:00,92.1200,92.3600,92.1150,92.3300,38378\r\n2018-02-20 15:24:00,92.1500,92.2700,92.1100,92.1200,47901\r\n2018-02-20 15:23:00,92.1500,92.2400,92.0800,92.1700,91762\r\n2018-02-20 15:22:00,92.2200,92.2300,92.0900,92.1500,97382\r\n2018-02-20 15:21:00,92.3500,92.3650,92.1950,92.2200,97107\r\n2018-02-20 15:20:00,92.2600,92.3600,92.2500,92.3550,40112\r\n2018-02-20 15:19:00,92.3450,92.4150,92.2400,92.2550,81734\r\n2018-02-20 15:18:00,92.3600,92.4200,92.3000,92.3500,45292\r\n2018-02-20 15:17:00,92.4200,92.4500,92.3400,92.3500,37189\r\n2018-02-20 15:16:00,92.2900,92.4400,92.2500,92.4110,52561\r\n2018-02-20 15:15:00,92.3948,92.4100,92.1900,92.2800,128143\r\n2018-02-20 15:14:00,92.4100,92.4300,92.3700,92.4000,34509\r\n2018-02-20 15:13:00,92.3750,92.4600,92.3700,92.4150,41375\r\n2018-02-20 15:12:00,92.3400,92.4000,92.3300,92.3700,41537\r\n2018-02-20 15:11:00,92.3000,92.3300,92.2500,92.3300,60080\r\n2018-02-20 15:10:00,92.3500,92.3700,92.2900,92.3000,38403\r\n2018-02-20 15:09:00,92.3800,92.4300,92.2500,92.3300,75878\r\n2018-02-20 15:08:00,92.5900,92.5900,92.3400,92.3700,115010\r\n2018-02-20 15:07:00,92.5600,92.6300,92.5600,92.6000,54001\r\n2018-02-20 15:06:00,92.5500,92.5860,92.5300,92.5500,31491\r\n2018-02-20 15:05:00,92.6238,92.6350,92.5400,92.5400,33364\r\n2018-02-20 15:04:00,92.5500,92.6250,92.5050,92.6215,40404\r\n2018-02-20 15:03:00,92.6700,92.6800,92.5500,92.5550,38028\r\n2018-02-20 15:02:00,92.6400,92.7400,92.6400,92.6600,42959\r\n2018-02-20 15:01:00,92.5300,92.6500,92.5200,92.6399,44658\r\n2018-02-20 15:00:00,92.5900,92.6000,92.5110,92.5200,29593\r\n2018-02-20 14:59:00,92.5650,92.6164,92.5350,92.5800,45261\r\n2018-02-20 14:58:00,92.6200,92.6500,92.5300,92.5633,41475\r\n2018-02-20 14:57:00,92.5700,92.6700,92.5700,92.6100,37197\r\n2018-02-20 14:56:00,92.6500,92.6600,92.5200,92.5600,79050\r\n2018-02-20 14:55:00,92.7100,92.7393,92.6000,92.6400,68984\r\n2018-02-20 14:54:00,92.8050,92.8050,92.7100,92.7100,68074\r\n2018-02-20 14:53:00,92.7700,92.8400,92.7700,92.8000,51935\r\n2018-02-20 14:52:00,92.8500,92.8650,92.7600,92.7700,46414\r\n2018-02-20 14:51:00,92.8600,92.9150,92.8400,92.8600,27240\r\n2018-02-20 14:50:00,92.8800,92.9100,92.8467,92.8600,21122\r\n2018-02-20 14:49:00,92.9000,92.9250,92.8500,92.8800,37585\r\n2018-02-20 14:48:00,92.9400,92.9500,92.8600,92.9000,33653\r\n2018-02-20 14:47:00,93.0300,93.0300,92.9450,92.9450,24308\r\n2018-02-20 14:46:00,92.9850,93.0500,92.9800,93.0206,35892\r\n2018-02-20 14:45:00,92.9656,92.9900,92.9600,92.9850,19261\r\n2018-02-20 14:44:00,93.0000,93.0000,92.9300,92.9600,28600\r\n2018-02-20 14:43:00,92.9800,93.0200,92.9500,93.0000,28364\r\n2018-02-20 14:42:00,93.0160,93.0250,92.9700,92.9700,33673\r\n2018-02-20 14:41:00,93.0500,93.0600,93.0100,93.0150,36256\r\n2018-02-20 14:40:00,93.0050,93.0500,93.0050,93.0500,27178\r\n2018-02-20 14:39:00,93.0450,93.0452,92.9850,93.0000,24327\r\n2018-02-20 14:38:00,93.0100,93.0600,93.0000,93.0400,34724\r\n2018-02-20 14:37:00,92.9750,93.0200,92.9600,93.0000,18400\r\n2018-02-20 14:36:00,92.9850,93.0200,92.9700,92.9799,43722\r\n2018-02-20 14:35:00,92.9748,92.9900,92.9100,92.9800,31897\r\n2018-02-20 14:34:00,92.9850,92.9850,92.9500,92.9707,27647\r\n2018-02-20 14:33:00,92.9900,93.0200,92.9700,92.9800,29878\r\n2018-02-20 14:32:00,92.9400,93.0200,92.9300,92.9900,50975\r\n2018-02-20 14:31:00,92.8400,92.9400,92.8200,92.9400,27065\r\n2018-02-20 14:30:00,92.9200,92.9200,92.8200,92.8400,45118\r\n2018-02-20 14:29:00,92.9500,92.9755,92.9300,92.9300,26385\r\n2018-02-20 14:28:00,93.0000,93.0100,92.9200,92.9450,60597\r\n2018-02-20 14:27:00,92.9700,93.0000,92.9700,92.9950,20676\r\n2018-02-20 14:26:00,92.9000,92.9700,92.8900,92.9700,21898\r\n2018-02-20 14:25:00,92.9100,92.9200,92.8750,92.8900,23266\r\n2018-02-20 14:24:00,92.8600,92.9400,92.8500,92.9200,49774\r\n2018-02-20 14:23:00,92.8800,92.9000,92.8500,92.8600,29598\r\n2018-02-20 14:22:00,92.8400,92.9000,92.8300,92.8700,39998\r\n2018-02-20 14:21:00,92.8300,92.8467,92.7950,92.8300,20300\r\n"

            //look at PopulateStockListFromParsedMasterList in the stockservice for some examples
            //first line is header: timestamp,open,high,low,close,volume


        }
    }
}
