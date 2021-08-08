using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.EndPoints;
using dataneo.TutorialLibs.Domain.ValueObjects;
using dataneo.TutorialLibs.FileIO.Win.Services;
using dataneo.TutorialLibs.Persistence.EF.SQLite.Respositories;
using Serilog;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace dataneo.TutorialLibs.WPF.Actions
{
    internal class AddNewTutorialAction
    {
        private readonly Window _window;

        public AddNewTutorialAction(Window window)
        {
            this._window = window;
        }
        public async Task<Result> AddAsync()
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

            return await addTutorialEngine.AddTutorialAsync(directory.Value);
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
