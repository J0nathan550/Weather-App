using Newtonsoft.Json;
using Weather_App.Models;

namespace Weather_App.Pages;

public partial class TestFirstWithoutListViewPage : ContentPage
{
    private Settings settings = new Settings();
	public TestFirstWithoutListViewPage()
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
        string json = await client.GetStringAsync($"https://api.weatherapi.com/v1/forecast.json&lang=fa?key={Utils.apiKey}&lang=ru&q={settings.City}&days={settings.LoadDays}&aqi=yes&alerts=yes");
        WeatherAPI api = JsonConvert.DeserializeObject<WeatherAPI>(json);
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
            fetchWeatherList.Children.Add(dateLabel);

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
            fetchWeatherList.Children.Add(forecastInfoDescription);

            Label qualityOfWindTitle = new Label();
            qualityOfWindTitle.Text = "Качество ветра";
            qualityOfWindTitle.HorizontalTextAlignment = TextAlignment.Center;
            fetchWeatherList.Children.Add(qualityOfWindTitle);

            string qualityForecastDay = api.forecast.Forecastday[i].Day.AirQuality.UsEpaIndex switch
            {
                1 => "Отличное",
                2 => "Умеренное",
                3 => "Опасный для чувствительной группы людей",
                4 => "Опасный для людей",
                5 => "Очень опасный для людей",
                6 => "Смертельный",
                _ => "Неизвестный"
            };

            Label qualityOfWindDescription = new Label();
            qualityOfWindDescription.Text = $"Окись углерода: {api.forecast.Forecastday[i].Day.AirQuality.Co}\n" +
            $"Оксид азота: {api.forecast.Forecastday[i].Day.AirQuality.No2}\n" +
            $"Озон: {api.forecast.Forecastday[i].Day.AirQuality.O3}\n" +
            $"Оксид серы: {api.forecast.Forecastday[i].Day.AirQuality.So2}\n" +
            $"Pm25: {api.forecast.Forecastday[i].Day.AirQuality.Pm25}\n" +
            $"Pm10: {api.forecast.Forecastday[i].Day.AirQuality.Pm10}\n" +
            $"Качество воздуха: {qualityForecastDay}";
            fetchWeatherList.Children.Add(qualityOfWindDescription);

            Label astronomicLabelTitle = new Label();
            astronomicLabelTitle.Text = "Астрономическая информация";
            astronomicLabelTitle.HorizontalTextAlignment = TextAlignment.Center;
            fetchWeatherList.Children.Add(astronomicLabelTitle);

            Label astronomicLabelDescription = new Label();
            astronomicLabelDescription.Text = $"Восход будет в: {api.forecast.Forecastday[i].Astro.Sunrise} 🌄\n" +
            $"Закат будет в: {api.forecast.Forecastday[i].Astro.Sunset} 🌇\n" +
            $"Восход луны будет в: {api.forecast.Forecastday[i].Astro.Moonrise} 🌑\n" +
            $"Заход луны будет в: {api.forecast.Forecastday[i].Astro.Moonset} 🌕\n" +
            $"Фаза луны: {api.forecast.Forecastday[i].Astro.MoonPhase} 🌓\n" +
            $"Освещение луны: {api.forecast.Forecastday[i].Astro.MoonIllumination} 🕯︎\n" +
            $"Восходит сейчас луна: {(api.forecast.Forecastday[i].Astro.IsMoonUp == 1 ? "Да" : "Нет")} 🌘\n" +
            $"Восходит сейчас солнце: {(api.forecast.Forecastday[i].Astro.IsSunUp == 1 ? "Да" : "Нет")} 🌅";
            fetchWeatherList.Children.Add(astronomicLabelDescription);

            Label forecastByHoursTitle = new Label();
            forecastByHoursTitle.Text = "Прогноз погоды по часам";
            forecastByHoursTitle.HorizontalTextAlignment = TextAlignment.Center;
            fetchWeatherList.Children.Add(forecastByHoursTitle);

            for (int j = 0; j < api.forecast.Forecastday[i].Hour.Count; j++)
            {
                string qualityForecastHour = api.forecast.Forecastday[i].Hour[j].AirQuality.UsEpaIndex switch
                {
                    1 => "Отличное",
                    2 => "Умеренное",
                    3 => "Опасный для чувствительной группы людей",
                    4 => "Опасный для людей",
                    5 => "Очень опасный для людей",
                    6 => "Смертельный",
                    _ => "Неизвестный"
                };

                string uvQualityForecastHour = api.forecast.Forecastday[i].Hour[j].Uv switch
                {
                    double iF when i >= 0 && i <= 2 => $"{iF}. Низкий, Меры защиты не нужны.",
                    double iF when i >= 3 && i <= 5 => $"{iF}. Умеренный, Необходима небольшая защита.",
                    double iF when i >= 6 && i <= 7 => $"{iF}. Высокий, Необходима защита используйте солнцезащитные средства.",
                    double iF when i >= 8 && i <= 10 => $"{iF}. Очень высокий, Необходима усиленная защита используйте солнцезащитные средства, находитесь меньше на улице.",
                    double iF when i > 10 => $"{iF}. Чрезмерный, Нужна максимальная защита. Обязательно используйте сильные солнцезащитные средства, избегайте нахождения под солнечными лучами.",
                    _ => "Неизвестный"
                };

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
                $"Ультрафиолетовый индекс: {uvQualityForecastHour}";
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
                $"Качество воздуха: {qualityForecastHour}";
                fetchWeatherList.Children.Add(forecastByHoursDescription);
                fetchWeatherList.Children.Add(labelQualityTitle);
                fetchWeatherList.Children.Add(labelForecastAirQualityHours);
            }
        }
    }
    private void Button_Clicked(object sender, EventArgs e)
    {
        fetchWeatherList.Clear();
        LoadInfo();
    }
}