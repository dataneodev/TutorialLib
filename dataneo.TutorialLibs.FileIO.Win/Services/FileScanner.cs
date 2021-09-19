using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.Extensions;
using dataneo.TutorialLibs.Domain.Tutorials;
using dataneo.TutorialLibs.FileIO.Win.Translation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.FileIO.Win.Services
{
    public sealed class FileScanner : IFileScanner
    {
        public async Task<Result<IReadOnlyList<string>>> GetRootDirectoryFromPathAsync(
                 DirectoryPath folderPath,
                 CancellationToken cancellationToken = default)
        {
            Guard.Against.Null(folderPath, nameof(folderPath));
            return await Result
                .Success((folderPath, cancellationToken))
                .Ensure(data => Directory.Exists(data.folderPath.Source), Errors.DIRECTORY_NOT_FOUND)
                .OnSuccessTry(async fpath =>
                {
                    return await Task.Run(() => Directory.GetDirectories(
                                                    fpath.folderPath.Source,
                                                    String.Empty,
                                                    SearchOption.TopDirectoryOnly),
                                                fpath.cancellationToken)
                                     .ConfigureAwait(false) as IReadOnlyList<string>;
                }
                , exception => Errors.ERROR_SEARCHING_FILES_IN_FOLDER)
                .ConfigureAwait(false);
        }

        public async Task<Result<IReadOnlyList<string>>> GetFilesFromPathAsync(
                DirectoryPath folderPath,
                IHandledFileExtension handledFileExtension,
                CancellationToken cancellationToken)
        {
            Guard.Against.Null(folderPath, nameof(folderPath));
            Guard.Against.Null(handledFileExtension, nameof(handledFileExtension));

            return await Result
                .Success((folderPath, handledFileExtension, cancellationToken))
                .Ensure(data => Directory.Exists(data.folderPath.Source), Errors.DIRECTORY_NOT_FOUND)
                .OnSuccessTry(async fpath =>
                {
                    var option = new EnumerationOptions
                    {
                        IgnoreInaccessible = true,
                        ReturnSpecialDirectories = false,
                        MatchCasing = MatchCasing.CaseInsensitive,
                        RecurseSubdirectories = true,
                    };

                    var files = await Task.Run(() => Directory.GetFiles(fpath.folderPath.Source, "*.*", option),
                                               fpath.cancellationToken)
                                           .ConfigureAwait(false);
                    return (fpath.handledFileExtension, files, cancellationToken);
                }, exception => Errors.ERROR_SEARCHING_FILES_IN_FOLDER)
                .Ensure(fileResult => fileResult.cancellationToken.IsCancellationRequested == false,
                                      Errors.CANCELED_BY_USER)
                .Map(filesResult => filesResult.files.Where(w => handledFileExtension.FileAreSupported(w))
                                                     .ToArray() as IReadOnlyList<string>)
                .ConfigureAwait(false);
        }
    }
}
