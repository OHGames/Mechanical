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
        #region Variables

        /// <summary>
        /// The list of current controller states.
        /// </summary>
        public static GamePadState[] CurrentStates { get; private set; }

        /// <summary>
        /// The list of previous controller states.
        /// </summary>
        public static GamePadState[] PreviousStates { get; private set; }

        /// <summary>
        /// How long each button has been held.
        /// </summary>
        public static Dictionary<Buttons, float>[] TimeButtonsHeld { get; set; }

        /// <summary>
        /// The max amount of controllers there are.
        /// </summary>
        public static int MaxControllerCount { get => GamePad.MaximumGamePadCount; }
        #endregion

        public static void Initialize()
        {
            CurrentStates = new GamePadState[MaxControllerCount];
            PreviousStates = new GamePadState[MaxControllerCount];

            TimeButtonsHeld = new Dictionary<Buttons, float>[MaxControllerCount];

            for (int i = 0; i < MaxControllerCount; i++)
            {
                TimeButtonsHeld[i] = new Dictionary<Buttons, float>();
            }

        }

        #region Update
        public static void Update(float deltaTime)
        {
            for (int i = 0; i < MaxControllerCount; i++)
            {
                PreviousStates[i] = CurrentStates[i];
                CurrentStates[i] = GamePad.GetState(i);
            }

            // loop through all states
            for (int i = 0; i < MaxControllerCount; i++)
            {
                // loop through buttons.
                // https://dotnethow.net/iterate-through-an-enumeration-enum-in-c/
                foreach (Buttons button in Enum.GetValues(typeof(Buttons)))
                {
                    if (IsButtonDown(button, i))
                    {
                        if (TimeButtonsHeld[i].ContainsKey(button))
                        {
                            // increment.
                            TimeButtonsHeld[i][button] = TimeButtonsHeld[i][button] + deltaTime;
                        }
                        else
                        {
                            // add to list
                            TimeButtonsHeld[i].Add(button, 0);
                        }
                    }
                    else
                    {
                        if (TimeButtonsHeld[i].ContainsKey(button))
                        {
                            TimeButtonsHeld[i].Remove(button);
                        }
                    }
                }
            }

        }
        #endregion

        #region Buttons

        /// <summary>
        /// Gets how long a button has been held for.
        /// </summary>
        /// <param name="button">The button to check.</param>
        /// <param name="index">The player index.</param>
        /// <returns>The amount of time a button has been held.</returns>
        public static float GetTimeHeld(Buttons button, PlayerIndex index)
        {
            if (TimeButtonsHeld[(int)index].ContainsKey(button)) return TimeButtonsHeld[(int)index][button];
            else return 0;
        }

        /// <summary>
        /// Gets how long a button has been held for.
        /// </summary>
        /// <param name="button">The button to check.</param>
        /// <param name="index">The player index.</param>
        /// <returns>The amount of time a button has been held.</returns>
        public static float GetTimeHeld(Buttons button, int index)
        {
            if (TimeButtonsHeld[index].ContainsKey(button)) return TimeButtonsHeld[index][button];
            else return 0;
        }

        /// <summary>
        /// If the keys are down.
        /// </summary>
        /// <param name="button">The button to check.</param>
        /// <returns>A bool array of 4 elements. If the element is true the button is down.</returns>
        public static bool[] IsButtonDowns(Buttons button)
        {
            bool[] downs = new bool[MaxControllerCount];
            for (int i = 0; i < MaxControllerCount; i++)
            {
                downs[i] = CurrentStates[i].IsButtonDown(button);
            }
            return downs;
        }

        /// <summary>
        /// If the keys are down.
        /// </summary>
        /// <param name="button">The button to check.</param>
        /// <returns>A bool array of 4 elements. If the element is true the button is up.</returns>
        public static bool[] IsButtonUps(Buttons button)
        {
            bool[] ups = new bool[MaxControllerCount];
            for (int i = 0; i < MaxControllerCount; i++)
            {
                ups[i] = CurrentStates[i].IsButtonUp(button);
            }
            return ups;
        }

        /// <summary>
        /// If the button is down.
        /// </summary>
        /// <param name="button">The button to check.</param>
        /// <param name="index">The player index.</param>
        /// <returns>True if the button is down and controller is connected.</returns>
        public static bool IsButtonDown(Buttons button, PlayerIndex index)
        {
            return CurrentStates[(int)index].IsConnected && CurrentStates[(int)index].IsButtonDown(button);
        }

        /// <summary>
        /// If the button is down.
        /// </summary>
        /// <param name="button">The button to check.</param>
        /// <param name="index">The player index.</param>
        /// <returns>True if the button is down and controller is conneced.</returns>
        public static bool IsButtonDown(Buttons button, int index)
        {
            return CurrentStates[index].IsConnected && CurrentStates[index].IsButtonDown(button);
        }


        /// <summary>
        /// If the button is up.
        /// </summary>
        /// <param name="button">The button to check.</param>
        /// <param name="index">The player index.</param>
        /// <returns>True if the button is up and controller is connected and controller was connected both frames.</returns>
        public static bool IsButtonUp(Buttons button, PlayerIndex index)
        {
            return CurrentStates[(int)index].IsConnected && CurrentStates[(int)index].IsButtonUp(button);
        }

        /// <summary>
        /// If the button is up.
        /// </summary>
        /// <param name="button">The button to check.</param>
        /// <param name="index">The player index.</param>
        /// <returns>True if the button is up and controller is connected and controller was connected both frames.</returns>
        public static bool IsButtonUp(Buttons button, int index)
        {
            return CurrentStates[index].IsConnected && CurrentStates[index].IsButtonUp(button);
        }

        /// <summary>
        /// If a button has been clicked.
        /// </summary>
        /// <param name="button">The button to check.</param>
        /// <param name="index">The player index.</param>
        /// <returns>True if the button was pressed this frame and released the last frame and controller was connected both frames.</returns>
        public static bool WasButtonClicked(Buttons button, PlayerIndex index)
        {
            return CurrentStates[(int)index].IsConnected && PreviousStates[(int)index].IsConnected && PreviousStates[(int)index].IsButtonUp(button) && CurrentStates[(int)index].IsButtonDown(button);
        }

        /// <summary>
        /// If a button has been clicked.
        /// </summary>
        /// <param name="button">The button to check.</param>
        /// <param name="index">The player index.</param>
        /// <returns>True if the button was pressed this frame and released the last frame and controller was connected both frames.</returns>
        public static bool WasButtonClicked(Buttons button, int index)
        {
            return CurrentStates[index].IsConnected && PreviousStates[index].IsConnected && PreviousStates[index].IsButtonUp(button) && CurrentStates[index].IsButtonDown(button);
        }

        /// <summary>
        /// If the button is pressed.
        /// </summary>
        /// <param name="button">The button to check.</param>
        /// <param name="index">The player index.</param>
        /// <returns>True if the button was pressed this frame and last frame and controller was connected both frames.</returns>
        public static bool IsButtonHeld(Buttons button, PlayerIndex index)
        {
            return CurrentStates[(int)index].IsConnected && PreviousStates[(int)index].IsConnected && PreviousStates[(int)index].IsButtonDown(button) && CurrentStates[(int)index].IsButtonDown(button);
        }

        /// <summary>
        /// If the button is pressed.
        /// </summary>
        /// <param name="button">The button to check.</param>
        /// <param name="index">The player index.</param>
        /// <returns>True if the button was pressed this frame and last frame and controller was connected both frames.</returns>
        public static bool IsButtonHeld(Buttons button, int index)
        {
            return CurrentStates[index].IsConnected && PreviousStates[index].IsConnected && PreviousStates[index].IsButtonDown(button) && CurrentStates[index].IsButtonDown(button);
        }

        /// <summary>
        /// If the controller is connected.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>True if the controller is connected.</returns>
        public static bool IsConnected(PlayerIndex index)
        {
            return CurrentStates[(int)index].IsConnected;
        }

        /// <summary>
        /// If the controller is connected.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>True if the controller is connected.</returns>
        public static bool IsConnected(int index)
        {
            return CurrentStates[index].IsConnected;
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
            Vibrate(index, left, right, 0, 0);
        }

        /// <summary>
        /// Vibrate the controller.
        /// </summary>
        /// <remarks>
        /// Clamping is enforced so feel free to make a crazy high number or crazy low number for 0 and 1 ;)
        /// </remarks>
        /// <param name="index">The player index.</param>
        /// <param name="left">The left controller vibration amount. Value between 0 and 1.</param>
        /// <param name="right">The right controller vibration amount. Value between 0 and 1.</param>
        public static void Vibrate(int index, float left, float right)
        {
            Vibrate(index, left, right, 0, 0);
        }

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
        /// <param name="leftTrigger">(XBOX ONE ONLY)The left trigger vibration amount. Value between 0 and 1.</param>
        /// <param name="rightTrigger">(XBOX ONE ONLY)The right trigger vibration amount. Value between 0 and 1.</param>
        public static void Vibrate(PlayerIndex index, float left, float right, float leftTrigger, float rightTrigger)
        {
            GamePad.SetVibration(index, left.Clamp(0, 1), right.Clamp(0, 1), leftTrigger.Clamp(0, 1), rightTrigger.Clamp(0, 1));
        }

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
        /// <param name="leftTrigger">(XBOX ONE ONLY)The left trigger vibration amount. Value between 0 and 1.</param>
        /// <param name="rightTrigger">(XBOX ONE ONLY)The right trigger vibration amount. Value between 0 and 1.</param>
        public static void Vibrate(int index, float left, float right, float leftTrigger, float rightTrigger)
        {
            GamePad.SetVibration(index, left.Clamp(0, 1), right.Clamp(0, 1), leftTrigger.Clamp(0, 1), rightTrigger.Clamp(0, 1));
        }

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

        /// <summary>
        /// Get the gamepad capabilities.
        /// </summary>
        /// <param name="index">The player index.</param>
        /// <returns>The capabilities of the gamepad.</returns>
        public static GamePadCapabilities GetCapabilities(int index)
        {
            return GamePad.GetCapabilities(index);
        }
        #endregion

        #region Trigger
        /// <summary>
        /// Get the left trigger value.
        /// </summary>
        /// <param name="index">The player index.</param>
        /// <returns>The value of the trigger between 0 and 1. Returns 0 if controller is not connected.</returns>
        public static float GetLeftTrigger(PlayerIndex index)
        {
            bool connected = CurrentStates[(int)index].IsConnected;
            return connected ? CurrentStates[(int)index].Triggers.Left : 0;
        }

        /// <summary>
        /// Get the left trigger value.
        /// </summary>
        /// <param name="index">The player index.</param>
        /// <returns>The value of the trigger between 0 and 1. Returns 0 if controller is not connected.</returns>
        public static float GetLeftTrigger(int index)
        {
            bool connected = CurrentStates[index].IsConnected;
            return connected ? CurrentStates[index].Triggers.Left : 0;
        }

        /// <summary>
        /// Get the right trigger value.
        /// </summary>
        /// <param name="index">The player index.</param>
        /// <returns>The value of the trigger between 0 and 1. Returns 0 if not connected.</returns>
        public static float GetRightTrigger(PlayerIndex index)
        {
            bool connected = CurrentStates[(int)index].IsConnected;
            return connected ? CurrentStates[(int)index].Triggers.Right : 0;
        }

        /// <summary>
        /// Get the right trigger value.
        /// </summary>
        /// <param name="index">The player index.</param>
        /// <returns>The value of the trigger between 0 and 1. Returns 0 if not connected.</returns>
        public static float GetRightTrigger(int index)
        {
            bool connected = CurrentStates[index].IsConnected;
            return connected ? CurrentStates[index].Triggers.Right : 0;
        }
        #endregion

        #region Thumbsticks

        /// <summary>
        /// Gets the position of the left thumbstick.
        /// </summary>
        /// <param name="index">The player index.</param>
        /// <returns>The position of the thumbstick. Returns 0 vector if not connected.</returns>
        public static Vector2 GetLeftThumbstick(PlayerIndex index)
        {
            bool connected = CurrentStates[(int)index].IsConnected;
            return connected ? CurrentStates[(int)index].ThumbSticks.Left : Vector2.Zero;
        }


        /// <summary>
        /// Gets the position of the left thumbstick.
        /// </summary>
        /// <param name="index">The player index.</param>
        /// <returns>The position of the thumbstick. Returns 0 vector if not connected.</returns>
        public static Vector2 GetLeftThumbstick(int index)
        {
            bool connected = CurrentStates[index].IsConnected;
            return connected ? CurrentStates[index].ThumbSticks.Left : Vector2.Zero;
        }

        /// <summary>
        /// Gets the position of the right thumbstick.
        /// </summary>
        /// <param name="index">The player index.</param>
        /// <returns>The position of the thumbstick. Returns 0 vector if not connected.</returns>
        public static Vector2 GetRightThumbstick(PlayerIndex index)
        {
            bool connected = CurrentStates[(int)index].IsConnected;
            return connected ? CurrentStates[(int)index].ThumbSticks.Right : Vector2.Zero;
        }

        /// <summary>
        /// Gets the position of the right thumbstick.
        /// </summary>
        /// <param name="index">The player index.</param>
        /// <returns>The position of the thumbstick. Returns 0 vector if not connected.</returns>
        public static Vector2 GetRightThumbstick(int index)
        {
            bool connected = CurrentStates[index].IsConnected;
            return connected ? CurrentStates[index].ThumbSticks.Right : Vector2.Zero;
        }

        #endregion

    }
}
