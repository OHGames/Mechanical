/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O. H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/USED_CODE_LICENSES.txt for more info.
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// This class returns a value from -1 to 1 based on the input.
    /// </summary>
    public class MechAxis
    {
        #region Variables

        /// <summary>
        /// The key that will be used to get a negative value.
        /// </summary>
        public Keys NegativeKey { get; private set; }

        /// <summary>
        /// The key that will be used to get a positive value.
        /// </summary>
        public Keys PositiveKey { get; private set; }

        /// <summary>
        /// An alternative key that will get a negative value.
        /// </summary>
        public Keys AlternativeNegativeKey { get; private set; }

        /// <summary>
        /// An alternative key that will get a positive value.
        /// </summary>
        public Keys AlternativePositiveKey { get; private set; }

        /// <summary>
        /// A button that will get a negative value.
        /// </summary>
        public Buttons NegativeButton { get; private set; }

        /// <summary>
        /// A button that will get a positive value.
        /// </summary>
        public Buttons PositiveButton { get; private set; }

        /// <summary>
        /// An alternate button that will get a negative value.
        /// </summary>
        public Buttons AlternativeNegativeButton { get; private set; }

        /// <summary>
        /// An alternate button that will get a positive value.
        /// </summary>
        public Buttons AlternativePositiveButton { get; private set; }

        /// <summary>
        /// If the axis is using a keyboard and a controller(s).
        /// </summary>
        private bool usingBoth;

        /// <summary>
        /// If using only a keyboard.
        /// </summary>
        private bool usingKeyboard;

        /// <summary>
        /// The direction of the controller.
        /// </summary>
        public MechControllerAxisDirection Direction { get; set; }

        /// <summary>
        /// The axis to get.
        /// </summary>
        public MechControllerAxes Axis { get; set; }

        /// <summary>
        /// How much a key or button needs to be pressed in order to get registered.
        /// </summary>
        public float Deadzone { get; set; } = 0.15f;

        #endregion

        #region Constructors

        /// <summary>
        /// Makes a new mech axis with keyboard input only.
        /// </summary>
        /// <param name="negative">The negative key.</param>
        /// <param name="positive">The positive key.</param>
        /// <param name="altNeg">The alternate negative key.</param>
        /// <param name="altPos">The alternate positive key.</param>
        public MechAxis(Keys negative, Keys positive, Keys altNeg, Keys altPos)
        {
            NegativeKey = negative;
            PositiveKey = positive;
            AlternativeNegativeKey = altNeg;
            AlternativePositiveKey = altPos;
            usingKeyboard = true;
        }

        /// <summary>
        /// Makes a new mech axis with controller input only.
        /// </summary>
        /// <param name="negative">The negative button.</param>
        /// <param name="positive">The positive button.</param>
        /// <param name="altNeg">The alternate negative button.</param>
        /// <param name="altPos">The alternate positive button.</param>
        ///<param name="axes">The type of thumbstick or trigger to use.</param>
        /// <param name="direction">The direction of the controller to use as the axis.</param>
        public MechAxis(Buttons negative, Buttons positive, Buttons altNeg, Buttons altPos, MechControllerAxisDirection direction, MechControllerAxes axes)
        {
            NegativeButton = negative;
            PositiveButton = positive;
            AlternativeNegativeButton = altNeg;
            AlternativePositiveButton = altPos;
            usingKeyboard = false;
            Direction = direction;
            Axis = axes;
        }

        /// <summary>
        /// Make a new axis with a controller and a keyboard.
        /// </summary>
        /// <param name="negative">The negative key.</param>
        /// <param name="positive">The positive key.</param>
        /// <param name="altNeg">The alternate negative key.</param>
        /// <param name="altPos">The alternate positive key.</param>
        /// <param name="negativeB">The negative button.</param>
        /// <param name="positiveB">The positive button.</param>
        /// <param name="altNegB">The alternate negative button.</param>
        /// <param name="altPosB">The alternate positive button.</param>
        /// <param name="axes">The type of thumbstick or trigger to use.</param>
        /// <param name="direction">The direction of the controller to use as the axis.</param>
        public MechAxis(Keys negative, Keys positive, Keys altNeg, Keys altPos, Buttons negativeB, Buttons positiveB, Buttons altNegB, Buttons altPosB, MechControllerAxisDirection direction, MechControllerAxes axes) : this(negative, positive, altNeg, altPos)
        {
            NegativeButton = negativeB;
            PositiveButton = positiveB;
            AlternativeNegativeButton = altNegB;
            AlternativePositiveButton = altPosB;
            usingBoth = true;
            Direction = direction;
            Axis = axes;
        }

        #endregion

        #region GetAxis
        /// <summary>
        /// Gets the axis.
        /// </summary>
        /// <returns>A float with the </returns>
        /// https://github.com/Yeti47/Yetibyte.Himalaya/blob/master/Yetibyte.Himalaya/Controls/GameControlAxis.cs
        public float GetAxis(int index = 1)
        {
            bool isPosKey = false;
            bool isNegKey = false;
            if (usingKeyboard || usingBoth)
            {
                // positive down.
                if (PositiveKey != 0 && NegativeKey != 0 && MechKeyboard.IsKeyDown(PositiveKey) && MechKeyboard.IsKeyUp(NegativeKey))
                {
                    isPosKey = true;
                }
                // positive down.
                else if (AlternativePositiveKey != 0 && AlternativeNegativeKey != 0 && MechKeyboard.IsKeyDown(AlternativePositiveKey) && MechKeyboard.IsKeyUp(AlternativeNegativeKey))
                {
                    isPosKey = true;
                }
                // negative down.
                if (PositiveKey != 0 && NegativeKey != 0 && MechKeyboard.IsKeyDown(NegativeKey) && MechKeyboard.IsKeyUp(PositiveKey))
                {
                    isNegKey = true;
                }
                else if (AlternativePositiveKey != 0 && AlternativeNegativeKey != 0 && MechKeyboard.IsKeyDown(AlternativeNegativeKey) && MechKeyboard.IsKeyUp(AlternativePositiveKey))
                {
                    isNegKey = true;
                }
            }

            bool isPosCon = false;
            bool isNegCon = false;

            // controller.
            if (!usingKeyboard || usingBoth)
            {
                // pos
                if (PositiveButton != 0 
                    && MechController.IsButtonDown(PositiveButton, index) 
                    && NegativeButton != 0 
                    && MechController.IsButtonUp(NegativeButton, index))
                {
                    isPosCon = true;
                }
                // alt pos
                else if (AlternativePositiveButton != 0 
                    && MechController.IsButtonDown(AlternativePositiveButton, index) 
                    && AlternativeNegativeButton != 0 
                    && MechController.IsButtonUp(AlternativeNegativeButton, index))
                {
                    isPosCon = true;
                }

                // neg
                if (AlternativePositiveButton != 0 
                    && MechController.IsButtonDown(NegativeButton, index) 
                    && AlternativeNegativeButton != 0 
                    && MechController.IsButtonUp(PositiveButton, index))
                {
                    isNegCon = true;
                }
                // alt neg
                else if (AlternativePositiveButton != 0 
                    && MechController.IsButtonDown(AlternativeNegativeButton, index) 
                    && AlternativeNegativeButton != 0 
                    && MechController.IsButtonDown(AlternativePositiveButton, index))
                {
                    isNegCon = true;
                }
            }

            float keyValue = 0;

            if (isPosKey || isPosCon) keyValue += 1;
            else if (isNegKey || isNegCon) keyValue -= 1;

            float conAxisValue = 0;

            if (!usingKeyboard || usingBoth)
            {
                switch (Axis)
                {
                    case MechControllerAxes.None:
                        break;
                    case MechControllerAxes.LeftThumbstick:

                        if (Direction == MechControllerAxisDirection.Horizontal) conAxisValue = MechController.GetLeftThumbstick(index).X;
                        else if (Direction == MechControllerAxisDirection.Vertical) conAxisValue = MechController.GetLeftThumbstick(index).Y;
                        break;

                    case MechControllerAxes.RightThumbstick:
                        if (Direction == MechControllerAxisDirection.Horizontal) conAxisValue = MechController.GetRightThumbstick(index).X;
                        else if (Direction == MechControllerAxisDirection.Vertical) conAxisValue = MechController.GetRightThumbstick(index).Y;
                        break;
                    case MechControllerAxes.LeftTrigger:
                        conAxisValue = MechController.GetLeftTrigger(index);
                        break;
                    case MechControllerAxes.RightTrigger:
                        conAxisValue = MechController.GetRightTrigger(index);
                        break;
                    default:
                        break;
                }
            }

            float potentialVal = Math.Abs(keyValue) > Math.Abs(conAxisValue) ? keyValue : conAxisValue;

            return potentialVal >= Deadzone ? potentialVal : 0;

        }
        #endregion
    }

    /// <summary>
    /// An enum that contains the direction that will be looked for on the <see cref="MechAxis"/>.
    /// </summary>
    /// https://github.com/Yeti47/Yetibyte.Himalaya/blob/master/Yetibyte.Himalaya/Controls
    public enum MechControllerAxisDirection
    {
        Horizontal, Vertical
    }

    /// <summary>
    /// An enum that contains the different types of game pad controls that return different values.
    /// </summary>
    /// https://github.com/Yeti47/Yetibyte.Himalaya/blob/master/Yetibyte.Himalaya/Controls
    public enum MechControllerAxes
    {
        None, LeftThumbstick, RightThumbstick, LeftTrigger, RightTrigger
    }

}
