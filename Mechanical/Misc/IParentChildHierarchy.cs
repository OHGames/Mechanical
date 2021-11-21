using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// A class used to implement parent-child relationships.
    /// </summary>
    /// <typeparam name="T">The type of child and parent.</typeparam>
    public interface IParentChildHierarchy<T>
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
        /// Set the parent.
        /// </summary>
        /// <param name="parent">The parent to set.</param>
        void SetParent(T parent);

    }
}
