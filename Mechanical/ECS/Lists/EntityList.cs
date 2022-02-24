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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// The EntityList is a convenient method to store and update entities. This should only be used for scenes.
    /// </summary>
    [DataContract]
    public class EntityList : GameLikeList<Entity>
    {
        /// <summary>
        /// Just added items.
        /// </summary>
        [DataMember]
        private List<Entity> justAdded = new List<Entity>();

        [DataMember]
        private List<IDrawable> drawable = new List<IDrawable>();

        private bool safeToChange = true;

        /// <summary>
        /// The entities in the background.
        /// </summary>
        private IDrawable[] bg;

        /// <summary>
        /// Entities in the mid ground.
        /// </summary>
        private IDrawable[] mg;

        /// <summary>
        /// Entities in the foreground.
        /// </summary>
        private IDrawable[] fg;

        /// <summary>
        /// If the list is dirty and we need to resort.
        /// </summary>
        private bool isDirty = true;

        #region Indexors
        /// <summary>
        /// This returns an entity from the specified index. Or returns the entity with the id.
        /// </summary>
        /// <param name="index">The index or ID of the entity.</param>
        /// <param name="id">This bool makes it so that it uses the ID instead of the index.</param>
        /// <returns>The entity at the specified index or ID.</returns>
        public Entity this[int index, bool id = false]
        {
            get
            {
                // Get the id. Because there is only 1 entity that has the specified id, get the first one.
                return id ? ((Entity[])items.Where(e => e.ID == index))[0] : items[index];
            }
        }

        /// <summary>
        /// Gets items based on name.
        /// </summary>
        /// <param name="name">The name of the entity.</param>
        /// <returns>A list of items with the name.</returns>
        public Entity[] this[string name]
        {
            get
            {
                return (Entity[])items.Where(e => e.Name == name);
            }
        }

        /// <summary>
        /// Gets items based on tags.
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public Entity[] this[string[] tags]
        {
            get
            {
                return (Entity[])items.Where(e => e.Tags.ContainsAny(tags));
            }
        }
        #endregion

        /// <summary>
        /// Add an entity to the list. The entity will be added next frame.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <exception cref="Exception">Throws exception when there is an entity already added.</exception>
        public override void Add(Entity entity)
        {
            if (!Contains(entity))
            {
                if (safeToChange)
                {
                    items.Add(entity);

                    if (entity.GetType().GetInterface("IDrawable") != null)
                    {
                        drawable.Add(entity as IDrawable);
                    }
                }
                else toAdd.Enqueue(entity);
            }
            else
            {
                throw new Exception($"The entity, {entity.Name} id: {entity.ID}, is already added!");
            }
            isDirty = true;
        }

        /// <summary>
        /// Removes items from the list. The entity will be removed next frame.
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="Exception">Throws exception when the is entity is not in the list.</exception>
        public override void Remove(Entity entity)
        {
            if (Contains(entity))
            {
                if (safeToChange) items.Remove(entity);
                else toRemove.Enqueue(entity);
            }
            else
            {
                throw new Exception($"The entity, {entity.Name} id: {entity.ID}, is not in the list!");
            }
            isDirty = true;
        }

        /// <summary>
        /// This function checks to make sure the entity is contained in the list.
        /// </summary>
        /// <param name="entity"></param>
        public override bool Contains(Entity entity) => items.Contains(entity);

        /// <summary>
        /// Update all the items. This function also adds and removes items that need to be so.
        /// </summary>
        /// <param name="deltaTime">The time since last frame.</param>
        public override void Update(float deltaTime)
        {
            for (int i = 0; i < toAdd.Count; i++)
            {
                // add items.
                Entity e = toAdd.Dequeue();
                // add to scene
                items.Add(e);
                // call function.
                e.OnAdded();
                // add to just added
                justAdded.Add(e);
            }
            for (int j = 0; j < toRemove.Count; j++)
            {
                // remove items.
                Entity e = toRemove.Dequeue();
                items.Remove(e);
                e.OnRemoved();
            }

            // awake all items that have been added
            justAdded.ForEach(e => e.Awake());

            justAdded.Clear();

            safeToChange = false;
            // update items
            for (int k = 0; k < items.Count; k++)
            {
                items[k].Update(deltaTime);
            }
            safeToChange = true;
        }

        /// <summary>
        /// Draw all the items.
        /// </summary>
        public override void Draw()
        {
            if (drawable.Count > 0)
            {

                if (isDirty)
                {
                    bg = drawable.Where(e => e.RenderLayer == RenderLayer.Background).ToArray();
                    mg = drawable.Where(e => e.RenderLayer == RenderLayer.Midground).ToArray();
                    fg = drawable.Where(e => e.RenderLayer == RenderLayer.Foreground).ToArray();

                    bg = bg.OrderBy(e => e.RenderOrder).ToArray();
                    mg = mg.OrderBy(e => e.RenderOrder).ToArray();
                    fg = fg.OrderBy(e => e.RenderOrder).ToArray();

                    isDirty = false;
                }

                for (int i = 0; i < bg.Count(); i++)
                {
                    bg[i].Draw();
                }

                for (int i = 0; i < mg.Count(); i++)
                {
                    mg[i].Draw();
                }

                for (int i = 0; i < fg.Count(); i++)
                {
                    fg[i].Draw();
                }

            }
        }

        /// <summary>
        /// Load the content on all items.
        /// </summary>
        /// <param name="content"></param>
        public override void LoadContent(ContentManager content)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].LoadContent(content);
            }
        }

        /// <summary>
        /// Initalize all items.
        /// </summary>
        public override void Initialize()
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Initialize();
            }
        }

        /// <summary>
        /// Get all the entities.
        /// </summary>
        /// <returns>A list of entities.</returns>
        public override List<Entity> GetAll()
        {
            return items;
        }

        /// <summary>
        /// Debug draw all entities.
        /// </summary>
        public override void DebugDraw(bool editorRender)
        {
            if (drawable.Count > 0)
            {
                if (isDirty)
                {
                    bg = drawable.Where(e => e.RenderLayer == RenderLayer.Background).ToArray();
                    mg = drawable.Where(e => e.RenderLayer == RenderLayer.Midground).ToArray();
                    fg = drawable.Where(e => e.RenderLayer == RenderLayer.Foreground).ToArray();

                    bg = bg.OrderBy(e => e.RenderOrder).ToArray();
                    mg = mg.OrderBy(e => e.RenderOrder).ToArray();
                    fg = fg.OrderBy(e => e.RenderOrder).ToArray();

                    isDirty = false;
                }

                for (int i = 0; i < bg.Count(); i++)
                {
                    bg[i].DebugDraw(editorRender);
                }

                for (int i = 0; i < mg.Count(); i++)
                {
                    mg[i].DebugDraw(editorRender);
                }

                for (int i = 0; i < fg.Count(); i++)
                {
                    fg[i].DebugDraw(editorRender);
                }

            }
        }

        public override void Add(params Entity[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                Add(items[i]);
            }
        }
    }
}
