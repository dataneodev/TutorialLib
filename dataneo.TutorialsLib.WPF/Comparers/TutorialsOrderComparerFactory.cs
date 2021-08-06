using dataneo.TutorialLibs.Domain.DTO;
using System;
using System.Collections.Generic;

namespace dataneo.TutorialsLibs.WPF.Comparers
{
    internal static class TutorialsOrderComparerFactory
    {
        private static readonly Dictionary<TutorialsOrderType, IComparer<TutorialHeaderDto>> _cache = new();

        public static IComparer<TutorialHeaderDto> GetComparer(TutorialsOrderType tutorialsOrderType)
        {
            if (_cache.TryGetValue(tutorialsOrderType, out IComparer<TutorialHeaderDto> comparer))
            {
                return comparer;
            }

            var newComparer = CreateComparer(tutorialsOrderType);
            AddToCache(tutorialsOrderType, newComparer);
            return newComparer;
        }

        private static void AddToCache(TutorialsOrderType tutorialsOrderType, IComparer<TutorialHeaderDto> comparer)
        {
            lock (_cache)
            {
                if (_cache.ContainsKey(tutorialsOrderType))
                    return;
                _cache.Add(tutorialsOrderType, comparer);
            }
        }

        private static IComparer<TutorialHeaderDto> CreateComparer(TutorialsOrderType tutorialsOrderType)
            => tutorialsOrderType switch
            {
                TutorialsOrderType.ByDateAdd => new TutorialHeaderDtoByDateAddComparer(),
                TutorialsOrderType.ByLastVisit => new TutorialHeaderDtoByLastVisitComparer(),
                TutorialsOrderType.ByRating => new TutorialHeaderDtoByRatingComparer(),
                TutorialsOrderType.ByTitle => new TutorialHeaderDtoByTitleComparer(),
                _ => throw new InvalidOperationException()
            };
    }
}
