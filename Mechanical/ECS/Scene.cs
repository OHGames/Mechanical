/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O.H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/USED_CODE_LICENSES.txt for more info.
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
    [DataContract]
    /// <summary>
    /// The scene is a container for <see cref="Entity"/>s.
    /// 
    /// <para>
    /// The scene is responsible for updating and drawing the entities.
    /// </para>
    /// </summary>
    public class Scene : IEnumerable<Entity>, IDisposable
    {
        [DataMember]
        /// <summary>
        /// The list of entities in the scene.
        /// </summary>
        public EntityList Entities { get; set; } = new EntityList();

        [DataMember]
        /// <summary>
        /// The name of the scene.
        /// </summary>
        public string Name { get; set; }

        [DataMember]
        /// <summary>
        /// The index of the scene.
        /// </summary>
        public int SceneIndex { get; set; }

        [DataMember]
        /// <summary>
        /// If the scene will update.
        /// </summary>
        public bool Paused { get; set; }

        [DataMember]
        /// <summary>
        /// If the scene is active. It is active it updates and draws.
        /// </summary>
        public bool IsActiveScene { get; set; }

        [DataMember]
        /// <summary>
        /// The color to clear the render target to.
        /// </summary>
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
        }

        public virtual void LoadContent(ContentManager content)
        {
            Entities.LoadContent(content);
        }

        public virtual void Update(float deltaTime)
        {
            Entities.Update(deltaTime);
        }

        /// <summary>
        /// Draw the scene to the render target. This will change the current target so make sure to put it back.
        /// </summary>
        public virtual void Draw()
        {
            Entities.Draw();
        }

        /// <summary>
        /// Draw the scene to the render target using the special draw method for specific entities.. This will change the current target so make sure to put it back.
        /// </summary>
        public virtual void DebugDraw(bool editorRender)
        {
            //engine.GraphicsDevice.SetRenderTarget(null);
            //engine.GraphicsDevice.SetRenderTarget(RenderTarget);
            //engine.GraphicsDevice.Clear(ClearColor);
            Entities.DebugDraw(editorRender);
            //engine.GraphicsDevice.SetRenderTarget(null);
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
        }

        public virtual void OnGraphicsDeviceReset(object sender, System.EventArgs e)
        {
            RenderTarget = new RenderTarget2D(engine.GraphicsDevice, TargetWitdh, TargetHeight);
        }

    }
}
