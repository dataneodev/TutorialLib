using Ardalis.GuardClauses;
using dataneo.SharedKernel;
using System;

namespace dataneo.TutorialLibs.Domain.Settings
{
    public sealed class SettingItem : BaseEntity
    {
        public const int MaxNameLength = 96;
        public const int MaxValueLength = 2048;

        public string Name { get; private set; }
        public string Value { get; private set; }

        public SettingItem(string name, string value)
        {
            Name = Guard.Against.NullOrWhiteSpace(name, nameof(name)).Trim();
            if (name.Trim().Length > MaxNameLength)
                throw new ArgumentException($"Max length of name is {MaxNameLength}");
            this.Value = value;
        }

        public void UpdateValue(string value)
        {
            if ((value?.Trim()?.Length ?? 0) > MaxValueLength)
                throw new ArgumentException($"Max length of value is {MaxNameLength}");
            this.Value = value.Trim();
        }
    }
}
