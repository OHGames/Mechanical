/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O.H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/USED_CODE_LICENSES.txt for more info.
 */

using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Mechanical
{
    /// <summary>
    /// A wrapper for the <see cref="Keyboard"/>.
    /// </summary>
    public static class MechKeyboard
    {

        /// <summary>
        /// The current state of the keyboard.
        /// </summary>
        public static KeyboardState CurrentState { get; set; }

        /// <summary>
        /// The previous state of the keyboard.
        /// </summary>
        public static KeyboardState PreviousState { get; set; }

        /// <summary>
        /// How long each button was held.
        /// </summary>
        public static Dictionary<Keys, float> TimeHeld = new Dictionary<Keys, float>();

        /// <summary>
        /// The temp value.
        /// </summary>
        private static Dictionary<Keys, float> timeHeldTemp = new Dictionary<Keys, float>();

        public static void Update(float deltaTime)
        {
            PreviousState = CurrentState;

            CurrentState = Keyboard.GetState();

            Keys[] pressed = CurrentState.GetPressedKeys();

            timeHeldTemp = TimeHeld;

            for (int i = 0; i < pressed.Length; i++)
            {
                Keys k = pressed[i];
                // if held previously. We dont check if key is no longer held because it only has the currently pressed keys.
                if (timeHeldTemp.ContainsKey(k) && CurrentState.IsKeyDown(k))
                {
                    timeHeldTemp[k] = timeHeldTemp[k] + deltaTime;
                }
                // if not pressed yet.
                else if (timeHeldTemp.ContainsKey(k))
                {
                    timeHeldTemp.Add(k, 0);
                }
            }

            TimeHeld.Clear();

            // we make a temp because it only goes through pressed keys this frame. If there is a key no longer pressed we want to clear it from the list.
            TimeHeld = timeHeldTemp;
        }

        /// <summary>
        /// Get the time a key has been held for.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>The amount of time that a key has been held.</returns>
        public static float GetTimeHeld(Keys key)
        {
            if (TimeHeld.ContainsKey(key)) return TimeHeld[key];
            else return 0;
        }

        /// <summary>
        /// If the key went from released to pressed.
        /// </summary>
        /// <param name="key">They key to check.</param>
        /// <returns>True if the key was clicked.</returns>
        public static bool WasKeyClicked(Keys key)
        {
            return CurrentState.IsKeyDown(key) && PreviousState.IsKeyUp(key);
        }

        /// <summary>
        /// If the key was held this frame and last frame.
        /// </summary>
        /// <param name="key">They key to check.</param>
        /// <returns>True if the key was held.</returns>
        public static bool IsKeyHeld(Keys key)
        {
            return CurrentState.IsKeyDown(key) && PreviousState.IsKeyDown(key);
        }

        /// <summary>
        /// Get all the pressed keys.
        /// </summary>
        /// <returns>All the keys that are pressed.</returns>
        public static Keys[] GetPressedKeys()
        {
            return CurrentState.GetPressedKeys();
        }

        /// <summary>
        /// If the keyboard state has changed since the last frame.
        /// </summary>
        /// <returns>True if the keyboard state has changed.</returns>
        public static bool HasChanged()
        {
            return CurrentState != PreviousState;
        }

        /// <summary>
        /// If the key is currently down.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>True if the key is currently held down.</returns>
        public static bool IsKeyDown(Keys key) => CurrentState.IsKeyDown(key);

        /// <summary>
        /// If the key is currently up.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>True if the key is currently up.</returns>
        public static bool IsKeyUp(Keys key) => CurrentState.IsKeyUp(key);

    }
}
