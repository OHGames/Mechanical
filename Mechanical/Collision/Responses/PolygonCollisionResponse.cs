/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O. H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/Use%20Code%20Licenses.md for more info.
 */

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// A response to a collision with a polygon.
    /// </summary>
    public struct PolygonCollisionResponse : ICollisionResponse<Polygon>
    {
        public bool Colliding { get; set; }
        public Polygon Other { get; set; }

        /// <summary>
        /// How much the 2 polygons intercect.
        /// </summary>
        public float IntercectDistance { get; set; }

        /// <summary>
        /// The normal to move in to get out of the shape.
        /// </summary>
        public Vector2 Normal { get; set; }

        /// <summary>
        /// Represents no collision detected.
        /// </summary>
        public static readonly PolygonCollisionResponse NoCollision = new PolygonCollisionResponse(
            false, Polygon.Empty, 0, Vector2.Zero);

        public PolygonCollisionResponse(
            bool colliding, Polygon other, float intercectDistance, Vector2 normal)
        {
            Colliding = colliding;
            Other = other;
            IntercectDistance = intercectDistance;
            Normal = normal;
        }

        public static bool operator ==(PolygonCollisionResponse a, PolygonCollisionResponse b)
        {
            return a.Colliding == b.Colliding
                && a.Other == b.Other;
        }

        public static bool operator !=(PolygonCollisionResponse a, PolygonCollisionResponse b)
        {
            return !(a == b);
        }
    }
}
