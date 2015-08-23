using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WeatherApp
{
    /// <summary>
    /// This Class OpenMapDataService works with the web service :http://api.openweathermap.org.
    /// This Class is singelton.
    /// The Class is responsible to send http request and return the response in wData object.
    /// </summary>
    public class OpenMapDataService : IWeatherDataService
    {
        private static OpenMapDataService instance;
      
        private  OpenMapDataService()
        { }

        public static  OpenMapDataService Instance
        {
            get { return instance ?? (instance = new OpenMapDataService()); }
        }
        /// <summary>
        /// This method is responsible to send http request to the web service and get the reponse to XML,
        /// The XML will provide all the info we need to build wData object and return this object.
        /// </summary>
        public WeatherData GetWeatherData(Location location)
        {
            XDocument xdoc;
            //wData is the object that holds the weather info.
            var wData = new WeatherData();
            //api var holds the http response.
            var api = string.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&mode=xml", location.Name);

            //Parse the xdoc .
            try
            {
                xdoc = XDocument.Load(api);

                var list = from item in xdoc.Descendants("current")
                    select new
                    {

                        Name = item.Element("city").Attribute("name").Value,
                        CoordLat = item.Element("city").Element("coord").Attribute("lat").Value,
                        CoordLon = item.Element("city").Element("coord").Attribute("lon").Value,
                        Country = item.Element("city").Element("country").Value,
                        SunRise = item.Element("city").Element("sun").Attribute("rise").Value,
                        SunSet = item.Element("city").Element("sun").Attribute("set").Value,


                        TempValue = item.Element("temperature").Attribute("value").Value,


                        WindSpeedValue = item.Element("wind").Element("speed").Attribute("value").Value,
                        WindSpeedText = item.Element("wind").Element("speed").Attribute("name").Value,

                        IconName = item.Element("weather").Attribute("icon").Value,
                        IconText = item.Element("weather").Attribute("value").Value,
                        LastUpdate = item.Element("lastupdate").Attribute("value").Value

                    };
                //Insert the relevant parameters to wData object.
                foreach (var data in list)
                {
                    //Insert all the City parameters 
                    wData.City = new City();
                    wData.City.Name = data.Name;
                    wData.City.Country = data.Country;
                    wData.City.Coords.Lat = double.Parse(data.CoordLat);
                    wData.City.Coords.Lon = double.Parse(data.CoordLon);
                    wData.City.Sun.Rise = DateTime.Parse(data.SunRise);
                    wData.City.Sun.Set = DateTime.Parse(data.SunSet);
                    //Insert all the Temperature parameters 
                    wData.Temp = new Temperature();
                    wData.Temp.Value = double.Parse(data.TempValue);
                    wData.Temp.Value -= 272.15;
                    //Insert all the wind parameters 
                    wData.Wind = new Wind();
                    wData.Wind.Speed = new Speed();
                    wData.Wind.Speed.Value = double.Parse(data.WindSpeedValue);
                    wData.Wind.Speed.Name = data.WindSpeedText;

                    wData.Weather = new Weather();
                    wData.Weather.Icon = "http://openweathermap.org/img/w/" + data.IconName + ".png";
                    wData.Weather.Value = data.IconText;

                    wData.LastUpdateTime = DateTime.Parse(data.LastUpdate);
                }
            }
            //Exceptions
            catch (WebException)
            {
                throw new WeatherDataServiceException("Web connectivity Exception");
            }
            catch (Exception)
            {

                throw new WeatherDataServiceException("Parsing Exception");
            }
            //return the wData object.
            return wData;
        }
    }
}
