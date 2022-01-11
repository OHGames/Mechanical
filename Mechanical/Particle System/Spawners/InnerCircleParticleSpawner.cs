using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// The inner circle particle spawner spawns particles within a circle.
    /// </summary>
    public class InnerCircleParticleSpawner : IParticleSpawner
    {
        public Vector2 SystemPosition { get; set; }

        /// <summary>
        /// The max radius of the circle.
        /// </summary>
        public float MaxRadius { get; set; }

        public InnerCircleParticleSpawner(Vector2 systemPosition, float radius)
        {
            SystemPosition = systemPosition;
            MaxRadius = radius;
        }

        public Vector2 Spawn()
        {
            float radius = new Random().Next(0, (int)MaxRadius);
            float angle = new Random().Next(0, 360);
            angle = angle.ToRadians();

            return SystemPosition + new Vector2((float)Math.Cos(angle) * radius, (float)Math.Sin(angle) * radius);

        }
    }
}
