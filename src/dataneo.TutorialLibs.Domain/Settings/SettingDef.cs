using Ardalis.GuardClauses;

namespace dataneo.TutorialLibs.Domain.Settings
{
    public class SettingDef : ISettingDef
    {
        public SettingDef(string name, ValueType valueType)
        {
            Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
            ValueType = valueType;
        }

        public string Name { get; }
        public ValueType ValueType { get; }
    }
}
