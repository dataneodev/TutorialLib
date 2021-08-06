using dataneo.TutorialLibs.Domain.DTO;
using System;
using System.Collections.Generic;

namespace dataneo.TutorialsLibs.WPF.Comparers
{
    public sealed class TutorialHeaderDtoByTitleComparer : IComparer<TutorialHeaderDto>
    {
        public int Compare(TutorialHeaderDto x, TutorialHeaderDto y)
            => StringComparer.InvariantCultureIgnoreCase.Compare(x.Name, y.Name);
    }
}
