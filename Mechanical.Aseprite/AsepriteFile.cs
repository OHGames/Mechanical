/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O. H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/USED_CODE_LICENSES.txt for more info.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mehcanical.Aseprite
{
    /// <summary>
    /// The Asprite File class represents an Asprite spritesheet export.
    /// </summary>
    /// <remarks>
    /// This is not to be confused with <c>.ase</c> or <c>.aseprite</c> files.
    /// </remarks>
    public class AsepriteFile
    {

        /// <summary>
        /// The directory of the file.
        /// </summary>
        public string FileDirectory { get; set; }

        /// <summary>
        /// The name of the file.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The name of the image that this Asprite file is made for.
        /// </summary>
        public string Image { get; }

        /// <summary>
        /// The frames in this file.
        /// </summary>
        public List<AspriteFrame> Frames { get; set; } = new List<AspriteFrame>();

        /// <summary>
        /// The frame tags in this file.
        /// </summary>
        public List<AsepriteFrameTag> FrameTags { get; set; } = new List<AsepriteFrameTag>();

        /// <summary>
        /// The slices in the file.
        /// </summary>
        public List<AsepriteSlice> Slices { get; set; } = new List<AsepriteSlice>();

    }
}
