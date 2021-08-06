using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Entities;
using dataneo.TutorialLibs.Domain.Enums;
using dataneo.TutorialLibs.Persistence.EF.SQLite.Respositories;
using System.Threading.Tasks;

namespace dataneo.TutorialsLibs.WPF.Actions
{
    public static class ChangeTutorialRatingAction
    {
        public static async Task<Result> ChangeAsync(int tutoriaId, RatingStars ratingStars)
        {
            using var respository = new TutorialRespositoryAsync();
            return await Result.Success(respository)
                .OnSuccessTry(repo => repo.GetByIdAsync(tutoriaId), error => error.Message)
                .Map(tutorial => (Maybe<Tutorial>)tutorial)
                .Map(maybeTutorial => maybeTutorial.ToResult("Nie znaleziono tutorialu o id {tutoriaId.ToString()}"))
                .Bind(b => b)
                .Tap(tut => tut.SetRating(ratingStars))
                .OnSuccessTry(tut => respository.UpdateAsync(tut), error => error.Message);
        }
    }
}
