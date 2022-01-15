using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// Represents a convex polygon.
    /// </summary>
    public struct Polygon
    {
        /// <summary>
        /// An empty polygon.
        /// </summary>
        public static readonly Polygon Empty = new Polygon(new Vector2[] {});

        /// <summary>
        /// All of the verticies in this polygon.
        /// </summary>
        public Vector2[] Verticies { get; set; }

        /// <summary>
        /// How many verticies are in the polygon.
        /// </summary>
        public int VertexCount => Verticies.Length;

        /// <summary>
        /// If the shape is a convex shape.
        /// </summary>
        public bool IsConvex { get; private set; }

        public Polygon(params Vector2[] verticies) : this()
        {
            Verticies = verticies;
            IsConvex = IsShapeConvex(verticies);
        }

        // dirived from https://stackoverflow.com/a/1881201
        /// <summary>
        /// If the shape is convex or concave. This only works with shapes that are not self-intercecting.
        /// </summary>
        /// <param name="verticies">The verticies to check.</param>
        /// <returns>True if shape is convex.</returns>
        public static bool IsShapeConvex(Vector2[] verticies)
        {
            float sign = 0;

            // loop through all plates.
            for (int i = 0; i < verticies.Length; i++)
            {
                Vector2 a = verticies[i];
                Vector2 b = verticies.WrapIndex(i + 1);
                Vector2 c = verticies.WrapIndex(i + 2);

                float dx1 = b.X - a.X;
                float dy1 = b.Y - a.Y;
                float dx2 = c.X - a.X;
                float dy2 = c.Y - a.Y;

                float crossProduct = (dx1 * dy2) - (dy1 * dx2);

                float newSign = Math.Sign(crossProduct);

                // if the number is not all the same sign, it is concave.
                if (newSign != sign)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool operator ==(Polygon a, Polygon b)
        {
            return a.Verticies == b.Verticies;
        }

        public static bool operator !=(Polygon a, Polygon b)
        {
            return !(a == b);
        }

    }
}
