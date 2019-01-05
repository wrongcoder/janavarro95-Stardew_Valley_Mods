using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using NAudio.Wave;
using StardewValley;

namespace StardewSymphonyRemastered.Framework
{
    /// <summary>TODO: Make this class</summary>
    public class WavMusicPack : MusicPack
    {
        /// <summary>The refererence to the information for the current song.</summary>
        public Song currentSong;

        /// <summary>The directory where all of the songs are stored.</summary>
        public string songsDirectory;

        /// <summary>The currently playing sound.</summary>
        public SoundEffectInstance sound;

        /// <summary>The name of the music pack/</summary>
        public string Name => this.musicPackInformation.name;

        public Dictionary<string, SoundEffectInstance> sounds;
        /// <summary>Construct an instance.</summary>
        /// <param name="directoryToMusicPack"></param>
        public WavMusicPack(string directoryToMusicPack)
        {
            this.directory = directoryToMusicPack;
            this.songsDirectory = Path.Combine(this.directory, "Songs");
            this.songInformation = new SongSpecifics();
            this.musicPackInformation = MusicPackMetaData.readFromJson(directoryToMusicPack);
            this.sounds = new Dictionary<string, SoundEffectInstance>();
            this.loadMusicFiles();
        }

        /// <summary>Load in the music files from the pack's respective Directory/Songs folder. Typically Content/Music/Wav/FolderName/Songs</summary>
        private void loadMusicFiles()
        {
            List<string> wavFiles = Directory.GetFiles(this.songsDirectory, "*.wav").ToList();
            wavFiles.AddRange(Directory.GetFiles(this.songsDirectory, "*.mp3"));

            DateTime span = DateTime.Now;
            foreach (string wav in wavFiles)
            {
                MemoryStream memoryStream = new MemoryStream();
                AudioFileReader fileReader = new AudioFileReader(wav);
                fileReader.CopyTo(memoryStream);

                SoundEffect eff = null;

                Stream waveFileStream = File.OpenRead(wav); //TitleContainer.OpenStream(file);

                if (wav.Contains(".wav"))
                    eff = SoundEffect.FromStream(waveFileStream);
                else if (wav.Contains(".mp3"))
                {
                    using (Mp3FileReader reader = new Mp3FileReader(waveFileStream))
                    {
                        using (WaveStream pcmStream = WaveFormatConversionStream.CreatePcmStream(reader))
                        {
                            StardewSymphony.ModMonitor.Log("MP3 CONVERT! " + Path.GetFileNameWithoutExtension(wav) + ".wav");
                            WaveFileWriter.CreateWaveFile(Path.Combine(this.songsDirectory, (Path.GetFileNameWithoutExtension(wav) + ".wav")), pcmStream);

                            waveFileStream = File.OpenRead((Path.GetFileNameWithoutExtension(wav) + ".wav")); //TitleContainer.OpenStream(file);
                            eff = SoundEffect.FromStream(waveFileStream);

                            File.Delete(Path.Combine(this.songsDirectory, (Path.GetFileNameWithoutExtension(wav) + ".wav")));
                        }
                    }
                }
                else if (wav.Contains(".ogg"))
                {
                    StardewSymphony.ModMonitor.Log("Sorry, but .ogg files are currently not supported. Keep bugging the mod author (me) for this if you want it!", StardewModdingAPI.LogLevel.Alert);
                    continue;
                }


                //SoundEffect eff = new SoundEffect(wavData, 48000 , AudioChannels.Mono);

                if (eff == null)
                    continue;
                SoundEffectInstance instance = eff.CreateInstance();


                string name = Path.GetFileNameWithoutExtension(wav);
                if (this.sounds.ContainsKey(name))
                    continue;
                this.sounds.Add(name, instance);

                //waveFileStream.Dispose();
                Song song = new Song(name);
                this.songInformation.listOfSongsWithoutTriggers.Add(song);
                //listOfSongs.Add(song);
            }
            if (StardewSymphony.Config.EnableDebugLog)
                StardewSymphony.ModMonitor.Log("Time to load WAV music pack: " + this.musicPackInformation.name + span.Subtract(DateTime.Now).ToString());
        }

        /// <summary>Used to play a song.</summary>
        public override void playSong(string name)
        {
            //string pathToSong = this.getSongPathFromName(name);

            bool exists = this.sounds.TryGetValue(name, out this.sound);

            if (exists)
            {
                this.currentSong = new Song(name);
                this.sound.Play();
            }
            else
            {
                StardewSymphony.ModMonitor.Log("An error occured where we can't find the song anymore. Weird. Please contact Omegasis with a SMAPI Log and describe when/how the event occured.");
            }
        }

        /// <summary>Used to stop the currently playing song.</summary>
        public override void stopSong()
        {
            Game1.currentSong?.Stop(AudioStopOptions.Immediate);
            if (this.currentSong == null)
                return;

            this.sound?.Stop(true);
            this.currentSong = null;
        }

        public override bool isPlaying()
        {
            return this.sound?.State == SoundState.Playing;
        }
    }
}
