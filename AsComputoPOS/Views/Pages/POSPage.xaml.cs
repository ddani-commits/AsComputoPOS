using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Controls;
using TamoPOS.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;
using System.Windows.Input;
using TamoPOS.Models;

namespace TamoPOS.Views.Pages
{
    public partial class POSPage : INavigableView<POSPageViewModel>, INotifyPropertyChanged
    {
        public POSPageViewModel ViewModel { get; }
        Stopwatch keyTimer = new Stopwatch();
        List<long> keyIntervals = new List<long>();
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
            Loaded += (s, e) => ProductViewSearch.Focus();
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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.Filter(((TextBox)sender).Text);
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            bool isHuman = true;
            if (keyTimer.IsRunning)
            {
                keyTimer.Stop();
                keyIntervals.Add(keyTimer.ElapsedMilliseconds);

                // The minimun lenght a barcode can be is 8
                if (keyIntervals.Count > 8) keyIntervals.RemoveAt(0);

                double average = keyIntervals.Average();
                if (average < 15) isHuman = false;
                if (average > 15) isHuman = true;
            }

            keyTimer.Restart();

            if (e.Key == Key.Return && ViewModel.DisplayProducts.Cast<object>().Count() == 1)
            {
                var pp = ViewModel.DisplayProducts.Cast<ProductPurchase>().First();
                ViewModel.AddProductToCart(pp);
                if (!isHuman) ProductViewSearch.Clear();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
