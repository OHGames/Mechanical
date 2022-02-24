/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O. H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/USED_CODE_LICENSES.txt for more info.
 */

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// The animation event is an event that will trigger when a certain frame(s) is shown.
    /// </summary>
    [DataContract]
    public struct SpriteAnimationEvent
    {

        /// <summary>
        /// The frames. NULL by default!
        /// </summary>
        [DataMember]
        public int[] FramesToHook { get; set; }

        /// <summary>
        /// The name of the event.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

    }
}
