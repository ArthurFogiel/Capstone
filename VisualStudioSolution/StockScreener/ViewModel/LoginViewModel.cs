using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using StockScreener.Interfaces;

namespace StockScreener.ViewModel
{
    /// <summary>
    /// Supporting the LoginView
    /// <para>
    /// See http://www.mvvmlight.net for base class
    /// </para>
    /// </summary>
    public class LoginViewModel : ViewModelBase, ILoginViewModel
    {
        private IUserInfoService _userService;
        private IStockService _stockservice;
        #region constructor
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public LoginViewModel(IUserInfoService userService, IStockService stockService)
        {
            _userService = userService;
            _stockservice = stockService;
            //listen to property changes to know when the logged in user changes
            _userService.PropertyChanged += _userService_PropertyChanged;
            //see if we have any failures reading/writing to disk
            CheckSaveReadErrors();
        }

        
        #endregion

        #region ILoginViewModel
        string _userName="";
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                Set(ref _userName, value);
            }
        }

        public IStockService StockService
        {
            get
            {
                return _stockservice;
            }
        }

        private ICommand _loginCommand;
        /// <summary>
        /// Button for logging in
        /// </summary>
        public ICommand LoginCommand
        {
            get
            {
                if(_loginCommand == null)
                {
                    _loginCommand = new CommandHandler(() => LoginPressed());
                }
                return _loginCommand;
            }
        }

        private ICommand _creatUserCommand;
        /// <summary>
        /// Button for creating users
        /// </summary>
        //TODO
        public ICommand CreateUser
        {
            get
            {
                if(_creatUserCommand == null)
                {
                    _creatUserCommand = new CommandHandler(() => CreateUserPressed());
                }
                return _creatUserCommand;
            }
        }

        /// <summary>
        /// Is there a logged in user?
        /// </summary>
        public bool IsLoggedIn
        {
            get { return _userService.LoggedInUser != null; }
        }

        public string AppTitle
        {
            get
            {
                if (!IsLoggedIn)
                    return "Stock Screener: User: No User Logged In";
                else if (_userService.LoggedInUser != null)
                {
                    return "Stock Screener: User: " + _userService.LoggedInUser.Name;
                }
                return "";
            }
        }

        /// <summary>
        /// Code called when login is pressed
        /// </summary>
        public void LoginPressed()
        {
            if (!_userService.LogInUser(UserName))
            {
                MessageBox.Show("Failed to log in!  Create a new user or enter an existing username");
            }
        }

        public void CreateUserPressed()
        {
            if (UserName == "")
            {
                MessageBox.Show("User Name cannot be empty!  Please enter a user name");
                return;
            }

            if (!_userService.CreateUser(UserName))
            {
                MessageBox.Show("That username is already taken! Please enter a different username");
            }

        }
        #endregion

        private void _userService_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "LoggedInUser")
            {
                //Let everyone know the is logged in changed.
                RaisePropertyChanged("IsLoggedIn");
                //Update the title
                RaisePropertyChanged("AppTitle");
            }
            if(e.PropertyName == "FailedUserLoadOrSave")
            {
                CheckSaveReadErrors();
            }
        }

        private void CheckSaveReadErrors()
        {
            if(_userService.FailedUserLoadOrSave)
            {
                MessageBox.Show("Failed to Read/Save the user file to disk.  Ensure read/write permissions are set on the folder at: " + Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Screener\\");
            }
        }
    }
}