﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// A tile is a 2d object that will be rendered on a <see cref="Tilemap"/>
    /// </summary>
    public struct Tile : ICloneable
    {
        /// <summary>
        /// The rectangle that will be used to draw the texture. Source rectangle.
        /// </summary>
        public Rectangle? SourceRectangle { get; set; }

        /// <summary>
        /// This rectangle is the tile on-screen.
        /// </summary>
        public Rectangle Rectangle { get; set; }

        /// <summary>
        /// If this tile can collide.
        /// </summary>
        public bool CanCollide { get; set; }

        /// <summary>
        /// The texture that will be used to draw.
        /// </summary>
        public Texture2D Tilesheet { get; set; }

        /// <summary>
        /// A null tile.
        /// </summary>
        public static Tile NULL = new Tile(null, false, null, Rectangle.Empty);

        public Tile(Rectangle? sourceRectangle, bool collidable, Texture2D texture, Rectangle rectangle)
        {
            SourceRectangle = sourceRectangle;
            CanCollide = collidable;
            Tilesheet = texture;
            Rectangle = rectangle;
        }

        public object Clone()
        {
            return new Tile(SourceRectangle, CanCollide, Tilesheet, Rectangle);
        }

        public static bool operator ==(Tile t1, Tile t2)
        {
            return t1.SourceRectangle == t2.SourceRectangle && t1.CanCollide == t2.CanCollide && t1.Tilesheet == t2.Tilesheet && t1.Rectangle == t2.Rectangle;
        }

        public static bool operator !=(Tile t1, Tile t2)
        {
            return !(t1 == t2);
        }


    }
}