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
    /// Put this attribute on a function so that a button appers next to the variable value to run that function. This is like a command.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class EditorFunctionAttribute : Attribute
    {

        /// <summary>
        /// The name of the variable that the button will be applied too.
        /// </summary>
        public string VariableName { get; private set; }

        /// <summary>
        /// The name of the class that the button will be applied to.
        /// </summary>
        public string ClassName { get; private set; }

        /// <summary>
        /// If the function will appear next to a variable of a certain type (recommended).
        /// </summary>
        public bool UsingType { get; private set; } = true;

        /// <summary>
        /// The type that this will be affiliated with.
        /// </summary>
        public Type Type { get; private set; }

        /// <summary>
        /// Apply a function to a specific variable name and class.
        /// </summary>
        /// <param name="name">The name of the variable to use. Set to * to apply to all variables, and type:(typehere) to apply to a specific type on a class.</param>
        /// <param name="className">The name of the class.</param>
        public EditorFunctionAttribute(string name, string className)
        {
            VariableName = name;
            ClassName = className;
            UsingType = false;
        }

        /// <summary>
        /// The type to apply to.
        /// </summary>
        /// <param name="type">The type.</param>
        public EditorFunctionAttribute(Type type)
        {
            Type = type;
            UsingType = true;
        }

    }
}
