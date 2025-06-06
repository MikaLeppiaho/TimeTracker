using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TimeTracker.Models
{
    public enum ProjectStatus
    {
        Active,
        Passive
    }

    public class Project
    {
        public int ProjectId { get; set; }

        public int? CustomerId { get; set; }

        public Customer Customer { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        // Optional allotted time for the project (e.g., total hours)
        public TimeSpan? AllocatedTime { get; set; }
        public ProjectStatus Status { get; set; } = ProjectStatus.Active;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public ICollection<TaskItem> TaskItems { get; set; } = new List<TaskItem>();
        public TimeSpan TotalTimeSpent
        {
            get
            {
                var tasks = TaskItems ?? new List<TaskItem>();
                    return tasks.Aggregate(TimeSpan.Zero, (sum, task) => sum + task.AccumulatedTime);
            }
        }

        public TimeSpan TimeLeft
        {
            get
            {
                if (AllocatedTime.HasValue)
                {
                    // If there are no tasks, TotalTimeSpent will be zero,
                    // so TimeLeft will be equal to AllocatedTime.
                    var left = AllocatedTime.Value - TotalTimeSpent;
                    return left < TimeSpan.Zero ? TimeSpan.Zero : left;
                }
                return TimeSpan.Zero;
            }
        }
    }
}