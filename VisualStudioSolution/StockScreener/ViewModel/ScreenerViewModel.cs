using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml;
using GalaSoft.MvvmLight;
using StockScreener.Interfaces;

namespace StockScreener.ViewModel
{
    /// <summary>
    /// This class contains properties that the Screener View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net for base class
    /// </para>
    /// </summary>
    public class ScreenerViewModel : ViewModelBase, IStockScreenerViewModel
    {
        private readonly IUserInfoService _userService;
        private readonly IStockService _stockService;

        [GalaSoft.MvvmLight.Ioc.PreferredConstructor]
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public ScreenerViewModel(IUserInfoService userService, IStockService stockService)
        {
            _userService = userService;
            _stockService = stockService;

            //listen to logged in user changes to update the watch list
            _userService.PropertyChanged += _userService_PropertyChanged;
            //initialize to already logged in user
            MatchWatchedToUser();

        }


        /// <summary>
        /// This constructor is Just for designer for showing the UI elements
        /// </summary>
        public ScreenerViewModel()
        {

        }

        #region IStockScreenerViewModel

        public IUserInfoService UserInfoService
        {
            get { return _userService; }
        }

        public IStockService StockService
        {
            get { return _stockService; }
        }

        private ObservableCollection<IStock> _filteredStocks = new ObservableCollection<IStock>();
        /// <summary>
        /// The filtered list of stocks using the logged in users settings and the full list of stocks from the model
        /// </summary>
        public ObservableCollection<IStock> FilteredStocks
        {
            get
            {
                return _filteredStocks;
            }
            private set
            {
                _filteredStocks = value;
                RaisePropertyChanged();
            }
        }
        private ObservableCollection<IStock> _watchedStocks = new ObservableCollection<IStock>();
        /// <summary>
        /// The list of stocks from the users watch list
        /// </summary>
        public ObservableCollection<IStock> WatchedStocks
        {
            get
            {
                return _watchedStocks;
            }
            private set
            {
                _watchedStocks = value;
                RaisePropertyChanged();
            }
        }


        private ICommand _saveFavorite;
        public ICommand SaveFavorite
        {
            get
            {
                if (_saveFavorite == null)
                {
                    _saveFavorite = new CommandHandler(() => SaveFavoritePressed());
                }
                return _saveFavorite;
            }
        }

        private void SaveFavoritePressed()
        {
            if (_userService.LoggedInUser == null)
            {
                MessageBox.Show("No user is logged in, cannot save the settings!");
                return;
            }
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "xml files (*.xml)|*.xml";
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(saveFileDialog1.FileName));
                    using (var stream = File.OpenWrite(saveFileDialog1.FileName))
                    {
                        var xmlWriterSettings = new XmlWriterSettings() { Indent = true, NewLineOnAttributes = true };
                        using (var writer = XmlWriter.Create(stream, xmlWriterSettings))
                        {
                            _userService.LoggedInUser.Settings.WriteXml(writer);
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Failed to save the settings to file.  File PATH = " + saveFileDialog1.FileName);
                }
            }
        }

        private ICommand _loadFavorite;
        public ICommand LoadFavorite
        {
            get
            {
                if (_loadFavorite == null)
                {
                    _loadFavorite = new CommandHandler(() => LoadFavoritePressed());
                }
                return _loadFavorite;
            }
        }

        private void LoadFavoritePressed()
        {
            if (_userService.LoggedInUser == null)
            {
                MessageBox.Show("No user is logged in, cannot load the settings!");
                return;
            }

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "xml files (*.xml)|*.xml";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (XmlReader reader = XmlReader.Create(openFileDialog1.FileName))
                    {
                        while (reader.Read())
                        {
                            // Only detect start elements.
                            if (reader.IsStartElement())
                            {
                                // Get element name and switch on it.
                                switch (reader.Name)
                                {
                                    case "Setting":
                                        _userService.LoggedInUser.Settings.ReadXml(reader);
                                        return;
                                }
                            }
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Failed to read the settings file: " + openFileDialog1.FileName);
                }
            }
        }

        private ICommand _openHelp;
        public ICommand OpenHelp
        {
            get
            {
                if (_openHelp == null)
                {
                    _openHelp = new CommandHandler(() => OpenHelpPressed());
                }
                return _openHelp;
            }
        }

