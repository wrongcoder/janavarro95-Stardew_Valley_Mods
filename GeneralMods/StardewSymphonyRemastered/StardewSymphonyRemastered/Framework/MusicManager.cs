using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewSymphonyRemastered;
using StardewValley;
using System.IO;

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

        Random packSelector;
        Random songSelector;
        /// <summary>
        /// Constructor.
        /// </summary>
        public MusicManager()
        {
            this.musicPacks = new Dictionary<string, MusicPack>();
            this.currentMusicPack = null;
            packSelector = new Random(Game1.random.Next(1,1000000));
            songSelector = new Random(Game1.player.deepestMineLevel + Game1.player.facingDirection + packSelector.Next(0,10000));
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

        /// <summary>
        /// Get the information associated with the current music pack.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Iterates across all music packs and determines which music packs contain songs that can be played right now.
        /// </summary>
        /// <param name="songListKey"></param>
        /// <returns></returns>
        public Dictionary<MusicPack,List<Song>> getListOfApplicableMusicPacks(string songListKey)
        {
            Dictionary<MusicPack, List<Song>> listOfValidDictionaries = new Dictionary<MusicPack, List<Song>>();
            foreach(var v in this.musicPacks)
            {
              var songList= v.Value.songInformation.getSongList(songListKey).Value;
                if (songList.Count > 0)
                {
                    listOfValidDictionaries.Add(v.Value, songList);
                }
            }
            return listOfValidDictionaries;
        }

        /// <summary>
        /// Selects the actual song to be played right now based off of the selector key. The selector key should be called when the player's location changes.
        /// </summary>
        /// <param name="songListKey"></param>
        public void selectMusic(string songListKey)
        {
            var listOfValidMusicPacks = getListOfApplicableMusicPacks(songListKey);
            if (listOfValidMusicPacks.Count == 0)
            {
                //No valid songs to play at this time.
                StardewSymphony.ModMonitor.Log("Error: There are no songs to play across any music pack. Are you sure you did this properly?");
                return;
            }
            
            int randInt = packSelector.Next(0, listOfValidMusicPacks.Count-1);

            var musicPackPair = listOfValidMusicPacks.ElementAt(randInt);

            //used to swap the music packs and stop the last playing song.
            this.swapMusicPacks(musicPackPair.Key.musicPackInformation.name);

            int randInt2 = songSelector.Next(0, musicPackPair.Value.Count);

            var songName = musicPackPair.Value.ElementAt(randInt2);

            this.currentMusicPack.playSong(songName.name);
        }


        /// <summary>
        /// Adds a valid xwb music pack to the list of music packs available.
        /// </summary>
        /// <param name="musicPack"></param>
        /// <param name="displayLogInformation">Whether or not to display the process to the console. Will include information from the pack's metadata. Default:False</param>
        /// <param name="xwbMusicPack">If displayLogInformation is also true this will display the name of all of the songs in the music pack when it is added in.</param>
        public void addMusicPack(MusicPack musicPack,bool displayLogInformation=false,bool displaySongs=false)
        {
            if (displayLogInformation == true)
            {
                StardewSymphony.ModMonitor.Log("Adding a new music pack!");

    
                StardewSymphony.ModMonitor.Log("    Location:" + musicPack.shortenedDirectory);
                StardewSymphony.ModMonitor.Log("    Name:" + musicPack.musicPackInformation.name);
                StardewSymphony.ModMonitor.Log("    Author:" + musicPack.musicPackInformation.author);
                StardewSymphony.ModMonitor.Log("    Description:" + musicPack.musicPackInformation.description);
                StardewSymphony.ModMonitor.Log("    Version Info:" + musicPack.musicPackInformation.versionInfo);
                StardewSymphony.ModMonitor.Log("    Song List:");

                if (displaySongs == true)
                {
                    foreach(var song in musicPack.songInformation.listOfSongsWithoutTriggers)
                    {
                        StardewSymphony.ModMonitor.Log("        " + song.name);
                    }
                }
            }
            this.musicPacks.Add(musicPack.musicPackInformation.name,musicPack);
        }
    }
}
