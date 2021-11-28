/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O.H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/USED_CODE_LICENSES.txt for more info.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// The ID Manager is a class used to handle Entity IDs.
    /// </summary>
    public static class IDManager
    {

        /// <summary>
        /// The list of IDS in use, or have been used.
        /// </summary>
        public static List<int> IDs = new List<int>();

        private static Random random = new Random();

        /// <summary>
        /// This function gets a new ID.
        /// </summary>
        /// <returns>A new ID for the entity.</returns>
        public static int GetId()
        {
            int id = random.Next();

            while (IDs.Contains(id)) id = random.Next();

            IDs.Add(id);

            return id;
        }

        /// <summary>
        /// Add an ID to the id list.
        /// </summary>
        /// <param name="id">The ID to add.</param>
        public static void AddID(int id) 
        { 
            if (!IDs.Contains(id)) 
                IDs.Add(id); 
        }

    }
}
