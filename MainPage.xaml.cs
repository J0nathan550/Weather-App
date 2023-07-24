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

            Label locationName = new Label();
            locationName.Text = $"Город: {api.location.Name}";
            fetchWeatherList.Add(locationName);
            
            Label regionLabel = new Label();
            regionLabel.Text = $"Регион: {api.location.Region}";
            fetchWeatherList.Add(regionLabel);

            Label countryLabel = new Label();
            countryLabel.Text = $"Страна: {api.location.Country}";
            fetchWeatherList.Add(countryLabel);

            Label latitudeLabel = new Label();
            latitudeLabel.Text = $"Широта на Google Maps: {api.location.Lat}";
            fetchWeatherList.Add(latitudeLabel);

            Label longitudeLabel = new Label();
            longitudeLabel.Text = $"Долгота на Google Maps: {api.location.Lon}";
            fetchWeatherList.Add(longitudeLabel);

            Label timeZoneIDLabel = new Label();
            timeZoneIDLabel.Text = $"Временная Зона ID: {api.location.TzId}";
            fetchWeatherList.Add(timeZoneIDLabel);

            Label localTimeLabel = new Label();
            localTimeLabel.Text = $"Текущие время: {api.location.Localtime}";
            fetchWeatherList.Add(localTimeLabel);

            Label lastUpdatedLabel = new Label();
            lastUpdatedLabel.Text = $"Последние обновление: {api.current.LastUpdated}";
            fetchWeatherList.Add(lastUpdatedLabel);

            Label tempCelciusLabel = new Label();
            tempCelciusLabel.Text = $"Температура в цельсиях: {api.current.TempC}";
            fetchWeatherList.Add(tempCelciusLabel);

            Label tempFahrenheitLabel = new Label();
            tempFahrenheitLabel.Text = $"Температура в фаренгейте: {api.current.TempF}";
            fetchWeatherList.Add(tempFahrenheitLabel);

            Label conditionLabel = new Label();
            conditionLabel.Text = $"Текущая погода: {api.current.Condition.Text}";
            fetchWeatherList.Add(conditionLabel);

            Label windKphLabel = new Label();
            windKphLabel.Text = $"Ветер (км/ч): {api.current.WindKph}";
            fetchWeatherList.Add(windKphLabel);

            Label windMphLabel = new Label();
            windMphLabel.Text = $"Ветер (миль/ч): {api.current.WindMph}";
            fetchWeatherList.Add(windMphLabel);

            Label windDegreeLabel = new Label();
            windDegreeLabel.Text = $"Градус ветра: {api.current.WindDegree}";
            fetchWeatherList.Add(windDegreeLabel);

            Label windDirLabel = new Label();
            windDirLabel.Text = $"Направление ветра: {api.current.WindDir}";
            fetchWeatherList.Add(windDirLabel);

            Label pressureMbLabel = new Label();
            pressureMbLabel.Text = $"Атмосферное давление (Ртутного Столба): {api.current.PressureIn}";
            fetchWeatherList.Add(pressureMbLabel);

            Label precipInLabel = new Label();
            precipInLabel.Text = $"Осадок (Ртутного Столба): {api.current.PrecipIn}";
            fetchWeatherList.Add(precipInLabel);

            Label humidityLabel = new Label();
            humidityLabel.Text = $"Влажность: {api.current.Humidity}";
            fetchWeatherList.Add(humidityLabel);

            Label cloudLabel = new Label();
            cloudLabel.Text = $"Облачность: {api.current.Cloud}";
            fetchWeatherList.Add(cloudLabel);

            Label feelsLikeCLabel = new Label();
            feelsLikeCLabel.Text = $"Чувствуется градусов в цельсиях как: {api.current.FeelslikeC}";
            fetchWeatherList.Add(feelsLikeCLabel);

            Label feelsLikeFLabel = new Label();
            feelsLikeFLabel.Text = $"Чувствуется градусов в фаренгейтах как: {api.current.FeelslikeF}";
            fetchWeatherList.Add(feelsLikeFLabel);

            Label GustKph = new Label();
            GustKph.Text = $"Порыв ветра (км/ч): {api.current.GustKph}";
            fetchWeatherList.Add(GustKph);

            Label airQualityTitleLabel = new Label();
            airQualityTitleLabel.Text = "Качество ветра";
            airQualityTitleLabel.HorizontalTextAlignment = TextAlignment.Center;
            fetchWeatherList.Add(airQualityTitleLabel);

            Label airQualityLabel = new Label();
            string quality = string.Empty;
            switch (api.current.AirQuality.UsEpaIndex)
            {
                case 1:
                    quality = "Отличное";
                    break;
                case 2:
                    quality = "Умеренное";
                    break;
                case 3:
                    quality = "Опасный для чувствительной группы людей";
                    break;
                case 4:
                    quality = "Опасный для людей";
                    break;
                case 5:
                    quality = "Очень опасный для людей";
                    break;
                case 6:
                    quality = "Смертельный";
                    break;
            }
            airQualityLabel.Text = $"Окись углерода: {api.current.AirQuality.Co}\nОксид азота: {api.current.AirQuality.No2}\nОзон: {api.current.AirQuality.O3}\nОксид серы: {api.current.AirQuality.So2}\nPm25: {api.current.AirQuality.Pm25}\nPm10: {api.current.AirQuality.Pm10}\nКачество воздуха: {quality}";
            fetchWeatherList.Add(airQualityLabel);

            Label forecastTitle = new Label();
            forecastTitle.Text = "Прогноз погоды";
            forecastTitle.HorizontalTextAlignment = TextAlignment.Center;
            fetchWeatherList.Add(forecastTitle);

            for (int i = 0; i < api.forecast.Forecastday.Count; i++)
            {
                Label dateLabel = new Label();
                dateLabel.Text = $"Дата: {api.forecast.Forecastday[i].Date}";
                fetchWeatherList.Add(dateLabel);

                Label allInfoLabel = new Label();
                allInfoLabel.Text = $"Максимальные предпологаемые градусы в цельсиях: {api.forecast.Forecastday[i].Day.MaxtempC}\n" +
                    $"Максимальные предпологаемые градусы в фаренгейтах: {api.forecast.Forecastday[i].Day.MaxtempF}\n" +
                    $"Минимальные предполагаемые градусы в цельсиях: {api.forecast.Forecastday[i].Day.MintempC}\n" +
                    $"Минимальные предполагаемые градусы в фаренгейтах: {api.forecast.Forecastday[i].Day.MintempF}\n" +
                    $"Средняя температура в цельсиях: {api.forecast.Forecastday[i].Day.AvgtempC}\n" +
                    $"Средняя температура в фаренгейтах: {api.forecast.Forecastday[i].Day.AvgtempF}\n" +
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