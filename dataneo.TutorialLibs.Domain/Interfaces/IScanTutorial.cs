using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.ValueObjects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.Domain.Interfaces
{
    public interface ITutorialScaner
    {
        Task<Result<IReadOnlyList<string>>> GetFilesPathAsync(
                        string folderPath,
                        HashSet<string> handledExtensions,
                        CancellationToken cancellationToken);

        Task<Result<EpisodeFile>> GetFileDetailsAsync(string filePath, CancellationToken cancellationToken);
    }
}
