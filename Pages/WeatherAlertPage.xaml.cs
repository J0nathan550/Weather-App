using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Newtonsoft.Json;
using Weather_App.Models;

namespace Weather_App.Pages;

public partial class WeatherAlertPage : ContentPage
{
    private Settings settings = new Settings();
    private VerticalStackLayout mainStackLayout;
    private Frame mainFrame;

    public WeatherAlertPage()
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
            string json = await client.GetStringAsync($"https://api.weatherapi.com/v1/forecast.json?key={Utils.apiKey}&q={settings.City}&days=0&aqi=no&alerts=yes");
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

                Label headLineLabel = new Label();
                if (api.alerts.Alert.Count == 0)
                {
                    headLineLabel.Text = $"Зараз немає ніяких погодних попереджень.";
                    headLineLabel.HorizontalTextAlignment = TextAlignment.Center;
                    mainStackLayout.Children.Add(headLineLabel);
                    mainView.IsRefreshing = false;
                    return;
                }

                headLineLabel = new Label();
                headLineLabel.Text = $"Заголовок: {api.alerts.Alert[0].Headline}";
                mainStackLayout.Children.Add(headLineLabel);

                Label msgLabel = new Label();
                msgLabel.Text = $"Повідомлення: {api.alerts.Alert[0].Msgtype}";
                mainStackLayout.Children.Add(msgLabel);

                Label severityLabel = new Label();
                severityLabel.Text = $"Суворість: {api.alerts.Alert[0].Severity}";
                mainStackLayout.Children.Add(severityLabel);

                Label urgencyLabel = new Label();
                urgencyLabel.Text = $"Терміновість: {api.alerts.Alert[0].Urgency}";
                mainStackLayout.Children.Add(urgencyLabel);

                Label areasLabel = new Label();
                areasLabel.Text = $"Області: {api.alerts.Alert[0].Areas}";
                mainStackLayout.Children.Add(areasLabel);

                Label categoryLabel = new Label();
                categoryLabel.Text = $"Категорія: {api.alerts.Alert[0].Category}";
                mainStackLayout.Children.Add(categoryLabel);

                Label certaintyLabel = new Label();
                certaintyLabel.Text = $"Впевненість: {api.alerts.Alert[0].Certainty}";
                mainStackLayout.Children.Add(certaintyLabel);

                Label eventLabel = new Label();
                eventLabel.Text = $"Події: {api.alerts.Alert[0].Event}";
                mainStackLayout.Children.Add(eventLabel);

                Label noteLabel = new Label();
                noteLabel.Text = $"Примітка: {api.alerts.Alert[0].Note}";
                mainStackLayout.Children.Add(noteLabel);

                Label effectiveLabel = new Label();
                effectiveLabel.Text = $"Ефективність: {api.alerts.Alert[0].Effective}";
                mainStackLayout.Children.Add(effectiveLabel);

                Label expiresLabel = new Label();
                expiresLabel.Text = $"Закінчується: {api.alerts.Alert[0].Expires}";
                mainStackLayout.Children.Add(expiresLabel);

                Label descLabel = new Label();
                descLabel.Text = $"Опис: {api.alerts.Alert[0].Desc}";
                mainStackLayout.Children.Add(descLabel);

                Label instuctionLabel = new Label();
                instuctionLabel.Text = $"Інструкція: {api.alerts.Alert[0].Instruction}";
                mainStackLayout.Children.Add(instuctionLabel);

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
            string json = await client.GetStringAsync($"https://api.weatherapi.com/v1/forecast.json?key={Utils.apiKey}&q={settings.City}&days=0&aqi=no&alerts=yes");
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

                Label headLineLabel = new Label();
                if (api.alerts.Alert.Count == 0)
                {
                    headLineLabel.Text = $"Зараз немає ніяких погодних попереджень.";
                    headLineLabel.HorizontalTextAlignment = TextAlignment.Center;
                    mainStackLayout.Children.Add(headLineLabel);
                    mainView.IsRefreshing = false;
                    return;
                }

                headLineLabel = new Label();
                headLineLabel.Text = $"Заголовок: {api.alerts.Alert[0].Headline}";
                mainStackLayout.Children.Add(headLineLabel);

                Label msgLabel = new Label();
                msgLabel.Text = $"Повідомлення: {api.alerts.Alert[0].Msgtype}";
                mainStackLayout.Children.Add(msgLabel);

                Label severityLabel = new Label();
                severityLabel.Text = $"Суворість: {api.alerts.Alert[0].Severity}";
                mainStackLayout.Children.Add(severityLabel);

                Label urgencyLabel = new Label();
                urgencyLabel.Text = $"Терміновість: {api.alerts.Alert[0].Urgency}";
                mainStackLayout.Children.Add(urgencyLabel);

                Label areasLabel = new Label();
                areasLabel.Text = $"Області: {api.alerts.Alert[0].Areas}";
                mainStackLayout.Children.Add(areasLabel);

                Label categoryLabel = new Label();
                categoryLabel.Text = $"Категорія: {api.alerts.Alert[0].Category}";
                mainStackLayout.Children.Add(categoryLabel);

                Label certaintyLabel = new Label();
                certaintyLabel.Text = $"Впевненість: {api.alerts.Alert[0].Certainty}";
                mainStackLayout.Children.Add(certaintyLabel);

                Label eventLabel = new Label();
                eventLabel.Text = $"Події: {api.alerts.Alert[0].Event}";
                mainStackLayout.Children.Add(eventLabel);

                Label noteLabel = new Label();
                noteLabel.Text = $"Примітка: {api.alerts.Alert[0].Note}";
                mainStackLayout.Children.Add(noteLabel);

                Label effectiveLabel = new Label();
                effectiveLabel.Text = $"Ефективність: {api.alerts.Alert[0].Effective}";
                mainStackLayout.Children.Add(effectiveLabel);

                Label expiresLabel = new Label();
                expiresLabel.Text = $"Закінчується: {api.alerts.Alert[0].Expires}";
                mainStackLayout.Children.Add(expiresLabel);

                Label descLabel = new Label();
                descLabel.Text = $"Опис: {api.alerts.Alert[0].Desc}";
                mainStackLayout.Children.Add(descLabel);

                Label instuctionLabel = new Label();
                instuctionLabel.Text = $"Інструкція: {api.alerts.Alert[0].Instruction}";
                mainStackLayout.Children.Add(instuctionLabel);

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