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
    /// This Class WWOnlineDataService works with the second web service :http://api.worldweatheronline.com
    /// This Class is singelton.
    /// The Class is responsible to send http request and return the response in wData object.
    /// </summary>
    class WWOnlineDataService : IWeatherDataService
    {
        private static WWOnlineDataService instance;

        private WWOnlineDataService()
        { }

        public static WWOnlineDataService Instance
        {
            get { return instance ?? (instance = new WWOnlineDataService()); }
        }
        /// <summary>
        /// This method is responsible to send http request to the provided URL and get the reponse to XML,
        /// The XML will provide all the detalis we need to build Weather Data object and return it.
        /// </summary>   
        public WeatherData GetWeatherData(Location location)
        {
            //wData is the object that holds the weather info.
            var wData = new WeatherData();
          
            XDocument xdoc;
            //api var holds the http response.
            var api = string.Format("http://api.worldweatheronline.com/free/v2/weather.ashx?key=dbc688a2aa9e77e11005ceb43ab37&q={0}&format=xml", location.Name);
            //Parse the XML
            try
            {
                xdoc = XDocument.Load(api);

                var list = from item in xdoc.Descendants("data")
                    select new
                    {

                        Name = item.Element("request").Element("query").Value,
                        LastTime = item.Element("current_condition").Element("observation_time").Value,
                        TempValue = item.Element("current_condition").Element("temp_C").Value,
                        LastDate = item.Element("weather").Element("date").Value,
                        SunRise = item.Element("weather").Element("astronomy").Element("sunrise").Value,
                        SunSet = item.Element("weather").Element("astronomy").Element("sunset").Value,

                        WindSpeedValue = item.Element("weather").Element("hourly").Element("windspeedMiles").Value,


                        IconName = item.Element("weather").Element("hourly").Element("weatherIconUrl").Value,
                        IconText = item.Element("weather").Element("hourly").Element("weatherDesc").Value


                    };
                //Insert the relevant parameters to wData object.
                foreach (var data in list)
                {
                    wData.City = new City();
                    var tokens = data.Name.Split(',');
                    wData.City.Name = tokens[0];
                    wData.City.Country = tokens[1];

                    wData.City.Sun.Rise = DateTime.Parse(data.SunRise);
                    wData.City.Sun.Set = DateTime.Parse(data.SunSet);

                    wData.Temp = new Temperature();
                    wData.Temp.Value = double.Parse(data.TempValue);

                    wData.Wind = new Wind();
                    wData.Wind.Speed = new Speed();
                    wData.Wind.Speed.Value = double.Parse(data.WindSpeedValue);

                    wData.Weather = new Weather();
                    wData.Weather.Icon = data.IconName;
                    wData.Weather.Value = data.IconText;

                    wData.LastUpdateTime = DateTime.Parse(data.LastDate + " " + data.LastTime);
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

            //return the wData object which now is updated .
            return wData;
        }
    }
}
