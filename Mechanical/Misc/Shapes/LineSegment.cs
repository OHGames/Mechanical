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
    /// Represents a line segment.
    /// </summary>
    public struct LineSegment
    {

        /// <summary>
        /// The left point of the segment.
        /// </summary>
        public Vector2 A { get; set; }

        /// <summary>
        /// The right point of the segment.
        /// </summary>
        public Vector2 B { get; set; }

        /// <summary>
        /// The distance between the <see cref="A"/> and <see cref="B"/> locations.
        /// </summary>
        public float Distance
        {
            get
            {
                return Vector2.Distance(A, B);
            }
        }

        /// <summary>
        /// The squared distance between the <see cref="A"/> and <see cref="B"/> locations.
        /// </summary>
        public float DistanceSquared
        {
            get
            {
                return Vector2.DistanceSquared(A, B);
            }
        }

        /// <summary>
        /// The center point between <see cref="A"/> and <see cref="B"/>.
        /// </summary>
        public Vector2 MidPoint
        {
            get
            {
                // https://www.calculatorsoup.com/calculators/geometry-plane/midpoint-calculator.php
                // evaluates (xm, ym) = ( (x1 + x2) / 2 , (y1 + y2) / 2 )
                return new Vector2(MechMath.AverageF(A.X, A.X), MechMath.AverageF(A.Y, B.Y));
            }
        }

        public LineSegment(Vector2 a, Vector2 b)
        {
            A = a;
            B = b;
        }

        public static bool operator ==(LineSegment a, LineSegment b)
        {
            return a.A == b.A
                && a.B == b.B;
        }

        public static bool operator !=(LineSegment a, LineSegment b)
        {
            return !(a == b);
        }

    }
}
