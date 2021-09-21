using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Translation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace dataneo.TutorialLibs.Domain.Tutorials.Services
{
    internal class FilePathsDeconstruction
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
            var rootSplit = rootPath.Source.Split(
                                Path.DirectorySeparatorChar,
                                StringSplitOptions.RemoveEmptyEntries);

            var returnList = new List<EpisodeFolderDeconstruction>(files.Count);
            foreach (var deconstructionResult in files.Select(filePath => GetDeconstructedFilePath(rootSplit, filePath)))
            {
                if (deconstructionResult.IsFailure)
                    return deconstructionResult.ConvertFailure<IReadOnlyList<EpisodeFolderDeconstruction>>();
                returnList.Add(deconstructionResult.Value);
            }

            return Result.Success(returnList as IReadOnlyList<EpisodeFolderDeconstruction>);
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
            var tutorialname = rootFolder
                                    .Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries)
                                    .LastOrDefault();
            if (String.IsNullOrWhiteSpace(tutorialname))
                return Result.Failure<string>(Errors.TUTORIAL_NAME_INCORECT);
            return tutorialname;
        }
    }
}
