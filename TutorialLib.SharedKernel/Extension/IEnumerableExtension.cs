using System;
using System.Collections.Generic;

namespace TutorialLibs.SharedKernel.Extension
{
    public static class IEnumerableExtension
    {
        public static List<T> ToList<T>(this IEnumerable<T> enumerable, int predictedCapacity, bool trimExcess = false)
        {
            if (predictedCapacity < 0)
                throw new ArgumentException("predictedCapacity < 0");

            var returnList = new List<T>(predictedCapacity);
            returnList.AddRange(enumerable);

            if (trimExcess && returnList.Count != returnList.Capacity)
            {
                returnList.TrimExcess();
            }

            return returnList;
        }
    }
}
