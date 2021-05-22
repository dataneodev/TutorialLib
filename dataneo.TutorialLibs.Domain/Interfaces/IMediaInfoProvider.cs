using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.ValueObjects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.Domain.Interfaces
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
