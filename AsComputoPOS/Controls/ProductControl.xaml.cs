using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UiDesktopApp1.Models;

namespace UiDesktopApp1.Controls
{
    /// <summary>
    /// Lógica de interacción para ProductControl.xaml
    /// </summary>
    public partial class ProductControl : UserControl
    {
                // Define the DependencyProperty 'Data'
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(Product), typeof(ProductControl),
                new PropertyMetadata(null, OnDataChanged));

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ProductControl control && e.NewValue is Product product)
            {
                control.DataContext = product;
            }
        }

        public Product Data
        {
            get => (Product)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }
        public ProductControl()
        {
            InitializeComponent();
        }
    }
}
