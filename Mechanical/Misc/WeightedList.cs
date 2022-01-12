/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O. H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/USED_CODE_LICENSES.txt for more info.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Mechanical
{
    /// <summary>
    /// The weighted list class is a class used to generate random items with weighting.
    /// 
    /// </summary>
    /// <remarks>
    /// This class essencially is just a list 
    /// </remarks>
    /// <typeparam name="T">The type of item.</typeparam>
    public class WeightedList<T> : IList<T>, IEnumerable<T>
    {
        /// <summary>
        /// Get the weight of an item.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>The item at the index.</returns>
        /// <exception cref="NotImplementedException">When trying to set a value, use <see cref="Add(T, int)"/></exception>
        public int this[T item] { get => Items[item]; set => throw new NotImplementedException(); }

        /// <summary>
        /// Returns the item at a specific index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The item at the index.</returns>
        /// <exception cref="NotImplementedException">To remove an item use <see cref="Remove(T)"/></exception>
        public T this[int index] { get => Items.Keys.ElementAt(index); set => throw new NotImplementedException(); }

        /// <summary>
        /// The items in the list.
        /// </summary>
        public Dictionary<T, int> Items { get; } = new Dictionary<T, int>();

        public int Count => Items.Count;

        public bool IsReadOnly => false;

        /// <summary>
        /// The list of items added based on weight.
        /// </summary>
        private List<T> weightedList = new List<T>();

        public WeightedList()
        {
            // (～￣ ▽ ￣)～
        }

        /// <summary>
        /// Make a new weighted list with items.
        /// </summary>
        /// <param name="items">The items (pre-weighted)</param>
        public WeightedList(params T[] items)
        {
            weightedList = items.ToList();
        }

        /// <summary>
        /// Make a new weighted list with weights.
        /// </summary>
        /// <param name="items">The dictionary of weights.</param>
        public WeightedList(Dictionary<T, int> items)
        {
            Items = items;
            SetList();
        }

        /// <summary>
        /// Get a random item.
        /// </summary>
        /// <returns></returns>
        public T Get()
        {
            // make a new random for new seed.
            Random random = new Random();

            return weightedList[random.Next(0, Count - 1)];
        }

        private void SetList()
        {
            // clear
            weightedList.Clear();
            // loop through all items
            foreach (var item in Items)
            {
                // the weight.
                int weight = item.Value;
                // add based on weight.
                for (int j = 0; j < weight; j++)
                {
                    weightedList.Add(item.Key);
                }
            }
        }

        /// <summary>
        /// Add an item with a specific weight. If an item is already in the list it sets the weight.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="weight">The weight of the item.</param>
        public void Add(T item, int weight)
        {
            if (Items.ContainsKey(item))
            {
                // set weight
                Items[item] = weight;
            }
            else
            {
                // add item
                Items.Add(item, weight);
            }
            SetList();
        }

        public void Add(T item)
        {
            Add(item, 0);
        }

        public void Clear()
        {
            Items.Clear();
        }

        public bool Contains(T item)
        {
            return Items.ContainsKey(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Items.Keys.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return Items.Keys.ToList().IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            if (!Items.ContainsKey(item))
            {
                throw new Exception($"Cannot remove {item} because it is not in list.");
            }
            else
            {
                var b = Items.Remove(item);
                SetList();
                return b;
            }
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }
    }
}
