using System.Collections.ObjectModel;
using TamoPOS.Helpers;
using TamoPOS.Services;
using Wpf.Ui.Controls;

namespace UiDesktopApp1.ViewModels.Windows
{
    public partial class MainWindowViewModel(IPoSPanelService panelService, IServiceProvider serviceProvider) : ObservableObject
    {
        [ObservableProperty]
        private string _applicationTitle = "As Computo PoS";

        [ObservableProperty]
        private ObservableCollection<object> _menuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "Home",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                TargetPageType = typeof(Views.Pages.DashboardPage)
            },
            new NavigationViewItem()
            {
                Content = "Category",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Grid24 },
                TargetPageType = typeof(Views.Pages.CategoryPage)
            },
            new NavigationViewItem()
            {
                Content = "Employees",
                Icon = new SymbolIcon { Symbol = SymbolRegular.People24 },
                TargetPageType = typeof(Views.Pages.EmployeesPage)
            },
            new NavigationViewItem()
            {
                Content = "Inventory",
                Icon = new SymbolIcon { Symbol = SymbolRegular.ClipboardBulletListLtr20 },
                TargetPageType = typeof(Views.Pages.InventoryPage)
            },
            new NavigationViewPointOfSale(panelService, serviceProvider)
            {
                Content = "Point Of Sale",
                Icon = new SymbolIcon { Symbol = SymbolRegular.BarcodeScanner20 },
            },
            new NavigationViewItem()
            {
                Content = "Products",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Cube20},
                TargetPageType = typeof(Views.Pages.ProductsPage)
            },
            new NavigationViewItem()
            {
                Content = "Sales History",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Receipt20},
                TargetPageType = typeof(Views.Pages.SalesHistoryPage)
            },
            new NavigationViewItem()
            {
                Content = "Suppliers",
                Icon = new SymbolIcon { Symbol = SymbolRegular.VehicleTruckProfile20},
                TargetPageType = typeof(Views.Pages.SuppliersPage)
            }
        };

        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "Settings",
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
