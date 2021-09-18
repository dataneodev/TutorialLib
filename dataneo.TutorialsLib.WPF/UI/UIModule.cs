using dataneo.TutorialLibs.WPF.UI.CategoryManage;
using dataneo.TutorialLibs.WPF.UI.Player;
using dataneo.TutorialLibs.WPF.UI.TutorialList;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace dataneo.TutorialLibs.WPF.UI
{
    public class UIModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RequestNavigate(RegionNames.ContentRegion, nameof(TutorialListPage));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<PlayerPage>();
            containerRegistry.RegisterForNavigation<TutorialListPage>();
            containerRegistry.RegisterDialog<CategoryWindow>();
        }
    }
}
