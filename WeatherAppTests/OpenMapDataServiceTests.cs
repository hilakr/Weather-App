using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using WeatherApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;

namespace WeatherApp.Tests

{
    [TestClass()]
    public class OpenMapDataServiceTests
    {
        [TestMethod()]
        public void GetWeatherDataTest()
        {
            Location location = new Location("bat yam");
            ///wDataTest holds the Weather Data info for Bat yam city in IL, We'll check if the data in wDataTest is equal to data from...
            WeatherData wDataTest = OpenMapDataService.Instance.GetWeatherData(location);
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
                //Check if the Data is Equal 
                foreach (var data in list)
                {
                    //Check City parameters
                    
                                Assert.AreEqual(wDataTest.City.Name, data.Name);
                                Assert.AreEqual(wDataTest.City.Country, data.Name);
                                Assert.AreEqual(wDataTest.City.Coords.Lat,double.Parse(data.CoordLat));
                                Assert.AreEqual(wDataTest.City.Coords.Lon,double.Parse(data.CoordLon));
                                Assert.AreEqual(wDataTest.City.Sun.Rise,DateTime.Parse(data.SunRise));
                                Assert.AreEqual(wDataTest.City.Sun.Set,DateTime.Parse(data.SunSet));

                                //Check Temp parameters
                                Assert.AreEqual(wDataTest.Temp.Value,double.Parse(data.TempValue));

                                //Check Wind parameters
                                Assert.AreEqual(wDataTest.Wind.Speed.Value,double.Parse(data.WindSpeedValue));
                                Assert.AreEqual(wDataTest.Wind.Speed.Name,data.WindSpeedText);

                                //Check Weather parameters
                                Assert.AreEqual(wDataTest.Weather.Icon,data.IconName);
                                Assert.AreEqual(wDataTest.Weather.Value,data.IconText);

                                //Check the Time
                                Assert.AreEqual(wDataTest.LastUpdateTime,DateTime.Parse(data.LastUpdate));
                    
                }
            }
            //Exceptions
            catch (AssertFailedException)
            {
                throw new WeatherDataServiceException("The Data isn't Equal , check the method \n");
            }
            catch (WebException)
            {
                throw new WeatherDataServiceException("Web connectivity Exception");
            }
            catch (Exception)
            {

                throw new WeatherDataServiceException("Parsing Exception");
            }
   
        }
    }
}
