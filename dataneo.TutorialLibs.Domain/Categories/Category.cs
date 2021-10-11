﻿using CSharpFunctionalExtensions;
using dataneo.SharedKernel;
using dataneo.TutorialLibs.Domain.Translation;
using dataneo.TutorialLibs.Domain.Tutorials;
using System.Collections.Generic;

namespace dataneo.TutorialLibs.Domain.Categories
{
    public sealed class Category : BaseEntity, IAggregateRoot
    {
        public const short MaxCategoryName = 48;
        public const short MinCategoryName = 1;

        public string Name { get; private set; }

        private readonly ICollection<Tutorial> _tutorials = new List<Tutorial>();

        private Category() { }

        public static Result<Category> Create(string name)
        {
            var nameValidate = ValidateName(name);
            if (nameValidate.IsFailure)
                return nameValidate.ConvertFailure<Category>();

            return new Category
            {
                Name = name.Trim()
            };
        }

        public static Result ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure(Errors.INVALID_CATEGORY_NAME);

            if (name.Contains("|"))
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
            var nameValidate = ValidateName(name);
            if (nameValidate.IsFailure)
                return nameValidate;

            this.Name = name.Trim();
            return Result.Success();
        }

        public override string ToString() => this.Name;
    }
}
