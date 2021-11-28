/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O.H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/USED_CODE_LICENSES.txt for more info.
 */
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// If an entity wants to draw, it needs this class.
    /// </summary>
    public interface IDrawable
    {

        /// <summary>
        /// The render layer.
        /// </summary>
        RenderLayer RenderLayer { get; set; }

        /// <summary>
        /// The order for rendering. Lower number renders first.
        /// </summary>
        int RenderOrder { get; set; }

        void Draw();

        void DebugDraw(bool editorRender);

    }
}
