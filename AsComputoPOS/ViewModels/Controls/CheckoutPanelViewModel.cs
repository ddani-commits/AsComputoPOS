namespace TamoPOS.ViewModels.Controls
{
    public partial class CheckoutPanelViewModel: ViewModel
    {
        public List<string> Colors { get; } = new() { "Efectivo", "Debito/Credito" };
    }
}
