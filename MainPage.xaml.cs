using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Weather_App.Models;

namespace Weather_App;

public partial class MainPage : ContentPage
{
    private double ScreenWidth // Fixing Searchbar 
    {
        get
        {
            return DeviceDisplay.Current.MainDisplayInfo.Width / DeviceDisplay.Current.MainDisplayInfo.Density;
        }
    }
    private string apiKey = "7979326c13a1461591381800232307";
    public MainPage()
	{
        InitializeComponent();
        citySearchBar.WidthRequest = ScreenWidth;
        citySearchBar.SearchButtonPressed += CitySearchBar_SearchButtonPressed;
    }

    private async void CitySearchBar_SearchButtonPressed(object sender, EventArgs e)
    {
        try
        {
            HttpClient client = new HttpClient();
            string json = await client.GetStringAsync($"https://api.weatherapi.com/v1/forecast.json&lang=fa?key={apiKey}&lang=ru&q={citySearchBar.Text}&days=3&aqi=yes&alerts=yes");
            WeatherAPI api = JsonConvert.DeserializeObject<WeatherAPI>(json);
            fetchWeatherList.Clear();
            
            Label currentWeatherTitleLabel = new Label();
            currentWeatherTitleLabel.Text = "Информация о текущей погоде";
            currentWeatherTitleLabel.HorizontalTextAlignment = TextAlignment.Center;
            fetchWeatherList.Add(currentWeatherTitleLabel);

            Label currentWeatherDescriptionLabel = new Label();
            currentWeatherDescriptionLabel.Text = $"Город: {api.location.Name}\n" +
                $"Регион: {api.location.Region}\n" +
                $"Страна: {api.location.Country}\n" +
                $"Широта на Google Maps: {api.location.Lat}°\n" +
                $"Долгота на Google Maps: {api.location.Lon}°\n" +
                $"Временная Зона ID: {api.location.TzId}\n" +
                $"Текущие время: {api.location.Localtime}\n" +
                $"Последние обновление: {api.current.LastUpdated}\n" +
                $"Температура в цельсиях: {api.current.TempC} °C\n" +
                $"Температура в фаренгейте: {api.current.TempF} °F\n" +
                $"Текущая погода: {api.current.Condition.Text}\n" +
                $"Ветер (км/ч): {api.current.WindKph}\n" +
                $"Ветер (миль/ч): {api.current.WindMph}\n" +
                $"Градус ветра: {api.current.WindDegree}°\n" +
                $"Направление ветра: {api.current.WindDir} ☴\n" +
                $"Атмосферное давление (Ртутного Столба): {api.current.PressureIn}\n" +
                $"Осадок (Ртутного Столба): {api.current.PrecipIn} ↓\n" +
                $"Влажность: {api.current.Humidity} 🜁\n" +
                $"Облачность: {api.current.Cloud} ☁️\n" +
                $"Чувствуется градусов в цельсиях как: {api.current.FeelslikeC} °C\n" +
                $"Чувствуется градусов в фаренгейтах как: {api.current.FeelslikeF} °F\n" +
                $"Порыв ветра (км/ч): {api.current.GustKph} 💨";
            fetchWeatherList.Add(currentWeatherDescriptionLabel);

            Label airQualityTitleLabel = new Label();
            airQualityTitleLabel.Text = "Качество ветра";
            airQualityTitleLabel.HorizontalTextAlignment = TextAlignment.Center;
            fetchWeatherList.Add(airQualityTitleLabel);

            Label airQualityLabel = new Label();
            string quality = string.Empty;

            quality = api.current.AirQuality.UsEpaIndex switch
            {
                1 => "Отличное",
                2 => "Умеренное",
                3 => "Опасный для чувствительной группы людей",
                4 => "Опасный для людей",
                5 => "Очень опасный для людей",
                6 => "Смертельный",
                _ => "Неизвестный"
            };

            airQualityLabel.Text = $"Окись углерода: {api.current.AirQuality.Co} CO\n" +
                $"Оксид азота: {api.current.AirQuality.No2} No2\n" +
                $"Озон: {api.current.AirQuality.O3} O3\n" +
                $"Оксид серы: {api.current.AirQuality.So2} So2\n" +
                $"Pm25: {api.current.AirQuality.Pm25}\n" +
                $"Pm10: {api.current.AirQuality.Pm10}\n" +
                $"Качество воздуха: {quality}";
            fetchWeatherList.Add(airQualityLabel);

            Label forecastTitle = new Label();
            forecastTitle.Text = "Прогноз погоды";
            forecastTitle.HorizontalTextAlignment = TextAlignment.Center;
            fetchWeatherList.Add(forecastTitle);

            for (int i = 0; i < api.forecast.Forecastday.Count; i++)
            {
                int currentIndexDate = i + 1; 
                string forecastDate = currentIndexDate switch
                {
                    1 => "Сегодня",
                    2 => "Завтра",
                    3 => "Послезавтра",
                    4 => "На 3 день",
                    5 => "На 4 день",
                    6 => "На 5 день",
                    7 => "На 6 день",
                    8 => "На 7 день",
                    9 => "На 8 день",
                    10 => "На 9 день",
                    _ => "Неизвестный прогноз",
                };
                Label dateLabel = new Label();
                dateLabel.Text = $"{forecastDate}: {api.forecast.Forecastday[i].Date}";
                fetchWeatherList.Add(dateLabel);

                Label allInfoLabel = new Label();
                allInfoLabel.Text = 
                    $"Макс. возмож. температура в цельсиях: {api.forecast.Forecastday[i].Day.MaxtempC} °C\n" +
                    $"Макс. возмож. температура в фаренгейтах: {api.forecast.Forecastday[i].Day.MaxtempF} °F\n" +
                    $"Мин. возмож. температура в цельсиях: {api.forecast.Forecastday[i].Day.MintempC} °C\n" +
                    $"Мин. возмож. температура в фаренгейтах: {api.forecast.Forecastday[i].Day.MintempF} °F\n" +
                    $"Сред. температура в цельсиях: {api.forecast.Forecastday[i].Day.AvgtempC} °C\n" +
                    $"Сред. температура в фаренгейтах: {api.forecast.Forecastday[i].Day.AvgtempF} °F\n" +
                    $"";
                fetchWeatherList.Add(allInfoLabel);
            }

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
}