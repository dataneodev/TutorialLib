using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.Extensions;
using dataneo.TutorialLibs.Domain.DTO;
using dataneo.TutorialLibs.Domain.Enums;
using dataneo.TutorialLibs.Persistence.EF.SQLite.Respositories;
using dataneo.TutorialsLibs.WPF.Actions;
using dataneo.TutorialsLibs.WPF.Comparers;
using dataneo.TutorialsLibs.WPF.UI.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace dataneo.TutorialsLibs.WPF.UI
{
    internal sealed class MainWindowVM : BaseViewModel
    {
        private readonly Window _parentHandle;

        public Action<bool> SetWindowVisibility;

        private IEnumerable<TutorialHeaderDto> tutorials;
        public IEnumerable<TutorialHeaderDto> Tutorials
        {
            get { return tutorials; }
            set { tutorials = value; Notify(); }
        }

        private TutorialsOrderType selectedTutorialsOrderType = TutorialsOrderType.ByDateAdd;
        public TutorialsOrderType SelectedTutorialsOrderType
        {
            get { return selectedTutorialsOrderType; }
            set
            {
                selectedTutorialsOrderType = value;
                Notify();
                SetNewTutorialsHeader(Tutorials, value);
            }
        }

        public ICommand RatingChangedCommand { get; }
        public ICommand PlayTutorialCommand { get; }
        public ICommand AddTutorialCommand { get; }
        public ICommand SearchForUpdateCommand { get; }
        public ICommand SearchForNewTutorialsCommand { get; }

        public MainWindowVM(Window parentHandle)
        {
            this._parentHandle = Guard.Against.Null(parentHandle, nameof(parentHandle));
            this.RatingChangedCommand = new Command<ValueTuple<int, RatingStars>>(RatingChangedCommandImpl);
            this.PlayTutorialCommand = new Command<int>(PlayTutorialCommandImpl);
            this.AddTutorialCommand = new Command(AddTutorialCommandImplAsync);
        }

        private async void AddTutorialCommandImplAsync()
            => await Result
               .Try(() => new AddNewTutorialAction(this._parentHandle).AddAsync(), e => e.Message)
               .Bind(r => r)
               .OnSuccessTry(() => LoadTutorialsDtoAsync(this.SelectedTutorialsOrderType))
               .OnFailure(error => ErrorWindow.ShowError(this._parentHandle, error));

        private async void PlayTutorialCommandImpl(int tutorialId)
        {
            this.SetWindowVisibility?.Invoke(false);
            var playerWindow = new PlayerWindow(tutorialId, () => ClosePlayerWindow(tutorialId));
            await Result.Try(async () => await playerWindow.LoadAsync());
        }

        private async void RatingChangedCommandImpl(ValueTuple<int, RatingStars> tutorialIdAndRating)
            => await Result
                .Success(tutorialIdAndRating)
                .Map(input => ChangeTutorialRatingAction.ChangeAsync(input.Item1, input.Item2))
                .Bind(r => r)
                .OnFailure(error => ErrorWindow.ShowError(this._parentHandle, error));

        private void ClosePlayerWindow(int playedTutorialId)
        {
            this.SetWindowVisibility?.Invoke(true);
        }

        public async Task LoadTutorialsDtoAsync(TutorialsOrderType tutorialsOrderType)
        {
            using var repo = new TutorialRespositoryAsync();
            var tutorialHeaders = await repo.GetTutorialHeadersDtoByIdAsync();
            SetNewTutorialsHeader(tutorialHeaders, tutorialsOrderType);

        }

        private void SetNewTutorialsHeader(IEnumerable<TutorialHeaderDto> tutorialHeaders,
                                           TutorialsOrderType tutorialsOrderType)
        {
            var comparer = TutorialsOrderComparerFactory.GetComparer(tutorialsOrderType);
            this.Tutorials = tutorialHeaders.OrderBy(o => o, comparer)
                                            .ToArray();
        }
    }
}
