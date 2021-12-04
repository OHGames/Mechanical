/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O.H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/USED_CODE_LICENSES.txt for more info.
 * 
 * Thanks to this tutorial series: https://manbeardgames.com/docs/tutorials/monogame-3-8/scene-transitions/introduction
 * Some changes made.
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// The scene transition is a class that will change the way the screen is drawn when changing scenes.
    /// </summary>
    public abstract class SceneTransition : IDisposable
    {

        /// <summary>
        /// If the transition is animating.
        /// </summary>
        public bool Transitioning { get; set; }

        /// <summary>
        /// The type of transition.
        /// </summary>
        public TransitionType Type { get; set; }

        /// <summary>
        /// The time it takes to transition.
        /// </summary>
        public TimeSpan TransitionTime { get; set; }

        /// <summary>
        /// The time that is left in the transition.
        /// </summary>
        public TimeSpan TimeLeft { get; set; }

        /// <summary>
        /// When the transition is done.
        /// </summary>
        public event Action OnComplete = delegate { };

        /// <summary>
        /// The render target to draw to.
        /// </summary>
        public RenderTarget2D RenderTarget { get; set; }

        /// <summary>
        /// The render target that the scene draws to.
        /// </summary>
        public RenderTarget2D SceneRenderTarget { get; set; }

        /// <summary>
        /// The color to clear the render target with.
        /// </summary>
        public Color ClearColor { get; set; }

        public SceneTransition(TransitionType type, TimeSpan time, Color color)
        {
            Type = type;
            TimeLeft = TransitionTime = time;

            ClearColor = color;
        }

        /// <summary>
        /// Starts the transition
        /// </summary>
        /// <param name="renderTarget">The scene's render target.</param>
        public void Start(RenderTarget2D renderTarget)
        {
            SceneRenderTarget = renderTarget;
            Transitioning = true;
        }

        public virtual void Initialize()
        {
            RenderTarget = new RenderTarget2D(Engine.Instance.GraphicsDevice,
                Engine.Instance.GraphicsDevice.PresentationParameters.BackBufferWidth,
                Engine.Instance.GraphicsDevice.PresentationParameters.BackBufferHeight);
        }

        public virtual void LoadContent(ContentManager content)
        {

        }

        public virtual void Update(float deltaTime)
        {
            TimeLeft -= TimeSpan.FromMilliseconds(deltaTime);

            if (TimeLeft <= TimeSpan.Zero)
            {
                Transitioning = false;
                OnComplete.Invoke();
            }

        }

        /// <summary>
        /// Start the drawing of the batch.
        /// </summary>
        /// <param name="color">The clear color.</param>
        public virtual void BeginDraw()
        {
            Engine.Instance.GraphicsDevice.SetRenderTarget(RenderTarget);
            Engine.Instance.GraphicsDevice.Clear(ClearColor);
        }

        /// <summary>
        /// Draw the transition.
        /// </summary>
        /// <param name="color">The clear color.</param>
        public virtual void Draw()
        {
            BeginDraw();
            Render();
            EndDraw();
        }

        /// <summary>
        /// This function actually does the drawing.
        /// </summary>
        public abstract void Render();

        /// <summary>
        /// Stops draing the transition.
        /// </summary>
        public virtual void EndDraw()
        {
            Engine.Instance.GraphicsDevice.SetRenderTarget(null);
        }

        public virtual void Dispose()
        {
            if (RenderTarget != null)
                RenderTarget.Dispose();
        }

        public virtual void OnGraphicsDeviceCreated()
        {
            RenderTarget = new RenderTarget2D(Engine.Instance.GraphicsDevice,
                Engine.Instance.GraphicsDevice.PresentationParameters.BackBufferWidth,
                Engine.Instance.GraphicsDevice.PresentationParameters.BackBufferHeight);
        }

        public virtual void OnGraphicsDeviceReset()
        {
            RenderTarget = new RenderTarget2D(Engine.Instance.GraphicsDevice,
                Engine.Instance.GraphicsDevice.PresentationParameters.BackBufferWidth,
                Engine.Instance.GraphicsDevice.PresentationParameters.BackBufferHeight);
        }
    }

    /// <summary>
    /// The type of transition that is currently happening.
    /// </summary>
    public enum TransitionType
    {
        /// <summary>
        /// When the screen goes from blocked to visible.
        /// </summary>
        In,

        /// <summary>
        /// When the screen goes from visible to blocked.
        /// </summary>
        Out
    }

}
