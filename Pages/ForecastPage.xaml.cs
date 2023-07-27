using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching;
using Newtonsoft.Json;
using Weather_App.Models;

namespace Weather_App.Pages;

public partial class ForecastPage : ContentPage
{
    private Settings settings = new Settings();
    private VerticalStackLayout firstFrameLayout;
    public ForecastPage()
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
                testLayout.Clear();
                updateList.IsRefreshing = true;
            });
            HttpClient client = new HttpClient();
            settings = Utils.LoadSettingsData(settings);
            if (settings == null || string.IsNullOrEmpty(settings.City) || settings.LoadDays == 0)
            {
                // show error that we have no paramaters that we didn't defined in settings.
                return;
            }
            string json = await client.GetStringAsync($"https://api.weatherapi.com/v1/forecast.json?key={Utils.apiKey}&q={settings.City}&days={settings.LoadDays}&aqi=no&alerts=no");
            WeatherAPI api = JsonConvert.DeserializeObject<WeatherAPI>(json);

            for (int i = 0; i < api.forecast.Forecastday.Count; i++)
            {

                string uvQualityForecast = api.forecast.Forecastday[i].Day.Uv switch
                {
                    double iF when iF >= 0 && iF <= 2 => $"{iF}. Низкий, Меры защиты не нужны.",
                    double iF when iF >= 3 && iF <= 5 => $"{iF}. Умеренный, Необходима небольшая защита.",
                    double iF when iF >= 6 && iF <= 7 => $"{iF}. Высокий, Необходима защита используйте солнцезащитные средства.",
                    double iF when iF >= 8 && iF <= 10 => $"{iF}. Очень высокий, Необходима усиленная защита используйте солнцезащитные средства, находитесь меньше на улице.",
                    double iF when iF > 10 => $"{iF}. Чрезмерный, Нужна максимальная защита. Обязательно используйте сильные солнцезащитные средства, избегайте нахождения под солнечными лучами.",
                    _ => "Неизвестный"
                };
                await Dispatcher.DispatchAsync(() =>
                {
                    Label forecastBasicInfo = new Label();
                    forecastBasicInfo.Text = $"Макс. возмож. температура в цельсиях: {api.forecast.Forecastday[i].Day.MaxtempC} °C\n" +
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
                    firstFrameLayout = new VerticalStackLayout();
                    Frame mainFrame = new Frame();
                    testLayout.Add(mainFrame);
                    mainFrame.Content = firstFrameLayout;
                    firstFrameLayout.Add(forecastBasicInfo);
                });


                for (int j = 0; j < api.forecast.Forecastday[i].Hour.Count; j++)
                {
                    string uvQualityForecastHour = api.forecast.Forecastday[i].Hour[j].Uv switch
                    {
                        double iF when iF >= 0 && iF <= 2 => $"{iF}. Низкий, Меры защиты не нужны.",
                        double iF when iF >= 3 && iF <= 5 => $"{iF}. Умеренный, Необходима небольшая защита.",
                        double iF when iF >= 6 && iF <= 7 => $"{iF}. Высокий, Необходима защита используйте солнцезащитные средства.",
                        double iF when iF >= 8 && iF <= 10 => $"{iF}. Очень высокий, Необходима усиленная защита используйте солнцезащитные средства, находитесь меньше на улице.",
                        double iF when iF > 10 => $"{iF}. Чрезмерный, Нужна максимальная защита. Обязательно используйте сильные солнцезащитные средства, избегайте нахождения под солнечными лучами.",
                        _ => "Неизвестный"
                    };
                    await Dispatcher.DispatchAsync(() =>
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
                        $"Ультрафиолетовый индекс: {uvQualityForecastHour}";
                        Frame secondFrame = new Frame();
                        secondFrame.Content = forecastByHoursDescription;
                        firstFrameLayout.Add(secondFrame);
                    });
                }
            }
            await Dispatcher.DispatchAsync(() =>
            {
                updateList.IsRefreshing = false;
            });
        }
        catch (Exception ex)
        {
            await Dispatcher.DispatchAsync(() =>
            {
                updateList.IsRefreshing = false;
            });
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            string text = ex.Message;
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;

            var toast = Toast.Make(text, duration, fontSize);

            await toast.Show(cancellationTokenSource.Token);
        }
    }

    private async void updateList_Refreshing(object sender, EventArgs e)
    {
        try
        {
            LoadInfo();
        }
        catch (Exception ex)
        {
            await Dispatcher.DispatchAsync(() =>
            {
                updateList.IsRefreshing = false;
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