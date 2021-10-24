﻿using Ardalis.GuardClauses;
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
        private readonly IHandledFileExtension _handledFileExtension;

        public FileScanner(IHandledFileExtension handledFileExtension)
        {
            this._handledFileExtension = Guard.Against.Null(handledFileExtension, nameof(handledFileExtension));
        }

        public async Task<Result<IReadOnlyList<string>>> GetRootDirectoryFromPathAsync(
                 DirectoryPath folderPath,
                 CancellationToken cancellationToken = default)
        {
            Guard.Against.Null(folderPath, nameof(folderPath));
            return await Result
                .Success((folderPath, cancellationToken))
                .Ensure(data => Directory.Exists(data.folderPath.Source), Errors.DIRECTORY_NOT_FOUND)
                .OnSuccessTry(fpath => GetDirectoriesAsync(fpath.folderPath, fpath.cancellationToken),
                                        exception => Errors.ERROR_SEARCHING_FILES_IN_FOLDER)
                .ConfigureAwait(false);
        }

        private static async Task<IReadOnlyList<string>> GetDirectoriesAsync(DirectoryPath folderPath, CancellationToken cancellationToken)
            => await Task.Run(() => Directory.GetDirectories(
                                                   path: folderPath.Source,
                                                   searchPattern: String.Empty,
                                                   searchOption: SearchOption.TopDirectoryOnly),
                                        cancellationToken)
                         .ConfigureAwait(false) as IReadOnlyList<string>;

        public async Task<Result<IReadOnlyList<string>>> GetFilesFromPathAsync(
                                                            DirectoryPath folderPath,
                                                            CancellationToken cancellationToken)
        {
            Guard.Against.Null(folderPath, nameof(folderPath));

            return await Result
                .Success((folderPath, this._handledFileExtension, cancellationToken))
                .Ensure(data => Directory.Exists(data.folderPath.Source), Errors.DIRECTORY_NOT_FOUND)
                .OnSuccessTry(async fpath =>
                {
                    var files = await ScanDirectoryAsync(fpath.folderPath, fpath.cancellationToken).ConfigureAwait(false);
                    return (fpath._handledFileExtension, files, cancellationToken);
                }, exception => Errors.ERROR_SEARCHING_FILES_IN_FOLDER)
                .Ensure(fileResult => fileResult.cancellationToken.IsCancellationRequested == false, Errors.CANCELED_BY_USER)
                .Map(filesResult => filesResult.files.Where(w => _handledFileExtension.FileAreSupported(w))
                                                     .ToArray() as IReadOnlyList<string>)
                .ConfigureAwait(false);
        }

        private static Task<string[]> ScanDirectoryAsync(DirectoryPath folderPath, CancellationToken cancellationToken)
        {
            var option = new EnumerationOptions
            {
                IgnoreInaccessible = true,
                ReturnSpecialDirectories = false,
                MatchCasing = MatchCasing.CaseInsensitive,
                RecurseSubdirectories = true,
            };

            return Task.Run(() => Directory.GetFiles(folderPath.Source, "*.*", option), cancellationToken);
        }
    }
}