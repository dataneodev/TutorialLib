using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Translation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace dataneo.TutorialLibs.Domain.ValueObjects
{
    public class EpisodeFile : ValueObject
    {
        public const short MinPlayTimeInSeconds = 1;
        public const short MinFileSizeInBytes = 128;

        public TimeSpan PlayTime { get; private set; }
        public string FileName { get; private set; }
        public long FileSize { get; private set; }
        public DateTime DateCreated { get; private set; }
        public DateTime DateModified { get; private set; }

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

            var invalidChars = Path.GetInvalidFileNameChars();
            if (invalidChars.Any(a => fileName.Contains(a)))
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
