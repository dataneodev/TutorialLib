using TutorialLibs.SharedKernel.Common;
using TutorialLibs.SharedKernel.Interfaces;

namespace TutorialLibs.Domain.Entities
{
    public class Tutorial : BaseEntity, IAggregateRoot
    {
        public string Name { get; set; }
        public string FolderPath { get; set; }
        public Episode[] Episodes { get; set; }
    }
}
