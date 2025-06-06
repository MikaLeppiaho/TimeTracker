[[_TOC_]]

# Introduction

<!-- User to fill this section -->

# Functional description

The TimeTracker application is a WPF-based solution designed to manage customers, projects, tasks, and time entries. It provides a user-friendly interface for tracking time and managing tasks efficiently. Below is a description of its key features:

## Features

### Home Tab

-   **Projects List**: Displays active projects.
-   **Today's Tasks**: Lists tasks created for the current day.
-   **Task Management**:
    -   Start a new task for a selected project.
    -   Pause or resume tasks.
    -   View elapsed time for tasks.

### Customers Tab

-   **Customer Management**:
    -   Add new customers with name and contact information.
    -   View a list of existing customers.
-   **Project Management**:
    -   Add projects for a selected customer.
    -   Specify project details such as name, description, allocated time, and status.
    -   View projects associated with a selected customer.

### Reporting Tab

-   **Weekly Report**:
    -   Displays tasks created during the current week.
    -   Shows task details such as date, name, description, status, and elapsed time.

# Technical description

The application is built using the MVVM (Model-View-ViewModel) pattern and utilizes the following technologies:

## Tech Stack

-   **Frontend**: WPF (Windows Presentation Foundation)
-   **Backend**: Entity Framework Core with SQLite as the database provider
-   **Programming Language**: C#

## Architecture

The application consists of the following components:

-   **Models**: Define the data structure for customers, projects, tasks, and time entries.
-   **ViewModels**: Handle the business logic and data binding for the views.
-   **Views**: Provide the user interface using XAML.
-   **Data**: Contains the `TimeTrackerContext` class for database interactions.

## Data Flow

::: mermaid
graph LR
User[User] -->|Interacts with| Views
Views -->|Binds to| ViewModels
ViewModels -->|Accesses| Models
Models -->|Persisted by| Data[Database]
:::

# Installation instructions

## Prerequisites

-   [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
-   [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) with the following workloads:
    -   .NET Desktop Development
    -   Data storage and processing

## Steps

1. Clone the repository:
    ```bash
    git clone https://github.com/yourusername/TimeTracker.git
    cd TimeTracker
    ```
2. Open the solution in Visual Studio:
    - Open `TimeTracker.sln`.
3. Restore NuGet packages:
    - Right-click on the solution in Solution Explorer and select `Restore NuGet Packages`.
4. Build the solution:
    - Press `Ctrl+Shift+B` or select `Build > Build Solution`.
5. Run the application:
    - Press `F5` or select `Debug > Start Debugging`.

## Database Setup

The database file (`TimeTracker.db`) is created automatically in the project directory when the application is run for the first time. The `OnStartup` method in `App.xaml.cs` ensures the database is created:

```csharp
protected override void OnStartup(StartupEventArgs e)
{
    base.OnStartup(e);
    using (var context = new TimeTrackerContext())
    {
        context.Database.EnsureCreated();
    }
}
```

# User Manual

## Home Tab

The Home Tab is the central hub for managing daily tasks and projects. It provides an overview of active projects and tasks for the current day.

1. **Projects List**:
    - Displays all active projects.
    - Select a project to view its associated tasks.
2. **Today's Tasks**:
    - Lists tasks created for the current day.
    - Provides details such as task name, description, status, and elapsed time.
3. **Task Management**:
    - **Start Task**: Begin a new task for the selected project. This will automatically pause any currently active task.
    - **Toggle Pause/Resume**: Pause an active task or resume a paused task.
    - **Elapsed Time**: View the total time spent on a task, including the current session if the task is active.

## Customers Tab

The Customers Tab allows you to manage customer information and their associated projects. It is divided into two sections: Customer Management and Project Management.

1. **Customer Management**:
    - Add new customers by entering their name and contact information, then clicking **Add Customer**.
    - View a list of existing customers.
    - Select a customer to manage their projects.
2. **Project Management**:
    - Add new projects for the selected customer by providing details such as name, description, allocated time, and status.
    - View a list of projects associated with the selected customer.
    - Projects can be marked as active or passive, depending on their current status.

## Reporting Tab

The Reporting Tab provides a weekly overview of tasks, helping you analyze productivity and track progress.

1. **Weekly Report**:
    - Displays tasks created during the current week.
    - Shows task details such as date, name, description, status, and elapsed time.
    - The report is automatically updated to reflect the current week.
2. **Navigation**:
    - Future updates may include navigation buttons to view reports for previous or upcoming weeks.
