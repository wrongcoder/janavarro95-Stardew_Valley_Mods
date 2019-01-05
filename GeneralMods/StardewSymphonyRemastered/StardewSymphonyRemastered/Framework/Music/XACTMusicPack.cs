using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using StardewValley;

namespace StardewSymphonyRemastered.Framework
{
    /// <summary>TODO: Make this work and add in overrided functions.</summary>
    public class XACTMusicPack : MusicPack
    {
        public WaveBank WaveBank;
        public ISoundBank SoundBank;

        public Cue currentCue;

        public string WaveBankPath;
        public string SoundBankPath;

        /// <summary>Construct an instance.</summary>
        public XACTMusicPack(string directoryToXwb, string pathToWaveBank, string pathToSoundBank)
        {
            this.directory = directoryToXwb;
            this.WaveBankPath = pathToWaveBank;
            this.SoundBankPath = pathToSoundBank;
            this.songInformation = new SongSpecifics();
            this.currentCue = null;
            this.musicPackInformation = MusicPackMetaData.readFromJson(directoryToXwb);
            if (this.musicPackInformation == null)
            {
                if (StardewSymphony.Config.EnableDebugLog)
                    StardewSymphony.ModMonitor.Log("Error: MusicPackInformation.json not found at: " + directoryToXwb + ". Blank information will be put in place.", StardewModdingAPI.LogLevel.Warn);
                this.musicPackInformation = new MusicPackMetaData("???", "???", "", "0.0.0", "");
            }

            this.WaveBank = new WaveBank(Game1.audioEngine, this.WaveBankPath);
            this.SoundBank = new SoundBankWrapper(new SoundBank(Game1.audioEngine, this.SoundBankPath));
            this.loadMusicFiles();
        }

        /// <summary>Load all of the generic music file names into the music pack's information.</summary>
        private void loadMusicFiles()
        {
            var listOfSongStrings = MusicHexProcessor.ProcessSongNamesFromHex(this, StardewSymphony.Reset, this.SoundBankPath);

            List<Song> listofSongs = new List<Song>();
            foreach (string songname in listOfSongStrings)
            {
                Song song = new Song(songname);
                listofSongs.Add(song);
            }

            this.songInformation.listOfSongsWithoutTriggers = listofSongs;
        }

        /// <summary>Get the cue from the list of songs.</summary>
        /// <param name="name">The name of the song to get.</param>
        private Cue getCue(string name)
        {
            if (!this.songInformation.isSongInList(name))
            {
                if (StardewSymphony.Config.EnableDebugLog)
                    StardewSymphony.ModMonitor.Log("Error! The song " + name + " could not be found in music pack " + this.musicPackInformation.name + ". Please ensure that this song is part of this music pack located at: " + this.WaveBankPath + " or contact the music pack author: " + this.musicPackInformation.author, StardewModdingAPI.LogLevel.Error);
                return null;
            }
            else
            {
                return this.SoundBank.GetCue(name);
            }
        }

        /// <summary>Play a song.</summary>
        /// <param name="name">The name of the song to play.</param>
        public override void playSong(string name)
        {
            this.currentCue = this.getCue(name);
            if (this.currentCue == null)
                return; //getCue will throw the error message.

            Game1.waveBank = this.WaveBank;
            Game1.soundBank = this.SoundBank;
            this.currentCue.Play();
            StardewSymphony.Reset();
        }

        /// <summary>Stops the currently playing song and nulls the current song.</summary>
        public override void stopSong()
        {
            Game1.currentSong?.Stop(AudioStopOptions.Immediate);
            if (this.currentCue != null)
            {
                Game1.waveBank = this.WaveBank;
                Game1.soundBank = this.SoundBank;
                this.currentCue.Stop(AudioStopOptions.Immediate);
                StardewSymphony.Reset();
                this.currentCue = null;
            }
        }

        public override bool isPlaying()
        {
            if (this.currentCue == null) return false;
            return this.currentCue.IsPlaying;
        }
    }
}
