using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// A sprite atlas is a class that contains a <see cref="Texture2D"/> and a <see cref="Dictionary{TKey, TValue}"/> on images inside the atlas. It basically is an image with data on it.
    /// </summary>
    public struct SpriteAtlas
    {

        /// <summary>
        /// The texture of the atlas.
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// The list of rectangles that correspond with a rectangle.
        /// </summary>
        public Dictionary<string, Rectangle> Subimages { get; set; }

        public SpriteAtlas(Texture2D texture, Dictionary<string, Rectangle> subimages)
        {
            Texture = texture;
            Subimages = subimages;
        }
    }
}
