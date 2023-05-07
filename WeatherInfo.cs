using System.Collections.Generic;

/// <summary>
/// TODO: fill all needed fields
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
        public double high_temp { get; set; }
        public Weather weather { get; set; }
    }

    public class Weather
    {
        public string icon { get; set; }
        public string code { get; set; }
        public string description { get; set; }
    }
}