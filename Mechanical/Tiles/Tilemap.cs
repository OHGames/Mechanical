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
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// The tilemap is an entity that will render <see cref="Tile"/>s
    /// 
    /// TODO: refactor
    /// </summary>
    [DataContract]
    public class Tilemap : Entity, IDrawable
    {
        /// <summary>
        /// The list of tiles.
        /// </summary>
        [DataMember]
        public Tile[,] Tiles { get; set; }

        /// <summary>
        /// The width of each tile.
        /// </summary>
        [DataMember]
        public int TileWidth { get; set; }

        /// <summary>
        /// The height of each tile.
        /// </summary>
        [DataMember]
        public int TileHeight { get; set; }

        /// <summary>
        /// How many tiles there are on the x-axis.
        /// </summary>
        [DataMember]
        public int Width { get; set; }

        /// <summary>
        /// How many tiles there are on the y-axis.
        /// </summary>
        [DataMember]
        public int Height { get; set; }

        /// <summary>
        /// The effect to apply.
        /// </summary>
        public Effect Effect { get; set; } = null;

        /// <summary>
        /// The tint.
        /// </summary>
        [DataMember]
        public Color Tint { get; set; } = Color.White;

        /// <summary>
        /// The render layer.
        /// </summary>
        [DataMember]
        public RenderLayer RenderLayer { get; set; } = RenderLayer.Background;

        /// <summary>
        /// The render order.
        /// </summary>
        [DataMember]
        public int RenderOrder { get; set; } = 0;

        /// <summary>
        /// A private cache to the engine.
        /// </summary>
        private Engine engine;

        /// <summary>
        /// The padded camera view.
        /// </summary>
        private Rectangle paddedRectangle;

        /// <summary>
        /// Make a new tilemap.
        /// </summary>
        /// <param name="name">The entity name.</param>
        /// <param name="width">The width of the tilemap.</param>
        /// <param name="height">The height of the tilemap.</param>
        /// <param name="tileWidth">The width of each tile.</param>
        /// <param name="tileHeight">The height of each tile.</param>
        /// <param name="scene">The scane.</param>
        /// <param name="engine">The engine.</param>
        public Tilemap(string name, int width, int height, int tileWidth, int tileHeight, Scene scene, Engine engine) : base(name, scene)
        {

            Width = width;
            Height = height;
            TileWidth = tileWidth;
            TileHeight = tileHeight;

            Tiles = new Tile[Width, Height];
            this.engine = engine;
            // set the rectangles.
            for (int x = 0; x < Tiles.GetLength(0); x++)
            {
                for (int y = 0; y < Tiles.GetLength(1); y++)
                {
                    Tile t = Tiles[x, y];
                    Vector2 pos = new Vector2(x * TileWidth + Transform.Position.X, y * TileWidth + Transform.Position.Y);
                    t.Rectangle = new Rectangle((int)pos.X, (int)pos.Y, TileWidth, TileHeight);
                    Tiles[x, y] = t;
                }
            }

            if (engine.Camera != null)
                paddedRectangle = engine.Camera.CameraRectPadded;
        }

        public override void Update(float deltaTime)
        {
            // set the rectangles.
            for (int x = 0; x < Tiles.GetLength(0); x++)
            {
                for (int y = 0; y < Tiles.GetLength(1); y++)
                {
                    // update rectangles
                    Tile t = Tiles[x, y];
                    Vector2 pos = new Vector2(x * TileWidth + Transform.Position.X, y * TileWidth + Transform.Position.Y);
                    t.Rectangle = new Rectangle((int)pos.X, (int)pos.Y, TileWidth, TileHeight);
                    Tiles[x, y] = t;
                }
            }

            paddedRectangle = engine.Camera.CameraRectPadded;
            base.Update(deltaTime);
        }

        public override void Draw()
        {
            for (int x = 0; x < Tiles.GetLength(0); x++)
            {
                for (int y = 0; y < Tiles.GetLength(1); y++)
                {

                    Tile t = Tiles[x, y];

                    if (t != Tile.NULL)
                    {
                        if (paddedRectangle.Contains(t.Rectangle.Location.ToVector2()))
                            Drawing.Draw(t.Tilesheet, t.Rectangle, t.SourceRectangle, Tint, Effect);
                    }

                }
            }
        }

        public override void DebugDraw(bool editorRender)
        {
            Draw();

            for (int x = 0; x < Tiles.GetLength(0); x++)
            {
                for (int y = 0; y < Tiles.GetLength(1); y++)
                {
                    Tile t = Tiles[x, y];

                    if (paddedRectangle.Contains(t.Rectangle.Location.ToVector2()))
                        // the position of the current tile.
                        Drawing.DrawRectangle(Tiles[x, y].Rectangle, Color.White, 2 / engine.Camera.ActualZoom);
                }
            }

        }

        /// <summary>
        /// Sets the tile at the specified LOCATION, not INDEX.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="tile">The tile to set.</param>
        public void SetTileAt(int x, int y, Tile tile)
        {
            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < Tiles.GetLength(1); j++)
                {
                    // check to see if location is a tile.
                    if (Tiles[i, j].Rectangle.Contains(x, y))
                    {
                        // set tile
                        Tiles[i, j] = tile;
                    }
                }
            }
        }

        /// <summary>
        /// Sets the tile at the specified LOCATION, not INDEX.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <param name="tile">The tile to set.</param>
        public void SetTileAt(Vector2 position, Tile tile)
        {
            SetTileAt((int)position.X, (int)position.Y, tile);
        }

        /// <summary>
        /// Set the tile at the specified INDEX, not POSITION.
        /// </summary>
        /// <param name="x">The first index.</param>
        /// <param name="y">The second index.</param>
        /// <param name="tile">The tile to set.</param>
        public void SetTile(int x, int y, Tile tile)
        {
            if (x >= Tiles.GetLength(0) || y >= Tiles.GetLength(1) || x < 0 || y < 0) throw new ArgumentException("Invalid index");

            Tiles[x, y] = tile;
        }

        /// <summary>
        /// Set the tile at the specified INDEX, not POSITION.
        /// </summary>
        /// <param name="tile">The tile to set.</param>
        /// <param name="index">The index.</param>
        public void SetTile(Vector2 index, Tile tile)
        {
            if (index.X >= Tiles.GetLength(0) || index.Y >= Tiles.GetLength(1) || index.X < 0 || index.Y < 0) throw new ArgumentException("Invalid index");

            Tiles[(int)index.X, (int)index.Y] = tile;
        }

    }
}
