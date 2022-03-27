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
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// The particle is a single peice of a particle system.
    /// </summary>
    public struct Particle
    {

        /// <summary>
        /// The position of the particle.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The rotation of the particle.
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// The starting rotation of the particle.
        /// </summary>
        public float StartRotation { get; set; }

        /// <summary>
        /// The ending rotation of the particle.
        /// </summary>
        public float EndRotation { get; set; }

        /// <summary>
        /// The size of the particle.
        /// </summary>
        public Vector2 Size { get; set; }

        /// <summary>
        /// The starting size of the particle.
        /// </summary>
        public Vector2 StartSize { get; set; }

        /// <summary>
        /// The end size of the particle.
        /// </summary>
        public Vector2 EndSize { get; set; }

        /// <summary>
        /// The texture of the particle.
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// The source rectangle of the <see cref="Texture"/>.
        /// </summary>
        public Rectangle? SourceRectangle { get; set; }

        /// <summary>
        /// The animation.
        /// </summary>
        public SpriteAnimation Animation { get; set; }

        /// <summary>
        /// The velocity of the particle.
        /// </summary>
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// The speed of the particle.
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        /// The starting speed of the particle.
        /// </summary>
        public float StartSpeed { get; set; }

        /// <summary>
        /// The ending speed of the particle.
        /// </summary>
        public float EndSpeed { get; set; }

        /// <summary>
        /// The angle that the particle will move at.
        /// </summary>
        public float Angle { get; set; }

        /// <summary>
        /// The starting life of the particle.
        /// </summary>
        public float StartLife { get; set; }

        /// <summary>
        /// The current life of the particle.
        /// </summary>
        public float Life { get; set; }

        /// <summary>
        /// How long the particle has been alive.
        /// </summary>
        public float TimeAlive { get; set; }

        /// <summary>
        /// If the particle is alive.
        /// </summary>
        public bool IsAlive { get; set; }

        /// <summary>
        /// The color of the particle.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// The starting color of the particle.
        /// </summary>
        public Color StartColor { get; set; }

        /// <summary>
        /// The end color of the particle.
        /// </summary>
        public Color EndColor { get; set; }

        /// <summary>
        /// The opacity of the particle.
        /// </summary>
        public float Opacity 
        { 
            get
            {
                return opacity;
            }
            set
            {
                opacity = value.Normalize(0, 255);
            }
        }

        /// <summary>
        /// The opacity of the particle.
        /// </summary>
        private float opacity;

        /// <summary>
        /// The starting opacity of the particle.
        /// </summary>
        private float startOpacity;

        /// <summary>
        /// The starting opacity of the particle.
        /// </summary>
        public float StartOpacity
        {
            get { return startOpacity; }
            set { startOpacity = value.Normalize(0, 255); }
        }

        /// <summary>
        /// The ending opacity of the particle.
        /// </summary>
        private float endOpacity;

        /// <summary>
        /// The ending opacity of the particle.
        /// </summary>
        public float EndOpacity
        {
            get { return endOpacity; }
            set { endOpacity = value.Normalize(0, 255); }
        }

        /// <summary>
        /// The system that the particle is part of.
        /// </summary>
        private ParticleSystem system;

        /// <summary>
        /// The life ratio.
        /// </summary>
        private float lifeRatio;

        public Particle(Vector2 position, float rotation, Vector2 size, Texture2D texture, float speed, float angle, float life, Color color, ParticleSystem system, float opacity = 255, Rectangle? sourceRectangle = null) : this()
        {
            Position = position;
            Rotation = rotation;
            Size = size;
            Texture = texture;
            Speed = speed;
            Angle = angle;
            StartLife = Life = life;
            SourceRectangle = sourceRectangle;

            var rad = angle.ToRadians();

            Velocity = new Vector2()
            {
                X = speed * (float)Math.Cos(rad),
                Y = -speed * (float)Math.Sin(rad),
            };

            Color = color;
            Opacity = opacity;

            this.system = system;
        }

        public Particle(ParticleSystem system) : this()
        {
            this.system = system;
        }

        public void Update(float deltaTime)
        {
            // get radians
            var rad = Angle.ToRadians();

            // http://buildnewgames.com/particle-systems/
            // set velocity.
            Velocity = new Vector2()
            {
                X = Speed * (float)Math.Cos(rad),
                Y = -Speed * (float)Math.Sin(rad),
            };

            // TODO: change the way gravity is implemented, add acceleration variable

            Position -= system.Gravity * deltaTime;
            Position += Velocity * deltaTime;

            lifeRatio = TimeAlive / Life;

            Color = Color.Lerp(StartColor, EndColor, lifeRatio);
            Size = Vector2.Lerp(StartSize, EndSize, lifeRatio);
            // end http://buildnewgames.com/particle-systems/
            Rotation = MathHelper.Lerp(StartRotation, EndRotation, lifeRatio);
            Speed = MathHelper.Lerp(StartSpeed, EndSpeed, lifeRatio);
            opacity = MathHelper.Lerp(StartOpacity, EndOpacity, lifeRatio);
            // TODO: implement more interpolation types later

            if (Animation != null)
            {
                Animation.Update(deltaTime);

                SourceRectangle = Animation.GetCurrentRectangle();
            }

            TimeAlive++;

        }

    }
}
