using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using TamoPOS.Data;
using TamoPOS.Models;
using TamoPOS.Views.Windows;
using Wpf.Ui;

namespace TamoPOS.Services
{
    public class PoSPanelService : IPoSPanelService
    {
        private INavigationWindow? _navigationWindow;
        private MainWindow? _mainWindow;
        public ObservableCollection<Product> ProductsInStock { get; set; } = new();
        private readonly ApplicationDbContext _appDbContext = new();

        public PoSPanelService(){}

        public bool _isSidePanelExpanded = false;
        public bool IsSidePanelExpanded
        {
            get => _isSidePanelExpanded;
            set
            {
                _isSidePanelExpanded = value;
            }
        }

        public void LoadProductsInStock()
        {
            ProductsInStock.Clear();
            var productPurchases = _appDbContext.ProductPurchases
                .Where(productPurchase => productPurchase.QuantityRemaining > 0)
                .GroupBy(productPurchase => productPurchase.ProductId)
                .Select(g => g.OrderBy(pp => (double)pp.QuantityRemaining!).First().Product)
                .ToList();

            foreach (var productPurchase in productPurchases)
            {
                ProductsInStock.Add(productPurchase);
            }
        }

        public void CollapseSidePanel(IServiceProvider serviceProvider)
        {
            _mainWindow = serviceProvider.GetService<MainWindow>();
            if (_mainWindow == null) return;
            else
            {
                _mainWindow.SidePanelColumn.Width = new GridLength(0);
                IsSidePanelExpanded = false;
            }
        }

        public void ExpandSidePanel(IServiceProvider serviceProvider)
        {
            _mainWindow = serviceProvider.GetService<MainWindow>();
            _mainWindow.SidePanelColumn.Width = new GridLength(1, GridUnitType.Star);
            IsSidePanelExpanded = true;
        }
    }
}
