using Ardalis.Specification;

namespace dataneo.TutorialLibs.Domain.Tutorials
{
    public class TutorialsForDirectoryPathSpecification : Specification<Tutorial>
    {
        public TutorialsForDirectoryPathSpecification(DirectoryPath directoryPath)
        {
            Query.AsNoTracking()
                 .Where(w => directoryPath.Source.Contains(w.BasePath.Source));
        }
    }
}
