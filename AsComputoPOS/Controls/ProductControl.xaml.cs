using System.Diagnostics;
using System.Windows.Controls;
using TamoPOS.Models;

namespace TamoPOS.Controls
{
    public partial class ProductControl : UserControl
    {
        // Cambia el tipo de la DependencyProperty 'Data' a ProductPurchase
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(ProductPurchase), typeof(ProductControl),
                new PropertyMetadata(null, OnDataChanged));

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ProductControl control && e.NewValue is ProductPurchase productPurchase)
            {
                control.DataContext = productPurchase;
            }
        }

        public ProductPurchase Data
        {
            get => (ProductPurchase)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }
        public ProductControl()
        {
            InitializeComponent();
        }
    }
}
