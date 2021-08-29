using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Persistence.EF.SQLite.Context;
using System.Windows;

namespace dataneo.TutorialLibs.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            //   Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us"); // tests
            Result.DefaultConfigureAwait = true;
            using var dbContext = new ApplicationDbContext();
            dbContext.Database.EnsureCreated();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            System.Windows.MessageBox.Show(string.Format("An error occured: {0}", e.Exception.Message), "Error");
            e.Handled = true;
        }
    }
}
