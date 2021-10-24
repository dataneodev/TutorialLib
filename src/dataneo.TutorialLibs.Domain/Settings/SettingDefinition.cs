namespace dataneo.TutorialLibs.Domain.Settings
{
    public static class SettingDefinition
    {
        public static ISettingDef LastCategory = new SettingDef(nameof(LastCategory), null, ValueType.Integer, true);
    }
}
