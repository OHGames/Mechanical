/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O. H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/USED_CODE_LICENSES.txt for more info.
 */

using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace Mechanical
{
    [DataContract]
    public sealed class ComponentList : GameLikeList<Component>
    {
        [DataMember]
        private List<Component> justAdded = new List<Component>();

        /// <summary>
        /// The drawable components.
        /// </summary>
        [DataMember]
        private List<IDrawableComponent> drawables = new List<IDrawableComponent>();

        [DataMember]
        public Entity attached;

        private bool safeToChange = true;

        /// <summary>
        /// If we need to resort the components.
        /// </summary>
        private bool isDirty = true;

        /// <summary>
        /// Get component from list.
        /// </summary>
        /// <param name="index">The index</param>
        /// <returns></returns>
        public Component this[int index]
        {
            get => items[index];
        }

        public ComponentList(Entity entity)
        {
            attached = entity;
            justAdded = new List<Component>();
            drawables = new List<IDrawableComponent>();
        }

        /// <summary>
        /// Add a component to the list.
        /// </summary>
        /// <param name="item">The component to add.</param>
        /// <exception cref="Exception">Throws excpetion if the component is already in list or if the component is in the list and only 1 is allowed.</exception>
        public override void Add(Component item)
        {
            // if it is allowed to be added
            if (!Contains(item) || item.AllowMultiple)
            {
                if (safeToChange) 
                { 
                    items.Add(item);
                    // if component is drawable
                    if (item.GetType().GetInterface("IDrawableComponent") != null)
                    {
                        drawables.Add(item as IDrawableComponent);
                    }
                }
                else toAdd.Enqueue(item);
            }
            else
            {
                throw new Exception($"The component, {item.GetType().Name}, is already in the list.");
            }
            isDirty = true;
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
        public override void Draw()
        {
            // sort by render order.
            if (drawables.Count > 0)
            {
                if (isDirty)
                {
                    drawables = drawables.OrderBy(c => c.RenderOrder).ToList();
                    isDirty = false;
                }

                for (int i = 0; i < drawables.Count(); i++)
                {
                    drawables[i].Draw();
                }
            }
        }

        /// <summary>
        /// Initialize all components.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public override void Initialize()
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Initialize();
            }
        }

        /// <summary>
        /// Load the content of all components.
        /// </summary>
        /// <param name="content"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void LoadContent(ContentManager content)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].LoadContent(content);
            }
        }

        /// <summary>
        /// Remove a component from the list.
        /// </summary>
        /// <param name="item">The component to remove.</param>
        /// <exception cref="Exception">Throws exception if component cannot be removed or component is not in list.</exception>
        public override void Remove(Component item)
        {
            if (!item.CanBeRemoved || !Contains(item)) throw new Exception($"The component, {item.GetType().Name}, cannot be removed or is not in the list!");
            else
            {
                if (safeToChange) items.Remove(item);
                else toRemove.Enqueue(item);
            }
        }

        /// <summary>
        /// Update the components and add or remove waiting components.
        /// </summary>
        /// <param name="deltaTime">The time since last frame.</param>
        public override void Update(float deltaTime)
        {
            int count = toAdd.Count;
            for (int i = 0; i < count; i++)
            {
                Component c = toAdd.Dequeue();
                bool shouldAdd = true;
                if (c.GetType().GetCustomAttribute<NeedsComponentAttribute>() != null)
                {
                    Type[] types = c.GetType().GetCustomAttribute<NeedsComponentAttribute>().TypesNeeded;

                    // the components required is not in the list/.
                    if (!items.ContainsAllOfType(types))
                    {
                        // check the waiting components
                        if (!toAdd.ToList().ContainsAllOfType(types))
                        {
                            // should not be added
                            shouldAdd = false;
                            // add again later
                            toAdd.Enqueue(c);
                        }
                    }
                }

                if (shouldAdd)
                {
                    items.Add(c);
                    c.OnAdded();
                    justAdded.Add(c);

                    // if component is drawable
                    if (c.GetType().GetInterface("IDrawableComponent") != null)
                    {
                        drawables.Add((IDrawableComponent)c);
                    }
                }
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

            safeToChange = false;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Active)
                    items[i].Update(deltaTime);
            }
            safeToChange = true;
        }

        /// <summary>
        /// Get all items from the list.
        /// </summary>
        /// <returns>A list of components.</returns>
        public override List<Component> GetAll()
        {
            return items;
        }

        public override void DebugDraw(bool editorRender)
        {
            // sort by render order.
            if (drawables.Count > 0)
            {
                if (isDirty)
                {
                    drawables = drawables.OrderBy(c => c.RenderOrder).ToList();
                    isDirty = false;
                }

                for (int i = 0; i < drawables.Count(); i++)
                {
                    drawables[i].DebugDraw(editorRender);
                }
            }
        }

        /// <summary>
        /// When the entity awakes.
        /// </summary>
        public void EntityAwakes()
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].EntityAwakes();
            }
        }

        /// <summary>
        /// When the entity is added.
        /// </summary>
        public void OnEntityAdded()
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].EntityAdded();
            }
        }

        /// <summary>
        /// When the entity is removed.
        /// </summary>
        public void OnEntityRemoved()
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].EntityRemoved();
            }
        }

        /// <summary>
        /// When the entity is destroyed.
        /// </summary>
        public void OnEntityDestroyed()
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].OnEntityDestroyed();
            }
        }

        public override void Add(params Component[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                Add(items[i]);
            }
        }
    }
}
