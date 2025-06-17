using System.Diagnostics;

namespace TamoPOS.ViewModels.Pages
{
    public partial class PurchaseOrderDetailViewModel: ViewModel
    {
        [ObservableProperty]
        private string _variable = "This is a placeholder variable for PurchaseOrderDetailViewModel.";

        [ObservableProperty]
        private int _id;

        public PurchaseOrderDetailViewModel() { }

        public void LoadDetails(int Id)
        {
            this.Id = Id;
            Debug.WriteLine("details id ", this.Id);
        }
    }
}