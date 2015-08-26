using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace WeatherApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            //automatic action when createn wpf window
            InitializeComponent();
            WeatherDataGrid.Visibility = Visibility.Collapsed;
            LogoPanel.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// This method is responsible to run the weather data service accroding the icon that was clicked.
        /// </summary>
        private async void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            LogoPanel.Visibility = Visibility.Collapsed;
            WeatherDataGrid.Visibility = Visibility.Visible;
            
            //serType holds the click of the user 
            WeatherDataServiceFactory.ServiceType serType;

            if ((bool) OpenToggleButton.IsChecked)
                serType = WeatherDataServiceFactory.ServiceType.OPEN_WEATHER_MAP;
            else
                serType = WeatherDataServiceFactory.ServiceType.WORLD_WEATHER_ONLINE;

            try
            {
                //Var holds the relevant service instance
                var data = WeatherDataServiceFactory.getWeatherDataService(serType)
               .GetWeatherData(new Location(CityTextBox.Text));

                BuildViewByData(data);
            }
                //if a problem will accured exception will be thrown 
            catch (WeatherDataServiceException ex)
            {

                WeatherDataGrid.Visibility = Visibility.Collapsed;
                LogoPanel.Visibility = Visibility.Visible;
                this.ShowMessageAsync("Error!", ex.Message);
            }
            
        }

        //Create the gui for the user 
        private void BuildViewByData(WeatherData data)
        {
            CityText.Content = data.City.Name + ", " + data.City.Country;
            SunText.Content = data.City.Sun.Rise.ToShortTimeString() + "/" + data.City.Sun.Set.ToShortTimeString();
            TempText.Content = data.Temp.Value + "°C";
            WindText.Content = data.Wind.Speed.Value + "m/s " + data.Wind.Speed.Name;
            LastTimeText.Content = data.LastUpdateTime.ToString("F");
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(data.Weather.Icon);
            logo.EndInit();
            Icon.Source = logo;
            IconLabel.Content = data.Weather.Value;
        }

        //Check which web serivce was choosen by ther user (by the click)
        //Click on open weather map web service
        private void OpenToggleButton_OnClick(object sender, RoutedEventArgs e)
        {
            OpenToggleButton.IsChecked = true;
            WwOnlinwToggleButton.IsChecked = false;
        }
        //Click on open world weather online button 
        private void WwOnlinwToggleButton_OnClick(object sender, RoutedEventArgs e)
        {
            WwOnlinwToggleButton.IsChecked = true;
            OpenToggleButton.IsChecked = false;
        }
        //Click on back button 
        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            WeatherDataGrid.Visibility = Visibility.Collapsed;
            LogoPanel.Visibility = Visibility.Visible;
        }
    }
}
