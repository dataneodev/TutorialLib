using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.Helpers;
using dataneo.TutorialLibs.Domain.Interfaces;
using dataneo.TutorialLibs.Domain.ValueObjects;
using dataneo.TutorialLibs.FileIO.Win.Translation;
using MediaInfo.DotNetWrapper.Enumerations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.FileIO.Win.Services
{
    public sealed class MediaInfoProvider : IMediaInfoProvider
    {
        public async Task<Result<EpisodeFile>> GetFileDetailsAsync(
                string filePath,
                CancellationToken cancellationToken)
        {
            Guard.Against.NullOrWhiteSpace(filePath, nameof(filePath));
            return (await GetFilesDetailsAsync(ArrayHelper.SingleElementToArray(filePath), cancellationToken))
                        .Ensure(data => data.Count > 0, "No result")
                        .Bind(list => list.First());
        }

        public Task<Result<IReadOnlyList<Result<EpisodeFile>>>> GetFilesDetailsAsync(
                IEnumerable<string> filesPath,
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
                                 .ToArray() as IReadOnlyList<Result<EpisodeFile>>);

                    if (inputData.cancellationToken.IsCancellationRequested)
                        return Result.Failure<IReadOnlyList<Result<EpisodeFile>>>(Errors.CANCELED_BY_USER);

                    return Result.Success(data);
                }, exception => exception.Message)
                .Bind(b => b);
        }

        private IEnumerable<Result<EpisodeFile>> GetEpisodesFileResult(
                    IEnumerable<string> filesPath,
                    MediaInfo.DotNetWrapper.MediaInfo mediaInfo,
                    CancellationToken cancellationToken)
        {
            foreach (var filePath in filesPath)
            {
                if (cancellationToken.IsCancellationRequested)
                    yield break;

                if (!File.Exists(filePath))
                    yield return Result.Failure<EpisodeFile>(Errors.FILE_NOT_FOUND);

                yield return GetEpisodeFileResult(filePath, mediaInfo);
            }
        }

        private Result<EpisodeFile> GetEpisodeFileResult(string filePath, MediaInfo.DotNetWrapper.MediaInfo mediaInfo)
        {
            mediaInfo.Open(filePath);

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

            return Result.Failure<long>(Errors.ERROR_READING_FILE_LENGTH);
        }

        private Result<TimeSpan> GetDuration(MediaInfo.DotNetWrapper.MediaInfo mediaInfo)
        {
            if (int.TryParse(mediaInfo.Get(StreamKind.General, 0, "Duration"), out int fileDuration))
                return TimeSpan.FromMilliseconds(fileDuration);

            return Result.Failure<TimeSpan>(Errors.ERROR_READING_VIDEO_DURATION);
        }
    }
}
