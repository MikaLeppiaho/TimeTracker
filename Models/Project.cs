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

        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        // Optional allotted time for the project (e.g., total hours)
        public TimeSpan? AllocatedTime { get; set; }
        public ProjectStatus Status { get; set; } = ProjectStatus.Active;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public ICollection<TaskItem> TaskItems { get; set; }
    }
}