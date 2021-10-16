namespace dataneo.TutorialLibs.FileIO.WinTests.Services
{
    public class FileScanner_GetFilesFromPathAsyncTests
    {
        private const string MediaFolder = "media";
        private const string Tutorial_1 = "Tutorial_1";
        private const string Tutorial_2 = "Tutorial_2_Empty";
        private const string SampleMediaFile1 = "file_example_MP4_640_3MG.mp4";

        //    [Fact]
        //    public async void GetFilesPathAsyncPathNullArguments()
        //    {
        //        var scanerEngine = new FileScanner();
        //        using var cts = new CancellationTokenSource();

        //        Func<Task<Result<IReadOnlyList<string>>>> act1 = async () =>
        //            await scanerEngine.GetFilesFromPathAsync(null, new HandledFileExtension(), cts.Token);

        //        act1.Should().Throw<ArgumentNullException>();
        //    }

        //    [Fact]
        //    public async void GetFilesPathAsyncPathEmptyArguments()
        //    {
        //        var scanerEngine = new FileScanner();
        //        using var cts = new CancellationTokenSource();

        //        Func<Task<Result<IReadOnlyList<string>>>> act1 = async () =>
        //            await scanerEngine.GetFilesFromPathAsync(String.Empty, new HandledFileExtension(), cts.Token);

        //        act1.Should().Throw<ArgumentException>();
        //    }

        //    [Fact]
        //    public async void GetFilesPathAsyncHandledFormatsNull()
        //    {
        //        var scanerEngine = new FileScanner();
        //        using var cts = new CancellationTokenSource();

        //        Func<Task<Result<IReadOnlyList<string>>>> act1 = async () =>
        //            await scanerEngine.GetFilesFromPathAsync(@"C:\test.mp4", null, cts.Token);

        //        act1.Should().Throw<ArgumentNullException>();
        //    }

        //    [Fact]
        //    public async void GetFilesPathAsyncFakePath()
        //    {
        //        var scanerEngine = new FileScanner();
        //        using var cts = new CancellationTokenSource();

        //        var findResult = await scanerEngine.GetFilesFromPathAsync(
        //            @"C:\Test1234\Elo",
        //            new HandledFileExtension(),
        //            cts.Token);

        //        findResult.IsSuccess.Should().BeFalse();
        //    }

        //    [Fact]
        //    public async void EmptyFolder()
        //    {
        //        var tuturialPath = GetTutorial_2_Path();
        //        using var cts = new CancellationTokenSource();

        //        Directory.Exists(tuturialPath).Should().BeTrue();

        //        var scanerEngine = new FileScanner();
        //        var files = await scanerEngine.GetFilesFromPathAsync(
        //            tuturialPath,
        //            new HandledFileExtension(),
        //            cts.Token);
        //        files.IsSuccess.Should().BeTrue();
        //        files.Value.Should().BeEmpty();
        //    }

        //    [Fact]
        //    public async void FindSampleFile()
        //    {
        //        var tuturialPath = GetTutorial_1_Path();
        //        using var cts = new CancellationTokenSource();

        //        Directory.Exists(tuturialPath).Should().BeTrue();

        //        var scanerEngine = new FileScanner();
        //        var files = await scanerEngine.GetFilesFromPathAsync(
        //            tuturialPath,
        //            new HandledFileExtension(),
        //            cts.Token);
        //        files.IsSuccess.Should().BeTrue();
        //        files.Value.Should().ContainMatch($"*{SampleMediaFile1}");
        //    }

        //    private string GetTutorial_1_Path()
        //        => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, MediaFolder, Tutorial_1);

        //    private string GetTutorial_2_Path()
        //        => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, MediaFolder, Tutorial_2);
    }
}
