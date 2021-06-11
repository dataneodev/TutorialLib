using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Entities;
using dataneo.TutorialLibs.Domain.Interfaces;
using dataneo.TutorialLibs.Domain.Interfaces.Respositories;
using dataneo.TutorialLibs.Domain.ValueObjects;
using Serilog;
using System.Threading;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.Domain.Services
{
    public sealed class AddTutorial
    {
        private readonly IFileScanner _fileScanner;
        private readonly IMediaInfoProvider _mediaInfoProvider;
        private readonly IHandledFileExtension _handledFileExtension;
        private readonly ITutorialRespositoryAsync _tutorialRespositoryAsync;
        private readonly ILogger _logger;

        public AddTutorial(IFileScanner fileScanner,
                            IMediaInfoProvider mediaInfoProvider,
                            IHandledFileExtension handledFileExtension,
                            ITutorialRespositoryAsync tutorialRespositoryAsync,
                            ILogger logger)
        {
            this._fileScanner = Guard.Against.Null(fileScanner, nameof(fileScanner));
            this._mediaInfoProvider = Guard.Against.Null(mediaInfoProvider, nameof(mediaInfoProvider));
            this._handledFileExtension = Guard.Against.Null(handledFileExtension, nameof(handledFileExtension));
            this._tutorialRespositoryAsync = Guard.Against.Null(tutorialRespositoryAsync, nameof(tutorialRespositoryAsync));
            this._logger = Guard.Against.Null(logger, nameof(logger));
        }

        public async Task<Result<Tutorial>> AddTutorialAsync(DirectoryPath tutorialPath, CancellationToken cancelationToken = default)
        {
            Guard.Against.Null(tutorialPath, nameof(tutorialPath));



            return Result.Failure<Tutorial>("Not implement");
        }
    }
}
