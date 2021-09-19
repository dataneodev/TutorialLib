﻿using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Translation;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.Domain.Categories.Services
{
    public class AddCategory
    {
        private readonly ICategoryRespositoryAsync _categoryRespositoryAsync;

        public AddCategory(ICategoryRespositoryAsync categoryRespositoryAsync)
        {
            this._categoryRespositoryAsync = Guard.Against.Null(categoryRespositoryAsync, nameof(categoryRespositoryAsync));
        }

        public async Task<Result> AddNewCategory(Category category)
           => await Result
                .Try(() => this._categoryRespositoryAsync.CountAsync(new CategoryWithName(category)))
                .Ensure(count => count == 0, Errors.CATEGORY_NAME_EXISTS)
                .OnSuccessTry(count => this._categoryRespositoryAsync.AddAsync(category));
    }
}
