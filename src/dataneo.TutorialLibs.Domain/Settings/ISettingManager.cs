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
        Task SetIntValueAsync(ISettingDef settingDef, int value);
        Task SetIntCollectionValueAsync(ISettingDef settingDef, IReadOnlyList<int> value);
        Task SetDoubleValueAsync(ISettingDef settingDef, double value);
        Task SetStringValueAsync(ISettingDef settingDef, string value);
    }
}
