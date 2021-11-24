using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// If a component wants to draw, it needs this class.
    /// </summary>
    public interface IDrawableComponent
    {

        /// <summary>
        /// The order for rendering. Lower number renders first.
        /// </summary>
        int RenderOrder { get; set; }

        void Draw();

    }
}
