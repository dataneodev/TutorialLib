using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Translation;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.Domain.Categories
{
    public class UpdateCategory
    {
        private readonly ICategoryRespositoryAsync _categoryRespositoryAsync;

        public UpdateCategory(ICategoryRespositoryAsync categoryRespositoryAsync)
        {
            this._categoryRespositoryAsync = Guard.Against.Null(categoryRespositoryAsync, nameof(categoryRespositoryAsync));
        }

        public async Task<Result> UpdateCategoryNameAsync(Category category, string newName)
        {
            Guard.Against.Null(category, nameof(category));
            return await category
                .SetName(newName)
                .OnSuccessTry(() => this._categoryRespositoryAsync.CountAsync(new CategoryWithNameSpecification(category)))
                .Ensure(count => count == 0, Errors.CATEGORY_NAME_EXISTS)
                .OnSuccessTry(count => this._categoryRespositoryAsync.UpdateAsync(category), error => error.InnerException?.Message);
        }
    }
}
