using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Newtonsoft.Json;
using Weather_App.Models;

namespace Weather_App.Pages;

public partial class AirQualityPage : ContentPage
{
    private List<WeatherAPI> weatherAPIList = new List<WeatherAPI>();
    private Settings settings = new Settings();

    public AirQualityPage()
	{
		InitializeComponent();
        LoadInfo();
    }

    private async void LoadInfo()
    {
        try
        {
            await Dispatcher.DispatchAsync(() =>
            {
                mainView.IsRefreshing = true;
            });
            HttpClient client = new HttpClient();
            settings = Utils.LoadSettingsData(settings);
            if (settings == null || string.IsNullOrEmpty(settings.City) || settings.LoadDays == 0)
            {
                // show error that we have no paramaters that we didn't defined in settings.
                return;
            }
            string json = await client.GetStringAsync($"https://api.weatherapi.com/v1/current.json?key={Utils.apiKey}&q={settings.City}&aqi=yes");
            WeatherAPI api = JsonConvert.DeserializeObject<WeatherAPI>(json);
            weatherAPIList.Add(new WeatherAPI() { current = api.current });
            await Dispatcher.DispatchAsync(() =>
            {
                mainView.ItemsSource = weatherAPIList;
                mainView.IsRefreshing = false;
            });
        }
        catch (Exception ex)
        {
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
            await Dispatcher.DispatchAsync(() =>
            {
                mainView.ItemsSource = null;
            });
            HttpClient client = new HttpClient();
            settings = Utils.LoadSettingsData(settings);
            if (settings == null || string.IsNullOrEmpty(settings.City) || settings.LoadDays == 0)
            {
                // show error that we have no paramaters that we didn't defined in settings.
                return;
            }
            string json = await client.GetStringAsync($"https://api.weatherapi.com/v1/current.json?key={Utils.apiKey}&q={settings.City}&aqi=yes");
            WeatherAPI api = JsonConvert.DeserializeObject<WeatherAPI>(json);
            weatherAPIList.Add(new WeatherAPI() { current = api.current });
            await Dispatcher.DispatchAsync(() =>
            {
                mainView.ItemsSource = weatherAPIList;
                mainView.IsRefreshing = false;
            });
        }
        catch (Exception ex)
        {
            weatherAPIList.Clear();
            await Dispatcher.DispatchAsync(() =>
            {
                mainView.IsRefreshing = false;
                mainView.ItemsSource = null;
            });
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            string text = ex.Message;
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;

            var toast = Toast.Make(text, duration, fontSize);

            await toast.Show(cancellationTokenSource.Token);
        }
    }

}