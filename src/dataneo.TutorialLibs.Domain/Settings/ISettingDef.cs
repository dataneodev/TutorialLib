namespace dataneo.TutorialLibs.Domain.Settings
{
    public interface ISettingDef
    {
        string Name { get; }
        string DefaultValue { get; }
        ValueType ValueType { get; }
        bool IsNull { get; }
    }
}
