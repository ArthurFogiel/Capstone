
using System.Windows.Input;

namespace StockScreener.Interfaces
{
    public interface ILoginViewModel
    {
        /// <summary>
        /// Connected to the text box for the inputted user name
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// Do we have a user logged in?  Check if logged in user in the service is not null
        /// </summary>
        bool IsLoggedIn { get; set; }

        /// <summary>
        /// Handle button press for logging in
        /// </summary>
        ICommand LoginCommand { get; }

        /// <summary>
        /// Handle create user button press
        /// </summary>
        ICommand CreateUser { get; }
    }
}
