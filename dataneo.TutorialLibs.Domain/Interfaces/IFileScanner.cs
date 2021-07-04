using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.ValueObjects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.Domain.Interfaces
{
    public interface IFileScanner
    {
        Task<Result<IReadOnlyList<string>>> GetRootDirectoryFromPathAsync(
            DirectoryPath folderPath,
            CancellationToken cancellationToken);

        Task<Result<IReadOnlyList<string>>> GetFilesFromPathAsync(
            DirectoryPath folderPath,
            IHandledFileExtension handledFileExtension,
            CancellationToken cancellationToken);
    }
}
