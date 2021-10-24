using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.SharedKernel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace dataneo.TutorialLibs.Domain.Settings
{
    public sealed class Setting : BaseEntity, IAggregateRoot
    {
        public const int DefaultId = 1;

        private readonly List<SettingItem> _settingItems = new List<SettingItem>();
        public IReadOnlyList<SettingItem> SettingItems => new ReadOnlyCollection<SettingItem>(this._settingItems);

        public Setting()
        {
            Id = DefaultId;
        }

        public void AddSettingItem(SettingItem settingItem)
        {
            Guard.Against.Null(settingItem, nameof(settingItem));
            if (GetSettingItem(settingItem.Name).HasValue)
                throw new InvalidOperationException("SettingItem with this name exists");

            this._settingItems.Add(settingItem);
        }

        public Maybe<SettingItem> GetSettingItem(ISettingDef settingDef)
        {
            Guard.Against.Null(settingDef, nameof(settingDef));
            return GetSettingItem(settingDef.Name);
        }

        public Maybe<SettingItem> GetSettingItem(string name)
        {
            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            return this._settingItems.FirstOrDefault(f => f.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
