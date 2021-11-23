using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// Misc extension methods for the <see cref="float"/> and <see cref="int"/> types.
    /// </summary>
    public static class FloatAndIntExtensions
    {

        /// <summary>
        /// Clamp the value between a number.
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped value.</returns>
        /// <exception cref="ArgumentException">When the minimum is greater than the maximum.</exception>
        public static int Clamp(this int value, int min, int max)
        {
            if (min > max) throw new ArgumentException("the minimum is more than the maximum.");

            if (value > max) return max;
            else if (value < min) return min;
            else return value;
        }

        /// <summary>
        /// Clamp the value between a number.
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped value.</returns>
        /// <exception cref="ArgumentException">When the minimum is greater than the maximum.</exception>
        public static float Clamp(this float value, float min, float max)
        {
            if (min > max) throw new ArgumentException("the minimum is more than the maximum.");

            if (value > max) return max;
            else if (value < min) return min;
            else return value;
        }

    }
}
