using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Markup;
using TamoPOS.Data;
using TamoPOS.Models;
using TamoPOS.Services;
using Wpf.Ui.Controls;

namespace TamoPOS.Controls
{
    public partial class NewProductPurchaseContentDialog : ContentDialog, INotifyPropertyChanged
    {
        private readonly Action<ProductPurchase>? _saveProductPurchase;
        private readonly ContentPresenter? _contentPresenter;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IPoSPanelService _posPanelService;

        public event PropertyChangedEventHandler? PropertyChanged;

        // This code is very ugly. Hopefully someday i will know how to make it better
        private decimal _publicPrice;
        public decimal PublicPrice
        {
            get => _publicPrice;
            set
            {
                _publicPrice = value;
                _flatMargin = _publicPrice - _purchasePrice;
                _percentageMargin = _purchasePrice == 0 ? 0 : (_flatMargin / _purchasePrice) * 100m;
                OnPropertyChanged(nameof(PublicPrice));
                OnPropertyChanged(nameof(FlatMargin));
                OnPropertyChanged(nameof(PercentageMargin));
            }
        }

        private decimal _purchasePrice;
        public decimal PurchasePrice
        {
            get => _purchasePrice;
            set
            {
                _purchasePrice = value;
                RecalculateFromPurchasePrice();
                OnPropertyChanged(nameof(PurchasePrice));
            }
        }

        private decimal _flatMargin;
        public decimal FlatMargin
        {
            get => _flatMargin;
            set
            {
                _flatMargin = value;
                _percentageMargin = _purchasePrice == 0 ? 0 : (_flatMargin / _purchasePrice) * 100m;
                _publicPrice = _purchasePrice + _flatMargin;
                OnPropertyChanged(nameof(FlatMargin));
                OnPropertyChanged(nameof(PercentageMargin));
                OnPropertyChanged(nameof(PublicPrice));
            }
        }

        private decimal _percentageMargin;
        public decimal PercentageMargin
        {
            get => _percentageMargin;
            set
            {
                _percentageMargin = value;
                _flatMargin = _purchasePrice * _percentageMargin / 100m;
                _publicPrice = _purchasePrice + _flatMargin;
                OnPropertyChanged(nameof(PercentageMargin));
                OnPropertyChanged(nameof(FlatMargin));
                OnPropertyChanged(nameof(PublicPrice));
            }
        }

        private decimal _subtotal;
        public decimal Subtotal {
            get => _subtotal;
            set
            {
                _subtotal = value;
            }
        }

        private decimal _quantity;
        public decimal Quantity { 
            get => _quantity; 
            set
            {
                _quantity = value;
                decimal sub = _quantity * _purchasePrice;
                Subtotal = sub;
                OnPropertyChanged(nameof(Subtotal));
                OnPropertyChanged(nameof(SubtotalString));
            } 
        }

        private decimal _quantityRemaining;
        public decimal QuantityRemaining
        {
            get => _quantityRemaining;
            set
            {
                _quantityRemaining = value;
            }
        }

        public string SubtotalString => "$ " + _subtotal.ToString("N2");
             
        private void RecalculateFromPurchasePrice()
        {
            _flatMargin = _purchasePrice * _percentageMargin / 100m;
            _publicPrice = _purchasePrice + _flatMargin;
            _subtotal = _purchasePrice * _quantity;
            OnPropertyChanged(nameof(FlatMargin));
            OnPropertyChanged(nameof(PublicPrice));
            OnPropertyChanged(nameof(SubtotalString));
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public List<string> ProductsList { get; set; } = new List<string>();

        public NewProductPurchaseContentDialog(
            ApplicationDbContext dbContext,
            ContentPresenter? contentPresenter, 
            Action<ProductPurchase> saveProductPurchase,
            IPoSPanelService posPanelService
        ) : base(contentPresenter)
        {
            _contentPresenter = contentPresenter;
            _applicationDbContext = dbContext;
            _saveProductPurchase = saveProductPurchase;
            _posPanelService = posPanelService;
            InitializeComponent();
            DataContext = this;
        }

        protected override void OnButtonClick(ContentDialogButton button)
        {
            if (button == ContentDialogButton.Primary)
            {
                // todo: show feedback to tell the user to pick a product
                if (_selectedProduct is null) return;

                ProductPurchase productPurchase = new ProductPurchase
                {
                    Product = _selectedProduct,
                    UnitPrice = PurchasePrice,
                    Quantity = Quantity,
                    Subtotal = Subtotal,
                    Total = Subtotal,   
                    FlatProfitMargin = FlatMargin,
                    PercentProfitMargin = PercentageMargin / 100,
                    SalePrice = PublicPrice,
                    QuantityRemaining = QuantityRemaining
                };

                _saveProductPurchase?.Invoke(productPurchase);

                // just update the whole list, might hurt performance on long product lists
                if (productPurchase.QuantityRemaining > 0) _posPanelService.LoadProductsInStock();

                base.OnButtonClick(button);
                Debug.WriteLine("primary button clicked");
            }
            else if (button == ContentDialogButton.Secondary)
            {
                // Secondary operation
                ClearFields();
                Debug.WriteLine("Secondary button clicked");
            }
            else if (button == ContentDialogButton.Close)
            {
                // Close dialog without saving
                Debug.WriteLine("Cancel button clicked");
                base.OnButtonClick(button);
            }
        }

        public void ClearFields() { }

        private void ProductAutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var products = _applicationDbContext  
                    .Products
                    .Where(product => product.Name.Contains(sender.Text))
                    .ToList();
                ProductAutoSuggestBox.OriginalItemsSource = products;
            }
        }

        private Product _selectedProduct;
        private void ProductAutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if(args.SelectedItem is Product) _selectedProduct = args.SelectedItem as Product;
        }
    }
}
