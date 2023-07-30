using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Newtonsoft.Json;
using Weather_App.Models;

namespace Weather_App.Pages;

public partial class AstronomicPage : ContentPage
{
    private Settings settings = new Settings();
    private Frame mainFrame;
    private VerticalStackLayout mainStackLayout;
    public AstronomicPage()
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
            string json = await client.GetStringAsync($"https://api.weatherapi.com/v1/forecast.json?key={Utils.apiKey}&q={settings.City}&days={settings.LoadDays}&aqi=no&alerts=no");
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

                Label sunRiseLabel = new Label();
                sunRiseLabel.Text = $"Схід буде в: {api.forecast.Forecastday[0].Astro.Sunrise} 🌄";
                mainStackLayout.Add(sunRiseLabel);

                Label moonRiseLabel = new Label();
                moonRiseLabel.Text = $"Схід місяця буде в: {api.forecast.Forecastday[0].Astro.Moonrise} 🌑";
                mainStackLayout.Add(moonRiseLabel);

                Label moonSetLabel = new Label();
                moonSetLabel.Text = $"Захід місяця буде в: {api.forecast.Forecastday[0].Astro.Moonset} 🌕";
                mainStackLayout.Add(moonSetLabel);

                Label moonPhaseLabel = new Label();
                moonPhaseLabel.Text = $"Фаза місяця: {api.forecast.Forecastday[0].Astro.MoonPhase} 🌓";
                mainStackLayout.Add(moonPhaseLabel);

                Label MoonIlluminationLabel = new Label();
                MoonIlluminationLabel.Text = $"Освітлення місяця: {api.forecast.Forecastday[0].Astro.MoonIllumination} 🕯︎";
                mainStackLayout.Add(MoonIlluminationLabel);

                Label isMoonUpLabel = new Label();
                isMoonUpLabel.Text = $"Сходить зараз місяць: {(api.forecast.Forecastday[0].Astro.IsMoonUp == 1 ? "Так" : "Ні")} 🌘";
                mainStackLayout.Add(isMoonUpLabel);

                Label isSunGoingUpLabel = new Label();
                isSunGoingUpLabel.Text = $"Сходить зараз сонце: {(api.forecast.Forecastday[0].Astro.IsSunUp == 1 ? "Так" : "Ні")} 🌅";
                mainStackLayout.Add(isSunGoingUpLabel);

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
            string json = await client.GetStringAsync($"https://api.weatherapi.com/v1/forecast.json?key={Utils.apiKey}&q={settings.City}&days=1&aqi=no&alerts=no");
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

                Label sunRiseLabel = new Label();
                sunRiseLabel.Text = $"Схід буде в: {api.forecast.Forecastday[0].Astro.Sunrise} 🌄";
                mainStackLayout.Add(sunRiseLabel);

                Label moonRiseLabel = new Label();
                moonRiseLabel.Text = $"Схід місяця буде в: {api.forecast.Forecastday[0].Astro.Moonrise} 🌑";
                mainStackLayout.Add(moonRiseLabel);

                Label moonSetLabel = new Label();
                moonSetLabel.Text = $"Захід місяця буде в: {api.forecast.Forecastday[0].Astro.Moonset} 🌕";
                mainStackLayout.Add(moonSetLabel);

                Label moonPhaseLabel = new Label();
                moonPhaseLabel.Text = $"Фаза місяця: {api.forecast.Forecastday[0].Astro.MoonPhase} 🌓";
                mainStackLayout.Add(moonPhaseLabel);

                Label MoonIlluminationLabel = new Label();
                MoonIlluminationLabel.Text = $"Освітлення місяця: {api.forecast.Forecastday[0].Astro.MoonIllumination} 🕯︎";
                mainStackLayout.Add(MoonIlluminationLabel);

                Label isMoonUpLabel = new Label();
                isMoonUpLabel.Text = $"Сходить зараз місяць: {(api.forecast.Forecastday[0].Astro.IsMoonUp == 1 ? "Так" : "Ні")} 🌘";
                mainStackLayout.Add(isMoonUpLabel);

                Label isSunGoingUpLabel = new Label();
                isSunGoingUpLabel.Text = $"Сходить зараз сонце: {(api.forecast.Forecastday[0].Astro.IsSunUp == 1 ? "Так" : "Ні")} 🌅";
                mainStackLayout.Add(isSunGoingUpLabel);

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