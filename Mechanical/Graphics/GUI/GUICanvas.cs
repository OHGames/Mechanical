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
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Mechanical
{
    [DataContract]
    /// <summary>
    /// The GUI Canvas is the manager for all things UI.
    /// 
    /// <para>
    /// The Canvas is updated and connected to the current scene. All of the UI in the canvas are similar to entities, due to the similarities in rendering, animating, and positioning.
    /// </para>
    /// </summary>
    public class GUICanvas
    {
        [DataMember]
        /// <summary>
        /// A list of the elements in this canvas.
        /// </summary>
        public GUIElementList Elements { get; set; } = new GUIElementList();

        /// <summary>
        /// The scene that will be updating the UI.
        /// </summary>
        public Scene AttachedScene { get; set; }

        /// <summary>
        /// All UI will be rendered to a render target. It will then be rendered to the <see cref="Screen.RenderTarget"/>.
        /// </summary>
        public RenderTarget2D RenderTarget { get; private set; }

        [DataMember]
        /// <summary>
        /// If the canvas is visible.
        /// </summary>
        public bool Visible { get; set; } = true;

        [DataMember]
        /// <summary>
        /// If the canvas is updating.
        /// </summary>
        public bool Paused { get; set; } = false;

        [DataMember]
        /// <summary>
        /// If the canvas will draw or update.
        /// </summary>
        public bool Active { get; set; } = true;

        [DataMember]
        /// <summary>
        /// The transparency of the canvas. This will be changed when the render target is drawn.
        /// </summary>
        public float Transparency { get => transparency; set => transparency = value.Clamp(0, 255); }

        /// <summary>
        /// The transparency.
        /// </summary>
        private float transparency = 256;

        [DataMember]
        /// <summary>
        /// The render order of the canvas.
        /// </summary>
        public int RenderOrder { get; set; }

        public GUICanvas(Scene scene)
        {
            AttachedScene = scene;
        }

        public GUICanvas(Scene scene, params GUIElement[] elements) : this(scene)
        {
            Elements.Add(elements);
        }

        public void Initailize()
        {
            Elements.Initialize();
        }

        public void LoadContent(ContentManager content)
        {
            Elements.LoadContent(content);
        }

        public void Update(float deltaTime)
        {
            if (Active && !Paused)
            {
                Elements.Update(deltaTime);
            }
        }

        public void Draw()
        {
            if (Active && Visible)
            {
                Elements.Draw();
            }
        }

        public void DebugDraw(bool editorRender)
        {
            if (Active && Visible)
            {
                Elements.DebugDraw(editorRender);

                Drawing.DrawRectangle(Engine.Instance.GraphicsDevice.Viewport.Bounds, Color.Red, 2);
            }

        }

    }
}
