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
            InitializeComponent();
            WeatherDataGrid.Visibility = Visibility.Collapsed;
            LogoPanel.Visibility = Visibility.Visible;
        }

        private async void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            LogoPanel.Visibility = Visibility.Collapsed;
            WeatherDataGrid.Visibility = Visibility.Visible;
            WeatherDataServiceFactory.ServiceType serType;
            if ((bool) OpenToggleButton.IsChecked)
                serType = WeatherDataServiceFactory.ServiceType.OPEN_WEATHER_MAP;
            else
                serType = WeatherDataServiceFactory.ServiceType.WORLD_WEATHER_ONLINE;

            try
            {
                var data = WeatherDataServiceFactory.getWeatherDataService(serType)
               .GetWeatherData(new Location(CityTextBox.Text));

                BuildViewByData(data);
            }
            catch (WeatherDataServiceException ex)
            {

                WeatherDataGrid.Visibility = Visibility.Collapsed;
                LogoPanel.Visibility = Visibility.Visible;
                this.ShowMessageAsync("Error!", ex.Message);
            }
            
        }


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

        private void OpenToggleButton_OnClick(object sender, RoutedEventArgs e)
        {
            OpenToggleButton.IsChecked = true;
            WwOnlinwToggleButton.IsChecked = false;
        }

        private void WwOnlinwToggleButton_OnClick(object sender, RoutedEventArgs e)
        {
            WwOnlinwToggleButton.IsChecked = true;
            OpenToggleButton.IsChecked = false;
        }

        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            WeatherDataGrid.Visibility = Visibility.Collapsed;
            LogoPanel.Visibility = Visibility.Visible;
        }
    }
}
