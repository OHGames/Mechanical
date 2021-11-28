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

        public Keys NegativeKey { get; private set; }

        public Keys PositiveKey { get; private set; }

        public Keys AlternativeNegativeKey { get; private set; }

        public Keys AlternativePositiveKey { get; private set; }

        public Buttons NegativeButton { get; private set; }

        public Buttons PositiveButton { get; private set; }

        public Buttons AlternativeNegativeButton { get; private set; }

        public Buttons AlternativePositiveButton { get; private set; }

        public PlayerIndex PlayerIndex { get; private set; }

        private bool usingBoth;

        private bool usingOnlyKeyboard;

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
            usingOnlyKeyboard = true;
        }

        /// <summary>
        /// Makes a new mech axis with controller input only.
        /// </summary>
        /// <param name="negative">The negative button.</param>
        /// <param name="positive">The positive button.</param>
        /// <param name="altNeg">The alternate negative button.</param>
        /// <param name="altPos">The alternate positive button.</param>
        public MechAxis(Buttons negative, Buttons positive, Buttons altNeg, Buttons altPos, PlayerIndex index)
        {
            NegativeButton = negative;
            PositiveButton = positive;
            AlternativeNegativeButton = altNeg;
            AlternativePositiveButton = altPos;
            usingOnlyKeyboard = false;
            PlayerIndex = index;
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
        public MechAxis(Keys negative, Keys positive, Keys altNeg, Keys altPos, Buttons negativeB, Buttons positiveB, Buttons altNegB, Buttons altPosB, PlayerIndex index) : this(negative, positive, altNeg, altPos)
        {
            NegativeButton = negativeB;
            PositiveButton = positiveB;
            AlternativeNegativeButton = altNegB;
            AlternativePositiveButton = altPosB;
            usingBoth = true;
            PlayerIndex = index;
        }

        /// <summary>
        /// Gets the axis.
        /// 
        /// TODO: refactor and make better.
        /// </summary>
        /// <returns></returns>
        public int GetAxisRaw()
        {
            if (usingOnlyKeyboard || usingBoth)
            {
                // if the negative is down and the positive is up.
                if (MechKeyboard.IsKeyDown(NegativeKey) && MechKeyboard.IsKeyUp(PositiveKey))
                {
                    return -1;
                }
                // if the negative is up and the positive is down.
                else if (MechKeyboard.IsKeyUp(NegativeKey) && MechKeyboard.IsKeyDown(PositiveKey))
                {
                    return 1;
                }
                // either none are down or both at same time. Check alternatives.
                else
                {
                    // if the negative is down and the positive is up.
                    if (MechKeyboard.IsKeyDown(AlternativeNegativeKey) && MechKeyboard.IsKeyUp(AlternativePositiveKey))
                    {
                        return -1;
                    }
                    // if the negative is up and the positive is down.
                    else if (MechKeyboard.IsKeyUp(AlternativeNegativeKey) && MechKeyboard.IsKeyDown(AlternativePositiveKey))
                    {
                        return 1;
                    }
                    // nothing
                    else
                    {
                        return 0;
                    }
                }
            }
            else if (!usingOnlyKeyboard || usingBoth)
            {
                // if the negative is down and the positive is up.
                if (MechController.IsButtonDown(NegativeButton, PlayerIndex) && MechController.IsButtonUp(PositiveButton, PlayerIndex))
                {
                    return -1;
                }
                // if the negative is up and the positive is down.
                else if (MechController.IsButtonUp(NegativeButton, PlayerIndex) && MechController.IsButtonDown(PositiveButton, PlayerIndex))
                {
                    return 1;
                }
                // either none are down or both at same time. Check alternatives.
                else
                {
                    // if the negative is down and the positive is up.
                    if (MechController.IsButtonDown(AlternativeNegativeButton, PlayerIndex) && MechController.IsButtonUp(AlternativePositiveButton, PlayerIndex))
                    {
                        return -1;
                    }
                    // if the negative is up and the positive is down.
                    else if (MechController.IsButtonUp(AlternativeNegativeButton, PlayerIndex) && MechController.IsButtonDown(AlternativePositiveButton, PlayerIndex))
                    {
                        return 1;
                    }
                    // nothing
                    else
                    {
                        return 0;
                    }
                }
            }
            else
            {
                return 0;
            }
        }

    }
}
