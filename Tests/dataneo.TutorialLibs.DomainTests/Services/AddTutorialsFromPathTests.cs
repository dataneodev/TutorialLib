using dataneo.TutorialLibs.Domain.Services;
using dataneo.TutorialLibs.FileIO.Win.Services;

namespace dataneo.TutorialLibs.DomainTests.Services
{
    public class AddTutorialsFromPathTests
    {

        private AddTutorialsFromPath GetDefaultEngine()
            => new AddTutorialsFromPath(
                fileScanner: new FileScanner(),
                mediaInfoProvider: new MediaInfoProvider(),
                handledFileExtension: new HandledFileExtension(),
                new );
    }
}
