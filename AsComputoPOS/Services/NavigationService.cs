using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using AsComputoPOS.ViewModels;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace AsComputoPOS.Services
{
    internal class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _currentViewModel = _serviceProvider.GetRequiredService<SecondPageViewModel>();
        }

        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            private set
            {
                _currentViewModel = value;
            }
        }

        public void NavigateTo<T>() where T : ViewModelBase
        {
            var viewModel = _serviceProvider.GetRequiredService<T>();
            CurrentViewModel = viewModel;
        }
    }
}
