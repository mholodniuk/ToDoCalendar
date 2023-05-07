using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ToDoCalendar.WeatherInfo
{
    public class WeatherForecast
    {
        public string ValidDate { get; set; }
        public long Ts { get; set; }
        public string Datetime { get; set; }
        public double WindGustSpd { get; set; }
        public double WindSpd { get; set; }
        public Weather Weather { get; set; }
        public double Temperature { get; set; }
    }

    public class Weather
    {
        public string Icon { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public static class Program
    {
        public static WeatherForecast weatherForecast { get; private set; }

        public static async Task GetWeatherInfo()
        {
            HttpClient client = new HttpClient();
            string call, API_KEY, city;
            API_KEY = "72f159d74c034c0c8802ab9f590524e9";
            city = "Wrocław";
            call = $"https://api.weatherbit.io/v2.0/forecast/daily?city={city}&key={API_KEY}&format=json";
            var response = await client.GetAsync(call);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var forecast = JsonConvert.DeserializeObject<WeatherForecast>(jsonString);

                weatherForecast = forecast;
            }
            else
            {
                Console.WriteLine("Nie udało się pobrać danych z API.");
            }
        }
    }
}