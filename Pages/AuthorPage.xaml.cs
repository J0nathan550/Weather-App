using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace Weather_App.Pages;

public partial class AuthorPage : ContentPage
{
	public AuthorPage()
	{
		InitializeComponent();
	}

    private void TelegramButtonClicked(object sender, EventArgs e)
    {
        OpenBrowser("https://t.me/brenntkopfsd");
    }

    private void YouTubeButtonClicked(object sender, EventArgs e)
    {
        OpenBrowser("https://www.youtube.com/channel/UCZhjs2f-DsO7SXf5FJCKbxA");
    }

    private void PlayMarketButtonClicked(object sender, EventArgs e)
    {

    }

    private void WebAPIButtonClicked(object sender, EventArgs e)
    {
        OpenBrowser("https://www.weatherapi.com");
    }

    private async void OpenBrowser(string link)
    {
        try
        {
            Uri uri = new Uri(link);
            BrowserLaunchOptions options = new BrowserLaunchOptions()
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show,
                PreferredToolbarColor = Color.Parse("#fc7f03"),
                PreferredControlColor = Color.Parse("#fc7f03")
            };

            await Browser.Default.OpenAsync(uri, options);
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