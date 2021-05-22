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
    public class MediaInfoProviderTests
    {
        private const string MediaFolder = "media";
        private const string SampleMediaFile1 = "file_example_MP4_640_3MG.mp4";

        [Fact]
        public async void GetEpisodeFile()
        {
            var mediaPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, MediaFolder);
            using var cts = new CancellationTokenSource();

            Directory.Exists(mediaPath).Should().BeTrue();

            var scanerEngine = new FileScanner();
            var files = await scanerEngine.GetFilesFromPathAsync(mediaPath, HandledFormats.HandledFileExtensions, cts.Token);
            files.IsSuccess.Should().BeTrue();

            var mediaFile = files.Value.FirstOrDefault(f => f.EndsWith(SampleMediaFile1));
            mediaFile.Should().NotBeNullOrEmpty();

            var mediaInfoProvider = new MediaInfoProvider();

            var result = await mediaInfoProvider.GetFileDetailsAsync(mediaFile, cts.Token);
            result.IsSuccess.Should().BeTrue();

            result.Value.FileName.Should().Be(SampleMediaFile1);
            result.Value.FileSize.Should().Be(3114374);
            result.Value.PlayTime.Should().Be(TimeSpan.FromMilliseconds(30527));
        }
    }
}
