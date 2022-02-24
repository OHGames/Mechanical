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
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// An animated UI image.
    /// </summary>
    [DataContract]
    public class GUIAnimatedImage : GUIElement
    {

        /// <summary>
        /// The texture to render.
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// The rectangle of the texture 
        /// </summary>
        [DataMember]
        public Rectangle? SourceRectangle { get; set; }

        /// <summary>
        /// The effect to draw with.
        /// </summary>
        public Effect Effect { get; set; }

        /// <summary>
        /// The sprite effects to use.
        /// </summary>
        [DataMember]
        public SpriteEffects Effects { get; set; }

        /// <summary>
        /// A list of animations.
        /// </summary>
        [DataMember]
        public Dictionary<string, SpriteAnimation> Animations { get; set; }

        /// <summary>
        /// The current animation.
        /// </summary>
        [DataMember]
        public SpriteAnimation CurrentAnimation { get; private set; }

        /// <summary>
        /// The current animation's name.
        /// </summary>
        [DataMember]
        public string CurrentAnimationName { get; private set; }

        /// <summary>
        /// On an animation event.
        /// 
        /// <para>
        /// The first string is the names of the events, the second string is the animation name. You can use this event, or have a reference to the animation that you want to use and hook onto that.
        /// </para>
        /// </summary>
        public event Action<string[], string> OnAnimationEvent;

        /// <summary>
        /// On an animation frame.
        /// 
        /// <para>
        /// The first string is the animation name. You can use this event, or have a reference to the animation that you want to use and hook onto that.
        /// </para>
        /// </summary>
        public event Action<string> OnAnimationFrame;

        /// <summary>
        /// When the animation reaches the end.
        /// 
        /// <para>
        /// The first string is the animation name. You can use this method, or have a reference to the animation that you want to use and hook onto that.
        /// </para>
        /// </summary>
        public event Action<string> OnAnimationReachedEnd;

        public GUIAnimatedImage(GUICanvas canvas, string name, Texture2D texture, SpriteAnimation animation, string animationName, Rectangle? sourceRect = null, Effect effect = null, SpriteEffects spriteEffects = SpriteEffects.None) : base(canvas, name)
        {

            Texture = texture;
            SourceRectangle = sourceRect;
            Effect = effect;
            Effects = spriteEffects;

            CurrentAnimation = animation;
            CurrentAnimationName = animationName;

        }

        public override void Update(float deltaTime)
        {
            CurrentAnimation.Update(deltaTime);
            SourceRectangle = (Rectangle?)CurrentAnimation.GetCurrentRectangle();
        }

        public override void Draw()
        {
            Drawing.Draw(Texture, Bounds, SourceRectangle, Color, Rotation, Origin, Effects, 0, Effect);
        }

        /// <summary>
        /// Add an animation.
        /// </summary>
        /// <param name="animation">The animation.</param>
        /// <param name="name">The name of the animation.</param>
        public void AddAnimation(SpriteAnimation animation, string name)
        {
            if (Animations.ContainsKey(name)) throw new Exception($"The animation name, {name}, already exists.");

            Animations.Add(name, animation);
            animation.OnAnimationEvent += InternalOnAnimationEvent;
            animation.OnAnimationFrame += InternalOnAnimationFrame;
            animation.OnAnimationReachedEnd += InternalOnAnimationReachedEnd;
        }

        /// <summary>
        /// Remove an animation from the list.
        /// </summary>
        /// <param name="name"></param>
        public void RemoveAnimation(string name)
        {
            if (!Animations.ContainsKey(name) || CurrentAnimationName == name) throw new Exception($"The animation {name} is the current animation or is not in the list!");

            Animations[name].OnAnimationEvent -= InternalOnAnimationEvent;
            Animations[name].OnAnimationFrame -= InternalOnAnimationFrame;
            Animations[name].OnAnimationReachedEnd -= InternalOnAnimationReachedEnd;
            Animations.Remove(name);
        }

        /// <summary>
        /// Play the animation.
        /// </summary>
        /// <param name="name">The name of the animation.</param>
        /// <exception cref="Exception">When the animation name is not present.</exception>
        public void Play(string name)
        {
            if (!Animations.ContainsKey(name)) throw new Exception($"The animation, {name}, is not in the list!");
            if (CurrentAnimationName != name || (CurrentAnimationName == name && CurrentAnimation.Paused))
            {
                // if it is paused, just play it.
                if (!CurrentAnimation.Paused)
                {
                    // stop animation
                    CurrentAnimation.Stop();

                    Animations[name].Play();
                    CurrentAnimation = Animations[name];
                    CurrentAnimationName = name;
                }
                else
                {
                    CurrentAnimation.Play();
                }
            }
        }

        /// <summary>
        /// Stops the current animation.
        /// </summary>
        public void Stop()
        {
            // stop animation
            CurrentAnimation.Stop();
        }

        /// <summary>
        /// Pauses the current animation.
        /// </summary>
        public void Pause()
        {
            // Pause animation.
            CurrentAnimation.Pause();
        }

        /// <summary>
        /// Pauses the animation.
        /// </summary>
        public void Reset()
        {
            CurrentAnimation.Reset();
        }

        /// <summary>
        /// This is used to invoke <see cref="OnAnimationReachedEnd"/>
        /// </summary>
        private void InternalOnAnimationReachedEnd()
        {
            OnAnimationReachedEnd.Invoke(CurrentAnimationName);
        }

        /// <summary>
        /// This is used to invoke <see cref="OnAnimationFrame"/>
        /// </summary>
        private void InternalOnAnimationFrame()
        {
            OnAnimationFrame.Invoke(CurrentAnimationName);
        }

        /// <summary>
        /// This is used to invoke the <see cref="OnAnimationEvent"/>
        /// </summary>
        /// <param name="obj"></param>
        private void InternalOnAnimationEvent(string[] obj)
        {
            // the current animation name because it is the only one being updated.
            OnAnimationEvent.Invoke(obj, CurrentAnimationName);
        }

    }
}
