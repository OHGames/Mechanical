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
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mechanical
{
    /// <summary>
    /// A wrapper for the <see cref="Controller"/>
    /// 
    /// TODO: Add time held property.
    /// </summary>
    public static class MechController
    {
        #region States
        /// <summary>
        /// The current gamepad state for player 1.
        /// </summary>
        public static GamePadState CurrentState1 { get; set; }

        /// <summary>
        /// The previous gamepad state for player 1.
        /// </summary>
        public static GamePadState PreviousState1 { get; set; }

        /// <summary>
        /// The current gamepad state for player 2.
        /// </summary>
        public static GamePadState CurrentState2 { get; set; }

        /// <summary>
        /// The previous gamepad state for player 2.
        /// </summary>
        public static GamePadState PreviousState2 { get; set; }

        /// <summary>
        /// The current gamepad state for player 3.
        /// </summary>
        public static GamePadState CurrentState3 { get; set; }

        /// <summary>
        /// The previous gamepad state for player 3.
        /// </summary>
        public static GamePadState PreviousState3 { get; set; }

        /// <summary>
        /// The current gamepad state for player 4.
        /// </summary>
        public static GamePadState CurrentState4 { get; set; }

        /// <summary>
        /// The previous gamepad state for player 4.
        /// </summary>
        public static GamePadState PreviousState4 { get; set; }

        #endregion

        #region Update
        public static void Update(float deltaTime)
        {
            // player 1
            PreviousState1 = CurrentState1;
            CurrentState1 = GamePad.GetState(PlayerIndex.One);

            // player 2
            PreviousState2 = CurrentState2;
            CurrentState2 = GamePad.GetState(PlayerIndex.Two);

            // player 3
            PreviousState3 = CurrentState3;
            CurrentState3 = GamePad.GetState(PlayerIndex.Three);

            // player 4
            PreviousState4 = CurrentState4;
            CurrentState4 = GamePad.GetState(PlayerIndex.Four);
        }
        #endregion

        #region Buttons

        /// <summary>
        /// If the keys are down.
        /// </summary>
        /// <param name="button">The button to check.</param>
        /// <returns>A bool array of 4 elements. If the element is true the button is down.</returns>
        public static bool[] IsButtonDowns(Buttons button)
        {
            bool[] downs = new bool[4];
            downs[0] = CurrentState1.IsButtonDown(button);
            downs[1] = CurrentState2.IsButtonDown(button);
            downs[2] = CurrentState3.IsButtonDown(button);
            downs[3] = CurrentState4.IsButtonDown(button);
            return downs;
        }

        /// <summary>
        /// If the keys are down.
        /// </summary>
        /// <param name="button">The button to check.</param>
        /// <returns>A bool array of 4 elements. If the element is true the button is up.</returns>
        public static bool[] IsButtonUps(Buttons button)
        {
            bool[] ups = new bool[4];
            ups[0] = CurrentState1.IsButtonUp(button);
            ups[1] = CurrentState2.IsButtonUp(button);
            ups[2] = CurrentState3.IsButtonUp(button);
            ups[3] = CurrentState4.IsButtonUp(button);
            return ups;
        }

        /// <summary>
        /// If the button is down.
        /// </summary>
        /// <param name="button">The button to check.</param>
        /// <param name="index">The player index.</param>
        /// <returns>True if the button is down.</returns>
        public static bool IsButtonDown(Buttons button, PlayerIndex index)
        {
            switch (index)
            {
                case PlayerIndex.One:
                    return CurrentState1.IsButtonDown(button);
                case PlayerIndex.Two:
                    return CurrentState2.IsButtonDown(button);
                case PlayerIndex.Three:
                    return CurrentState3.IsButtonDown(button);
                case PlayerIndex.Four:
                    return CurrentState4.IsButtonDown(button);
            }
            return false;
        }

        /// <summary>
        /// If the button is up.
        /// </summary>
        /// <param name="button">The button to check.</param>
        /// <param name="index">The player index.</param>
        /// <returns>True if the button is up.</returns>
        public static bool IsButtonUp(Buttons button, PlayerIndex index)
        {
            switch (index)
            {
                case PlayerIndex.One:
                    return CurrentState1.IsButtonUp(button);
                case PlayerIndex.Two:
                    return CurrentState2.IsButtonUp(button);
                case PlayerIndex.Three:
                    return CurrentState3.IsButtonUp(button);
                case PlayerIndex.Four:
                    return CurrentState4.IsButtonUp(button);
            }
            return false;
        }

        /// <summary>
        /// If a button has been clicked.
        /// </summary>
        /// <param name="button">The button to check.</param>
        /// <param name="index">The player index.</param>
        /// <returns>True if the button was pressed this frame and released the last frame.</returns>
        public static bool WasButtonClicked(Buttons button, PlayerIndex index)
        {
            switch (index)
            {
                case PlayerIndex.One:
                    return PreviousState1.IsButtonUp(button) && CurrentState1.IsButtonDown(button);
                case PlayerIndex.Two:
                    return PreviousState2.IsButtonUp(button) && CurrentState2.IsButtonDown(button);
                case PlayerIndex.Three:
                    return PreviousState3.IsButtonUp(button) && CurrentState3.IsButtonDown(button);
                case PlayerIndex.Four:
                    return PreviousState4.IsButtonUp(button) && CurrentState4.IsButtonDown(button);
            }
            return false;
        }

        /// <summary>
        /// If the button is pressed.
        /// </summary>
        /// <param name="button">The button to check.</param>
        /// <param name="index">The player index.</param>
        /// <returns>True if the button was pressed this frame and last frame.</returns>
        public static bool IsButtonHeld(Buttons button, PlayerIndex index)
        {
            switch (index)
            {
                case PlayerIndex.One:
                    return PreviousState1.IsButtonDown(button) && CurrentState1.IsButtonDown(button);
                case PlayerIndex.Two:
                    return PreviousState2.IsButtonDown(button) && CurrentState2.IsButtonDown(button);
                case PlayerIndex.Three:
                    return PreviousState3.IsButtonDown(button) && CurrentState3.IsButtonDown(button);
                case PlayerIndex.Four:
                    return PreviousState4.IsButtonDown(button) && CurrentState4.IsButtonDown(button);
            }
            return false;
        }

        /// <summary>
        /// If the controller is connected.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>True if the controller is connected.</returns>
        public static bool IsConnected(PlayerIndex index)
        {
            switch (index)
            {
                case PlayerIndex.One:
                    return CurrentState1.IsConnected;
                case PlayerIndex.Two:
                    return PreviousState2.IsConnected;
                case PlayerIndex.Three:
                    return PreviousState3.IsConnected;
                case PlayerIndex.Four:
                    return PreviousState4.IsConnected;
            }
            return false;
        }

        #endregion

        #region Vibration
        /// <summary>
        /// Vibrate the controller.
        /// </summary>
        /// <remarks>
        /// Clamping is enforced so feel free to make a crazy high number or crazy low number for 0 and 1 ;)
        /// </remarks>
        /// <param name="index">The player index.</param>
        /// <param name="left">The left controller vibration amount. Value between 0 and 1.</param>
        /// <param name="right">The right controller vibration amount. Value between 0 and 1.</param>
        public static void Vibrate(PlayerIndex index, float left, float right)
        {
            GamePad.SetVibration(index, left.Clamp(0, 1), right.Clamp(0, 1));
        }

#if XBOXONE
        /// <summary>
        /// Vibrate the controller.
        /// </summary>
        /// <remarks>
        /// Clamping is enforced so feel free to make a crazy high number or crazy low number for 0 and 1 ;)
        /// 
        /// This function is Xbox One only.
        /// </remarks>
        /// <param name="index">The player index.</param>
        /// <param name="left">The left motor vibration amount. Value between 0 and 1.</param>
        /// <param name="right">The right motor vibration amount. Value between 0 and 1.</param>
        /// <param name="leftTrigger">The left trigger vibration amount. Value between 0 and 1.</param>
        /// <param name="rightTrigger">The right trigger vibration amount. Value between 0 and 1.</param>
        public static void Vibrate(PlayerIndex index, float left, float right, float leftTrigger, float rightTrigger)
        {
            GamePad.SetVibration(index, left.Clamp(0, 1), right.Clamp(0, 1), leftTrigger.Clamp(0, 1), rightTrigger.Clamp(0, 1));
        }
#endif

        #endregion

        #region Capabilities
        /// <summary>
        /// Get the gamepad capabilities.
        /// </summary>
        /// <param name="index">The player index.</param>
        /// <returns>The capabilities of the gamepad.</returns>
        public static GamePadCapabilities GetCapabilities(PlayerIndex index)
        {
            return GamePad.GetCapabilities(index);
        }
        #endregion

        #region Trigger
        /// <summary>
        /// Get the left trigger value.
        /// </summary>
        /// <param name="index">The player index.</param>
        /// <returns>The value of the trigger between 0 and 1.</returns>
        public static float GetLeftTrigger(PlayerIndex index)
        {
            switch (index)
            {
                case PlayerIndex.One:
                    return CurrentState1.Triggers.Left;
                case PlayerIndex.Two:
                    return CurrentState2.Triggers.Left;
                case PlayerIndex.Three:
                    return CurrentState3.Triggers.Left;
                case PlayerIndex.Four:
                    return CurrentState4.Triggers.Left;
            }
            return 0;
        }

        /// <summary>
        /// Get the right trigger value.
        /// </summary>
        /// <param name="index">The player index.</param>
        /// <returns>The value of the trigger between 0 and 1.</returns>
        public static float GetRightTrigger(PlayerIndex index)
        {
            switch (index)
            {
                case PlayerIndex.One:
                    return CurrentState1.Triggers.Right;
                case PlayerIndex.Two:
                    return CurrentState2.Triggers.Right;
                case PlayerIndex.Three:
                    return CurrentState3.Triggers.Right;
                case PlayerIndex.Four:
                    return CurrentState4.Triggers.Right;
            }
            return 0;
        }
        #endregion

        #region Thumbsticks

        /// <summary>
        /// Gets the position of the left thumbstick.
        /// </summary>
        /// <param name="index">The player index.</param>
        /// <returns>The position of the thumbstick.</returns>
        public static Vector2 GetLeftThumbstick(PlayerIndex index)
        {
            switch (index)
            {
                case PlayerIndex.One:
                    return CurrentState1.ThumbSticks.Left;
                case PlayerIndex.Two:
                    return CurrentState2.ThumbSticks.Left;
                case PlayerIndex.Three:
                    return CurrentState3.ThumbSticks.Left;
                case PlayerIndex.Four:
                    return CurrentState4.ThumbSticks.Left;
            }
            return Vector2.Zero;
        }

        /// <summary>
        /// Gets the position of the right thumbstick.
        /// </summary>
        /// <param name="index">The player index.</param>
        /// <returns>The position of the thumbstick.</returns>
        public static Vector2 GetRightThumbstick(PlayerIndex index)
        {
            switch (index)
            {
                case PlayerIndex.One:
                    return CurrentState1.ThumbSticks.Right;
                case PlayerIndex.Two:
                    return CurrentState2.ThumbSticks.Right;
                case PlayerIndex.Three:
                    return CurrentState3.ThumbSticks.Right;
                case PlayerIndex.Four:
                    return CurrentState4.ThumbSticks.Right;
            }
            return Vector2.Zero;
        }

        #endregion

    }
}
