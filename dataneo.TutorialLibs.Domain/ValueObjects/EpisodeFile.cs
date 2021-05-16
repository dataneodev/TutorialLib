using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Translation;
using System;
using System.Collections.Generic;

namespace dataneo.TutorialLibs.Domain.ValueObjects
{
    public class EpisodeFile : ValueObject
    {
        public const short MinPlayTimeInSeconds = 1;
        public const short MinFileSizeInBytes = 1024;

        public TimeSpan PlayTime { get; init; }
        public string FileName { get; init; }
        public long FileSize { get; init; }
        public DateTime DateCreated { get; init; }
        public DateTime DateModified { get; init; }

        private EpisodeFile() { }

        public static Result<EpisodeFile> Create(
            TimeSpan playTime,
            string fileName,
            long fileSize,
            DateTime dateCreated,
            DateTime dateModified)
        {
            if (playTime.TotalSeconds < MinPlayTimeInSeconds)
                return FailureResult(Errors.EPISODE_TO_SHORT);

            if (fileSize < MinFileSizeInBytes)
                return FailureResult(Errors.FILE_SIZE_TO_SMALL);

            if (string.IsNullOrWhiteSpace(fileName))
                return FailureResult(Errors.FILENAME_INCORECT);

            return Result.Success(
                new EpisodeFile
                {
                    FileName = fileName,
                    FileSize = fileSize,
                    PlayTime = playTime,
                    DateCreated = dateCreated,
                    DateModified = dateModified
                });

            Result<EpisodeFile> FailureResult(string msg)
              => Result.Failure<EpisodeFile>(msg);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return PlayTime;
            yield return FileName;
            yield return FileSize;
            yield return DateCreated;
            yield return DateModified;
        }
    }
}
