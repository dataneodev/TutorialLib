using Ardalis.GuardClauses;
using System;

namespace dataneo.TutorialLibs.Domain.Settings
{
    public class SettingDef : ISettingDef
    {
        public SettingDef(string name, string defaultValue, ValueType valueType, bool isNull)
        {
            Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
            DefaultValue = defaultValue;
            ValueType = valueType;
            IsNull = isNull;

            if (!isNull && String.IsNullOrWhiteSpace(defaultValue))
                throw new InvalidOperationException("SettingDef must have default value");
        }

        public string Name { get; }
        public string DefaultValue { get; }
        public ValueType ValueType { get; }
        public bool IsNull { get; }
    }
}
