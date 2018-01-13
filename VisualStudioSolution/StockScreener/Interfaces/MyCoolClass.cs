using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScreener.Interfaces
{
    class MyCoolClass : IMyCoolInterface
    {
        public string test
        {
            get { return ""; }
        }

        public bool test2
        {
            get { return false; }
            set { }
        }


        public List<IStock> Test;


        void GetStocks()
        {
            var oneDollarStocks = Test.Where(x => x.CurrentPrice == 1);
        }
    }
}
