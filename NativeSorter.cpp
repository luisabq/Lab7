#include "NativeSorter.h"
#include <algorithm>

extern "C" EXPORT_API void TestSort(int a[], int length)
{
    std::sort(a, a + length);
}
