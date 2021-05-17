using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Interfaces;
using dataneo.TutorialLibs.Domain.ValueObjects;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.FileIO.Win.Services
{
    public class TutorialScaner : ITutorialScaner
    {
        public Task<Result<EpisodeFile>> GetFileDetailsAsync(string filePath, CancellationToken cancellationToken)
        {
            Guard.Against.NullOrWhiteSpace(filePath, nameof(filePath));



        }

        public Task<Result<IReadOnlyList<string>>> GetFilesPathAsync(string folderPath, HashSet<string> handledExtensions, CancellationToken cancellationToken)
        {
            Guard.Against.NullOrWhiteSpace(folderPath, nameof(folderPath));
            Guard.Against.Null(handledExtensions, nameof(handledExtensions));

            var files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);
        }

        private bool FilterFilePath(string filePath, HashSet<string> handledExtensions)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return false;

            Path.GetExtension(filePath)


        }
    }
}
