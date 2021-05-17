using dataneo.SharedKernel;
using System;
using System.Collections.Generic;

namespace dataneo.TutorialLibs.Domain.Entities
{
    public sealed class Tutorial : BaseEntity, IAggregateRoot
    {
        public string Name { get; set; }
        public string BasePath { get; set; }
        public IReadOnlyList<Episode> Episodes { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime ModifiedTime { get; set; }
    }
}
