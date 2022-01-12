/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O. H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/USED_CODE_LICENSES.txt for more info.
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// A basic image element. It just has a sprite component.
    /// </summary>
    public class GUIImage : GUIElement
    {

        /// <summary>
        /// The texture to render.
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// The rectangle of the texture 
        /// </summary>
        public Rectangle? SourceRectangle { get; set; }

        /// <summary>
        /// The effect to draw with.
        /// </summary>
        public Effect Effect { get; set; }

        /// <summary>
        /// The sprite effects to use.
        /// </summary>
        public SpriteEffects Effects { get; set; }

        public GUIImage(GUICanvas canvas, string name, Texture2D texture, Rectangle? sourceRect = null, Effect effect = null, SpriteEffects spriteEffects = SpriteEffects.None) : base(canvas, name)
        {

            Texture = texture;
            SourceRectangle = sourceRect;
            Effect = effect;
            Effects = spriteEffects;

        }

        public override void Draw()
        {
            Drawing.Draw(Texture, Bounds, SourceRectangle, Color, Rotation, Origin, Effects, 0, Effect);
        }

    }
}
