using dataneo.TutorialLibs.Domain.Interfaces;
using dataneo.TutorialLibs.Domain.Interfaces.Respositories;
using dataneo.TutorialLibs.Domain.Services;
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
            containerRegistry.Register<ApplicationDbContext>();
            containerRegistry.Register<ITutorialRespositoryAsync, TutorialRespositoryAsync>();
            containerRegistry.Register<ICategoryRespositoryAsync, CattegoryRespositoryAsync>();
            containerRegistry.Register<IDateTimeProivder, DateTimeProivder>();
            containerRegistry.Register<IFileScanner, FileScanner>();
            containerRegistry.Register<IHandledFileExtension, HandledFileExtension>();
            containerRegistry.Register<IMediaInfoProvider, MediaInfoProvider>();
        }
    }
}
