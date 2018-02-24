
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Ioc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StockScreener.Interfaces;

namespace StockScreener.Model
{
    /// <summary>
    /// Implementation of a stock service
    /// Manages getting and updating stocks
    /// </summary>
    public class StockService : Notifyable, IStockService
    {
        /// <summary>
        /// This service is in charge of updating and maintaining the master list of stocks
        /// </summary>
        [PreferredConstructorAttribute]
        public StockService()
        {

            //On another thread initialize the stocks by asking nasdaq .com for a master list of stocks
            Task task = Task.Factory.StartNew(() =>
            {
                //Ask for all the stock tickers supported and initialize the list
                try
                {
                    GenerateStocksFromMasterLists();
                    IsOnline = true;
                    //Don't set initialized, we want to query everything once.
                }
                catch (WebException e)
                {
                    Debug.WriteLine("");
                    Debug.WriteLine("Setting online to false since web exception:");
                    Debug.WriteLine(e.InnerException);
                    Debug.WriteLine("");
                    IsOnline = false;
                    InitializedSuccessfully = false;
                }
                catch (Exception e)
                {
                    InitializedSuccessfully = false;
                }
                finally
                {
                }

            });

        }


        #region IStockService

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

        private bool _isOnline = false;

        public bool IsOnline
        {
            get
            {
                return _isOnline;
            }
            private set
            {
                //if nothing changed return
                if (_isOnline = value)
                    return;
                _isOnline = value;
                RaisePropertyChanged();
            }
        }

        private bool _isInitialized = false;

        public bool IsInitialized
        {
            get { return _isInitialized; }
            private set
            {
                _isInitialized = value;
                RaisePropertyChanged();
            }
        }

        private bool _initializedSuccessfully = true;
        public bool InitializedSuccessfully
        {
            get
            {
                return _initializedSuccessfully;
            }
            private set
            {
                _initializedSuccessfully = value;
                RaisePropertyChanged();
            }
        }

        #endregion


        #region Private Methods

        /// <summary>
        /// Generate the Stocks List from a master list (done at startup)
        /// </summary>
        private void GenerateStocksFromMasterLists()
        {
            var stopWatch = new Stopwatch();
            //setup request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("https://api.iextrading.com/1.0/ref-data/symbols?format=csv"));

            // Set some reasonable limits on resources used by this request
            request.MaximumAutomaticRedirections = 4;
            request.MaximumResponseHeadersLength = 4;
            // Set credentials to use for this request.
            request.Credentials = CredentialCache.DefaultCredentials;
            string responseString = "";
            stopWatch.Start();
            //Ask IEX for the symbols they support
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                // Get the stream associated with the response.
                Stream receiveStream = response.GetResponseStream();

                // Pipes the stream to a higher level stream reader with the required encoding format. 
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                responseString = readStream.ReadToEnd();
            }
            stopWatch.Stop();
            Debug.WriteLine("");
            Debug.WriteLine("Time to get all tickers = " + stopWatch.ElapsedMilliseconds + " ms.");
            Debug.WriteLine("");
            stopWatch.Reset();

            stopWatch.Start();
            //Go and create stocks and add to our list of stocks
            PopulateStockListFromParsedMasterList(responseString);
            stopWatch.Stop();
            Debug.WriteLine("");
            Debug.WriteLine("Time to parse all stocks = " + stopWatch.ElapsedMilliseconds + " ms. NumberStocks = " + Stocks.Count());
            Debug.WriteLine("");
        }

