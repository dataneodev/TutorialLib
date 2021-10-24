using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using System;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.Domain.Settings
{
    public sealed class SettingManager : ISettingManager
    {
        private readonly ISettingRespositoryAsync _settingRespositoryAsync;

        public SettingManager(ISettingRespositoryAsync settingRespositoryAsync)
        {
            this._settingRespositoryAsync = Guard.Against.Null(settingRespositoryAsync, nameof(settingRespositoryAsync));
        }

        public Task<Maybe<double>> GetValueAsDoubleAsync(ISettingDef settingDef)
        {
            Guard.Against.Null(settingDef, nameof(settingDef));
            if (settingDef.ValueType != ValueType.Double)
                return Maybe<double>.None;


        }

        public Task<Maybe<int>> GetValueAsIntAsync(ISettingDef settingDef)
        {
            Guard.Against.Null(settingDef, nameof(settingDef));
            if (settingDef.ValueType != ValueType.Integer)
                return Maybe<int>.None;
        }

        public Task<Maybe<string>> GetValueAsStringAsync(ISettingDef settingDef)
        {
            Guard.Against.Null(settingDef, nameof(settingDef));
            if (settingDef.ValueType != ValueType.String)
                return Maybe<string>.None;
        }

        public Task SetDoubleValue(ISettingDef settingDef, double value)
        {
            Guard.Against.Null(settingDef, nameof(settingDef));
            if (settingDef.ValueType != ValueType.Double)
                throw new InvalidOperationException($"{settingDef.Name} except only double");

            this._
        }

        public Task SetIntValue(ISettingDef settingDef, int value)
        {
            Guard.Against.Null(settingDef, nameof(settingDef));
            if (settingDef.ValueType != ValueType.Integer)
                throw new InvalidOperationException($"{settingDef.Name} except only integer");

            throw new NotImplementedException();
        }

        public Task SetStringValue(ISettingDef settingDef, string value)
        {
            Guard.Against.Null(settingDef, nameof(settingDef));
            if (settingDef.ValueType != ValueType.String)
                throw new InvalidOperationException($"{settingDef.Name} except only string");

            throw new NotImplementedException();
        }

        private async Task<Maybe<string>> GetValueFromSettingManagerAsync(ISettingDef settingDef)
        {
            var settingManager = await _settingRespositoryAsync.GetByIdAsync(1);
            return settingManager?.GetValue(settingDef.Name);

        }
    }
}
