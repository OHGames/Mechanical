/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O. H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/USED_CODE_LICENSES.txt for more info.
 */

using Mechanical;
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
            // get the meta object.
            JObject meta = (JObject)mainObject["meta"];
            // get the tags.
            file.FrameTags = LoadFrameTags(meta);
            // get the layers.
            file.Layers = LoadLayers(meta);
            // get slices
            file.Slices = LoadSlices(meta);

            // return
            return file;
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

        /// <summary>
        /// Load the <see cref="AsepriteLayer"/>s.
        /// </summary>
        /// <param name="meta">The meta object.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="AsepriteLayer"/>s.</returns>
        private static List<AsepriteLayer> LoadLayers(JObject meta)
        {
            JArray layers = (JArray)meta["layers"];

            List<AsepriteLayer> list = new List<AsepriteLayer>();

            for (int i = 0; i < layers.Count; i++)
            {

                JObject layer = (JObject)layers[i];

                AsepriteLayer aLayer = new AsepriteLayer()
                {
                    BlendMode = (string)layer["blendMode"],
                    Name = (string)layer["name"],
                    Opacity = (int)layer["opacity"]
                };

                // get color.
                string color = (string)CheckIfValueExists("color", layer);
                aLayer.Color = ColorHelper.FromHexRGBA(color);

                aLayer.UserData = (string)CheckIfValueExists("data", layer);

                list.Add(aLayer);
            }

            return list;

        }

        /// <summary>
        /// Loads the slices.
        /// </summary>
        /// <param name="meta">The meta object.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="AsepriteSlice"/>s.</returns>
        private static List<AsepriteSlice> LoadSlices(JObject meta)
        {
            JArray slices = (JArray)meta["slices"];

            List<AsepriteSlice> list = new List<AsepriteSlice>();

            for (int i = 0; i < slices.Count; i++)
            {

                JObject slice = (JObject)slices[i];

                AsepriteSlice aSlice = new AsepriteSlice()
                {
                    Color = ColorHelper.FromHexRGBA((string)slice["color"]),
                    Name = (string)slice["name"],
                    UserData = (string)CheckIfValueExists((string)slice["data"], slice)
                };

                aSlice.Keys = LoadSliceKeys(slice);

                list.Add(aSlice);
            }

            return list;

        }

        /// <summary>
        /// Loads the keys for a slice.
        /// </summary>
        /// <param name="slice">The <see cref="JObject"/> of the <see cref="AsepriteSlice"/> to load from.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="AsepriteSliceKey"/>s.</returns>
        private static List<AsepriteSliceKey> LoadSliceKeys(JObject slice)
        {
            JArray keys = (JArray)slice["keys"];

            List<AsepriteSliceKey> list = new List<AsepriteSliceKey>();

            for (int i = 0; i < keys.Count; i++)
            {
                JObject key = (JObject)keys[i];

                AsepriteSliceKey aKey = new AsepriteSliceKey()
                {
                    Frame = (int)key["frame"],
                    Bounds = new Rectangle(
                        (int)key["bounds"]["x"],
                        (int)key["bounds"]["y"],
                        (int)key["bounds"]["w"],
                        (int)key["bounds"]["h"]
                    )
                };

                JObject pivot = (JObject)CheckIfValueExists("pivot", key);
                if (pivot != null)
                {
                    aKey.Pivot = new Vector2((float)pivot["x"], (float)pivot["y"]);
                }

                JObject nineSlice = (JObject)CheckIfValueExists("center", key);
                if (nineSlice != null)
                {
                    aKey.NineSlice = new Rectangle(
                        (int)nineSlice["x"],
                        (int)nineSlice["y"],
                        (int)nineSlice["w"],
                        (int)nineSlice["h"]
                    );
                }

                list.Add(aKey);

            }

            return list;

        }

        /// <summary>
        /// Check if the value exists.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <param name="obj">The object to get from.</param>
        /// <returns></returns>
        private static object CheckIfValueExists(string name, JObject obj)
        {
            try
            {
                return obj[name];
            }
            catch
            {
                return null;
            }
        }

    }
}
