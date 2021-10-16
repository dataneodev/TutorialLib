using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.Domain.Tutorials
{
    public interface IFileScanner
    {
        Task<Result<IReadOnlyList<string>>> GetRootDirectoryFromPathAsync(
            DirectoryPath folderPath,
            CancellationToken cancellationToken);

        Task<Result<IReadOnlyList<string>>> GetFilesFromPathAsync(
            DirectoryPath folderPath,
            CancellationToken cancellationToken);
    }
}
