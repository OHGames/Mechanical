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
    /// A particle spawner that spawns a particle in a circle.
    /// </summary>
    public class CircleParticleSpawner : IParticleSpawner
    {
        public Vector2 SystemPosition { get; set; }

        /// <summary>
        /// The radius of the circle.
        /// </summary>
        public float Radius { get; set; }

        public CircleParticleSpawner(float radius)
        {
            Radius = radius;
        }

        public CircleParticleSpawner(Circle circle)
        {
            Radius = circle.Radius;
        }

        public Vector2 Spawn()
        {
            // get random angle.
            float angle = new Random().Next(0, 360);
            angle = angle.ToRadians();

            // use math.
            // https://gamedev.stackexchange.com/a/9610/148084
            return SystemPosition + new Vector2((float)Math.Cos(angle) * Radius, (float)Math.Sin(angle) * Radius);
        }
    }
}