        private void OpenHelpPressed()
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //once you have the path you get the directory with:
            var directory = Path.GetDirectoryName(path);
            directory += "\\Views\\Resources\\Stock Screener User Guide.pdf";
            //If we got here, user hit enter so pop up file explorer to exe path
            Process.Start(directory);
        }



        private ICommand _apply;
        /// <summary>
        /// When apply is pushed, we want to refresh the FilteredStocks with only stocks from the stock service stocks list that are within
        /// The LoggedInUser.Settings values.
        /// </summary>
        public ICommand Apply
        {
            get
            {
                if (_apply == null)
                {
                    _apply = new CommandHandler(() => ApplyPressed());
                }
                return _apply;
            }
        }

        /// <summary>
        /// When apply is pressed we want to filter based on settings
        /// </summary>
        private void ApplyPressed()
        {
            //if somehow no user bail out
            if (_userService.LoggedInUser == null) return;

            //Get the users settings and use them to filter the stocks
            var tempList = new ObservableCollection<IStock>();
            //make a quick reference to the settings
            var settings = _userService.LoggedInUser.Settings;
            int multiplier = 0;
            if (settings.MarketCapUnits == MarketCapUnitsEnum.Millions)
                multiplier = 1;
            else
                multiplier = 1000;
            foreach (var stock in _stockService.Stocks)
            {
                //settings volume is in millions, settings market cap varies based on units.
                if((stock.MarketCap > (settings.MarketCapMin* multiplier) && stock.MarketCap < (settings.MarketCapMax* multiplier)) &&
                    (stock.CurrentPrice > settings.PriceMin && stock.CurrentPrice < settings.PriceMax) &&
                    (stock.CurrentVolume > (settings.VolumeMin) && stock.CurrentVolume < (settings.VolumeMax)))
                {
                    tempList.Add(stock);
                }
            }
            FilteredStocks = tempList;
            //save the user settings to remember the change
            _userService.SaveUsersToFile();
        }

        private ICommand _logout;
        /// <summary>
        /// Log out the user command
        /// </summary>
        public ICommand LogOut
        {
            get
            {
                if (_logout == null)
                {
                    _logout = new CommandHandler(() => LogOutPressed());
                }
                return _logout;
            }
        }

        /// <summary>
        /// Log out the user
        /// </summary>
        private void LogOutPressed()
        {
            UserInfoService.LogOffUser();
        }

        private ICommand _watchCommand;
        public ICommand WatchCommand
        {
            get
            {
                if (_watchCommand == null)
                {
                    _watchCommand = new CommandHandler(param => WatchPressed(param));
                }
                return _watchCommand;
            }
        }

        private void WatchPressed(object param)
        {
            //add the stock to the users settings as watch if not alread
            if (!UserInfoService.LoggedInUser.WatchedStocks.Contains(((IStock)param).Ticker))
            {
                UserInfoService.LoggedInUser.WatchedStocks.Add(((IStock)param).Ticker);
                //Save user to disk since we changed to items
                UserInfoService.SaveUsersToFile();
                //Update the watch list
                WatchedStocks.Add((IStock)param);
            }
        }

        private ICommand _removeWatchCommand;
        public ICommand RemoveWatchCommand
        {
            get
            {
                if (_removeWatchCommand == null)
                {
                    _removeWatchCommand = new CommandHandler(param => RemoveWatchPressed(param));
                }
                return _removeWatchCommand;
            }
        }
        /// <summary>
        /// Remove the watched stock
        /// </summary>
        /// <param name="param"></param>
        private void RemoveWatchPressed(object param)
        {
            //remove from the user which will trigger a refresh
            if(_userService.LoggedInUser.WatchedStocks.Any(x=>x == ((IStock)param).Ticker))
            {
                _userService.LoggedInUser.WatchedStocks.Remove(((IStock)param).Ticker);
                _userService.SaveUsersToFile();
                MatchWatchedToUser();
            }
        }


        #endregion

        #region Private methods

        private void _userService_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "LoggedInUser")
            {
                MatchWatchedToUser();
                //Apply the newly logged in users filters
                Apply.Execute(null);
            }
        }

        private void MatchWatchedToUser()
        {
            if (_userService.LoggedInUser != null)
            {
                var stocks = new ObservableCollection<IStock>();
                foreach (var stock in _userService.LoggedInUser.WatchedStocks)
                {
                    var stockReference = _stockService.Stocks.FirstOrDefault(x => x.Ticker == stock);
                    //Just in case somehow the watched stock no longer exists in the list of stocks
                    if (stockReference != null)
                        stocks.Add(stockReference);
                }
                WatchedStocks = stocks;
            }
        }

        #endregion

    }
}