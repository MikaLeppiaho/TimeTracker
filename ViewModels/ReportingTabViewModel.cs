using System.Collections.ObjectModel;
using TimeTracker.Data;
using TimeTracker.Models;

namespace TimeTracker.ViewModels
{
    public class ReportingViewModel : BaseViewModel
    {
        // Start and end of the current week.
        public DateTime CurrentWeekStart { get; private set; }
        public DateTime CurrentWeekEnd { get; private set; }

        // Collection of tasks for the current week.
        public ObservableCollection<TaskItem> WeeklyTasks { get; set; } = new ObservableCollection<TaskItem>();

        public ReportingViewModel()
        {
            SetCurrentWeek(DateTime.Now);
            LoadWeeklyReport();
        }

        // Computes the start (Monday) and end (Sunday) dates for the week of the given date.
        private void SetCurrentWeek(DateTime reference)
        {
            // Calculate difference from Monday.
            int diff = reference.DayOfWeek - DayOfWeek.Monday;
            if (diff < 0) diff += 7;
            CurrentWeekStart = reference.AddDays(-diff).Date;
            CurrentWeekEnd = CurrentWeekStart.AddDays(6).Date;
        }

        // Loads all tasks created in the current week.
        public void LoadWeeklyReport()
        {
            using (var context = new TimeTrackerContext())
            {
                var tasks = context.TaskItems
                    .Where(t => t.CreatedAt.Date >= CurrentWeekStart && t.CreatedAt.Date <= CurrentWeekEnd)
                    .ToList();

                WeeklyTasks.Clear();
                foreach (var task in tasks)
                {
                    WeeklyTasks.Add(task);
                }
            }
        }

        // Later you can add commands to move to previous/next weeks and reload the report.
    }
}