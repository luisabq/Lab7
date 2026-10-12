using System;

namespace ManagedSorter
{
    public static class IntegerSorter
    {
        // Same job as the native plugin: sort an int array in place.
        public static void TestSort(int[] a, int length)
        {
            Array.Sort(a, 0, length);
        }
    }
}
