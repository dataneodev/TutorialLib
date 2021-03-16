using System;

namespace TutorialsLib.Core.Entities
{
    public abstract class BaseEntity
    {
        public virtual Guid Id { get; protected set; }
    }
}
