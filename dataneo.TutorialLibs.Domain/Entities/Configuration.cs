using Ardalis.GuardClauses;
using dataneo.SharedKernel;

namespace dataneo.TutorialLibs.Domain.Entities
{
    public class Configuration : BaseEntity
    {
        private Configuration() { }

        public Configuration(string name, string value)
        {
            Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Value = value;
        }

        public string Name { get; private set; }
        public string Value { get; private set; }
    }
}
