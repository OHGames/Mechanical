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
        public Vector2 Position 
        {
            get => position; 
            set
            {
                Vector2 delta = value - position;
                for (int i = 0; i < Children.Count; i++)
                {
                    Children[i].Position += delta;
                }
                position = value;
            }
        }

        /// <summary>
        /// The position, but private.
        /// </summary>
        private Vector2 position;

        /// <summary>
        /// The scale of the entity.
        /// </summary>
        public Vector2 Scale
        {
            get
            {
                Vector2 relation = HasParent ? Parent.Scale : Vector2.Zero;
                return LocalScale * relation;
            }
        }

        /// <summary>
        /// The rotation.
        /// </summary>
        public float Rotation
        {
            get
            {
                float relation = HasParent ? Parent.Rotation : 0;
                return LocalRotation + relation;
            }
        }

        /// <summary>
        /// The local position of the tranform.
        /// </summary>
        public Vector2 LocalPosition 
        {
            get
            {
                Vector2 relaton = HasParent ? Parent.LocalPosition : Vector2.Zero;
                return Position - relaton;
            }
            set 
            {
                Vector2 relaton = HasParent ? Parent.LocalPosition : Vector2.Zero;
                Position = value + relaton;
            } 
        }

        /// <summary>
        /// The scale relative to the parent.
        /// </summary>
        public Vector2 LocalScale { get; set; }

        /// <summary>
        /// The rotation relative to the parent.
        /// </summary>
        public float LocalRotation { get; set; }

        /// <summary>
        /// The point to render the entity at.
        /// </summary>
        public Vector2 Origin { get; set; }

        /// <summary>
        /// The bounds of the transform.
        /// </summary>
        public Rectangle Bounds { get; set; }

        /// <summary>
        /// The private version of the parent.
        /// </summary>
        private Transform parent;
        
        public Transform Parent { get => parent; set => SetParent(value); }

        public List<Transform> Children { get; set; } = new List<Transform>();

        public string HierarchyPath { get => Parent == null ? $"{Attached.Name}.Tranform" : $"{Parent.HierarchyPath}/{Attached.Name}.Tranform"; }

        public bool HasParent => Parent != null;

        public Transform(Entity e, Vector2 position) : base(e)
        {
            Position = position;
            CanBeRemoved = false;
            AllowMultiple = false;
        }

        public Transform(Entity e) : base(e) 
        {
            AllowMultiple = false;
            CanBeRemoved = false;
        }

        /// <summary>
        /// Move the position by the amount.
        /// </summary>
        /// <param name="amount">The amount to translate by.</param>
        public void Translate(Vector2 amount)
        {
            Position += amount;
        }

        /// <summary>
        /// Move the local position by the amount.
        /// </summary>
        /// <param name="amount">The amount to translate by.</param>
        public void TranslateLocally(Vector2 amount) => LocalPosition += amount;

        /// <summary>
        /// Change the scale by the amount.
        /// </summary>
        /// <param name="amount"></param>
        public void ScaleBy(Vector2 amount) => LocalScale += amount;

        /// <summary>
        /// Change the rotation by an amount.
        /// </summary>
        /// <param name="amount"></param>
        public void RotateBy(float amount) => LocalRotation += amount;

        #region Parenting

        public void AddChild(Transform child)
        {
            if (!IsParentOf(child))
            {
                Children.Add(child);
                // dont call SetParent function to avoid loops.
                child.Parent = this;
            }
        }

        public void RemoveChild(Transform child)
        {
            if (Children.Contains(child))
            {
                Children.Remove(child);
                child.SetParent(null);
            }
        }

        /// <summary>
        /// Set the parent. Set to null to remove parent.
        /// </summary>
        /// <param name="parent">The parent to set.</param>
        private void SetParent(Transform parent)
        {
            if (HasParent)
            {
                // remove this component has the child.
                this.parent.RemoveChild(this);
            }

            // set this as child of new parent.
            parent?.AddChild(this);

            // add new parent.
            this.parent = parent;
        }

        public bool IsParentOf(Transform child)
        {
            return Children.Contains(child);
        }

        public Transform GetAncestor()
        {
            Transform currentChild = this;
            while (currentChild.HasParent)
            {
                currentChild = currentChild.Parent;
            }
            return currentChild;
        }

        #endregion
    }
}
