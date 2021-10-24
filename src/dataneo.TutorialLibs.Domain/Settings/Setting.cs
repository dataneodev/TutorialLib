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
        private List<SettingItem> _settingItems = new List<SettingItem>();
        public IReadOnlyList<SettingItem> SettingItems => new ReadOnlyCollection<SettingItem>(_settingItems);

        public void AddSettingItem(SettingItem settingItem)
        {
            Guard.Against.Null(settingItem, nameof(settingItem));
            if (this._settingItems.Any(a => a.Name.Equals(settingItem.Name, StringComparison.InvariantCultureIgnoreCase)))
                throw new InvalidOperationException("SettingItem with this name exists");

            this._settingItems.Add(settingItem);
        }

        public Maybe<string> GetValue(string name)
        {
            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            return this._settingItems.FirstOrDefault(f => f.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                                     ?.Name;
        }
    }
}
