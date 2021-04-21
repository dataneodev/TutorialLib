﻿using System;
using TutorialLibs.SharedKernel.Common;
using TutorialLibs.SharedKernel.Interfaces;

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
