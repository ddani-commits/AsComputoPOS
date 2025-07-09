using System.Diagnostics;
using System.Windows.Controls;
using TamoPOS.ViewModels.Controls;

namespace TamoPOS.Controls.PointOfSalePanel
{
    public partial class CheckoutPanel : UserControl
    {
        public CheckoutPanelViewModel? ViewModel;
        public CheckoutPanel()
        {
            InitializeComponent();
        }

        public void SetViewModel(CheckoutPanelViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
        }

        private void Edit_Checked(object sender, RoutedEventArgs e)
        {
            Edit.Content = "Finalizar";
            ViewModel.IsEditing = true;
        }

        private void Edit_Unchecked(object sender, RoutedEventArgs e)
        {
            Edit.Content = "Editar";
            ViewModel.IsEditing = false;
        }

        private void Edit_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if((bool)e.NewValue == false){
                Debug.WriteLine("Edit button visibility changed to false, resetting state.");
                Edit.IsChecked = false;
                ViewModel.IsEditing = false;
            }
        }
    }
}
