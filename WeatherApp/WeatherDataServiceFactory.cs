using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{
    /// <summary>
    /// This class is the WeatherDataServiceFactory which enables to add more web services .
    /// </summary>
   
    public static class WeatherDataServiceFactory
    {
        public enum ServiceType
        {
            OPEN_WEATHER_MAP,
            WORLD_WEATHER_ONLINE
        };

        public static IWeatherDataService getWeatherDataService(ServiceType service)
        {
            IWeatherDataService dataService = null;
            switch (service)
            {
                case ServiceType.OPEN_WEATHER_MAP:
                    dataService = OpenMapDataService.Instance;
                    break;
                case ServiceType.WORLD_WEATHER_ONLINE:
                    dataService = WWOnlineDataService.Instance;
                    break;

            }

            return dataService;
        }
    }
}
