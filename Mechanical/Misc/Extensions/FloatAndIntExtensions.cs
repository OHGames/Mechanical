﻿using System;
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

        /// <summary>
        /// This converts degrees to radians.
        /// </summary>
        /// <param name="degrees">The degrees to convert.</param>
        /// <returns>The converted degrees.</returns>
        public static int ToRadians(this int degrees)
        {
            return (int)(degrees * Math.PI / 180);
        }

        /// <summary>
        /// This converts radians to degrees.
        /// </summary>
        /// <param name="radians">The radians to convert.</param>
        /// <returns>The converted radians.</returns>
        public static int ToDegrees(this int radians)
        {
            return (int)(radians * 180 / Math.PI);
        }

        /// <summary>
        /// This converts degrees to radians.
        /// </summary>
        /// <param name="degrees">The degrees to convert.</param>
        /// <returns>The converted degrees.</returns>
        public static float ToRadians(this float degrees)
        {
            return (float)(degrees * Math.PI / 180);
        }

        /// <summary>
        /// This converts radians to degrees.
        /// </summary>
        /// <param name="radians">The radians to convert.</param>
        /// <returns>The converted radians.</returns>
        // thank you Math Is Fun
        // https://www.mathsisfun.com/geometry/radians.html
        public static float ToDegrees(this float radians)
        {
            return (float)(radians * 180 / Math.PI);
        }

    }
}
