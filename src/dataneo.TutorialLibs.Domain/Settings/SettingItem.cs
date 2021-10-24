using Ardalis.GuardClauses;
using dataneo.SharedKernel;

namespace dataneo.TutorialLibs.Domain.Settings
{
    public sealed class SettingItem : BaseEntity
    {
        public string Name { get; private set; }
        public string Value { get; private set; }

        public SettingItem(string name, string value)
        {
            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Guard.Against.NullOrWhiteSpace(value, nameof(value));
        }

        public void UpdateValue(string value)
        {
            this.Value = value;
        }
    }
}
