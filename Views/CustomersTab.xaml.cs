using System.Windows.Controls;
using TimeTracker.ViewModels;

namespace TimeTracker.Views
{
    /// <summary>
    /// Interaction logic for CustomersTab.xaml
    /// </summary>
    public partial class CustomersTab : UserControl
    {
        public CustomersTab()
        {
            InitializeComponent();
            DataContext = new CustomersTabViewModel();
        }
    }
}
