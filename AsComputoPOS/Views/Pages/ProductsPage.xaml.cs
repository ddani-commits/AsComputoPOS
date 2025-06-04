using System.ComponentModel;
using System.Diagnostics;
using UiDesktopApp1.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;

namespace UiDesktopApp1.Views.Pages
{
    public partial class ProductsPage : INavigableView<ProductsViewModel>, INotifyPropertyChanged
    {
        public ProductsViewModel ViewModel { get; }
        public double _productControlWidth;
        public double ProductControlWidth {
            get => _productControlWidth;
            set
            {
                _productControlWidth = value;
                OnPropertyChanged(nameof(ProductControlWidth));
            }
        }
        public ProductsPage(ProductsViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private void ItemsControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            int columns;
            double availableWidth = e.NewSize.Width;

            if (availableWidth < 853) { columns = 2; } else { columns = 3; }

            int padding = 10 * 2; // Assuming 10px padding on each side

            ProductControlWidth = (availableWidth / columns) - padding;
            Debug.WriteLine($"Available Width: {availableWidth}, ProductControlWidth: {ProductControlWidth}");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
