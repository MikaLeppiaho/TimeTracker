using System.Windows;
using System.Windows.Controls;
using TimeTracker.ViewModels;

namespace TimeTracker.Views
{
    public partial class HomeTab : UserControl
    {
        public HomeTab()
        {
            InitializeComponent();
            DataContext = new HomeTabViewModel();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is HomeTabViewModel viewModel)
            {
                viewModel.RefreshData();
            }
        }
    }
}