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
using System.Linq;

namespace Mechanical
{
    /// <summary>
    /// This class handles and stores <see cref="Keybind"/>s.
    /// </summary>
    public static class Keybinds
    {
        /// <summary>
        /// The keybinds that have been added. The string is the name of the keybind.
        /// </summary>
        public static Dictionary<string, Keybind> RegisterdKeybinds = new Dictionary<string, Keybind>();

        /// <summary>
        /// Add a keybind.
        /// </summary>
        /// <param name="keybind">The keybind to add.</param>
        /// <param name="name">The name to attribute the keybind with.</param>
        /// <exception cref="Exception">When name already exists in dictonary.</exception>
        public static void Add(Keybind keybind, string name)
        {
            if (RegisterdKeybinds.ContainsKey(name)) throw new Exception($"{name} is already a registered keybind.");

            RegisterdKeybinds.Add(name, keybind);
        }

        /// <summary>
        /// Remove a keybind.
        /// </summary>
        /// <param name="name">The name of the keybind to remove.</param>
        /// <exception cref="ArgumentException">If the name does not exist.</exception>
        public static void Remove(string name)
        {
            if (!RegisterdKeybinds.ContainsKey(name)) throw new ArgumentException($"{name} does not exist in the list yet.");

            RegisterdKeybinds.Remove(name);
        }

        /// <summary>
        /// Gets a keybind from a name.
        /// </summary>
        /// <param name="name">The name of the keybind.</param>
        /// <returns>The keybind the name attributed to.</returns>
        /// <exception cref="ArgumentException">When the name does not exist.</exception>
        public static Keybind GetKeybind(string name)
        {
            if (RegisterdKeybinds.ContainsKey(name))
            {
                return RegisterdKeybinds[name];
            }
            else
            {
                throw new ArgumentException($"{name} is not a registered name.");
            }
        }

        /// <summary>
        /// Update the keybinds.
        /// </summary>
        /// <param name="deltaTime">The time since last frame (modified).</param>
        public static void Update(float deltaTime)
        {
            for (int i = 0; i < RegisterdKeybinds.Count; i++)
            {
                Keybind k = RegisterdKeybinds.ElementAt(i).Value;
                k.Update(deltaTime);

                string name = RegisterdKeybinds.Keys.ElementAt(i);
                RegisterdKeybinds[name] = k;
            }
        }

    }
}
