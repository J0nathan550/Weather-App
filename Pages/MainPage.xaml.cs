using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Newtonsoft.Json;
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

            string uvQuality = api.current.Uv switch
            {
                double i when i >= 0 && i <= 2 => $"{i}. Низкий, Меры защиты не нужны.",
                double i when i >= 3 && i <= 5 => $"{i}. Умеренный, Необходима небольшая защита.",
                double i when i >= 6 && i <= 7 => $"{i}. Высокий, Необходима защита используйте солнцезащитные средства.",
                double i when i >= 8 && i <= 10 => $"{i}. Очень высокий, Необходима усиленная защита используйте солнцезащитные средства, находитесь меньше на улице.",
                double i when i > 10 => $"{i}. Чрезмерный, Нужна максимальная защита. Обязательно используйте сильные солнцезащитные средства, избегайте нахождения под солнечными лучами.",
                _ => "Неизвестный"
            };

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
                $"Общая видимость в (км): {api.current.VisKm}\n" +
                $"Общая видимость в (милях): {api.current.VisMiles}\n" +
                $"Ультрафиолетовый индекс: {uvQuality}\n" +
                $"Порыв ветра (км/ч): {api.current.GustKph} 💨";
            fetchWeatherList.Add(currentWeatherDescriptionLabel);

            Label airQualityTitleLabel = new Label();
            airQualityTitleLabel.Text = "Качество ветра";
            airQualityTitleLabel.HorizontalTextAlignment = TextAlignment.Center;
            fetchWeatherList.Add(airQualityTitleLabel);

            Label airQualityLabel = new Label();

            string quality = api.current.AirQuality.UsEpaIndex switch
            {
                1 => "Отличное",
                2 => "Умеренное",
                3 => "Опасный для чувствительной группы людей",
                4 => "Опасный для людей",
                5 => "Очень опасный для людей",
                6 => "Смертельный",
                _ => "Неизвестный"
            };


            airQualityLabel.Text = $"Окись углерода: {api.current.AirQuality.Co}\n" +
                $"Оксид азота: {api.current.AirQuality.No2}\n" +
                $"Озон: {api.current.AirQuality.O3}\n" +
                $"Оксид серы: {api.current.AirQuality.So2}\n" +
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
                dateLabel.HorizontalTextAlignment = TextAlignment.Center;  
                fetchWeatherList.Add(dateLabel);

                string uvQualityForecast = api.forecast.Forecastday[i].Day.Uv switch
                {
                    double iF when iF >= 0 && iF <= 2 => $"{iF}. Низкий, Меры защиты не нужны.",
                    double iF when iF >= 3 && iF <= 5 => $"{iF}. Умеренный, Необходима небольшая защита.",
                    double iF when iF >= 6 && iF <= 7 => $"{iF}. Высокий, Необходима защита используйте солнцезащитные средства.",
                    double iF when iF >= 8 && iF <= 10 => $"{iF}. Очень высокий, Необходима усиленная защита используйте солнцезащитные средства, находитесь меньше на улице.",
                    double iF when iF > 10 => $"{iF}. Чрезмерный, Нужна максимальная защита. Обязательно используйте сильные солнцезащитные средства, избегайте нахождения под солнечными лучами.",
                    _ => "Неизвестный"
                };

                Label forecastInfoDescription = new Label();
                forecastInfoDescription.Text =
                $"Макс. возмож. температура в цельсиях: {api.forecast.Forecastday[i].Day.MaxtempC} °C\n" +
                $"Макс. возмож. температура в фаренгейтах: {api.forecast.Forecastday[i].Day.MaxtempF} °F\n" +
                $"Мин. возмож. температура в цельсиях: {api.forecast.Forecastday[i].Day.MintempC} °C\n" +
                $"Мин. возмож. температура в фаренгейтах: {api.forecast.Forecastday[i].Day.MintempF} °F\n" +
                $"Сред. температура в цельсиях: {api.forecast.Forecastday[i].Day.AvgtempC} °C\n" +
                $"Сред. температура в фаренгейтах: {api.forecast.Forecastday[i].Day.AvgtempF} °F\n" +
                $"Макс. ветер в (км/ч): {api.forecast.Forecastday[i].Day.MaxwindKph}\n" +
                $"Макс. ветер в (миль/ч): {api.forecast.Forecastday[i].Day.MaxwindMph}\n" +
                $"Общее кол-во осадков в (мм): {api.forecast.Forecastday[i].Day.TotalprecipMm}\n" +
                $"Общее кол-во осадков в (дюймах):{api.forecast.Forecastday[i].Day.TotalprecipIn}\n" +
                $"Общая видимость в (км): {api.forecast.Forecastday[i].Day.AvgvisKm}\n" +
                $"Общая видимость в (милях): {api.forecast.Forecastday[i].Day.AvgvisMiles}\n" +
                $"Средняя влажность: {api.forecast.Forecastday[i].Day.Avghumidity}\n" +
                $"Будет ли дождь: {(api.forecast.Forecastday[i].Day.DailyWillItRain == 1 ? "Да" : "Нет")}\n" +
                $"Шанс на дождь: {api.forecast.Forecastday[i].Day.DailyChanceOfRain}%\n" +
                $"Будет ли снег: {(api.forecast.Forecastday[i].Day.DailyWillItSnow == 1 ? "Да" : "Нет")}\n" +
                $"Шанс на снег: {api.forecast.Forecastday[i].Day.DailyChanceOfSnow}%\n" +
                $"Условная погода: {api.forecast.Forecastday[i].Day.Condition.Text}\n" +
                $"Ультрафиолетовый индекс: {uvQualityForecast}";
                fetchWeatherList.Add(forecastInfoDescription);

                Label qualityOfWindTitle = new Label();
                qualityOfWindTitle.Text = "Качество ветра";
                qualityOfWindTitle.HorizontalTextAlignment = TextAlignment.Center;    
                fetchWeatherList.Add(qualityOfWindTitle);

                Label qualityOfWindDescription = new Label();
                qualityOfWindDescription.Text = $"Окись углерода: {api.forecast.Forecastday[i].Day.AirQuality.Co}\n" +
                $"Оксид азота: {api.forecast.Forecastday[i].Day.AirQuality.No2}\n" +
                $"Озон: {api.forecast.Forecastday[i].Day.AirQuality.O3}\n" +
                $"Оксид серы: {api.forecast.Forecastday[i].Day.AirQuality.So2}\n" +
                $"Pm25: {api.forecast.Forecastday[i].Day.AirQuality.Pm25}\n" +
                $"Pm10: {api.forecast.Forecastday[i].Day.AirQuality.Pm10}\n" +
                $"Качество воздуха: {quality}";
                fetchWeatherList.Add(qualityOfWindDescription);

                Label astronomicLabelTitle = new Label();
                astronomicLabelTitle.Text = "Астрономическая информация";
                astronomicLabelTitle.HorizontalTextAlignment = TextAlignment.Center;
                fetchWeatherList.Add(astronomicLabelTitle);
                    
                Label astronomicLabelDescription = new Label();
                astronomicLabelDescription.Text = $"Восход будет в: {api.forecast.Forecastday[i].Astro.Sunrise} 🌄\n" +
                $"Закат будет в: {api.forecast.Forecastday[i].Astro.Sunset} 🌇\n" +
                $"Восход луны будет в: {api.forecast.Forecastday[i].Astro.Moonrise} 🌑\n" +
                $"Заход луны будет в: {api.forecast.Forecastday[i].Astro.Moonset} 🌕\n" +
                $"Фаза луны: {api.forecast.Forecastday[i].Astro.MoonPhase} 🌓\n" +
                $"Освещение луны: {api.forecast.Forecastday[i].Astro.MoonIllumination} 🕯︎\n" +
                $"Восходит сейчас луна: {(api.forecast.Forecastday[i].Astro.IsMoonUp == 1 ? "Да" : "Нет")} 🌘\n" +
                $"Восходит сейчас солнце: {(api.forecast.Forecastday[i].Astro.IsSunUp == 1 ? "Да" : "Нет")} 🌅";
                fetchWeatherList.Add(astronomicLabelDescription);

                Label forecastByHoursTitle = new Label();
                forecastByHoursTitle.Text = "Прогноз погоды по часам";
                forecastByHoursTitle .HorizontalTextAlignment = TextAlignment.Center;   
                fetchWeatherList.Add(forecastByHoursTitle);

                for (int j = 0; j < api.forecast.Forecastday[i].Hour.Count; j++)
                {
                    Label forecastByHoursDescription = new Label();
                    forecastByHoursDescription.Text = $"Время: {api.forecast.Forecastday[i].Hour[j].Time}\n" +
                    $"Температура в цельсиях: {api.forecast.Forecastday[i].Hour[j].TempC} °C\n" +
                    $"Температура в фаренгейте: {api.forecast.Forecastday[i].Hour[j].TempF} °F\n" +
                    $"Текущая погода: {api.forecast.Forecastday[i].Hour[j].Condition.Text}\n" +
                    $"Ветер (км/ч): {api.forecast.Forecastday[i].Hour[j].WindKph}\n" +
                    $"Ветер (миль/ч): {api.forecast.Forecastday[i].Hour[j].WindMph}\n" +
                    $"Градус ветра: {api.forecast.Forecastday[i].Hour[j].WindDegree}°\n" +
                    $"Направление ветра: {api.forecast.Forecastday[i].Hour[j].WindDir} ☴\n" +
                    $"Атмосферное давление (Ртутного Столба): {api.forecast.Forecastday[i].Hour[j].PressureIn}\n" +
                    $"Осадок (Ртутного Столба): {api.forecast.Forecastday[i].Hour[j].PrecipIn} ↓\n" +
                    $"Влажность: {api.forecast.Forecastday[i].Hour[j].Humidity} 🜁\n" +
                    $"Облачность: {api.forecast.Forecastday[i].Hour[j].Cloud} ☁️\n" +
                    $"Чувствуется градусов в цельсиях как: {api.forecast.Forecastday[i].Hour[j].FeelslikeC} °C\n" +
                    $"Чувствуется градусов в фаренгейтах как: {api.forecast.Forecastday[i].Hour[j].FeelslikeF} °F\n" +
                    $"Температура точки росы газа в цельсиях: {api.forecast.Forecastday[i].Hour[j].DewpointC} °F\n" +
                    $"Температура точки росы газа в фаренгайте: {api.forecast.Forecastday[i].Hour[j].DewpointF} °F\n" +
                    $"Будет ли дождь: {(api.forecast.Forecastday[i].Day.DailyWillItRain == 1 ? "Да" : "Нет")}\n" +
                    $"Шанс на дождь: {api.forecast.Forecastday[i].Day.DailyChanceOfRain}%\n" +
                    $"Будет ли снег: {(api.forecast.Forecastday[i].Day.DailyWillItSnow == 1 ? "Да" : "Нет")}\n" +
                    $"Шанс на снег: {api.forecast.Forecastday[i].Day.DailyChanceOfSnow}%\n" +
                    $"Общая видимость в (км): {api.forecast.Forecastday[i].Hour[j].VisKm}\n" +
                    $"Общая видимость в (милях): {api.forecast.Forecastday[i].Hour[j].VisMiles}\n" +
                    $"Порыв ветра (км/ч): {api.forecast.Forecastday[i].Hour[j].GustKph} 💨" +
                    $"Ультрафиолетовый индекс: {uvQuality}";
                    Label labelQualityTitle = new Label();
                    labelQualityTitle.Text = "Качество ветра";
                    labelQualityTitle.HorizontalTextAlignment = TextAlignment.Center;
                    Label labelForecastAirQualityHours = new Label();
                    labelForecastAirQualityHours.Text = 
                    $"Окись углерода: {api.forecast.Forecastday[i].Hour[j].AirQuality.Co}\n" +
                    $"Оксид азота: {api.forecast.Forecastday[i].Hour[j].AirQuality.No2}\n" +
                    $"Озон: {api.forecast.Forecastday[i].Hour[j].AirQuality.O3}\n" +
                    $"Оксид серы: {api.forecast.Forecastday[i].Hour[j].AirQuality.So2}\n" +
                    $"Pm25: {api.forecast.Forecastday[i].Hour[j].AirQuality.Pm25}\n" +
                    $"Pm10: {api.forecast.Forecastday[i].Hour[j].AirQuality.Pm10}\n" +
                    $"Качество воздуха: {quality}";
                    fetchWeatherList.Add(forecastByHoursDescription);
                    fetchWeatherList.Add(labelQualityTitle);
                    fetchWeatherList.Add(labelForecastAirQualityHours);
                }
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