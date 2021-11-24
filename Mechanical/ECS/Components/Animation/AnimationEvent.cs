using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// The animation event is an event that will trigger when a certain frame(s) is shown.
    /// </summary>
    public struct AnimationEvent
    {

        /// <summary>
        /// The frames. NULL by default!
        /// </summary>
        public int[] FramesToHook { get; set; }

        /// <summary>
        /// The name of the event.
        /// </summary>
        public string Name { get; set; }

    }
}
