using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// This is a special transition used when you dont what a scene transition.
    /// </summary>
    public class NoTransition : SceneTransition
    {
        public NoTransition(TransitionType type, TimeSpan time, Color color) : base(type, time, color)
        {
            time = TimeSpan.FromSeconds(0);
        }

        public override void Render()
        {
            
        }
    }
}
