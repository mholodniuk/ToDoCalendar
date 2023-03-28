using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ToDoCalendar.UserControls;

namespace ToDoCalendar
{
    public partial class MainWindow : Window
    {
        public int currentYear = DateTime.Now.Year;
        public int currentMonth = DateTime.Now.Month;
        public string currentMonthString = DateTime.Now.ToString("MMM");
        public int currentDay = DateTime.Now.Day;
        public string currentDayString = DateTime.Now.DayOfWeek.ToString();

        public MainWindow()
        {
            InitializeComponent();
            CurrentDayProp.Text = currentDay.ToString();
            CurrentDayStringProp.Text = currentDayString;
            CurrentMonthStringProp.Text = currentMonthString;

            initToDos();
            using (var context = new CalendarContext())
            {
                foreach (var date in context.Dates)
                {
                    Console.WriteLine(date.Day);
                }
            }
        }

        private void initToDos()
        {
            IList<Activity> activities;
            arrayOfActivities.Children.Clear();

            using (var context = new CalendarContext())
            {
                activities = context.Activities.ToList();
            }
            foreach (var activity in activities)
            {
                Item item = new Item();
                item.SetActivity(activity);
                arrayOfActivities.Children.Add(item);
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
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

        private void ChangeCurrentMonth(object sender, RoutedEventArgs e)
        {

        }
    }
}
