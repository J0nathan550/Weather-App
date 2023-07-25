using Newtonsoft.Json;
namespace Weather_App.Models
{
    public class WeatherAPI
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        [JsonProperty("location")]
        public Location location;

        [JsonProperty("current")]
        public Current current;

        [JsonProperty("forecast")]
        public Forecast forecast;

        [JsonProperty("alerts")]
        public Alerts alerts;

        public class AirQuality
        {
            [JsonProperty("co")]
            public double Co;

            [JsonProperty("no2")]
            public double No2;

            [JsonProperty("o3")]
            public double O3;

            [JsonProperty("so2")]
            public double So2;

            [JsonProperty("pm2_5")]
            public double Pm25;

            [JsonProperty("pm10")]
            public double Pm10;

            [JsonProperty("us-epa-index")]
            public int UsEpaIndex;

            [JsonProperty("gb-defra-index")]
            public int GbDefraIndex;
        }

        public class Alerts
        {
            [JsonProperty("alert")]
            public List<object> Alert;
        }

        public class Astro
        {
            [JsonProperty("sunrise")]
            public string Sunrise;

            [JsonProperty("sunset")]
            public string Sunset;

            [JsonProperty("moonrise")]
            public string Moonrise;

            [JsonProperty("moonset")]
            public string Moonset;

            [JsonProperty("moon_phase")]
            public string MoonPhase;

            [JsonProperty("moon_illumination")]
            public string MoonIllumination;

            [JsonProperty("is_moon_up")]
            public int IsMoonUp;

            [JsonProperty("is_sun_up")]
            public int IsSunUp;
        }

        public class Condition
        {
            [JsonProperty("text")]
            public string Text;

            [JsonProperty("icon")]
            public string Icon;

            [JsonProperty("code")]
            public int Code;
        }

        public class Current
        {
            [JsonProperty("last_updated_epoch")]
            public int LastUpdatedEpoch;

            [JsonProperty("last_updated")]
            public string LastUpdated;

            [JsonProperty("temp_c")]
            public double TempC;

            [JsonProperty("temp_f")]
            public double TempF;

            [JsonProperty("is_day")]
            public bool IsDay;

            [JsonProperty("condition")]
            public Condition Condition;

            [JsonProperty("wind_mph")]
            public double WindMph;

            [JsonProperty("wind_kph")]
            public double WindKph;

            [JsonProperty("wind_degree")]
            public int WindDegree;

            [JsonProperty("wind_dir")]
            public string WindDir;

            [JsonProperty("pressure_mb")]
            public double PressureMb;

            [JsonProperty("pressure_in")]
            public double PressureIn;

            [JsonProperty("precip_mm")]
            public double PrecipMm;

            [JsonProperty("precip_in")]
            public double PrecipIn;

            [JsonProperty("humidity")]
            public int Humidity;

            [JsonProperty("cloud")]
            public int Cloud;

            [JsonProperty("feelslike_c")]
            public double FeelslikeC;

            [JsonProperty("feelslike_f")]
            public double FeelslikeF;

            [JsonProperty("vis_km")]
            public double VisKm;

            [JsonProperty("vis_miles")]
            public double VisMiles;

            [JsonProperty("uv")]
            public double Uv;

            [JsonProperty("gust_mph")]
            public double GustMph;

            [JsonProperty("gust_kph")]
            public double GustKph;

            [JsonProperty("air_quality")]
            public AirQuality AirQuality;
        }

        public class Day
        {
            [JsonProperty("maxtemp_c")]
            public double MaxtempC;

            [JsonProperty("maxtemp_f")]
            public double MaxtempF;

            [JsonProperty("mintemp_c")]
            public double MintempC;

            [JsonProperty("mintemp_f")]
            public double MintempF;

            [JsonProperty("avgtemp_c")]
            public double AvgtempC;

            [JsonProperty("avgtemp_f")]
            public double AvgtempF;

            [JsonProperty("maxwind_mph")]
            public double MaxwindMph;

            [JsonProperty("maxwind_kph")]
            public double MaxwindKph;

