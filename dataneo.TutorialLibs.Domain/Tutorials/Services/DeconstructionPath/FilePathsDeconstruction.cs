using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Translation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace dataneo.TutorialLibs.Domain.Tutorials.Services
{
    internal sealed class FilePathsDeconstruction
    {
        public Result<IReadOnlyList<FolderWithFiles>> GetFolderWithFiles(DirectoryPath rootPath, IReadOnlyList<string> files)
          => GetDeconstructedFilesPath(rootPath, files)
                .Map(decFiles => GetGrupedFolders(decFiles));

        private IReadOnlyList<FolderWithFiles> GetGrupedFolders(IReadOnlyList<EpisodeFolderDeconstruction> episodeFolderDeconstructions)
            => episodeFolderDeconstructions
                .GroupBy(g => g.Folder.Trim(), StringComparer.InvariantCultureIgnoreCase)
                .Select(s => new FolderWithFiles(s.Key, s.Select(s => s.FileName).ToArray()))
                .ToArray();

        private Result<IReadOnlyList<EpisodeFolderDeconstruction>> GetDeconstructedFilesPath(DirectoryPath rootPath, IReadOnlyList<string> files)
        {
            var rootSplit = rootPath.Source.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);
            var deconstructionResult = files.Select(filePath => GetDeconstructedFilePath(rootSplit, filePath)).ToArray();

            if (deconstructionResult.Any(a => a.IsFailure))
                return Result.Combine(deconstructionResult.Where(w => w.IsFailure))
                             .ConvertFailure<IReadOnlyList<EpisodeFolderDeconstruction>>();
            return Result.Success(
                deconstructionResult.Select(s => s.Value)
                                    .ToArray() as IReadOnlyList<EpisodeFolderDeconstruction>);
        }

        private Result<EpisodeFolderDeconstruction> GetDeconstructedFilePath(string[] rootSplit, string episodePath)
        {
            var episodeSplit = episodePath.Split(
                    Path.DirectorySeparatorChar,
                    StringSplitOptions.RemoveEmptyEntries);
            return EpisodeFolderDeconstruction.Create(rootSplit, episodeSplit);
        }

        public Result<string> GetTutorialName(string rootFolder)
        {
            var tutorialname = rootFolder.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries)
                                         .LastOrDefault();
            if (String.IsNullOrWhiteSpace(tutorialname))
                return Result.Failure<string>(Errors.TUTORIAL_NAME_INCORECT);
            return tutorialname;
        }
    }
}
