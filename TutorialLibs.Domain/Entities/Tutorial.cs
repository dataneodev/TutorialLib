using dataneo.DDD;
using System;


namespace TutorialLibs.Domain.Entities
{
    public class Tutorial : BaseEntity, IAggregateRoot
    {
        public string Name { get; set; }
        public string BasePath { get; set; }
        public Episode[] Episodes { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime ModifiedTime { get; set; }
    }
}
