namespace StardewSymphonyRemastered.Framework
{
    /// <summary>The class to be used to manage individual songs.</summary>
    public class Song
    {
        /// <summary>The name of the song itself.</summary>
        public string name;

        /// <summary>Constructor to be used for WAV files.</summary>
        public Song(string name)
        {
            this.name = name;
        }
    }
}
