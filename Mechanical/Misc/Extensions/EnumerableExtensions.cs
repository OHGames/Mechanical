/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O. H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/Use%20Code%20Licenses.md for more info.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// A collection of extension methods for <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static class EnumerableExtensions
    {

        /// <summary>
        /// This function sees if list contains at least one item from a list of items.
        /// </summary>
        /// <typeparam name="T">The type of objects in the lists.</typeparam>
        /// <param name="list">The list of items.</param>
        /// <param name="items">The list of items to check.</param>
        /// <returns>True if the list contains at least one item.</returns>
        public static bool ContainsAny<T>(this IEnumerable<T> list, T[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (list.Contains(items[i])) return true;
            }
            // did not match any.
            return false;
        }

        /// <summary>
        /// This function sees if list contains at least one item from a list of items.
        /// </summary>
        /// <typeparam name="T">The type of objects in the lists.</typeparam>
        /// <param name="list">The list of items.</param>
        /// <param name="items">The list of items to check.</param>
        /// <returns>True if the list contains at least one item.</returns>
        public static bool ContainsAny<T>(this IEnumerable<T> list, IEnumerable<T> items) => ContainsAny<T>(list, items);

        /// <summary>
        /// This function checks to see if a list contains all the elements of a second list.
        /// </summary>
        /// <typeparam name="T">The type of items in the lists.</typeparam>
        /// <param name="list">The list of items.</param>
        /// <param name="items">The list of items to check.</param>
        /// <returns>True if all items in the second parameter are present in the list.</returns>
        public static bool ContainsAll<T>(this IEnumerable<T> list, T[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                // if the item is not in list.
                if (!list.Contains(items[i])) return false;
            }
            // all items are in list
            return true;
        }

        /// <summary>
        /// This function checks to see if a list contains all the elements of a second list.
        /// </summary>
        /// <typeparam name="T">The type of items in the lists.</typeparam>
        /// <param name="list">The list of items.</param>
        /// <param name="items">The list of items to check.</param>
        /// <returns>True if all items in the second parameter are present in the list.</returns>
        public static bool ContainsAll<T>(this IEnumerable<T> list, IEnumerable<T> items) => ContainsAll<T>(list, items);

        /// <summary>
        /// This function checks to see if a list contains any items of the specified type list.
        /// </summary>
        /// <typeparam name="T">The type of list.</typeparam>
        /// <param name="list">The list of items.</param>
        /// <param name="types">The list of types to check.</param>
        /// <returns>True if the list contains an item that has a type defined in the types parameter.</returns>
        public static bool ContainsAnyOfType<T>(this IEnumerable<T> list, Type[] types)
        {
            // loop through list.
            for (int i = 0; i < list.Count(); i++)
            {
                // if the type is in the types list.
                if (types.Contains(list.ElementAt(i).GetType())) return true;
            }
            return false;
        }

        /// <summary>
        /// This function checks to see if a list contains any items of the specified type list.
        /// </summary>
        /// <typeparam name="T">The type of list.</typeparam>
        /// <param name="list">The list of items.</param>
        /// <param name="types">The list of types to check.</param>
        /// <returns>True if the list contains an item that has a type defined in the types parameter.</returns>
        public static bool ContainsAnyOfType<T>(this IEnumerable<T> list, IEnumerable<Type> types) => ContainsAnyOfType<T>(list, types.ToArray());

        /// <summary>
        /// This function checks to see if a list contains all of the types in the specified type list.
        /// </summary>
        /// <typeparam name="T">The type of list.</typeparam>
        /// <param name="list">The list to check.</param>
        /// <param name="types">The list of types.</param>
        /// <returns>True if the list contains all items that have a type defined in the types parameter.</returns>
        public static bool ContainsAllOfType<T>(this IEnumerable<T> list, Type[] types)
        {
            for (int i = 0; i < types.Length; i++)
            {
                // there is no items in the list with type.
                if (list.Where(t => t.GetType() == types[i]).Count() == 0)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// This function checks to see if a list contains all of the types in the specified type list.
        /// </summary>
        /// <typeparam name="T">The type of list.</typeparam>
        /// <param name="list">The list to check.</param>
        /// <param name="types">The list of types.</param>
        /// <returns>True if the list contains all items that have a type defined in the types parameter.</returns>
        public static bool ContainsAllOfType<T>(this IEnumerable<T> list, IEnumerable<Type> types) => ContainsAllOfType<T>(list, types.ToArray());

        /// <summary>
        /// Wrap an index so it is never out of bounds. Special use case only.
        /// </summary>
        /// <typeparam name="T">The type of list.</typeparam>
        /// <param name="list">The list to use.</param>
        /// <param name="index">The index to possibly wrap.</param>
        /// <returns>The element of the list at the wrapped index.</returns>
        public static T WrapIndex<T>(this IEnumerable<T> list, int index)
        {
            // https://youtu.be/Zgf1DYrmSnk?t=693 code under MIT license
            return list.ElementAt(index % list.Count());
        }

    }
}
