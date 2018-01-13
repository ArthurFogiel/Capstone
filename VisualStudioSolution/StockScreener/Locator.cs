/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:MvvmLight1.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
*/

using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using StockScreener.Interfaces;
using StockScreener.Model;
using StockScreener.ViewModel;

namespace StockScreener
{
    /// <summary>
    /// This class registers Interfaces to implementation
    /// Also provides properties for views to bind to view models
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class Locator
    {
        static Locator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            //Register Services interfaces to implementation
            SimpleIoc.Default.Register<IStockService, StockService>();
            SimpleIoc.Default.Register<IUserInfoService, UserInfoService>();

            //Register View Models interfaces to implementations
            SimpleIoc.Default.Register<ILoginViewModel, LoginViewModel>();
            SimpleIoc.Default.Register<IStockScreenerViewModel, ScreenerViewModel>();
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]


        //Properties available to views the retrieve the registered implementation

        public ILoginViewModel LoginViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ILoginViewModel>();
            }
        }

        public IStockScreenerViewModel ScreenerViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IStockScreenerViewModel>();
            }
        }


        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}