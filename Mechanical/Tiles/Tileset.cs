using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Mechanical
{
    /// <summary>
    /// A tileset holds a list of <see cref="Tile"/>s that are created from an image.
    /// </summary>
    [DataContract]
    public class Tileset
    {

        /// <summary>
        /// The image that the tileset will use.
        /// </summary>
        public Texture2D TilesetImage { get; set; }

        /// <summary>
        /// The list of tiles in the tilemap.
        /// </summary>
        [DataMember]
        public Tile[] Tiles { get; set; }

        /// <summary>
        /// The width of the tiles.
        /// </summary>
        [DataMember]
        public int TileWidth { get; private set; }

        /// <summary>
        /// The height of the tiles.
        /// </summary>
        [DataMember]
        public int TileHeight { get; private set; }

        /// <summary>
        /// Make a new tileset and automatically map the tiles.
        /// </summary>
        /// <param name="texture">The texture of the tileset.</param>
        /// <param name="tileWidth">The width of each tile on the image.</param>
        /// <param name="tileHeight">The height of the tile on the image.</param>
        public Tileset(Texture2D texture, int tileWidth, int tileHeight)
        {
            TilesetImage = texture;
            TileWidth = tileWidth;
            TileHeight = tileHeight;

            Tiles = MapTiles();
        }

        /// <summary>
        /// Make a new tilemap with a texture and a list of tiles. The tiles must have the same height and width.
        /// </summary>
        /// <param name="texture">The texture of the tileset.</param>
        /// <param name="tiles">The tiles.</param>
        public Tileset(Texture2D texture, Tile[] tiles)
        {
            TilesetImage = texture;
            Tiles = tiles;

            if (tiles[0].SourceRectangle == null)
            {
                TileWidth = tiles[0].Tilesheet.Width;
                TileHeight = tiles[0].Tilesheet.Height;
            }
            else
            {
                TileWidth = (int)(tiles[0].SourceRectangle?.Width);
                TileHeight = (int)(tiles[0].SourceRectangle?.Height);
            }
            
        }

        /// <summary>
        /// Make a new tilemap with a texture and a list of tiles. The tiles must have the same height and width.
        /// </summary>
        /// <param name="texture">The texture of the tileset.</param>
        /// <param name="tiles">The tiles.</param>
        public Tileset(Texture2D texture, List<Tile> tiles) : this(texture, tiles.ToArray())
        {

        }

        /// <summary>
        /// Automatically map the tiles.
        /// </summary>
        /// <returns>A list of the tiles.</returns>
        public Tile[] MapTiles()
        {
            // how many tiles can fit in the set.
            int widthCount = TilesetImage.Width / TileWidth;
            int heightCount = TilesetImage.Height / TileHeight;
            int tileCount = widthCount + heightCount;

            Tile[] tiles = new Tile[tileCount];

            int x = 0;
            int y = 0;
            for (int i = 0; i < tileCount; i++)
            {
                // set rectangle to empty because the tilemap will set it.
                tiles[i] = new Tile(new Rectangle(x, y, TileWidth, TileHeight), TilesetImage, Rectangle.Empty);
                x += TileWidth;

                if (x >= TilesetImage.Width)
                {
                    x = 0;
                    y += TileHeight;
                }
            }


            return tiles;
        }

    }
}
