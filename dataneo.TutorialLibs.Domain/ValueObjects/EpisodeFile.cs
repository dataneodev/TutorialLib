using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;

namespace dataneo.TutorialLibs.Domain.ValueObjects
{
    public class EpisodeFile : ValueObject
    {
        public const short MinPlayTimeInSeconds = 1;

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
                return


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
