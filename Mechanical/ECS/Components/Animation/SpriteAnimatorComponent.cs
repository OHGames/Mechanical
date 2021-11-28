/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O.H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/USED_CODE_LICENSES.txt for more info.
 */

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mechanical
{
    /// <summary>
    /// The Sprite Animator Component is used to animate the see <see cref="SpriteComponent"/>
    /// </summary>
    [NeedsComponent(typeof(SpriteComponent))]
    [DataContract]
    public sealed class SpriteAnimatorComponent : Component
    {

        /// <summary>
        /// A reference to the sprite.
        /// </summary>
        [DataMember]
        private SpriteComponent sprite;

        /// <summary>
        /// The current animation.
        /// </summary>
        [DataMember]
        public Animation CurrentAnimation { get; set; }

        /// <summary>
        /// The current animaton's name.
        /// </summary>
        [DataMember]
        public string CurrentAnimationName { get; set; }

        [DataMember]
        public Dictionary<string, Animation> Animations { get; set; } = new Dictionary<string, Animation>();

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

        public SpriteAnimatorComponent(Entity entity, Animation animation, string name) : base(entity)
        {
            // get the component.
            // This will never return null because it is a required component.
            sprite = entity.GetComponent<SpriteComponent>();

            // TODO: get the rectangle data from animation and set sprite.
            CurrentAnimation = animation;
            CurrentAnimationName = name;

            Animations.Add(CurrentAnimationName, CurrentAnimation);
            AllowMultiple = false;
        }

        public override void Update(float deltaTime)
        {
            sprite.SourceRectangle = (Rectangle?)CurrentAnimation.GetCurrentRectangle();
        }

        /// <summary>
        /// Add an animation.
        /// </summary>
        /// <param name="animation"></param>
        /// <param name="name"></param>
        public void AddAnimation(Animation animation, string name)
        {
            if (Animations.ContainsKey(name)) throw new Exception($"The animation name, {name}, is already used");

            Animations.Add(name, animation);
            animation.OnAnimationEvent += InternalOnAnimationEvent;
            animation.OnAnimationFrame += InternalOnAnimationFrame;
            animation.OnAnimationReachedEnd += InternalOnAnimationReachedEnd;
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
        /// Remove an animation from the list.
        /// </summary>
        /// <param name="name"></param>
        public void RemoveAnimation(string name)
        {
            if (!Animations.ContainsKey(name) || CurrentAnimationName == name) throw new Exception($"The animation {name} is the current animation or is not in the list!");

            Animations[name].OnAnimationEvent -= InternalOnAnimationEvent;
            Animations.Remove(name);
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

    }
}
