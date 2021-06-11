using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Translation;
using System.IO;
using System.Linq;

namespace dataneo.TutorialLibs.Domain.ValueObjects
{
    public sealed class DirectoryPath
    {
        public string Source { get; private set; }

        private DirectoryPath() { }

        public static Result<DirectoryPath> Create(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return Result.Failure<DirectoryPath>(Errors.INVALID_DIRECTORY);

            var invalidChars = Path.GetInvalidPathChars();

            if (invalidChars.Any(c => path.Contains(c)))
                return Result.Failure<DirectoryPath>(Errors.INVALID_DIRECTORY);

            return new DirectoryPath
            {
                Source = Path.GetFullPath(path),
            };
        }
    }
}
