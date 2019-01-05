using System.Collections.Generic;

namespace StardewSymphonyRemastered.Framework
{
    /// <summary>A class that keeps track of the trigger and the list of songs associated with that trigger.</summary>
    class SongListNode
    {
        /// <summary>The trigger name for the list of songs.</summary>
        public string trigger;

        /// <summary>The list of songs associated with a trigger.</summary>
        public List<Song> songList;

        /// <summary>Construct an instance.</summary>
        public SongListNode(string Trigger, List<Song> SongList)
        {
            this.trigger = Trigger;
            this.songList = SongList;
        }

        /// <summary>Save functionality.</summary>
        public void WriteToJson(string path)
        {
            StardewSymphony.ModHelper.WriteJsonFile(path, this);
        }

        /// <summary>Load functionality.</summary>
        public static SongListNode ReadFromJson(string path)
        {
            return StardewSymphony.ModHelper.ReadJsonFile<SongListNode>(path);
        }
    }
}
