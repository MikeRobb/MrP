using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MrP.Core
{
    public static class ArrayExtensions
    {
        public static T[] Range<T>(this T[] arr, int startIndex, int? count = null)
        {
            if (!count.HasValue)
                count = arr.Length - startIndex;

            return arr.ToList().GetRange(startIndex, count.Value).ToArray();
        }
    }
}
