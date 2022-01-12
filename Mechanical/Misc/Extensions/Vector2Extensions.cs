/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O. H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/USED_CODE_LICENSES.txt for more info.
 */

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// Extensions for <see cref="Vector2"/>
    /// </summary>
    public static class Vector2Extensions
    {

        /// <summary>
        /// This sets the x axis of the vector2. This will be used in Getters and Setters, lists, and arrays.
        /// </summary>
        /// <param name="vec">The vector2 to change.</param>
        /// <param name="x">The x axis.</param>
        /// <returns>The vector2 with a new x.</returns>
        public static Vector2 SetX(this Vector2 vec, float x)
        {
            vec.X = x;
            return vec;
        }

        /// <summary>
        /// This sets the x axis of the vector2. This will be used in Getters and Setters, lists, and arrays.
        /// </summary>
        /// <param name="vec">The vector2 to change.</param>
        /// <param name="x">The x axis.</param>
        /// <returns>The vector2 with a new x.</returns>
        public static void SetX(ref Vector2 vec, ref float x)
        {
            vec.X = x;
        }

        /// <summary>
        /// This sets the y axis of the vector2. This will be used in Getters and Setters, lists, and arrays.
        /// </summary>
        /// <param name="vec">The vector2 to change.</param>
        /// <param name="y">The y axis.</param>
        /// <returns>The vector2 with a new y.</returns>
        public static Vector2 SetY(this Vector2 vec, float y)
        {
            vec.Y = y;
            return vec;
        }

        /// <summary>
        /// This sets the y axis of the vector2. This will be used in Getters and Setters, lists, and arrays.
        /// </summary>
        /// <param name="vec">The vector2 to change.</param>
        /// <param name="y">The y axis.</param>
        /// <returns>The vector2 with a new y.</returns>
        public static void SetY(ref Vector2 vec, ref float y)
        {
            vec.Y = y;
        }

    }
}
