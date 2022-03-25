/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O. H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/USED_CODE_LICENSES.txt for more info.
 */

using Mechanical;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mehcanical.Aseprite
{
    /// <summary>
    /// The Aseprite Layer struct represents an Aseprite layer.
    /// </summary>
    public struct AsepriteLayer
    {

        /// <summary>
        /// The name of the layer.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The color of the layer.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// The user data.
        /// </summary>
        public string UserData { get; set; }

        /// <summary>
        /// The opacity of the layer.
        /// </summary>
        public int Opacity { get; set; }

        /// <summary>
        /// The normalized opacity is the opacity between 0 and 1.
        /// </summary>
        public float NormalizedOpacity => Opacity.Normalize(0, 255);

        /// <summary>
        /// The blend mode of the layer.
        /// </summary>
        public string BlendMode { get; set; }

    }
}
