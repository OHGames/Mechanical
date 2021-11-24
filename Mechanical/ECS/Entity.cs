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
    /// 
    /// TODO: add parents and decide wither enmtities should draw themselves.
    /// </summary>
    public class Entity : IEnumerable<Component>
    {

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
    }
}
