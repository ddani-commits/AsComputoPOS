using System.Collections.ObjectModel;
using TamoPOS.Helpers;
using TamoPOS.Services;
using Wpf.Ui.Controls;

namespace TamoPOS.ViewModels.Windows
{
    public partial class MainWindowViewModel(IPOSService panelService, IServiceProvider serviceProvider) : ViewModel
    {

        [ObservableProperty]
        private string _applicationTitle = "As Computo PoS";

        [ObservableProperty]
        private ObservableCollection<object> _menuItems =
        [
            new NavigationViewItem()
            {
                Content = "Punto de venta",
                Icon = new SymbolIcon { Symbol = SymbolRegular.BarcodeScanner20 },
                TargetPageType = typeof(Views.Pages.POSPage)
            },

            new NavigationViewItemSeparator(),

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
                    new NavigationViewItem()
                    {
                        Content = "Compras",
                        Icon = new SymbolIcon { Symbol = SymbolRegular.Clipboard20},
                        TargetPageType = typeof(Views.Pages.PurchaseOrdersPage),
                    },
                    new NavigationViewItem()
                    {
                        Content = "Productos",
                        Icon = new SymbolIcon { Symbol = SymbolRegular.Cube20},
                        TargetPageType = typeof(Views.Pages.ProductsPage),
                    },
                    new NavigationViewItem(){
                        Content = "Categorías",
                        Icon = new SymbolIcon { Symbol = SymbolRegular.Grid20   },
                        TargetPageType = typeof(Views.Pages.CategoryPage),
                    }
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
