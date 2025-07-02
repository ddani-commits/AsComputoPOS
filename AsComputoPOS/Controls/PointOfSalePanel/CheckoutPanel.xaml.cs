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
        }

        private void Edit_Unchecked(object sender, RoutedEventArgs e)
        {
            Edit.Content = "Editar";
        }
    }
}
