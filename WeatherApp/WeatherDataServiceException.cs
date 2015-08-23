using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{
    [Serializable()]
    /// <summary>
    /// This class handles with the exceptions that were thrown from WeatherDataService class .  
    /// </summary>
    public class WeatherDataServiceException : System.Exception
    {
        public WeatherDataServiceException()
        {
        }

        public WeatherDataServiceException(string message)
            : base(message)
        {
            Console.WriteLine(message);
        }

        public WeatherDataServiceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
