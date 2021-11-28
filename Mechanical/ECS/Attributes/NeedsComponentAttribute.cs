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
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// Add this attribute to any component that needs another component to work.
    /// 
    /// Until the component(s) that are needed is added, it will not be added.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class NeedsComponentAttribute : Attribute
    {
        public Type[] TypesNeeded { get; set; }

        public NeedsComponentAttribute(params Type[] components)
        {
            TypesNeeded = components;
        }

    }
}
