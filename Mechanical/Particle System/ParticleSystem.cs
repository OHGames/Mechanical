using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// The particle system handles, updates, draws, and keeps track of <see cref="Particle"/>s.
    /// </summary>
    public class ParticleSystem
    {
        #region Variables
        /// <summary>
        /// The particles.
        /// </summary>
        public Particle[] Particles { get; set; }

        /// <summary>
        /// The seed for the particles. Set to -1 for a random seed.
        /// </summary>
        public float Seed { get; set; }

        #region Velocity
        /// <summary>
        /// The velocity of the particles as they are created.
        /// </summary>
        public Vector2 InitialVelocity { get; set; }

        /// <summary>
        /// The minumim and maximim velocities of the particles when emitted and random.
        /// </summary>
        public Vector2[] MinMaxVelocities { get; set; } = new Vector2[2];

        /// <summary>
        /// If the velocities should be randomized when the particle is emitted.
        /// </summary>
        public bool VaryVelocities { get; set; }

        #endregion

        #region Spread
        /// <summary>
        /// The minimum rotation in degrees that the particle will be emited at is the X value and the maximum is the Y.
        /// <para>
        /// Example:
        /// <code>
        /// Spread = new Vector2(0, 180); // emits particles to the right.
        /// Spread = new Vector2(-90, 90); // emits particles upwards.
        /// Spread = new Vector2(0, 360); // emits partices in a circle.
        /// </code>
        /// </para>
        /// </summary>
        public Vector2 Spread { get; set; }

        /// <summary>
        /// If the spread should be randomised.
        /// </summary>
        public bool VarySpread { get; set; }

        /// <summary>
        /// If this variable is true, the spread would not be randomised and it will increment the spread for each particle emitted.
        /// </summary>
        public bool IncrementSpread { get; set; }

        /// <summary>
        /// The current increment of the spread.
        /// </summary>
        protected int spreadIncrement;

        #endregion

        #region Size
        /// <summary>
        /// The starting size of the particles.
        /// </summary>
        public Vector2 InitialSize { get; set; } = Vector2.One;

        /// <summary>
        /// The minimum and maximim sizes a particle can be when emitted and random.
        /// </summary>
        public Vector2[] MinMaxSizes { get; set; } = new Vector2[2];

        // todo: maybe set to a weighted list?

        /// <summary>
        /// If the sizes should be random when the particle is emitted.
        /// </summary>
        public bool VarySizes { get; set; }

        #endregion

        #region Rotation

        /// <summary>
        /// The starting rotation of the particles.
        /// </summary>
        public float InitialRotation { get; set; }

        /// <summary>
        /// The minimum and maximum values for the rotation of a partcle when random and emitted.
        /// </summary>
        public float[] MinMaxRotations { get; set; } = new float[2];

        /// <summary>
        /// If the rotation should be randomised.
        /// </summary>
        public bool VaryRotation { get; set; }

        #endregion

        #region Color

        /// <summary>
        /// The starting color for the particles.
        /// </summary>
        public Color InitialColor { get; set; } = Color.White;

        /// <summary>
        /// The list of starting colors for the particles when random.
        /// </summary>
        public WeightedList<Color> StartingColors { get; set; } = new WeightedList<Color>();

        /// <summary>
        /// Set the particle colors to be random.
        /// </summary>
        public bool VaryColors { get; set; }

        #endregion

        #region Transparency

        /// <summary>
        /// The initail transparency.
        /// </summary>
        public float InitailTransparency { get; set; } = 256;

        /// <summary>
        /// The minimum and maximum transparencies.
        /// </summary>
        public float[] MinMaxTransparencies { get; set; } = new float[2];

        /// <summary>
        /// If the transparency should be random.
        /// </summary>
        public bool VaryTransparencies { get; set; }

        #endregion

        #region TTL

        /// <summary>
        /// The TTL for the particles.
        /// </summary>
        public float TTL { get; set; }

        /// <summary>
        /// The minumum and maximum time to live.
        /// </summary>
        public float[] TTLMinMax { get; set; } = new float[2];

        /// <summary>
        /// If the TTL should be random.
        /// </summary>
        public bool VaryTTL { get; set; }

        #endregion

        #endregion

        /// <summary>
        /// The max amount of particles that this system will update or keep track of. Defaults to 100.
        /// </summary>
        public int MaxCapacity; // https://stackoverflow.com/questions/1433307/speed-of-c-sharp-lists, https://curryncode.com/2016/09/12/c-fast-collections-performance-tips/ idea of capacity.

        public ParticleSystem(int capacity = 100)
        {
            MaxCapacity = capacity;
            Particles = new Particle[capacity];
        }

    }
}
