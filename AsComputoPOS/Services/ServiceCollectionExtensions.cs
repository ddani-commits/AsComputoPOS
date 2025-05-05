using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsComputoPOS.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace AsComputoPOS.Services
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommonServices(this IServiceCollection collection)
        {
            collection.AddSingleton<INavigationService, NavigationService>();
            collection.AddTransient<IAuthenticationService, AuthenticationService>();   
            collection.AddTransient<FirstPageViewModel>();
            collection.AddTransient<SecondPageViewModel>();
            collection.AddTransient<LoginViewModel>();
            collection.AddTransient<RegisterViewModel>();
            collection.AddTransient<MainWindowViewModel>();
        }

    }
}
