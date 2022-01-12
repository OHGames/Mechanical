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
    /// The inner circle particle spawner spawns particles within a circle.
    /// </summary>
    public class InnerCircleParticleSpawner : IParticleSpawner
    {
        public Vector2 SystemPosition { get; set; }

        /// <summary>
        /// The max radius of the circle.
        /// </summary>
        public float MaxRadius { get; set; }

        public InnerCircleParticleSpawner(float radius)
        {
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
