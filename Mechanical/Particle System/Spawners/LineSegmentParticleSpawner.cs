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
    /// The line segment particle spawner emits particles along a line with the system at the center.
    /// </summary>
    public class LineSegmentParticleSpawner : IParticleSpawner
    {
        public Vector2 SystemPosition { get; set; }

        /// <summary>
        /// The left side.
        /// </summary>
        private Vector2 a;

        /// <summary>
        /// The right side.
        /// </summary>
        private Vector2 b;

        /// <summary>
        /// How far each side should extend from the center (<see cref="SystemPosition"/>).
        /// </summary>
        public float ExtendBy { get; set; }

        public LineSegmentParticleSpawner(float extendBy)
        {
            ExtendBy = extendBy;

            a = new Vector2(SystemPosition.X - ExtendBy, SystemPosition.Y);
            b = new Vector2(SystemPosition.X + ExtendBy, SystemPosition.Y);
        }

        public LineSegmentParticleSpawner(LineSegment segment)
        {
            ExtendBy = segment.Distance / 2;

            a = segment.A;
            b = segment.B;
        }

        public Vector2 Spawn()
        {
            a = new Vector2(SystemPosition.X - ExtendBy, SystemPosition.Y);
            b = new Vector2(SystemPosition.X + ExtendBy, SystemPosition.Y);
            return new Vector2(new Random().Next((int)a.X, (int)b.X), SystemPosition.Y);
        }
    }
}
