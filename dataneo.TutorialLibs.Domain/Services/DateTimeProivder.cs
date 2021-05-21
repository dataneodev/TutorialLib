using dataneo.TutorialLibs.Domain.Interfaces;
using System;

namespace dataneo.TutorialLibs.Domain.Services
{
    public sealed class DateTimeProivder : IDateTimeProivder
    {
        public DateTime Now => DateTime.Now;
    }
}
