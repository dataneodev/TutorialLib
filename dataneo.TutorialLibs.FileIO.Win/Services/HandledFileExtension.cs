using dataneo.TutorialLibs.Domain.Interfaces;
using System.Collections.Generic;

namespace dataneo.TutorialLibs.FileIO.Win.Services
{
    public sealed class HandledFileExtension : IHandledFileExtension
    {
        public HashSet<string> HandledFileExtensions { get; }

        public bool ExtensionAreSupported(string fileExtension)
        {
            throw new System.NotImplementedException();
        }

        public bool FileAreSupported(string fileName)
        {
            throw new System.NotImplementedException();
        }
    }
}
