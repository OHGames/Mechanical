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
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// The screen is a class used to handle scaling and rendering of the game's render target.
    /// </summary>
    [DataContract]
    public class Screen
    {
        /// <summary>
        /// The width of the render target.
        /// </summary>
        [DataMember]
        public int TargetWidth { get; set; }

        /// <summary>
        /// The height of the render target.
        /// </summary>
        [DataMember]
        public int TargetHeight { get; set; }

        /// <summary>
        /// The width of the render target when it is being drawn onto the screen.
        /// </summary>
        [DataMember]
        public int ScreenWidth { get; set; }

        /// <summary>
        /// The height of the render target when it is being drawn onto the screen.
        /// </summary>
        [DataMember]
        public int ScreenHeight { get; set; }

        /// <summary>
        /// The render target to draw.
        /// </summary>
        public RenderTarget2D RenderTarget { get; set; }

        /// <summary>
        /// The color to clear the back buffer to when drawing the screen.
        /// </summary>
        [DataMember]
        public Color ClearColor { get; set; } = Color.Black;

        /// <summary>
        /// The matrix representing the transformation of the screen.
        /// </summary>
        public Matrix ScreenMatrix { get; set; }

        /// <summary>
        /// The viewport.
        /// </summary>
        public Viewport Viewport { get; set; }

        public Screen(int width, int height)
        {
            TargetWidth = width;
            TargetHeight = height;

            MakeRenderTarget();
        }

        /// <summary>
        /// Remake the render target.
        /// </summary>
        public void MakeRenderTarget()
        {
            RenderTarget = new RenderTarget2D(Engine.Instance.GraphicsDevice, TargetWidth, TargetHeight);
        }

        /// <summary>
        /// Get the new screen.
        /// </summary>
        /// <param name="engine">A reference to the engine.</param>
        //https://youtu.be/yUSB_wAVtE8
        private void GetScreen(Engine engine)
        {

            float windowAspect = engine.GraphicsDevice.PresentationParameters.BackBufferWidth / 
                (float)engine.GraphicsDevice.PresentationParameters.BackBufferHeight;

            float targetAspect = TargetWidth / (float)TargetHeight;

            float x = 0, y = 0;
            float w = engine.GraphicsDevice.PresentationParameters.BackBufferWidth;
            float h = engine.GraphicsDevice.PresentationParameters.BackBufferHeight;


            if (windowAspect > targetAspect)
            {
                w = (h * targetAspect);
                x = (engine.GraphicsDevice.PresentationParameters.BackBufferWidth - w) / 2;
            }
            else if (windowAspect < targetAspect)
            {
                h = (w / targetAspect);
                y = (engine.GraphicsDevice.PresentationParameters.BackBufferHeight - h) / 2f;
            }

            ScreenMatrix = Matrix.CreateScale(ScreenWidth / (float)TargetWidth) * Matrix.CreateTranslation(new Vector3(x, y, 0));

            ScreenWidth = (int)w;
            ScreenHeight = (int)h;


            //var a = ScreenWidth / (float)ScreenHeight;
            //ScreenWidth -= Padding * 2;
            //ScreenHeight -= (int)(a * Padding * 2);

            Viewport = new Viewport()
            {
                X = (int)x,
                Y = (int)y,
                Width = ScreenWidth,
                Height = ScreenHeight,
                MinDepth = 0,
                MaxDepth = 1
            };

            // fix padding

        }

        /// <summary>
        /// Draw the screen.
        /// </summary>
        /// <param name="engine">A reference to the engine.</param>
        public void Draw(Engine engine)
        {
            // clear target.
            engine.GraphicsDevice.SetRenderTarget(null);

            //DrawToTarget();

            engine.GraphicsDevice.Clear(ClearColor);

            GetScreen(engine);

            // call this one because Drawing uses default draw method.
            engine.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            Drawing.Draw(RenderTarget, new Rectangle(Viewport.X, Viewport.Y, ScreenWidth, ScreenHeight), Color.White);
            Drawing.End();

        }

    }
}
