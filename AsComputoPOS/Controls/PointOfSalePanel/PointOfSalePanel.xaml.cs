using System.Windows.Controls;

namespace TamoPOS.Controls.PointOfSalePanel
{
    public partial class PointOfSalePanel : UserControl
    {
        public List<string> Colors { get; } = new() { "Efectivo", "Debito/Credito"};
        public PointOfSalePanel()
        {
            InitializeComponent();
            DataContext = this;
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
