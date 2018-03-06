using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardewSymphonyRemastered.Framework
{
    class SongListNode
    {
        public string trigger;
        public List<Song> songList;

        public SongListNode()
        {

        }

        public SongListNode(string Trigger, List<Song> SongList)
        {
            this.trigger = Trigger;
            this.songList = SongList;
        }

        public void WriteToJson(string path)
        {
            StardewSymphony.ModHelper.WriteJsonFile(path, this);
        }
        
        public static SongListNode ReadFromJson(string path)
        {
            return StardewSymphony.ModHelper.ReadJsonFile<SongListNode>(path);
        }

    }
}
