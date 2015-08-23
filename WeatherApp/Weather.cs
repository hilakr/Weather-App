using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{
    /// <summary>
    /// This class decribes Weather and the Weather's parameters that provided by the web.
    /// </summary>
    public class Weather
    {
        public int Number { get; set; }
        public string Value { get; set; }
        public string Icon { get; set; }
    }
}
