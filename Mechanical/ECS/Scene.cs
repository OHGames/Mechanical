using System;
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
    public class Scene
    {

        /// <summary>
        /// The list of entities in the scene.
        /// </summary>
        public EntityList Entities { get; set; } = new EntityList();

    }
}
