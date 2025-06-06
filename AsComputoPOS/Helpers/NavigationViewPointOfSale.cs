using System.Diagnostics;
using TamoPOS.Services;
using Wpf.Ui.Controls;

namespace TamoPOS.Helpers
{
    public class NavigationViewPointOfSale : NavigationViewItem
    {
        private IPoSPanelService _poSPanelService;
        private IServiceProvider _serviceProvider;

        public NavigationViewPointOfSale(IPoSPanelService posPanelService, IServiceProvider serviceProvider)
        { 
            _poSPanelService = posPanelService;
            _serviceProvider = serviceProvider;
        }
        protected override void OnClick()
        {
            base.OnClick();
            if(_poSPanelService.IsSidePanelExpanded)
            {
                Debug.WriteLine("PointOfSale clicked");
                _poSPanelService.CollapseSidePanel(_serviceProvider);
            } else
            {
                _poSPanelService.ExpandSidePanel(_serviceProvider);
                Debug.WriteLine("PoSPanelService is null, cannot expand side panel");
            }
        }
    }
}
