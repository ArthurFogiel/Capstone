using StockScreener.Model;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace StockScreener.Interfaces
{
    public interface IUser: IXmlSerializable
    {
        /// <summary>
        /// string name of the user
        /// </summary>
        string Name { get; }
        /// <summary>
        /// List of stocks to watch
        /// </summary>
        List<string> WatchedStocks { get; set; }
        /// <summary>
        /// Users settings, updated on each apply
        /// </summary>
        ISettings Settings { get; }

    }
}
