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
    /// A class used to implement parent-child relationships.
    /// </summary>
    /// <typeparam name="T">The type of child and parent.</typeparam>
    public interface IParentChildHierarchy<T> where T : IParentChildHierarchy<T>
    {

        /// <summary>
        /// The parent.
        /// </summary>
        T Parent { get; set; }

        /// <summary>
        /// The list of children.
        /// </summary>
        List<T> Children { get; set; }

        /// <summary>
        /// A path of the hirearchy.
        /// </summary>
        string HierarchyPath { get; }

        /// <summary>
        /// If the child has a parent.
        /// </summary>
        bool HasParent { get; }

        /// <summary>
        /// Add a child.
        /// </summary>
        /// <param name="child">The child to add</param>
        void AddChild(T child);

        /// <summary>
        /// Remove a child.
        /// </summary>
        /// <param name="child"></param>
        void RemoveChild(T child);

        /// <summary>
        /// Set the parent. Set to null to remove parent.
        /// </summary>
        /// <param name="parent">The parent to set.</param>
        //void SetParent(T parent);

        /// <summary>
        /// If the object is the parent of the child.
        /// </summary>
        /// <param name="child">The child.</param>
        /// <returns>True if the parent's list of children contains the child.</returns>
        bool IsParentOf(T child);

        /// <summary>
        /// Get the top-most parent.
        /// </summary>
        /// <returns>The top-most parent of the hierarchy.</returns>
        T GetAncestor();

    }
}
