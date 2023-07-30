using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Newtonsoft.Json;
using Weather_App.Models;

namespace Weather_App.Pages;

public partial class CurrentWeatherPage : ContentPage
{
    private Settings settings = new Settings();
    private Frame mainFrame;
    private VerticalStackLayout mainStackLayout;

    public CurrentWeatherPage()
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

            string uvQuality = api.current.Uv switch
            {
                double iF when iF >= 0 && iF <= 2 => $"{iF}. Низький, Заходи захисту не потрібні.",
                double iF when iF >= 3 && iF <= 5 => $"{iF}. Помірний, потрібний невеликий захист.",
                double iF when iF >= 6 && iF <= 7 => $"{iF}. Високий, Необхідний захист використовуйте сонцезахисні засоби.",
                double iF when iF >= 8 && iF <= 10 => $"{iF}. Дуже високий, Необхідний посилений захист використовуйте сонцезахисні засоби, що менше на вулиці.",
                double iF when iF > 10 => $"{iF}. Надмірний, Потрібен максимальний захист. Обов'язково використовуйте сильні сонцезахисні засоби, уникайте знаходження під сонячним промінням.",
                _ => "Невідомий"
            };

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

                Label lastUpdatedWeatherLabel = new Label();
                lastUpdatedWeatherLabel.Text = $"Останнє оновлення погоди: {api.current.LastUpdated} 🕛";
                mainStackLayout.Add(lastUpdatedWeatherLabel);

                Label temperatureCelciusLabel = new Label();
                temperatureCelciusLabel.Text = $"Температура: {api.current.TempC}°C 🌡️";
                mainStackLayout.Add(temperatureCelciusLabel);

                Label temperatureFarenLabel = new Label();
                temperatureFarenLabel.Text = $"Температура: {api.current.TempF}°F 🌡️";
                mainStackLayout.Add(temperatureFarenLabel);

                Label currentWeatherLabel = new Label();
                currentWeatherLabel.Text = $"Поточна погода: {api.current.Condition.Text} 🌤️";
                mainStackLayout.Add(currentWeatherLabel);

                Label windKphLabel = new Label();
                windKphLabel.Text = $"Вітер: {api.current.WindKph} км/год 💨";
                mainStackLayout.Add(windKphLabel);

                Label windMphLabel = new Label();
                windMphLabel.Text = $"Вітер: {api.current.WindMph} миль/год 💨";
                mainStackLayout.Add(windMphLabel);

                Label windDegreeLabel = new Label();
                windDegreeLabel.Text = $"Градус вітру: {api.current.WindDegree}° 🧭";
                mainStackLayout.Add(windDegreeLabel);

                Label windDirectionLabel = new Label();
                windDirectionLabel.Text = $"Напрямок вітру: {api.current.WindDir} 🧭";
                mainStackLayout.Add(windDirectionLabel);

                Label atmospherePressureLabel = new Label();
                atmospherePressureLabel.Text = $"Атмосферний тиск (Ртутного стовпа): {api.current.PressureIn} 🫠";
                mainStackLayout.Add(atmospherePressureLabel);

                Label precipLabel = new Label();
                precipLabel.Text = $"Осад (Ртутного Стовпа): {api.current.PrecipIn} ☔";
                mainStackLayout.Add(precipLabel);

                Label humidityLabel = new Label();
                humidityLabel.Text = $"Вологість: {api.current.Humidity} 💦";
                mainStackLayout.Add(humidityLabel);

                Label cloudinessLabel = new Label();
                cloudinessLabel.Text = $"Хмарність: {api.current.Cloud} ☁️";
                mainStackLayout.Add(cloudinessLabel);

                Label feelsLikeCelciusLabel = new Label();
                feelsLikeCelciusLabel.Text = $"Відчувається градусів: {api.current.FeelslikeC}°C 🌡️";
                mainStackLayout.Add(feelsLikeCelciusLabel);

                Label feelsLikeFarenLabel = new Label();
                feelsLikeFarenLabel.Text = $"Відчувається градусів: {api.current.FeelslikeF}°F 🌡️";
                mainStackLayout.Add(feelsLikeFarenLabel);

                Label visibilityKmLabel = new Label();
                visibilityKmLabel.Text = $"Загальна видимість: {api.current.VisKm} км 🚇";
                mainStackLayout.Add(visibilityKmLabel);

                Label visibilityMilesLabel = new Label();
                visibilityMilesLabel.Text = $"Загальна видимість: {api.current.VisMiles} милях 🚇";
                mainStackLayout.Add(visibilityMilesLabel);

                Label uvLabel = new Label();
                uvLabel.Text = $"Ультрафиолетовый индекс: {uvQuality} ⛱️";
                mainStackLayout.Add(uvLabel);

