using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ToDoCalendar.UserControls
{
    public partial class Item : UserControl
    {
        public int ID { get; set; }
        
        public Item()
        {
            InitializeComponent();
        }

        public void SetActivity(Activity activity)
        {
            this.ID = activity.ID;
            this.Title = activity.Name;
            this.Color = Brushes.White;
            this.Time = activity.StartTime;
            this.IconBell = FontAwesome.WPF.FontAwesomeIcon.ClockOutline;
            this.Icon = activity.Done ? FontAwesome.WPF.FontAwesomeIcon.CheckCircle : FontAwesome.WPF.FontAwesomeIcon.CircleThin;
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(Item));


        public string Time
        {
            get { return (string)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register("Time", typeof(string), typeof(Item));


        public SolidColorBrush Color
        {
            get { return (SolidColorBrush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(SolidColorBrush), typeof(Item));


        public FontAwesome.WPF.FontAwesomeIcon Icon
        {
            get { return (FontAwesome.WPF.FontAwesomeIcon)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(FontAwesome.WPF.FontAwesomeIcon), typeof(Item));


        public FontAwesome.WPF.FontAwesomeIcon IconBell
        {
            get { return (FontAwesome.WPF.FontAwesomeIcon)GetValue(IconBellProperty); }
            set { SetValue(IconBellProperty, value); }
        }

        public static readonly DependencyProperty IconBellProperty = DependencyProperty.Register("IconBell", typeof(FontAwesome.WPF.FontAwesomeIcon), typeof(Item));

        private void ItemEditActivity(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Console.WriteLine("Item Edit button clicked");
        }

        private void ItemDeleteActivity(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Console.WriteLine("Item Delete button clicked");
        }

        private void ItemCheckActivity(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            using (var context = new CalendarContext())
            {
                var activity = context.Activities.Find(this.ID);
                if (activity != null)
                {
                    if (activity.Done)
                    {
                        this.Icon = FontAwesome.WPF.FontAwesomeIcon.CircleThin;
                        context.Entry(activity).Entity.Done = false;
                    }
                    else
                    {
                        this.Icon = FontAwesome.WPF.FontAwesomeIcon.CheckCircle;
                        context.Entry(activity).Entity.Done = true;
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}