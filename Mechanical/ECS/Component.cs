using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    public class Component
    {
        /// <summary>
        /// If the component is updating.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// If the component is allowed to have more than one on an entity.
        /// </summary>
        public bool AllowMultiple { get; set; }

        /// <summary>
        /// If the component can be removed from the entity.
        /// </summary>
        public bool CanBeRemoved { get; set; }

        /// <summary>
        /// The entity attached to the component.
        /// </summary>
        public Entity Attached { get; set; }

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

        public virtual void DebugDraw()
        {

        }

        public virtual void Awake()
        {

        }

        public virtual void OnRemoved()
        {

        }

        public void OnAdded()
        {

        }

    }
}
