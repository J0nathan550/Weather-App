using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Newtonsoft.Json;
using Weather_App.Models;

namespace Weather_App.Pages;

public partial class InformationAboutCityPage : ContentPage
{
    private Settings settings = new Settings();
    private VerticalStackLayout mainStackLayout;
    private Frame mainFrame; 

    public InformationAboutCityPage()
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
            string json = await client.GetStringAsync($"https://api.weatherapi.com/v1/current.json?key={Utils.apiKey}&q={settings.City}&aqi=no");
            WeatherAPI api = JsonConvert.DeserializeObject<WeatherAPI>(json);
            
            await Dispatcher.DispatchAsync(() =>
            {
                mainFrame = new Frame();
                mainFrame.BorderColor = Colors.Black;
                mainFrame.BackgroundColor = Colors.Transparent;

                mainLayout.Add(mainFrame);
                mainStackLayout = new VerticalStackLayout();
                mainStackLayout.Spacing = 15;
                mainStackLayout.BackgroundColor = Colors.Transparent;

                mainFrame.Content = mainStackLayout;
            
                Label cityLabel = new Label();
                cityLabel.Text = $"Місто: {api.location.Name} 🏙️";
                mainStackLayout.Children.Add(cityLabel);

                Label regionLabel = new Label();
                regionLabel.Text = $"Регіон: {api.location.Region} 🌏";
                mainStackLayout.Children.Add(regionLabel);

                Label countryLabel = new Label();
                countryLabel.Text = $"Країна: {api.location.Country} 🚩";
                mainStackLayout.Children.Add(countryLabel);

                Label timeZoneLabel = new Label();
                timeZoneLabel.Text = $"Часова Зона ID: {api.location.TzId}";
                mainStackLayout.Children.Add(timeZoneLabel);

                Label currentLocalTimeLabel = new Label();
                currentLocalTimeLabel.Text = $"Поточний час: {api.location.Localtime} 🕐";
                mainStackLayout.Children.Add(currentLocalTimeLabel);

                mainView.IsRefreshing = false;
            });
        }
        catch (Exception ex)
        {
            await Dispatcher.DispatchAsync(() =>
            {
                mainStackLayout.Clear();
                mainFrame.Content = null;
                mainView.IsRefreshing = false;
                mainLayout.Clear();
            });
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
            await Dispatcher.DispatchAsync(() =>
            {
                mainStackLayout.Clear();
                mainFrame.Content = null;
                mainLayout.Clear();
            });
            HttpClient client = new HttpClient();
            settings = Utils.LoadSettingsData(settings);
            string json = await client.GetStringAsync($"https://api.weatherapi.com/v1/current.json?key={Utils.apiKey}&q={settings.City}&aqi=no");
            WeatherAPI api = JsonConvert.DeserializeObject<WeatherAPI>(json);
            await Dispatcher.DispatchAsync(() =>
            {
                mainFrame = new Frame();
                mainFrame.BorderColor = Colors.Black;
                mainFrame.BackgroundColor = Colors.Transparent;

                mainLayout.Add(mainFrame);
                mainStackLayout = new VerticalStackLayout();
                mainStackLayout.Spacing = 15;
                mainStackLayout.BackgroundColor = Colors.Transparent;

                mainFrame.Content = mainStackLayout;

                Label cityLabel = new Label();
                cityLabel.Text = $"Місто: {api.location.Name} 🏙️";
                mainStackLayout.Children.Add(cityLabel);

                Label regionLabel = new Label();
                regionLabel.Text = $"Регіон: {api.location.Region} 🌏";
                mainStackLayout.Children.Add(regionLabel);

                Label countryLabel = new Label();
                countryLabel.Text = $"Країна: {api.location.Country} 🚩";
                mainStackLayout.Children.Add(countryLabel);

                Label timeZoneLabel = new Label();
                timeZoneLabel.Text = $"Часова Зона ID: {api.location.TzId}";
                mainStackLayout.Children.Add(timeZoneLabel);

                Label currentLocalTimeLabel = new Label();
                currentLocalTimeLabel.Text = $"Поточний час: {api.location.Localtime} 🕐";
                mainStackLayout.Children.Add(currentLocalTimeLabel);

                mainView.IsRefreshing = false;
            });
        }
        catch (Exception ex)
        {
            await Dispatcher.DispatchAsync(() =>
            {
                mainStackLayout.Clear();
                mainFrame.Content = null;
                mainView.IsRefreshing = false;
                mainLayout.Clear();
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