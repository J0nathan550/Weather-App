using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Newtonsoft.Json;
using Weather_App.Models;

namespace Weather_App.Pages;

public partial class InformationAboutCityPage : ContentPage
{
    private List<WeatherAPI> weatherAPIList = new List<WeatherAPI>();
    private Settings settings = new Settings();

    public InformationAboutCityPage()
	{
		InitializeComponent();
        LoadInfo();
    }

    private async void LoadInfo()
    {
        try
        {
            HttpClient client = new HttpClient();
            settings = Utils.LoadSettingsData(settings);
            if (settings == null || string.IsNullOrEmpty(settings.City) || settings.LoadDays == 0)
            {
                // show error that we have no paramaters that we didn't defined in settings.
                return;
            }
            string json = await client.GetStringAsync($"https://api.weatherapi.com/v1/current.json?key={Utils.apiKey}&q={settings.City}&aqi=no");
            WeatherAPI api = JsonConvert.DeserializeObject<WeatherAPI>(json);
            weatherAPIList.Add(new WeatherAPI() { location = api.location });
            Pin pin = new Pin()
            {
                Label = api.location.Name,
                Location = new Location()
                {
                    Latitude = api.location.Lat,
                    Longitude = api.location.Lon
                },
            };
            Location location = new Location(api.location.Lat, api.location.Lon);
            MapSpan mapSpan = new MapSpan(location, 0.01, 0.01);
            map.MoveToRegion(mapSpan);
            map.Pins.Add(pin);
            mainView.ItemsSource = weatherAPIList;
        }
        catch (Exception ex)
        {
            weatherAPIList.Clear();
            mainView.IsRefreshing = false;
            mainView.ItemsSource = null;
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            string text = ex.Message;
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;

            var toast = Toast.Make(text, duration, fontSize);

            await toast.Show(cancellationTokenSource.Token);
        }
    }
    private async void ListView_Refreshing(object sender, EventArgs e)
    {
        try
        {
            weatherAPIList.Clear();
            mainView.ItemsSource = null;
            HttpClient client = new HttpClient();
            settings = Utils.LoadSettingsData(settings);
            if (settings == null || string.IsNullOrEmpty(settings.City) || settings.LoadDays == 0)
            {
                // show error that we have no paramaters that we didn't defined in settings.
                return;
            }
            string json = await client.GetStringAsync($"https://api.weatherapi.com/v1/current.json?key={Utils.apiKey}&q={settings.City}&aqi=no");
            WeatherAPI api = JsonConvert.DeserializeObject<WeatherAPI>(json);
            weatherAPIList.Add(new WeatherAPI() { location = api.location });
            map.Pins.Clear();
            Pin pin = new Pin()
            {
                Label = api.location.Name,
                Location = new Location()
                {
                    Latitude = api.location.Lat,
                    Longitude = api.location.Lon
                },
            };
            Location location = new Location(api.location.Lat, api.location.Lon);
            MapSpan mapSpan = new MapSpan(location, 0.01, 0.01);
            map.MoveToRegion(mapSpan);
            map.Pins.Add(pin);
            mainView.ItemsSource = weatherAPIList;
            mainView.IsRefreshing = false;
        }
        catch (Exception ex)
        {
            weatherAPIList.Clear();
            mainView.IsRefreshing = false;
            mainView.ItemsSource = null;
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            string text = ex.Message;
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;

            var toast = Toast.Make(text, duration, fontSize);

            await toast.Show(cancellationTokenSource.Token);
        }
    }
}