/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O.H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/USED_CODE_LICENSES.txt for more info.
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Mechanical
{
    [DataContract]
    /// <summary>
    /// The entity class is anything that can be draw or updated in a <see cref="Scene"/> 
    /// </summary>
    public class Entity : IParentChildHierarchy<Entity>, IEnumerable<Component>
    {
        #region Variables
        [DataMember]
        /// <summary>
        /// The name of the entity.
        /// </summary>
        public string Name { get; set; } = "Entity";

        [DataMember]
        /// <summary>
        /// The static, nonchanging ID of the entity.
        /// 
        /// <para>
        /// This can be used for grabbing an entity from the scene.
        /// </para>
        /// </summary>
        public int ID { get; set; } = IDManager.GetId();

        [DataMember]
        /// <summary>
        /// A list of strings used to group entities.
        /// </summary>
        public List<string> Tags = new List<string>();
        
        [DataMember]
        /// <summary>
        /// The list of components attached to this entity.
        /// </summary>
        public ComponentList Components { get; private set; }

        /// <summary>
        /// A refrence to the Transform component on the entity.
        /// </summary>
        // This is the first index because it should always be the first component. 
        public Transform Transform { get => (Transform)Components[0]; }

        [DataMember]
        /// <summary>
        /// If the entity will update or draw.
        /// </summary>
        public bool Active { get; set; }
        
        [DataMember]
        /// <summary>
        /// If the entity will be drawn.
        /// </summary>
        public bool Visible { get; set; }

        [DataMember]
        /// <summary>
        /// If the entity will update.
        /// </summary>
        public bool Paused
        {
            get; set;
        }

        [DataMember]
        /// <summary>
        /// If the entity has an override for the <see cref="DebugDraw"/> function
        /// </summary>
        public bool HasDebugDraw { get; set; }

        [DataMember]
        /// <summary>
        /// The scene that the entity is in.
        /// </summary>
        public Scene Scene { get; set; }

        [DataMember]
        /// <summary>
        /// The parent, but private.
        /// </summary>
        private Entity parent;

        public Entity Parent { get => parent; set => SetParent(value); }

        public string HierarchyPath => HasParent ? $"{Parent.HierarchyPath}/{Name}" : Name;

        public List<Entity> Children { get; set; } = new List<Entity>();

        public bool HasParent => Parent != null;

        #endregion

        #region Constructors

        public Entity(string name, Scene scene)
        {
            Scene = scene;
            Name = name;
            Components = new ComponentList(this);
            Components.Add(new Transform(this));
        }

        public Entity(string name, Vector2 position, Scene scene) : this(name, scene)
        {
            Transform.Position = position;
        }

        public Entity(string name, string[] tags, Vector2 position, Scene scene) : this(name, position, scene)
        {
            Tags = tags.ToList();
        }

        #endregion

        #region Main Functions

        public virtual void Initalize()
        {
            Components.Initialize();
        }

        public virtual void LoadContent(ContentManager content)
        {
            Components.LoadContent(content);
        }

        public virtual void Update(float deltaTime)
        {
            Components.Update(deltaTime);
        }

        public virtual void Draw()
        {
            Components.Draw();
        }

        public virtual void DebugDraw(bool editorRender)
        {
            if (!HasDebugDraw) Draw();
        }

        /// <summary>
        /// When all entities in the current frame has been added.
        /// </summary>
        public virtual void Awake()
        {
            Components.EntityAwakes();
        }

        /// <summary>
        /// When the entity is added to the scene.
        /// </summary>
        public virtual void OnAdded()
        {
            Components.OnEntityAdded();
        }

        /// <summary>
        /// When the entity is removed from the scene.
        /// </summary>
        public virtual void OnRemoved()
        {
            Components.OnEntityRemoved();
        }

        /// <summary>
        /// Destroy the entity.
        /// </summary>
        public virtual void Destroy()
        {
            Components.OnEntityDestroyed();
        }

        /// <summary>
        /// This will duplicate the entity.
        /// </summary>
        /// <returns></returns>
        public virtual Entity Clone()
        {
            Entity e = new Entity(Name, Tags.ToArray(), Transform.Position, Scene);
            var c = Components;
            e.Components = c;
            for (int i = 0; i < e.Components.Count; i++)
            {
                e.Components[i].Attached = e;
            }
            e.Components.attached = e;
            e.Active = Active;
            e.Paused = Paused;
            e.Visible = Visible;
            e.HasDebugDraw = HasDebugDraw;
            return e;
        }
        #endregion

        #region Components

        /// <summary>
        /// A shorthand to add components to an entity.
        /// </summary>
        /// <param name="component">The component to add.</param>
        public void AddComponent(Component component) => Components.Add(component);

        /// <summary>
        /// A shorthand to remove components to an entity.
        /// </summary>
        /// <param name="component"></param>
        public void RemoveComponent(Component component) => Components.Remove(component);

        /// <summary>
        /// Get the first component of that type.
        /// </summary>
        /// <typeparam name="T">The component's type.</typeparam>
        /// <returns>The first component that matches the type.</returns>
        public T GetComponent<T>() => Components.OfType<T>().First();

        /// <summary>
        /// Gets all components of the specified type.
        /// </summary>
        /// <typeparam name="T">The components' type.</typeparam>
        /// <returns>All components that match the type.</returns>
        public T[] GetComponents<T>() => Components.OfType<T>().ToArray();

        #endregion

        #region Enumerator
        public IEnumerator<Component> GetEnumerator()
        {
            return Components.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Components.GetEnumerator();
        }

        #endregion

        #region Parenting
        public void AddChild(Entity child)
        {
            if (!IsParentOf(child))
            {
                Children.Add(child);
                // dont call SetParent function to avoid loops.
                child.Parent = this;
                // call event
                child.OnParentAdded(this);
                // add transform.
                Transform.AddChild(child.Transform);
            }
        }

        public void RemoveChild(Entity child)
        {
            if (Children.Contains(child))
            {
                Children.Remove(child);
                child.SetParent(null);
                child.OnParentRemoved(this);
                Transform.RemoveChild(child.Transform);
            }
        }

        /// <summary>
        /// When the entity's parent is removed.
        /// </summary>
        /// <param name="parent">The parent that was removed.</param>
        public virtual void OnParentRemoved(Entity parent)
        {

        }

        /// <summary>
        /// When the entity gets a parent.
        /// </summary>
        /// <param name="parent">The new parent.</param>
        public virtual void OnParentAdded(Entity parent)
        {

        }

        public void SetParent(Entity parent)
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

        public bool IsParentOf(Entity child)
        {
            return Children.Contains(child);
        }

        public Entity GetAncestor()
        {
            Entity currentChild = this;
            while (currentChild.HasParent)
            {
                currentChild = currentChild.Parent;
            }
            return currentChild;
        }
        #endregion
    }
}
