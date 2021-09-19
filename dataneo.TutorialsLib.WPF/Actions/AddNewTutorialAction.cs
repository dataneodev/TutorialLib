using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Tutorials;
using dataneo.TutorialLibs.FileIO.Win.Services;
using Serilog;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dataneo.TutorialLibs.WPF.Actions
{
    internal class AddNewTutorialAction
    {
        private readonly ITutorialRespositoryAsync _tutorialRespositoryAsync;

        public AddNewTutorialAction(ITutorialRespositoryAsync tutorialRespositoryAsync)
        {
            this._tutorialRespositoryAsync = Guard.Against.Null(tutorialRespositoryAsync, nameof(tutorialRespositoryAsync));
        }

        public async Task<Result> AddAsync()
        {
            var directory = GetDirectoryUserSelect();
            if (directory.HasNoValue)
                return Result.Success();

            var addTutorialEngine = new AddTutorial(
                new FileScanner(),
                new MediaInfoProvider(),
                new HandledFileExtension(),
                this._tutorialRespositoryAsync,
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
