using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// A simple fade-in-fade-out transition.
    /// </summary>
    public sealed class FadeTransition : SceneTransition
    {

        float alpha = 0;

        public FadeTransition(TransitionType type, TimeSpan time, Color color) : base(type, time, color)
        {

            if (type == TransitionType.In) alpha = 1;
            else alpha = 0;

        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            if (Transitioning)
                ClearColor *= GetAlpha();
        }

        public override void Render()
        {

        }

        public float GetAlpha()
        {
            double timeRemaining = TimeLeft.TotalSeconds;

            if (Type == TransitionType.In)
            {
                return (float)(timeRemaining / TransitionTime.TotalSeconds);
            }
            else
            {
                return (float)(1 - (timeRemaining / TransitionTime.TotalSeconds));
            }

        }

    }
}
