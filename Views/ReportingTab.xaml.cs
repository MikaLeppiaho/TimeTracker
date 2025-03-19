using System.Windows.Controls;
using TimeTracker.ViewModels;

namespace TimeTracker.Views
{
    /// <summary>
    /// Interaction logic for ReportingTab.xaml
    /// </summary>
    public partial class ReportingTab : UserControl
    {
        public ReportingTab()
        {
            InitializeComponent();
            DataContext = new ReportingViewModel();
        }
    }
}
