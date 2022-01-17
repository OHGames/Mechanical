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
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace Mechanical
{
    /// <summary>
    /// The console renderer.
    /// </summary>
    public class ConsoleWindow
    {

        /// <summary>
        /// The string that will be added before the command.
        /// </summary>
        public const string Prefix = "> ";

        /// <summary>
        /// The size of the output.
        /// </summary>
        public Vector2 OutputSize { get; private set; }

        /// <summary>
        /// The size of the input.
        /// </summary>
        public Vector2 InputSize { get; private set; }

        /// <summary>
        /// The color of the output window.
        /// </summary>
        public Color OutputColor { get => outputColor * OutputTransparency; }

        /// <summary>
        /// The transparency of the output window.
        /// </summary>
        public float OutputTransparency { get; private set; } = 0.65f;

        /// <summary>
        /// The color of the output window.
        /// </summary>
        private Color outputColor = Color.Black;

        /// <summary>
        /// The color of the input window.
        /// </summary>
        public Color InputColor { get => inputColor * InputTransparency; }

        /// <summary>
        /// The transparency of the input window.
        /// </summary>
        public float InputTransparency { get; private set; } = 0.7f;

        /// <summary>
        /// The color of the input window.
        /// </summary>
        private Color inputColor = Color.Black;

        /// <summary>
        /// If the cursor should be rendered.
        /// </summary>
        private bool renderCursor = true;

        /// <summary>
        /// The character(s) to be used as a cursor.
        /// </summary>
        public const string Cursor = "|";

        /// <summary>
        /// How fast, in seconds, the cursor should blink.
        /// </summary>
        public float CursorBlinkSpeed = 0.5f;

        /// <summary>
        /// How long it has been in seconds since the cursor blinked.
        /// </summary>
        public float timeSinceBlink = 0;

        /// <summary>
        /// The position of the input text.
        /// </summary>
        private Vector2 inputPosition;

        /// <summary>
        /// The amount of space from the side of the screen.
        /// </summary>
        private float textPadding = 25;

        /// <summary>
        /// How much space is in between each line of output text.
        /// </summary>
        private float lineHeight = 40;

        /// <summary>
        /// The font to render the text with.
        /// </summary>
        private SpriteFont font;

        /// <summary>
        /// The scroll.
        /// </summary>
        private float scroll = 0;

        /// <summary>
        /// If we can scroll.
        /// </summary>
        private bool canScroll = false;

        /// <summary>
        /// The maximum length of a string before it is wrapped.
        /// </summary>
        public int MaxCharacterCount { get; private set; }

        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("Mechanical/console font");
        }

        public void Update(float deltaTime)
        {
            if (!Console.IsTyping && !Console.WasTyping)
            {
                if (timeSinceBlink >= CursorBlinkSpeed)
                {
                    timeSinceBlink = 0;
                    renderCursor = !renderCursor;
                }
                else
                {
                    timeSinceBlink += deltaTime;
                }
            }
            else
            {
                timeSinceBlink = 0;
                renderCursor = true;
            }

            // change size with window change.
            // The height will be half of the window.
            float halfY = Engine.Instance.Screen.ScreenHeight / 2f;
            OutputSize = new Vector2(Engine.Instance.Screen.ScreenWidth, halfY);
            // the input size.
            InputSize = new Vector2(Engine.Instance.Screen.ScreenWidth, 50);
            inputPosition = new Vector2(0, halfY);

            canScroll = Console.Output.Count * lineHeight > OutputSize.Y;

            if (canScroll)
            {
                scroll += MechMouse.ScrollDelta / 2;
                scroll = scroll.Clamp((-Console.Output.Count * lineHeight) + lineHeight * 5 /* << this adds padding */ , 0);
            }

            var charWidth = font.MeasureString("A").X;
            MaxCharacterCount = (int)(OutputSize.X - textPadding) / (int)charWidth;
        }

        public void Draw()
        {
            // draw the output window.
            Drawing.Draw(Drawing.Pixel, new Rectangle(Engine.Instance.Screen.Viewport.X, Engine.Instance.Screen.Viewport.Y, (int)OutputSize.X, (int)OutputSize.Y), OutputColor * OutputTransparency);

            // draw the input window.
            Drawing.Draw(Drawing.Pixel, new Rectangle(Engine.Instance.Screen.Viewport.X + (int)inputPosition.X, Engine.Instance.Screen.Viewport.Y + (int)inputPosition.Y, (int)InputSize.X, (int)InputSize.Y), InputColor * InputTransparency);

            // draw the prefix
            Drawing.DrawString(font, Prefix, new Vector2(textPadding, inputPosition.Y + font.MeasureString(Prefix).Y / 2), Color.Yellow);

            // todo fix output

            List<ConsoleLine> invert = new List<ConsoleLine>(Console.Output);
            invert.Reverse();

            // draw the output text.
            for (int i = 0; i < invert.Count; i++)
            {
                ConsoleLine cLine = invert[i];
                string line = cLine.Text;
                Color color = cLine.Color;


                // get location of bottom then subtract by the line, then offset by input, then offset by scroll, then offset by padding so scrolled text is not inside input box.
                Vector2 pos = new Vector2(textPadding, OutputSize.Y - (i * lineHeight) - InputSize.Y - scroll - 5 /* padding */);
                // add tab if wrapped.
                if (cLine.IsWrappedLine) pos.X += font.MeasureString(" ").X * 4; 

                if (pos.Y < OutputSize.Y)
                    Drawing.DrawString(font, line, pos, color);
            }

            string caretedText = "";

            if (Console.CurrentInput != "")
            {
                if (!renderCursor)
                    caretedText = Console.CurrentInput.Insert(Console.caretPosition, " ");
                else
                    caretedText = Console.CurrentInput.Insert(Console.caretPosition, Cursor);
            }
            else
            {
                if (renderCursor)
                    caretedText = Cursor;
            }

            // draw input text.
            Drawing.DrawString(font, caretedText, new Vector2(textPadding + font.MeasureString(Prefix).X, 
                inputPosition.Y + font.MeasureString(Prefix).Y / 2f), Color.Yellow);

        }

    }
}
