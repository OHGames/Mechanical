using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// A bunch of extension methods for shapes.
    /// </summary>
    public static class ShapeExtensions
    {
        /// <summary>
        /// Turns a <see cref="Circle"/> into a <see cref="Polygon"/>.
        /// </summary>
        /// <param name="circle">The circle to turn into a polygon.</param>
        /// <param name="segments">How many segments there should be.</param>
        /// <returns>A new polygon.</returns>
        public static Polygon ToPolygon(this Circle circle, int segments = 16)
        {
            Vector2[] verticies = new Vector2[segments];

            float inc = MathHelper.PiOver2 / segments;
            float currentInc = 0;

            for (int i = 0; i < segments; i++)
            {
                float x = (float)Math.Cos(currentInc) * circle.Radius;
                float y = (float)Math.Sin(currentInc) * circle.Radius;

                verticies[i] = new Vector2(x, y) + circle.Center;

                currentInc += inc;
            }

            return new Polygon(verticies);

        }

        /// <summary>
        /// Turns a <see cref="Rectangle"/> into a <see cref="Polygon"/>.
        /// </summary>
        /// <param name="rectangle">The rectangle to turn into a polygon.</param>
        /// <returns>A new polygon.</returns>
        public static Polygon ToPolygon(this Rectangle rectangle)
        {
            Vector2[] verticies = new Vector2[4];
            // top left
            verticies[0] = new Vector2(rectangle.X, rectangle.Y);
            // top right
            verticies[1] = new Vector2(rectangle.X + rectangle.Width, rectangle.Y);
            // bottom right
            verticies[2] = new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height);
            // bottom left
            verticies[3] = new Vector2(rectangle.X, rectangle.Y + rectangle.Height);

            return new Polygon(verticies);
        }
    }
}
