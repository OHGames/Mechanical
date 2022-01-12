/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O. H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/USED_CODE_LICENSES.txt for more info.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// The layer to render an entity. There are 3 layers, background, midground, and foreground.
    /// </summary>
    public enum RenderLayer
    {

        Background = 0,

        Midground = 1,

        Foreground = 2

    }
}
