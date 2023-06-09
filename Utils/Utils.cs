﻿namespace ToDoCalendar.Utils
{
    /// <summary>
    /// Utility methods
    /// Used in application to validate data or perform repetitive tasks
    /// </summary>
    public class Utils
    {
        public static bool isActivityValid(string name, string time)
        {
            return name != null && name != "" && time != null && time != "";
        }

        public static FontAwesome.WPF.FontAwesomeIcon getIconForActivity(Activity activity)
        {
            return activity.Done ? FontAwesome.WPF.FontAwesomeIcon.CheckCircle : FontAwesome.WPF.FontAwesomeIcon.CircleThin;
        }
    }
}
