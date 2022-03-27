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

namespace Mehcanical.Aseprite
{
    /// <summary>
    /// The Aseprite Slice struct represents an Aseprite slice.
    /// </summary>
    public struct AsepriteSlice
    {

        /// <summary>
        /// The name of the slice.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The color of the slice.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Some user-specified data about this slice.
        /// </summary>
        public string UserData { get; set; }

        /// <summary>
        /// The keys of this slice.
        /// </summary>
        public List<AsepriteSliceKey> Keys { get; set; }

    }

    /// <summary>
    /// The slice information for specific frames.
    /// </summary>
    public struct AsepriteSliceKey
    {
        /// <summary>
        /// The 0-based index of the frame that this information is for.
        /// </summary>
        public int Frame { get; set; }

        /// <summary>
        /// The slice's shape.
        /// </summary>
        public Rectangle Bounds { get; set; }

        /// <summary>
        /// The pivot point of the slice.
        /// </summary>
        public Vector2 Pivot { get; set; }

        /// <summary>
        /// The 9-sclice center of the sprite.
        /// </summary>
        public Rectangle NineSlice { get; set; }
    }

}
