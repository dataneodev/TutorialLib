﻿using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Application.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.Application.Interfaces
{
    public interface ITutorialScan
    {
        Task<Result<IReadOnlyList<EpisodeFile>>> GetFilesAsync(string folderPath);
    }
}
