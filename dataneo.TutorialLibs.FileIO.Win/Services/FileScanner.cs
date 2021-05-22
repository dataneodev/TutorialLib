using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.Extensions;
using dataneo.TutorialLibs.Domain.Interfaces;
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
                string folderPath,
                CancellationToken cancellationToken)
        {
            Guard.Against.NullOrWhiteSpace(folderPath, nameof(folderPath));
            return await Result
                .Success((folderPath, cancellationToken))
                .OnSuccessTry(async fpath =>
                {
                    var option = new EnumerationOptions
                    {
                        IgnoreInaccessible = true,
                        ReturnSpecialDirectories = false,
                        MatchCasing = MatchCasing.CaseInsensitive,
                        RecurseSubdirectories = false,
                    };

                    return await Task.Run(() => Directory.GetFiles(fpath.folderPath, "*.*", option),
                                            fpath.cancellationToken) as IReadOnlyList<string>;
                }, exception => Errors.ERROR_SEARCHING_FILES_IN_FOLDER);
        }

        public async Task<Result<IReadOnlyList<string>>> GetFilesFromPathAsync(
                string folderPath,
                HashSet<string> handledExtensions,
                CancellationToken cancellationToken)
        {
            Guard.Against.NullOrWhiteSpace(folderPath, nameof(folderPath));
            Guard.Against.Null(handledExtensions, nameof(handledExtensions));

            return await Result
                .Success((folderPath, handledExtensions, cancellationToken))
                .OnSuccessTry(async fpath =>
                {
                    var option = new EnumerationOptions
                    {
                        IgnoreInaccessible = true,
                        ReturnSpecialDirectories = false,
                        MatchCasing = MatchCasing.CaseInsensitive,
                        RecurseSubdirectories = true,
                    };

                    var files = await Task.Run(
                                    () => Directory.GetFiles(fpath.folderPath, "*.*", option),
                                    fpath.cancellationToken);
                    return (fpath.handledExtensions, files, cancellationToken);
                }, exception => Errors.ERROR_SEARCHING_FILES_IN_FOLDER)
                .Ensure(fileResult => fileResult.cancellationToken.IsCancellationRequested == false,
                                      Errors.CANCELED_BY_USER)
                .Map(filesResult => filesResult.files.Where(w => FilterFilePath(w, filesResult.handledExtensions))
                                                     .ToArray() as IReadOnlyList<string>);
        }

        private bool FilterFilePath(string filePath, HashSet<string> handledExtensions)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return false;
            var fileExtension = Path.GetExtension(filePath.AsSpan()).TrimStart('.');
            return handledExtensions.Contains(fileExtension.ToString());
        }
    }
}
