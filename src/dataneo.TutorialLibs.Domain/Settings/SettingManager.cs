using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.Domain.Settings
{
    public sealed class SettingManager : ISettingManager
    {
        private const char CollectionSeparator = ';';

        private readonly ISettingRespositoryAsync _settingRespositoryAsync;

        public SettingManager(ISettingRespositoryAsync settingRespositoryAsync)
        {
            this._settingRespositoryAsync = Guard.Against.Null(settingRespositoryAsync, nameof(settingRespositoryAsync));
        }

        public async Task<Maybe<double>> GetValueAsDoubleAsync(ISettingDef settingDef)
        {
            var stringValue = await GetValueAsStringAsync(settingDef, ValueType.ValueOfDouble)
                                        .ConfigureAwait(false);
            if (stringValue.HasNoValue)
                return Maybe<double>.None;

            if (double.TryParse(stringValue.GetValueOrThrow(), out double returnValue))
                return returnValue;

            return Maybe<double>.None;
        }

        public async Task<Maybe<int>> GetValueAsIntAsync(ISettingDef settingDef)
        {
            var stringValue = await GetValueAsStringAsync(settingDef, ValueType.ValueOfInteger)
                                        .ConfigureAwait(false);
            if (stringValue.HasNoValue)
                return Maybe<int>.None;

            if (int.TryParse(stringValue.GetValueOrThrow(), out int returnValue))
                return returnValue;

            return Maybe<int>.None;
        }

        public async Task<Maybe<IReadOnlyList<int>>> GetValueIntCollectionAsync(ISettingDef settingDef)
        {
            var stringValue = await GetValueAsStringAsync(settingDef, ValueType.ValueOfArrayInteger)
                                        .ConfigureAwait(false);
            if (stringValue.HasNoValue)
                return Maybe<IReadOnlyList<int>>.None;

            var parsedValues = DeconstructIntCollection(stringValue.GetValueOrThrow());
            if (parsedValues.IsFailure)
                throw new ArgumentException();

            return Maybe<IReadOnlyList<int>>.From(parsedValues.Value);
        }

        private Result<IReadOnlyList<int>> DeconstructIntCollection(string value)
        {
            var strArr = value?.Split(CollectionSeparator, StringSplitOptions.RemoveEmptyEntries |
                                                           StringSplitOptions.TrimEntries) ?? new string[0];
            var parsedValue = strArr.Select(s => ParseIntValue(s)).ToArray();
            if (parsedValue.Any(a => a.HasNoValue))
                return Result.Failure<IReadOnlyList<int>>("Invalid input");
            return parsedValue.Select(s => s.GetValueOrThrow()).ToArray();
        }

        private Maybe<int> ParseIntValue(string value)
        {
            if (int.TryParse(value, out int returnValue))
                return returnValue;
            return Maybe<int>.None;
        }

        public Task<Maybe<string>> GetValueAsStringAsync(ISettingDef settingDef)
            => GetValueAsStringAsync(settingDef, ValueType.ValueOfString);

        private Task<Maybe<string>> GetValueAsStringAsync(ISettingDef settingDef, ValueType valueType)
        {
            Guard.Against.Null(settingDef, nameof(settingDef));
            if (settingDef.ValueType != valueType)
                return Task.FromResult(Maybe<string>.None);

            return GetValueFromSettingManagerAsync(settingDef);
        }

        public Task SetDoubleValue(ISettingDef settingDef, double value)
        {
            GuardSave(settingDef, ValueType.ValueOfDouble);
            return SetValueToSettingManagerAsync(settingDef, value.ToString());
        }

        public Task SetIntValue(ISettingDef settingDef, int value)
        {
            GuardSave(settingDef, ValueType.ValueOfInteger);
            return SetValueToSettingManagerAsync(settingDef, value.ToString());
        }

        public Task SetIntCollectionValue(ISettingDef settingDef, IReadOnlyList<int> value)
        {
            GuardSave(settingDef, ValueType.ValueOfArrayInteger);
            return SetValueToSettingManagerAsync(settingDef, String.Join(CollectionSeparator, value));
        }

        public Task SetStringValue(ISettingDef settingDef, string value)
        {
            GuardSave(settingDef, ValueType.ValueOfString);
            return SetValueToSettingManagerAsync(settingDef, value);
        }

        private static void GuardSave(ISettingDef settingDef, ValueType valueType)
        {
            Guard.Against.Null(settingDef, nameof(settingDef));
            if (settingDef.ValueType != valueType)
                throw new InvalidOperationException();
        }

        private async Task<Maybe<string>> GetValueFromSettingManagerAsync(ISettingDef settingDef)
        {
            var settingManager = await GetSettingAsync().ConfigureAwait(false);
            return settingManager.GetSettingItem(settingDef.Name).Map(s => s.Value);
        }

        private async Task SetValueToSettingManagerAsync(ISettingDef settingDef, string value)
        {
            var settingManager = await GetSettingAsync().ConfigureAwait(false);
            var settingItem = settingManager.GetSettingItem(settingDef.Name);
            if (settingItem.HasValue)
            {
                settingItem.GetValueOrThrow().UpdateValue(value);
                await this._settingRespositoryAsync.UpdateAsync(settingManager).ConfigureAwait(false);
                return;
            }

            var newettingItem = new SettingItem(settingDef.Name, value);
            settingManager.AddSettingItem(newettingItem);
            await this._settingRespositoryAsync.UpdateAsync(settingManager).ConfigureAwait(false);
        }

        private async Task<Setting> GetSettingAsync()
        {
            var setting = await this._settingRespositoryAsync.GetByIdAsync(Setting.DefaultId)
                                                                         .ConfigureAwait(false);
            if (setting is not null)
                return setting;

            var newSetting = new Setting();
            await this._settingRespositoryAsync.AddAsync(newSetting).ConfigureAwait(false);
            return newSetting;
        }
    }
}
