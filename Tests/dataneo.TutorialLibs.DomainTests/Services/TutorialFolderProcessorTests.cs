using dataneo.TutorialLibs.Domain.Services;
using dataneo.TutorialLibs.FileIO.Win.Services;
using FluentAssertions;
using System;
using System.IO;
using System.Threading;
using Xunit;

namespace dataneo.TutorialLibs.DomainTests.Services
{
    public class TutorialFolderProcessorTests
    {
        private const string MediaFolder = "media";
        private const string Tutorial_1 = "Tutorial_1";
        private const string Tutorial_2 = "Tutorial_2_Empty";
        private const string SampleMediaFile1 = "file_example_MP4_640_3MG.mp4";

        [Fact]
        public async void RawTest()
        {
            var processorEngine = GetDefaultEngine();
            using var cts = new CancellationTokenSource();

            var result = await processorEngine.GetTutorialForFolderAsync(
                                    GetTutorial_1_Folder(),
                                    cts.Token);
            result.IsSuccess.Should().BeTrue();

        }

        private TutorialFolderProcessor GetDefaultEngine()
            => new TutorialFolderProcessor(
                fileScanner: new FileScanner(),
                mediaInfoProvider: new MediaInfoProvider(),
                dateTimeProivder: new DateTimeProivder(),
                handledFileExtension: new HandledFileExtension());

        private string GetTutorial_1_Folder()
            => Path.Combine(AppDomain.CurrentDomain.BaseDirectory
                                    .Replace(
                                        @"dataneo.TutorialLibs.DomainTests",
                                        @"dataneo.TutorialLibs.FileIO.WinTests"),
                            MediaFolder, Tutorial_1);

        [Fact]
        public async void Test11()
        {
            var processorEngine = GetDefaultEngine();
            using var cts = new CancellationTokenSource();

            var result = await processorEngine.GetTutorialForFolderAsync(
                                    @"n:\Tutoriale\PluralsightDecrypted\Applying Functional Principles in C#\",
                                    cts.Token);
            result.IsSuccess.Should().BeTrue();

        }
    }
}
