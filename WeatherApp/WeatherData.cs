using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{
    /// <summary>
    /// This class describes the WeatherData that summarizes the weather data we get from the web service.  
    /// </summary>
    public class WeatherData
    {
        public City City { get; set; }
        public Temperature Temp { get; set; }
        public Wind Wind { get; set; }
        public Weather Weather { get; set; }
        public DateTime LastUpdateTime { get; set; } 
    }
}
