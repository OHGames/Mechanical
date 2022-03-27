
/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O. H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/Use%20Code%20Licenses.md for more info.
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// A GUI Element is anything that will be updated by the <see cref="GUICanvas"/>.
    /// </summary>
    [DataContract]
    public class GUIElement : IParentChildHierarchy<GUIElement>
    {
        #region Variables
        /// <summary>
        /// The canvas that the element is tied to.
        /// </summary>
        [DataMember]
        public GUICanvas Canvas { get; set; }

        /// <summary>
        /// The parent.
        /// </summary>
        [DataMember]
        private GUIElement parent;

        /// <summary>
        /// The name of the element.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        public GUIElement Parent { get => parent; set => SetParent(value); }

        [DataMember]
        public List<GUIElement> Children { get; set; } = new List<GUIElement>();

        public string HierarchyPath => HasParent ? $"{Parent.HierarchyPath}/{Name}" : Name;

        public bool HasParent => Parent != null;

        /// <summary>
        /// If the mouse in in the <see cref="Bounds"/>.
        /// </summary>
        public bool IsMouseIn { get; private set; }

        #region Transform

        /// <summary>
        /// The position of the element.
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
        /// The position.
        /// </summary>
        [DataMember]
        private Vector2 position;

        /// <summary>
        /// The local position of element.
        /// </summary>
        [DataMember]
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
        /// The rotation of the element.
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
        /// The local rotation of the element.
        /// </summary>
        [DataMember]
        public float LocalRotation { get; set; }

        /// <summary>
        /// The scale of the element.
        /// </summary>
        public Vector2 Scale
        {
            get
            {
                Vector2 relation = HasParent ? Parent.Scale : Vector2.One;
                return LocalScale * relation;
            }
        }

        /// <summary>
        /// The local scale of the element.
        /// </summary>
        [DataMember]
        public Vector2 LocalScale { get; set; }

        /// <summary>
        /// The origin of the element.
        /// </summary>
        [DataMember]
        public Vector2 Origin { get; set; } = Vector2.Zero;

        /// <summary>
        /// The bounds of the element.
        /// </summary>
        [DataMember]
        public Rectangle Bounds { get; set; } = Rectangle.Empty;

        #endregion

        #region Renderer

        /// <summary>
        /// The order in which the element will draw.
        /// </summary>
        [DataMember]
        public int RenderOrder { get; set; }

        /// <summary>
        /// The transparency of the element.
        /// </summary>
        [DataMember]
        public float Transparency { get; set; }

        /// <summary>
        /// The tint of the element.
        /// </summary>
        [DataMember]
        public Color Color { get; set; }

        #endregion

        #endregion

        public GUIElement(GUICanvas canvas, string name)
        {
            Canvas = canvas;
            Name = name;
        }

        public virtual void Initialize()
        {

        }

        public virtual void LoadContent(ContentManager content)
        {

        }

        public virtual void Update(float deltaTime)
        {

        }

        public virtual void Draw()
        {

        }

        public virtual void DebugDraw(bool editor)
        {

        }

        /// <summary>
        /// When the element is added to a canvas.
        /// </summary>
        public virtual void OnAdded()
        {

        }

        /// <summary>
        /// When the element is removed from the canvas.
        /// </summary>
        public virtual void OnRemoved()
        {

        }

        /// <summary>
        /// This happens after all the elements have been added.
        /// </summary>
        public virtual void Awake()
        {

        }

        /// <summary>
        /// Makes a copy of this element.
        /// </summary>
        /// <returns>A copy of the element.</returns>
        public virtual GUIElement Clone()
        {
            GUIElement e = new GUIElement(Canvas, Name + " Copy")
            {
                Bounds = Bounds,
                Children = Children,
                LocalPosition = LocalPosition,
                Position = Position,
                LocalRotation = LocalRotation,
                LocalScale = LocalScale,
                Origin = Origin
            };
            return e;
        }

        /// <summary>
        /// When the mouse clicks on the element.
        /// </summary>
        public virtual void OnMouseClick()
        {

        }

        /// <summary>
        /// When the mouse enters the element's <see cref="Bounds"/>.
        /// </summary>
        public virtual void OnMouseEnter()
        {
            IsMouseIn = true;
        }

        /// <summary>
        /// When the mnouse leaves the element's <see cref="Bounds"/>.
        /// </summary>
        public virtual void OnMouseExit()
        {
            IsMouseIn = false;
        }

        #region Parenting

        public void AddChild(GUIElement child)
        {
            if (!IsParentOf(child))
            {
                Children.Add(child);
                child.Parent = this;
                // call event
                child.OnParentAdded(this);
            }
        }

        public GUIElement GetAncestor()
        {
            GUIElement currentChild = this;
            while (currentChild.HasParent)
            {
                currentChild = currentChild.Parent;
            }
            return currentChild;
        }

        public bool IsParentOf(GUIElement child)
        {
            return Children.Contains(child);
        }

        public void RemoveChild(GUIElement child)
        {
            if (Children.Contains(child))
            {
                Children.Remove(child);
                child.SetParent(null);
                child.OnParentRemoved(this);
            }
        }

        public void SetParent(GUIElement parent)
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


        public virtual void OnParentRemoved(GUIElement parent)
        {

        }

        public virtual void OnParentAdded(GUIElement parent)
        {

        }

        #endregion
    }
}