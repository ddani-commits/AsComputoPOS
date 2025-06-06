using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using UiDesktopApp1.Views.Windows;
using Wpf.Ui;

namespace TamoPOS.Services
{
    public class PoSPanelService : IPoSPanelService
    {
        //private readonly MainWindow mainWindow = serviceProvider.GetService<INavigationWindow>() as MainWindow;
        private INavigationWindow? _navigationWindow;
        private MainWindow? _mainWindow;

        public PoSPanelService(){}

        public bool _isSidePanelExpanded = false;
        public bool IsSidePanelExpanded
        {
            get => _isSidePanelExpanded;
            set
            {
                _isSidePanelExpanded = value;
            }
        }

        public void CollapseSidePanel(IServiceProvider serviceProvider)
        {
            // Logic to collapse the side panel
            _mainWindow = serviceProvider.GetService<MainWindow>();
            if (_mainWindow == null)
            {
                Debug.WriteLine("SidePanelColumn is null, cannot collapse side panel.");
                return;
            }
            else
            {
                _mainWindow.SidePanelColumn.Width = new GridLength(0);
                IsSidePanelExpanded = false;
            }
            Debug.WriteLine("Side panel collapsed.");
        }

        public void ExpandSidePanel(IServiceProvider serviceProvider)
        {
            // Logic to expand the side panel
            Debug.WriteLine("Side panel expanded.");
            _mainWindow = serviceProvider.GetService<MainWindow>();
            _mainWindow.SidePanelColumn.Width = new GridLength(1, GridUnitType.Star);
            IsSidePanelExpanded = true;

        }
    }
}
