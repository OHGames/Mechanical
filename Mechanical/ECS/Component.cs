/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O. H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/Use%20Code%20Licenses.md for more info.
 */

using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Mechanical
{
    
    /// <summary>
    /// The component is attached to the enmtity and can change it.
    /// </summary>
    [DataContract]
    public class Component
    {
        /// <summary>
        /// If the component is updating.
        /// </summary>
        [DataMember]
        public bool Active { get; set; }

        /// <summary>
        /// If the component is allowed to have more than one on an entity.
        /// </summary>
        [DataMember]
        public bool AllowMultiple { get; set; }

        /// <summary>
        /// If the component can be removed from the entity.
        /// </summary>
        [DataMember]
        public bool CanBeRemoved { get; set; }

        /// <summary>
        /// The entity attached to the component.
        /// </summary>
        [DataMember]
        public Entity Attached { get; set; }

        /// <summary>
        /// If the compinent has an override for the <see cref="DebugDraw"/> function
        /// </summary>
        [DataMember]
        public bool HasDebugDraw { get; set; }

        public Component(Entity entity)
        {
            Attached = entity;
        }

        public Component() { }

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

        public virtual void DebugDraw(bool editorRender)
        {
            if (!HasDebugDraw) Draw();
        }

        public virtual void Awake()
        {

        }

        public virtual void OnRemoved()
        {

        }

        public virtual void OnAdded()
        {

        }

        public virtual void EntityAwakes()
        {

        }

        public virtual void EntityAdded()
        {

        }

        public virtual void EntityRemoved()
        {

        }

        public void OnEntityDestroyed()
        {

        }

    }
}
