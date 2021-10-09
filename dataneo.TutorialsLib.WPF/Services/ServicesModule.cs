using dataneo.TutorialLibs.Domain.Categories;
using dataneo.TutorialLibs.Domain.Tutorials;
using dataneo.TutorialLibs.FileIO.Win.Services;
using dataneo.TutorialLibs.Persistence.EF.SQLite.Context;
using dataneo.TutorialLibs.Persistence.EF.SQLite.Respositories;
using Prism.Ioc;
using Prism.Modularity;

namespace dataneo.TutorialLibs.WPF.Services
{
    public class ServicesModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ApplicationDbContext>();
            containerRegistry.RegisterSingleton<ITutorialRespositoryAsync, TutorialRespositoryAsync>();
            containerRegistry.RegisterSingleton<ICategoryRespositoryAsync, CattegoryRespositoryAsync>();
            containerRegistry.RegisterSingleton<IDateTimeProivder, DateTimeProivder>();
            containerRegistry.RegisterSingleton<IFileScanner, FileScanner>();
            containerRegistry.RegisterSingleton<IHandledFileExtension, HandledFileExtension>();
            containerRegistry.RegisterSingleton<IMediaInfoProvider, MediaInfoProvider>();
        }
    }
}
