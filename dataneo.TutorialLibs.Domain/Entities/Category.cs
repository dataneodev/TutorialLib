﻿using CSharpFunctionalExtensions;
using dataneo.SharedKernel;
using dataneo.TutorialLibs.Domain.Translation;
using System.Collections.Generic;

namespace dataneo.TutorialLibs.Domain.Entities
{
    public sealed class Category : BaseEntity
    {
        public const short MaxCategoryName = 32;
        public const short MinCategoryName = 1;

        public string Name { get; private set; }
        public IReadOnlyList<Tutorial> Tutorials { get; private set; }

        private Category() { }

        public static Result<Category> Create(string name)
        {
            var nameValidate = Validate(name);
            if (nameValidate.IsFailure)
                return nameValidate.ConvertFailure<Category>();

            return new Category
            {
                Name = name.Trim()
            };
        }

        private static Result Validate(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure(Errors.INVALID_CATEGORY_NAME);

            var trimedName = name.Trim();
            if (trimedName.Length > MaxCategoryName)
                return Result.Failure(Errors.INVALID_CATEGORY_NAME);

            if (trimedName.Length < MinCategoryName)
                return Result.Failure(Errors.INVALID_CATEGORY_NAME);

            return Result.Success();
        }

        public Result SetName(string name)
        {
            var nameValidate = Validate(name);
            if (nameValidate.IsFailure)
                return nameValidate;

            this.Name = name.Trim();
            return Result.Success();
        }
    }
}