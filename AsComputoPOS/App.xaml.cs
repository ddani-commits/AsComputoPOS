using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Windows.Threading;
using TamoPOS.Services;
using TamoPOS.Data;
using TamoPOS.Services;
using TamoPOS.ViewModels.Pages;
using TamoPOS.ViewModels.Windows;
using TamoPOS.Views.Pages;
using TamoPOS.Views.Windows;
using Wpf.Ui;
using Wpf.Ui.DependencyInjection;

namespace TamoPOS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        // The.NET Generic Host provides dependency injection, configuration, logging, and other services.
        // https://docs.microsoft.com/dotnet/core/extensions/generic-host
        // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
        // https://docs.microsoft.com/dotnet/core/extensions/configuration
        // https://docs.microsoft.com/dotnet/core/extensions/logging
        private static readonly IHost _host = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(c => { c.SetBasePath(Path.GetDirectoryName(AppContext.BaseDirectory)); })
            .ConfigureServices((context, services) =>
            {
                services.AddNavigationViewPageProvider();

                services.AddHostedService<ApplicationHostService>();

                // Theme manipulation
                services.AddSingleton<IThemeService, ThemeService>();

                // TaskBar manipulation
                services.AddSingleton<ITaskBarService, TaskBarService>();

                // Service containing navigation, same as INavigationWindow... but without window
                services.AddSingleton<INavigationService, NavigationService>();

                services.AddSingleton<IContentDialogService, ContentDialogService>();

                // Main window with navigation
                //services.AddSingleton<INavigationWindow, MainWindow>();

                services.AddSingleton<MainWindow>();
                services.AddSingleton<INavigationWindow>(sp => sp.GetRequiredService<MainWindow>());

                services.AddSingleton<MainWindowViewModel>();

                services.AddSingleton<IAuthenticationService, AuthenticationService>();
                services.AddSingleton<IPoSPanelService, PoSPanelService>();

                // App UI Pages
                services.AddSingleton<AuthWindow>();
                services.AddSingleton<AuthWindowViewModel>();

                services.AddSingleton<SettingsPage>();
                services.AddSingleton<SettingsViewModel>();

                services.AddSingleton<CategoryPage>();
                services.AddSingleton<CategoryViewModel>(); 

                services.AddSingleton<EmployeesPage>();
                services.AddSingleton<EmployeesViewModel>();

                services.AddSingleton<InventoryPage>();
                services.AddSingleton<InventoryViewModel>();

                services.AddSingleton<PointOfSalePage>();
                services.AddSingleton<PointOfSaleViewModel>();

                services.AddSingleton<ProductsPage>();
                services.AddSingleton<ProductsViewModel>();

                services.AddSingleton<SalesHistoryPage>();
                services.AddSingleton<SalesHistoryViewModel>();

                services.AddSingleton<SuppliersPage>();
                services.AddSingleton<SuppliersViewModel>();
            }).Build();

        /// <summary>
        /// Gets services.
        /// </summary>
        public static IServiceProvider Services
        {
            get { return _host.Services; }
        }

        /// <summary>
        /// Occurs when the application is loading.
        /// </summary>
        private async void OnStartup(object sender, StartupEventArgs e)
        {
            // Starting the app before cleaning database
            // lead to inconsistencies
            using (var db = new ApplicationDbContext())
            {
                //db.Database.EnsureDeleted(); // <- Sirve para borrar la base de datos, cada vez que se abre la aplicación
                db.Database.EnsureCreated();
            }
            await _host.StartAsync();
        }

        /// <summary>
        /// Occurs when the application is closing.
        /// </summary>
        private async void OnExit(object sender, ExitEventArgs e)
        {
            await _host.StopAsync();

            _host.Dispose();
        }

        /// <summary>
        /// Occurs when an exception is thrown by an application but not handled.
        /// </summary>
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
        }
    }
}
