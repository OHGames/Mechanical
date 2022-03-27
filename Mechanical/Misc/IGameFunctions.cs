/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O. H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/Use%20Code%20Licenses.md for more info.
 */

using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// This interface comes with functions that are like the <see cref="Microsoft.Xna.Framework.Game"/>
    /// </summary>
    public interface IGameFunctions
    {

        /// <summary>
        /// When the game starts, initailize is called.
        /// </summary>
        void Initialize();

        /// <summary>
        /// When content needs to be loaded, use this function.
        /// </summary>
        /// <param name="content">The content manager.</param>
        void LoadContent(ContentManager content);

        /// <summary>
        /// Every frame, this function is called.
        /// </summary>
        /// <param name="deltaTime">How much time has passed since last frame. This value is modified.</param>
        void Update(float deltaTime);

        /// <summary>
        /// When something needs to be drawn, use this function. Use alongside <see cref="Drawing"/>.
        /// </summary>
        void Draw();

        /// <summary>
        /// What to draw when in debug mode.
        /// </summary>
        /// <param name="editorRender">If the debug is the editor.</param>
        void DebugDraw(bool editorRender);
    }
}
