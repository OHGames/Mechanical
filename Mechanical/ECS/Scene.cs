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
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// The scene is a container for <see cref="Entity"/>s.
    /// 
    /// <para>
    /// The scene is responsible for updating and drawing the entities.
    /// </para>
    /// </summary>
    [DataContract]
    public class Scene : IEnumerable<Entity>, IDisposable
    {
        /// <summary>
        /// The list of entities in the scene.
        /// </summary>
        [DataMember]
        public EntityList Entities { get; set; } = new EntityList();

        /// <summary>
        /// The name of the scene.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// The index of the scene.
        /// </summary>
        [DataMember]
        public int SceneIndex { get; set; }

        /// <summary>
        /// If the scene will update.
        /// </summary>
        [DataMember]
        public bool Paused { get; set; }

        /// <summary>
        /// If the scene is active. It is active it updates and draws.
        /// </summary>
        [DataMember]
        public bool IsActiveScene { get; set; } = true;

        /// <summary>
        /// The color to clear the render target to.
        /// </summary>
        [DataMember]
        public Color ClearColor { get; set; } = Color.CornflowerBlue;

        /// <summary>
        /// A render target for the scene to draw to.
        /// </summary>
        public RenderTarget2D RenderTarget { get; set; }

        /// <summary>
        /// A reference to the engine. 
        /// </summary>
        protected Engine engine = Engine.Instance;

        /// <summary>
        /// The width of the game window.
        /// </summary>
        public int TargetWitdh { get; set; }

        /// <summary>
        /// The height of the game window.
        /// </summary>
        public int TargetHeight { get; set; }

        /// <summary>
        /// This handles the GUI for this scene.
        /// </summary>
        [DataMember]
        public GUIManager GUI { get; set; } = new GUIManager();

        public Scene(string name)
        {
            Name = name;
            TargetWitdh = Engine.Instance.GameWidth;
            TargetHeight = Engine.Instance.GameHeight;
        }

        public virtual void Initialize()
        {
            RenderTarget = new RenderTarget2D(engine.GraphicsDevice, TargetWitdh, TargetHeight);
            Entities.Initialize();
            GUI.Initialize();
        }

        public virtual void LoadContent(ContentManager content)
        {
            Entities.LoadContent(content);
            GUI.LoadContent(content);
        }

        public virtual void Update(float deltaTime)
        {

            if (IsActiveScene && !Paused)
            {
                Entities.Update(deltaTime);
                GUI.Update(deltaTime);
            }

        }

        /// <summary>
        /// Draw the scene to the render target. This will change the current target so make sure to put it back.
        /// </summary>
        public virtual void Draw()
        {
            //engine.GraphicsDevice.SetRenderTarget(RenderTarget);
            //engine.GraphicsDevice.Clear(ClearColor);

            if (IsActiveScene)
            {
                Entities.Draw();

                // This will set a new target and then set to null. 
                //GUI.Draw();
            }

        }

        /// <summary>
        /// Draw the scene to the render target using the special draw method for specific entities.. This will change the current target so make sure to put it back.
        /// </summary>
        public virtual void DebugDraw(bool editorRender)
        {
            //engine.GraphicsDevice.SetRenderTarget(RenderTarget);
            //engine.GraphicsDevice.Clear(ClearColor);

            if (IsActiveScene)
            {
                Entities.DebugDraw(editorRender);

                // This will set a new target and then set to null. 
                //GUI.DebugDraw(editorRender);
            }
        }

        /// <summary>
        /// Shorthand to add entities.
        /// </summary>
        /// <param name="e"></param>
        public void Add(Entity e) => Entities.Add(e);

        /// <summary>
        /// Shorthand to remove entities.
        /// </summary>
        /// <param name="e"></param>
        public void Remove(Entity e) => Entities.Remove(e);

        public IEnumerator<Entity> GetEnumerator()
        {
            return Entities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Entities.GetEnumerator();
        }

        public virtual void Dispose()
        {
            if (RenderTarget != null) RenderTarget.Dispose();
        }

        public virtual void OnGraphicsDeviceCreated(object sender, System.EventArgs e)
        {
            RenderTarget = new RenderTarget2D(engine.GraphicsDevice, TargetWitdh, TargetHeight);
            GUI.OnGraphicsDeviceCreated(sender, e);
        }

        public virtual void OnGraphicsDeviceReset(object sender, System.EventArgs e)
        {
            RenderTarget = new RenderTarget2D(engine.GraphicsDevice, TargetWitdh, TargetHeight);
            GUI.OnGraphicsDeviceReset(sender, e);
        }

    }
}
