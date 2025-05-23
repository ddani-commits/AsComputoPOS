using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsComputoPOS.ViewModels;
using AsComputoPOS.ViewModels.Category;
using AsComputoPOS.ViewModels.Inventory;
using AsComputoPOS.ViewModels.PointOfSale;
using AsComputoPOS.ViewModels.Products;
using AsComputoPOS.ViewModels.SalesHistory;
using AsComputoPOS.ViewModels.Suppliers;
using Microsoft.Extensions.DependencyInjection;

namespace AsComputoPOS.Services
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommonServices(this IServiceCollection collection)
        {
            collection.AddSingleton<INavigationService, NavigationService>();
            collection.AddSingleton<IAuthenticationService, AuthenticationService>();

            collection.AddSingleton<NavigationBarViewModel>();
            collection.AddTransient<EmployeesViewModel>();
            collection.AddTransient<SuppliersViewModel>();
            collection.AddTransient<SalesHistoryViewModel>();
            collection.AddTransient<ProductsViewModel>();
            collection.AddTransient<InventoryViewModel>();
            collection.AddTransient<PointOfSaleViewModel>();
            collection.AddTransient<LoginViewModel>();
            collection.AddTransient<RegisterViewModel>();
            collection.AddTransient<CategoryViewModel>();
            collection.AddTransient<MainWindowViewModel>();
        }

    }
}
