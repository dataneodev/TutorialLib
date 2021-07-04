using dataneo.TutorialLibs.Persistence.EF.SQLite.Context;
using System.Windows;

namespace dataneo.TutorialsLib.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {

            using var dbContext = new ApplicationDbContext();
            dbContext.Database.EnsureCreated();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {

        }
    }
}
