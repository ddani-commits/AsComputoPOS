using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using AsComputoPOS.ViewModels;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;

namespace AsComputoPOS.Services
{
    internal class NavigationService : INavigationService, INotifyPropertyChanged
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
                if(_currentViewModel != value)
                {
                _currentViewModel = value;
                    OnPropertyChanged(nameof(CurrentViewModel));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void NavigateTo<T>() where T : ViewModelBase
        {
            var viewModel = _serviceProvider.GetRequiredService<T>();
            CurrentViewModel = viewModel;
        }
    }
}
