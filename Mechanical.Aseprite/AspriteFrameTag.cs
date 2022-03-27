/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O. H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/Use%20Code%20Licenses.md for more info.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mehcanical.Aseprite
{
    /// <summary>
    /// The Aseprite Tag is a struct that represents Asprite's frame tag system.
    /// </summary>
    public struct AsepriteFrameTag
    {
        /// <summary>
        /// The direction of the tag.
        /// </summary>
        public AsepriteFrameTagDirection Direction { get; set; }

        /// <summary>
        /// What frame the tag starts on (0-based index).
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// What frame the tag ends on (0-based index).
        /// </summary>
        public int End { get; set; }

        /// <summary>
        /// The name of the tag.
        /// </summary>
        public string Name { get; set; }

    }

    /// <summary>
    /// The direction a <see cref="AsepriteFrameTag"/> can move in.
    /// </summary>
    public enum AsepriteFrameTagDirection
    {
        /// <summary>
        /// The animation goes forward.
        /// </summary>
        Forward,

        /// <summary>
        /// The animation goes backward.
        /// </summary>
        Reverse,

        /// <summary>
        /// The animation will play forward then play backward when it reaches the end.
        /// </summary>
        PingPong
    }

}
