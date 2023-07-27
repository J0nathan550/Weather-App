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
		Utils.LoadSettingsData(userSettings);
		citySearchBar.Text = userSettings.City;
		switchSaveBackground.IsToggled = userSettings.SaveInfo;
		languagePicker.SelectedIndex = userSettings.LanguageIndex;
		loadInfoPicker.SelectedIndex = userSettings.LoadDays;
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
		try
		{
            userSettings.City = citySearchBar.Text;
            userSettings.SaveInfo = switchSaveBackground.IsToggled;
			userSettings.LanguageIndex = languagePicker.SelectedIndex;
			userSettings.LoadDays = loadInfoPicker.SelectedIndex; 
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