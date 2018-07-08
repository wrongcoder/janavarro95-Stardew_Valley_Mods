using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vocalization.Framework
{
    public class ReplacementStrings
    {

        public string farmerName = "<Player's Name>";
        public string bandName = "<Sam's Band Name>";
        public string bookName = "<Elliott's Book Name>";
        public string rivalName = "<Rival's Name>";
        public string petName = "<Pet's Name>";
        public string farmName = "<Farm Name>";
        public string favoriteThing = "<Favorite Thing>";
        public string kid1Name = "<Kid 1's Name>";
        public string kid2Name = "<Kid 2's Name>";

        public List<string> adjStrings;
        public List<string> nounStrings;
        public List<string> placeStrings;
        public List<string> spouseNames;


        public ReplacementStrings()
        {
            loadAdjStrings();
            loadNounStrings();
            loadPlaceStrings();
            loadSpouseStrings();
        } 

        public void loadAdjStrings()
        {
            adjStrings = new List<string>();
        }

        public void loadNounStrings()
        {
            nounStrings = new List<string>();
        }

        public void loadPlaceStrings()
        {
            placeStrings = new List<string>();
        }

        /// <summary>
        /// Load all associated spouse names.
        /// </summary>
        public void loadSpouseStrings()
        {
            spouseNames = new List<string>();
            foreach(var loc in Game1.locations)
            {
                foreach(var character in loc.characters)
                {
                    if (character.datable.Value)
                    {
                        spouseNames.Add(character.Name);
                    }
                }
            }
        }


    }
}
