using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TimeTracker.Models;

namespace TimeTracker.Models
{
    public class TaskItem : INotifyPropertyChanged
    {
        private int _taskItemId;
        public int TaskItemId
        {
            get => _taskItemId;
            set { _taskItemId = value; OnPropertyChanged(nameof(TaskItemId)); }
        }

        private int _projectId;
        public int ProjectId
        {
            get => _projectId;
            set { _projectId = value; OnPropertyChanged(nameof(ProjectId)); }
        }

        private Project _project;
        public Project Project
        {
            get => _project;
            set { _project = value; OnPropertyChanged(nameof(Project)); }
        }

        private string _name;
        [Required]
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(nameof(Description)); }
        }

        private TaskStatus _status;
        public TaskStatus Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(nameof(Status)); }
        }

        private DateTime _createdAt = DateTime.Now;
        public DateTime CreatedAt
        {
            get => _createdAt;
            set { _createdAt = value; OnPropertyChanged(nameof(CreatedAt)); }
        }

        private DateTime _updatedAt = DateTime.Now;
        public DateTime UpdatedAt
        {
            get => _updatedAt;
            set { _updatedAt = value; OnPropertyChanged(nameof(UpdatedAt)); }
        }

        private ICollection<TimeEntry> _timeEntries;
        public ICollection<TimeEntry> TimeEntries
        {
            get => _timeEntries;
            set { _timeEntries = value; OnPropertyChanged(nameof(TimeEntries)); }
        }

        private TimeSpan _accumulatedTime = TimeSpan.Zero;
        public TimeSpan AccumulatedTime
        {
            get => _accumulatedTime;
            set { _accumulatedTime = value; OnPropertyChanged(nameof(AccumulatedTime)); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum TaskStatus
    {
        Active,
        Paused,
        Completed
    }
}