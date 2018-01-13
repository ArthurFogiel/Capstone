using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StockScreener.Model
{
    /// <summary>
    /// This is so classes can support raising property changes to support bindings or let others listen to changes
    /// </summary>
    public abstract class Notifyable: INotifyPropertyChanged
    {
        //So classes can subscribe to events
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
