
using System.Windows.Input;

namespace StockScreener.Interfaces
{
    public interface ILoginViewModel
    {
        string UserName { get; set; }

        IUserInfoService UserInfoService { get; }

        ICommand LoginCommand { get; }


        ICommand CreateUser { get; }
    }
}
