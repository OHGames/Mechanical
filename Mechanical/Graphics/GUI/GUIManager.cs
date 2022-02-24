using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization;

namespace Mechanical
{
    /// <summary>
    /// The GUI Manager handles updating and drawing <see cref="GUICanvas"/>es.
    /// 
    /// TODO: make a IGameLikeList
    /// </summary>
    [DataContract]
    public class GUIManager : IGameFunctions
    {
        /// <summary>
        /// The canvases that are currently being updated. When the list is added or removed from, the list os reordered by render order, making sure that the higher numbers render last.
        /// </summary>
        [DataMember]
        public List<GUICanvas> Canvases { get; set; } = new List<GUICanvas>();

        /// <summary>
        /// The render target that the manager will render to.
        /// </summary>
        public RenderTarget2D RenderTarget { get; set; }

        /// <summary>
        /// The width of the gui.
        /// </summary>
        public int TargetWitdh { get; set; } = Engine.Instance.GraphicsDeviceManager.PreferredBackBufferWidth;

        /// <summary>
        /// The height of the gui.
        /// </summary>
        public int TargetHeight { get; set; } = Engine.Instance.GraphicsDeviceManager.PreferredBackBufferHeight;

        /// <summary>
        /// The canvases to remove.
        /// </summary>
        private Queue<GUICanvas> toRemove = new Queue<GUICanvas>();

        /// <summary>
        /// The canvases to add.
        /// </summary>
        private Queue<GUICanvas> toAdd = new Queue<GUICanvas>();

        /// <summary>
        /// If it is safe to add or remove a canvas.
        /// </summary>
        private bool isSafe = true;

        /// <summary>
        /// Adds a canvas to the manager.
        /// </summary>
        /// <param name="canvas"></param>
        public void AddCanvas(GUICanvas canvas)
        {
            if (isSafe)
            {
                Canvases.Add(canvas);
            }
            else
            {
                toAdd.Enqueue(canvas);
                Canvases.OrderBy(c => c.RenderOrder);
            }
        }

        /// <summary>
        /// Remove the canvas from the manager.
        /// </summary>
        /// <param name="canvas">The canvas to add.</param>
        public void RemoveCanvas(GUICanvas canvas)
        {
            if (isSafe)
                Canvases.Remove(canvas);
            else
                toRemove.Enqueue(canvas);
        }

        public void Initialize()
        {
            RenderTarget = new RenderTarget2D(Engine.Instance.GraphicsDevice, TargetWitdh, TargetHeight);

            for (int i = 0; i < Canvases.Count; i++)
            {
                Canvases[i].Initailize();
            }

        }

        public void Draw()
        {
            if (Canvases.Count > 0)
            {
                GraphicsDevice d = Engine.Instance.GraphicsDevice;

                d.SetRenderTarget(RenderTarget);
                d.Clear(Color.Transparent);

                for (int i = 0; i < Canvases.Count; i++)
                {
                    Canvases[i].Draw();
                }

                d.SetRenderTarget(null);

            }
        }

        public void LoadContent(ContentManager content)
        {
            for (int i = 0; i < Canvases.Count; i++)
            {
                Canvases[i].LoadContent(content);
            }
        }

        public void Update(float deltaTime)
        {

            isSafe = false;

            for (int i = 0; i < toAdd.Count; i++)
            {
                Canvases.Add(toAdd.Dequeue());
                Canvases.OrderBy(c => c.RenderOrder);
            }

            for (int i = 0; i < toRemove.Count; i++)
            {
                Canvases.Remove(toRemove.Dequeue());
                Canvases.OrderBy(c => c.RenderOrder);
            }

            for (int i = 0; i < Canvases.Count; i++)
            {
                Canvases[i].Update(deltaTime);
            }

            isSafe = true;
        }

        public virtual void OnGraphicsDeviceCreated(object sender, System.EventArgs e)
        {
            RenderTarget = new RenderTarget2D(Engine.Instance.GraphicsDevice, TargetWitdh, TargetHeight);
        }

        public virtual void OnGraphicsDeviceReset(object sender, System.EventArgs e)
        {
            RenderTarget = new RenderTarget2D(Engine.Instance.GraphicsDevice, TargetWitdh, TargetHeight);
        }

        public void DebugDraw(bool editorRender)
        {
            for (int i = 0; i < Canvases.Count; i++)
            {
                Canvases[i].DebugDraw(editorRender);
            }
        }
    }
}
