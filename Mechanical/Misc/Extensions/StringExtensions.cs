/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O. H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/Use%20Code%20Licenses.md for more info.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// Extensions for <see cref="string"/>s.
    /// </summary>
    public static class StringExtensions
    {

        /// <summary>
        /// Split a string by arguments. This will remove the quotes.
        /// </summary>
        /// <param name="arg">The arguments.</param>
        /// <returns>A string split by arguments. This will remove the quotes.</returns>
        public static string[] SplitArgs(this string arg)
        {
            // https://stackoverflow.com/a/29899 help
            List<string> args = new List<string>();

            bool inQuotes = false;
            string currentArg = "";
            for (int i = 0; i < arg.Length; i++)
            {
                char c = arg[i];
                if (!inQuotes && (c == ' ' || i >= arg.Length - 1))
                {
                    if (i >= arg.Length - 1) currentArg += c;
                    args.Add(currentArg);
                    currentArg = "";
                }
                else if (!inQuotes && (c == '\"' || c == '\''))
                {
                    inQuotes = true;
                }
                else if (inQuotes && (c == '\"' || c == '\''))
                {
                    inQuotes = false;
                }
                else
                {
                    currentArg += c;
                }
            }
            return args.ToArray();
        }

    }
}
