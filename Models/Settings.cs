namespace Weather_App.Models
{
    public class Settings
    {
        public string City { get; set; } = "London";
        public bool SaveInfo { get; set; } = false;
        public int LanguageIndex { get; set; } = 0;
        public int LoadDays { get; set; } = 1;
    }
}