        /// <summary>
        /// Parse a exchange list into stocks and add to the Stocks list
        /// </summary>
        /// <param name="list"></param>
        /// <param name="exchange"></param>
        private void PopulateStockListFromParsedMasterList(string list)
        {
            ObservableCollection<IStock> ParsedStocks = new ObservableCollection<IStock>();
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
                    //ensure we have 6 items
                    //Symbol,Name,Date,IsEnabled, type, id
                    if (items.Length != 6) continue;
                    //only if enabled
                    if (items[3] != "true") continue;
                    try
                    {
                        
                        var stock = new Stock(items[0], items[1]);
                        ParsedStocks.Add(stock);
                    }
                    //Its ok if we fail, it just won't be added.  The stock will post a debug message.
                    catch
                    {
                        var output = new StringBuilder();
                        foreach (var item in items)
                        {
                            output.Append(item);
                        }
                        Debug.WriteLine("Failed parsing line into stock: " + output.ToString());
                    }
                }
            }

            //Get on the UI thread to update the stock list
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                foreach (var item in ParsedStocks)
                {
                    Stocks.Add(item);
                }
                //start listening
                StartUpdaterThreads();
            }));

        }

        /// <summary>
        /// See if market should be open or not
        /// </summary>
        /// <returns></returns>
        private bool MarketOpen()
        {
            var time = DateTime.UtcNow;
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(time, easternZone);
            //not gauranteed in case of holiday, but should be good in most cases
            if ((easternTime.Hour < 16 && (easternTime.Hour > 9 && easternTime.Minute > 30)) &&
                (easternTime.DayOfWeek > 0 && (int)easternTime.DayOfWeek < 6))
                return true;
            return false;
        }

        /// <summary>
        /// Begin the thread(s) to update the stocks real time
        /// </summary>
        private void StartUpdaterThreads()
        {
            //Only up to 100 can be requested at a time, so plit our stocks into groups of 100
            var numberToCreate = Stocks.Count() / 100;
            //if there is a remainder, just add an extra 100, the take below will only grab whats there
            if (Stocks.Count() % 100 != 0)
                numberToCreate++;
            //create the lists
            var tickerGroups = new List<List<string>>();
            for (int i = 0; i < numberToCreate; i++)
            {
                List<string> subList = Stocks.Select(x => x.Ticker).ToList<string>().Skip(i * 100).Take(100).ToList();

                tickerGroups.Add(subList);
            }
            
            //Debug timings
            var stopWatch = new Stopwatch();

            //do forever in another thread
            //Note we could use more than one thread but since it only takes a couple seconds and the data does not update on the server faster than once a minute no need yet.
            Task thread = Task.Factory.StartNew(() =>
            {
                while (true)
                {

                    //if the time is outside of 9am - 5pm easter M - F there is no way we will get values so bail after as long as we initialized once
                    if (!MarketOpen() && IsInitialized)
                    {
                        Debug.WriteLine("");
                        Debug.WriteLine("Market CANNOT be open, skipping update");
                        Debug.WriteLine("");
                        //sleep for 1 minute then try again
                        Thread.Sleep(60000);
                        continue;
                    }

                    stopWatch.Start();
                    //iterate over groups and update each
                    foreach (var group in tickerGroups)
                    {
                        try
                        {
                            UpdateStocks(group);
                            IsOnline = true;
                        }

                        catch (WebException e)
                        {
                            Debug.WriteLine("");
                            Debug.WriteLine("Setting online to false since web exception:");
                            Debug.WriteLine(e.InnerException);
                            Debug.WriteLine("");
                            //If web exception, bail out and set not online to true
                            IsOnline = false;
                            if (!IsInitialized)
                            {
                                IsInitialized = true;
                                InitializedSuccessfully = false;
                            }
                            break;
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine("");
                            Debug.WriteLine("Exception when updating stocks");
                            Debug.WriteLine(e.InnerException);
                            Debug.WriteLine("");
                            //otherwise could be bad parse, just continue
                        }
                    }
                    stopWatch.Stop();
                    Debug.WriteLine("");
                    Debug.WriteLine("Finish updating all stocks in " + stopWatch.ElapsedMilliseconds + " ms");
                    Debug.WriteLine("");
                    stopWatch.Reset();
                    //We initialized at least once
                    if(!IsInitialized)
                    {
                        IsInitialized = true;
                        InitializedSuccessfully = true;
                    }

                    Debug.WriteLine("Sleep for a a minute so we don't thrash the server.");
                    Thread.Sleep(60000);
                }
            });
        }

        /// <summary>
        /// Request quotes for the passed in tickers and update our stocks to reflect the new values
        /// </summary>
        /// <param name="tickers"></param>
        private void UpdateStocks(List<string> tickers)
        {
            //make the string list to send
            var stringParam = new StringBuilder();
            foreach (var stock in tickers)
            {
                stringParam.Append(stock);
                if (tickers.IndexOf(stock) != tickers.Count - 1)
                    stringParam.Append(",");
            }

            //Debug timings
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("https://api.iextrading.com/1.0/stock/market/batch?symbols={0}&types=quote", stringParam.ToString()));
            // Set some reasonable limits on resources used by this request
            request.MaximumAutomaticRedirections = 4;
            request.MaximumResponseHeadersLength = 4;
            // Set credentials to use for this request.
            request.Credentials = CredentialCache.DefaultCredentials;
            string stringReturn = "";
            //On failure log to output window and skip out
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                    stringReturn = readStream.ReadToEnd();
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine("");
                Debug.WriteLine("Failed to get http response: Message = " + request.ToString());
                Debug.WriteLine("Exception = " + e.InnerException);
                Debug.WriteLine("");
                return;
            }

            //parse the resturned JSON
            JObject json = JObject.Parse(stringReturn);
            //turn each response into a quote then update the stock
            foreach (var x in json)
            {
                string name = x.Key;
                JToken value = x.Value.First.First;

                Quote quote = null;
                try
                {
                    quote = JsonConvert.DeserializeObject<Quote>(value.ToString());
                }
                catch (Exception e)
                {
                    //log and ignore to continue
                    Debug.WriteLine("Failed to deserialize JSON quote: " + value.ToString());
                }

                //get the stock from the master list and update it
                var listStock = Stocks.FirstOrDefault(stockInList => stockInList.Ticker == quote.Symbol);
                ////make sure it exists
                if (listStock != null)
                {
                    listStock.UpdateFromQuote(quote);
                }    
            }

            //print some timings for debugging
            stopWatch.Stop();
            Debug.WriteLine("");
            Debug.WriteLine("Completed http request for list of stocks in: " + stopWatch.ElapsedMilliseconds + " ms.  For group: " + tickers[0]);
            Debug.WriteLine("");
        }
        #endregion
    }
}