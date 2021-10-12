using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.Extensions;
using dataneo.TutorialLibs.Domain.Translation;
using dataneo.TutorialLibs.Domain.Tutorials.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.Domain.Tutorials
{
    internal sealed class TutorialCreator
    {
        private readonly IFileScanner _fileScanner;
        private readonly IDateTimeProivder _dateTimeProivder;
        private readonly IMediaInfoProvider _mediaInfoProvider;

        public TutorialCreator(
            IFileScanner fileScanner,
            IMediaInfoProvider mediaInfoProvider,
            IDateTimeProivder dateTimeProivder)
        {
            this._fileScanner = Guard.Against.Null(fileScanner, nameof(fileScanner));
            this._dateTimeProivder = Guard.Against.Null(dateTimeProivder, nameof(dateTimeProivder));
            this._mediaInfoProvider = Guard.Against.Null(mediaInfoProvider, nameof(mediaInfoProvider));
        }

        public async Task<Result<Maybe<Tutorial>>> GetTutorialForFolderAsync(DirectoryPath path, CancellationToken cancelationToken = default)
        {
            Guard.Against.Null(path, nameof(path));
            return await this._fileScanner.GetFilesFromPathAsync(
                                folderPath: path,
                                cancellationToken: cancelationToken)
                    .Bind(files => GetTutorialFromFilesAsync(path.Source, files, cancelationToken));
        }

        private async Task<Result<Maybe<Tutorial>>> GetTutorialFromFilesAsync(
                        string rootFolder,
                        IReadOnlyList<string> files,
                        CancellationToken cancelationToken)
        {
            if (files.Count == 0)
                return Maybe<Tutorial>.None;

            var basePath = DirectoryPath.Create(Path.GetFullPath(rootFolder));
            if (basePath.IsFailure)
                return basePath.ConvertFailure<Maybe<Tutorial>>();

            var deconstructionEngine = new FilePathsDeconstruction();
            var deconstructedPaths = deconstructionEngine.GetFolderWithFiles(basePath.Value, files);
            if (deconstructedPaths.IsFailure)
                return deconstructedPaths.ConvertFailure<Maybe<Tutorial>>();

            var tutorialName = deconstructionEngine.GetTutorialName(rootFolder);
            if (tutorialName.IsFailure)
                return tutorialName.ConvertFailure<Maybe<Tutorial>>();

            if (cancelationToken.IsCancellationRequested)
                return Result.Failure<Maybe<Tutorial>>(Errors.CANCELED_BY_USER);

            var folders = await GetFoldersWithEpisodesAsync(
                                    basePath.Value,
                                    tutorialName.Value,
                                    deconstructedPaths.Value,
                                    cancelationToken)
                                .ConfigureAwait(false);

            if (folders.IsFailure)
                return folders.ConvertFailure<Maybe<Tutorial>>();

            if (cancelationToken.IsCancellationRequested)
                return Result.Failure<Maybe<Tutorial>>(Errors.CANCELED_BY_USER);

            if (folders.Value.Count == 0)
                return Maybe<Tutorial>.None;

            var tutorial = Tutorial.Create(
                 tutorialName.Value,
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
                        DirectoryPath rootPath,
                        string tutorialName,
                        IReadOnlyList<FolderWithFiles> episodeFolderStructures,
                        CancellationToken cancellationToken)
        {
            var folderList = new List<Folder>(episodeFolderStructures.Count);
            foreach (var folder in episodeFolderStructures)
            {
                if (cancellationToken.IsCancellationRequested)
                    return Result.Failure<IReadOnlyList<Folder>>(Errors.CANCELED_BY_USER);

                var folderResult = await GetFolderWithEpisodesAsync(rootPath, tutorialName, folder, cancellationToken);

                if (folderResult.IsFailure)
                    return folderResult.ConvertFailure<IReadOnlyList<Folder>>();

                folderList.Add(folderResult.Value);
            }
            return folderList;
        }

        private async Task<Result<Folder>> GetFolderWithEpisodesAsync(
                                                DirectoryPath rootPath,
                                                string tutorialName,
                                                FolderWithFiles folderWithFiles,
                                                CancellationToken cancellationToken)
        {
            var episodesResult = await GetEpisodesResultAsync(
                                            rootPath,
                                            folderWithFiles,
                                            cancellationToken);

            if (episodesResult.IsFailure)
                return episodesResult.ConvertFailure<Folder>();

            var folderName = string.IsNullOrWhiteSpace(folderWithFiles.folder) ? tutorialName : folderWithFiles.folder;

            return Folder.Create(
                            folderWithFiles.folder,
                            folderName,
                            episodesResult.Value);
        }

        private async Task<Result<IReadOnlyList<Episode>>> GetEpisodesResultAsync(
                                                                DirectoryPath rootPath,
                                                                FolderWithFiles folderWithFiles,
                                                                CancellationToken cancellationToken)
        {
            var episodesFiles = await this._mediaInfoProvider.GetFilesDetailsAsync(
                                        folderWithFiles.files.Select(
                                            files => Path.Combine(rootPath.Source, folderWithFiles.folder, files)),
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

            return Result.Success(episodes.Select(s => s.Value).ToArray() as IReadOnlyList<Episode>);
        }
    }
}
