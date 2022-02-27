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
        /// <returns><see cref="PolygonCollisionResponse.NoCollision"/> if there is no collision 
        /// and a <see cref="PolygonCollisionResponse"/> when there is a collision.</returns>
        public static PolygonCollisionResponse IsColliding(Polygon a, Polygon b)
        {
            float intercect = float.MaxValue;
            Vector2 intercectNormal = Vector2.Zero;

            // loop through A.
            for (int i = 0; i < a.VertexCount; i++)
            {
                Vector2 va = a.Verticies[i];
                // get next vertex.
                Vector2 vb = a.Verticies.WrapIndex(i + 1);

                Vector2 edge = vb - va;
                // get the normal (axis).
                Vector2 normal = new Vector2(-edge.Y, edge.X);

                ProjectVerticies(a.Verticies, normal, out float minA, out float maxA);
                ProjectVerticies(b.Verticies, normal, out float minB, out float maxB);

                if (minA >= maxB || minB >= maxA)
                {
                    // no collision.
                    return PolygonCollisionResponse.NoCollision;
                }

                float axisDepth = Math.Min(minA - maxB, minB - maxA);
                if (axisDepth < intercect)
                {
                    intercect = axisDepth;
                    intercectNormal = normal;
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

                if (minA >= maxB || minB >= maxA)
                {
                    // no collision.
                    return PolygonCollisionResponse.NoCollision;
                }

                float axisDepth = Math.Min(minA - maxB, minB - maxA);
                if (axisDepth < intercect)
                {
                    intercect = axisDepth;
                    intercectNormal = normal;
                }

            }

            intercect /= intercectNormal.Length();
            intercectNormal.Normalize();

            Vector2 centerA = FindArtithmaticMean(a.Verticies);
            Vector2 centerB = FindArtithmaticMean(b.Verticies);

            Vector2 direction = centerB - centerA;

            if (Vector2.Dot(direction, intercectNormal) < 0)
            {
                intercectNormal = -intercectNormal;
            }

            // if we are here there are no gaps so there is a collision.
            return new PolygonCollisionResponse(true, b, intercect, intercectNormal);
        }

        //https://github.com/twobitcoder101/FlatPhysics this function here. Licensed under the MIT license.
        /// <summary>
        /// Finds the "center" of the verticies.
        /// </summary>
        /// <param name="verticies">The verticies to use.</param>
        /// <returns>The "center" of the verticies.</returns>
        private static Vector2 FindArtithmaticMean(Vector2[] verticies)
        {
            float sumX = 0;
            float sumY = 0;

            for (int i = 0; i < verticies.Length; i++)
            {
                Vector2 v = verticies[i];
                sumX += v.X;
                sumY += v.Y;
            }

            return new Vector2(sumX / verticies.Length, sumY / verticies.Length);
        }

        //https://github.com/twobitcoder101/FlatPhysics this function here. Licensed under the MIT license.
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

        #region Circle vs Polygon
        //https://github.com/twobitcoder101/FlatPhysics this function here. Licensed under the MIT license.
        /// <summary>
        /// Checks if a <see cref="Circle"/> and a <see cref="Polygon"/> are colliding.
        /// </summary>
        /// <param name="circle">The circle to check.</param>
        /// <param name="polygon">The polygon to check.</param>
        /// <returns><see cref="PolygonCollisionResponse.NoCollision"/> if there is no collision
        /// and a <see cref="PolygonCollisionResponse"/> when there is a collision.</returns>
        public static PolygonCollisionResponse IsColliding(Polygon polygon, Circle circle) => IsColliding(circle, polygon);

        //https://github.com/twobitcoder101/FlatPhysics this function here. Licensed under the MIT license.
        /// <summary>
        /// Checks if a <see cref="Circle"/> and a <see cref="Polygon"/> are colliding.
        /// </summary>
        /// <param name="circle">The circle to check.</param>
        /// <param name="polygon">The polygon to check.</param>
        /// <returns><see cref="PolygonCollisionResponse.NoCollision"/> if there is no collision
        /// and a <see cref="PolygonCollisionResponse"/> when there is a collision.</returns>
        public static PolygonCollisionResponse IsColliding(Circle circle, Polygon polygon)
        {
            float intercect = float.MaxValue;
            Vector2 intercectNormal = Vector2.Zero;

            // loop through polygon.
            for (int i = 0; i < polygon.VertexCount; i++)
            {
                Vector2 va = polygon.Verticies[i];
                // get next vertex.
                Vector2 vb = polygon.Verticies.WrapIndex(i + 1);

                Vector2 edge = vb - va;
                // get the normal.
                Vector2 normal = new Vector2(-edge.Y, edge.X);
                normal.Normalize();

                ProjectVerticies(polygon.Verticies, normal, out float minA, out float maxA);
                ProjectCircle(circle, normal, out float minB, out float maxB);

                if (minA >= maxB || minB >= maxA)
                {
                    // no collision.
                    return PolygonCollisionResponse.NoCollision;
                }

                float axisDepthPoly = Math.Min(maxB - minA, maxA - minB);
                if (axisDepthPoly < intercect)
                {
                    intercect = axisDepthPoly;
                    intercectNormal = normal;
                }

            }
            // project circle.

            int closestVertexIndex = FindClosestPointOnPolygon(circle, polygon);
            Vector2 closest = polygon.Verticies[closestVertexIndex];

            Vector2 axis = closest - circle.Center;
            axis.Normalize();

            ProjectVerticies(polygon.Verticies, axis, out float minAC, out float maxAC);
            ProjectCircle(circle, axis, out float minBC, out float maxBC);

            if (minAC >= maxBC || minBC >= maxAC)
            {
                // no collision.
                return PolygonCollisionResponse.NoCollision;
            }


            float axisDepth = Math.Min(maxBC - minAC, maxAC - minBC);

            if (axisDepth < intercect)
            {
                intercect = axisDepth;
                intercectNormal = axis;
            }


            Vector2 centerB = FindArtithmaticMean(polygon.Verticies);

            Vector2 direction = centerB - circle.Center;

            if (Vector2.Dot(direction, intercectNormal) < 0)
            {
                intercectNormal = -intercectNormal;
            }

            return new PolygonCollisionResponse(true, polygon, intercect, intercectNormal);
        }

        //https://github.com/twobitcoder101/FlatPhysics this function here. Licensed under the MIT license.
        /// <summary>
        /// This finds the closest point on a polygon to a circle.
        /// </summary>
        /// <param name="circle">The circle.</param>
        /// <param name="polygon">The polygon.</param>
        /// <returns>The index of the closest point to the circle.</returns>
        private static int FindClosestPointOnPolygon(Circle circle, Polygon polygon)
        {
            int result = -1;
            float min = float.MaxValue;

            for (int i = 0; i < polygon.VertexCount; i++)
            {
                Vector2 vertex = polygon.Verticies[i];
                float distance = Vector2.Distance(vertex, circle.Center);

                if (distance < min)
                {
                    min = distance;
                    result = i;
                }
            }

            return result;
        }

        //https://github.com/twobitcoder101/FlatPhysics this function here. Licensed under the MIT license.
        /// <summary>
        /// Projects a circle onto an axis.
        /// </summary>
        /// <param name="circle">The circle.</param>
        /// <param name="axis">The axis.</param>
        /// <param name="min">The min of the projection.</param>
        /// <param name="max">The max of the projection.</param>
        private static void ProjectCircle(Circle circle, Vector2 axis, out float min, out float max)
        {
            Vector2 direction = Vector2.Normalize(axis);
            Vector2 directionAndRaduis = direction * circle.Radius;

            Vector2 a = circle.Center + directionAndRaduis;
            Vector2 b = circle.Center - directionAndRaduis;

            min = Vector2.Dot(a, axis);
            max = Vector2.Dot(b, axis);

            if (min > max)
            {
                // swap.
                float temp = min;
                min = max;
                max = temp;
            }
        }


        #endregion

        #region Rectangle vs Circle

        /// <summary>
        /// Checks if a <see cref="Rectangle"/> and a <see cref="Circle"/> are colliding.
        /// </summary>
        /// <param name="circle">The circle to check.</param>
        /// <param name="rectangle">The rectangle to check.</param>
        /// <returns><see cref="PolygonCollisionResponse.NoCollision"/> if there is no collision 
        /// and a <see cref="PolygonCollisionResponse"/> when there is a collision.</returns>
        public static PolygonCollisionResponse IsColliding(Circle circle, Rectangle rectangle) =>
            IsColliding(circle, rectangle.ToPolygon());

        /// <summary>
        /// Checks if a <see cref="Rectangle"/> and a <see cref="Circle"/> are colliding.
        /// </summary>
        /// <param name="circle">The circle to check.</param>
        /// <param name="rectangle">The rectangle to check.</param>
        /// <returns><see cref="PolygonCollisionResponse.NoCollision"/> if there is no collision 
        /// and a <see cref="PolygonCollisionResponse"/> when there is a collision.</returns>
        public static PolygonCollisionResponse IsColliding(Rectangle rectangle, Circle circle) => IsColliding(circle, rectangle);

        #endregion

        /// <summary>
        /// Gets half of the rectangle's dimenstions.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <returns>Half of the width.</returns>
        public static Vector2 Half(this Rectangle rect)
        {
            return new Vector2(rect.Width / 2f, rect.Height / 2f);
        }

    }
}
