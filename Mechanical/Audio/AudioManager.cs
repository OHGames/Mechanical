using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// The audio manager handles audio.
    /// 
    /// <para>
    /// It is also a small wrapper for the <see cref="MediaPlayer"/> class.
    /// </para>
    /// </summary>
    public static class AudioManager
    {

        /// <summary>
        /// A list of sound effects. The key is the name that will be used to play.
        /// </summary>
        public static Dictionary<string, SoundEffect> SoundEffects { get; } = new Dictionary<string, SoundEffect>();

        /// <summary>
        /// A list of all the songs. The key is the name that will be used to play.
        /// </summary>
        public static Dictionary<string, Song> Songs { get; } = new Dictionary<string, Song>();

        /// <summary>
        /// The list of sound effect instances that are being played.
        /// </summary>
        public static Dictionary<string, SoundEffectInstance> Instances { get; } = new Dictionary<string, SoundEffectInstance>();

        /// <summary>
        /// If the audio manager repeats songs.
        /// </summary>
        public static bool RepeatsSongs { get => MediaPlayer.IsRepeating; set => MediaPlayer.IsRepeating = value; }

        /// <summary>
        /// The volume that songs are played at.
        /// </summary>
        public static float SongVolume { get => MediaPlayer.Volume; set => MediaPlayer.Volume = value; }

        /// <summary>
        /// If the songs are muted.
        /// </summary>
        public static bool SongsMuted { get => MediaPlayer.IsMuted; set => MediaPlayer.IsMuted = value; }

        /// <summary>
        /// The state of the media player.
        /// </summary>
        public static MediaState MediaState { get => MediaPlayer.State; }

        /// <summary>
        /// Adds a sound effect.
        /// </summary>
        /// <param name="name">The name to register the effect as.</param>
        /// <param name="effect">The effect to add.</param>
        /// <exception cref="ArgumentException">When the name is already registered.</exception>
        public static void AddSoundEffect(string name, SoundEffect effect)
        {
            if (SoundEffects.ContainsKey(name)) throw new ArgumentException($"{name} is already a sound effect name.");

            SoundEffects.Add(name, effect);
        }

        /// <summary>
        /// Removes a sound effect.
        /// </summary>
        /// <param name="name">The name of the effect to remove.</param>
        /// <exception cref="ArgumentException">When the name is not registered.</exception>
        public static void RemoveSoundEffect(string name)
        {
            ThrowNotRegisteredEffect(name);

            SoundEffects.Remove(name);
        }

        /// <summary>
        /// Adds a song.
        /// </summary>
        /// <param name="name">The name to register the effect as.</param>
        /// <param name="song">The song to add.</param>
        /// <exception cref="ArgumentException">When the name is already registered.</exception>
        public static void AddSong(string name, Song song)
        {
            if (Songs.ContainsKey(name)) throw new ArgumentException($"{name} is already a song name.");

            Songs.Add(name, song);
        }

        /// <summary>
        /// Removes a song.
        /// </summary>
        /// <param name="name">The name of the song to remove.</param>
        /// <exception cref="ArgumentException">When the name is not registered.</exception>
        public static void RemoveSong(string name)
        {
            ThrowNotRegisteredSong(name);

            Songs.Remove(name);
        }

        /// <summary>
        /// Plays a song.
        /// </summary>
        /// <param name="name">The name of the song.</param>
        /// <exception cref="ArgumentException">When the name is not registered.</exception>
        public static void PlaySong(string name)
        {
            ThrowNotRegisteredSong(name);

            //https://gamedev.stackexchange.com/q/146196\
            //https://creativecommons.org/licenses/by-sa/3.0/
            MediaPlayer.Play(Songs[name]);
        }

        /// <summary>
        /// Plays a song.
        /// </summary>
        /// <param name="name">The name of the song.</param>
        /// <param name="startPos">The starting position of the song to play.</param>
        /// <exception cref="ArgumentException">When the name is not registered.</exception>
        public static void PlaySong(string name, TimeSpan? startPos)
        {
            ThrowNotRegisteredSong(name);

            //https://gamedev.stackexchange.com/q/146196\
            //https://creativecommons.org/licenses/by-sa/3.0/
            MediaPlayer.Play(Songs[name], startPos);
        }

        /// <summary>
        /// Plays a song.
        /// </summary>
        /// <param name="name">The name of the song.</param>
        /// <exception cref="ArgumentException">When the name is not registered.</exception>
        public static void PauseSong(string name)
        {
            ThrowNotRegisteredSong(name);

            MediaPlayer.Pause();
        }

        /// <summary>
        /// Resumes a paused song.
        /// </summary>
        /// <param name="name">The name of the song.</param>
        /// <exception cref="ArgumentException">When the name is not registered.</exception>
        public static void ResumeSong(string name)
        {
            ThrowNotRegisteredSong(name);

            MediaPlayer.Resume();
        }

        /// <summary>
        /// Stop the song.
        /// </summary>
        /// <param name="name">The name of the song.</param>
        /// <exception cref="ArgumentException">When the name is not registered.</exception>
        public static void StopSong(string name)
        {
            ThrowNotRegisteredSong(name);

            MediaPlayer.Stop();
        }

        /// <summary>
        /// Creates a sound effect instance.
        /// </summary>
        /// <param name="name">The name of the sound effect.</param>
        /// <returns>The sound effect instance.</returns>
        /// <exception cref="ArgumentException">When the name is not registered.</exception>
        public static SoundEffectInstance CreateInstance(string name)
        {
            ThrowNotRegisteredEffect(name);

            return SoundEffects[name].CreateInstance();
        }

        /// <summary>
        /// Plays the sound effect.
        /// </summary>
        /// <param name="name">The name of the sound effect.</param>
        /// <returns>False if the effect was not played due to hardware limitations (effect count).</returns>
        /// <remarks>
        /// Play returns false if more SoundEffectInstances are currently playing then the platform allows.
        /// To loop a sound or apply 3D effects, call SoundEffect.CreateInstance() and SoundEffectInstance.Play() instead.
        /// SoundEffectInstances used by SoundEffect.Play() are pooled internally.
        /// </remarks>
        /// ^^^Taken from https://docs.monogame.net/api/Microsoft.Xna.Framework.Audio.SoundEffect.html
        /// <exception cref="ArgumentException">When the name is not registered.</exception>
        public static bool PlaySoundEffect(string name)
        {
            ThrowNotRegisteredEffect(name);

            return SoundEffects[name].Play();
        }

        /// <summary>
        /// Plays the sound effect with specified parameters.
        /// </summary>
        /// <param name="name">The name of the effect. </param>
        /// <param name="volume">The volume to play effect at. Volume, ranging from 0.0 (silence) to 1.0 (full volume). Volume during playback is scaled by SoundEffect.MasterVolume.</param>
        /// <param name="pitch">The pitch to play the effect at. Pitch adjustment, ranging from -1.0 (down an octave) to 0.0 (no change) to 1.0 (up an octave).</param>
        /// <param name="pan">The panning to play the effect at. Panning, ranging from -1.0 (left speaker) to 0.0 (centered), 1.0 (right speaker).</param>
        /// <returns>False if the effect was not played due to hardware limitations (effect count).</returns>
        /// <remarks>
        /// Play returns false if more SoundEffectInstances are currently playing then the platform allows.
        /// To apply looping or simulate 3D audio, call SoundEffect.CreateInstance() and SoundEffectInstance.Play() instead.
        /// SoundEffectInstances used by SoundEffect.Play() are pooled internally.
        /// </remarks>
        /// ^^^Taken from https://docs.monogame.net/api/Microsoft.Xna.Framework.Audio.SoundEffect.html
        public static bool PlaySoundEffect(string name, float volume, float pitch, float pan)
        {
            ThrowNotRegisteredEffect(name);

            return SoundEffects[name].Play(volume, pitch, pan);
        }

        /// <summary>
        /// This function will throw an exception when the name is not a song name.
        /// </summary>
        /// <param name="name">The name of the song.</param>
        /// <exception cref="ArgumentException">When the name is not registred.</exception>
        private static void ThrowNotRegisteredSong(string name)
        {
            if (!Songs.ContainsKey(name)) throw new ArgumentException($"{name} is not a song name.");
        }

        /// <summary>
        /// This function will throw an exception when the name is not a sound effert name.
        /// </summary>
        /// <param name="name">The name of the sound effect.</param>
        /// <exception cref="ArgumentException">When the name is not registred.</exception>
        private static void ThrowNotRegisteredEffect(string name)
        {
            if (!SoundEffects.ContainsKey(name)) throw new ArgumentException($"{name} is not a song name.");
        }

    }
}
