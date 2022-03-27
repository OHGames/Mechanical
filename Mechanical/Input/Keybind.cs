/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O. H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/Use%20Code%20Licenses.md for more info.
 */

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
    public class Keybind
    {

        /// <summary>
        /// The keys needed to be pressed down in order to trigger.
        /// </summary>
        public List<Keys> Keys { get; set; } = new List<Keys>();

        /// <summary>
        /// The keys that are also used to trigger. The dictonary key is the key the alternate is mapped to. The Value is a list of keys that can replace the Key. 
        /// </summary>
        public Dictionary<Keys, Keys[]> AlternateKeys { get; set; } = new Dictionary<Keys, Keys[]>();

        /// <summary>
        /// The buttons needed to trigger.
        /// </summary>
        public List<Buttons> Buttons { get; set; } = new List<Buttons>();

        /// <summary>
        /// The buttons that are also used to trigger. The dictonary key is the button the alternate is mapped to. The Value is a list of buttons that can replace the Key.
        /// </summary>
        public Dictionary<Buttons, Buttons[]> AlternateButtons { get; set; } = new Dictionary<Buttons, Buttons[]>();

        /// <summary>
        /// If all required keys or buttons are pressed.
        /// </summary>
        public bool IsDown { get; private set; }

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
        /// </summary>
        //public bool UsingBoth { get; private set; }

        /// <summary>
        /// The player index that this keybind will be attached to.
        /// </summary>
        public PlayerIndex PlayerIndex { get; set; } = PlayerIndex.One;

        /// <summary>
        /// How long the keybind has been held for (game time).
        /// </summary>
        public float TimeHeld { get; private set; }

        /// <summary>
        /// An event that is called when the keybind is triggered.
        /// </summary>
        // https://codereview.stackexchange.com/a/1143
        // did not know events are null by default, thanks for info.
        // https://codereview.stackexchange.com/a/1147
        // thanks for: = delagate { }
        public event Action KeybindTriggered = delegate { };

        /// <summary>
        /// When the keybind is no longer on.
        /// </summary>
        // https://codereview.stackexchange.com/a/1143
        // did not know events are null by default, thanks for info.
        // https://codereview.stackexchange.com/a/1147
        // thanks for: = delagate { }
        public event Action KeybindUntriggered = delegate { };

        public Keybind(Keys[] keys, Dictionary<Keys, Keys[]> alternate = null)
        {
            Keys = keys.ToList();
            if (alternate != null)
                AlternateKeys = alternate;
            else
                AlternateKeys = new Dictionary<Keys, Keys[]>();
            UsingKeyboard = true;
        }

        public Keybind(List<Keys> keys, Dictionary<Keys, Keys[]> alternate) : this(keys.ToArray(), alternate)
        {
        }

        public Keybind(Buttons[] buttons, Dictionary<Buttons, Buttons[]> alternate = null, PlayerIndex index = PlayerIndex.One)
        {
            Buttons = buttons.ToList();
            if (alternate != null)
                AlternateButtons = alternate;
            else
                AlternateButtons = new Dictionary<Buttons, Buttons[]>();
            PlayerIndex = index;
        }

        public Keybind(List<Buttons> buttons, Dictionary<Buttons, Buttons[]> alternate, PlayerIndex index = PlayerIndex.One) : this(buttons.ToArray(), alternate, index)
        {

        }

        public void Update(float deltaTime)
        {
            int downCount = 0;

            var prevHeld = IsDown;

            if (UsingKeyboard)
            {
                for (int i = 0; i < Keys.Count; i++)
                {
                    Keys k = Keys[i];

                    if (MechKeyboard.IsKeyDown(k))
                    {
                        downCount++;
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
                                    downCount++;
                                    IsSomeDown = true;
                                    break;
                                }
                            }
                        }
                    }
                }


                // if enough alts are used and keys are down.
                if (downCount >= Keys.Count)
                {
                    // if triggered
                    if (!IsDown)
                    {
                        KeybindTriggered.Invoke();
                    }
                    IsDown = true;
                    IsSomeDown = true;
                }
                else
                {
                    if (IsDown)
                    {
                        KeybindUntriggered.Invoke();
                    }
                    IsDown = false;
                }
            }

            if (!UsingKeyboard)
            {
                for (int i = 0; i < Buttons.Count; i++)
                {
                    Buttons b = Buttons[i];
                    if (MechController.IsButtonDown(b, PlayerIndex))
                    {
                        downCount++;
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
                                    downCount++;
                                    IsSomeDown = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                // check if all down and alts.
                if (downCount >= Buttons.Count)
                {
                    if (!IsDown)
                    {
                        KeybindTriggered.Invoke();
                    }
                    IsDown = true;
                }
                else
                {
                    if (IsDown)
                    {
                        KeybindUntriggered.Invoke();
                    }
                    IsDown = false;
                }
            }

            if (IsDown)
            {
                // if we should add
                if (prevHeld)
                    TimeHeld += deltaTime;
                else
                    TimeHeld = 0;
            }
            else
            {
                TimeHeld = 0;
            }
        }

    }
}
