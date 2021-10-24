using dataneo.TutorialLibs.Domain.Categories;
using dataneo.TutorialLibs.Domain.Settings;
using dataneo.TutorialLibs.Domain.Tutorials;
using dataneo.TutorialLibs.FileIO.Win.Services;
using dataneo.TutorialLibs.Persistence.EF.SQLite.Context;
using dataneo.TutorialLibs.Persistence.EF.SQLite.Respositories;
using dataneo.TutorialLibs.WPF.UI.Dialogs;
using Prism.Ioc;
using Prism.Modularity;

namespace dataneo.TutorialLibs.WPF.Services
{
    public class ServicesModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider) { }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ApplicationDbContext>();
            containerRegistry.Register<ITutorialRespositoryAsync, TutorialRespositoryAsync>();
            containerRegistry.Register<ICategoryRespositoryAsync, CattegoryRespositoryAsync>();
            containerRegistry.Register<ISettingRespositoryAsync, SettingRespositoryAsync>();
            containerRegistry.Register<ISettingManager, SettingManager>();
            containerRegistry.RegisterSingleton<IDateTimeProivder, DateTimeProivder>();
            containerRegistry.RegisterSingleton<IHandledFileExtension, HandledFileExtension>();
            containerRegistry.Register<IFileScanner, FileScanner>();
            containerRegistry.Register<IMediaInfoProvider, MediaInfoProvider>();
            containerRegistry.Register<ILogger, LoggerDialog>();
            containerRegistry.Register<IAddTutorial, AddTutorial>();
        }
    }
}
