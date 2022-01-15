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
        /// Represents no collision detected.
        /// </summary>
        public static readonly PolygonCollisionResponse NoCollision = new PolygonCollisionResponse(
            false, Polygon.Empty);

        public PolygonCollisionResponse(
            bool colliding, Polygon other)
        {
            Colliding = colliding;
            Other = other;
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
