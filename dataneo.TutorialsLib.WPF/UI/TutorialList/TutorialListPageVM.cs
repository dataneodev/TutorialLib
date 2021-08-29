using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.Extensions;
using dataneo.TutorialLibs.Domain.DTO;
using dataneo.TutorialLibs.Domain.Enums;
using dataneo.TutorialLibs.Persistence.EF.SQLite.Respositories;
using dataneo.TutorialLibs.WPF.Actions;
using dataneo.TutorialLibs.WPF.Comparers;
using dataneo.TutorialLibs.WPF.UI.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace dataneo.TutorialLibs.WPF.UI.TutorialList
{
    internal sealed class TutorialListPageVM : BaseViewModel
    {
        private readonly MainWindow _parentHandle;

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

        private IEnumerable<CategoryMenuItem> _categories = new List<CategoryMenuItem>()
        {
            CategoryMenuItem.CreateForAll(),
            CategoryMenuItem.CreateForNoCategory()
        };
        public IEnumerable<CategoryMenuItem> Categories
        {
            get { return _categories; }
            set { _categories = value; Notify(); }
        }

        public ICommand RatingChangedCommand { get; }
        public ICommand PlayTutorialCommand { get; }
        public ICommand AddTutorialCommand { get; }
        public ICommand SearchForUpdateCommand { get; }
        public ICommand SearchForNewTutorialsCommand { get; }
        public ICommand FilterByCategory { get; }

        public TutorialListPageVM(MainWindow parentHandle)
        {
            this._parentHandle = Guard.Against.Null(parentHandle, nameof(parentHandle));
            this.RatingChangedCommand = new Command<ValueTuple<int, RatingStars>>(RatingChangedCommandImpl);
            this.PlayTutorialCommand = new Command<int>(PlayTutorialCommandImpl);
            this.AddTutorialCommand = new Command(AddTutorialCommandImplAsync);
            this.FilterByCategory = new Command<CategoryMenuItem>(FilterByCategoryImplAsync);
        }

        private async void AddTutorialCommandImplAsync()
            => await Result
               .Try(() => new AddNewTutorialAction(this._parentHandle).AddAsync(), e => e.Message)
               .Bind(r => r)
               .OnSuccessTry(() => LoadTutorialsDtoAsync(this.SelectedTutorialsOrderType))
               .OnFailure(error => ErrorWindow.ShowError(this._parentHandle, error));

        private async void PlayTutorialCommandImpl(int tutorialId)
            => await Result.Try(async () => this._parentHandle.PlayTutorialAsync(tutorialId), exception => exception.Message)
                        .OnFailure(error => ErrorWindow.ShowError(this._parentHandle, error));

        private async void RatingChangedCommandImpl(ValueTuple<int, RatingStars> tutorialIdAndRating)
            => await Result
                .Success(tutorialIdAndRating)
                .Map(input => ChangeTutorialRatingAction.ChangeAsync(input.Item1, input.Item2))
                .Bind(r => r)
                .OnFailure(error => ErrorWindow.ShowError(this._parentHandle, error));

        private void FilterByCategoryImplAsync(CategoryMenuItem obj)
        {




        }

        public async Task LoadTutorialsDtoAsync(TutorialsOrderType tutorialsOrderType)
        {
            using var repo = new TutorialRespositoryAsync();
            var tutorialHeaders = await repo.GetAllTutorialHeadersDtoAsync();
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
