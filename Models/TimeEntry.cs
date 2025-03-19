namespace TimeTracker.Models
{
    public class TimeEntry
    {
        public int TimeEntryId { get; set; }

        public int TaskItemId { get; set; }

        public TaskItem TaskItem { get; set; }

        public DateTime StartTime { get; set; }

        // Nullable end time for active tasks
        public DateTime? EndTime { get; set; }

        // Duration can be calculated or stored
        public TimeSpan Duration { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}