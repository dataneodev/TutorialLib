using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.Domain.Tutorials
{
    public interface IMediaInfoProvider
    {
        Task<Result<EpisodeFile>> GetFileDetailsAsync(
            string filePath,
            CancellationToken cancellationToken);

        Task<Result<IReadOnlyList<Result<EpisodeFile>>>> GetFilesDetailsAsync(
            IEnumerable<string> filesPath,
            CancellationToken cancellationToken);
    }
}
