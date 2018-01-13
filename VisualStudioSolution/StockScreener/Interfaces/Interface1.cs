using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScreener.Interfaces
{
    public interface IStock
    {
        string ticker { get; set; }

        float MarketCap { get; set; }

        float CurrentPrice { get; set; }

        float LastClosePrice { get; set; }

        float CurrentVolume { get; set; }
    }
}
