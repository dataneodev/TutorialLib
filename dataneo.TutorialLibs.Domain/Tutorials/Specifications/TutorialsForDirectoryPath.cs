using Ardalis.Specification;

namespace dataneo.TutorialLibs.Domain.Tutorials
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
