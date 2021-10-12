using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Tutorials;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dataneo.TutorialLibs.WPF.Actions
{
    internal class AddNewTutorialAction
    {
        private readonly IAddTutorial _addTutorial;

        public AddNewTutorialAction(IAddTutorial addTutorial)
        {
            this._addTutorial = Guard.Against.Null(addTutorial, nameof(addTutorial));
        }

        public async Task<Result> AddAsync()
        {
            var directory = GetDirectoryUserSelect();
            if (directory.HasNoValue)
                return Result.Success();
            return await _addTutorial.AddTutorialAsync(directory.Value);
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
