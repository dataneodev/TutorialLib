using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Services;
using dataneo.TutorialLibs.Domain.ValueObjects;
using dataneo.TutorialLibs.FileIO.Win.Services;
using dataneo.TutorialLibs.Persistence.EF.SQLite.Respositories;
using Serilog;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dataneo.TutorialsLib.WPF.Actions
{
    internal static class AddNewTutorial
    {
        public static async Task<Result> AddAsync()
        {
            var directory = GetDirectoryUserSelect();
            if (directory.HasNoValue)
                return Result.Success();

            using var repo = new TutorialRespositoryAsync();
            var addTutorialEngine = new AddTutorial(
                new FileScanner(),
                new MediaInfoProvider(),
                new HandledFileExtension(),
                repo,
                Log.Logger);

            var res = await addTutorialEngine.AddTutorialAsync(directory.Value);
            return res;
        }

        private static Maybe<DirectoryPath> GetDirectoryUserSelect()
        {
            var dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            var directory = DirectoryPath.Create(dialog.SelectedPath);
            return directory.IsSuccess ?
                Maybe<DirectoryPath>.From(directory.Value) :
                Maybe<DirectoryPath>.None;
        }
    }
}
