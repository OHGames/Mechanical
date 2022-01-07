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

        public LineSegmentParticleSpawner(Vector2 systemPosition, float extendBy)
        {
            SystemPosition = systemPosition;
            ExtendBy = extendBy;

            a = new Vector2(SystemPosition.X - ExtendBy, SystemPosition.Y);
            b = new Vector2(SystemPosition.X + ExtendBy, SystemPosition.Y);

        }

        public Vector2 Spawn()
        {
            return new Vector2(new Random().Next((int)a.X, (int)b.X), SystemPosition.Y);
        }
    }
}
