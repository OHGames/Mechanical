/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O. H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/Use%20Code%20Licenses.md for more info.
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// This is an extension class for importing a <see cref="SpriteAnimation"/> from an aseprite animation file.
    /// </summary>
    public static class SpriteAnimationLoader
    {

        /// <summary>
        /// The extentions to use.
        /// </summary>
        public readonly static string[] Extensions = new string[]
        {
            ".json",
            ".mechanim"
        };

        /// <summary>
        /// Loads a <see cref="SpriteAnimation"/> from an Aseprite json spritesheet export.
        /// </summary>
        /// <param name="content">This is here just for the extension. Leave <c>null</c> if not calling through extension.</param>
        /// <param name="name">The name of the file. This is exactly the same format as <see cref="ContentManager.Load{T}(string)"/>.</param>
        /// <param name="animationName">The name of the animation tag to use when importing from an Aseprite animation.</param>
        /// <param name="frameSpeed">The speed of each frame on screen. Set to <c>-1</c> to grab from file.</param>
        /// <param name="loops">If the animation loops.</param>
        /// <returns>A new <see cref="SpriteAnimation"/>.</returns>
        /// <exception cref="FileNotFoundException">When file cant be found.</exception>
        public static SpriteAnimation LoadAnimation(this ContentManager content, 
            string name, 
            string animationName = "",
            float frameSpeed = -1,
            bool loops = true)
        {
            // get the content directory.
            string directory = Path.Combine(Environment.CurrentDirectory, content.RootDirectory);

            string file = "";

            for (int i = 0; i < Extensions.Length; i++)
            {
                string dir = Path.Combine(directory, name + Extensions[i]);
                // check for the file.
                if (File.Exists(dir))
                {
                    file = File.ReadAllText(dir);
                    break;
                }
            }

            // empty file.
            if (string.IsNullOrWhiteSpace(file)) throw new FileNotFoundException($"Cannot find file {name} of file is empty.");

            return ProcessJSON(file, animationName, frameSpeed, loops);
        }

        /// <summary>
        /// Loads all <see cref="SpriteAnimation"/>s from an Aseprite sprite sheet data file using the frame tags to seperate animations.
        /// </summary>
        /// <param name="content">This is here just for the extension. Leave <c>null</c> if not calling through extension.</param>
        /// <param name="name">The name of the file. This is exactly the same format as <see cref="ContentManager.Load{T}(string)"/>.</param>
        /// <param name="frameSpeed">The speed of each frame on screen. Set to <c>-1</c> to grab from file.</param>
        /// <param name="loops">If the animation loops.</param>
        /// <returns>A new dictionary where the dictionary key is the tag name and the value is the <see cref="SpriteAnimation"/>.</returns>
        /// <exception cref="FileNotFoundException">When file cant be found.</exception>
        public static Dictionary<string, SpriteAnimation> LoadAnimations(this ContentManager content,
            string name,
            float frameSpeed = -1,
            bool loops = true)
        {
            // get the content directory.
            string directory = Path.Combine(Environment.CurrentDirectory, content.RootDirectory);

            string file = "";

            for (int i = 0; i < Extensions.Length; i++)
            {
                string dir = Path.Combine(directory, name + Extensions[i]);
                // check for the file.
                if (File.Exists(dir))
                {
                    file = File.ReadAllText(dir);
                    break;
                }
            }
            // empty file.
            if (string.IsNullOrWhiteSpace(file)) throw new FileNotFoundException($"Cannot find file {name} of file is empty.");

            // parse to get the tags.
            JObject json = JObject.Parse(file);

            JArray tags = (JArray)json["meta"]["frameTags"];

            Dictionary<string, SpriteAnimation> animations = new Dictionary<string, SpriteAnimation>();

            for (int i = 0; i < tags.Count; i++)
            {
                // frame tag object.
                JObject frameTagData = (JObject)tags[i];
                string tagName = (string)frameTagData["name"];

                animations.Add(
                    tagName,
                    // get the sprite animation from the tag name.
                    ProcessJSON(file, tagName, frameSpeed, loops)
                );
            }

            return animations;

        }

        /// <summary>
        /// Process the json file.
        /// </summary>
        /// <param name="file">The file contents.</param>
        /// <param name="animationName">The name of the animation tag to get.</param>
        /// <param name="frameSpeed">The speed of the animation</param>
        /// <param name="loops">If the animation should loop.</param>
        /// <returns>A new sprite animation.</returns>
        private static SpriteAnimation ProcessJSON(
            string file, 
            string animationName = "",
            float frameSpeed = -1,
            bool loops = true)
        {
            JObject input = JObject.Parse(file);

            bool specificAnimation = !string.IsNullOrWhiteSpace(animationName);

            // get the frames.
            JArray frames = (JArray)input["frames"];

            int to = 0;
            int from = frames.Count - 1;
            string direction = "forward";
            float speed = frameSpeed;

            // get the animation from the tag.
            if (specificAnimation)
            {
                // the frame tags array.
                JArray frameTagArray = (JArray)input["meta"]["frameTags"];

                // if there is an animation with the name.
                bool foundAnimation = false;

                for (int i = 0; i < frameTagArray.Count; i++)
                {
                    // get the frame.
                    JObject jsonFrame = (JObject)frameTagArray[i];

                    // check the name and set data.
                    if ((string)jsonFrame["name"] == animationName)
                    {

                        to = (int)jsonFrame["to"];
                        from = (int)jsonFrame["from"];
                        direction = (string)jsonFrame["direction"];
                        foundAnimation = true;
                        break;
                    }
                    else
                    {
                        continue;
                    }

                }

                if (!foundAnimation) throw new Exception($"The animation tag, {animationName}, cannot be found.");

            }

            int totalFrames = (to + 1) - from;

            Rectangle[] rects = new Rectangle[totalFrames];

            for (int i = from; i <= to; i++)
            {
                JObject frameData = (JObject)frames[i];

                if (speed == -1)
                {
                    // get from first frame. Each frame can be different but Mechanical doesn't support this.
                    // divide by 1000 because the duration is in milliseconds.
                    speed = (int)frames[i]["duration"] / 1000f;
                }

                // we use i - from because if we have frames 1,2,3,4,5,6,7,8,9,10
                // and from = 5 and to = 9
                // and i = 5 we are at the start.
                // So i(5) - from(5) equals 0
                // if i = 6
                // we are at the next frame. So i(6) - from(5) equals 1, which is the index we are on.
                // I am so smart. My math teacher would be proud.
                rects[i - from] = new Rectangle(
                    (int)frameData["frame"]["x"],
                    (int)frameData["frame"]["y"],
                    (int)frameData["frame"]["w"],
                    (int)frameData["frame"]["h"]);
            }

            switch (direction)
            {
                // if the animation goes backward.
                case "backward":
                    rects = rects.Reverse().ToArray();
                    break;
                case "pingpong":
                    // trim the rects to that the frame is not held for longer.
                    List<Rectangle> trimmedRects = rects.ToList();
                    // get last one
                    trimmedRects.RemoveAt(trimmedRects.Count - 1);
                    // get first
                    trimmedRects.RemoveAt(0);
                    // concat
                    rects = rects.Concat(trimmedRects).ToArray();
                    break;
                // already forward
                case "forward":
                default:
                    break;
            }

            return new SpriteAnimation(rects)
            {
                FrameSpeed = speed,
                Loops = loops,
                Paused = false
            };

        }

    }
}
