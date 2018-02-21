
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
        private ObservableCollection<IStock> tempInitialStocks = new ObservableCollection<IStock>();

        [PreferredConstructorAttribute]
        public StockService()
        {
            //Testing: get microsoft stock
            //   GetStock("MSFT");
            //

            //Read from nasdaq.com the master list of stocks and initial values
            GenerateStocksFromMasterLists();

            //TESTING JUST KEEP TOGGLING THE PRICE TO SEE IT MOVE
            int count = 0;
            Task t2 = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    count++;
                    Thread.Sleep(200);
                    foreach (var stock in Stocks)
                    {
                        stock.CurrentPrice = count % 2 == 0 ? stock.CurrentPrice - 1 : stock.CurrentPrice + 1;
                    }
                }
            });

            ////TESTING see how long it takes to query all our stocks through Alpha vantage
            //var stopWatch = new Stopwatch();
            //var stopWatch2 = new Stopwatch();
            //stopWatch.Start();
            //foreach(var stock in Stocks)
            //{

            //    stopWatch2.Start();
            //    GetStock(stock.Ticker);
            //    stopWatch2.Stop();
            //    Debug.WriteLine("Time to query stock " + stock.Ticker + " = " + stopWatch2.ElapsedMilliseconds + " ms.");
            //    stopWatch2.Reset();
            //}
            //stopWatch.Stop();
            //Debug.WriteLine("Time to query all stocks = " + stopWatch.ElapsedMilliseconds + " ms.  For: " + Stocks.Count() + " stocks");


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

        /// <summary>
        /// Passing in a stock get back a new one with the real time update from alphavantage
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private IStock GetUpdatedStock(IStock input)
        {
            try
            {
                string csvResturn = "";
                //THIS IS JUST TESTING ON HOW TO GET A STOCK VALUE.  WANTED TO DO IN A PLACE I KNEW WOULD GET CALLED
                using (var httpClient = new WebClient())
                {
                    //todo use either csv or json and parse into the IStock object
                    csvResturn = httpClient.DownloadString(string.Format("https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={0}&interval=1min&outputsize=compact&apikey=V2K6MPHJVXGETLMF&datatype=csv", input.Ticker));
                    //createa a stock from the value and return it
                    return new Stock(csvResturn.Split(','), StockStringSource.AlphaVantage, input.Exchange);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed to retrieve stock: " + input.Ticker);
                return null;
            }

        }

        /// <summary>
        /// Refresh the values inside a stock from another
        /// </summary>
        /// <param name="stock"></param>
        private void UpdateStock(IStock stock)
        {
            //request new info
            var retrievedStock = GetUpdatedStock(stock);
            //populate the stock with the result
            stock.UpdateFromStock(retrievedStock);
        }

      
        /// <summary>
        /// Generate the Stocks List from a master list (done at startup)
        /// </summary>
        private void GenerateStocksFromMasterLists()
        {
            string nasdaqItems;
            string nyseItems;
            string amexItems;
            var stopWatch = new Stopwatch();
            using (var httpClient = new WebClient())
            {

                stopWatch.Start();
                nasdaqItems = httpClient.DownloadString(string.Format("https://www.nasdaq.com/screening/companies-by-name.aspx?letter=0&exchange=nasdaq&render=download"));
                nyseItems = httpClient.DownloadString(string.Format("https://www.nasdaq.com/screening/companies-by-name.aspx?letter=0&exchange=nyse&render=download"));
                amexItems = httpClient.DownloadString(string.Format("https://www.nasdaq.com/screening/companies-by-name.aspx?letter=0&exchange=amex&render=download"));
                stopWatch.Stop();
                Debug.WriteLine("Time to query all stocks = " + stopWatch.ElapsedMilliseconds + " ms.");
            }
            stopWatch.Start();
            PopulateStockListFromParsedMasterList(nasdaqItems, ExchangeEnums.NASDAQ);
            PopulateStockListFromParsedMasterList(amexItems, ExchangeEnums.NYSE);
            PopulateStockListFromParsedMasterList(nyseItems, ExchangeEnums.AMEX);
            stopWatch.Stop();
            Debug.WriteLine("Time to parse all stocks = " + stopWatch.ElapsedMilliseconds + " ms. NumberStocks = " + Stocks.Count());
        }

        /// <summary>
        /// Parse a exchange list into stocks and add to the Stocks property
        /// </summary>
        /// <param name="list"></param>
        /// <param name="exchange"></param>
        private void PopulateStockListFromParsedMasterList(string list, ExchangeEnums exchange)
        {
            using (StringReader sr = new StringReader(list))
            {
                string line;
                //skip first line since its the header
                var test = sr.ReadLine();
                while ((line = sr.ReadLine()) != null)
                {


                    // extract the fields
                    Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                    string[] items = CSVParser.Split(line);


                    // clean up the fields (remove " and leading spaces)
                    for (int i = 0; i < items.Length; i++)
                    {
                        items[i] = items[i].TrimStart(' ', '"');
                        items[i] = items[i].TrimEnd('"');
                    }
                    //ensure we have 8 items
                    //Symbol,Name,LastSale,MarketCap,IPOyear,Sector,industry,Summary Quote,
                    if (items.Length != 9) continue;
                    try
                    {
                        var stock = new Stock(items, StockStringSource.NasdaqDotCom, exchange);
                        Stocks.Add(stock);
                    }
                    //Its ok if we fail, it just won't be added.  The stock will post a debug message.
                    catch
                    {
                        var output = new StringBuilder();
                        foreach(var item in items)
                        {
                            output.Append(item);
                        }
                        Debug.WriteLine("Failed parsing line into stock: " + output.ToString());
                    }
                }
            }
        }
        #endregion

    }
}