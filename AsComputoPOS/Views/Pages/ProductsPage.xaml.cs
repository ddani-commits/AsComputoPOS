using System.ComponentModel;
using System.Diagnostics;
using TamoPOS.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;

namespace TamoPOS.Views.Pages
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
        public int Columns { get; set; } = 3;
        public ProductsPage(ProductsViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private void ItemsControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //int Columns = 1;
            double availableWidth = e.NewSize.Width;
            if (availableWidth > 581) Columns = 2;
            if (availableWidth > 870) Columns = 3;
            //if (availableWidth < 853) { columns = 2; } else { columns = 3; }

            int padding = 8 * 2; // Assuming 10px padding on each side

            ProductControlWidth = (availableWidth / Columns) ;
            Debug.WriteLine($"Available Width: {availableWidth}, ProductControlWidth: {ProductControlWidth}, Padding: {0}");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
