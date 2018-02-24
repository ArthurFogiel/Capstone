using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScreener.Model
{
    public class Wrapper
    {

        [JsonProperty("quote")]
        public Quote ValueSet{get;set;}
    }
}
