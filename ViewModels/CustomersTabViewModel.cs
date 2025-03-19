using System.Collections.ObjectModel;
using System.Windows.Input;
using TimeTracker.Common;
using TimeTracker.Data;
using TimeTracker.Models;

namespace TimeTracker.ViewModels
{
    public class CustomersTabViewModel : BaseViewModel
    {
        // Existing customer properties
        public ObservableCollection<Customer> Customers { get; set; } = new ObservableCollection<Customer>();

        private string _newCustomerName = string.Empty;
        public string NewCustomerName
        {
            get => _newCustomerName;
            set
            {
                _newCustomerName = value;
                OnPropertyChanged();
                ((RelayCommand)AddCustomerCommand).RaiseCanExecuteChanged();
            }
        }

        private string _newCustomerContact = string.Empty;
        public string NewCustomerContact
        {
            get => _newCustomerContact;
            set
            {
                _newCustomerContact = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddCustomerCommand { get; }

        // New properties for project management
        private Customer? _selectedCustomer;
        public Customer? SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged();
                LoadProjects();
            }
        }

        public ObservableCollection<Project> Projects { get; set; } = new ObservableCollection<Project>();

        private string _newProjectName = string.Empty;
        public string NewProjectName
        {
            get => _newProjectName;
            set
            {
                _newProjectName = value;
                OnPropertyChanged();
                ((RelayCommand)AddProjectCommand).RaiseCanExecuteChanged();
            }
        }

        private string _newProjectDescription = string.Empty;
        public string NewProjectDescription
        {
            get => _newProjectDescription;
            set
            {
                _newProjectDescription = value;
                OnPropertyChanged();
            }
        }

        // Enter allocated time as a string in "hh:mm" format (optional)
        private string _newProjectAllocatedTime = string.Empty;
        public string NewProjectAllocatedTime
        {
            get => _newProjectAllocatedTime;
            set
            {
                _newProjectAllocatedTime = value;
                OnPropertyChanged();
            }
        }

        // New property for the project status
        private ProjectStatus _newProjectStatus = ProjectStatus.Passive;
        public ProjectStatus NewProjectStatus
        {
            get => _newProjectStatus;
            set
            {
                _newProjectStatus = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddProjectCommand { get; }

        public CustomersTabViewModel()
        {
            AddCustomerCommand = new RelayCommand(AddCustomer, CanAddCustomer);
            AddProjectCommand = new RelayCommand(AddProject, CanAddProject);
            LoadCustomers();
        }

        private bool CanAddCustomer()
        {
            return !string.IsNullOrWhiteSpace(NewCustomerName);
        }

        private void AddCustomer()
        {
            var customer = new Customer
            {
                Name = NewCustomerName,
                ContactInfo = NewCustomerContact,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            using (var context = new TimeTrackerContext())
            {
                context.Customers.Add(customer);
                context.SaveChanges();
            }

            NewCustomerName = string.Empty;
            NewCustomerContact = string.Empty;
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            using (var context = new TimeTrackerContext())
            {
                var customersList = context.Customers.ToList();
                Customers.Clear();
                foreach (var customer in customersList)
                {
                    Customers.Add(customer);
                }
            }
        }

        // Project management methods

        private bool CanAddProject()
        {
            // Allow adding project only if a customer is selected and a project name is provided.
            return SelectedCustomer != null && !string.IsNullOrWhiteSpace(NewProjectName);
        }

        private void AddProject()
        {
            TimeSpan? allocatedTime = null;
            if (!string.IsNullOrWhiteSpace(NewProjectAllocatedTime))
            {
                if (TimeSpan.TryParse(NewProjectAllocatedTime, out TimeSpan result))
                {
                    allocatedTime = result;
                }
            }

            var project = new Project
            {
                CustomerId = SelectedCustomer!.CustomerId,
                Name = NewProjectName,
                Description = NewProjectDescription,
                AllocatedTime = allocatedTime,
                Status = NewProjectStatus,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            using (var context = new TimeTrackerContext())
            {
                context.Projects.Add(project);
                context.SaveChanges();
            }

            NewProjectName = string.Empty;
            NewProjectDescription = string.Empty;
            NewProjectAllocatedTime = string.Empty;
            NewProjectStatus = ProjectStatus.Passive;

            LoadProjects();
        }

        private void LoadProjects()
        {
            Projects.Clear();
            if (SelectedCustomer == null)
                return;

            using (var context = new TimeTrackerContext())
            {
                var projectsList = context.Projects
                    .Where(p => p.CustomerId == SelectedCustomer.CustomerId)
                    .ToList();
                foreach (var project in projectsList)
                {
                    Projects.Add(project);
                }
            }
        }
    }
}
