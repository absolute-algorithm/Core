using System;
using System.Diagnostics;

namespace AbsoluteAlgorithm.Core.Diagnostics;

/// <summary>
/// Tracks the performance of code blocks and logs the elapsed time using the Logger.
/// Usage: wrap in a using statement to automatically log duration on dispose.
/// </summary>
public sealed class PerformanceTracker : IDisposable
{
    private readonly string _label;
    private readonly Stopwatch _sw;

    /// <summary>
    /// Initializes a new instance of the <see cref="PerformanceTracker"/> class and starts timing.
    /// </summary>
    /// <param name="label">A label describing the code block being tracked.</param>
    public PerformanceTracker(string label)
    {
        _label = label;
        _sw = Stopwatch.StartNew();
    }

    /// <summary>
    /// Stops timing and logs the elapsed time using the Logger.
    /// </summary>
    public void Dispose()
    {
        _sw.Stop();
        Logger.Info($"{_label} finished in {_sw.Elapsed.TotalMilliseconds:F2}ms");
    }
}