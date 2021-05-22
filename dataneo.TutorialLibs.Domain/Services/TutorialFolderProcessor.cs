using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.Extensions;
using dataneo.TutorialLibs.Domain.Constans;
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

        private readonly ITutorialScaner _tutorialScaner;
        private readonly IDateTimeProivder _dateTimeProivder;

        public TutorialFolderProcessor(ITutorialScaner tutorialScaner, IDateTimeProivder dateTimeProivder)
        {
            this._tutorialScaner = Guard.Against.Null(tutorialScaner, nameof(tutorialScaner));
            this._dateTimeProivder = Guard.Against.Null(dateTimeProivder, nameof(dateTimeProivder));
        }

        public async Task<Result<Maybe<Tutorial>>> GetTutorialForFolderAsync(string path, CancellationToken cancelationToken)
        {
            Guard.Against.NullOrWhiteSpace(path, nameof(path));
            return await _tutorialScaner.GetFilesPathAsync(
                                folderPath: path,
                                handledExtensions: HandledFormats.HandledFileExtensions,
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

            foreach (var directory in grupedFolders)
            {
                if (cancellationToken.IsCancellationRequested)
                    return Result.Failure<IReadOnlyList<Folder>>("Canceled by user");

                var episodesResult = await GetEpisodesResult(directory, rootPath, cancellationToken);
                if (episodesResult.IsFailure)
                    return episodesResult.ConvertFailure<IReadOnlyList<Folder>>();

                var folder = new Folder
                {
                    FolderName = directory.Key.Trim(),
                    Name = String.IsNullOrWhiteSpace(directory.Key) ? String.Empty : directory.Key,
                    Order = 1,
                    Episodes = episodesResult.Value
                };

                folderList.Add(folder);
            }

            return folderList;
        }

        private async Task<Result<IReadOnlyList<Episode>>> GetEpisodesResult(
                        IEnumerable<EpisodeFolderDeconstruction> episodeFolderDeconstructions,
                        string rootPath,
                        CancellationToken cancellationToken)
        {
            var episodesFiles = await this._tutorialScaner.GetFilesDetailsAsync(
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
            public static Result<EpisodeFolderDeconstruction> Create(string[] rootPath, string[] episodePath)
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

        private IEnumerable<Result<EpisodeFolderDeconstruction>> GetDeconstructedFilesPath(string rootPath, IEnumerable<string> files)
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
