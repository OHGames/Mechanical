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
    /// The collision class contains lots of functions to detect collisions between shapes. This only detects intercections.
    /// </summary>
    public static class Collision
    {

        #region AABB vs AABB
        // https://github.com/noonat/intersect This function here. This function is licensed under the Zlib-Libpng License (Zlib).
        /// <summary>
        /// Checks if 2 <see cref="Rectangle"/>s are colliding.
        /// </summary>
        /// <param name="a">The first rectangle.</param>
        /// <param name="b">The second rectangle</param>
        /// <returns>A <see cref="RectangleCollisionResponse"/> for <paramref name="a"/>. If there is no collision, it returns <see cref="RectangleCollisionResponse.NoCollision"/></returns>
        public static RectangleCollisionResponse IsColliding(Rectangle a, Rectangle b)
        {
            float dx = b.Center.X - a.Center.X;
            float px = ((b.Width / 2) + (a.Width / 2)) - (float)Math.Abs(dx);

            // there is no overlap.
            if (px <= 0)
            {
                return RectangleCollisionResponse.NoCollision;
            }

            float dy = b.Center.Y - a.Center.Y;
            float py = ((b.Height / 2) + (a.Height / 2)) - Math.Abs(dy);

            // there is no overlap.
            if (py <= 0)
            {
                return RectangleCollisionResponse.NoCollision;
            }

            if (px < py)
            {
                float sx = Math.Sign(dx);

                Vector2 pos = new Vector2(a.X + (a.Width / 2) * sx, b.Y);

                return new RectangleCollisionResponse(true, px * sx, pos, new Vector2(sx, 0), b);
            }
            else
            {
                float sy = Math.Sign(dy);

                Vector2 pos = new Vector2(b.X, a.Y + (a.Width / 2) * sy);

                return new RectangleCollisionResponse(true, py * sy, pos, new Vector2(0, sy), b);
            }
        }

        #endregion

        #region Circle vs Circle
        // https://github.com/twobitcoder101/FlatPhysics this function here. Licensed under the MIT license.
        /// <summary>
        /// Checks if 2 <see cref="Circle"/>s are colliding.
        /// </summary>
        /// <param name="a">The circle to check.</param>
        /// <param name="b">The other circle to check.</param>
        /// <returns><see cref="CircleCollisionResponse.NoCollision"/> when there is no collision, and
        /// <see cref="CircleCollisionResponse"/> when there is a collision.</returns>
        public static CircleCollisionResponse IsColliding(Circle a, Circle b)
        {
            // get the distance of the 2 centers
            float distance = Vector2.Distance(a.Center, b.Center);

            // add the radii
            float radii = a.Radius + b.Radius;

            // no collision, they are far apart.
            if (distance >= radii)
            {
                return CircleCollisionResponse.NoCollision;
            }

            Vector2 normal = Vector2.Normalize(b.Center - a.Center);
            float intercect = radii - distance;

            // TODO: change normal to a normalized vector and change intercectDistance to a float
            // to apply the resolution multiply the distance and the normal and add onto location.

            return new CircleCollisionResponse(true, intercect, normal, b);

        }

        #endregion

        #region SAT (Polygon vs Polygon)
        //https://github.com/twobitcoder101/FlatPhysics this function here. Licensed under the MIT license.
        /// <summary>
        /// Checks if 2 <see cref="Polygon"/>s are colliding.
        /// </summary>
        /// <param name="a">The polygon to check.</param>
        /// <param name="b">The other polygon.</param>
        /// <returns><see cref="PolygonCollisionResponse.NoCollision"/> if there is no collision, 
        /// and a <see cref="PolygonCollisionResponse"/> when there is a collision.</returns>
        public static PolygonCollisionResponse IsColliding(Polygon a, Polygon b)
        {
            // loop through A.
            for (int i = 0; i < a.VertexCount; i++)
            {
                Vector2 va = a.Verticies[i];
                // get next vertex.
                Vector2 vb = a.Verticies.WrapIndex(i + 1);

                Vector2 edge = vb - va;
                // get the normal.
                Vector2 normal = new Vector2(-edge.Y, edge.X);

                ProjectVerticies(a.Verticies, normal, out float minA, out float maxA);
                ProjectVerticies(b.Verticies, normal, out float minB, out float maxB);

                if (minA >= maxB || minB >= maxA)
                {
                    // no collision.
                    return PolygonCollisionResponse.NoCollision;
                }

            }

            // loop through B.
            for (int i = 0; i < b.VertexCount; i++)
            {
                Vector2 va = b.Verticies[i];
                // get next vertex.
                Vector2 vb = b.Verticies.WrapIndex(i + 1);

                Vector2 edge = vb - va;
                // get the normal.
                Vector2 normal = new Vector2(-edge.Y, edge.X);

                ProjectVerticies(a.Verticies, normal, out float minA, out float maxA);
                ProjectVerticies(b.Verticies, normal, out float minB, out float maxB);

                if (minA >= maxB || minB >= maxA)
                {
                    // no collision.
                    return PolygonCollisionResponse.NoCollision;
                }

            }

            // if we are here there are no gaps so there is a collision.
            return new PolygonCollisionResponse(true, b);
        }

        /// <summary>
        /// Projects a list of verticies to an axis for SAT collision detection. 
        /// This is used in <see cref="IsColliding(Polygon, Polygon)"/>.
        /// </summary>
        /// <param name="verticies">The verticies to project.</param>
        /// <param name="axis">The axis to project to.</param>
        /// <param name="min">The min of the projection.</param>
        /// <param name="max">The max of the projection.</param>
        private static void ProjectVerticies(Vector2[] verticies, Vector2 axis, out float min, out float max)
        {
            min = float.MaxValue;
            max = float.MinValue;

            for (int i = 0; i < verticies.Length; i++)
            {
                Vector2 v = verticies[i];
                float proj = Vector2.Dot(v, axis);

                if (proj < min)
                {
                    min = proj;
                }

                if (proj > max)
                {
                    max = proj;
                }
            }
        }
        #endregion

        /// <summary>
        /// Gets half of the rectangle's dimenstions.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <returns>Half of the width.</returns>
        public static Vector2 Half(this Rectangle rect)
        {
            return new Vector2(rect.Width / 2, rect.Height / 2);
        }

    }
}
