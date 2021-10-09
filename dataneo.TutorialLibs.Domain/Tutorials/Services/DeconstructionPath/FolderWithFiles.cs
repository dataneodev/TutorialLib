using System.Collections.Generic;

namespace dataneo.TutorialLibs.Domain.Tutorials.Services
{
    internal record FolderWithFiles(string folder, IReadOnlyList<string> files);
}
