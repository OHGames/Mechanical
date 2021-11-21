using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// The Transform component is required on all entities. It has data on the size, position, and origin of the entity.
    /// </summary>
    public sealed class Transform : Component, IParentChildHierarchy<Transform>
    {

        /// <summary>
        /// The position in world space.
        /// </summary>
        public Vector2 WorldPosition { get; }

        /// <summary>
        /// The private version of the parent.
        /// </summary>
        private Transform parent;

        public Transform Parent { get => parent; set => SetParent(value); }

        public List<Transform> Children { get; set; } = new List<Transform>();

        public void AddChild(Transform child)
        {
            throw new NotImplementedException();
        }

        public void RemoveChild(Transform child)
        {
            throw new NotImplementedException();
        }

        public void SetParent(Transform parent)
        {
            // remove this component has the child.
            parent.RemoveChild(this);
            // add new parent.
            this.parent = parent;

            // set this as child of new parent.
            parent.AddChild(this);
        }
    }
}
