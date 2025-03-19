using System.Configuration;
using System.Data;
using System.Windows;
using TimeTracker.Data;

namespace TimeTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            using (var context = new TimeTrackerContext())
            {
                context.Database.EnsureCreated();
            }
        }
    }

}
