using dataneo.TutorialLibs.Domain.Tutorials;
using System;
using System.Collections.Generic;
using System.IO;

namespace dataneo.TutorialLibs.FileIO.Win.Services
{
    public sealed class HandledFileExtension : IHandledFileExtension
    {
        public readonly static HashSet<string> _handledFileExtensions =
            new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
        {
            "avi",
            "mp4",
            "mkv",
            "mov",
            "webm",
        };

        public HashSet<string> HandledFileExtensions => _handledFileExtensions;

        public bool ExtensionAreSupported(string fileExtension)
        {
            if (string.IsNullOrWhiteSpace(fileExtension))
                return false;

            return HandledFileExtensions.Contains(fileExtension);
        }

        public bool FileAreSupported(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return false;

            var fileExtension = Path.GetExtension(fileName.AsSpan()).TrimStart('.');
            return HandledFileExtensions.Contains(fileExtension.ToString());
        }
    }
}
