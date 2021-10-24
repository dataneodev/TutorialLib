using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.Domain.Settings
{
    public interface ISettingManager
    {
        Task<Maybe<int>> GetValueAsIntAsync(ISettingDef settingDef);
        Task<Maybe<IReadOnlyList<int>>> GetValueIntCollectionAsync(ISettingDef settingDef);
        Task<Maybe<double>> GetValueAsDoubleAsync(ISettingDef settingDef);
        Task<Maybe<string>> GetValueAsStringAsync(ISettingDef settingDef);
        Task SetIntValue(ISettingDef settingDef, int value);
        Task SetIntCollectionValue(ISettingDef settingDef, IReadOnlyList<int> value);
        Task SetDoubleValue(ISettingDef settingDef, double value);
        Task SetStringValue(ISettingDef settingDef, string value);
    }
}
