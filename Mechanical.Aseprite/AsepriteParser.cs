/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O. H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/USED_CODE_LICENSES.txt for more info.
 */

using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mehcanical.Aseprite
{
    /// <summary>
    /// The Aseprite parser takes the <see cref="JObject"/> povided by the <see cref="AsepriteFile"/> 
    /// and returns a new <see cref="AsepriteFile"/> from it.
    /// </summary>
    internal static class AsepriteParser
    {

        /// <summary>
        /// Parse the JSON.
        /// </summary>
        /// <param name="json">The JSON to parse.</param>
        /// <returns>A new <see cref="AsepriteFile"/>.</returns>
        public static AsepriteFile Parse(string json)
        {

            // parse the JSON.
            JObject mainObject = JObject.Parse(json);

            // make the new file.
            AsepriteFile file = new AsepriteFile();

            // get the frames first.
            file.Frames = LoadFrames(mainObject);

            file.FrameTags = LoadFrameTags((JObject)mainObject["meta"]);

        }

        /// <summary>
        /// Load the frames from the <see cref="JObject"/>.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="AsepriteFrame"/>s.</returns>
        private static List<AsepriteFrame> LoadFrames(JObject obj)
        {
            // get the "frames" array.
            JArray frames = (JArray)obj["frames"];

            List<AsepriteFrame> newList = new List<AsepriteFrame>();

            // loop through frames.
            for (int i = 0; i < frames.Count; i++)
            {
                // the frame object.
                JObject frame = (JObject)frames[i];

                AsepriteFrame aFrame = new AsepriteFrame()
                {
                    // the duration
                    FrameDuration = (int)frame["duration"],
                    // the frame.
                    FrameRect = new Rectangle(
                        (int)frame["frame"]["x"],
                        (int)frame["frame"]["y"],
                        (int)frame["frame"]["w"],
                        (int)frame["frame"]["h"]
                    ),
                    // the initial size of the frame.
                    SpriteSourceSize = new Rectangle(
                        (int)frame["spriteSourceSize"]["x"],
                        (int)frame["spriteSourceSize"]["y"],
                        (int)frame["spriteSourceSize"]["w"],
                        (int)frame["spriteSourceSize"]["h"]
                    )
                };

                newList.Add(aFrame);
            }

            return newList;

        }

        /// <summary>
        /// Load the <see cref="AsepriteFrameTag"/>s.
        /// </summary>
        /// <param name="meta">The meta object.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="AsepriteFrameTag"/>s.</returns>
        private static List<AsepriteFrameTag> LoadFrameTags(JObject meta)
        {
            // get the array.
            JArray array = (JArray)meta["frameTags"];
            List<AsepriteFrameTag> tags = new List<AsepriteFrameTag>();

            for (int i = 0; i < array.Count; i++)
            {
                // get object.
                JObject obj = (JObject)array[i];

                AsepriteFrameTag tag = new AsepriteFrameTag()
                {
                    // get the enum name.
                    Direction = (AsepriteFrameTagDirection)Enum.Parse(typeof(AsepriteFrameTagDirection), (string)obj["direction"], true),
                    Start = (int)obj["from"],
                    End = (int)obj["to"],
                };

                tags.Add(tag);

            }

            return tags;
        }

    }
}
