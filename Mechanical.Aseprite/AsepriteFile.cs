/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O. H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/USED_CODE_LICENSES.txt for more info.
 */

using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
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
        public string Image { get; set; }

        /// <summary>
        /// The frames in this file.
        /// </summary>
        public List<AsepriteFrame> Frames { get; set; } = new List<AsepriteFrame>();

        /// <summary>
        /// The frame tags in this file.
        /// </summary>
        public List<AsepriteFrameTag> FrameTags { get; set; } = new List<AsepriteFrameTag>();

        /// <summary>
        /// The slices in the file.
        /// </summary>
        public List<AsepriteSlice> Slices { get; set; } = new List<AsepriteSlice>();

        /// <summary>
        /// The layers in the file.
        /// </summary>
        public List<AsepriteLayer> Layers { get; set; } = new List<AsepriteLayer>();

        
        /// <summary>
        /// Load an Aseprite file.
        /// </summary>
        /// <param name="content">The content manager.</param>
        /// <param name="path">The path relative to the content folder specified in <see cref="ContentManager.RootDirectory"/>.</param>
        /// <returns>A new <see cref="AsepriteFile"/>.</returns>
        public static AsepriteFile Load(ContentManager content, string path)
        {
            return Load($"{Environment.CurrentDirectory}/{content.RootDirectory}/{path}.json");
        }

        /// <summary>
        /// Load an Aseprite file.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <returns>A new <see cref="AsepriteFile"/>.</returns>
        public static AsepriteFile Load(string path)
        {
            // get the content.
            string fileContent = File.ReadAllText(path);

            return AsepriteParser.Parse(fileContent);
        }

    }

    /// <summary>
    /// Extensions.
    /// </summary>
    public static class ContentManagerExtensions
    {

        /// <summary>
        /// Load an Aseprite file.
        /// </summary>
        /// <param name="manager">The content manager.</param>
        /// <param name="path">The path relative to the content folder specified in <see cref="ContentManager.RootDirectory"/>.</param>
        /// <returns>A new <see cref="AsepriteFile"/>.</returns>
        public static AsepriteFile Load(this ContentManager manager, string path)
        {
            return AsepriteFile.Load(manager, path);
        }

    }

}
