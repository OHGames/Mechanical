using Microsoft.Xna.Framework.Input;

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

        public static void Update(float deltaTime)
        {
            PreviousState = CurrentState;

            CurrentState = Keyboard.GetState();
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
