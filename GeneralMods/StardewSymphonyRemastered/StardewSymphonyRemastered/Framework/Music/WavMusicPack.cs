using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using NAudio.Vorbis;
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

        bool loop;

        /// <summary>The name of the music pack/</summary>
        public string Name => this.musicPackInformation.name;

        public Dictionary<string, SoundEffectInstance> sounds;
        /// <summary>Construct an instance.</summary>
        /// <param name="directoryToMusicPack"></param>
        public WavMusicPack(string directoryToMusicPack, bool loop = false)
        {
            this.directory = directoryToMusicPack;
            this.setModDirectoryFromFullDirectory();
            this.songsDirectory = Path.Combine(this.directory, "Songs");
            this.songInformation = new SongSpecifics();
            this.musicPackInformation = MusicPackMetaData.readFromJson(directoryToMusicPack);
            this.loop = loop;
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

        /// <summary>A shortened directory name for display purposes.</summary>
        public override void setModDirectoryFromFullDirectory()
        {
            string[] spliter = this.directory.Split(Path.DirectorySeparatorChar);
            string directoryLocation = "";
            for (int i = spliter.Length - 6; i < spliter.Length; i++)
            {
                directoryLocation += spliter[i];

                if (i != spliter.Length - 1)
                    directoryLocation += Path.DirectorySeparatorChar;
            }
        }

        /*
        /// <summary>Load a wav file into the stream to be played.</summary>
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

        /// <summary>Returns the name of the currently playing song.</summary>
        public override string getNameOfCurrentSong()
        {
            return this.currentSong?.name ?? "";
        }

        /// <summary>Load in the music files from the pack's respective Directory/Songs folder. Typically Content/Music/Wav/FolderName/Songs</summary>
        public override void loadMusicFiles()
        {
            List<string> wavFiles = Directory.GetFiles(this.songsDirectory, "*.wav").ToList();
            wavFiles.AddRange(Directory.GetFiles(this.songsDirectory, "*.mp3"));
            wavFiles.AddRange(Directory.GetFiles(this.songsDirectory, "*.ogg"));

            DateTime span = DateTime.Now;
            foreach (string wav in wavFiles)
            {
                SoundEffect eff = null;

                Stream waveFileStream = File.OpenRead(wav); //TitleContainer.OpenStream(file);
                

                if (wav.Contains(".wav"))
                {
                    eff = SoundEffect.FromStream(waveFileStream);
                    waveFileStream.Close();
                }
                else if (wav.Contains(".mp3"))
                {
                    using (Mp3FileReader reader = new Mp3FileReader(waveFileStream))
                    {
                        using (WaveStream pcmMP3Stream = WaveFormatConversionStream.CreatePcmStream(reader))
                        {
                            StardewSymphony.DebugLog("Converting: " + this.songsDirectory + Path.GetFileName(wav));
                            WaveFileWriter.CreateWaveFile(Path.Combine(this.songsDirectory, (Path.GetFileNameWithoutExtension(wav) + ".wav")), pcmMP3Stream);

                            waveFileStream = File.OpenRead(Path.Combine(this.songsDirectory, (Path.GetFileNameWithoutExtension(wav) + ".wav"))); //TitleContainer.OpenStream(file);
                            eff = SoundEffect.FromStream(waveFileStream);

                            waveFileStream.Close();
                            File.Delete(Path.Combine(this.songsDirectory, (Path.GetFileNameWithoutExtension(wav) + ".wav")));
                        }
                    }
                }
                else if (wav.Contains(".ogg"))
                {

                    //Credits: https://social.msdn.microsoft.com/Forums/vstudio/en-US/100a97af-2a1c-4b28-b464-d43611b9b5d6/converting-multichannel-ogg-to-stereo-wav-file?forum=csharpgeneral

                    using (VorbisWaveReader vorbisStream = new VorbisWaveReader(wav))
                    {

                        StardewSymphony.DebugLog("Converting: " + this.songsDirectory+Path.GetFileName(wav));
                        WaveFileWriter.CreateWaveFile(Path.Combine(this.songsDirectory, (Path.GetFileNameWithoutExtension(wav) + ".wav")), vorbisStream.ToWaveProvider16());

                        //WaveFileReader reader = new WaveFileReader(Path.Combine(this.songsDirectory, (Path.GetFileNameWithoutExtension(wav) + ".wav")));
                        
                        waveFileStream = File.OpenRead(Path.Combine(this.songsDirectory, (Path.GetFileNameWithoutExtension(wav) + ".wav"))); //TitleContainer.OpenStream(file);
                        eff = SoundEffect.FromStream(waveFileStream);

                        waveFileStream.Close();

                        File.Delete(Path.Combine(this.songsDirectory, (Path.GetFileNameWithoutExtension(wav) + ".wav")));
                    }
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
                Song song = new Song(wav);
                this.songInformation.listOfSongsWithoutTriggers.Add(song);
                //listOfSongs.Add(song);
            }
            if (StardewSymphony.Config.EnableDebugLog)
                StardewSymphony.ModMonitor.Log("Time to load WAV music pack: " + this.musicPackInformation.name + span.Subtract(DateTime.Now).ToString());
        }

        /// <summary>Used to pause the current song.</summary>
        public override void pauseSong()
        {
            this.sound?.Pause();
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

        public override void playRandomSong()
        {
            Random r = Game1.random;
            int value = r.Next(0, this.songInformation.listOfSongsWithoutTriggers.Count);
            Song s = this.songInformation.listOfSongsWithoutTriggers.ElementAt(value);
            this.swapSong(s.name);
        }

        /// <summary>Used to resume the currently playing song.</summary>
        public override void resumeSong()
        {
            this.sound?.Resume();
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

        /// <summary>Used to change from one playing song to another;</summary>
        public override void swapSong(string songName)
        {
            this.stopSong();
            this.playSong(songName);
        }

        /// <summary>Get the son's name from the path.</summary>
        public string getSongNameFromPath(string path)
        {
            foreach (var song in this.songInformation.listOfSongsWithoutTriggers)
            {
                if (song.getPathToSong() == path)
                    return song.name;
            }
            return "";
        }

        /// <summary>Gets the song's path that shares the same name.</summary>
        public string getSongPathFromName(string name)
        {
            foreach (var song in this.songInformation.listOfSongsWithoutTriggers)
            {
                if (song.name == name)
                    return song.getPathToSong();
            }
            return "";
        }

        public override bool isPlaying()
        {
            return this.sound?.State == SoundState.Playing;
        }
    }
}
