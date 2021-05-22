using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Constans;
using dataneo.TutorialLibs.FileIO.Win.Services;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace dataneo.TutorialLibs.FileIO.WinTests.Services
{
    public class TutorialScanerTests
    {
        private const string MediaFolder = "media";
        private const string SampleMediaFile1 = "file_example_MP4_640_3MG.mp4";

        [Fact]
        public async void GetFilesPathAsyncPathNullArguments()
        {
            var scanerEngine = new TutorialScaner();
            using var cts = new CancellationTokenSource();

            Func<Task<Result<IReadOnlyList<string>>>> act1 = async () =>
                await scanerEngine.GetFilesPathAsync(null, HandledFormats.HandledFileExtensions, cts.Token);

            act1.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async void GetFilesPathAsyncPathEmptyArguments()
        {
            var scanerEngine = new TutorialScaner();
            using var cts = new CancellationTokenSource();

            Func<Task<Result<IReadOnlyList<string>>>> act1 = async () =>
                await scanerEngine.GetFilesPathAsync(String.Empty, HandledFormats.HandledFileExtensions, cts.Token);

            act1.Should().Throw<ArgumentException>();
        }

        [Fact]
        public async void GetFilesPathAsyncHandledFormatsNull()
        {
            var scanerEngine = new TutorialScaner();
            using var cts = new CancellationTokenSource();

            Func<Task<Result<IReadOnlyList<string>>>> act1 = async () =>
                await scanerEngine.GetFilesPathAsync(@"C:\test.mp4", null, cts.Token);

            act1.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async void GetFilesPathAsyncFakePath()
        {
            var scanerEngine = new TutorialScaner();
            using var cts = new CancellationTokenSource();

            var findResult = await scanerEngine.GetFilesPathAsync(@"C:\Test1234\Elo", HandledFormats.HandledFileExtensions, cts.Token);

            findResult.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public async void FindSampleFile()
        {
            var mediaPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, MediaFolder);
            using var cts = new CancellationTokenSource();

            Directory.Exists(mediaPath).Should().BeTrue();

            var scanerEngine = new TutorialScaner();
            var files = await scanerEngine.GetFilesPathAsync(mediaPath, HandledFormats.HandledFileExtensions, cts.Token);
            files.IsSuccess.Should().BeTrue();
            files.Value.Should().ContainMatch($"*{SampleMediaFile1}");
        }

        [Fact]
        public async void GetEpisodeFile()
        {
            var mediaPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, MediaFolder);
            using var cts = new CancellationTokenSource();

            Directory.Exists(mediaPath).Should().BeTrue();

            var scanerEngine = new TutorialScaner();
            var files = await scanerEngine.GetFilesPathAsync(mediaPath, HandledFormats.HandledFileExtensions, cts.Token);
            files.IsSuccess.Should().BeTrue();

            var mediaFile = files.Value.FirstOrDefault(f => f.EndsWith(SampleMediaFile1));
            mediaFile.Should().NotBeNullOrEmpty();

            var result = await scanerEngine.GetFileDetailsAsync(mediaFile, cts.Token);
            result.IsSuccess.Should().BeTrue();

            result.Value.FileName.Should().Be(SampleMediaFile1);
            result.Value.FileSize.Should().Be(3114374);
            result.Value.PlayTime.Should().Be(TimeSpan.FromMilliseconds(30527));
        }
    }
}
