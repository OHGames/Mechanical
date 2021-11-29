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
        /// The keys that are also used to trigger. The dictonary key is the key the alternate is mapped to. The Value is a list of keys that can replace the Key. 
        /// </summary>
        public Dictionary<Keys, Keys[]> AlternateKeys { get; set; }

        /// <summary>
        /// The buttons needed to trigger.
        /// </summary>
        public List<Buttons> Buttons { get; set; }

        /// <summary>
        /// The buttons that are also used to trigger. The dictonary key is the button the alternate is mapped to. The Value is a list of buttons that can replace the Key.
        /// </summary>
        public Dictionary<Buttons, Buttons[]> AlternateButtons { get; set; }

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

        public Keybind(Keys[] keys, Dictionary<Keys, Keys[]> alternate)
        {
            IsKeyboardDown = false;
            IsSomeDown = false;
            Keys = keys.ToList();
            AlternateKeys = alternate;
            Buttons = new List<Buttons>();
            AlternateButtons = new Dictionary<Buttons, Buttons[]>();
            UsingBoth = false;
            UsingKeyboard = true;
            IsBothDown = false;
            IsControllerDown = false;
            PlayerIndex = PlayerIndex.One;
            TimeHeld = 0;
        }

        public Keybind(List<Keys> keys, Dictionary<Keys, Keys[]> alternate) : this(keys.ToArray(), alternate)
        {
        }

        public Keybind(Keys[] keys, Dictionary<Keys, Keys[]> altKeys, Buttons[] buttons, Dictionary<Buttons, Buttons[]> altButtons, PlayerIndex index = PlayerIndex.One) : this(keys, altKeys)
        {
            AlternateButtons = altButtons;
            Buttons = buttons.ToList();
            UsingBoth = true;
            PlayerIndex = index;
        }

        public Keybind(List<Keys> keys, Dictionary<Keys, Keys[]> altKeys, List<Buttons> buttons, Dictionary<Buttons, Buttons[]> altButtons, PlayerIndex index = PlayerIndex.One) : this(keys, altKeys)
        {
            AlternateButtons = altButtons;
            Buttons = buttons;
            UsingBoth = true;
            PlayerIndex = index;
        }

        public Keybind(Buttons[] buttons, Dictionary<Buttons, Buttons[]> alternate, PlayerIndex index = PlayerIndex.One)
        {
            IsKeyboardDown = false;
            IsSomeDown = false;
            Keys = new List<Keys>();
            AlternateKeys = new Dictionary<Keys, Keys[]>();
            Buttons = buttons.ToList();
            AlternateButtons = alternate;
            UsingBoth = false;
            UsingKeyboard = false;
            IsBothDown = false;
            IsControllerDown = false;
            PlayerIndex = PlayerIndex.One;
            TimeHeld = 0;
        }

        public Keybind(List<Buttons> buttons, Dictionary<Buttons, Buttons[]> alternate, PlayerIndex index = PlayerIndex.One) : this(buttons.ToArray(), alternate, index)
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
                    else
                    {
                        // check alts
                        if (AlternateKeys.ContainsKey(k))
                        {
                            // get replacements.
                            Keys[] keys = AlternateKeys[k];
                            for (int j = 0; j < keys.Length; j++)
                            {
                                if (MechKeyboard.IsKeyDown(keys[j]))
                                {
                                    keysDownCount++;
                                    IsSomeDown = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (keysDownCount == Keys.Count)
                {
                    IsKeyboardDown = true;
                    IsSomeDown = true;
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
                    else
                    {
                        // check alts
                        if (AlternateButtons.ContainsKey(b))
                        {
                            // get replacements.
                            Buttons[] buttons = AlternateButtons[b];
                            for (int j = 0; j < buttons.Length; j++)
                            {
                                if (MechController.IsButtonDown(buttons[j], PlayerIndex))
                                {
                                    buttonsDownCount++;
                                    IsSomeDown = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                // check if all down.
                if (buttonsDownCount == Buttons.Count)
                {
                    IsControllerDown = true;
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
