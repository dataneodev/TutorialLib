using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Translation;
using System.Threading;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.Domain.Tutorials
{
    public sealed class AddTutorial : IAddTutorial
    {
        private readonly IFileScanner _fileScanner;
        private readonly IMediaInfoProvider _mediaInfoProvider;
        private readonly ITutorialRespositoryAsync _tutorialRespositoryAsync;
        private readonly ILogger _logger;
        private readonly IDateTimeProivder _dateTimeProivder;

        public AddTutorial(IFileScanner fileScanner,
                            IMediaInfoProvider mediaInfoProvider,
                            ITutorialRespositoryAsync tutorialRespositoryAsync,
                            IDateTimeProivder dateTimeProivder,
                            ILogger logger)
        {
            this._fileScanner = Guard.Against.Null(fileScanner, nameof(fileScanner));
            this._mediaInfoProvider = Guard.Against.Null(mediaInfoProvider, nameof(mediaInfoProvider));
            this._tutorialRespositoryAsync = Guard.Against.Null(tutorialRespositoryAsync, nameof(tutorialRespositoryAsync));
            this._logger = Guard.Against.Null(logger, nameof(logger));
            this._dateTimeProivder = Guard.Against.Null(dateTimeProivder, nameof(dateTimeProivder));
        }

        public async Task<Result<Tutorial>> AddTutorialAsync(DirectoryPath tutorialPath, CancellationToken cancelationToken = default)
        {
            Guard.Against.Null(tutorialPath, nameof(tutorialPath));

            if (await CheckIfDirectoryPathUsed(tutorialPath, cancelationToken).ConfigureAwait(false))
                return Result.Failure<Tutorial>(Errors.TUTORIAL_PATH_ALREADY_USED);

            var tutorialFolderProcessor = new TutorialCreator(
                                            fileScanner: this._fileScanner,
                                            mediaInfoProvider: this._mediaInfoProvider,
                                            dateTimeProivder: this._dateTimeProivder);

            var folderTutrialResult = await tutorialFolderProcessor.GetTutorialForFolderAsync(
                                                    tutorialPath,
                                                    cancelationToken)
                                                .ConfigureAwait(false);

            return await folderTutrialResult
                .Bind(result => result.ToResult(Errors.NO_FILES_IN_DIRECTORY))
                .OnSuccessTry(tutorial => this._tutorialRespositoryAsync.AddAsync(tutorial, cancelationToken),
                                                exception => exception?.InnerException?.Message)
                .ConfigureAwait(false);
        }

        private async Task<bool> CheckIfDirectoryPathUsed(DirectoryPath directoryPath, CancellationToken cancelationToken)
        {
            var spec = new TutorialsForDirectoryPathSpecification(directoryPath);
            Maybe<Tutorial> findResult = await this._tutorialRespositoryAsync
                                                        .FirstOrDefaultAsync(spec, cancelationToken)
                                                        .ConfigureAwait(false);
            return findResult.HasValue;
        }
    }
}
