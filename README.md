# TimeTracker

TimeTracker is a WPF application for managing customers, projects, tasks, and time entries. It uses Entity Framework Core with SQLite as the database provider.

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) with the following workloads:
  - .NET Desktop Development
  - Data storage and processing (for Entity Framework tools)
- SQLite (included with the Entity Framework Core SQLite package)

## Getting Started

1. **Clone the repository:**
git clone https://github.com/yourusername/TimeTracker.git
cd TimeTracker


2. **Open the solution in Visual Studio:**

    Open `TimeTracker.sln` in Visual Studio 2022.

3. **Restore NuGet packages:**

    Visual Studio should automatically restore the required NuGet packages. If not, you can restore them manually by right-clicking on the solution in Solution Explorer and selecting `Restore NuGet Packages`.

4. **Build the solution:**

    Build the solution by pressing `Ctrl+Shift+B` or by selecting `Build > Build Solution` from the menu.

5. **Run the application:**

    Press `F5` to run the application or select `Debug > Start Debugging` from the menu.

## Database Setup

The application uses SQLite as the database provider. The database file (`TimeTracker.db`) will be created automatically in the project directory when the application is run for the first time.

### Ensuring Database Creation

The database is ensured to be created in the `OnStartup` method 
protected override void OnStartup(StartupEventArgs e) {
	base.OnStartup(e);
	using (var context = new TimeTrackerContext()) {
		context.Database.EnsureCreated();
	} 
}

## Project Structure

- **Data**: Contains the `TimeTrackerContext` class which is the Entity Framework Core DbContext.
- **Models**: Contains the model classes for `Customer`, `Project`, `TaskItem`, and `TimeEntry`.
- **ViewModels**: Contains the ViewModel classes for the MVVM pattern.
- **Views**: Contains the XAML files for the WPF views.
- **Migrations**: Contains the Entity Framework Core migrations (if any).

