using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using TamoPOS.Services;
using TamoPOS.Views.Windows;
using Wpf.Ui.Abstractions.Controls;
using Wpf.Ui.Appearance;

namespace TamoPOS.ViewModels.Pages
{
    public partial class SettingsViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private string _appVersion = String.Empty;

        [ObservableProperty]
        private ApplicationTheme _currentTheme = ApplicationTheme.Unknown;

        public Task OnNavigatedToAsync()
        {
            if (!_isInitialized)
                InitializeViewModel();

            return Task.CompletedTask;
        }

        public Task OnNavigatedFromAsync() => Task.CompletedTask;

        private void InitializeViewModel()
        {
            CurrentTheme = ApplicationThemeManager.GetAppTheme();
            AppVersion = $"TamoPOS - {GetAssemblyVersion()}";

            _isInitialized = true;
        }

        private string GetAssemblyVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString()
                ?? String.Empty;
        }

        [RelayCommand]
        private void OnChangeTheme(string parameter)
        {
            switch (parameter)
            {
                case "theme_light":
                    if (CurrentTheme == ApplicationTheme.Light)
                        break;

                    ApplicationThemeManager.Apply(ApplicationTheme.Light);
                    CurrentTheme = ApplicationTheme.Light;

                    break;

                default:
                    if (CurrentTheme == ApplicationTheme.Dark)
                        break;

                    ApplicationThemeManager.Apply(ApplicationTheme.Dark);
                    CurrentTheme = ApplicationTheme.Dark;

                    break;
            }
        }

        private static void RestartApplication()
        {
            string exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName; 
            if(!string.IsNullOrEmpty(exePath))
            {
                System.Diagnostics.Process.Start(exePath);
                System.Windows.Application.Current.Shutdown();
            }
        }

        [RelayCommand]
        private void RestartApp()
        {
            RestartApplication();
        }
    }
}
