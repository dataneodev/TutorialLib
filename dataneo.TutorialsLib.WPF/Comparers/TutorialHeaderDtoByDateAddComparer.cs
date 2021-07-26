using dataneo.TutorialLibs.Domain.DTO;
using System.Collections.Generic;

namespace dataneo.TutorialsLib.WPF.Comparers
{
    public sealed class TutorialHeaderDtoByDateAddComparer : IComparer<TutorialHeaderDto>
    {
        public int Compare(TutorialHeaderDto x, TutorialHeaderDto y)
        {
            if (x.DateAdd == y.DateAdd)
                return 0;

            if (x.DateAdd > y.DateAdd)
                return -1;

            return 1;
        }
    }
}
