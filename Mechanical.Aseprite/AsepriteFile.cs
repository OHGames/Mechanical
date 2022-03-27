/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O. H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/Use%20Code%20Licenses.md for more info.
 */

using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            AsepriteFile file = AsepriteParser.Parse(fileContent);

            file.FileDirectory = path;
            file.FileName = Path.GetFileNameWithoutExtension(path);

            return file;
        }

        /// <summary>
        /// Get the layers of the file.
        /// </summary>
        /// <returns>The layers.</returns>
        public IEnumerable<AsepriteLayer> GetLayers() => new List<AsepriteLayer>(Layers);

        /// <summary>
        /// Get the frames of the file.
        /// </summary>
        /// <returns>The frames.</returns>
        public IEnumerable<AsepriteFrame> GetFrames() => new List<AsepriteFrame>(Frames);

        /// <summary>
        /// Get the frame tags of the file.
        /// </summary>
        /// <returns>The frame tags.</returns>
        public IEnumerable<AsepriteFrameTag> GetFrameTags() => new List<AsepriteFrameTag>(FrameTags);

        /// <summary>
        /// Get the slices.
        /// </summary>
        /// <returns>The slices.</returns>
        public IEnumerable<AsepriteSlice> GetSlices() => new List<AsepriteSlice>(Slices);

        /// <summary>
        /// Gets a frame tag based on its name.
        /// </summary>
        /// <param name="name">The name of the tag.</param>
        /// <returns>An <see cref="AsepriteFrameTag"/>.</returns>
        /// <exception cref="ArgumentException">When the frame <paramref name="name"/> is not in the file.</exception>
        public AsepriteFrameTag GetFrameTag(string name)
        {
            // there should only be one of the name so get the first.
            AsepriteFrameTag tag = FrameTags.Where(t => t.Name.ToLower() == name.ToLower()).FirstOrDefault();
            
            // if the tag was not found.
            if (tag.Name == null)
            {
                throw new ArgumentException($"The frame tag name, {name}, is not valid.");
            }
            else
            {
                return tag;
            }
        }

        /// <summary>
        /// Gets a slice based on its name.
        /// </summary>
        /// <param name="name">The name of the slice.</param>
        /// <returns>An <see cref="AsepriteSlice"/></returns>
        /// <exception cref="ArgumentException">When the slice <paramref name="name"/> is not in the file.</exception>
        public AsepriteSlice GetSlice(string name)
        {
            // there should only be one so get the first.
            AsepriteSlice slice = Slices.Where(s => s.Name.ToLower() == name.ToLower()).FirstOrDefault();

            // if the slice was not found.
            if (slice.Name == null)
            {
                throw new ArgumentException($"The slice name, {name}, is not valid.");
            }
            else
            {
                return slice;
            }
        }

        /// <summary>
        /// Gets a layer based on its name.
        /// </summary>
        /// <param name="name">The name of the layer.</param>
        /// <returns>An <see cref="AsepriteLayer"/></returns>
        /// <exception cref="ArgumentException">When the layer <paramref name="name"/> is not in the file.</exception>
        public AsepriteLayer GetLayer(string name)
        {
            // there should only be one so get the first.
            AsepriteLayer layer = Layers.Where(l => l.Name.ToLower() == name.ToLower()).FirstOrDefault();

            // if the layer was not found.
            if (layer.Name == null)
            {
                throw new ArgumentException($"The layer name, {name}, is not valid.");
            }
            else
            {
                return layer;
            }
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
