﻿using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.Extensions;
using dataneo.TutorialLibs.Domain.Entities;
using dataneo.TutorialLibs.Domain.Interfaces;
using dataneo.TutorialLibs.Domain.Translation;
using dataneo.TutorialLibs.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

[assembly: InternalsVisibleToAttribute("dataneo.TutorialLibs.DomainTests")]

namespace dataneo.TutorialLibs.Domain.Services
{
    internal sealed class TutorialFolderProcessor
    {
        private const byte MaxSubFolderLevels = 1;

        private readonly IFileScanner _fileScanner;
        private readonly IDateTimeProivder _dateTimeProivder;
        private readonly IMediaInfoProvider _mediaInfoProvider;
        private readonly IHandledFileExtension _handledFileExtension;

        public TutorialFolderProcessor(
            IFileScanner fileScanner,
            IMediaInfoProvider mediaInfoProvider,
            IDateTimeProivder dateTimeProivder,
            IHandledFileExtension handledFileExtension)
        {
            this._fileScanner = Guard.Against.Null(fileScanner, nameof(fileScanner));
            this._dateTimeProivder = Guard.Against.Null(dateTimeProivder, nameof(dateTimeProivder));
            this._mediaInfoProvider = Guard.Against.Null(mediaInfoProvider, nameof(mediaInfoProvider));
            this._handledFileExtension = Guard.Against.Null(handledFileExtension, nameof(handledFileExtension));
        }

        public async Task<Result<Maybe<Tutorial>>> GetTutorialForFolderAsync(
                                                        DirectoryPath path,
                                                        CancellationToken cancelationToken = default)
        {
            Guard.Against.Null(path, nameof(path));
            return await this._fileScanner.GetFilesFromPathAsync(
                                folderPath: path,
                                handledFileExtension: this._handledFileExtension,
                                cancellationToken: cancelationToken)
                    .Bind(async files => await GetTutorialFromFilesAsync(path.Source, files, cancelationToken)
                                                .ConfigureAwait(false));
        }

        private async Task<Result<Maybe<Tutorial>>> GetTutorialFromFilesAsync(
                        string rootFolder,
                        IReadOnlyList<string> files,
                        CancellationToken cancelationToken)
        {
            if (files.Count == 0)
                return Maybe<Tutorial>.None;

            var deconstructedPaths = GetDeconstructedFilesPath(rootFolder, files)
                                        .ToArray(files.Count);

            if (cancelationToken.IsCancellationRequested)
                return Result.Failure<Maybe<Tutorial>>(Errors.CANCELED_BY_USER);

            if (deconstructedPaths.Any(a => a.IsFailure))
                return Result.Combine(deconstructedPaths)
                             .ConvertFailure<Maybe<Tutorial>>();

            var folders = await GetFoldersWithEpisodesAsync(
                            rootFolder,
                            deconstructedPaths.Select(s => s.Value),
                            cancelationToken).ConfigureAwait(false);

            if (folders.IsFailure)
                return folders.ConvertFailure<Maybe<Tutorial>>();

            if (cancelationToken.IsCancellationRequested)
                return Result.Failure<Maybe<Tutorial>>(Errors.CANCELED_BY_USER);

            if (folders.Value.Count == 0)
                return Maybe<Tutorial>.None;

            var basePath = DirectoryPath.Create(Path.GetFullPath(rootFolder));
            if (basePath.IsFailure)
                return basePath.ConvertFailure<Maybe<Tutorial>>();

            var tutorialName = rootFolder.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries)
                                         .LastOrDefault();

            var tutorial = Tutorial.Create(
                 tutorialName,
                 basePath.Value,
                 folders.Value,
                 this._dateTimeProivder.Now);

            if (tutorial.IsFailure)
                return tutorial.ConvertFailure<Maybe<Tutorial>>();

            OrderTutorial(tutorial.Value);

            return Maybe<Tutorial>.From(tutorial.Value);
        }

        private static void OrderTutorial(Tutorial tutorial)
        {
            var orderEngine = new OrderTutorialDefault();
            orderEngine.OrderFoldersAndEpisodesByName(tutorial);
        }

