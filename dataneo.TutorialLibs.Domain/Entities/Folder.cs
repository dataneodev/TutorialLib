using dataneo.SharedKernel;
using System.Collections.Generic;

namespace dataneo.TutorialLibs.Domain.Entities
{
    public sealed class Folder : BaseEntity
    {
        public int ParentTutorialId { get; set; }
        public short Order { get; set; }
        public string Name { get; set; }
        public string FolderName { get; set; }
        public IReadOnlyList<Episode> Episodes { get; set; }
    }
}
