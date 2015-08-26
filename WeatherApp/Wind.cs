
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{

    /// <summary>
    /// This class decribes the wind's parameters that provided by the web service.
    /// </summary>
    public class Wind
    {
        public Speed Speed { get; set; }
        public Direction Direction { get; set; }
    }

    public class Speed
    {
        public double Value { get; set; }
        public string Name { get; set; }
    }

    public class Direction
    {
        public double Value { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
