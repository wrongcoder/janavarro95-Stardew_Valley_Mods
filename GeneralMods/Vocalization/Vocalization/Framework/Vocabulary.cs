using StardewValley;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vocalization.Framework
{
    public class Vocabulary
    {

        public static string[] getRandomNegativeItemSlanderNouns()
        {
            string[] strArray = Game1.content.LoadString(Path.Combine("Strings","Lexicon:RandomNegativeItemNoun")).Split('#');
            return strArray;
        }

        public static string[] getRandomDeliciousAdjectives(NPC n = null)
        {
            string[] strArray;
            if (n != null && n.Age == 2)
                strArray = Game1.content.LoadString("Strings\\Lexicon:RandomDeliciousAdjective_Child").Split('#');
            else
                strArray = Game1.content.LoadString("Strings\\Lexicon:RandomDeliciousAdjective").Split('#');
            return strArray;
        }

        public static string[] getRandomNegativeFoodAdjectives(NPC n = null)
        {
            string[] strArray;
            if (n != null && n.Age == 2)
                strArray = Game1.content.LoadString("Strings\\Lexicon:RandomNegativeFoodAdjective_Child").Split('#');
            else if (n != null && n.Manners == 1)
                strArray = Game1.content.LoadString("Strings\\Lexicon:RandomNegativeFoodAdjective_Polite").Split('#');
            else
                strArray = Game1.content.LoadString("Strings\\Lexicon:RandomNegativeFoodAdjective").Split('#');
            return strArray;
        }

        public static string[] getRandomSlightlyPositiveAdjectivesForEdibleNoun(NPC n = null)
        {
            string[] strArray = Game1.content.LoadString("Strings\\Lexicon:RandomSlightlyPositiveFoodAdjective").Split('#');
            return strArray;
        }

        public static string[] getRandomNegativeAdjectivesForEventOrPerson(NPC n = null)
        {
            Random random = new Random((int)Game1.stats.DaysPlayed + (int)Game1.uniqueIDForThisGame / 2);
            string[] strArray;
            if (n != null && n.Age != 0)
                strArray = Game1.content.LoadString("Strings\\Lexicon:RandomNegativeAdjective_Child").Split('#');
            else if (n != null && n.Gender == 0)
                strArray = Game1.content.LoadString("Strings\\Lexicon:RandomNegativeAdjective_AdultMale").Split('#');
            else if (n != null && n.Gender == 1)
                strArray = Game1.content.LoadString("Strings\\Lexicon:RandomNegativeAdjective_AdultFemale").Split('#');
            else
                strArray = Game1.content.LoadString("Strings\\Lexicon:RandomNegativeAdjective_PlaceOrEvent").Split('#');
            return strArray;
        }

        public static string[] getRandomPositiveAdjectivesForEventOrPerson(NPC n = null)
        {
            Random random = new Random((int)Game1.stats.DaysPlayed + (int)Game1.uniqueIDForThisGame / 2);
            string[] strArray;
            if (n != null && n.Age != 0)
                strArray = Game1.content.LoadString("Strings\\Lexicon:RandomPositiveAdjective_Child").Split('#');
            else if (n != null && n.Gender == 0)
                strArray = Game1.content.LoadString("Strings\\Lexicon:RandomPositiveAdjective_AdultMale").Split('#');
            else if (n != null && n.Gender == 1)
                strArray = Game1.content.LoadString("Strings\\Lexicon:RandomPositiveAdjective_AdultFemale").Split('#');
            else
                strArray = Game1.content.LoadString("Strings\\Lexicon:RandomPositiveAdjective_PlaceOrEvent").Split('#');
            return strArray;
        }

    }
}
