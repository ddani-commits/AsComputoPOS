using System.Linq;
using AsComputoPOS.Data;
using AsComputoPOS.Services;
using AsComputoPOS.ViewModels;
using AsComputoPOS.Views;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;

namespace AsComputoPOS
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            using (var db = new ApplicationDbContext())
            {   
                // db.Database.EnsureDeleted(); // <- Sirve para borrar la base de datos, cada vez que se abre la aplicación.

                db.Database.EnsureCreated();
            }
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
                    // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
                    DisableAvaloniaDataAnnotationValidation();

                    var collection = new ServiceCollection();
                    collection.AddCommonServices();
                    var services = collection.BuildServiceProvider();

                    var vm = services.GetRequiredService<MainWindowViewModel>();
                    desktop.MainWindow = new MainWindow
                    {
                        //DataContext = new MainWindowViewModel(),
                        DataContext = vm,
                    };
                }

            base.OnFrameworkInitializationCompleted();
        }

        private void DisableAvaloniaDataAnnotationValidation()
        {
            // Get an array of plugins to remove
            var dataValidationPluginsToRemove =
                BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

            // remove each entry found
            foreach (var plugin in dataValidationPluginsToRemove)
            {
                BindingPlugins.DataValidators.Remove(plugin);
            }
        }
    }
}