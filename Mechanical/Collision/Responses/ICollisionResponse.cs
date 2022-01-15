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
    /// Represents a data that is collected from 2 overlaping shapes.
    /// </summary>
    /// <typeparam name="T">The type of shape.</typeparam>
    public interface ICollisionResponse<T>
    {

        /// <summary>
        /// If there is a collision.
        /// </summary>
        bool Colliding { get; set; }

        /// <summary>
        /// The other object that was collided with.
        /// </summary>
        T Other { get; set; }

    }
}
