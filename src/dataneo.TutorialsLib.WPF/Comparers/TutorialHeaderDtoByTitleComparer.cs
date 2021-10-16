using dataneo.TutorialLibs.Domain.Tutorials;
using System;
using System.Collections.Generic;

namespace dataneo.TutorialLibs.WPF.Comparers
{
    public sealed class TutorialHeaderDtoByTitleComparer : IComparer<TutorialHeaderDto>
    {
        public int Compare(TutorialHeaderDto x, TutorialHeaderDto y)
            => StringComparer.InvariantCultureIgnoreCase.Compare(x.Name, y.Name);
    }
}
