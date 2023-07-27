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
        mainView.ItemsSource = weatherAPIList;
    }
    private async void ListView_Refreshing(object sender, EventArgs e)
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
        mainView.ItemsSource = weatherAPIList;
        mainView.IsRefreshing = false;
    }
}