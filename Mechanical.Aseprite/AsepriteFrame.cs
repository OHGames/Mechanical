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

namespace Mehcanical.Aseprite
{
    /// <summary>
    /// The Aseprite Frame struct represents a frame from an <see cref="AsepriteFile"/>.
    /// </summary>
    public struct AsepriteFrame
    {

        /// <summary>
        /// The rectangle that represents this frame on the image.
        /// </summary>
        public Rectangle FrameRect { get; set; }

        /// <summary>
        /// The sprite source size. (initial image size)
        /// </summary>
        public Rectangle SpriteSourceSize { get; set; }

        /// <summary>
        /// The frame duration in miliseconds.
        /// </summary>
        public int FrameDuration { get; set; }

        /// <summary>
        /// The frame duration in seconds.
        /// </summary>
        public float FrameDurationInSeconds => FrameDuration / 1000;

    }
}
