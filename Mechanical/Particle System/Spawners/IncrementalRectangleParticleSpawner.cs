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
    /// The incremental rectangle particle spawner spawns particles with an increment.
    /// </summary>
    public class IncrementalRectangleParticleSpawner : IParticleSpawner
    {
        public Vector2 SystemPosition { get; set; }

        /// <summary>
        /// the current increment.
        /// </summary>
        private float currentIncrement;

        /// <summary>
        /// How much the increment should change each particle.
        /// </summary>
        public float Increment { get; set; }

        /// <summary>
        /// The width of the rectangle from the center (<see cref="SystemPosition"/>)
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// The height of the rectangle from the center (<see cref="SystemPosition"/>)
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        /// The direction to increment.
        /// </summary>
        public IncrementDirection Direction { get; set; }

        /// <summary>
        /// The current side the increment is on.
        /// </summary>
        private float currentSide = 1;

        public IncrementalRectangleParticleSpawner ( 
                float width, 
                float height, 
                IncrementDirection direction = IncrementDirection.Clockwise, 
                float currentIncrement = 0, 
                float increment = 10 
            )
        {
            this.currentIncrement = currentIncrement;
            Increment = increment;
            Width = width;
            Height = height;
            Direction = direction;
        }

        public IncrementalRectangleParticleSpawner(
                Rectangle rectangle,
                IncrementDirection direction = IncrementDirection.Clockwise,
                float currentIncrement = 0,
                float increment = 10
            ) 
            : this(rectangle.Width, rectangle.Height, direction, currentIncrement, increment)
        {

        }

        public Vector2 Spawn()
        {
            // the dir multiplies the min and max so that the direction changes based on rotation direction.
            float dir = 1;
            if (Direction == IncrementDirection.CounterClockwise)
                dir = -1;

            switch (currentSide)
            {
                // top
                case 1:
                    // multiply by 2 because we are shifting the whole numberline by Width so 0 == -Width.
                    float normal1 = (currentIncrement * 2).Normalize(0, Width * 2);

                    IncrementCurrent();

                    return 
                        Vector2.Lerp(new Vector2(-Width * dir, -Height), new Vector2(Width * dir, -Height), normal1)
                        + SystemPosition;
                // right
                case 2:
                    float normal2 = (currentIncrement * 2).Normalize(0, Height * 2);

                    IncrementCurrent();

                    return 
                        Vector2.Lerp(new Vector2(Width * dir, -Height), new Vector2(Width * dir, Height), normal2)
                        + SystemPosition;
                // bottom
                case 3:
                    float normal3 = (currentIncrement * 2).Normalize(0, Width * 2);

                    IncrementCurrent();

                    return
                        Vector2.Lerp(new Vector2(Width * dir, Height), new Vector2(-Width * dir, Height), normal3)
                        + SystemPosition;
                // left
                case 4:
                    float normal4 = (currentIncrement * 2).Normalize(0, Height * 2);

                    IncrementCurrent();

                    return
                        Vector2.Lerp(new Vector2(-Width * dir, Height), new Vector2(-Width * dir, -Height), normal4)
                        + SystemPosition;

            }
            return Vector2.Zero;
        }

        private void IncrementCurrent()
        {
            currentIncrement += Increment;

            if (currentIncrement >= 100)
            {
                currentIncrement = 0;
                currentSide++;
            }
            if (currentSide > 4)
            {
                currentSide = 1;
                currentIncrement = 0;
            }
        }

    }
}
