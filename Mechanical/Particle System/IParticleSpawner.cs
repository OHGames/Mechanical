/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O.H. Games
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
    /// A particle spawner determines the position of a particle relative to the position of the <see cref="ParticleSystem"/>.
    /// </summary>
    public interface IParticleSpawner
    {

        /// <summary>
        /// The position of the system.
        /// </summary>
        Vector2 SystemPosition { get; set; }

        /// <summary>
        /// Spawn a particle by placing it in the right position.
        /// </summary>
        Vector2 Spawn();

    }
}
