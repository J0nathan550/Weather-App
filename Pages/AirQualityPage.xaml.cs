using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Newtonsoft.Json;
using Weather_App.Models;

namespace Weather_App.Pages;

public partial class AirQualityPage : ContentPage
{
    private Settings settings = new Settings();
    private Frame mainFrame;
    private VerticalStackLayout mainStackLayout;

    public AirQualityPage()
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
            string json = await client.GetStringAsync($"https://api.weatherapi.com/v1/current.json?key={Utils.apiKey}&q={settings.City}&aqi=yes");
            WeatherAPI api = JsonConvert.DeserializeObject<WeatherAPI>(json);

            string quality = api.current.AirQuality.UsEpaIndex switch
            {
                1 => "�����",
                2 => "������",
                3 => "����������� ��� ������� ����� �����",
                4 => "����������� ��� �����",
                5 => "���� ����������� ��� �����",
                6 => "�����������",
                _ => "��������"
            };

            await Dispatcher.DispatchAsync(() =>
            {
                mainFrame = new Frame();
                mainFrame.BorderColor = Colors.Black;
                mainLayout.Add(mainFrame);
                mainStackLayout = new VerticalStackLayout();
                mainStackLayout.Spacing = 15;
                mainFrame.Content = mainStackLayout;

                Label monokcydLabel = new Label();
                monokcydLabel.Text = $"��������� �������: {api.current.AirQuality.Co}";
                mainStackLayout.Add(monokcydLabel);

                Label oksydLabel = new Label();
                oksydLabel.Text = $"����� �����: {api.current.AirQuality.No2}";
                mainStackLayout.Add(oksydLabel);

                Label ozonLabel = new Label();
                ozonLabel.Text = $"����: {api.current.AirQuality.O3}";
                mainStackLayout.Add(ozonLabel);

                Label oksydSeriLabel = new Label();
                oksydSeriLabel.Text = $"����� ����: {api.current.AirQuality.So2}";
                mainStackLayout.Add(oksydSeriLabel);

                Label hardParticles25Label = new Label();
                hardParticles25Label.Text = $"����� �������� (25): {api.current.AirQuality.Pm25}";
                mainStackLayout.Add(hardParticles25Label);

                Label hardParticles10Label = new Label();
                hardParticles10Label.Text = $"����� �������� (10): {api.current.AirQuality.Pm10}";
                mainStackLayout.Add(hardParticles10Label);

                Label airQualityLabel = new Label();
                airQualityLabel.Text = $"����� �������: {quality}";
                mainStackLayout.Add(airQualityLabel);

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
            string json = await client.GetStringAsync($"https://api.weatherapi.com/v1/current.json?key={Utils.apiKey}&q={settings.City}&aqi=yes");
            WeatherAPI api = JsonConvert.DeserializeObject<WeatherAPI>(json);
            string quality = api.current.AirQuality.UsEpaIndex switch
            {
                1 => "�����",
                2 => "������",
                3 => "����������� ��� ������� ����� �����",
                4 => "����������� ��� �����",
                5 => "���� ����������� ��� �����",
                6 => "�����������",
                _ => "��������"
            };
            await Dispatcher.DispatchAsync(() =>
            {
                mainFrame = new Frame();
                mainFrame.BorderColor = Colors.Black;
                mainLayout.Add(mainFrame);
                mainStackLayout = new VerticalStackLayout();
                mainStackLayout.Spacing = 15;
                mainFrame.Content = mainStackLayout;

                Label monokcydLabel = new Label();
                monokcydLabel.Text = $"��������� �������: {api.current.AirQuality.Co}";
                mainStackLayout.Add(monokcydLabel);

                Label oksydLabel = new Label();
                oksydLabel.Text = $"����� �����: {api.current.AirQuality.No2}";
                mainStackLayout.Add(oksydLabel);

                Label ozonLabel = new Label();
                ozonLabel.Text = $"����: {api.current.AirQuality.O3}";
                mainStackLayout.Add(ozonLabel);

                Label oksydSeriLabel = new Label();
                oksydSeriLabel.Text = $"����� ����: {api.current.AirQuality.So2}";
                mainStackLayout.Add(oksydSeriLabel);

                Label hardParticles25Label = new Label();
                hardParticles25Label.Text = $"����� �������� (25): {api.current.AirQuality.Pm25}";
                mainStackLayout.Add(hardParticles25Label);

                Label hardParticles10Label = new Label();
                hardParticles10Label.Text = $"����� �������� (10): {api.current.AirQuality.Pm10}";
                mainStackLayout.Add(hardParticles10Label);

                Label airQualityLabel = new Label();
                airQualityLabel.Text = $"����� �������: {quality}";
                mainStackLayout.Add(airQualityLabel);

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