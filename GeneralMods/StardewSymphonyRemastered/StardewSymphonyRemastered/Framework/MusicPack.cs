using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardewSymphonyRemastered.Framework
{
    /// <summary>
    /// A base class that xnb and wav packs will derive commonalities from.
    /// //Make Music Pack Meta data
    /// </summary>
    public class MusicPack 
    {
        public string directory;
        public string shortenedDirectory;
        public StardewSymphonyRemastered.Framework.SongSpecifics songInformation;
        public MusicPackMetaData musicPackInformation;


        public virtual void playSong(string name)
        {

        }

        public virtual void pauseSong()
        {

        }

        public virtual void stopSong()
        {

        }

        public virtual void resumeSong()
        {

        }

        public virtual void loadMusicFiles()
        {

        }

        public virtual void swapSong(string songName)
        {

        }

        public virtual string getNameOfCurrentSong()
        {
            return "";
        }

        public virtual void setModDirectoryFromFullDirectory()
        {
            
        }
    }
}