                Label gustKphLabel = new Label();
                gustKphLabel.Text = $"Порив вітру (км/год): {api.current.GustKph} 💨";
                mainStackLayout.Add(gustKphLabel);

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
            string uvQuality = api.current.Uv switch
            {
                double i when i >= 0 && i <= 2 => $"{i}. Низкий, Меры защиты не нужны.",
                double i when i >= 3 && i <= 5 => $"{i}. Умеренный, Необходима небольшая защита.",
                double i when i >= 6 && i <= 7 => $"{i}. Высокий, Необходима защита используйте солнцезащитные средства.",
                double i when i >= 8 && i <= 10 => $"{i}. Очень высокий, Необходима усиленная защита используйте солнцезащитные средства, находитесь меньше на улице.",
                double i when i > 10 => $"{i}. Чрезмерный, Нужна максимальная защита. Обязательно используйте сильные солнцезащитные средства, избегайте нахождения под солнечными лучами.",
                _ => "Неизвестный"
            };

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

                Label lastUpdatedWeatherLabel = new Label();
                lastUpdatedWeatherLabel.Text = $"Останнє оновлення погоди: {api.current.LastUpdated} 🕛";
                mainStackLayout.Add(lastUpdatedWeatherLabel);

                Label temperatureCelciusLabel = new Label();
                temperatureCelciusLabel.Text = $"Температура: {api.current.TempC}°C 🌡️";
                mainStackLayout.Add(temperatureCelciusLabel);

                Label temperatureFarenLabel = new Label();
                temperatureFarenLabel.Text = $"Температура: {api.current.TempF}°F 🌡️";
                mainStackLayout.Add(temperatureFarenLabel);

                Label currentWeatherLabel = new Label();
                currentWeatherLabel.Text = $"Поточна погода: {api.current.Condition.Text} 🌤️";
                mainStackLayout.Add(currentWeatherLabel);

                Label windKphLabel = new Label();
                windKphLabel.Text = $"Вітер: {api.current.WindKph} км/год 💨";
                mainStackLayout.Add(windKphLabel);

                Label windMphLabel = new Label();
                windMphLabel.Text = $"Вітер: {api.current.WindMph} миль/год 💨";
                mainStackLayout.Add(windMphLabel);

                Label windDegreeLabel = new Label();
                windDegreeLabel.Text = $"Градус вітру: {api.current.WindDegree}° 🧭";
                mainStackLayout.Add(windDegreeLabel);

                Label windDirectionLabel = new Label();
                windDirectionLabel.Text = $"Напрямок вітру: {api.current.WindDir} 🧭";
                mainStackLayout.Add(windDirectionLabel);

                Label atmospherePressureLabel = new Label();
                atmospherePressureLabel.Text = $"Атмосферний тиск (Ртутного стовпа): {api.current.PressureIn} 🫠";
                mainStackLayout.Add(atmospherePressureLabel);

                Label precipLabel = new Label();
                precipLabel.Text = $"Осад (Ртутного Стовпа): {api.current.PrecipIn} ☔";
                mainStackLayout.Add(precipLabel);

                Label humidityLabel = new Label();
                humidityLabel.Text = $"Вологість: {api.current.Humidity} 💦";
                mainStackLayout.Add(humidityLabel);

                Label cloudinessLabel = new Label();
                cloudinessLabel.Text = $"Хмарність: {api.current.Cloud} ☁️";
                mainStackLayout.Add(cloudinessLabel);

                Label feelsLikeCelciusLabel = new Label();
                feelsLikeCelciusLabel.Text = $"Відчувається градусів: {api.current.FeelslikeC}°C 🌡️";
                mainStackLayout.Add(feelsLikeCelciusLabel);

                Label feelsLikeFarenLabel = new Label();
                feelsLikeFarenLabel.Text = $"Відчувається градусів: {api.current.FeelslikeF}°F 🌡️";
                mainStackLayout.Add(feelsLikeFarenLabel);

                Label visibilityKmLabel = new Label();
                visibilityKmLabel.Text = $"Загальна видимість: {api.current.VisKm} км 🚇";
                mainStackLayout.Add(visibilityKmLabel);

                Label visibilityMilesLabel = new Label();
                visibilityMilesLabel.Text = $"Загальна видимість: {api.current.VisMiles} милях 🚇";
                mainStackLayout.Add(visibilityMilesLabel);

                Label uvLabel = new Label();
                uvLabel.Text = $"Ультрафиолетовый индекс: {uvQuality} ⛱️";
                mainStackLayout.Add(uvLabel);

                Label gustKphLabel = new Label();
                gustKphLabel.Text = $"Порив вітру (км/год): {api.current.GustKph} 💨";
                mainStackLayout.Add(gustKphLabel);

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