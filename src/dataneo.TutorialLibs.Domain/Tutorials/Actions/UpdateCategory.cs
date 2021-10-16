using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Categories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.Domain.Tutorials.Services
{
    public sealed class UpdateCategory
    {
        private readonly ITutorialRespositoryAsync _tutorialRespositoryAsync;

        public UpdateCategory(ITutorialRespositoryAsync tutorialRespositoryAsync)
        {
            this._tutorialRespositoryAsync = Guard.Against.Null(tutorialRespositoryAsync, nameof(tutorialRespositoryAsync));
        }

        public async Task<Result> UpdateTutorialCategoriesAsync(int tutorialId, IEnumerable<Category> categories)
        {
            Guard.Against.NegativeOrZero(tutorialId, nameof(tutorialId));
            Guard.Against.Null(categories, nameof(categories));

            var tutorial = await this._tutorialRespositoryAsync.GetByIdAsync(tutorialId);
            if (tutorial is null)
                return Result.Failure("Tutorial not found");

            tutorial.SetNewCategories(categories);

            return await Result.Try(
                () => this._tutorialRespositoryAsync.UpdateAsync(tutorial),
                error => error.InnerException?.Message);
        }
    }
}
