using System.Globalization;
using System.Windows.Data;
using TimeTracker.Models;
using TaskStatus = TimeTracker.Models.TaskStatus;


namespace TimeTracker.Common
{

    public class ElapsedTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TaskItem task)
            {
                TimeSpan elapsed;
                if (task.Status == TaskStatus.Active)
                    elapsed = DateTime.Now - task.CreatedAt;
                else
                    elapsed = task.UpdatedAt - task.CreatedAt;
                return elapsed.ToString(@"hh\:mm");
            }
            return "00:00";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}