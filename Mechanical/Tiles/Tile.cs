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
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// A tile is a 2d object that will be rendered on a <see cref="Tilemap"/>
    /// </summary>
    [DataContract]
    public struct Tile : ICloneable
    {
        /// <summary>
        /// The rectangle that will be used to draw the texture. Source rectangle.
        /// </summary>
        [DataMember]
        public Rectangle? SourceRectangle { get; set; }

        /// <summary>
        /// This rectangle is the tile on-screen.
        /// </summary>
        [DataMember]
        public Rectangle Rectangle { get; set; }

        /// <summary>
        /// The texture that will be used to draw.
        /// </summary>
        public Texture2D Tilesheet { get; set; }

        /// <summary>
        /// A null tile.
        /// </summary>
        public static Tile NULL = new Tile(null, null, Rectangle.Empty);

        public Tile(Rectangle? sourceRectangle, Texture2D texture, Rectangle rectangle)
        {
            SourceRectangle = sourceRectangle;
            Tilesheet = texture;
            Rectangle = rectangle;
        }

        public object Clone()
        {
            return new Tile(SourceRectangle, Tilesheet, Rectangle);
        }

        public static bool operator ==(Tile t1, Tile t2)
        {
            return t1.SourceRectangle == t2.SourceRectangle && t1.Tilesheet == t2.Tilesheet && t1.Rectangle == t2.Rectangle;
        }

        public static bool operator !=(Tile t1, Tile t2)
        {
            return !(t1 == t2);
        }


    }
}
