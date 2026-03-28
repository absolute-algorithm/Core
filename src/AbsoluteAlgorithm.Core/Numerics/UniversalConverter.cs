using System;
using System.Numerics;

namespace AbsoluteAlgorithm.Core.Numerics;

/// <summary>
/// A high-performance conversion engine for physical, digital, and temporal dimensions.
/// Uses a base-unit normalization pattern for O(1) scaling.
/// </summary>
public static class UniversalConverter
{
    #region Scale Definitions (Relative to Base)
    /// <summary>
    /// Provides scale factors for length units relative to the meter.
    /// </summary>
    public static readonly Dictionary<string, double> LengthScales = new()
    {
        { "mm", 0.001 }, { "cm", 0.01 }, { "m", 1.0 }, { "km", 1000.0 },
        { "in", 0.0254 }, { "ft", 0.3048 }, { "yd", 0.9144 }, { "mi", 1609.34 }
    };

    /// <summary>
    /// Provides scale factors for liquid volume units relative to the liter.
    /// </summary>
    public static readonly Dictionary<string, double> LiquidScales = new()
    {
        { "ml", 0.001 }, { "l", 1.0 }, { "cl", 0.01 }, { "dl", 0.1 },
        { "gal", 3.78541 }, { "qt", 0.946353 }, { "pt", 0.473176 }, { "cup", 0.236588 }
    };

    /// <summary>
    /// Provides scale factors for mass units relative to the gram.
    /// </summary>
    public static readonly Dictionary<string, double> MassScales = new()
    {
        { "mg", 0.001 }, { "g", 1.0 }, { "kg", 1000.0 }, { "mt", 1000000.0 },
        { "oz", 28.3495 }, { "lb", 453.592 }
    };


    /// <summary>
    /// Provides scale factors for data size units relative to the byte.
    /// </summary>
    public static readonly Dictionary<string, double> DataScales = new()
    {
        { "b", 1.0 }, { "kb", 1024.0 }, { "mb", 1048576.0 }, 
        { "gb", 1073741824.0 }, { "tb", 1099511627776.0 }
    };

    /// <summary>
    /// Provides scale factors for time units relative to the second.
    /// </summary>
    public static readonly Dictionary<string, double> TimeScales = new()
    {
        { "ms", 0.001 }, { "s", 1.0 }, { "min", 60.0 }, 
        { "hr", 3600.0 }, { "day", 86400.0 }, { "week", 604800.0 }
    };

    /// <summary>
    /// Provides scale factors for frequency units relative to the hertz.
    /// </summary>
    public static readonly Dictionary<string, double> FrequencyScales = new()
    {
        { "hz", 1.0 }, { "khz", 1000.0 }, { "mhz", 1000000.0 }, { "ghz", 1000000000.0 }
    };

    #endregion

    /// <summary>
    /// Standard conversion for Dimensions that use a zero-based scale (Length, Mass, Liquid).
    /// </summary>
    public static T Convert<T>(T value, string fromUnit, string toUnit, Dictionary<string, double> scales)
        where T : IFloatingPoint<T>
    {
        if (!scales.ContainsKey(fromUnit) || !scales.ContainsKey(toUnit))
            throw new ArgumentException("Unit key not found in the provided scale dictionary.");

        double baseValue = double.CreateChecked(value) * scales[fromUnit];
        double targetValue = baseValue / scales[toUnit];

        return T.CreateChecked(targetValue);
    }

    #region Specialized Temperature Logic

    /// <summary>
    /// Converts temperatures between Celsius (C), Fahrenheit (F), and Kelvin (K).
    /// </summary>
    /// <param name="value">The temperature value.</param>
    /// <param name="from">"C", "F", or "K"</param>
    /// <param name="to">"C", "F", or "K"</param>
    public static T ConvertTemperature<T>(T value, string from, string to) where T : IFloatingPoint<T>
    {
        double val = double.CreateChecked(value);

        // 1. Normalize input to Celsius
        double celsius = from.ToUpper() switch
        {
            "C" => val,
            "F" => (val - 32.0) / 1.8,
            "K" => val - 273.15,
            _ => throw new ArgumentException("Invalid 'from' temperature unit. Use C, F, or K.")
        };

        // 2. Convert Celsius to target
        double result = to.ToUpper() switch
        {
            "C" => celsius,
            "F" => (celsius * 1.8) + 32.0,
            "K" => celsius + 273.15,
            _ => throw new ArgumentException("Invalid 'to' temperature unit. Use C, F, or K.")
        };

        return T.CreateChecked(result);
    }

    #endregion
}