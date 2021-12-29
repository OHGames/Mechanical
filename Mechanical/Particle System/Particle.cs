using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// A single particle that is part of a <see cref="ParticleSystem"/>
    /// </summary>
    public struct Particle
    {

        /// <summary>
        /// How long the particle has been alive for.
        /// </summary>
        public float AliveTime { get; set; }

        /// <summary>
        /// The color of the particle.
        /// </summary>
        private Color color;

        /// <summary>
        /// The color of the particle.
        /// </summary>
        public Color Color
        {
            get { return color * Transparency; }
            set { color = value; Transparency = value.A; }
        }

        /// <summary>
        /// The transparency.
        /// </summary>
        private float transparency;

        /// <summary>
        /// The transparency.
        /// </summary>
        public float Transparency
        {
            get { return Color.A; }
            set { transparency = value.Clamp(0, 256).Normalize(0, 256); }
        }

        /// <summary>
        /// TTL means Time-To-Live, it is how long a particle will be alive for.
        /// </summary>
        public float TTL { get; set; }

        /// <summary>
        /// How much time this particle has left to live.
        /// </summary>
        public float TimeRemaining { get => TTL - AliveTime; }

        /// <summary>
        /// The rotation of the particle.
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// The spin velocity is how fast the particle will spin. Set to a negative number to spin backwards and vice versa.
        /// </summary>
        public float SpinVelocity { get; set; }

        /// <summary>
        /// The velocity of the particle.
        /// </summary>
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// The position of the particle.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The system that the particle is part of.
        /// </summary>
        public ParticleSystem System { get; set; }

        /// <summary>
        /// The scale of the particle.
        /// </summary>
        public Vector2 Scale { get; set; }

        /// <summary>
        /// How much the particle will scale by.
        /// </summary>
        public Vector2 ScaleVelocity { get; set; }

        /// <summary>
        /// The texture that the particle will use.
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// The origin of the particle.
        /// </summary>
        public Vector2 Origin { get; set; }

        /// <summary>
        /// The animation that the particle has. Set the animation to NULL for no animation.
        /// </summary>
        public SpriteAnimation Animation { get; set; }

        /// <summary>
        /// If the particle is animnated.
        /// </summary>
        public bool IsAnimated
        {
            get
            {
                return Animation != null;
            }
            set
            {
                if (value == false)
                {
                    Animation = null;
                }
            }
        }

        // thank you visual studio for generating this huge ctor :)
        /// <summary>
        /// Makes a new particle.
        /// </summary>
        /// <param name="color">The color of the particle.</param>
        /// <param name="transparency">The transparency of the particle.</param>
        /// <param name="tTL">The time to live.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="spinVelocity">The spin velocity.</param>
        /// <param name="velocity">The velocity.</param>
        /// <param name="position">The psoition.</param>
        /// <param name="system">The particle system.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="scaleVelocity">The scale velocity.</param>
        /// <param name="texture">The texture.</param>
        /// <param name="origin">The origin.</param>
        /// <param name="animation">The animation to use.</param>
        public Particle(Color color, float tTL, float rotation, float spinVelocity, Vector2 velocity, Vector2 position, ParticleSystem system, Vector2 scale, Vector2 scaleVelocity, Texture2D texture, Vector2 origin, SpriteAnimation animation = null, float transparency = 256) : this()
        {
            Color = color;
            // will normalize for us
            Transparency = transparency;
            TTL = tTL;
            Rotation = rotation;
            SpinVelocity = spinVelocity;
            Velocity = velocity;
            Position = position;
            System = system;
            Scale = scale;
            ScaleVelocity = scaleVelocity;
            Texture = texture;
            Origin = origin;
            Animation = animation;
        }
    }
}
