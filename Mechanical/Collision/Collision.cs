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
    /// The collision class contains lots of functions to detect collisions between shapes.
    /// </summary>
    public static class Collision
    {

        #region AABB vs AABB
        // http://web.archive.org/web/20201128223427/https://github.com/noonat/intersect This function here. This function is licensed under the Zlib-Libpng License (Zlib).
        /// <summary>
        /// Checks if 2 <see cref="Rectangle"/>s are colliding.
        /// </summary>
        /// <param name="a">The first rectangle.</param>
        /// <param name="b">The second rectangle</param>
        /// <returns>A <see cref="CollisionResponse"/> for <paramref name="a"/>. If there is no collision, it returns <see cref="CollisionResponse.NoCollision"/></returns>
        public static CollisionResponse IsColliding(Rectangle a, Rectangle b)
        {
            float dx = b.Center.X - a.Center.X;
            float px = ((b.Width / 2) + (a.Width / 2)) - (float)Math.Abs(dx);

            // there is no overlap.
            if (px <= 0)
            {
                return CollisionResponse.NoCollision;
            }

            float dy = b.Center.Y - a.Center.Y;
            float py = ((b.Height / 2) + (a.Height / 2)) - Math.Abs(dy);

            // there is no overlap.
            if (py <= 0)
            {
                return CollisionResponse.NoCollision;
            }

            if (px < py)
            {
                float sx = Math.Sign(dx);
                Vector2 intercect = new Vector2(px * sx, 0);

                Vector2 pos = new Vector2(a.X + (a.Width / 2) * sx, b.Y);

                return new CollisionResponse(true, intercect, pos, new Vector2(sx, 0));
            }
            else
            {
                float sy = Math.Sign(dy);
                Vector2 intercect = new Vector2(0, py * sy);

                Vector2 pos = new Vector2(b.X, a.Y + (a.Width / 2) * sy);

                return new CollisionResponse(true, intercect, pos, new Vector2(0, sy));
            }
        }

        #endregion

        #region AABB vs Point

        // http://web.archive.org/web/20201128223427/https://github.com/noonat/intersect This function here. This function is licensed under the Zlib-Libpng License (Zlib).
        /// <summary>
        /// Checks if a <see cref="Rectangle"/> and a <see cref="Vector2"/> (point) are colliding.
        /// </summary>
        /// <param name="rect">The rectangle to check.</param>
        /// <param name="point">The point.</param>
        /// <returns>
        /// A <see cref="CollisionResponse"/> with collision data and <see cref="CollisionResponse.NoCollision"/> 
        /// when there is no collision.
        /// </returns>
        public static CollisionResponse IsColliding(Rectangle rect, Vector2 point)
        {
            float dx = point.X - rect.Center.X;
            float px = rect.Half().X - Math.Abs(dx);

            if (px <= 0)
            {
                return CollisionResponse.NoCollision;
            }

            float dy = point.Y - rect.Center.Y;
            float py = rect.Half().Y - Math.Abs(dy);

            if (py <= 0)
            {
                return CollisionResponse.NoCollision;
            }

            if (px < py)
            {
                float sx = Math.Sign(dx);
                Vector2 intercect = new Vector2(px * sx, 0);

                Vector2 pos = new Vector2(rect.Center.X + (rect.Half().X * sx), point.Y);

                return new CollisionResponse(true, intercect, pos, new Vector2(sx, 0));
            }
            else
            {
                float sy = Math.Sign(dy);
                Vector2 intercect = new Vector2(0, py * sy);

                Vector2 pos = new Vector2(point.X, rect.Center.Y + (rect.Half().Y * sy));
                return new CollisionResponse(true, intercect, pos, new Vector2(0, sy));
            }

        }

        /// <summary>
        /// Checks if a <see cref="Rectangle"/> and a <see cref="Point"/> are colliding.
        /// </summary>
        /// <param name="rect">The rectangle to check.</param>
        /// <param name="point">The point.</param>
        /// <returns>
        /// A <see cref="CollisionResponse"/> with collision data and <see cref="CollisionResponse.NoCollision"/> 
        /// when there is no collision.
        /// </returns>
        public static CollisionResponse IsColliding(Rectangle rect, Point point) => IsColliding(rect, point.ToVector2());

        #endregion

        #region AABB vs Line Segment

        // http://web.archive.org/web/20201128223427/https://github.com/noonat/intersect This function here. This function is licensed under the Zlib-Libpng License (Zlib).
        /// <summary>
        /// Checks if a <see cref="LineSegment"/> and a <see cref="Rectangle"/> are colliding.
        /// </summary>
        /// <param name="rect">The rectangle to check.</param>
        /// <param name="segment">The segment to use.</param>
        /// <returns>A <see cref="CollisionResponse"/> with the collision data 
        /// and <see cref="CollisionResponse.NoCollision"/> when there is no collision.</returns>
        public static CollisionResponse IsColliding(Rectangle rect, LineSegment segment)
        {
            Vector2 a = segment.A;
            Vector2 b = segment.B;
            Vector2 delta = b - a;

            float scaleX = 1 / delta.X;
            float scaleY = 1 / delta.Y;
            float signX = Math.Sign(scaleX);
            float signY = Math.Sign(scaleY);

            float nearTimeX = (rect.Center.X - signX * (rect.Half().X) - a.X) * scaleX;
            float nearTimeY = (rect.Center.Y - signY * (rect.Half().Y) - a.Y) * scaleY;

            float farTimeX = (rect.Center.X + signX * (rect.Half().X) - a.X) * scaleX;
            float farTimeY = (rect.Center.Y + signY * (rect.Half().Y) - a.Y) * scaleY;

            if (nearTimeX > farTimeY || nearTimeY > farTimeX)
            {
                return CollisionResponse.NoCollision;
            }

            float nearTime = nearTimeX > nearTimeY ? nearTimeX : nearTimeY;
            float farTime = farTimeX < farTimeY ? farTimeX : farTimeY;

            if (nearTime >= 1 || farTime <= 0)
            {
                return CollisionResponse.NoCollision;
            }

            CollisionResponse response = new CollisionResponse();
            response.LineIntercectDistance = nearTime.Clamp(0, 1);

            if (nearTimeX > nearTimeY)
            {
                response.Normal = new Vector2(-signX, 0);
            }
            else
            {
                response.Normal = new Vector2(0, -signY);
            }

            response.IntercectDistance = new Vector2(
                (1 - response.LineIntercectDistance) * -delta.X,
                (1 - response.LineIntercectDistance) * -delta.X);

            response.Position = new Vector2(
                a.X + delta.X * response.LineIntercectDistance,
                a.Y + delta.Y * response.LineIntercectDistance);

            response.Colliding = true;

            return response;

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
