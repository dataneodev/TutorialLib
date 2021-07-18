using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.DTO;
using dataneo.TutorialLibs.Domain.Enums;
using dataneo.TutorialLibs.Persistence.EF.SQLite.Respositories;
using dataneo.TutorialsLib.WPF.Actions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
               .Tap(() => LoadTutorialsDtoAsync())
               .OnFailure(error => MessageBox.Show(error));
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

        public async Task LoadTutorialsDtoAsync()
        {
            using var repo = new TutorialRespositoryAsync();
            try
            {
                var tutorialHeaders = await repo.GetTutorialHeadersDtoByIdAsync();
                SetNewTutorialsHeader(tutorialHeaders);
            }
            catch (Exception e)
            {

            }


        }

        private void SetNewTutorialsHeader(IReadOnlyList<TutorialHeaderDto> tutorialHeaders)
        {
            this.Tutorials.Clear();
            foreach (var tutorialHeader in tutorialHeaders)
            {
                this.Tutorials.Add(tutorialHeader);
            }
        }
    }
}
