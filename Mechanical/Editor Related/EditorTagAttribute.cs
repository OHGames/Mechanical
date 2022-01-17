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
    /// The editor tag attribute will set tags to be used when serching for an entity class in the editor.
    /// </summary>
    public class EditorTagAttribute : Attribute
    {
        /// <summary>
        /// The tags.
        /// </summary>
        public string[] Tags { get; private set; }

        public EditorTagAttribute(string[] tags)
        {
            Tags = tags;
        }
    }
}
