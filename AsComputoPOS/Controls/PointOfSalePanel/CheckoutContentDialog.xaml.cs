using System.ComponentModel;
using System.Windows.Controls;
using TamoPOS.Services;
using Wpf.Ui.Controls;

namespace TamoPOS.Controls.PointOfSalePanel
{
    public partial class CheckoutContentDialog : ContentDialog, INotifyPropertyChanged
    {
        private readonly IPOSService _posPanelService;
        public event PropertyChangedEventHandler? PropertyChanged;

        // Money to return if customer pays more than the price of the products
        private decimal _changeDue = 0;
        public decimal ChangeDue
        {
            get => _changeDue;
            set
            {
                if (_changeDue != value)
                {
                    _changeDue = value;
                    OnPropertyChanged(nameof(ChangeDue));
                }
            }
        }

        public decimal Total => _posPanelService.Total;
        public decimal CashPayment; // the money paid by the customer

        public CheckoutContentDialog
        (
            ContentPresenter? contentPresenter,
            IPOSService posPanelService
        ) : base(contentPresenter)
        {
            InitializeComponent();
            DataContext = this;
            _posPanelService = posPanelService;
            Title = "Introduce el pago del cliente";
            Loaded += (s, e) => MoneyInput.Focus();
        }

        private void MoneyInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeDue = CashPayment - _posPanelService.Total;
        }

        private void ChangeDueTextBlock_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsTextNumeric(MoneyInput.Text + e.Text);
        }

        private void MoneyInput_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string pastedText = (string)e.DataObject.GetData(typeof(string));
                if (!IsTextNumeric(pastedText))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private bool IsTextNumeric(string text)
        {
            if (string.IsNullOrEmpty(text)) return true;

            if (text == ".") return true;

            if (CountChar(text, '.') > 1) return false;

            if (text.Contains('-')) return false;

            bool parsed = decimal.TryParse(text, out decimal value) && value >= 0;

            if (parsed) CashPayment = value;

            return parsed;
        }

        private int CountChar(string input, char c)
            => input.Count(ch => ch == c);

        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
