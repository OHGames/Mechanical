using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// The entity class is anything that can be draw or updated in a <see cref="Scene"/>
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// The name of the entity.
        /// </summary>
        public string Name { get; set; } = "Entity";

        /// <summary>
        /// The static, nonchanging ID of the entity.
        /// 
        /// <para>
        /// This can be used for grabbing an entity from the scene.
        /// </para>
        /// </summary>
        public int ID { get; set; } = IDManager.GetId();

        /// <summary>
        /// A list of strings used to group entities.
        /// </summary>
        public List<string> Tags = new List<string>();
        
        /// <summary>
        /// The list of components attached to this entity.
        /// </summary>
        public ComponentList Components { get; private set; }

        /// <summary>
        /// A refrence to the Transform component on the entity.
        /// </summary>
        // This is the first index because it should always be the first component. 
        public Transform Transform { get => (Transform)Components[0]; }

        /// <summary>
        /// If the entity will update or draw.
        /// </summary>
        public bool Active { get; set; }
        
        /// <summary>
        /// If the entity will be drawn.
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// If the entity will update.
        /// </summary>
        public bool Paused
        {
            get; set;
        }

        /// <summary>
        /// If the entity has an override for the <see cref="DebugDraw"/> function
        /// </summary>
        public bool HasDebugDraw { get; set; }

        public Entity(string name)
        {
            Name = name;
            Components = new ComponentList(this);
            Components.Add(new Transform(this));
        }

        public Entity(string name, Vector2 position) : this(name)
        {
            Transform.Position = position;
        }

        public Entity(string name, string[] tags, Vector2 position) : this(name, position)
        {
            Tags = tags.ToList();
        }

        public virtual void Initalize()
        {

        }

        public virtual void LoadContent(ContentManager content)
        {

        }

        public virtual void Update(float deltaTime)
        {
            Components.Update(deltaTime);
        }

        public virtual void Draw()
        {

        }

        public virtual void DebugDraw()
        {
            if (!HasDebugDraw) Draw();
        }

        public void Awake()
        {

        }

        public void OnAdded()
        {

        }

        public void OnRemoved()
        {

        }

        public void Destroy()
        {

        }

    }
}
