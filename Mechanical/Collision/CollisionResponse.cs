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
    /// Represents a data that is collected from 2 overlaping shapes.
    /// </summary>
    public struct CollisionResponse
    {

        /// <summary>
        /// If there is a collision.
        /// </summary>
        public bool Colliding { get; set; }

        /// <summary>
        /// The distance that a shape is colliding in.
        /// </summary>
        public Vector2 IntercectDistance { get; set; }

        /// <summary>
        /// The position of the collision.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The normal of the side of the collision.
        /// </summary>
        public Vector2 Normal { get; set; }

        /// <summary>
        /// A normalized number that represents how far along the <see cref="LineSegment"/> the collision happened.
        /// This is -1 when the type of collision is not a <see cref="LineSegment"/> vs a <see cref="Rectangle"/>.
        /// </summary>
        public float LineIntercectDistance { get; set; }

        /// <summary>
        /// Represents no collision detected.
        /// </summary>
        public static readonly CollisionResponse NoCollision = new CollisionResponse(
            false, Vector2.Zero, Vector2.Zero, Vector2.Zero, -1);

        public CollisionResponse(
            bool colliding, Vector2 intercectDistance, Vector2 position, Vector2 normal, float lineIntercectDistance = -1)
        {
            Colliding = colliding;
            IntercectDistance = intercectDistance;
            Position = position;
            Normal = normal;
            LineIntercectDistance = lineIntercectDistance;
        }

        public static bool operator ==(CollisionResponse a, CollisionResponse b)
        {
            return a.Colliding == b.Colliding 
                && a.IntercectDistance == b.IntercectDistance
                && a.Position == b.Position
                && a.Normal == b.Normal
                && a.LineIntercectDistance == b.LineIntercectDistance;
        }

        public static bool operator !=(CollisionResponse a, CollisionResponse b)
        {
            return !(a == b);
        }
    }
}
