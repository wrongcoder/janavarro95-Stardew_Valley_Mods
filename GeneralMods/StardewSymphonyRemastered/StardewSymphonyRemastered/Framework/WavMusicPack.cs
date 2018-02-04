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

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="directoryToMusicPack"></param>
        public WavMusicPack(string directoryToMusicPack)
        {
            this.directory = directoryToMusicPack;
            this.setModDirectoryFromFullDirectory();
            this.songsDirectory = Path.Combine(this.directory, "Songs");
            this.songInformation = new SongSpecifics();
            this.musicPackInformation = MusicPackMetaData.readFromJson(Path.Combine(directoryToMusicPack, "MusicPackInformation.json"));

            if (this.musicPackInformation == null)
            {
                StardewSymphony.ModMonitor.Log("Error: MusicPackInformation.json not found at: " + directoryToMusicPack + ". Blank information will be put in place.", StardewModdingAPI.LogLevel.Warn);
                this.musicPackInformation = new MusicPackMetaData("???", "???", "", "0.0.0");
            }
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
            this.shortenedDirectory = directoryLocation;
        }

        /// <summary>
        /// Returns the name of the currently playing song.
        /// </summary>
        /// <returns></returns>
        public override string getNameOfCurrentSong()
        {
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
                Song song = new Song(wav);
                listOfSongs.Add(song);
            }
            this.songInformation.listOfSongsWithoutTriggers = listOfSongs;
        }

        public override void pauseSong()
        {
            throw new NotImplementedException();
        }

        public override void playSong(string name)
        {
            throw new NotImplementedException();
        }

        public override void resumeSong()
        {
            throw new NotImplementedException();
        }

        public override void stopSong()
        {
            throw new NotImplementedException();
        }

        public override void swapSong(string songName)
        {
            throw new NotImplementedException();
        }

    }
}
