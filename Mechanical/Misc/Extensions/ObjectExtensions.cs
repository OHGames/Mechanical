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
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Mechanical
{
    /// <summary>
    /// Extensions for any object.
    /// </summary>
    public static class ObjectExtensions
    {

        //https://stackoverflow.com/a/78612 code comes from this answer here.
        // function licensed under https://creativecommons.org/licenses/by-sa/4.0/
        /// <summary>
        /// Returns a deep clone of an object. Must contain an empty constructor.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <param name="obj">The object to copy.</param>
        /// <returns>A copy of the <paramref name="obj"/></returns>
        public static T DeepClone<T>(this T obj) where T : new()
        {
            if (obj == null) return default;

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace
            };

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj, settings), settings);
        }

    }
}
