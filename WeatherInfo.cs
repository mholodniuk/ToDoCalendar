using System.Collections.Generic;

/// <summary>
/// Represenets data received from the api
/// </summary>
namespace ToDoCalendar.WeatherInfo
{
    public class WeatherForecast
    {
        public string city_name { get; set; }
        public List<Data> data { get; set; }
    }

    public class Data
    {
        public string high_temp { get; set; }

        public string datetime { get; set; }
        public Weather weather { get; set; }
    }

    public class Weather
    {
        public string icon { get; set; }
        public string code { get; set; }
        public string description { get; set; }
    }
}