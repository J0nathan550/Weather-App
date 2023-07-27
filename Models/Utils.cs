using Newtonsoft.Json;

namespace Weather_App.Models
{
    public class Utils
    {
        public static string apiKey = "7979326c13a1461591381800232307";
        public static string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/" + "WeatherApp/" + "save.json";
        public static string directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/" + "WeatherApp/";
        public static Settings LoadSettingsData(Settings userSettings)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    userSettings = JsonConvert.DeserializeObject<Settings>(json);
                }
                else
                {
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    var fs = File.Create(filePath);
                    fs.Close();
                    string json = JsonConvert.SerializeObject(userSettings);
                    File.WriteAllText(filePath, json);
                }
            }
            catch
            {
                File.Delete(filePath);
                var fs = File.Create(filePath);
                fs.Close();
                string json = JsonConvert.SerializeObject(userSettings);
                File.WriteAllText(filePath, json);
            }
            return userSettings;
        }
    }
}