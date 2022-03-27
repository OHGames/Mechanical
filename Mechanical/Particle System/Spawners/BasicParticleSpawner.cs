/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O. H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/Use%20Code%20Licenses.md for more info.
 */

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// The basic particle spawner spawns the particles at the system's location.
    /// </summary>
    public class BasicParticleSpawner : IParticleSpawner
    {
        public Vector2 SystemPosition { get; set; }

        public Vector2 Spawn()
        {
            return SystemPosition;
        }
    }
}
