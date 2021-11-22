using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections;
using System.Collections.Generic;
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
    public class Scene : IEnumerable<Entity>
    {

        /// <summary>
        /// The list of entities in the scene.
        /// </summary>
        public EntityList Entities { get; set; } = new EntityList();

        /// <summary>
        /// The name of the scene.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The index of the scene.
        /// </summary>
        public int SceneIndex { get; set; }

        /// <summary>
        /// If the scene will update.
        /// </summary>
        public bool Paused { get; set; }

        /// <summary>
        /// If the scene is active. It is active it updates and draws.
        /// </summary>
        public bool IsActiveScene { get; set; }

        /// <summary>
        /// The color to clear the render target to.
        /// </summary>
        public Color ClearColor { get; set; } = Color.CornflowerBlue;

        public Scene(string name)
        {
            Name = name;
        }

        public virtual void Initialize()
        {
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

        public virtual void Draw()
        {
            Entities.Draw();
        }

        public virtual void DebugDraw()
        {
            Entities.DebugDraw();
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
    }
}
