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
using TamoPOS.Models;
using Wpf.Ui.Controls;

namespace TamoPOS.Controls
{
    public partial class NewPurchaseOrderDialog : ContentDialog
    {
        private readonly Action<PurchaseOrder>? _savePurchaseOrder;
        public NewPurchaseOrderDialog(ContentPresenter? contentPresenter, Action<PurchaseOrder>? savePurchaseOrder = null) : base(contentPresenter)
        {
            InitializeComponent();
            _savePurchaseOrder =  savePurchaseOrder;
            //DataContext = this;
        }
    }
}
