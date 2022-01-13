/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O. H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/USED_CODE_LICENSES.txt for more info.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// Misc math functions.
    /// </summary>
    public static class MechMath
    {

        /// <summary>
        /// Finds the average of a group of numbers.
        /// </summary>
        /// <param name="items">The list of numbers.</param>
        /// <returns>The average.</returns>
        public static float AverageF(params float[] items)
        {
            float add = 0;
            for (int i = 0; i < items.Length; i++)
            {
                add += items[i];
            }

            return add / items.Length;
        }

        /// <summary>
        /// Finds the average of a group of numbers.
        /// </summary>
        /// <param name="items">The list of numbers.</param>
        /// <returns>The average.</returns>
        public static int Average(params int[] items)
        {
            int add = 0;
            for (int i = 0; i < items.Length; i++)
            {
                add += items[i];
            }

            return add / items.Length;
        }

    }
}
