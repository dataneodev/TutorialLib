using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.Extensions;
using dataneo.Helpers;
using dataneo.TutorialLibs.Domain.Interfaces;
using dataneo.TutorialLibs.Domain.ValueObjects;
using MediaInfo.DotNetWrapper.Enumerations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.FileIO.Win.Services
{
    public class TutorialScaner : ITutorialScaner
    {
        public async Task<Result<EpisodeFile>> GetFileDetailsAsync(
            string filePath,
            CancellationToken cancellationToken)
        {
            Guard.Against.NullOrWhiteSpace(filePath, nameof(filePath));
            return (await GetFilesDetailsAsync(ArrayHelper.SingleElementToArray(filePath), cancellationToken))
                        .Ensure(data => data.Count > 0, "Brak wyników")
                        .Bind(list => list.First());
        }

        public Task<Result<IReadOnlyList<Result<EpisodeFile>>>> GetFilesDetailsAsync(
                string[] filesPath,
                CancellationToken cancellationToken)
        {
            Guard.Against.Null(filesPath, nameof(filesPath));

            return Result
                .Success((filesPath, cancellationToken))
                .OnSuccessTry(async inputData =>
                {
                    using var mediaInfo = new MediaInfo.DotNetWrapper.MediaInfo();

                    var data = await Task.Run(() =>
                                GetEpisodesFileResult(
                                    inputData.filesPath,
                                    mediaInfo,
                                    inputData.cancellationToken)
                                 .ToArray(inputData.filesPath.Length) as IReadOnlyList<Result<EpisodeFile>>);

                    if (inputData.cancellationToken.IsCancellationRequested)
                        return Result.Failure<IReadOnlyList<Result<EpisodeFile>>>("Canceled at the user's request");

                    return Result.Success(data);
                }, exception => exception.Message)
                .Bind(b => b);
        }

        public async Task<Result<IReadOnlyList<string>>> GetFilesPathAsync(
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
                }, exception => exception.Message)
                .Ensure(fileResult => fileResult.cancellationToken.IsCancellationRequested == false,
                                      "Canceled at the user's request")
                .Ensure(filesResult => filesResult.files is not null, "File list is null")
                .Map(filesResult => filesResult.files.Where(w => FilterFilePath(w, filesResult.handledExtensions))
                                                     .ToArray() as IReadOnlyList<string>);
        }

        private IEnumerable<Result<EpisodeFile>> GetEpisodesFileResult(
                        string[] filesPath,
                        MediaInfo.DotNetWrapper.MediaInfo mediaInfo,
                        CancellationToken cancellationToken)
        {
            foreach (var filePath in filesPath)
            {
                if (cancellationToken.IsCancellationRequested)
                    yield break;

                mediaInfo.Open(filePath);
                yield return GetEpisodeFileResult(filePath, mediaInfo);
                mediaInfo.Close();
            }
        }

        private Result<EpisodeFile> GetEpisodeFileResult(string filePath, MediaInfo.DotNetWrapper.MediaInfo mediaInfo)
        {
            var fileSizeResult = GetFileSize(mediaInfo);
            if (fileSizeResult.IsFailure)
                return fileSizeResult.ConvertFailure<EpisodeFile>();

            var fileDuration = GetDuration(mediaInfo);
            if (fileDuration.IsFailure)
                return fileDuration.ConvertFailure<EpisodeFile>();

            FileInfo fInfo = new FileInfo(filePath);

            return EpisodeFile.Create(
                fileDuration.Value,
                Path.GetFileName(filePath),
                fileSizeResult.Value,
                fInfo.CreationTime,
                fInfo.LastWriteTime);
        }

        private Result<long> GetFileSize(MediaInfo.DotNetWrapper.MediaInfo mediaInfo)
        {
            if (long.TryParse(mediaInfo.Get(StreamKind.General, 0, "FileSize"), out long fileSize))
                return fileSize;

            return Result.Failure<long>("Error reading file size");
        }

        private Result<TimeSpan> GetDuration(MediaInfo.DotNetWrapper.MediaInfo mediaInfo)
        {
            if (int.TryParse(mediaInfo.Get(StreamKind.General, 0, "Duration"), out int fileDuration))
                return TimeSpan.FromMilliseconds(fileDuration);

            return Result.Failure<TimeSpan>("Error reading file length");
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
