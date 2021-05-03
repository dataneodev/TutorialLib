using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutorialLibs.Application.Model;

namespace TutorialLibs.Application.Interfaces
{
    public interface ITutorialScan
    {
        Task<Result<IReadOnlyList<EpisodeFile>>> GetFilesAsync(string folderPath);
    }
}
