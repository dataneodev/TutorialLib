using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Enums;
using System;
using System.Threading.Tasks;

namespace dataneo.TutorialsLib.WPF.Actions
{
    public static class ChangeTutorialRatingAction
    {
        public static Task<Result> ChangeratingForTutorialAsync(Guid tutoriaId, RatingStars ratingStars)
        {

            return Task.FromResult(Result.Failure("Not implement yet"));
        }
    }
}
