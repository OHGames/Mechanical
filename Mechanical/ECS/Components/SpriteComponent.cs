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
    /// The Sprite component is used to render images on screen.
    /// </summary>
    [DataContract]
    public sealed class SpriteComponent : Component, IDrawableComponent
    {

        //[DataMember]
        /// <summary>
        /// The texture to draw.
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// The color to draw with.
        /// </summary>
        [DataMember]
        public Color RenderColor { get; set; } = Color.White;

        //[DataMember]
        /// <summary>
        /// The size of the image.
        /// </summary>
        public Rectangle DestinationRectangle 
        {
            get
            {
                return destinationRectangle;
            }
            set
            {
                destinationRectangle = value;
            }
        }

        private Rectangle destinationRectangle = Rectangle.Empty;

        /// <summary>
        /// The rectangle that will be used to render a portion of a texture.
        /// </summary>
        [DataMember]
        public Rectangle? SourceRectangle { get; set; } = null;

        /// <summary>
        /// The effect to draw.
        /// </summary>
        public Effect Effect { get; set; } = null;

        /// <summary>
        /// The sprite effects.
        /// </summary>
        [DataMember]
        public SpriteEffects Effects { get; set; } = SpriteEffects.None;

        /// <summary>
        /// The render order of the component.
        /// </summary>
        [DataMember]
        public int RenderOrder { get; set; }

        public SpriteComponent(Entity entity, Texture2D texture) : base(entity)
        {
            Texture = texture;
            AllowMultiple = false;
        }

        public override void Draw()
        {

            Drawing.Draw(Texture, Attached.Transform.Position, SourceRectangle, RenderColor, 
                Attached.Transform.Rotation, Attached.Transform.Origin, 
                Attached.Transform.Scale,
                Effects, 0, Effect);
        }

    }
}
