using System.Windows.Controls;
using TamoPOS.Services;
using Wpf.Ui.Controls;

namespace TamoPOS.Controls.PointOfSalePanel
{
    public partial class CheckoutContentDialog : ContentDialog
    {
        private readonly IPOSService _posPanelService;
        private ContentPresenter? _contentPresenter;

        public CheckoutContentDialog
        (
            ContentPresenter? contentPresenter, 
            IPOSService posPanelService
        ) : base(contentPresenter)
        {
            InitializeComponent();
            DataContext = this;
            _posPanelService = posPanelService;
            _contentPresenter = contentPresenter;
        }
    }
}
