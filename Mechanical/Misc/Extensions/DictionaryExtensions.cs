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
using System.Text;
using System.Linq;

namespace Mechanical
{
    /// <summary>
    /// Extensions for the <see cref="Dictionary{TKey, TValue}"/>
    /// </summary>
    public static class DictionaryExtensions
    {

        /// <summary>
        /// Gets the value from the specified index.
        /// </summary>
        /// <typeparam name="TKey">The key's type.</typeparam>
        /// <typeparam name="TValue">The value's type.</typeparam>
        /// <param name="dictionary">The dictionary to use.</param>
        /// <param name="index">The 0-based index to get.</param>
        /// <returns>The value at the index.</returns>
        public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, int index)
        {
            return dictionary.Values.ElementAt(index);
        }

        /// <summary>
        /// Gets the key at the specified index.
        /// </summary>
        /// <typeparam name="TKey">The key's type.</typeparam>
        /// <typeparam name="TValue">The value's type.</typeparam>
        /// <param name="dictionary">The dictionary to use.</param>
        /// <param name="index">The 0-based index to get.</param>
        /// <returns>The key at the index.</returns>
        public static TKey GetKey<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, int index)
        {
            return dictionary.Keys.ElementAt(index);
        }

    }
}
