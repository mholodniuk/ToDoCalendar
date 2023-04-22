using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ToDoCalendar.UserControls;

namespace ToDoCalendar
{
    public partial class MainWindow : Window
    {
        public DateTime currentDate;

        public MainWindow()
        {
            InitializeComponent();
            currentDate = DateTime.Now;
            updateProps(currentDate);
            initToDos(currentDate);
        }

        private void updateProps(DateTime date)
        {
            CurrentDayProp.Text = date.Day.ToString();
            CurrentDayStringProp.Text = date.DayOfWeek.ToString();
            CurrentMonthStringProp.Text = date.ToString("MMM");
        }

        private void initToDos(DateTime ChosenDate)
        {
            IList<Activity> activities;
            arrayOfActivities.Children.Clear();

            using (var context = new CalendarContext())
            {
                activities = context.Activities
                    .Where(activity => DbFunctions.TruncateTime(activity.Date.Day) == DbFunctions.TruncateTime(ChosenDate))
                    .ToList();
            }
            foreach (var activity in activities)
            {
                Item item = new Item();
                item.SetActivity(activity);
                arrayOfActivities.Children.Add(item);
            }
        }

        private void Calendar_OnSelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            var calendar = sender as Calendar;

            if (calendar.SelectedDate.HasValue)
            {
                currentDate = calendar.SelectedDate.Value;
                updateProps(currentDate);
                initToDos(currentDate);
            }
        }

        private void AddActivity_Click(object sender, RoutedEventArgs e)
        {
            string Name = txtNote.Text;
            string Time = txtTime.Text;
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
                initToDos(currentDate);
            }
        }

        private int getSelectedDateID(DateTime selectedDate, CalendarContext context)
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
        private bool checkIfDateHasAcitivties(DateTime date, CalendarContext context)
        {
            return context.Dates.Any(d => DbFunctions.TruncateTime(d.Day) == DbFunctions.TruncateTime(date));
        }

        private void lblNote_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtNote.Focus();
        }

        private void lblTime_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtTime.Focus();
        }

        private void txtNote_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNote.Text) && txtNote.Text.Length > 0)
                lblNote.Visibility = Visibility.Collapsed;
            else
                lblNote.Visibility = Visibility.Visible;
        }

        private void txtTime_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTime.Text) && txtTime.Text.Length > 0)
                lblTime.Visibility = Visibility.Collapsed;
            else
                lblTime.Visibility = Visibility.Visible;
        }
    }
}
