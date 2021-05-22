﻿using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.Extensions;
using dataneo.TutorialLibs.Domain.Entities;
using dataneo.TutorialLibs.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.Domain.Services
{
    public sealed class TutorialFolderProcessor
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

        public async Task<Result<Maybe<Tutorial>>> GetTutorialForFolderAsync(string path, CancellationToken cancelationToken)
        {
            Guard.Against.NullOrWhiteSpace(path, nameof(path));
            return await this._fileScanner.GetFilesFromPathAsync(
                                folderPath: path,
                                handledFileExtension: this._handledFileExtension,
                                cancellationToken: cancelationToken)
                    .Bind(async files => await GetTutorialFromFilesAsync(path, files, cancelationToken));
        }

        private async Task<Result<Maybe<Tutorial>>> GetTutorialFromFilesAsync(
                        string rootFolder,
                        IReadOnlyList<string> files,
                        CancellationToken cancelationToken)
        {
            if (files.Count == 0)
                return Maybe<Tutorial>.None;

            var deconstructedPaths = GetDeconstructedFilesPath(rootFolder, files).ToArray(files.Count);

            if (cancelationToken.IsCancellationRequested)
                return Result.Failure<Maybe<Tutorial>>("Canceled by user");

            var allResult = Result.Combine(deconstructedPaths);

            if (allResult.IsFailure)
                return allResult.ConvertFailure<Maybe<Tutorial>>();

            var folders = await GetFoldersWithEpisodesAsync(
                            rootFolder,
                            deconstructedPaths.Select(s => s.Value),
                            cancelationToken);

            if (folders.IsFailure)
                return folders.ConvertFailure<Maybe<Tutorial>>();

            return Maybe<Tutorial>.From(new Tutorial
            {
                BasePath = Path.GetFullPath(rootFolder),
                Name = Path.GetDirectoryName(rootFolder),
                Folders = folders.Value,
                AddDate = this._dateTimeProivder.Now,
                ModifiedTime = this._dateTimeProivder.Now,
            });
        }

        private async Task<Result<IReadOnlyList<Folder>>> GetFoldersWithEpisodesAsync(
                        string rootPath,
                        IEnumerable<EpisodeFolderDeconstruction> episodeFolderStructures,
                        CancellationToken cancellationToken)
        {
            var grupedFolders = episodeFolderStructures.GroupBy(g => g.Folder?.Trim() ?? String.Empty,
                                                                     StringComparer.InvariantCultureIgnoreCase);
            var folderList = new List<Folder>(256);
            await foreach (var folderResult in GetFolderWithEpisodes(rootPath, grupedFolders, cancellationToken))
            {
                if (folderResult.IsFailure)
                    return folderResult.ConvertFailure<IReadOnlyList<Folder>>();

                if (cancellationToken.IsCancellationRequested)
                    return Result.Failure<IReadOnlyList<Folder>>("Canceled by user");

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
                    yield return Result.Failure<Folder>("Canceled by user");

                var episodesResult = await GetEpisodesResult(rootPath, directory, cancellationToken);
                if (episodesResult.IsFailure)
                    yield return episodesResult.ConvertFailure<Folder>();

                var folder = new Folder
                {
                    FolderName = directory.Key.Trim(),
                    Name = String.IsNullOrWhiteSpace(directory.Key) ? String.Empty : directory.Key,
                    Order = 1,
                    Episodes = episodesResult.Value
                };

                yield return folder;
            }
        }

        private async Task<Result<IReadOnlyList<Episode>>> GetEpisodesResult(
                        string rootPath,
                        IEnumerable<EpisodeFolderDeconstruction> episodeFolderDeconstructions,
                        CancellationToken cancellationToken)
        {
            var episodesFiles = await this._mediaInfoProvider.GetFilesDetailsAsync(
                                        episodeFolderDeconstructions.Select(s => Path.Combine(rootPath, s.Folder, s.FilePath)),
                                        cancellationToken);

            if (cancellationToken.IsCancellationRequested)
                return Result.Failure<IReadOnlyList<Episode>>("Canceled by user");

            if (episodesFiles.IsFailure)
                return episodesFiles.ConvertFailure<IReadOnlyList<Episode>>();

            if (episodesFiles.Value.Any(a => a.IsFailure))
                return Result.Combine(episodesFiles.Value.Where(a => a.IsFailure))
                             .ConvertFailure<IReadOnlyList<Episode>>();

            return episodesFiles.Value
                .Select(s => new Episode
                {
                    DateAdd = this._dateTimeProivder.Now,
                    File = s.Value,
                    Name = Path.GetFileNameWithoutExtension(s.Value.FileName),
                }).ToList();
        }

        private struct EpisodeFolderDeconstruction
        {
            public static Result<EpisodeFolderDeconstruction> Create(
                    string[] rootPath,
                    string[] episodePath)
            {
                if (episodePath.Length < 2 || episodePath.Length < rootPath.Length)
                    return Result.Failure<EpisodeFolderDeconstruction>("Niepoprawna scieżka episodu");

                if (episodePath.Length > rootPath.Length + MaxSubFolderLevels + 1)
                    return Result.Failure<EpisodeFolderDeconstruction>(
                        $"Maksymalnie {MaxSubFolderLevels} zagłebienie folderu dozwolone w folderze tutorialu");

                return new EpisodeFolderDeconstruction
                {
                    FilePath = episodePath[^1],
                    Folder = episodePath.Length > rootPath.Length + 1 ?
                                episodePath[^2] : String.Empty,
                };
            }
            public string Folder;
            public string FilePath;
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
