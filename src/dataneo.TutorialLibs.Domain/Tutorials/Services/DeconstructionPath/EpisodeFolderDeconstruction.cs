using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Translation;
using System;

namespace dataneo.TutorialLibs.Domain.Tutorials.Services
{
    internal sealed class EpisodeFolderDeconstruction
    {
        private const byte MaxSubFolderLevels = 1;

        public static Result<EpisodeFolderDeconstruction> Create(
                string[] rootPath,
                string[] episodePath)
        {
            if (episodePath.Length < 2 || episodePath.Length < rootPath.Length)
                return Result.Failure<EpisodeFolderDeconstruction>(Errors.INVALID_FILE_PATH);

            if (episodePath.Length > rootPath.Length + MaxSubFolderLevels + 1)
                return Result.Failure<EpisodeFolderDeconstruction>(Errors.TOO_MANY_DIRECTORY_LEVELS);

            var folder = episodePath.Length > rootPath.Length + 1 ? episodePath[^2] : String.Empty;
            var filePath = episodePath[^1];
            return new EpisodeFolderDeconstruction(folder, filePath);
        }

        public readonly string Folder;
        public readonly string FileName;

        private EpisodeFolderDeconstruction(string folder, string filePath)
        {
            Folder = folder;
            FileName = filePath;
        }
    }
}
