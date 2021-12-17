/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O.H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/USED_CODE_LICENSES.txt for more info.
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// A GUI Element is anything that will be updated by the <see cref="GUICanvas"/>.
    /// </summary>
    public class GUIElement : Entity
    {
        /// <summary>
        /// The canvas that the element is tied to.
        /// </summary>
        public GUICanvas Canvas { get; }

        /// <summary>
        /// If the mouse in in the <see cref="Bounds"/>.
        /// </summary>
        public bool IsMouseIn { get; private set; }

        /// <summary>
        /// The transparency of the element.
        /// </summary>
        public float Transparency { get; set; }


        public GUIElement(string name, GUICanvas canvas, Scene scene) : base(name, scene)
        {
        }

        /// <summary>
        /// Makes a copy of this element.
        /// </summary>
        /// <returns>A copy of the element.</returns>
        public override Entity Clone()
        {
            GUIElement e = new GUIElement(Name + " Clone", Canvas, Scene)
            {
                Active = Active,
                Children = new List<Entity>(Children),
                HasDebugDraw = HasDebugDraw,
                ID = IDManager.GetId(),
                IsMouseIn = IsMouseIn,
                Parent = Parent,
                Paused = Paused,
                Tags = Tags,
                Transparency = Transparency,
                Visible = Visible
            };
            return e;
        }

        /// <summary>
        /// When the mouse clicks on the element.
        /// </summary>
        public virtual void OnMouseClick()
        {

        }

        /// <summary>
        /// When the mouse enters the element's <see cref="Bounds"/>.
        /// </summary>
        public virtual void OnMouseEnter()
        {
            IsMouseIn = true;
        }

        /// <summary>
        /// When the mnouse leaves the element's <see cref="Bounds"/>.
        /// </summary>
        public virtual void OnMouseExit()
        {
            IsMouseIn = false;
        }

    }
}
