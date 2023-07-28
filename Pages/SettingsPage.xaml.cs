using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Newtonsoft.Json;
using Weather_App.Models;

namespace Weather_App.Pages;
public partial class SettingsPage : ContentPage
{
	private Settings userSettings = new Settings();

    public SettingsPage()
	{
		InitializeComponent();
		userSettings = Utils.LoadSettingsData(userSettings);
		citySearchBar.Text = userSettings.City;
		loadInfoPicker.SelectedIndex = userSettings.LoadDays - 1;
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
		try
		{
            userSettings.City = citySearchBar.Text;
			userSettings.LoadDays = loadInfoPicker.SelectedIndex + 1; 
            string json = JsonConvert.SerializeObject(userSettings);
			File.WriteAllText(Utils.filePath, json);
		}
		catch(Exception ex)
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