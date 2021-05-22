using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.Domain.Interfaces
{
    public interface IFileScanner
    {
        Task<Result<IReadOnlyList<string>>> GetRootDirectoryFromPathAsync(
            string folderPath,
            CancellationToken cancellationToken);

        Task<Result<IReadOnlyList<string>>> GetFilesFromPathAsync(
            string folderPath,
            HashSet<string> handledFileExtensions,
            CancellationToken cancellationToken);
    }
}
