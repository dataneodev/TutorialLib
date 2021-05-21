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

            var rootSplit = rootFolder.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);
            var episodeStructure = files.Select(s => GetFileStructure(rootSplit, s))
                                        .ToArray();

            var allResult = Result.Combine(episodeStructure);

            if (allResult.IsFailure)
                return allResult.ConvertFailure<Maybe<Tutorial>>();

            var folders = await GetFoldersWithEpisodesAsync(episodeStructure.Select(s => s.Value));

            if (folders.IsFailure)
                return folders.ConvertFailure<Maybe<Tutorial>>();
                  .
            return Maybe<Tutorial>.From(new Tutorial
            {
                BasePath = Path.GetFullPath(rootFolder),
                Name = Path.GetDirectoryName(rootFolder),
                Folders = folders.Value,
                AddDate = this._dateTimeProivder.Now,
                ModifiedTime = this._dateTimeProivder.Now,
            });
        }

        private Task<Result<IReadOnlyList<Folder>>> GetFoldersWithEpisodesAsync(IEnumerable<EpisodeFolderStructure> episodeFolderStructures)
        {
            var grupedFolders = episodeFolderStructures.GroupBy(g => g.Folder, StringComparer.InvariantCultureIgnoreCase);
            foreach (var directory in grupedFolders)
            {


                var folder = new Folder
                {
                    FolderName = directory.Key,
                    Name = directory.Key,
                    Order = 1,
                };
            }

            return Task.FromResult(Result.Failure<IReadOnlyList<Folder>>("Not finish"));
        }

        private async IAsyncEnumerable<Result<Episode>> GetEpisodes()
        {


            yield break;
        }

        private struct EpisodeFolderStructure
        {
            public string Folder;
            public string FilePath;
        }

        private Result<EpisodeFolderStructure> GetFileStructure(string[] rootSplit, string episodePath)
        {
            var episodeSplit = episodePath.Split(
                    Path.DirectorySeparatorChar,
                    StringSplitOptions.RemoveEmptyEntries);

            if (episodePath.Length < rootSplit.Length)
                return Result.Failure<EpisodeFolderStructure>("Niepoprawna scieżka episodu");

            if (episodePath.Length > rootSplit.Length + MaxSubFolderLevels + 1)
                return Result.Failure<EpisodeFolderStructure>(
                    $"Maksymalnie {MaxSubFolderLevels} zagłebienie folderu dozwolone w folderze tutorialu");

            return new EpisodeFolderStructure
            {
                FilePath = episodeSplit[^0],
                Folder = episodeSplit[^1],
            };
        }
    }
}
