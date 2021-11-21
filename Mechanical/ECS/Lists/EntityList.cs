using Microsoft.Xna.Framework.Content;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// The EntityList is a convenient method to store and update entities. This should only be used for scenes.
    /// </summary>
    public class EntityList : IEnumerable<Entity>
    {

        /// <summary>
        /// The entities in the list
        /// </summary>
        private List<Entity> entities = new List<Entity>();

        /// <summary>
        /// A queue of entities to add next update.
        /// </summary>
        private Queue<Entity> toAdd = new Queue<Entity>();

        /// <summary>
        /// A queue of entities to remove next update.
        /// </summary>
        private Queue<Entity> toRemove = new Queue<Entity>();

        /// <summary>
        /// Just added entities.
        /// </summary>
        List<Entity> justAdded = new List<Entity>();

        /// <summary>
        /// The amount of entities in the list.
        /// </summary>
        public int Count { get => entities.Count; }

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
                return id ? ((Entity[])entities.Where(e => e.ID == index))[0] : entities[index];
            }
        }

        /// <summary>
        /// Gets entities based on name.
        /// </summary>
        /// <param name="name">The name of the entity.</param>
        /// <returns>A list of entities with the name.</returns>
        public Entity[] this[string name]
        {
            get
            {
                return (Entity[])entities.Where(e => e.Name == name);
            }
        }

        /// <summary>
        /// Gets entities based on tags.
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public Entity[] this[string[] tags]
        {
            get
            {
                return (Entity[])entities.Where(e => e.Tags.ContainsAny(tags));
            }
        }
        #endregion

        /// <summary>
        /// Add an entity to the list. The entity will be added next frame.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <exception cref="Exception">Throws exception when there is an entity already added.</exception>
        public void Add(Entity entity)
        {
            if (!entities.Contains(entity))
            {
                toAdd.Enqueue(entity);
            }
            else
            {
                throw new Exception($"The entity, {entity.Name} id: {entity.ID}, is already added!");
            }
        }

        /// <summary>
        /// Removes entities from the list. The entity will be removed next frame.
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="Exception">Throws exception when the is entity is not in the list.</exception>
        public void Remove(Entity entity)
        {
            if (entities.Contains(entity))
            {
                toRemove.Enqueue(entity);
            }
            else
            {
                throw new Exception($"The entity, {entity.Name} id: {entity.ID}, is not in the list!");
            }
        }

        /// <summary>
        /// This function checks to make sure the entity is contained in the list.
        /// </summary>
        /// <param name="entity"></param>
        public void Contains(Entity entity) => entities.Contains(entity);

        /// <summary>
        /// Update all the entities. This function also adds and removes entities that need to be so.
        /// </summary>
        /// <param name="deltaTime">The time since last frame.</param>
        public void Update(float deltaTime)
        {
            for (int i = 0; i < toAdd.Count; i++)
            {
                // add entities.
                Entity e = toAdd.Dequeue();
                // add to scene
                entities.Add(e);
                // call function.
                e.OnAdded();
                // add to just added
                justAdded.Add(e);
            }
            for (int j = 0; j < toRemove.Count; j++)
            {
                // remove entities.
                Entity e = toRemove.Dequeue();
                entities.Remove(e);
                e.OnRemoved();
            }

            // awake all entities that have been added
            justAdded.ForEach(e => e.Awake());

            justAdded.Clear();

            // update entities
            for (int k = 0; k < entities.Count; k++)
            {
                entities[k].Update(deltaTime);
            }
        }

        /// <summary>
        /// Draw all the entities.
        /// </summary>
        public void Draw()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].Draw();
            }
        }

        /// <summary>
        /// Load the content on all entities.
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].LoadContent(content);
            }
        }

        /// <summary>
        /// Initalize all entities.
        /// </summary>
        public void Initalize()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].Initalize();
            }
        }

        #region Enumerator

        public IEnumerator<Entity> GetEnumerator()
        {
            return entities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return entities.GetEnumerator();
        }
        #endregion
    }
}
