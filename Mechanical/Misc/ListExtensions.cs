using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// A collection of extension methods for lists.
    /// </summary>
    public static class ListExtensions
    {

        /// <summary>
        /// This function sees if list contains at least one item from a list of items.
        /// </summary>
        /// <typeparam name="T">The type of objects in the lists.</typeparam>
        /// <param name="list">The list of items.</param>
        /// <param name="items">The list of items to check.</param>
        /// <returns>True if the list contains at least one item.</returns>
        public static bool ContainsAny<T>(this List<T> list, T[] items)
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
        public static bool ContainsAny<T>(this List<T> list, List<T> items) => ContainsAny<T>(list, items);

        /// <summary>
        /// This function checks to see if a list contains all the elements of a second list.
        /// </summary>
        /// <typeparam name="T">The type of items in the lists.</typeparam>
        /// <param name="list">The list of items.</param>
        /// <param name="items">The list of items to check.</param>
        /// <returns>True if all items in the second parameter are present in the list.</returns>
        public static bool ContainsAll<T>(this List<T> list, T[] items)
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
        public static bool ContainsAll<T>(this List<T> list, List<T> items) => ContainsAll<T>(list, items);

    }
}
