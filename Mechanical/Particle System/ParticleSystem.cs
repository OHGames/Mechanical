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
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// The particle system updates the particles.
    /// </summary>
    public class ParticleSystem : IGameFunctions
    {

        #region Variables
        /// <summary>
        /// The particles in the system.
        /// </summary>
        public Particle[] Particles { get; set; }

        /// <summary>
        /// The max amount of particles.
        /// </summary>
        public uint Capacity { get; set; }

        /// <summary>
        /// The spawner used to spawn particles.
        /// </summary>
        public IParticleSpawner ParticleSpawner { get; set; }

        /// <summary>
        /// The position of the system.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The texture of the particles.
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// The force of gravity that will be applied.
        /// </summary>
        public Vector2 Gravity { get; set; } = Vector2.Zero;

        /// <summary>
        /// The blend state to use when drawing the particles. Defaults to <see cref="BlendState.Additive"/>.
        /// </summary>
        public BlendState BlendState { get; set; } = BlendState.AlphaBlend;

        /// <summary>
        /// A reference to the spritebatch.
        /// </summary>
        private SpriteBatch spriteBatch;

        /// <summary>
        /// If the system is emitting particles.
        /// </summary>
        public bool IsEmitting { get; set; }

        /// <summary>
        /// How long the system should emit paticles for.
        /// </summary>
        private float emitFor;

        /// <summary>
        /// How many particles to emit per frame.
        /// </summary>
        public float PPF { get; set; }

        /// <summary>
        /// How many particles are active.
        /// </summary>
        public int ActiveParticles { get; set; }

        /// <summary>
        /// If the system is paused. When the system is paused, the time to emit will not be decremented.
        /// </summary>
        public bool Paused { get; set; }

        /// <summary>
        /// If the particles are animated.
        /// </summary>
        public bool AnimatedParticles { get; set; }

        /// <summary>
        /// The animations to give the particles when <see cref="AnimatedParticles"/> is <c>true</c>.
        /// </summary>
        public WeightedList<SpriteAnimation> Animations { get; set; } = new WeightedList<SpriteAnimation>();

        /// <regionsummary>
        /// The variables here will be changed on a particle when it is emitted.
        /// </regionsummary>
        #region Particle Values And Variation

        #region Start Speed
        /// <summary>
        /// The staring speed of the particles. When <see cref="StartSpeedVariability"/> is not 0, this will act as the Min in random calculations.
        /// </summary>
        public float StartParticleSpeed { get; set; }

        /// <summary>
        /// How much the particles' starting speed should vary. This will be added onto <see cref="StartParticleSpeed"/> and act as the Max in random calculations. To not vary the speed, set to 0.
        /// </summary>
        public float StartSpeedVariability { get; set; }
        #endregion

        #region End Speed

        /// <summary>
        /// The ending speed of the particles. When <see cref="EndSpeedVariability"/> is not 0, this will act as the Min in random calculations.
        /// </summary>
        public float EndParticleSpeed { get; set; }

        /// <summary>
        /// How much the particles' end speed should vary. This will be added onto <see cref="EndParticleSpeed"/> and act as the Max in random calculations. To not vary the speed, set to 0.
        /// </summary>
        public float EndSpeedVariability { get; set; }

        #endregion

        #region Angle
        /// <summary>
        /// The starting angle of a particles. When <see cref="AngleVariablility"/> is not 0, this will act as the Min in random calculations.
        /// </summary>
        public float ParticleAngle { get; set; }

        /// <summary>
        /// How much the particles' angle should vary. This will be added onto <see cref="ParticleAngle"/> and act as the Max in random calculations. To not vary the angle, set to 0.
        /// </summary>
        public float AngleVariablility { get; set; }
        #endregion

        #region Life

        /// <summary>
        /// The starting life of the particles. When <see cref="LifeVariability"/> is not 0, this will act as the Min in random calculations.
        /// </summary>
        public float ParticleLife { get; set; }

        /// <summary>
        /// How much the particles' life should vary. This will be added onto <see cref="ParticleLife"/> and act as the Max in random calculations. To not vary the life, set to 0.
        /// </summary>
        public float LifeVariability { get; set; }
        #endregion

        #region Start Color
        /// <summary>
        /// The start color of the particles. When any of the RGBA values of the <see cref="StartColorVariability"/> is not 0, they will act as the Min in random calculations.
        /// </summary>
        /// <remarks>
        /// An example:
        /// <code>
        /// StartColor = new Color(10, 10, 10, 255);
        /// StartColorVariability = new Color(0, 1, 0, 0);
        /// 
        /// StartColor = new Color(rand(r, r + var.R), rand(g, g + var.G), rand(b, r + var.B), rand(A, a + var.A)); // psuedo-code but you get the idea.
        /// 
        /// // Final result:
        /// StartColor = new Color(10, 11, 10, 255);
        /// </code>
        /// </remarks>
        public Color StartColor { get; set; }

        /// <summary>
        /// How much the particles' start color should vary. This will be added onto the RGBA values of <see cref="StartColor"/> and act as the Max in random calculations. To not vary the color, set values to 0.
        /// </summary>
        /// <remarks>
        /// An example:
        /// <code>
        /// StartColor = new Color(10, 10, 10, 255);
        /// StartColorVariability = new Color(0, 1, 0, 0);
        /// 
        /// StartColor = new Color(rand(r, r + var.R), rand(g, g + var.G), rand(b, r + var.B), rand(A, a + var.A)); // psuedo-code but you get the idea.
        /// 
        /// // Final result:
        /// StartColor = new Color(10, 11, 10, 255);
        /// </code>
        /// </remarks>
        public Color StartColorVariability { get; set; }
        #endregion

        #region End Color

        /// <summary>
        /// The end color of the particles. When any of the RGBA values of the <see cref="EndColorVariability"/> is not 0, they will act as the Min in random calculations.
        /// </summary>
        /// <remarks>
        /// See <see cref="StartColor"/> or <see cref="StartColorVariability"/> for an example.
        /// </remarks>
        public Color EndColor { get; set; }

        /// <summary>
        /// How much the particles' end color should vary. This will be added onto the RGBA values of <see cref="EndColor"/> and act as the Max in random calculations. To not vary the color, set values to 0.
        /// </summary>
        /// <remarks>
        /// See <see cref="StartColor"/> or <see cref="StartColorVariability"/> for an example.
        /// </remarks>
        public Color EndColorVariability { get; set; }

        #endregion

        #region Start Size

        /// <summary>
        /// The starting size of the particles. When any of the X or Y values of the <see cref="StartSizeVariability"/> is not 0, they will act as the Min in random calculations.
        /// </summary>
        /// <remarks>
        /// See <see cref="StartColor"/> or <see cref="StartColorVariability"/> for an example on variability.
        /// </remarks>
        public Vector2 StartSize { get; set; } = Vector2.One;

        /// <summary>
        /// How much the particles' start size should vary. This will be added onto the X and Y values of <see cref="StartSize"/> and act as the Max in random calculations. To not vary the size, set values to 0.
        /// </summary>
        /// <remarks>
        /// See <see cref="StartColor"/> or <see cref="StartColorVariability"/> for an example on variability.
        /// </remarks>
        public Vector2 StartSizeVariability { get; set; }

        #endregion

        #region End Size

        /// <summary>
        /// The ending size of the particles. When any of the X or Y values of the <see cref="EndSizeVariability"/> is not 0, they will act as the Min in random calculations.
        /// </summary>
        /// <remarks>
        /// See <see cref="StartColor"/> or <see cref="StartColorVariability"/> for an example on variability.
        /// </remarks>
        public Vector2 EndSize { get; set; } = Vector2.One;

        /// <summary>
        /// How much the particles' end size should vary. This will be added onto the X and Y values of <see cref="EndSize"/> and act as the Max in random calculations. To not vary the size, set values to 0.
        /// </summary>
        /// <remarks>
        /// See <see cref="StartColor"/> or <see cref="StartColorVariability"/> for an example on variability.
        /// </remarks>
        public Vector2 EndSizeVariability { get; set; }
        #endregion

        #region Start Rotation
        /// <summary>
        /// The starting rotation of a particles. When <see cref="StartRotationVariability"/> is not 0, this will act as the Min in random calculations.
        /// </summary>
        public float StartParticleRotation { get; set; }

        /// <summary>
        /// How much the particles' rotation should vary. This will be added onto <see cref="StartParticleRotation"/> and act as the Max in random calculations. To not vary the rotation, set to 0.
        /// </summary>
        public float StartRotationVariability { get; set; }
        #endregion

        #region End Rotation
        /// <summary>
        /// The ending rotation of a particles. When <see cref="EndRotationVariability"/> is not 0, this will act as the Min in random calculations.
        /// </summary>
        public float EndParticleRotation { get; set; }

        /// <summary>
        /// How much the particles' end rotation should vary. This will be added onto <see cref="EndParticleRotation"/> and act as the Max in random calculations. To not vary the rotation, set to 0.
        /// </summary>
        public float EndRotationVariability { get; set; }
        #endregion

        #region Start Opacity
        /// <summary>
        /// The starting opacity of a particles. When <see cref="StartOpacityVariability"/> is not 0, this will act as the Min in random calculations.
        /// </summary>
        public float StartParticleOpacity { get; set; } = 255;

        /// <summary>
        /// How much the particles' opacity should vary. This will be added onto <see cref="StartParticleOpacity"/> and act as the Max in random calculations. To not vary the opacity, set to 0.
        /// </summary>
        public float StartOpacityVariability { get; set; }
        #endregion

        #region End Opacity

        /// <summary>
        /// The ending opacity of a particles. When <see cref="EndOpacityVariability"/> is not 0, this will act as the Min in random calculations.
        /// </summary>
        public float EndParticleOpacity { get; set; } = 255;

        /// <summary>
        /// How much the particles' ending opacity should vary. This will be added onto <see cref="EndParticleOpacity"/> and act as the Max in random calculations. To not vary the opacity, set to 0.
        /// </summary>
        public float EndOpacityVariability { get; set; }

        #endregion

        // TODO: add more variations and modifiers

        #endregion
        

        #endregion

        /// <summary>
        /// Make a new particle spawner.
        /// </summary>
        /// <param name="ppf">The amount of particle emitted per frame</param>
        /// <param name="spawner">The spawner to use, if set to null it will use a <see cref="BasicParticleSpawner"/>.</param>
        /// <param name="position">The position of the system.</param>
        /// <param name="texture">The texture to use.</param>
        public ParticleSystem(int ppf, IParticleSpawner spawner, Vector2 position, Texture2D texture)
        {
            PPF = ppf;

            Position = position;
            Texture = texture;

            if (spawner == null)
            {
                ParticleSpawner = new BasicParticleSpawner();
            }
            else
            {
                ParticleSpawner = spawner;
            }


            ParticleSpawner.SystemPosition = Position;
        }

        public virtual void Initialize()
        {
            // get the max life
            float life = ParticleLife + LifeVariability;
            // make the capacity so that it will never run out.
            Capacity = (uint)((PPF * (life + 1)));

            Particles = new Particle[Capacity];
            
            for (int i = 0; i < Capacity; i++)
            {
                // set all particles to dead.
                Particles[i] = new Particle(this) { IsAlive = false };
            }
        }

        public virtual void LoadContent(ContentManager content)
        {
            spriteBatch = Drawing.SpriteBatch;
        }

        public virtual void Update(float deltaTime)
        {
            ParticleSpawner.SystemPosition = Position;

            if (Paused) return;

            if (IsEmitting)
            {
                // sort the particles so that the active ones come first.
                Particles = Particles.OrderBy(p => !p.IsAlive).ToArray();

                // for each particle to emit this frame.
                for (int i = 0; i < PPF; i++)
                {
                    // stop from overflowing.
                    if (ActiveParticles < Capacity)
                    {
                        Particle particle = Particles[ActiveParticles];
                        Particles[ActiveParticles] = GenerateParticle(particle);
                        ActiveParticles++;
                    }
                }

            }

            if (ActiveParticles > 0)
            {
                // update particles.
                for (int i = 0; i < Particles.Length; i++)
                {
                    if (Particles[i].IsAlive)
                    {
                        Particles[i].Update(deltaTime);

                        // kill if alive to long.
                        if (Particles[i].TimeAlive >= Particles[i].Life)
                        {
                            Particles[i].IsAlive = false;
                            ActiveParticles--;
                        }
                    }
                }
            }

            // only decrease life of emitter when not infinate.
            if (emitFor != -1)
            {
                emitFor -= deltaTime;
                if (emitFor <= 0)
                {
                    IsEmitting = false;
                    emitFor = 0;
                }
            }
        }

        public virtual void Draw()
        {
            // get starting blend state
            var blend = spriteBatch.GraphicsDevice.BlendState;
            // if it is different, restart the batch.
            if (BlendState != spriteBatch.GraphicsDevice.BlendState)
            {
                //spriteBatch.GraphicsDevice.BlendState = BlendState;
                spriteBatch.End();
                spriteBatch.Begin(blendState: BlendState, samplerState: SamplerState.PointClamp, transformMatrix: Engine.Instance.Camera.TransformationMatrix);
            }

            for (int i = 0; i < Particles.Length; i++)
            {
                // render active particles.
                Particle p = Particles[i];
                if (p.IsAlive)
                {
                    // change origin based on texture.
                    var origin = p.SourceRectangle == null 
                        ? new Vector2(p.Texture.Width / 2, p.Texture.Height / 2) 
                        : new Vector2((float)(p.SourceRectangle?.Width / 2), (float)(p.SourceRectangle?.Height / 2));

                    Drawing.Draw(p.Texture, 
                                 p.Position, 
                                 p.SourceRectangle, 
                                 p.Color * p.Opacity, 
                                 p.Rotation, origin, 
                                 p.Size, 
                                 SpriteEffects.None, 0);
                }
            }

            // if we need to change back.
            if (blend != spriteBatch.GraphicsDevice.BlendState)
            {
                // spriteBatch.GraphicsDevice.BlendState = blend;
                spriteBatch.End();
                Engine.Instance.DefaultBeginBatch();
            }

        }

        public virtual void DebugDraw(bool editorRender)
        {
            for (int i = 0; i < Particles.Length; i++)
            {
                // draws a line that represents its velocity vector.
                Drawing.DrawLine(Particles[i].Position, Particles[i].Position + Particles[i].Velocity, Color.Red, 2);
            }
            Draw();
        }

        /// <summary>
        /// Start emitting particles.
        /// </summary>
        /// <param name="seconds">How many seconds to emit particles. Set to -1 so emit infinatly.</param>
        public void Emit(float seconds = -1)
        {
            IsEmitting = true;
            emitFor = seconds;
        }

        /// <summary>
        /// Get the varied value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="vary">The variability.</param>
        /// <returns></returns>
        public float GetVariedValue(float value, float vary)
        {
            if (vary == 0) return value;
            else return new Random().Next((int)value, (int)(value + vary));
        }

        /// <summary>
        /// Generates a new particle based on a dead particle.
        /// </summary>
        /// <param name="particle">The dead particle</param>
        /// <returns>A "new" particle</returns>
        public Particle GenerateParticle(Particle particle)
        {
            particle.IsAlive = true;
            particle.TimeAlive = 0;
            particle.Position = ParticleSpawner.Spawn();

            particle.Angle = GetVariedValue(ParticleAngle, AngleVariablility);
            particle.Life = GetVariedValue(ParticleLife, LifeVariability);

            var rotation = GetVariedValue(StartParticleRotation, StartRotationVariability);
            particle.StartRotation = rotation;
            particle.Rotation = rotation;
            particle.EndRotation = GetVariedValue(EndParticleRotation, EndRotationVariability);

            var speed = GetVariedValue(StartParticleSpeed, StartSpeedVariability);
            particle.StartSpeed = speed;
            particle.Speed = speed;
            particle.EndSpeed = GetVariedValue(EndParticleSpeed, EndSpeedVariability);

            particle.Texture = Texture;

            var opacity = GetVariedValue(StartParticleOpacity, StartOpacityVariability);
            particle.StartOpacity = opacity;
            particle.Opacity = opacity;
            particle.EndOpacity = GetVariedValue(EndParticleOpacity, EndOpacityVariability);


            var color = new Color(
                   (int)GetVariedValue(StartColor.R, StartColorVariability.R),
                   (int)GetVariedValue(StartColor.G, StartColorVariability.G),
                   (int)GetVariedValue(StartColor.B, StartColorVariability.B)
                );
            particle.Color = color;
            particle.StartColor = color;
            particle.EndColor =
                new Color(
                    (int)GetVariedValue(EndColor.R, EndColorVariability.R),
                    (int)GetVariedValue(EndColor.G, EndColorVariability.G),
                    (int)GetVariedValue(EndColor.B, EndColorVariability.B)
                );


            var size = new Vector2(
                    GetVariedValue(StartSize.X, StartSizeVariability.X),
                    GetVariedValue(StartSize.Y, StartSizeVariability.Y)
                );
            particle.Size = size;
            particle.StartSize = size;
            particle.EndSize =
                new Vector2(
                    GetVariedValue(EndSize.X, EndSizeVariability.X),
                    GetVariedValue(EndSize.Y, EndSizeVariability.Y)
                );

            if (AnimatedParticles)
            {
                particle.Animation = Animations.Get();
                particle.SourceRectangle = particle.Animation.GetCurrentRectangle();
                particle.Animation.Play();
            }

            return particle;
        }

        /// <summary>
        /// Toggle the system.
        /// </summary>
        public void TogglePause()
        {
            Paused = !Paused;
        }

        /// <summary>
        /// Set the system to be paused or unpaused.
        /// </summary>
        /// <param name="pause">If the system should be paused or not.</param>
        public void Pause(bool pause)
        {
            Paused = pause;
        }

    }
}
