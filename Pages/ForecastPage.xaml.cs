using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
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
                    double iF when iF >= 0 && iF <= 2 => $"{iF}. Низький, Заходи захисту не потрібні.",
                    double iF when iF >= 3 && iF <= 5 => $"{iF}. Помірний, потрібний невеликий захист.",
                    double iF when iF >= 6 && iF <= 7 => $"{iF}. Високий, Необхідний захист використовуйте сонцезахисні засоби.",
                    double iF when iF >= 8 && iF <= 10 => $"{iF}. Дуже високий, Необхідний посилений захист використовуйте сонцезахисні засоби, що менше на вулиці.",
                    double iF when iF > 10 => $"{iF}. Надмірний, Потрібен максимальний захист. Обов'язково використовуйте сильні сонцезахисні засоби, уникайте знаходження під сонячним промінням.",
                    _ => "Невідомий"
                };

                await Dispatcher.DispatchAsync(() =>
                {
                    Label forecastBasicInfo = new Label();
                    forecastBasicInfo.Text = $"Макс. можл. температура в цельсіях: {api.forecast.Forecastday[i].Day.MaxtempC} °C\n" +
                    $"Макс. можл. температура у фаренгейтах: {api.forecast.Forecastday[i].Day.MaxtempF} °F\n" +
                    $"Мін. можл. температура в цельсіях: {api.forecast.Forecastday[i].Day.MintempC} °C\n" +
                    $"Мін. можл. температура у фаренгейтах: {api.forecast.Forecastday[i].Day.MintempF} °F\n" +
                    $"Сред. температура в цельсіях: {api.forecast.Forecastday[i].Day.AvgtempC} °C\n" +
                    $"Сред. температура у фаренгейтах: {api.forecast.Forecastday[i].Day.AvgtempF} °F\n" +
                    $"Макс. вітер (км/год): {api.forecast.Forecastday[i].Day.MaxwindKph}\n" +
                    $"Макс. вітер у (миль/год): {api.forecast.Forecastday[i].Day.MaxwindMph}\n" +
                    $"Загальна кількість опадів в (мм): {api.forecast.Forecastday[i].Day.TotalprecipMm}\n" +
                    $"Загальна кількість опадів в (дюймах): {api.forecast.Forecastday[i].Day.TotalprecipIn}\n" +
                    $"Загальна видимість (км): {api.forecast.Forecastday[i].Day.AvgvisKm}\n" +
                    $"Загальна видимість у (милях): {api.forecast.Forecastday[i].Day.AvgvisMiles}\n" +
                    $"Середня вологість: {api.forecast.Forecastday[i].Day.Avghumidity}\n" +
                    $"Чи буде дощ: {(api.forecast.Forecastday[i].Day.DailyWillItRain == 1 ? "Так" : "Ні")}\n" +
                    $"Шанс на дощ: {api.forecast.Forecastday[i].Day.DailyChanceOfRain}%\n" +
                    $"Чи буде сніг: {(api.forecast.Forecastday[i].Day.DailyWillItSnow == 1 ? "Так" : "Ні")}\n" +
                    $"Шанс на сніг: {api.forecast.Forecastday[i].Day.DailyChanceOfSnow}%\n" +
                    $"Умовна погода: {api.forecast.Forecastday[i].Day.Condition.Text}\n" +
                    $"Ультрафіолетовий індекс: {uvQualityForecast}";
                    firstFrameLayout = new VerticalStackLayout();
                    firstFrameLayout.Spacing = 20;
                    firstFrameLayout.BackgroundColor = Colors.Transparent;
                    Frame mainFrame = new Frame();
                    mainFrame.BackgroundColor = Colors.Transparent;
                    testLayout.Add(mainFrame);
                    mainFrame.Content = firstFrameLayout;
                    firstFrameLayout.Add(forecastBasicInfo);
                });


                for (int j = 0; j < api.forecast.Forecastday[i].Hour.Count; j++)
                {
                    string uvQualityForecastHour = api.forecast.Forecastday[i].Hour[j].Uv switch
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
                        Label forecastByHoursDescription = new Label();
                        forecastByHoursDescription.Text = $"Время: {api.forecast.Forecastday[i].Hour[j].Time}\n" +
                        $"Температура у цельсіях: {api.forecast.Forecastday[i].Hour[j].TempC} °C\n" +
                        $"Температура у фаренгейте: {api.forecast.Forecastday[i].Hour[j].TempF} °F\n" +
                        $"Поточна погода: {api.forecast.Forecastday[i].Hour[j].Condition.Text}\n" +
                        $"Вітер (км/год): {api.forecast.Forecastday[i].Hour[j].WindKph}\n" +
                        $"Вітер (миль/год): {api.forecast.Forecastday[i].Hour[j].WindMph}\n" +
                        $"Градус вітру: {api.forecast.Forecastday[i].Hour[j].WindDegree}°\n" +
                        $"Напрямок вітру: {api.forecast.Forecastday[i].Hour[j].WindDir} ☴\n" +
                        $"Атмосферний тиск (Ртутного Стовпа): {api.forecast.Forecastday[i].Hour[j].PressureIn}\n" +
                        $"Осад (Ртутного Стовпа): {api.forecast.Forecastday[i].Hour[j].PrecipIn} ↓\n" +
                        $"Вологість: {api.forecast.Forecastday[i].Hour[j].Humidity} 🜁\n" +
                        $"Хмарність: {api.forecast.Forecastday[i].Hour[j].Cloud} ☁️\n" +
                        $"Відчувається градусів у цельсіях як: {api.forecast.Forecastday[i].Hour[j].FeelslikeC} °C\n" +
                        $"Відчувається градусів у фаренгейтах як: {api.forecast.Forecastday[i].Hour[j].FeelslikeF} °F\n" +
                        $"Температура точки роси газу в цельсіях: {api.forecast.Forecastday[i].Hour[j].DewpointC} °F\n" +
                        $"Температура точки роси газу у фаренгайте: {api.forecast.Forecastday[i].Hour[j].DewpointF} °F\n" +
                        $"Чи буде дощ: {(api.forecast.Forecastday[i].Day.DailyWillItRain == 1 ? "Так" : "Ні")}\n" +
                        $"Шанс на дощ: {api.forecast.Forecastday[i].Day.DailyChanceOfRain}%\n" +
                        $"Чи буде сніг: {(api.forecast.Forecastday[i].Day.DailyWillItSnow == 1 ? "Так" : "Ні")}\n" +
                        $"Шанс на сніг: {api.forecast.Forecastday[i].Day.DailyChanceOfSnow}%\n" +
                        $"Загальна видимість (км): {api.forecast.Forecastday[i].Hour[j].VisKm}\n" +
                        $"Загальна видимість у (милях): {api.forecast.Forecastday[i].Hour[j].VisMiles}\n" +
                        $"Порив вітру (км/год): {api.forecast.Forecastday[i].Hour[j].GustKph} 💨" +
                        $"Ультрафіолетовий індекс: {uvQualityForecastHour}";
                        Frame secondFrame = new Frame();
                        secondFrame.Content = forecastByHoursDescription;
                        secondFrame.BackgroundColor = Colors.Transparent;
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