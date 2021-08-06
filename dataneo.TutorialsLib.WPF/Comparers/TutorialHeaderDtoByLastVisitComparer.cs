﻿using dataneo.TutorialLibs.Domain.DTO;
using System.Collections.Generic;

namespace dataneo.TutorialsLibs.WPF.Comparers
{
    public sealed class TutorialHeaderDtoByLastVisitComparer : IComparer<TutorialHeaderDto>
    {
        public int Compare(TutorialHeaderDto x, TutorialHeaderDto y)
        {
            if (x.LastPlayedDate == y.LastPlayedDate)
                return 0;

            if (x.LastPlayedDate > y.LastPlayedDate)
                return 1;

            return -1;
        }
    }
}
