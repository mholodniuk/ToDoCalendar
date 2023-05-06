namespace ToDoCalendar.Utils
{
    public class Utils
    {
        public static bool isActivityValid(string name, string time)
        {
            return name != null && name != "" && time != null && time != "";
        }
    }
}
