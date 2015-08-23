using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{
    /// <summary>
    /// This interface of WeatherDataService describes the actions the Weather app can provide.
    /// </summary>
    public interface IWeatherDataService
    {
         WeatherData GetWeatherData(Location location); 
    }
}
