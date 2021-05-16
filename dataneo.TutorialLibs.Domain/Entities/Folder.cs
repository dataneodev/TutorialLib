using dataneo.SharedKernel;

namespace dataneo.TutorialLibs.Domain.Entities
{
    public sealed class Folder : BaseEntity
    {
        public int ParentTutorialId { get; set; }
        public short Order { get; set; }
        public string Name { get; set; }
        public string FolderName { get; set; }
    }
}
