namespace dataneo.TutorialLibs.FileIO.WinTests.Services
{
    public class MediaInfoProviderTests
    {
        private const string MediaFolder = "media";
        private const string Tutorial_1 = "Tutorial_1";
        private const string Tutorial_2 = "Tutorial_2_Empty";
        private const string SampleMediaFile1 = "file_example_MP4_640_3MG.mp4";

        //[Fact]
        //public async void GetSingleEpisodeFile()
        //{
        //    var mediaPath = Path.Combine(
        //        AppDomain.CurrentDomain.BaseDirectory,
        //        MediaFolder,
        //        Tutorial_1);
        //    using var cts = new CancellationTokenSource();

        //    Directory.Exists(mediaPath).Should().BeTrue();

        //    var scanerEngine = new FileScanner();
        //    var files = await scanerEngine.GetFilesFromPathAsync(
        //        mediaPath,
        //        new HandledFileExtension(),
        //        cts.Token);

        //    files.IsSuccess.Should().BeTrue();

        //    var mediaFile = files.Value.FirstOrDefault(f => f.EndsWith(SampleMediaFile1));
        //    mediaFile.Should().NotBeNullOrEmpty();

        //    var mediaInfoProvider = new MediaInfoProvider();

        //    var result = await mediaInfoProvider.GetFileDetailsAsync(mediaFile, cts.Token);
        //    result.IsSuccess.Should().BeTrue();

        //    result.Value.FileName.Should().Be(SampleMediaFile1);
        //    result.Value.FileSize.Should().Be(3114374);
        //    result.Value.PlayTime.Should().Be(TimeSpan.FromMilliseconds(30527));
        //}

        //[Fact]
        //public async void GetEpisodeFromTutotial_1()
        //{
        //    var mediaPath = Path.Combine(AppDomain
        //        .CurrentDomain
        //        .BaseDirectory,
        //        MediaFolder,
        //        Tutorial_1);
        //    using var cts = new CancellationTokenSource();

        //    Directory.Exists(mediaPath).Should().BeTrue();

        //    var scanerEngine = new FileScanner();
        //    var files = await scanerEngine.GetFilesFromPathAsync(
        //        mediaPath,
        //        new HandledFileExtension(),
        //        cts.Token);

        //    files.IsSuccess.Should().BeTrue();

        //    var mediaInfoProvider = new MediaInfoProvider();
        //    var result = await mediaInfoProvider.GetFilesDetailsAsync(files.Value, cts.Token);
        //    result.IsSuccess.Should().BeTrue();

        //    result.Value.Should().NotBeEmpty();
        //    result.Value.Any(a => a.IsFailure).Should().BeFalse();
        //}
    }
}