            [JsonProperty("totalprecip_mm")]
            public double TotalprecipMm;

            [JsonProperty("totalprecip_in")]
            public double TotalprecipIn;

            [JsonProperty("totalsnow_cm")]
            public double TotalsnowCm;

            [JsonProperty("avgvis_km")]
            public double AvgvisKm;

            [JsonProperty("avgvis_miles")]
            public double AvgvisMiles;

            [JsonProperty("avghumidity")]
            public double Avghumidity;

            [JsonProperty("daily_will_it_rain")]
            public int DailyWillItRain;

            [JsonProperty("daily_chance_of_rain")]
            public int DailyChanceOfRain;

            [JsonProperty("daily_will_it_snow")]
            public int DailyWillItSnow;

            [JsonProperty("daily_chance_of_snow")]
            public int DailyChanceOfSnow;

            [JsonProperty("condition")]
            public Condition Condition;

            [JsonProperty("uv")]
            public double Uv;

            [JsonProperty("air_quality")]
            public AirQuality AirQuality;
        }

        public class Forecast
        {
            [JsonProperty("forecastday")]
            public List<Forecastday> Forecastday;
        }

        public class Forecastday
        {
            [JsonProperty("date")]
            public string Date;

            [JsonProperty("date_epoch")]
            public int DateEpoch;

            [JsonProperty("day")]
            public Day Day;

            [JsonProperty("astro")]
            public Astro Astro;

            [JsonProperty("hour")]
            public List<Hour> Hour;
        }

        public class Hour
        {
            [JsonProperty("time_epoch")]
            public int TimeEpoch;

            [JsonProperty("time")]
            public string Time;

            [JsonProperty("temp_c")]
            public double TempC;

            [JsonProperty("temp_f")]
            public double TempF;

            [JsonProperty("is_day")]
            public int IsDay;

            [JsonProperty("condition")]
            public Condition Condition;

            [JsonProperty("wind_mph")]
            public double WindMph;

            [JsonProperty("wind_kph")]
            public double WindKph;

            [JsonProperty("wind_degree")]
            public int WindDegree;

            [JsonProperty("wind_dir")]
            public string WindDir;

            [JsonProperty("pressure_mb")]
            public double PressureMb;

            [JsonProperty("pressure_in")]
            public double PressureIn;

            [JsonProperty("precip_mm")]
            public double PrecipMm;

            [JsonProperty("precip_in")]
            public double PrecipIn;

            [JsonProperty("humidity")]
            public int Humidity;

            [JsonProperty("cloud")]
            public int Cloud;

            [JsonProperty("feelslike_c")]
            public double FeelslikeC;

            [JsonProperty("feelslike_f")]
            public double FeelslikeF;

            [JsonProperty("dewpoint_c")]
            public double DewpointC;

            [JsonProperty("dewpoint_f")]
            public double DewpointF;

            [JsonProperty("will_it_rain")]
            public int WillItRain;

            [JsonProperty("chance_of_rain")]
            public int ChanceOfRain;

            [JsonProperty("will_it_snow")]
            public int WillItSnow;

            [JsonProperty("chance_of_snow")]
            public int ChanceOfSnow;

            [JsonProperty("vis_km")]
            public double VisKm;

            [JsonProperty("vis_miles")]
            public double VisMiles;

            [JsonProperty("gust_mph")]
            public double GustMph;

            [JsonProperty("gust_kph")]
            public double GustKph;

            [JsonProperty("uv")]
            public double Uv;

            [JsonProperty("air_quality")]
            public AirQuality AirQuality;
        }

        public class Location
        {
            [JsonProperty("name")]
            public string Name;

            [JsonProperty("region")]
            public string Region;

            [JsonProperty("country")]
            public string Country;

            [JsonProperty("lat")]
            public double Lat;

            [JsonProperty("lon")]
            public double Lon;

            [JsonProperty("tz_id")]
            public string TzId;

            [JsonProperty("localtime_epoch")]
            public int LocaltimeEpoch;

            [JsonProperty("localtime")]
            public string Localtime;
        }
    }
}