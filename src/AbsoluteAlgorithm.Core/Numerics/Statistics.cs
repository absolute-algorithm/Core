using System.Numerics;

namespace AbsoluteAlgorithm.Core.Numerics;

/// <summary>
/// Provides high-performance statistical analysis tools for data sets.
/// </summary>
public static class Statistics
{
    /// <summary>
    /// Calculates the arithmetic mean (average) of a collection.
    /// </summary>
    public static T Mean<T>(IEnumerable<T> values) where T : INumber<T>
    {
        T sum = T.Zero;
        int count = 0;
        foreach (var val in values)
        {
            sum += val;
            count++;
        }
        return count == 0 ? T.Zero : sum / T.CreateChecked(count);
    }

    /// <summary>
    /// Calculates the Median (middle value) of a collection. 
    /// If the count is even, it returns the average of the two middle elements.
    /// </summary>
    public static T Median<T>(IEnumerable<T> values) where T : INumber<T>
    {
        var sortedList = values.OrderBy(v => v).ToList();
        int count = sortedList.Count;

        if (count == 0) return T.Zero;

        int mid = count / 2;
        if (count % 2 != 0)
        {
            return sortedList[mid];
        }

        // Even count: average the two middle numbers
        return (sortedList[mid - 1] + sortedList[mid]) / T.CreateChecked(2);
    }

    /// <summary>
    /// Calculates the Variance of a data set.
    /// </summary>
    public static T Variance<T>(IEnumerable<T> values) where T : IFloatingPoint<T>
    {
        var list = values.ToList();
        if (list.Count < 2) return T.Zero;

        T avg = Mean(list);
        T sumOfSquares = T.Zero;

        foreach (var val in list)
        {
            T diff = val - avg;
            sumOfSquares += diff * diff;
        }

        return sumOfSquares / T.CreateChecked(list.Count);
    }

    /// <summary>
    /// Calculates the Standard Deviation (Square root of Variance).
    /// </summary>
    public static T StandardDeviation<T>(IEnumerable<T> values) where T : IFloatingPoint<T>, IRootFunctions<T>
    {
        return T.Sqrt(Variance(values));
    }

    /// <summary>
    /// Finds the Minimum and Maximum values in a single pass.
    /// </summary>
    public static (T Min, T Max) Range<T>(IEnumerable<T> values) where T : INumber<T>, IComparisonOperators<T, T, bool>
    {
        using var enumerator = values.GetEnumerator();
        if (!enumerator.MoveNext()) return (T.Zero, T.Zero);

        T min = enumerator.Current;
        T max = enumerator.Current;

        while (enumerator.MoveNext())
        {
            if (enumerator.Current < min) min = enumerator.Current;
            if (enumerator.Current > max) max = enumerator.Current;
        }

        return (min, max);
    }
}