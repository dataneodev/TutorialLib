using dataneo.TutorialLibs.Domain.Services;
using dataneo.TutorialLibs.FileIO.Win.Services;
using System.Threading;
using Xunit;

namespace dataneo.TutorialLibs.DomainTests.Services
{
    public class TutorialFolderProcessorTests
    {

        [Fact]
        public async void RawTest()
        {
            using var cts = new CancellationTokenSource();
            var folderProcessEngine = new TutorialFolderProcessor(
                new TutorialScaner(),
                new DateTimeProivder());

            var result = await folderProcessEngine.GetTutorialForFolderAsync(
                                    @"f:\Filmy na dysku\",
                                    cts.Token);

        }
    }
}
