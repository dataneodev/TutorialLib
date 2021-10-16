using Xunit;

namespace dataneo.TutorialLibs.FileIO.WinTests.Services
{
    public class FileScanner_GetRootDirectoryFromPathAsyncTests
    {
        private const string MediaFolder = "media";
        private const string Tutorial_1 = "Tutorial_1";
        private const string Tutorial_2 = "Tutorial_2_Empty";

        [Fact]
        public void MedaiFolderTest()
        {
            //var mediaFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, MediaFolder);

            //using var cts = new CancellationTokenSource();

            //Directory.Exists(mediaFolderPath).Should().BeTrue();

            //var scanerEngine = new FileScanner();
            //var files = await scanerEngine.GetRootDirectoryFromPathAsync(mediaFolderPath, cts.Token);
            //files.IsSuccess.Should().BeTrue();
            //files.Value.Should().ContainMatch($"*{Tutorial_1}");
            //files.Value.Should().ContainMatch($"*{Tutorial_2}");
        }
    }
}
