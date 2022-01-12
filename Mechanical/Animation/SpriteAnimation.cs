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
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mechanical
{
    /// <summary>
    /// This is a class that will store data on animations.
    /// 
    /// <para>
    /// Animations hold data on how the frames should be moved.
    /// </para>
    /// </summary>
    public sealed class SpriteAnimation
    {

        /// <summary>
        /// The frames.
        /// </summary>
        public List<Rectangle> Frames { get; set; } = new List<Rectangle>();

        /// <summary>
        /// The width of each frame.
        /// </summary>
        public int FrameWidth { get; set; }

        /// <summary>
        /// The height of each frame.
        /// </summary>
        public int FrameHeight { get; set; }

        /// <summary>
        /// The length of time a frame is shown on screen.
        /// </summary>
        public float FrameSpeed { get; set; } = 0.1f;

        /// <summary>
        /// The frame order will allow a user to change the order in whitch frames are shown in.
        /// 
        /// <para>
        /// The index of a rectangle is it's psoition in the <see cref="Frames"/> list.
        /// So a rectangle at index 4 would be the 5th in the list.
        /// </para>
        /// </summary>
        public List<int> FrameOrder { get; set; } = new List<int>();

        /// <summary>
        /// The current frame (the index for the <see cref="FrameOrder"/>).
        /// </summary>
        public int CurrentFrameIndex { get; set; }

        /// <summary>
        /// If the animation loops.
        /// </summary>
        public bool Loops { get; set; } = true;

        /// <summary>
        /// If the animation is paused.
        /// 
        /// <para>
        /// This is by default true because you need to run <see cref="Play"/> first.
        /// </para>
        /// </summary>
        public bool Paused { get; set; } = true;

        /// <summary>
        /// When the animation ends.
        /// </summary>
        public event Action OnAnimationReachedEnd;

        /// <summary>
        /// When the frame changes.
        /// </summary>
        public event Action OnAnimationFrame;

        /// <summary>
        /// The list of events.
        /// </summary>
        public List<SpriteAnimationEvent> Events = new List<SpriteAnimationEvent>();

        /// <summary>
        /// When the animation event is played. The paramerer is the name of the events triggered in this frame.
        /// </summary>
        public event Action<string[]> OnAnimationEvent;

        /// <summary>
        /// How much time passed since the last animation frame was shown.
        /// </summary>
        private float elapsedTime;

        /// <summary>
        /// Automatically make the cells and frame indexes.
        /// </summary>
        /// <param name="frameWidth">The width of each frame.</param>
        /// <param name="frameHeight">The height of each frame</param>
        /// <param name="textureWidth">The width of the texture.</param>
        /// <param name="textureHeight">The height of the texture.</param>
        public SpriteAnimation(int frameWidth, int frameHeight, int textureWidth, int textureHeight)
        {
            AutoSetFrames(frameWidth, frameHeight, textureWidth, textureHeight);
            for (int i = 0; i < Frames.Count; i++)
            {
                FrameOrder.Add(i);
            }
        }

        /// <summary>
        /// Set the frame order for the rectangles.
        /// </summary>
        /// <param name="rects">The rectangles to be the frames.</param>
        public SpriteAnimation(params Rectangle[] rects)
        {
            Frames = rects.ToList();
            for (int i = 0; i < Frames.Count; i++)
            {
                FrameOrder.Add(i);
            }
        }

        /// <summary>
        /// Make a new animation with automatic frames and frame order. (recomended)
        /// </summary>
        /// <param name="frameWidth">The width of each frame.</param>
        /// <param name="frameHeight">The height of each frame</param>
        /// <param name="textureWidth">The width of the texture.</param>
        /// <param name="textureHeight">The height of the texture.</param>
        /// <param name="frameOrder">The order of the frames.</param>
        public SpriteAnimation(int frameWidth, int frameHeight, int textureWidth, int textureHeight, int[] frameOrder)
        {
            AutoSetFrames(frameWidth, frameHeight, textureWidth, textureHeight);
            FrameOrder = frameOrder.ToList();
        }

        /// <summary>
        /// Create a new animation with preset frames and order. (recomended)
        /// </summary>
        /// <param name="rectangles">The frames.</param>
        /// <param name="frameOrder">The frame order.</param>
        public SpriteAnimation(Rectangle[] rectangles, int[] frameOrder)
        {
            Frames = rectangles.ToList();
            FrameOrder = frameOrder.ToList();
        }

        /// <summary>
        /// Automatically make the frames.
        /// </summary>
        /// <param name="frameWidth">The width of each frame.</param>
        /// <param name="frameHeight">The height of each frame</param>
        /// <param name="textureWidth">The width of the texture.</param>
        /// <param name="textureHeight">The height of the texture.</param>
        public void AutoSetFrames(int frameWidth, int frameHeight, int textureWidth, int textureHeight)
        {
            int x = 0;
            int y = 0;
            // integer division tells us how many frames can fit on the x axis.
            int xFrames = textureWidth / frameWidth;
            // integer division tells us how many frames can fit on the y axis.
            int yFrames = textureHeight / frameHeight;

            for (int i = 0; i < xFrames; i++)
            {
                for (int j = 0; j < yFrames; j++)
                {
                    // add rectangle.
                    Frames.Add(new Rectangle(x, y, frameWidth, frameHeight));
                    x += frameWidth;
                    if (x >= textureWidth)
                    {
                        x = 0;
                        y += frameHeight;
                    }
                }
            }
        }

        public void Update(float deltaTime)
        {

            if (Paused) return;

            // enough time has passed.
            if (elapsedTime >= FrameSpeed)
            {
                int nextFrame = CurrentFrameIndex + 1;
                // reached end of animation.
                if (nextFrame >= FrameOrder.Count - 1)
                {
                    OnAnimationReachedEnd.Invoke();
                    if (!Loops) Paused = true;
                    else CurrentFrameIndex = 0;
                }
                else
                {
                    CurrentFrameIndex = nextFrame;
                }
                // invoke the frames.
                OnAnimationFrame.Invoke();

                List<string> events = new List<string>();

                for (int i = 0; i < Events.Count; i++)
                {
                    if (Events[i].FramesToHook.Contains(CurrentFrameIndex))
                    {
                        events.Add(Events[i].Name);
                    }
                }

                // invoke the events.
                if (events.Count > 0) OnAnimationEvent.Invoke(events.ToArray());
                elapsedTime = 0;

            }
            else
            {
                elapsedTime += deltaTime;
            }

        }

        /// <summary>
        /// Play the animation. This will unpause the animation.
        /// </summary>
        public void Play()
        {
            Paused = false;
        }

        /// <summary>
        /// Set the frame to the start. This will still play.
        /// </summary>
        public void Reset()
        {
            CurrentFrameIndex = FrameOrder[0];
        }

        /// <summary>
        /// Pause the animation. To unpause, use <see cref="Play"/>.
        /// </summary>
        public void Pause()
        {
            Paused = true;
        }

        /// <summary>
        /// This will pause the animation, and set back to first frame.
        /// </summary>
        public void Stop()
        {
            Paused = true;
            CurrentFrameIndex = 0;
        }

        /// <summary>
        /// Get the current rectangle frame.
        /// </summary>
        /// <returns>A rectangle of the current frame.</returns>
        // We use the frame order because it contains the indexes for the frames.
        public Rectangle GetCurrentRectangle() => Frames[FrameOrder[CurrentFrameIndex]];

    }
}
