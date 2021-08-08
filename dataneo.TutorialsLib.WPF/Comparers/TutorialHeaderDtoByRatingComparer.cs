using dataneo.TutorialLibs.Domain.DTO;
using System.Collections.Generic;

namespace dataneo.TutorialLibs.WPF.Comparers
{
    public sealed class TutorialHeaderDtoByRatingComparer : IComparer<TutorialHeaderDto>
    {
        public int Compare(TutorialHeaderDto x, TutorialHeaderDto y)
        {
            if (x.Rating == y.Rating)
                return 0;

            if (x.Rating > y.Rating)
                return 1;

            return -1;
        }
    }
}
