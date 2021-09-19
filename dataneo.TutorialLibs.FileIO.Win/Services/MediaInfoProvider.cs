using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.Helpers;
using dataneo.TutorialLibs.Domain.Tutorials;
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
                CancellationToken cancellationToken = default)
        {
            Guard.Against.NullOrWhiteSpace(filePath, nameof(filePath));
            var filesDetails = await GetFilesDetailsAsync(ArrayHelper.SingleElementToArray(filePath), cancellationToken)
                                        .ConfigureAwait(false);

            return filesDetails.Ensure(data => data.Count > 0, "No result")
                               .Bind(list => list.First());
        }

        public Task<Result<IReadOnlyList<Result<EpisodeFile>>>> GetFilesDetailsAsync(
                IEnumerable<string> filesPath,
                CancellationToken cancellationToken = default)
        {
            Guard.Against.Null(filesPath, nameof(filesPath));

            return Result
                .Success((filesPath, cancellationToken))
                .OnSuccessTry(async inputData =>
                {
                    using var mediaInfo = new MediaInfo.DotNetWrapper.MediaInfo();
                    var data = await Task.Run(() => GetEpisodesFileResult(
                                                        inputData.filesPath,
                                                        mediaInfo,
                                                        inputData.cancellationToken).ToArray() as IReadOnlyList<Result<EpisodeFile>>)
                                         .ConfigureAwait(false);

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

            var fileDuration = GetDuration(filePath, mediaInfo);
            if (fileDuration.IsFailure)
                return fileDuration.ConvertFailure<EpisodeFile>();

            var fileSizeResult = GetFileSize(filePath, mediaInfo);
            if (fileSizeResult.IsFailure)
                return fileSizeResult.ConvertFailure<EpisodeFile>();

            FileInfo fInfo = new FileInfo(filePath);

            return EpisodeFile.Create(
                playTime: fileDuration.Value,
                fileName: Path.GetFileName(filePath),
                fileSize: fileSizeResult.Value,
                dateCreated: fInfo.CreationTime,
                dateModified: fInfo.LastWriteTime);
        }

        private Result<long> GetFileSize(string filePath, MediaInfo.DotNetWrapper.MediaInfo mediaInfo)
        {
            if (long.TryParse(mediaInfo.Get(StreamKind.General, 0, "FileSize"), out long fileSize))
                return fileSize;

            return Result.Failure<long>(String.Format(Errors.ERROR_READING_FILE_LENGTH, filePath));
        }

        private Result<TimeSpan> GetDuration(string filePath, MediaInfo.DotNetWrapper.MediaInfo mediaInfo)
        {
            if (int.TryParse(mediaInfo.Get(StreamKind.General, 0, "Duration"), out int fileDuration))
                return TimeSpan.FromMilliseconds(fileDuration);

            return Result.Failure<TimeSpan>(String.Format(Errors.ERROR_READING_VIDEO_DURATION, filePath));
        }
    }
}
