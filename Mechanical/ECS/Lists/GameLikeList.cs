/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O.H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/USED_CODE_LICENSES.txt for more info.
 */

using Microsoft.Xna.Framework.Content;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Mechanical
{
    [DataContract]
    /// <summary>
    /// The Game-Like List is a class that can be used to update items like the <see cref="Engine"/> class.
    /// </summary>
    /// <typeparam name="T">The type for the list.</typeparam>
    public abstract class GameLikeList<T> : IEnumerable<T>
    {
        [DataMember]
        /// <summary>
        /// The items in the list
        /// </summary>
        protected List<T> items = new List<T>();

        [DataMember]
        /// <summary>
        /// A queue of items to add next update.
        /// </summary>
        protected Queue<T> toAdd = new Queue<T>();

        [DataMember]
        /// <summary>
        /// A queue of items to remove next update.
        /// </summary>
        protected Queue<T> toRemove = new Queue<T>();

        /// <summary>
        /// The amount of items in the list.
        /// </summary>
        public int Count { get => items.Count; }

        public abstract void Add(T item);

        public abstract void Remove(T item);

        public abstract bool Contains(T item);

        public abstract void Initialize();

        public abstract void LoadContent(ContentManager content);

        public abstract void Update(float deltaTime);

        public abstract void Draw();

        public abstract void DebugDraw(bool editorRender);

        public abstract List<T> GetAll();

        public IEnumerator<T> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }
    }
}
