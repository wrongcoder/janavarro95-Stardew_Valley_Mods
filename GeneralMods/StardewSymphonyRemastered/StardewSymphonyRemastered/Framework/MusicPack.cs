using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardewSymphonyRemastered.Framework
{
    /// <summary>
    /// A base class that xnb and wav packs will derive commonalities from.
    /// </summary>
    public class MusicPack 
    {
        public string name;
        public string directory;
        public List<string> listOfSongs;


        public virtual void playSong(string name)
        {

        }

        public virtual void pauseSong(string name)
        {

        }

        public virtual void stopSong(string name)
        {

        }

        public virtual void returnSong(string name)
        {

        }

        public virtual void loadMusicFiles()
        {

        }

    }
}
