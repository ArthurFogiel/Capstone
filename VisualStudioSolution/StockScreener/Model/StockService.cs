
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using StockScreener.Interfaces;

namespace StockScreener.Model
{
    /// <summary>
    /// Implementation of a stock service
    /// Manages getting and updating stocks
    /// </summary>
    public class StockService : Notifyable, IStockService
    {
        private string stockFilePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Screener\\Stocks.xml";
        [PreferredConstructorAttribute]
        public StockService()
        {
            //Testing: get microsoft stock
            //   GetStock("MSFT");
            //

            if (File.Exists(stockFilePath))
            {
                //TODO Read the file
            }
            //If not, use the text file to generate the list of stocks
            else
            {
                CreateListFromText();
            }

            //TESTING JUST KEEP TOGGLING THE PRICE TO SEE IT MOVE
            int count = 0;
            Task t = Task.Factory.StartNew(() => {
                while(true)
                {
                    count++;
                    Thread.Sleep(100);
                    foreach (var stock in Stocks)
                    {
                        stock.CurrentPrice = count%2 == 0? stock.CurrentPrice -1: stock.CurrentPrice+1;
                    }
                }
            });

            //start forever updating the list
            //example:
            //foreach(var stock in Stocks)
            //{
            //    stock.UpdateFromStock(GetStock(stock.Ticker));
            //}
        }

        #region ISTockService
        private ObservableCollection<IStock> _stocks = new ObservableCollection<IStock>();
        /// <summary>
        /// List of all stocks we know about
        /// </summary>
        public ObservableCollection<IStock> Stocks
        {
            get
            {
                return _stocks;
            }
            set
            {
                _stocks = value;
                RaisePropertyChanged();
            }
        }



        #endregion


        #region Private Methods
        private void CreateListFromText()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "StockScreener.tickers_cap.txt";
            var stockList = new ObservableCollection<IStock>();

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    try
                    {


                        //strip out whitespace
                        line.Replace(" ", "");
                        var splitLine = line.Split(',');
                        if (splitLine.Length != 2)
                        {
                            Trace.WriteLine("Bad parse of value: " + line);
                            continue;
                        }
                        //make sure the market cap units contains B or M
                        if (splitLine[1][splitLine[1].Length - 1] == 'B' || splitLine[1][splitLine[1].Length - 1] == 'M')
                        {
                            var stock = new Stock(splitLine[0], splitLine[1]);
                            //Only dd if we don't have it already
                            if (!stockList.Any(x => x.Ticker == stock.Ticker))
                                stockList.Add(new Stock(splitLine[0], splitLine[1]));
                        }
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
            //make sure its unique
             Stocks = stockList;
        }


        //TODO
        private IStock GetStock(string ticker)
        {
            string jsonReturn = "";
            string csvResturn = "";
            //THIS IS JUST TESTING ON HOW TO GET A STOCK VALUE.  WANTED TO DO IN A PLACE I KNEW WOULD GET CALLED
            using (var httpClient = new WebClient())
            {
                //todo use either csv or json and parse into the IStock object
                jsonReturn = httpClient.DownloadString(string.Format("https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={0}&interval=1min&outputsize=compact&apikey=V2K6MPHJVXGETLMF", ticker));
                csvResturn = httpClient.DownloadString(string.Format("https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={0}&interval=1min&outputsize=compact&apikey=V2K6MPHJVXGETLMF&datatype=csv", ticker));
            }

            //createa a stock from the value and return it
            return new Stock(jsonReturn);
        }

        /// <summary>
        /// Refresh the values inside a stock
        /// </summary>
        /// <param name="stock"></param>
        private void UpdateStock(IStock stock)
        {
            //request new info
            var retrievedStock = GetStock(stock.Ticker);
            //populate the stock with the result
            stock.UpdateFromStock(retrievedStock);
        }
        #endregion

    }
}