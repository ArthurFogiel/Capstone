
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
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
        [PreferredConstructorAttribute]
        public StockService()
        {
            //Testing: get microsoft stock
            GetStock("MSFT");

            //TODO Initialize
            //See if we have a serialized list of stocks
            //If so load that in then iterate over them all to refresh a newer value

            //If not, use the text file to generate the list of stocks
            CreateListFromText();



            //start forever updating the list
            //example:
            //foreach(var stock in Stocks)
            //{
            //    stock.UpdateFromStock(GetStock(stock.Ticker));
            //}
        }

        private void CreateListFromText()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "StockScreener.tickers_cap.txt";
            var stockList = new List<IStock>();

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
                        if(splitLine.Length != 2)
                        {
                            Trace.WriteLine("Bad parse of value: " + line);
                            continue;
                        }
                        //make sure the market cap units contains B or M
                        if (splitLine[1][splitLine[1].Length - 1] == 'B' || splitLine[1][splitLine[1].Length - 1] == 'M')
                        {
                            var stock = new Stock(splitLine[0], splitLine[1]);
                            //Only dd if we don't have it already
                            if(!stockList.Any(x=>x.Ticker == stock.Ticker))
                                stockList.Add(new Stock(splitLine[0], splitLine[1]));
                        }
                    }
                    catch(Exception e)
                    {

                    }
                }
            }
            //make sure its unique
            Stocks = stockList;
        }

        private List<IStock> _stocks;

        public List<IStock> Stocks
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
    }
}