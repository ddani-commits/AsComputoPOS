namespace TamoPOS.Services
{
    public interface IPoSPanelService
    {
        void CollapseSidePanel(IServiceProvider serviceProvider);
        void ExpandSidePanel(IServiceProvider serviceProvider);
        bool IsSidePanelExpanded { get; set; }
    }
}
