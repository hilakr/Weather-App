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
        /// <summary>
        /// The test class will check if the method getWeatherData get the correct data from the web service and parse it correctly. 
        /// How? by compare between object that return from the method itself and the data that was received from the web service .
        /// </summary>
        public void GetWeatherDataTest()
        {
            //location is a city that I choose : "bay yam" to compare the data . 
            Location location = new Location("bat yam");
            ///wDataTest holds the Weather Data info for Bat yam city in IL, it's the object that return from the method.
            WeatherData wDataTest = OpenMapDataService.Instance.GetWeatherData(location);  
            
            //request the data from web service. 
            XDocument xdoc;
          
            //api var holds the http response.
            var api = string.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&mode=xml", location.Name);

            //Parse the xdoc and get the relevant info.
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
                                Assert.AreEqual(wDataTest.City.Country, data.Country);
                                Assert.AreEqual(wDataTest.City.Coords.Lat,double.Parse(data.CoordLat));
                                Assert.AreEqual(wDataTest.City.Coords.Lon,double.Parse(data.CoordLon));
                                Assert.AreEqual(wDataTest.City.Sun.Rise,DateTime.Parse(data.SunRise));
                                Assert.AreEqual(wDataTest.City.Sun.Set,DateTime.Parse(data.SunSet));

                                //Check Temp parameters
                                Console.WriteLine(double.Parse(data.TempValue)- 272.15);
                                Console.WriteLine(wDataTest.Temp.Value);

                                Assert.AreEqual(wDataTest.Temp.Value, double.Parse(data.TempValue) - 272.15);

                                //Check Wind parameters
                                Assert.AreEqual(wDataTest.Wind.Speed.Value,double.Parse(data.WindSpeedValue));
                                Assert.AreEqual(wDataTest.Wind.Speed.Name,data.WindSpeedText);

                                //Check the Time
                                Assert.AreEqual(wDataTest.LastUpdateTime,DateTime.Parse(data.LastUpdate));

                                Assert.Inconclusive("Verify the correctness of this test method.");
                    
                }
            }
            //Exceptions
            catch (AssertFailedException)
            {
                throw new WeatherDataServiceException("The Comparsion between the objects is failed");
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
