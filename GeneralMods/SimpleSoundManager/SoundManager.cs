using Microsoft.Xna.Framework.Audio;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSoundManager
{
    public class SoundManager
    {
        public WaveBank waveBank;
        public SoundBank soundBank;
        WaveBank vanillaWaveBank;
        SoundBank vanillaSoundBank;
        List<Cue> currentlyPlayingSounds;

        /// <summary>
        /// Make a new Sound Manager to play and manage sounds in a modded wave bank.
        /// </summary>
        /// <param name="newWaveBank">The path to the wave bank in the mod's asset folder.</param>
        /// <param name="newSoundBank">The path to the sound bank in the mod's asset folder.</param>
        public SoundManager(string newWaveBank, string newSoundBank)
        {
            this.waveBank = new WaveBank(Game1.audioEngine, newWaveBank);
            this.soundBank = new SoundBank(Game1.audioEngine, newSoundBank);
            this.currentlyPlayingSounds = new List<Cue>();
            vanillaWaveBank = Game1.waveBank;
            vanillaSoundBank = Game1.soundBank;
        }

        /// <summary>
        /// Make a new Sound Manager to play and manage sounds in a modded wave bank.
        /// </summary>
        /// <param name="newWaveBank">The reference to the wave bank in the mod's asset folder.</param>
        /// <param name="newSoundBank">The reference to the sound bank in the mod's asset folder.</param>
        public SoundManager(WaveBank newWaveBank, SoundBank newSoundBank)
        {
            this.waveBank = newWaveBank;
            this.soundBank = newSoundBank;
            this.currentlyPlayingSounds = new List<Cue>();
        }

        /// <summary>
        /// Play a sound from the mod's wave bank.
        /// </summary>
        /// <param name="soundName">The name of the sound in the mod's wave bank. This will fail if the sound doesn't exists. This is also case sensitive.</param>
        public void playSound(string soundName)
        {
            Game1.waveBank = this.waveBank;
            Game1.soundBank = this.soundBank;

            Cue currentCue = this.soundBank.GetCue(soundName);
            if (currentCue == null) return;
            else
            {
                currentCue.Play();
                currentlyPlayingSounds.Add(currentCue);
            }

            Game1.waveBank = this.vanillaWaveBank;
            Game1.soundBank = this.vanillaSoundBank;
            removeAllStopedSounds();
        }

        /// <summary>
        /// Pauses the first instance of this sound.
        /// </summary>
        /// <param name="soundName"></param>
        public void pauseSound(string soundName)
        {
            foreach (var v in currentlyPlayingSounds)
            {
                if (v.Name == soundName) v.Pause();
                break;
            }
            removeAllStopedSounds();
        }

        /// <summary>
        /// Pause all sounds that share the sound name in common.
        /// </summary>
        /// <param name="soundName"></param>
        public void pauseAllSoundsWithThisName(string soundName)
        {
            foreach (var v in currentlyPlayingSounds)
            {
                if (v.Name == soundName) v.Pause();
            }
            removeAllStopedSounds();
        }

        /// <summary>
        /// Pauses all of the sounds that the SoundManager class is keeping track of.
        /// </summary>
        public void pauseAllSounds()
        {
            foreach(var v in currentlyPlayingSounds)
            {
                v.Pause();
            }
            removeAllStopedSounds();
        }

        /// <summary>
        /// Resume the first instance of the sound that has this name.
        /// </summary>
        public void resumeSound(string soundName)
        {
            foreach(var v in currentlyPlayingSounds)
            {
                if (v.Name==soundName&&v.IsPaused) v.Resume();
                break;
            }
            removeAllStopedSounds();
        }

        /// <summary>
        /// Resume all paused sounds that have this name.
        /// </summary>
        /// <param name="soundName"></param>
        public void resumeAllSoundsWithThisName(string soundName)
        {
            foreach (var v in currentlyPlayingSounds)
            {
                if (v.Name == soundName && v.IsPaused) v.Resume();           
            }
            removeAllStopedSounds();
        }

        /// <summary>
        /// Resumes playing all paused sounds.
        /// </summary>
        public void resumeAllSounds()
        {
            foreach (var v in currentlyPlayingSounds)
            {
                if (v.IsPaused) v.Resume();
            }
            removeAllStopedSounds();
        }

        /// <summary>
        /// Stop the first instance of the sound that has this name.
        /// </summary>
        /// <param name="soundName"></param>
        public void stopSound(string soundName)
        {
            foreach (var v in currentlyPlayingSounds)
            {
                if (v.Name == soundName)
                {
                    v.Stop(AudioStopOptions.Immediate);
                    break;
                }
            }
            removeAllStopedSounds();
        }

        /// <summary>
        /// Stops all of the sounds that share this name.
        /// </summary>
        /// <param name="soundName"></param>
        public void stopAllSoundsWithThisName(string soundName)
        {
            foreach (var v in currentlyPlayingSounds)
            {
                if (v.Name == soundName)
                {
                    v.Stop(AudioStopOptions.Immediate);
                }
            }
            removeAllStopedSounds();
        }

        /// <summary>
        /// Stops all of the sounds that the SoundManager is keeping track of.
        /// </summary>
        public void stopAllSounds()
        {
            foreach(var v in currentlyPlayingSounds)
            {
                v.Stop(AudioStopOptions.Immediate);
            }
            removeAllStopedSounds();
        }

        /// <summary>
        /// Removes all of the sounds that have stoped playing. Used to clean up the list of songs that SoundManager is keeping track of, whether the sound is finished or it manually was stopped.
        /// </summary>
        public void removeAllStopedSounds()
        {
            List<Cue> cuesToRemove = new List<Cue>();
            foreach (var v in currentlyPlayingSounds)
            {
                if (v.IsStopped) cuesToRemove.Add(v);
            }
            foreach (var v in cuesToRemove)
            {
                currentlyPlayingSounds.Remove(v);
            }
            cuesToRemove.Clear();
        }
    }
}
