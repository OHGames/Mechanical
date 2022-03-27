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
    /// The respinse to a circle collision.
    /// </summary>
    public struct CircleCollisionResponse : ICollisionResponse<Circle>
    {
        public bool Colliding { get; set; }

        public Circle Other { get; set; }

        /// <summary>
        /// The distance that a shape is colliding in.
        /// </summary>
        public float IntercectDistance { get; set; }

        /// <summary>
        /// The normal of the side of the collision.
        /// </summary>
        public Vector2 Normal { get; set; }

        /// <summary>
        /// Represents no collision detected.
        /// </summary>
        public static readonly CircleCollisionResponse NoCollision = new CircleCollisionResponse(
            false, 0, Vector2.Zero, Circle.Empty);

        public CircleCollisionResponse(
            bool colliding, float intercectDistance, Vector2 normal, Circle other)
        {
            Colliding = colliding;
            IntercectDistance = intercectDistance;
            Normal = normal;
            Other = other;
        }

        public static bool operator ==(CircleCollisionResponse a, CircleCollisionResponse b)
        {
            return a.Colliding == b.Colliding
                && a.IntercectDistance == b.IntercectDistance
                && a.Normal == b.Normal
                && a.Other == b.Other;
        }

        public static bool operator !=(CircleCollisionResponse a, CircleCollisionResponse b)
        {
            return !(a == b);
        }

    }
}