        private async Task<Result<IReadOnlyList<Folder>>> GetFoldersWithEpisodesAsync(
                        string rootPath,
                        IEnumerable<EpisodeFolderDeconstruction> episodeFolderStructures,
                        CancellationToken cancellationToken)
        {
            var grupedFolders = episodeFolderStructures
                                    .GroupBy(g => g.Folder?.Trim() ?? String.Empty,
                                                  StringComparer.InvariantCultureIgnoreCase);
            var folderList = new List<Folder>(256);
            await foreach (var folderResult in GetFolderWithEpisodes(
                                                rootPath,
                                                grupedFolders,
                                                cancellationToken))
            {
                if (folderResult.IsFailure)
                    return folderResult.ConvertFailure<IReadOnlyList<Folder>>();

                if (cancellationToken.IsCancellationRequested)
                    return Result.Failure<IReadOnlyList<Folder>>(Errors.CANCELED_BY_USER);

                folderList.Add(folderResult.Value);
            }
            return folderList;
        }

        private async IAsyncEnumerable<Result<Folder>> GetFolderWithEpisodes(
                        string rootPath,
                        IEnumerable<IGrouping<string, EpisodeFolderDeconstruction>> grupedFolders,
                        CancellationToken cancellationToken)
        {
            foreach (var directory in grupedFolders)
            {
                if (cancellationToken.IsCancellationRequested)
                    yield break;

                var episodesResult = await GetEpisodesResult(
                    rootPath,
                    directory,
                    cancellationToken);

                if (episodesResult.IsFailure)
                    yield return episodesResult.ConvertFailure<Folder>();

                if (episodesResult.Value.Count == 0)
                    continue;

                var folder = Folder.Create(
                    directory.Key.Trim(),
                    episodesResult.Value);

                if (folder.IsFailure)
                    continue;

                yield return folder;
            }
        }

        private async Task<Result<IReadOnlyList<Episode>>> GetEpisodesResult(
                        string rootPath,
                        IEnumerable<EpisodeFolderDeconstruction> episodeFolderDeconstructions,
                        CancellationToken cancellationToken)
        {
            var episodesFiles = await this._mediaInfoProvider.GetFilesDetailsAsync(
                                         episodeFolderDeconstructions.Select(s => Path.Combine(rootPath, s.Folder, s.FileName)),
                                         cancellationToken)
                                      .ConfigureAwait(false);

            if (cancellationToken.IsCancellationRequested)
                return Result.Failure<IReadOnlyList<Episode>>(Errors.CANCELED_BY_USER);

            if (episodesFiles.IsFailure)
                return episodesFiles.ConvertFailure<IReadOnlyList<Episode>>();

            if (episodesFiles.Value.Any(a => a.IsFailure))
                return Result.Combine(episodesFiles.Value.Where(a => a.IsFailure))
                             .ConvertFailure<IReadOnlyList<Episode>>();

            var episodes = episodesFiles.Value
                                .Select(epFile => Episode.Create(epFile.Value, this._dateTimeProivder.Now))
                                .ToArray(episodesFiles.Value.Count);

            if (episodes.Any(a => a.IsFailure))
                return Result.Combine(episodes.Where(a => a.IsFailure))
                             .ConvertFailure<IReadOnlyList<Episode>>();

            return Result.Success(episodes.Select(s => s.Value)
                                          .ToArray() as IReadOnlyList<Episode>);
        }

        private class EpisodeFolderDeconstruction
        {
            public static Result<EpisodeFolderDeconstruction> Create(
                    string[] rootPath,
                    string[] episodePath)
            {
                if (episodePath.Length < 2 || episodePath.Length < rootPath.Length)
                    return Result.Failure<EpisodeFolderDeconstruction>(Errors.INVALID_FILE_PATH);

                if (episodePath.Length > rootPath.Length + MaxSubFolderLevels + 1)
                    return Result.Failure<EpisodeFolderDeconstruction>(Errors.TOO_MANY_DIRECTORY_LEVELS);

                var folder = episodePath.Length > rootPath.Length + 1 ? episodePath[^2] : String.Empty;
                var filePath = episodePath[^1];
                return new EpisodeFolderDeconstruction(folder, filePath);
            }

            public readonly string Folder;
            public readonly string FileName;

            private EpisodeFolderDeconstruction(string folder, string filePath)
            {
                Folder = folder;
                FileName = filePath;
            }
        }

        private IEnumerable<Result<EpisodeFolderDeconstruction>> GetDeconstructedFilesPath(
                    string rootPath, IEnumerable<string> files)
        {
            var rootSplit = rootPath.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);
            return files.Select(filePath => GetDeconstructedFilePath(rootSplit, filePath));
        }

        private Result<EpisodeFolderDeconstruction> GetDeconstructedFilePath(string[] rootSplit, string episodePath)
        {
            var episodeSplit = episodePath.Split(
                    Path.DirectorySeparatorChar,
                    StringSplitOptions.RemoveEmptyEntries);

            return EpisodeFolderDeconstruction.Create(rootSplit, episodeSplit);
        }
    }
}
