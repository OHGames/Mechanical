using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// A keybind holds data on the keys or buttons needed to trigger.
    /// </summary>
    public struct Keybind
    {

        /// <summary>
        /// The keys needed to be pressed down in order to trigger.
        /// </summary>
        public List<Keys> Keys { get; set; }

        /// <summary>
        /// The keys that are also used to trigger.
        /// </summary>
        public List<Keys> AlternateKeys { get; set; }

        /// <summary>
        /// The buttons needed to trigger.
        /// </summary>
        public List<Buttons> Buttons { get; set; } 

        /// <summary>
        /// The buttons that are also used to trigger.
        /// </summary>
        public List<Buttons> AlternateButtons { get; set; }

        /// <summary>
        /// If all required keys are pressed.
        /// </summary>
        public bool IsKeyboardDown { get; private set; }

        /// <summary>
        /// If all required buttons are pressed.
        /// </summary>
        public bool IsControllerDown { get; private set; }

        /// <summary>
        /// If all required buttons and keys are pressed.
        /// </summary>
        public bool IsBothDown { get; private set; }

        /// <summary>
        /// If some of the needed keys are down.
        /// </summary>
        public bool IsSomeDown { get; private set; }

        /// <summary>
        /// If the keybind uses the keyboard.
        /// </summary>
        public bool UsingKeyboard { get; private set; }

        /// <summary>
        /// If the keybind will use both a keyboard, and a controller. 
        /// <para>
        /// Note: The controller will not influence the keys. The keys and controller will be checked seperatly. The <see cref="IsKeyboardDown"/> is if the keyboard is down OR the controller.
        /// </para>
        /// </summary>
        public bool UsingBoth { get; private set; }

        /// <summary>
        /// The player index that this keybind will be attached to.
        /// </summary>
        public PlayerIndex PlayerIndex { get; set; }

        /// <summary>
        /// How long the keybind has been held for (game time).
        /// </summary>
        public float TimeHeld { get; private set; }

        public Keybind(Keys[] keys, Keys[] alternate)
        {
            IsKeyboardDown = false;
            IsSomeDown = false;
            Keys = keys.ToList();
            AlternateKeys = alternate.ToList();
            Buttons = new List<Buttons>();
            AlternateButtons = new List<Buttons>();
            UsingBoth = false;
            UsingKeyboard = true;
            IsBothDown = false;
            IsControllerDown = false;
            PlayerIndex = PlayerIndex.One;
            TimeHeld = 0;
        }

        public Keybind(List<Keys> keys, List<Keys> alternate) : this(keys.ToArray(), alternate.ToArray())
        {
        }

        public Keybind(Keys[] keys, Keys[] altKeys, Buttons[] buttons, Buttons[] altButtons, PlayerIndex index = PlayerIndex.One) : this(keys, altKeys)
        {
            AlternateButtons = altButtons.ToList();
            Buttons = buttons.ToList();
            UsingBoth = true;
            PlayerIndex = index;
        }

        public Keybind(List<Keys> keys, List<Keys> altKeys, List<Buttons> buttons, List<Buttons> altButtons, PlayerIndex index = PlayerIndex.One) : this(keys, altKeys)
        {
            AlternateButtons = altButtons;
            Buttons = buttons;
            UsingBoth = true;
            PlayerIndex = index;
        }

        public Keybind(Buttons[] buttons, Buttons[] alternate, PlayerIndex index = PlayerIndex.One)
        {
            IsKeyboardDown = false;
            IsSomeDown = false;
            Keys = new List<Keys>();
            AlternateKeys = new List<Keys>();
            Buttons = buttons.ToList();
            AlternateButtons = alternate.ToList();
            UsingBoth = false;
            UsingKeyboard = false;
            IsBothDown = false;
            IsControllerDown = false;
            PlayerIndex = PlayerIndex.One;
            TimeHeld = 0;
        }

        public Keybind(List<Buttons> buttons, List<Buttons> alternate, PlayerIndex index = PlayerIndex.One) : this(buttons.ToArray(), alternate.ToArray(), index)
        {

        }

        public void Update(float deltaTime)
        {
            int keysDownCount = 0;
            int buttonsDownCount = 0;

            // how many keys or buttons need to be down for both to happen.
            int countNeededForBoth = Keys.Count + Buttons.Count;

            if (UsingKeyboard || UsingBoth)
            {
                for (int i = 0; i < Keys.Count; i++)
                {
                    Keys k = Keys[i];

                    if (MechKeyboard.IsKeyDown(k))
                    {
                        keysDownCount++;
                        IsSomeDown = true;
                        continue;
                    }
                }

                if (keysDownCount == Keys.Count)
                {
                    IsKeyboardDown = true;
                    IsSomeDown = true;
                }

                // check for alternate keys.
                // remember, the alternates just make up for other keys.
                if (!IsKeyboardDown && IsSomeDown)
                {
                    for (int i = 0; i < AlternateKeys.Count; i++)
                    {
                        Keys k = AlternateKeys[i];

                        if (MechKeyboard.IsKeyDown(k))
                        {
                            keysDownCount++;
                            continue;
                        }
                    }
                }

                // if enough alts are used.
                if (keysDownCount >= Keys.Count)
                {
                    IsKeyboardDown = true;
                    IsSomeDown = true;
                }
            }

            if (!UsingKeyboard || UsingBoth)
            {
                for (int i = 0; i < Buttons.Count; i++)
                {
                    Buttons b = Buttons[i];
                    if (MechController.IsButtonDown(b, PlayerIndex))
                    {
                        buttonsDownCount++;
                        IsSomeDown = true;
                        continue;
                    }

                }
                // check if all down.
                if (buttonsDownCount == Buttons.Count)
                {
                    IsControllerDown = true;
                }


                // check alts
                if (IsSomeDown && !IsControllerDown)
                {
                    for (int i = 0; i < AlternateButtons.Count; i++)
                    {
                        Buttons b = Buttons[i];

                        if (MechController.IsButtonDown(b, PlayerIndex))
                        {
                            buttonsDownCount++;
                            IsSomeDown = true;
                            continue;
                        }
                    }
                }

                if (buttonsDownCount >= Buttons.Count)
                {
                    IsControllerDown = true;
                }
            }

            if (UsingBoth)
            {
                IsBothDown = keysDownCount + buttonsDownCount >= countNeededForBoth;
            }

            if (IsBothDown || IsControllerDown || IsKeyboardDown)
            {
                TimeHeld += deltaTime;
            }
            else
            {
                TimeHeld = 0;
            }
        }

    }
}
