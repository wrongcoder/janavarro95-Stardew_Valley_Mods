using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using StardewValley;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardewSymphonyRemastered.Framework
{
    /// <summary>
    /// TODO: Make this class
    /// </summary>
    public class WavMusicPack : MusicPack
    {
        public Song currentSong;
        public string songsDirectory;

        public SoundEffectInstance sound;

        bool loop;


        public Dictionary<string,SoundEffectInstance> sounds;
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="directoryToMusicPack"></param>
        public WavMusicPack(string directoryToMusicPack, bool Loop = false)
        {
            this.directory = directoryToMusicPack;
            this.setModDirectoryFromFullDirectory();
            this.songsDirectory = Path.Combine(this.directory, "Songs");
            this.songInformation = new SongSpecifics();
            this.musicPackInformation = MusicPackMetaData.readFromJson(directoryToMusicPack);
            this.loop = Loop;
            this.sounds = new Dictionary<string, SoundEffectInstance>();
            /*
            if (this.musicPackInformation == null)
            {
                //StardewSymphony.ModMonitor.Log("Error: MusicPackInformation.json not found at: " + directoryToMusicPack + ". Blank information will be put in place.", StardewModdingAPI.LogLevel.Warn);
                //this.musicPackInformation = new MusicPackMetaData("???", "???", "", "0.0.0","");
            }
            */
            //StardewSymphony.ModMonitor.Log(this.musicPackInformation.name.ToString());
            this.loadMusicFiles();
        }

        /// <summary>
        /// A shortened directory name for display purposes.
        /// </summary>
        /// <returns></returns>
        public override void setModDirectoryFromFullDirectory()
        {
            string[] spliter = this.directory.Split(Path.DirectorySeparatorChar);
            string directoryLocation = "";
            for (int i = spliter.Length - 6; i < spliter.Length; i++)
            {
                directoryLocation += spliter[i];

                if (i != spliter.Length - 1)
                {
                    directoryLocation += Path.DirectorySeparatorChar;
                }
            }
        }

        /*
        /// <summary>
        /// Load a wav file into the stream to be played.
        /// </summary>
        public void LoadWavFromFileToStream(string file)
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            System.IO.Stream waveFileStream = File.OpenRead(file); //TitleContainer.OpenStream(file);
            this.effect = SoundEffect.FromStream(waveFileStream);
            this.sound=this.effect.CreateInstance();
            this.currentSong = new Song(file);
            waveFileStream.Dispose();
        }
        */

        /// <summary>
        /// Returns the name of the currently playing song.
        /// </summary>
        /// <returns></returns>
        public override string getNameOfCurrentSong()
        {
            if (this.currentSong == null) return "";
            return this.currentSong.name;
        }

        /// <summary>
        /// Load in the music files from the pack's respective Directory/Songs folder. Typically Content/Music/Wav/FolderName/Songs
        /// </summary>
        public override void loadMusicFiles()
        {
            string[] wavFiles = Directory.GetFiles(this.songsDirectory, "*.wav");
            List<Song> listOfSongs = new List<Song>();
            foreach(var wav in wavFiles)
            {

                System.IO.Stream waveFileStream = File.OpenRead(wav); //TitleContainer.OpenStream(file);
                SoundEffect eff = SoundEffect.FromStream(waveFileStream);
                SoundEffectInstance instance = eff.CreateInstance();
                string name = Path.GetFileNameWithoutExtension(wav);
                this.sounds.Add(name, instance);

                waveFileStream.Dispose();
                Song song = new Song(wav);
                this.songInformation.listOfSongsWithoutTriggers.Add(song);
                //listOfSongs.Add(song);
            }
        }

        /// <summary>
        /// Used to pause the current song.
        /// </summary>
        public override void pauseSong()
        {
            if (this.sound != null) sound.Pause();
        }

        /// <summary>
        /// Used to play a song.
        /// </summary>
        /// <param name="name"></param>
        public override void playSong(string name)
        {
            string pathToSong = getSongPathFromName(name);

            bool exists=this.sounds.TryGetValue(name,out this.sound);

            if (exists)
            {
                this.currentSong = new Song(name);
                sound.Play();
            }
            else
            {
                StardewSymphony.ModMonitor.Log("An error occured where we can't find the song anymore. Weird. Please contact Omegasis with a SMAPI Log and describe when/how the event occured.");
            }
            
        }

        public override void playRandomSong()
        {
            Random r = Game1.random;
            int value=r.Next(0, this.songInformation.listOfSongsWithoutTriggers.Count);
            Song s = this.songInformation.listOfSongsWithoutTriggers.ElementAt(value);
            this.swapSong(s.name);
        }

        /// <summary>
        /// Used to resume the currently playing song.
        /// </summary>
        public override void resumeSong()
        {
            if (sound == null) return;
            sound.Resume();
        }

        /// <summary>
        /// Used to stop the currently playing song.
        /// </summary>
        public override void stopSong()
        {
            if (Game1.currentSong != null) Game1.currentSong.Stop(AudioStopOptions.Immediate);
            if (this.currentSong == null) return;
            if (sound != null)
            {
                sound.Stop(true);
                this.currentSong = null;
            }
        }

        /// <summary>
        /// Used to change from one playing song to another;
        /// </summary>
        /// <param name="songName"></param>
        public override void swapSong(string songName)
        {
            this.stopSong();
            this.playSong(songName);
        }

        /// <summary>
        /// Get the son's name from the path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>11111111111111
        public string getSongNameFromPath(string path)
        {
            foreach(var song in this.songInformation.listOfSongsWithoutTriggers)
            {
                if (song.getPathToSong()== path) return song.name;
            }
            return "";
        }

        /// <summary>
        /// Gets the song's path that shares the same name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string getSongPathFromName(string name)
        {
            foreach (var song in this.songInformation.listOfSongsWithoutTriggers)
            {
                if (song.name == name) return song.getPathToSong();
            }
            return "";
        }

        public override bool isPlaying()
        {
            if (this.sound == null) return false;
            if (this.sound.State == SoundState.Playing) return true;
            else return false;
        }
    }
}
