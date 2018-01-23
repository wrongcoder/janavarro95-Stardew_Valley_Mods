using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardewSymphonyRemastered.Framework
{
    /// <summary>
    /// TODO: Make this work and add in overrided functions.
    /// </summary>
   public class XwbMusicPack: MusicPack
    {
        public Microsoft.Xna.Framework.Audio.WaveBank WaveBank;
        public Microsoft.Xna.Framework.Audio.SoundBank SoundBank;

        public StardewSymphonyRemastered.Framework.SongSpecifics songInformation;

        public string XWBPath;

        public XwbMusicPack(string name, string directoryToXwb,string pathToXWB)
        {
            this.name = name;
            this.directory = directoryToXwb;
            this.XWBPath = pathToXWB;
            this.songInformation = new SongSpecifics();
        }

        public override void loadMusicFiles()
        {
          this.listOfSongs=StardewSymphonyRemastered.Framework.MusicHexProcessor.ProcessSongNamesFromHex(this,Class1.Reset,this.XWBPath);
        }
    }
}
