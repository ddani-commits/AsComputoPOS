using System.Windows.Controls;
using TamoPOS.Models;
using Wpf.Ui.Controls;

namespace TamoPOS.Controls
{
    /// <summary>
    /// Lógica de interacción para NewProductPurchaseContentDialog.xaml
    /// </summary>
    public partial class NewProductPurchaseContentDialog : ContentDialog
    {
        private readonly Action<ProductPurchase>? _saveProductPurchase;
        private readonly ContentPresenter? _contentPresenter;

        public NewProductPurchaseContentDialog(ContentPresenter? contentPresenter, Action<ProductPurchase> saveProductPurchase) : base(contentPresenter)
        {
            _contentPresenter = contentPresenter;
            _saveProductPurchase = saveProductPurchase;
            InitializeComponent();
        }
    }
}
