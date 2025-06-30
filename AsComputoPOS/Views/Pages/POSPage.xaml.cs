using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Controls;
using TamoPOS.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;

namespace TamoPOS.Views.Pages
{
    public partial class POSPage : INavigableView<POSPageViewModel>, INotifyPropertyChanged
    {
        public POSPageViewModel ViewModel { get; }
        public int Columns { get; set; } = 2;
        public double _productControlWidth;
        public double ProductControlWidth
        {
            get => _productControlWidth;
            set
            {
                _productControlWidth = value;
                OnPropertyChanged(nameof(ProductControlWidth));
            }
        }

        public POSPage(POSPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();
        }

        private void ItemsControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double availableWidth = e.NewSize.Width;
            double space = 4;
            if (availableWidth > 581) Columns = 2;
            if (availableWidth > 870)
            {
                Columns = 3;
                space = 2.5;
            }
            if (availableWidth < 549) Columns = 1;

            ProductControlWidth = (availableWidth / Columns) + space; // add the padding or something idk
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Debug.WriteLine(((TextBox)sender).Text);
            ViewModel.SearchProductByName(((TextBox)sender).Text);
        }
    }
}
