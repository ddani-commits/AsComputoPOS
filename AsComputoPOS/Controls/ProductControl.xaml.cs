using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
using TamoPOS.Models;

namespace TamoPOS.Controls
{
    public partial class ProductControl : UserControl
    {
        // Cambia el tipo de la DependencyProperty 'Data' a ProductPurchase
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(ProductPurchase), typeof(ProductControl),
                new PropertyMetadata(null, OnDataChanged));

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                nameof(Command),
                typeof(ICommand),
                typeof(ProductControl),
                new PropertyMetadata(null)
            );

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(
                nameof(CommandParameter),
                typeof(object),
                typeof(ProductControl),
                new PropertyMetadata(null)
            );

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Command != null && Command.CanExecute(CommandParameter))
            {
                Command.Execute(CommandParameter);
            } else
            {
                Debug.WriteLine("cannot execute");
            }
        }
    }
}
