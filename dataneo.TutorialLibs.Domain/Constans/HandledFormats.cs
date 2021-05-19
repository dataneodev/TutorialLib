using System;
using System.Collections.Generic;
using System.IO;

namespace dataneo.TutorialLibs.Domain.Constans
{
    public static class HandledFormats
    {
        public readonly static HashSet<string> HandledFileExtensions =
            new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
        {
            "avi",
            "mp4",
            "mkv",
            "mov"
        };

        public static bool ExtensionAreSupported(string fileExtension)
        {
            if (string.IsNullOrWhiteSpace(fileExtension))
                return false;

            return HandledFileExtensions.Contains(fileExtension);
        }

        public static bool FileAreSupported(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return false;

            var fileExtension = Path.GetExtension(fileName.AsSpan()).TrimStart('.');
            return HandledFileExtensions.Contains(fileExtension.ToString());
        }
    }
}
