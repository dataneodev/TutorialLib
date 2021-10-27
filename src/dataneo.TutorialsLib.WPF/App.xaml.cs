using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.WPF.Services;
using dataneo.TutorialLibs.WPF.UI;
using dataneo.TutorialLibs.WPF.UI.Player.Services;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;

namespace dataneo.TutorialLibs.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            //   Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us"); // tests
            Result.DefaultConfigureAwait = true;
            LibVLCSharp.Shared.Core.Initialize();
            VLCLoader.Init();
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ServicesModule>();
            moduleCatalog.AddModule<UIModule>();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            System.Windows.MessageBox.Show(string.Format("An error occured: {0}", e.Exception.Message), "Error");
            e.Handled = true;
        }
    }
}
