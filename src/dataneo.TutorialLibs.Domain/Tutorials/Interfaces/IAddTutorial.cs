using CSharpFunctionalExtensions;
using System.Threading;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.Domain.Tutorials
{
    public interface IAddTutorial
    {
        Task<Result<Tutorial>> AddTutorialAsync(DirectoryPath tutorialPath, CancellationToken cancelationToken = default);
    }
}
