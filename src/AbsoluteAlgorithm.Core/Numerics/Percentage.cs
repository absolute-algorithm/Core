using System;
using System.Numerics;

namespace AbsoluteAlgorithm.Core.Numerics
{
    /// <summary>
    /// Provides utility methods for percentage calculations.
    /// </summary>
    public static class Percentage
    {
        /// <summary>
        /// Calculates the percentage value of a number.
        /// </summary>
        /// <param name="value">The base value.</param>
        /// <param name="percent">The percentage to calculate (e.g., 15 for 15%).</param>
        /// <returns>The calculated percentage of the value.</returns>
        public static T Of<T>(T value, T percent)
            where T : INumber<T>
            => value * percent / T.CreateChecked(100);

        /// <summary>
        /// Calculates what percent one value is of another.
        /// </summary>
        /// <param name="part">The part value.</param>
        /// <param name="whole">The whole value.</param>
        /// <returns>The percentage that part is of whole.</returns>
        public static T PercentOf<T>(T part, T whole)
            where T : INumber<T>
            => whole == T.Zero ? T.Zero : (part / whole) * T.CreateChecked(100);

        /// <summary>
        /// Increases a value by a given percentage.
        /// </summary>
        /// <param name="value">The original value.</param>
        /// <param name="percent">The percentage to increase by.</param>
        /// <returns>The increased value.</returns>
        public static T IncreaseBy<T>(T value, T percent)
            where T : INumber<T>
            => value + Of(value, percent);

        /// <summary>
        /// Decreases a value by a given percentage.
        /// </summary>
        /// <param name="value">The original value.</param>
        /// <param name="percent">The percentage to decrease by.</param>
        /// <returns>The decreased value.</returns>
        public static T DecreaseBy<T>(T value, T percent)
            where T : INumber<T>
            => value - Of(value, percent);

        /// <summary>
        /// Calculates the percentage difference between two values.
        /// </summary>
        /// <param name="from">The original value.</param>
        /// <param name="to">The new value.</param>
        /// <returns>The percentage increase or decrease from 'from' to 'to'.</returns>
        public static T PercentageDifference<T>(T from, T to)
            where T : IFloatingPoint<T>
            => from == T.Zero ? T.Zero : ((to - from) / T.Abs(from)) * T.CreateChecked(100);
    }
}
