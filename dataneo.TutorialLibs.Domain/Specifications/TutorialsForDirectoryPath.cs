using Ardalis.Specification;
using dataneo.TutorialLibs.Domain.Entities;
using dataneo.TutorialLibs.Domain.ValueObjects;

namespace dataneo.TutorialLibs.Domain.Specifications
{
    public class TutorialsForDirectoryPath : Specification<Tutorial>
    {
        public TutorialsForDirectoryPath(DirectoryPath directoryPath)
        {
            Query.AsNoTracking()
                 .Where(w => directoryPath.Source.Contains(w.BasePath.Source));
        }
    }
}
