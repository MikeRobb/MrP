using System.Linq;

namespace MrP.Core
{
    public static class ArrayExtensions
    {
        public static T[] Range<T>(this T[] arr, int startIndex, int? count = null)
        {
            if (!count.HasValue)
                count = arr.Length - startIndex;

            if (startIndex > arr.Length)
                return new T[0];

            if (startIndex + count > arr.Length)
                count = arr.Length - startIndex;

            return arr.ToList().GetRange(startIndex, count.Value).ToArray();
        }

        public static bool Equals<T1, T2>(this T1[] arr1, T2[] arr2)
        {
            if (typeof(T1) != typeof(T2))
                return false;

            if (arr1.Length != arr2.Length)
                return false;

            for (var i = 0; i < arr1.Length; i++)
            {
                var a1 = arr1[i];
                var a2 = arr2[i];

                if (!a1.Equals(a2))
                    return false;
            }

            return true;
        }
    }
}
