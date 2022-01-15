using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// The response to a rectangle collision.
    /// </summary>
    public struct RectangleCollisionResponse : ICollisionResponse<Rectangle>
    {
        public bool Colliding { get; set; }

        public Rectangle Other { get; set; }

        /// <summary>
        /// The distance that a shape is colliding in.
        /// </summary>
        public float IntercectDistance { get; set; }

        /// <summary>
        /// The position of the collision.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The normal of the collision.
        /// </summary>
        public Vector2 Normal { get; set; }

        /// <summary>
        /// Represents no collision detected.
        /// </summary>
        public static readonly RectangleCollisionResponse NoCollision = new RectangleCollisionResponse(
            false, 0, Vector2.Zero, Vector2.Zero, Rectangle.Empty);

        public RectangleCollisionResponse(
            bool colliding, float intercectDistance, Vector2 position, Vector2 normal, Rectangle other)
        {
            Colliding = colliding;
            IntercectDistance = intercectDistance;
            Position = position;
            Normal = normal;
            Other = other;
        }

        public static bool operator ==(RectangleCollisionResponse a, RectangleCollisionResponse b)
        {
            return a.Colliding == b.Colliding
                && a.IntercectDistance == b.IntercectDistance
                && a.Position == b.Position
                && a.Normal == b.Normal
                && a.Other == b.Other;
        }

        public static bool operator !=(RectangleCollisionResponse a, RectangleCollisionResponse b)
        {
            return !(a == b);
        }
    }
}
