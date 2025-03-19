using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Threading;
using TimeTracker.Common;
using TimeTracker.Data;
using TimeTracker.Models;
using TimeTracker.ViewModels;
using TaskStatus = TimeTracker.Models.TaskStatus;

namespace TimeTracker.ViewModels
{
    public class HomeTabViewModel : BaseViewModel
    {
        // Available projects (only those marked as active)
        public ObservableCollection<Project> AvailableProjects { get; set; } = new ObservableCollection<Project>();

        private Project? _selectedProject;
        public Project? SelectedProject
        {
            get => _selectedProject;
            set
            {
                _selectedProject = value;
                OnPropertyChanged();
                ((RelayCommand)StartTaskCommand).RaiseCanExecuteChanged();
            }
        }

        // List of tasks created today
        public ObservableCollection<TaskItem> TodayTasks { get; set; } = new ObservableCollection<TaskItem>();

        // The currently active task (e.g. the one that was just started)
        private TaskItem? _activeTask;
        public TaskItem? ActiveTask
        {
            get => _activeTask;
            set {
               _activeTask = value;
               OnPropertyChanged(); 
               ((RelayCommand)ToggleTaskCommand).RaiseCanExecuteChanged();
            }
        }

        // The task selected from the task list.
        private TaskItem? _selectedTask;
        public TaskItem? SelectedTask
        {
            get => _selectedTask;
            set
            {
                _selectedTask = value;
                OnPropertyChanged();
                // Update the editable description when selection changes.
                TaskDescription = _selectedTask?.Description ?? "";
                OnPropertyChanged(nameof(CurrentTaskElapsedTime));
                ((RelayCommand)ToggleTaskCommand).RaiseCanExecuteChanged();
            }
        }

        // Editable task description. This updates the underlying selected (or active) task.
        private string _taskDescription = "";
        public string TaskDescription
        {
            get => _taskDescription;
            set
            {
                _taskDescription = value;
                OnPropertyChanged();
                if (SelectedTask != null)
                {
                    SelectedTask.Description = _taskDescription;
                    UpdateTask(SelectedTask);
                }
                else if (ActiveTask != null)
                {
                    ActiveTask.Description = _taskDescription;
                    UpdateTask(ActiveTask);
                }
            }
        }

        // Returns the elapsed time (in hh:mm) for the selected task (if any) or the active task.
        public string CurrentTaskElapsedTime
        {
            get
            {
                TaskItem? task = SelectedTask ?? ActiveTask;
                if (task != null)
                {
                    TimeSpan elapsed;
                    if (task.Status == TaskStatus.Active)
                    {
                        // Total elapsed time = previously accumulated time + current active session duration.
                        elapsed = task.AccumulatedTime + (DateTime.Now - task.CreatedAt);
                    }
                    else
                    {
                        // If paused, the elapsed time is the accumulated time.
                        elapsed = task.AccumulatedTime;
                    }
                    return elapsed.ToString(@"hh\:mm\:ss");
                }
                return "00:00";
            }
        }

        // Commands
        public ICommand StartTaskCommand { get; }
        public ICommand ToggleTaskCommand { get; }

        // Timer to refresh elapsed time display
        private readonly DispatcherTimer _timer;

 public HomeTabViewModel()
    {
        StartTaskCommand = new RelayCommand(StartTask, () => SelectedProject != null);
        ToggleTaskCommand = new RelayCommand(ToggleTask, () => SelectedTask != null || ActiveTask != null);

        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _timer.Tick += (s, e) =>
        {
            OnPropertyChanged(nameof(CurrentTaskElapsedTime));
            // Optionally force the TodayTasks list to update.
            OnPropertyChanged(nameof(TodayTasks));
        };
        _timer.Start();
    }

        public void RefreshData()
        {
            LoadAvailableProjects();
            LoadTodayTasks();
        }

        private void LoadAvailableProjects()
        {
            using (var context = new TimeTrackerContext())
            {
                var projects = context.Projects.Where(p => p.Status == ProjectStatus.Active).ToList();
                AvailableProjects.Clear();
                foreach (var proj in projects)
                {
                    AvailableProjects.Add(proj);
                }
            }
        }

        private void LoadTodayTasks()
        {
            using (var context = new TimeTrackerContext())
            {
                var tasks = context.TaskItems.Where(t => t.CreatedAt.Date == DateTime.Today).ToList();
                TodayTasks.Clear();
                foreach (var task in tasks)
                {
                    TodayTasks.Add(task);
                }
            }
        }

        // Starts a new task for the selected project.
        public void StartTask()
        {
            if (SelectedProject == null)
                return;

            // Pause any currently active task.
            if (ActiveTask != null && ActiveTask.Status == TaskStatus.Active)
                PauseTask(ActiveTask);

            var newTask = new TaskItem
            {
                ProjectId = SelectedProject.ProjectId,
                Name = SelectedProject.Name,
                Description = "",
                Status = TaskStatus.Active,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            using (var context = new TimeTrackerContext())
            {
                context.TaskItems.Add(newTask);
                context.SaveChanges();
            }

            ActiveTask = newTask;
            TaskDescription = "";
            TodayTasks.Add(newTask);
        }

        public void PauseTask(TaskItem task)
        {
            if (task.Status == TaskStatus.Active)
            {
                // Accumulate time from this active period.
                task.AccumulatedTime += DateTime.Now - task.CreatedAt;
                task.Status = TaskStatus.Paused;
                task.UpdatedAt = DateTime.Now;
                using (var context = new TimeTrackerContext())
                {
                    context.TaskItems.Update(task);
                    context.SaveChanges();
                }
                OnPropertyChanged(nameof(CurrentTaskElapsedTime));
            }
        }

        public void ResumeTask(TaskItem task)
        {
            if (task.Status == TaskStatus.Paused)
            {
                task.Status = TaskStatus.Active;
                // Mark the start of a new active period without losing the accumulated time.
                //task.CreatedAt = DateTime.Now;
                task.UpdatedAt = DateTime.Now;
                using (var context = new TimeTrackerContext())
                {
                    context.TaskItems.Update(task);
                    context.SaveChanges();
                }
                ActiveTask = task;
                OnPropertyChanged(nameof(CurrentTaskElapsedTime));
            }
        }

        public void ToggleTask()
        {
            // Use the selected task if available; otherwise, use the active task.
            TaskItem? taskToToggle = SelectedTask ?? ActiveTask;
            if (taskToToggle != null)
            {
                // If resuming a task and another task is active, pause the other one.
                if (taskToToggle.Status == TaskStatus.Paused && ActiveTask != null && ActiveTask != taskToToggle && ActiveTask.Status == TaskStatus.Active)
                {
                    PauseTask(ActiveTask);
                }

                if (taskToToggle.Status == TaskStatus.Active)
                {
                    PauseTask(taskToToggle);
                }
                else if (taskToToggle.Status == TaskStatus.Paused)
                {
                    ResumeTask(taskToToggle);
                }
                ((RelayCommand)ToggleTaskCommand).RaiseCanExecuteChanged();
            }
        }

        // Updates a task in the database.
        public void UpdateTask(TaskItem task)
        {
            task.UpdatedAt = DateTime.Now;
            using (var context = new TimeTrackerContext())
            {
                context.TaskItems.Update(task);
                context.SaveChanges();
            }
        }
    }
}