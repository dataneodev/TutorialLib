using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.DTO;
using dataneo.TutorialLibs.Domain.Enums;
using dataneo.TutorialLibs.Persistence.EF.SQLite.Respositories;
using dataneo.TutorialsLib.WPF.Actions;
using dataneo.TutorialsLib.WPF.Comparers;
using dataneo.TutorialsLib.WPF.UI.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace dataneo.TutorialsLib.WPF.UI
{
    internal sealed class MainWindowVM : BaseViewModel
    {
        private readonly Window _parentHandle;

        public Action<bool> SetWindowVisibility;

        public ObservableCollection<TutorialHeaderDto> Tutorials { get; } = new ObservableCollection<TutorialHeaderDto>();

        private TutorialsOrderType selectedTutorialsOrderType = TutorialsOrderType.ByDateAdd;
        public TutorialsOrderType SelectedTutorialsOrderType
        {
            get { return selectedTutorialsOrderType; }
            set
            {
                selectedTutorialsOrderType = value;
                Notify();
            }
        }

        public ICommand RatingChangedCommand { get; }
        public ICommand PlayTutorialCommand { get; }
        public ICommand AddTutorialCommand { get; }
        public ICommand SearchForUpdateCommand { get; }
        public ICommand SearchForNewTutorialsCommand { get; }

        public MainWindowVM(Window paretnHandle)
        {
            this._parentHandle = Guard.Against.Null(paretnHandle, nameof(paretnHandle));
            this.RatingChangedCommand = new Command<ValueTuple<Guid, RatingStars>>(RatingChangedCommandImpl);
            this.PlayTutorialCommand = new Command<Guid>(PlayTutorialCommandImpl);
            this.AddTutorialCommand = new Command(AddTutorialCommandImplAsync);
        }

        private async void AddTutorialCommandImplAsync()
        {
            await Result
               .Try(() => new AddNewTutorialAction(this._parentHandle).AddAsync(), e => e.Message)
               .Bind(r => r)
               .Tap(() => LoadTutorialsDtoAsync(this.SelectedTutorialsOrderType))
               .OnFailure(error => ErrorWindow.ShowError(this._parentHandle, error));
        }

        private void PlayTutorialCommandImpl(Guid tutorialId)
        {
            this.SetWindowVisibility?.Invoke(false);
            var playerWindow = new PlayerWindow(tutorialId, () => ClosePlayerWindow(tutorialId));
            playerWindow.Load();
        }

        private void RatingChangedCommandImpl(ValueTuple<Guid, RatingStars> tutorialIdAndRating)
        {

        }

        private void ClosePlayerWindow(Guid playedTutorialId)
        {
            this.SetWindowVisibility?.Invoke(true);
        }

        public async Task LoadTutorialsDtoAsync(TutorialsOrderType tutorialsOrderType)
        {
            using var repo = new TutorialRespositoryAsync();
            try
            {
                var tutorialHeaders = await repo.GetTutorialHeadersDtoByIdAsync();
                SetNewTutorialsHeader(tutorialHeaders, tutorialsOrderType);
            }
            catch (Exception e)
            {

            }


        }

        private void SetNewTutorialsHeader(IReadOnlyList<TutorialHeaderDto> tutorialHeaders,
                                           TutorialsOrderType tutorialsOrderType)
        {
            var comparer = TutorialsOrderComparerFactory.GetComparer(tutorialsOrderType);

            this.Tutorials.Clear();
            foreach (var tutorialHeader in tutorialHeaders.OrderBy(o => o, comparer))
            {
                this.Tutorials.Add(tutorialHeader);
            }
        }
    }
}
