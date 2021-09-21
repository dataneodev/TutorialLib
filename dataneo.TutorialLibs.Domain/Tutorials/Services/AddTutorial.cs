using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using System.Threading;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.Domain.Tutorials
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

            if (await CheckIfDirectoryPathUsed(tutorialPath, cancelationToken).ConfigureAwait(false))
                return Result.Failure<Tutorial>("Ten katalog jest już uzyty");

            var tutorialFolderProcessor = new TutorialCreator(
                                            fileScanner: this._fileScanner,
                                            mediaInfoProvider: this._mediaInfoProvider,
                                            dateTimeProivder: new DateTimeProivder(),
                                            handledFileExtension: this._handledFileExtension);

            var folderTutrialResult = await tutorialFolderProcessor.GetTutorialForFolderAsync(
                                                    tutorialPath,
                                                    cancelationToken)
                                                .ConfigureAwait(false);

            return await folderTutrialResult
                .Bind(result => result.ToResult("Nie znaleziono żadnych plików w folderze"))
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
