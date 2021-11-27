using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// The <see cref="Mouse"/> wrapper.
    /// </summary>
    public static class MechMouse
    {

        /// <summary>
        /// The current mouse state.
        /// </summary>
        public static MouseState CurrentMouse { get; private set; }

        /// <summary>
        /// The previous mouse state.
        /// </summary>
        public static MouseState PreviousMouse { get; private set; }

        /// <summary>
        /// The change in location of the mouse.
        /// </summary>
        public static Vector2 MouseDelta => PreviousMouse.Position.ToVector2() - CurrentMouse.Position.ToVector2();

        /// <summary>
        /// The change in the scroll of the scroll wheel.
        /// </summary>
        public static float ScrollDelta => PreviousMouse.ScrollWheelValue - CurrentMouse.ScrollWheelValue;

        /// <summary>
        /// A shorthand for the mouse position.
        /// </summary>
        public static Vector2 MousePosition => CurrentMouse.Position.ToVector2();

        /// <summary>
        /// Update the mouse.
        /// </summary>
        /// <param name="deltaTime"></param>
        public static void Update(float deltaTime)
        {
            PreviousMouse = CurrentMouse;

            CurrentMouse = Mouse.GetState();
        }

        /// <summary>
        /// When the left button is pressed.
        /// </summary>
        /// <returns>True if the mouse was pressed this frame and not the last.</returns>
        public static bool WasLeftButtonClicked()
        {
            return CurrentMouse.LeftButton == ButtonState.Pressed && PreviousMouse.LeftButton == ButtonState.Released;
        }

        /// <summary>
        /// When the right button is pressed.
        /// </summary>
        /// <returns>True if the mouse was pressed this frame and not the last.</returns>
        public static bool WasRightButtonClicked()
        {
            return CurrentMouse.RightButton == ButtonState.Pressed && PreviousMouse.RightButton == ButtonState.Released;
        }

        /// <summary>
        /// When the left button is pressed.
        /// </summary>
        /// <returns>True if the mouse was pressed this frame and not the last.</returns>
        public static bool WasMiddleButtonClicked()
        {
            return CurrentMouse.MiddleButton == ButtonState.Pressed && PreviousMouse.MiddleButton == ButtonState.Released;
        }

        /// <summary>
        /// When the left button is held.
        /// </summary>
        /// <returns>True if the mouse was pressed this frame and pressed the last frame.</returns>
        public static bool IsLeftButtonHeld()
        {
            return CurrentMouse.LeftButton == ButtonState.Pressed && PreviousMouse.LeftButton == ButtonState.Pressed;
        }

        /// <summary>
        /// When the right button is held.
        /// </summary>
        /// <returns>True if the mouse was pressed this frame and pressed the last frame.</returns>
        public static bool IsRightButtonHeld()
        {
            return CurrentMouse.RightButton == ButtonState.Pressed && PreviousMouse.RightButton == ButtonState.Pressed;
        }

        /// <summary>
        /// When the left button is held.
        /// </summary>
        /// <returns>True if the mouse was pressed this frame and pressed the last frame.</returns>
        public static bool IsMiddleButtonHeld()
        {
            return CurrentMouse.MiddleButton == ButtonState.Pressed && PreviousMouse.MiddleButton == ButtonState.Pressed;
        }

    }
}
