using System;

namespace dataneo.TutorialLibs.Domain.Tutorials
{
    public sealed class DateTimeProivder : IDateTimeProivder
    {
        public DateTime Now => DateTime.Now;
    }
}
