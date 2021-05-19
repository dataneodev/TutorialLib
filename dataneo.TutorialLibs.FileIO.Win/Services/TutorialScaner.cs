using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
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
        public Task<Result<IReadOnlyList<Result<EpisodeFile>>>> GetFilesDetailsAsync(
                string[] filesPath, CancellationToken cancellationToken)
        {
            Guard.Against.Null(filesPath, nameof(filesPath));

            return Result
                .Success((filesPath, cancellationToken))
                .OnSuccessTry(async inputData =>
                {
                    using (var mediaInfo = new MediaInfo.DotNetWrapper.MediaInfo())
                    {
                        var data = await Task.Run(() => ProcessFiles(inputData.filesPath, mediaInfo, inputData.cancellationToken));
                        if (inputData.cancellationToken.IsCancellationRequested)
                        {
                            return Result.Failure<IReadOnlyList<Result<EpisodeFile>>>("Canceled at the user's request");
                        }

                        return Result.Success(data);
                    }
                }, exception => exception.Message)
                .Bind(b => b);
        }

        private IReadOnlyList<Result<EpisodeFile>> ProcessFiles(
                        string[] filesPath,
                        MediaInfo.DotNetWrapper.MediaInfo mediaInfo,
                        CancellationToken cancellationToken)
        {
            var returnList = new List<Result<EpisodeFile>>(filesPath.Length);

            foreach (var filePath in filesPath)
            {
                if (cancellationToken.IsCancellationRequested)
                    return returnList;

                mediaInfo.Open(filePath);

                var fileSizeResult = GetFileSize(mediaInfo);
                var fileDuration = GetDuration(mediaInfo);


                mediaInfo.Close();
            }
            return returnList;
        }

        public Task<Result<EpisodeFile>> GetFileDetailsAsync(string filePath, CancellationToken cancellationToken)
        {
            Guard.Against.NullOrWhiteSpace(filePath, nameof(filePath));

        }

        private Result<long> GetFileSize(MediaInfo.DotNetWrapper.MediaInfo mediaInfo)
        {
            if (long.TryParse(mediaInfo.Get(StreamKind.General, 0, "FileSize"), out long fileSize))
            {
                return fileSize;
            }

            return Result.Failure<long>("Error reading file size");
        }

        private Result<TimeSpan> GetDuration(MediaInfo.DotNetWrapper.MediaInfo mediaInfo)
        {
            if (int.TryParse(mediaInfo.Get(StreamKind.General, 0, "Duration"), out int fileDuration))
            {
                return TimeSpan.FromMilliseconds(fileDuration);
            }

            return Result.Failure<TimeSpan>("Error reading file length");
        }

        public async Task<Result<IReadOnlyList<string>>> GetFilesPathAsync(string folderPath, HashSet<string> handledExtensions)
        {
            Guard.Against.NullOrWhiteSpace(folderPath, nameof(folderPath));
            Guard.Against.Null(handledExtensions, nameof(handledExtensions));

            return await Result
                .Success((folderPath, handledExtensions))
                .OnSuccessTry(async fpath =>
                {
                    var option = new EnumerationOptions
                    {
                        IgnoreInaccessible = true,
                        ReturnSpecialDirectories = false,
                        MatchCasing = MatchCasing.CaseInsensitive,
                        RecurseSubdirectories = true,
                    };

                    var files = await Task.Run(() => Directory.GetFiles(fpath.folderPath, "*.*", option));
                    return (fpath.handledExtensions, files);
                })
                .Ensure(filesResult => filesResult.files != null, "File list is null")
                .Map(filesResult => filesResult.files.Where(w => FilterFilePath(w, filesResult.handledExtensions))
                                                     .ToArray() as IReadOnlyList<string>);
        }

        private bool FilterFilePath(string filePath, HashSet<string> handledExtensions)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return false;

            var fileExtension = Path.GetExtension(filePath);
            return handledExtensions.Contains(fileExtension);
        }
    }
}
