using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{
    /// <summary>
    /// This class decribes City and the City's parameters that provided by the web.
    /// </summary>
    public class City
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Coordinates Coords { get; set; }
        public string Country { get; set; }
        public Sun Sun { get; set; }

        public City()
        {
            Coords = new Coordinates();
            Sun = new Sun();
        }
        
    }
    /// <summary>
    /// This class describes Coordinates of city.
    /// </summary>
    public class Coordinates
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
    }
    /// <summary>
    /// This class describes the sun rise and set of City 
    /// </summary>
    public class Sun
    {
        public DateTime Rise { get; set; }
        public DateTime Set { get; set; }
    }
}
