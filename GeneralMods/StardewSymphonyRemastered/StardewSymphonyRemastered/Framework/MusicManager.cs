using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewSymphonyRemastered;
namespace StardewSymphonyRemastered.Framework
{
    /// <summary>
    /// TODO: Make this manage all of the music.
    /// 
    /// Make it be able to load in multiple music packs from a general mod/MusicPacks Directory
    /// 
    /// 
    /// </summary>
    public class MusicManager
    {
        /// <summary>
        /// A dictionary containing all of the music packs loaded in.
        /// </summary>
        public Dictionary<string,MusicPack> musicPacks;

        public MusicPack currentMusicPack;


        /// <summary>
        /// Constructor.
        /// </summary>
        public MusicManager()
        {
            this.musicPacks = new Dictionary<string, MusicPack>();
            this.currentMusicPack = null;
        }

        /// <summary>
        /// Swaps between referenced music packs and stops the last playing song.
        /// </summary>
        /// <param name="nameOfNewMusicPack"></param>
        public void swapMusicPacks(string nameOfNewMusicPack)
        {
            if (isMusicPackValid(nameOfNewMusicPack) == true)
            {
                if (this.currentMusicPack.isNull()==false)
                {
                    this.currentMusicPack.stopSong();
                }
                this.currentMusicPack = getMusicPack(nameOfNewMusicPack);
            }
        }

        /// <summary>
        /// Plays the song from the currently loaded music pack.
        /// </summary>
        /// <param name="songName"></param>
        public void playSongFromCurrentPack(string songName)
        {
            if (this.currentMusicPack.isNull() == false)
            {
                this.currentMusicPack.playSong(songName);
            }
        }

        /// <summary>
        /// Resumes the paused song from the current music pack.
        /// </summary>
        public void pauseSongFromCurrentPack() {
            if (this.currentMusicPack.isNull() == false)
            {
                this.currentMusicPack.pauseSong();
            }
        }

        /// <summary>
        /// Stops the song from the current music pack.
        /// </summary>
        public void stopSongFromCurrentMusicPack()
        {
            if (this.currentMusicPack.isNull() == false)
            {
                this.currentMusicPack.stopSong();
            }
        }

        /// <summary>
        /// Resumes the song from the current music pack.
        /// </summary>
        public void resumeSongFromCurrentMusicPack()
        {
            if (this.currentMusicPack.isNull() == false)
            {
                this.currentMusicPack.resumeSong();
            }
        }

        /// <summary>
        /// Returns the name of the currently playing song.
        /// </summary>
        /// <returns></returns>
        public string getNameOfCurrentlyPlayingSong()
        {
            if (this.currentMusicPack.isNull() == false)
            {
                return this.currentMusicPack.getNameOfCurrentSong();
            }
            else
            {
                return "";
            }
        }

        public MusicPackMetaData getMusicPackInformation()
        {
            if (this.currentMusicPack.isNull() == false)
            {
                return this.currentMusicPack.musicPackInformation;
            }
            else return null;
        }

        /// <summary>
        /// Checks to see if the music pack has been loaded into the Music Manager.
        /// </summary>
        /// <param name="nameOfMusicPack"></param>
        /// <returns></returns>
        public bool isMusicPackValid(string nameOfMusicPack)
        {
            if (this.currentMusicPack.isNull() == false)
            {
                return musicPacks.ContainsKey(nameOfMusicPack);
            }
            else return false;
        }

        /// <summary>
        /// Gets the music pack from the
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public MusicPack getMusicPack(string name)
        {
            if (isMusicPackValid(name) == false)
            {
                StardewSymphony.ModMonitor.Log("Error, the music pack: " + name + " is not found. Please make sure it is loaded in and try again.");
                return null;
            }
            else
            {

                foreach (var pair in this.musicPacks)
                {
                    if (name == pair.Key) return pair.Value;
                }
                return null; //Needed I suppose to ensure this function compiles.
            }
        }
    }
}
