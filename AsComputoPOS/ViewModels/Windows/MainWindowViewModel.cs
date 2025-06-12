using System.Collections.ObjectModel;
using TamoPOS.Helpers;
using TamoPOS.Services;
using Wpf.Ui.Controls;

namespace TamoPOS.ViewModels.Windows
{
    public partial class MainWindowViewModel(IPoSPanelService panelService, IServiceProvider serviceProvider) : ViewModel
    {
        
        [ObservableProperty]
        private string _applicationTitle = "As Computo PoS";

        [ObservableProperty]
        private ObservableCollection<object> _menuItems =
        [
            new NavigationViewPointOfSale(panelService, serviceProvider)
            {
                Content = "Punto de venta",
                Icon = new SymbolIcon { Symbol = SymbolRegular.BarcodeScanner20 },
            },

            new NavigationViewItemSeparator(),
            new NavigationViewItem()
            {
                Content = "Productos",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Cube20},
                TargetPageType = typeof(Views.Pages.ProductsPage)
            },
            new NavigationViewItem()
            {
                Content = "Categorías",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Grid24 },
                TargetPageType = typeof(Views.Pages.CategoryPage)
            },
            new NavigationViewItem()
            {
                Content = "Empleados",
                Icon = new SymbolIcon { Symbol = SymbolRegular.People24 },
                TargetPageType = typeof(Views.Pages.EmployeesPage)
            },
            new NavigationViewItem()
            {
                Content = "Inventario",
                Icon = new SymbolIcon { Symbol = SymbolRegular.ClipboardBulletListLtr20 },
                MenuItemsSource = new object[]
                {
                    new NavigationViewItem("Ordenes de compra", typeof(Views.Pages.CategoryPage)),
                    new NavigationViewItem("Existencias", typeof(Views.Pages.SettingsPage))
                }
            },

            new NavigationViewItem()
            {
                Content = "Ventas",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Receipt20},
                TargetPageType = typeof(Views.Pages.SalesHistoryPage)
            },
            new NavigationViewItem()
            {
                Content = "Proveedores",
                Icon = new SymbolIcon { Symbol = SymbolRegular.VehicleTruckProfile20},
                TargetPageType = typeof(Views.Pages.SuppliersPage)
            }
        ];

        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "Ajustes",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(Views.Pages.SettingsPage)
            }
        };

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new()
        {
            new MenuItem { Header = "Home", Tag = "tray_home" }
        };
    }
}
