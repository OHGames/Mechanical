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
using System.Runtime.Serialization;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// Represents a circle.
    /// </summary>
    [DataContract]
    public struct Circle
    {

        /// <summary>
        /// An empty circle.
        /// </summary>
        public static readonly Circle Empty = new Circle(Vector2.Zero, 0);

        /// <summary>
        /// The position of the circle. (It's center)
        /// </summary>
        [DataMember]
        public Vector2 Center { get; set; }

        /// <summary>
        /// The radius of the circle.
        /// </summary>
        [DataMember]
        public float Radius
        {
            get => radius;
            set
            {
                radius = value;
                diameter = value * 2;
            }
        }

        /// <summary>
        /// The radius of the circle.
        /// </summary>
        private float radius;

        /// <summary>
        /// The diameter.
        /// </summary>
        private float diameter;

        /// <summary>
        /// The diameter.
        /// </summary>
        [DataMember]
        public float Diameter
        {
            get { return diameter; }
            set 
            { 
                diameter = value;
                radius = value / 2;
            }
        }

        /// <summary>
        /// The circumfurence of the circle.
        /// </summary>
        public float Circumfurence
        {
            get
            {
                // 2 * PI * radius
                return MathHelper.TwoPi * Radius;
            }
        }

        public Circle(Vector2 center, float radius) : this()
        {
            Center = center;
            Radius = radius;
        }

        public static bool operator ==(Circle a, Circle b)
        {
            return a.Center == b.Center
                // check diameter because the radius will be the same.
                && a.Diameter == b.Diameter;
        }

        public static bool operator !=(Circle a, Circle b)
        {
            return !(a == b);
        }

    }
}