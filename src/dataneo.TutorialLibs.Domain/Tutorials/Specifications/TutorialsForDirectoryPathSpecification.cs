using Ardalis.Specification;
using System.IO;

namespace dataneo.TutorialLibs.Domain.Tutorials
{
    public class TutorialsForDirectoryPathSpecification : Specification<Tutorial>
    {
        public TutorialsForDirectoryPathSpecification(DirectoryPath directoryPath)
        {
            var path = directoryPath.Source + Path.DirectorySeparatorChar;
            Query.AsNoTracking()
                 .Where(w => w.BasePath.Source.Equals(directoryPath.Source) || w.BasePath.Source.Contains(path));
        }
    }
}
