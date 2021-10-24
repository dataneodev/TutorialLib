namespace dataneo.TutorialLibs.Domain.Settings
{
    public interface ISettingDef
    {
        string Name { get; }
        ValueType ValueType { get; }
    }
}
