using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
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

        public Entity(string name)
        {
            Components = new ComponentList(this);
        }

        public Entity(string name, Vector2 position) : this(name)
        {

        }

        public Entity(string name, string[] tags, Vector2 position) : this(name, position)
        {

        }

        public virtual void Initalize()
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

        public virtual void DebugDraw()
        {

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
