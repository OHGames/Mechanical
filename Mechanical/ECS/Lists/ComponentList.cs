﻿using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    public sealed class ComponentList : GameLikeList<Component>
    {

        private List<Component> justAdded = new List<Component>();

        private readonly Entity attached;

        public ComponentList(Entity entity)
        {
            attached = entity;
        }

        /// <summary>
        /// Add a component to the list.
        /// </summary>
        /// <param name="item">The component to add.</param>
        /// <exception cref="Exception">Throws excpetion if the component is already in list or if the component is in the list and only 1 is allowed.</exception>
        public override void Add(Component item)
        {
            // if it is allowed to be added
            if (!items.Contains(item) || item.AllowMultiple)
            {
                toAdd.Enqueue(item);
            }
            else
            {
                throw new Exception($"The component, {item.GetType().Name}, is already in the list.");
            }
        }

        /// <summary>
        /// If the list contains a component.
        /// </summary>
        /// <param name="item">The component to check.</param>
        /// <returns>True if component is in list.</returns>
        public override bool Contains(Component item)
        {
            return items.Contains(item);
        }

        /// <summary>
        /// Draw the components.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public override void Draw()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initialize all components.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Load the content of all components.
        /// </summary>
        /// <param name="content"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void LoadContent(ContentManager content)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Remove a component from the list.
        /// </summary>
        /// <param name="item">The component to remove.</param>
        /// <exception cref="Exception">Throws exception if component cannot be removed or component is not in list.</exception>
        public override void Remove(Component item)
        {
            if (!item.CanBeRemoved || !items.Contains(item)) throw new Exception($"The component, {item.GetType().Name}, cannot be removed or is not in the list!");
            else
            {
                toRemove.Enqueue(item);
            }
        }

        /// <summary>
        /// Update the components and add or remove waiting components.
        /// </summary>
        /// <param name="deltaTime">The time since last frame.</param>
        public override void Update(float deltaTime)
        {
            for (int i = 0; i < toAdd.Count; i++)
            {
                Component c = toAdd.Dequeue();
                items.Add(c);
                c.OnAdded();
                justAdded.Add(c);
            }
            for (int i = 0; i < toRemove.Count; i++)
            {
                Component c = toRemove.Dequeue();
                items.Remove(c);
                c.OnRemoved();
            }

            // awake
            justAdded.ForEach(c => c.Awake());

            justAdded.Clear();

            for (int i = 0; i < items.Count; i++)
            {
                items[i].Update(deltaTime);
            }

        }

        /// <summary>
        /// Get all items from the list.
        /// </summary>
        /// <returns>A list of components.</returns>
        public override List<Component> GetAll()
        {
            return items;
        }

        public override void DebugDraw()
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].DebugDraw();
            }
        }

    }
}
