using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// The incremental circle particle spawner spawns particles in an incremental manner in the shape of a circle.
    /// </summary>
    public class IncrementalCircleParticleSpawner : IParticleSpawner
    {
        public Vector2 SystemPosition { get; set; }

        /// <summary>
        /// The current increment.
        /// </summary>
        private float currentIncrement;

        /// <summary>
        /// The increment.
        /// </summary>
        public float Increment { get; set; }
        
        /// <summary>
        /// The radius of the circle.
        /// </summary>
        public float Radius { get; set; }

        public IncrementDirection Direction { get; set; }

        public IncrementalCircleParticleSpawner (
                float radius, 
                float increment = 10, 
                float currentIncrement = 0, 
                IncrementDirection direction = IncrementDirection.Clockwise
            )
        {
            Radius = radius;
            Increment = increment;
            this.currentIncrement = currentIncrement;
            Direction = direction;
        }

        public Vector2 Spawn()
        {
            float rad = currentIncrement.ToRadians();
            //https://gamedev.stackexchange.com/a/9610
            Vector2 pos = SystemPosition + new Vector2((float)Math.Cos(rad) * Radius, (float)Math.Sin(rad) * Radius);

            if (Direction == IncrementDirection.Clockwise)
                currentIncrement += Increment;
            else
                currentIncrement -= Increment;

            return pos;
        }
    }
}
