using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// The tilemap is an entity that will render <see cref="Tile"/>s
    /// 
    /// TODO: refactor
    /// </summary>
    public class Tilemap : Entity, IDrawable
    {
        /// <summary>
        /// The list of tiles.
        /// </summary>
        public Tile[,] Tiles { get; set; }

        /// <summary>
        /// The width of each tile.
        /// </summary>
        public int TileWidth { get; set; }

        /// <summary>
        /// The height of each tile.
        /// </summary>
        public int TileHeight { get; set; }

        /// <summary>
        /// How many tiles there are on the x-axis.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// How many tiles there are on the y-axis.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// The effect to apply.
        /// </summary>
        public Effect Effect { get; set; } = null;

        /// <summary>
        /// The tint.
        /// </summary>
        public Color Tint { get; set; } = Color.White;

        /// <summary>
        /// The render layer.
        /// </summary>
        public RenderLayer RenderLayer { get; set; } = RenderLayer.Background;

        /// <summary>
        /// The render order.
        /// </summary>
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

                    if (t == Tile.NULL)
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
                        Drawing.DrawRectangle(Tiles[x, y].Rectangle, Color.White, 4 / engine.Camera.ActualZoom);
                }
            }

        }

    }
}
