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
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public LoginViewModel(IUserInfoService userService)
        {
            _userService = userService;
        }

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

        //TODO
        public ICommand CreateUser => throw new System.NotImplementedException();

        /// <summary>
        /// Code called when login is pressed
        /// </summary>
        public void LoginPressed()
        {
            Debug.WriteLine("I ENTERED THE LOGIN PRESSED");
            if (!_userService.LogInUser(UserName))
            {
                MessageBox.Show("Failed to log in!  Create a new user or enter an existing username");
            }
        }




        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}