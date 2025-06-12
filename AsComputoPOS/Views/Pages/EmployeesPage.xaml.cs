using TamoPOS.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;

// You must declare new pages like this
namespace TamoPOS.Views.Pages
{
    public partial class EmployeesPage : INavigableView<EmployeesViewModel>
    {
        public EmployeesViewModel ViewModel { get; }
        public EmployeesPage(EmployeesViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();
        }
    }
}
