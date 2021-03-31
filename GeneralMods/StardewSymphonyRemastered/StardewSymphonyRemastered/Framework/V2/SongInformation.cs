using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardewSymphonyRemastered.Framework.V2
{
    public class SongInformation
    {
        public string name;
        public Dictionary<string,SongConditionals> songConditionals;

        public SongInformation()
        {

            this.songConditionals = new Dictionary<string, SongConditionals>();
        }

        public SongInformation(string name):this()
        {
            this.name = name;
        }
        public SongInformation(string name,string key):this()
        {

            
        }

        public void AddSongConditional(string key)
        {
            if (this.songConditionals.ContainsKey(key)) return;
            else
            {
                this.songConditionals.Add(key, new SongConditionals(key));
            }
        }
        public void RemoveSongConditional(string key)
        {
            if (!this.songConditionals.ContainsKey(key)) return;
            else
            {
                this.songConditionals.Remove(key);
            }
        }
        /// <summary>
        /// Checks to see if a song can be played given a set of conditionals.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool canBePlayed(string key)
        {
            foreach(KeyValuePair<string,SongConditionals> pair in this.songConditionals)
            {
                SongConditionals temp = new SongConditionals(key);
                if (pair.Value.canBePlayed(temp) == true) return true;
            }
            return false;
        }

    }
}
