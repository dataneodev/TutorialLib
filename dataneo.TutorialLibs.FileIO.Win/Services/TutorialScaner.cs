using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Interfaces;
using dataneo.TutorialLibs.Domain.ValueObjects;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.FileIO.Win.Services
{
    public class TutorialScaner : ITutorialScaner
    {
        public Task<Result<EpisodeFile>> GetFileDetailsAsync(string filePath, CancellationToken cancellationToken)
        {
            Guard.Against.NullOrWhiteSpace(filePath, nameof(filePath));


            return Task.FromResult(Result.Failure<EpisodeFile>("dsf "));
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
