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
    /// An enum of different type of message that can be printed to the console.
    /// </summary>
    public enum ConsoleMessageType
    {
        /// <summary>
        /// A message that will be put out to the console. The color of the text will be white.
        /// </summary>
        Message = 1,

        /// <summary>
        /// Anytime there is output to the console from a command, use this type. The color will be blue.
        /// </summary>
        Output = 2,

        /// <summary>
        /// The warning is when there is a warning, something kinda went wrong or almost did. Color of text is yellow.
        /// </summary>
        Warning = 3,

        /// <summary>
        /// The error type if for errors. Color is red.
        /// </summary>
        Error = 4
    }
}
