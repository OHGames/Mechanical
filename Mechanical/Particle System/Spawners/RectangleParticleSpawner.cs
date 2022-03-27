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
using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// The rectangle particle spawner spawns particles on a square.
    /// </summary>
    public class RectangleParticleSpawner : IParticleSpawner
    {
        public Vector2 SystemPosition { get; set; }

        /// <summary>
        /// The width of the rectangle from the center (<see cref="SystemPosition"/>)
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// The height of the rectangle from the center (<see cref="SystemPosition"/>)
        /// </summary>
        public float Height { get; set; }

        public RectangleParticleSpawner(float width, float height)
        {
            Width = width;
            Height = height;
        }

        public RectangleParticleSpawner(Rectangle rectangle) : this(rectangle.Width, rectangle.Height)
        {

        }

        public Vector2 Spawn()
        {
            // get a random side.
            float side = new Random().Next(1, 5);

            switch (side)
            {
                // top
                case 1:
                    return new Vector2(new Random().Next((int)-Width, (int)Width), -Height) + SystemPosition;
                // right
                case 2:
                    return new Vector2(Width, new Random().Next((int)-Height, (int)Height)) + SystemPosition;
                // bottom
                case 3:
                    return new Vector2(new Random().Next((int)-Width, (int)Width), Height) + SystemPosition;
                // left
                case 4:
                    return new Vector2(-Width, new Random().Next((int)-Height, (int)Height)) + SystemPosition;
            }

            throw new Exception($"The side, {side}, is not between 1 and 4 somehow????");
        }
    }
}
