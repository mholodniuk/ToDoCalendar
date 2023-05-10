using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ToDoCalendar.UserControls;
using System.Globalization;
using ToDoCalendar.WeatherInfo;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace ToDoCalendar
{
    public class MyClass
    {
        public static string MyCity;

        static MyClass()
        {
            MyCity = "Wrocław";
        }
    }

    /// <summary>
    /// Main window class
    /// 
    /// Stores state of the application and defines method for user interactions (connects front-end with back-end logic)
    /// </summary>
    public partial class MainWindow : Window
    {
        public DateTime currentDate;

        CultureInfo polishCulture = new CultureInfo("pl-PL");
        WeatherForecast forecast1;
        
        /// <summary>
        /// Constructor - responsible for initializing component and member fields and UI.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }
        

        /// <summary>
        /// Performs asynchronous main window loading action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            forecast1 = await GetWeatherInfo("Wrocław");
            try
            {
                currentDate = DateTime.Now;
                initToDos(currentDate);
                updateProps(currentDate);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wystąpił błąd podczas pobierania prognozy pogody: " + ex.Message);
            }
            finally
            {
                this.Show();
            }
        }

        /// <summary>
        /// Updates current weather forecast when user changed value in dropdown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CityName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;

            if (selectedItem != null && selectedItem.Content != null)
            {
                string cityName = selectedItem.Content.ToString();
                forecast1 = await GetWeatherInfo(cityName);
                updateProps(currentDate);
            }
        }
        /// <summary>
        /// Initializes list of todo activities based on currently selected date
        /// 
        /// updates UI of the application
        /// </summary>
        /// <param name="ChosenDate"></param>
        private void initToDos(DateTime ChosenDate)
        {
            arrayOfActivities.Children.Clear();
            
            foreach (var activity in getAllActivities(ChosenDate))
            {
                Item item = new Item();
                item.SetActivity(activity);
                arrayOfActivities.Children.Add(item);
            }
        }

        /// <summary>
        /// Performs asynchronous api call to the weather api
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public async Task<WeatherForecast> GetWeatherInfo(string city)
        {
            HttpClient client = new HttpClient();
            var call = $"https://api.weatherbit.io/v2.0/forecast/daily?lang=pl&city={city}&country=Polska&key=72f159d74c034c0c8802ab9f590524e9";
            var response = await client.GetAsync(call);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                forecast1 = JsonConvert.DeserializeObject<WeatherForecast>(jsonString);
                return forecast1;
            }
            else
            {
                Console.WriteLine("Nie udało się pobrać danych z API.");
                return null;
            }
        }

        /// <summary>
        /// Updates current state of the program (date information controls) based on currently selected date
        /// </summary>
        /// <param name="date"></param>
        private void updateProps(DateTime date)
        {
            CurrentDayProp.Text = date.Day.ToString();
            CurrentDayStringProp.Text = date.ToString("dddd", polishCulture);
            CurrentMonthStringProp.Text = date.ToString("MMM");
            string formattedDate = currentDate.ToString("yyyy-MM-dd");

            for(int i = 0; i < 15; i++)
            {
                string s1 = formattedDate;
                string s2 = forecast1.data[i].datetime;
                if (s1 == s2)
                {
                    WeatherInfo.Text = forecast1.data[i].weather.description;
                    Temperature.Text = forecast1.data[i].high_temp + "°C";
                    break;
                }
                else
                {
                    WeatherInfo.Text = "Brak danych";
                    Temperature.Text = "";
                }
            }
            
            City.Text = "Miasto";
            WeatherInf.Text = "Pogoda";
        }

        /// <summary>
        /// Retrieves all activities found in database on given date and returns them
        /// </summary>
        /// <param name="ChosenDate"></param>
        /// <returns>list of activities</returns>
        public IList<Activity> getAllActivities(DateTime ChosenDate)
        {
            using (var context = new CalendarContext())
            {
                return context.Activities
                    .Where(activity => DbFunctions.TruncateTime(activity.Date.Day) == DbFunctions.TruncateTime(ChosenDate))
                    .ToList();
            }
        }

        /// <summary>
        /// Handles current date change (when user clicks on the calendar component)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Calendar_OnSelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            var calendar = sender as System.Windows.Controls.Calendar;

            if (calendar.SelectedDate.HasValue)
            {
                currentDate = calendar.SelectedDate.Value;
                updateProps(currentDate);
                initToDos(currentDate);
            }
        }

        /// <summary>
        /// Handles adding new activity on currently selected date
        /// 
        /// Performs validation if form is not empty, otherwise it does not add empty fields to the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddActivity_Click(object sender, RoutedEventArgs e)
        {
            string Name = txtNote.Text;
            string Time = "00:00";
            if (!ValidateTime(txtTime.Text)) { return; }
            else Time = txtTime.Text;


            if (!Utils.Utils.isActivityValid(Name, Time))
            {
                return;
            }

            using (var context = new CalendarContext())
            {
                int currentDateID = getSelectedDateID(currentDate, context);

                var newActivity = new Activity()
                {
                    Name = Name,
                    StartTime = Time,
                    Done = false,
                    DateID = currentDateID
                };

                context.Activities.Add(newActivity);
                context.SaveChanges();
            }

            initToDos(currentDate);
        }

        /// <summary>
        /// Retrieves ID of currently selected date 
        /// If entity with given date does not exist in the database, it is created and inserted
        /// </summary>
        /// <param name="selectedDate"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public int getSelectedDateID(DateTime selectedDate, CalendarContext context)
        {
            if (!checkIfDateHasAcitivties(currentDate, context))
            {
                var newDate = new Date()
                {
                    Day = currentDate,
                    Activities = new List<Activity>()
                };
                context.Dates.Add(newDate);
                context.SaveChanges();
                return newDate.ID;
            }
            else
            {
                return context.Dates.Where(d => DbFunctions.TruncateTime(d.Day) == DbFunctions.TruncateTime(currentDate)).First().ID;
            }
        }

        /// <summary>
        /// Utility method that checks if database contains entity with given date
        /// </summary>
        /// <param name="date"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool checkIfDateHasAcitivties(DateTime date, CalendarContext context)
        {
            return context.Dates.Any(d => DbFunctions.TruncateTime(d.Day) == DbFunctions.TruncateTime(date));
        }

        /// <summary>
        /// UI utility method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblNote_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtNote.Focus();
        }

        /// <summary>
        /// UI utility method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblTime_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtTime.Focus();
        }

        /// <summary>
        /// UI utility method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNote_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNote.Text) && txtNote.Text.Length > 0)
                lblNote.Visibility = Visibility.Collapsed;
            else
                lblNote.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// UI utility method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private bool ValidateTime(string time)
        {
            string[] timeParts = time.Split(':');

            if (timeParts.Length != 2)
            {
                MessageBox.Show("Niepoprawny format godziny!");
                return false;
            }

            bool firstPartIsValid = int.TryParse(timeParts[0], out int hours) && hours >= 0 && hours <= 23;
            bool secondPartIsValid = int.TryParse(timeParts[1], out int minutes) && minutes >= 0 && minutes <= 59;

            if (!firstPartIsValid || !secondPartIsValid)
            {
                MessageBox.Show("Niepoprawna godzina!");
                return false;
            }

            return true;
        }
        private void txtTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTime.Text) && txtTime.Text.Length > 0)
                lblTime.Visibility = Visibility.Collapsed;
            else
                lblTime.Visibility = Visibility.Visible;
        }
    }
}
