using dataneo.TutorialLibs.Domain.Constans;
using dataneo.TutorialLibs.FileIO.Win.Services;
using FluentAssertions;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using Xunit;

namespace dataneo.TutorialLibs.FileIO.WinTests.Services
{
    public class TutorialScanerTests
    {
        private const string MediaFolder = "media";
        private const string SampleMediaFile1 = "file_example_MP4_640_3MG.mp4";

        [Fact]
        public async void FindSampleFile()
        {
            var mediaPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, MediaFolder);

            Directory.Exists(mediaPath).Should().BeTrue();

            var scanerEngine = new TutorialScaner();
            var files = await scanerEngine.GetFilesPathAsync(mediaPath, HandledFormats.HandledFileExtensions);
            files.IsSuccess.Should().BeTrue();
            files.Value.Should().ContainMatch($"*{SampleMediaFile1}");
        }

        [Fact]
        public async void GetEpisodeFile()
        {
            var mediaPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, MediaFolder);

            Directory.Exists(mediaPath).Should().BeTrue();

            var scanerEngine = new TutorialScaner();
            var files = await scanerEngine.GetFilesPathAsync(mediaPath, HandledFormats.HandledFileExtensions);
            files.IsSuccess.Should().BeTrue();


            var mediaFile = files.Value.FirstOrDefault(f => f.EndsWith(SampleMediaFile1));
            mediaFile.Should().NotBeNullOrEmpty();

            var cts = new CancellationTokenSource();

            var result = await scanerEngine.GetFileDetailsAsync(mediaFile, cts.Token);
            result.IsSuccess.Should().BeTrue();


            result.Value.FileName.Should().Be(SampleMediaFile1);
            result.Value.FileSize.Should().Be(3114374);
            result.Value.PlayTime.Should().Be(TimeSpan.FromMilliseconds(30527));
        }
    }
}
