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

        public static string[] getRandomNegativeItemSlanderNouns(string translation)
        {
            string[] strArray = Vocalization.config.translationInfo.LoadString(Path.Combine("Strings","Lexicon:RandomNegativeItemNoun"),translation).Split('#');
            return strArray;
        }

        public static string[] getRandomDeliciousAdjectives(string translation,NPC n = null)
        {
            string[] strArray;
            if (n != null && n.Age == 2)
                strArray = Vocalization.config.translationInfo.LoadString(Path.Combine("Strings","Lexicon:RandomDeliciousAdjective_Child"),translation).Split('#');
            else
                strArray = Vocalization.config.translationInfo.LoadString(Path.Combine("Strings","Lexicon:RandomDeliciousAdjective"),translation).Split('#');
            return strArray;
        }

        public static string[] getRandomNegativeFoodAdjectives(string translation,NPC n = null)
        {
            string[] strArray;
            if (n != null && n.Age == 2)
                strArray = Vocalization.config.translationInfo.LoadString(Path.Combine("Strings","Lexicon:RandomNegativeFoodAdjective_Child"),translation).Split('#');
            else if (n != null && n.Manners == 1)
                strArray = Vocalization.config.translationInfo.LoadString(Path.Combine("Strings","Lexicon:RandomNegativeFoodAdjective_Polite"),translation).Split('#');
            else
                strArray = Vocalization.config.translationInfo.LoadString(Path.Combine("Strings","Lexicon:RandomNegativeFoodAdjective"),translation).Split('#');
            return strArray;
        }

        public static string[] getRandomSlightlyPositiveAdjectivesForEdibleNoun(string translation,NPC n = null)
        {
            string[] strArray = Vocalization.config.translationInfo.LoadString(Path.Combine("Strings","Lexicon:RandomSlightlyPositiveFoodAdjective"),translation).Split('#');
            return strArray;
        }

        public static string[] getRandomNegativeAdjectivesForEventOrPerson(string translation,NPC n = null)
        {
            Random random = new Random((int)Game1.stats.DaysPlayed + (int)Game1.uniqueIDForThisGame / 2);
            string[] strArray;
            if (n != null && n.Age != 0)
                strArray = Vocalization.config.translationInfo.LoadString(Path.Combine("Strings","Lexicon:RandomNegativeAdjective_Child"),translation).Split('#');
            else if (n != null && n.Gender == 0)
                strArray = Vocalization.config.translationInfo.LoadString(Path.Combine("Strings","Lexicon:RandomNegativeAdjective_AdultMale"),translation).Split('#');
            else if (n != null && n.Gender == 1)
                strArray = Vocalization.config.translationInfo.LoadString(Path.Combine("Strings","Lexicon:RandomNegativeAdjective_AdultFemale"),translation).Split('#');
            else
                strArray = Vocalization.config.translationInfo.LoadString(Path.Combine("Strings","Lexicon:RandomNegativeAdjective_PlaceOrEvent"),translation).Split('#');
            return strArray;
        }

        public static string[] getRandomPositiveAdjectivesForEventOrPerson(string translation,NPC n = null)
        {
            Random random = new Random((int)Game1.stats.DaysPlayed + (int)Game1.uniqueIDForThisGame / 2);
            string[] strArray;
            if (n != null && n.Age != 0)
                strArray = Vocalization.config.translationInfo.LoadString(Path.Combine("Strings","Lexicon:RandomPositiveAdjective_Child"),translation).Split('#');
            else if (n != null && n.Gender == 0)
                strArray = Vocalization.config.translationInfo.LoadString(Path.Combine("Strings","Lexicon:RandomPositiveAdjective_AdultMale"),translation).Split('#');
            else if (n != null && n.Gender == 1)
                strArray = Vocalization.config.translationInfo.LoadString(Path.Combine("Strings","Lexicon:RandomPositiveAdjective_AdultFemale"),translation).Split('#');
            else
                strArray = Vocalization.config.translationInfo.LoadString(Path.Combine("Strings","Lexicon:RandomPositiveAdjective_PlaceOrEvent"),translation).Split('#');
            return strArray;
        }

    }
}
