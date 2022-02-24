/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O. H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/USED_CODE_LICENSES.txt for more info.
 */

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// A simple GUI element that will render text.
    /// </summary>
    [DataContract]
    public class GUIText : GUIElement
    {
        /// <summary>
        /// The text to render.
        /// </summary>
        [DataMember]
        public string Text { get; set; }

        /// <summary>
        /// The font to use.
        /// </summary>
        public SpriteFont Font { get; set; }

        /// <summary>
        /// The effects to apply.
        /// </summary>
        [DataMember]
        public SpriteEffects SpriteEffects { get; set; }

        /// <summary>
        /// The effect to apply.
        /// </summary>
        public Effect Effect { get; set; }

        public GUIText(GUICanvas canvas, string name, string text, SpriteFont font, SpriteEffects effects = SpriteEffects.None, Effect effect = null) : base(canvas, name)
        {

            Text = text;
            Font = font;
            SpriteEffects = effects;
            Effect = effect;

        }

        public override void Draw()
        {
            Drawing.DrawString(Font, Text, Position, Color * Transparency, Rotation, Origin, Scale, SpriteEffects, 0, Effect);
        }

    }
}
