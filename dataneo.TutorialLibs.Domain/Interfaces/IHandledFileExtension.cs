using System.Collections.Generic;

namespace dataneo.TutorialLibs.Domain.Interfaces
{
    public interface IHandledFileExtension
    {
        HashSet<string> HandledFileExtensions { get; }
    }
}
