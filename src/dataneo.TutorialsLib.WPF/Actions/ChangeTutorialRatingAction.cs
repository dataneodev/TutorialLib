using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Tutorials;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.WPF.Actions
{
    public static class ChangeTutorialRatingAction
    {
        public static async Task<Result> ChangeAsync(ITutorialRespositoryAsync tutorialRespositoryAsync, int tutoriaId, RatingStars ratingStars)
        {
            Guard.Against.Null(tutorialRespositoryAsync, nameof(tutorialRespositoryAsync));
            Guard.Against.NegativeOrZero(tutoriaId, nameof(tutoriaId));

            return await Result.Success(tutorialRespositoryAsync)
                .OnSuccessTry(repo => repo.GetByIdAsync(tutoriaId), error => error.Message)
                .Map(tutorial => (Maybe<Tutorial>)tutorial)
                .Map(maybeTutorial => maybeTutorial.ToResult("Nie znaleziono tutorialu o id {tutoriaId.ToString()}"))
                .Bind(b => b)
                .Tap(tut => tut.SetRating(ratingStars))
                .OnSuccessTry(tut => tutorialRespositoryAsync.UpdateAsync(tut), error => error.Message);
        }
    }
}
