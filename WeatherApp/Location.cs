using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{
    /// <summary>
    /// This class describes the location ,it is the city name that the user provides to the Weather app.
    /// </summary>
    public class Location
    {
        public string Name { get; set; }

        public Location(string name)
        {
            Name = name;
        }
    }
}
