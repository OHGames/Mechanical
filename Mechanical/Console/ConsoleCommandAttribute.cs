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
    /// This attribute will allow a function to be called when a console command is run. The command function must have a single paremeter of a <see cref="string"/>[]
    /// <para>
    /// For aliases, put the attribute on again.
    /// </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ConsoleCommandAttribute : Attribute
    {

        /// <summary>
        /// The name of the command.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The text that will show up when the help command is ran.
        /// </summary>
        public string Usage { get; set; }

        /// <summary>
        /// Make a new console command.
        /// </summary>
        /// <param name="name">The name of the command.</param>
        /// <param name="usage">How the command will be used.</param>
        public ConsoleCommandAttribute(string name, string usage)
        {
            Name = name;
            Usage = usage;
        }
    }
}
