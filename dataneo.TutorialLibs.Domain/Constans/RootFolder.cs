using dataneo.TutorialLibs.Domain.Entities;

namespace dataneo.TutorialLibs.Domain.Constans
{
    public static class RootFolder
    {
        public static Folder GetRootFolder()
            => new Folder
            {
                FolderName = "Root",
                Name = "",
                Order = 1,
            };
    }
}
