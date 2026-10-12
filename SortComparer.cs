using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

public class SortComparer : MonoBehaviour
{
    // Name must match the built DLL: NativeSorter.dll (no extension here).
    [DllImport("NativeSorter")]
    private static extern void TestSort(int[] a, int length);

    [Tooltip("Text file with one integer per line (generated at Random.org).")]
    public TextAsset numbersFile;

    private int[] original;

    void Start()
    {
        original = LoadNumbers();

        // Each method gets a fresh copy so none of them sorts already-sorted data.
        RunNative(Copy());
        RunManaged(Copy());
        RunMonoBehaviour(Copy());
    }

    int[] Copy() => (int[])original.Clone();

    int[] LoadNumbers()
    {
        if (numbersFile != null)
        {
            string[] parts = numbersFile.text.Split(
                new[] { '\n', '\r', ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            int[] nums = new int[parts.Length];
            for (int i = 0; i < parts.Length; i++) nums[i] = int.Parse(parts[i]);
            return nums;
        }

        // Fallback only: use the Random.org file for your real submission.
        UnityEngine.Debug.LogWarning("No Random.org file assigned - using generated numbers.");
        var rng = new System.Random();
        var set = new System.Collections.Generic.HashSet<int>();
        while (set.Count < 10000) set.Add(rng.Next(1000000, 1000000001));
        int[] arr = new int[set.Count];
        set.CopyTo(arr);
        return arr;
    }

    void RunNative(int[] a)
    {
        Stopwatch sw = Stopwatch.StartNew();
        TestSort(a, a.Length);
        sw.Stop();
        UnityEngine.Debug.Log($"Native plugin took {sw.Elapsed.TotalMilliseconds:F3} ms to execute.");
    }

    void RunManaged(int[] a)
    {
        Stopwatch sw = Stopwatch.StartNew();
        ManagedSorter.IntegerSorter.TestSort(a, a.Length);
        sw.Stop();
        UnityEngine.Debug.Log($"Managed plugin took {sw.Elapsed.TotalMilliseconds:F3} ms to execute.");
    }

    void RunMonoBehaviour(int[] a)
    {
        Stopwatch sw = Stopwatch.StartNew();
        Array.Sort(a, 0, a.Length);
        sw.Stop();
        UnityEngine.Debug.Log($"MonoBehaviour class took {sw.Elapsed.TotalMilliseconds:F3} ms to execute.");
    }
}
