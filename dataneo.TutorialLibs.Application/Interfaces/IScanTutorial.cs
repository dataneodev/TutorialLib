using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.Application.Interfaces
{
    public interface ITutorialScan
    {
        Task<Result<IReadOnlyList<EpisodeFile>>> GetFilesAsync(string folderPath);
    }
}
