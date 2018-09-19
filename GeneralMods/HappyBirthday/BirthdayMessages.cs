using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.HappyBirthday
{
    public class BirthdayMessages
    {
        /// <summary>
        /// The actual birthday wishes given by an npc.
        /// </summary>
        public Dictionary<string, string> birthdayWishes;

        public Dictionary<string, string> spouseBirthdayWishes;

        /// <summary>
        /// TODO: Make this.
        /// </summary>
        private Dictionary<string, string> defaultSpouseBirthdayWishes = new Dictionary<string, string>()
        {
            ["Alex"] = "",
            ["Elliott"] = "",
            ["Harvey"] = "",
            ["Sam"] = "",
            ["Sebastian"] = "",
            ["Shane"] = "",
            ["Abigail"] = "",
            ["Emily"] = "",
            ["Haley"] = "",
            ["Leah"] = "",
            ["Maru"] = "",
            ["Penny"] = "",
        };

        /// <summary>
        /// Used to contain
        /// </summary>
        private Dictionary<string, string> defaultBirthdayWishes = new Dictionary<string, string>()
        {
            ["Robin"] = "Hey @, happy birthday! I'm glad you choose this town to move here to. ",
            ["Demetrius"] = "Happy birthday @! Make sure you take some time off today to enjoy yourself. $h",
            ["Maru"] = "Happy birthday @. I tried to make you an everlasting candle for you, but sadly that didn't work out. Maybe next year right? $h",
            ["Sebastian"] = "Happy birthday @. Here's to another year of chilling. ",
            ["Linus"] = "Happy birthday @. Thanks for visiting me even on your birthday. It makes me really happy. ",
            ["Pierre"] = "Hey @, happy birthday! Hopefully this next year for you will be a great one! ",
            ["Caroline"] = "Happy birthday @. Thank you for all that you've done for our community. I'm sure your parents must be proud of you.$h",
            ["Abigail"] = "Happy birthday @! Hopefully this year we can go on even more adventures together $h!",
            ["Alex"] = "Yo @, happy birthday! Maybe this will be your best year yet.$h",
            ["George"] = "When you get to my age birthdays come and go. Still happy birthday @.",
            ["Evelyn"] = "Happy birthday @. You have grown up to be such a fine individual and I'm sure you'll continue to grow. ",
            ["Lewis"] = "Happy birthday @! I'm thankful for what you have done for the town and I'm sure your grandfather would be proud of you.",
            ["Clint"] = "Hey happy birthday @. I'm sure this year is going to be great for you.",
            ["Penny"] = "Happy birthday @. May you enjoy all of life's blessings this year. ",
            ["Pam"] = "Happy birthday kid. We should have a drink to celebrate another year of life for you! $h",
            ["Emily"] = "I'm sensing a strong positive life energy about you, so it must be your birthday. Happy birthday @!$h",
            ["Haley"] = "Happy birthday @. Hopefully this year you'll get some good presents!$h",
            ["Jas"] = "Happy birthday @. I hope you have a good birthday.",
            ["Vincent"] = "Hey @ have you come to pl...oh it's your birthday? Happy birthday! ",
            ["Jodi"] = "Hello there @. Rumor has it that today is your birthday. In that case, happy birthday!$h",
            ["Kent"] = "Jodi told me that it was your birthday today @. Happy birthday and make sure to cherish every single day.",
            ["Sam"] = "Yo @ happy birthday! We'll have to have a birthday jam session for you some time!$h ",
            ["Leah"] = "Hey @ happy birthday! We should go to the saloon tonight and celebrate!$h ",
            ["Shane"] = "Happy birthday @. Keep working hard and I'm sure this next year for you will be a great one.",
            ["Marnie"] = "Hello there @. Everyone is talking about your birthday today and I wanted to make sure that I wished you a happy birthday as well, so happy birthday! $h ",
            ["Elliott"] = "What a wonderful day isn't it @? Especially since today is your birthday. I tried to make you a poem but I feel like the best way of putting it is simply, happy birthday. $h ",
            ["Gus"] = "Hey @ happy birthday! Hopefully you enjoy the rest of the day and make sure you aren't a stranger at the saloon!",
            ["Dwarf"] = "Happy birthday @. I hope that what I got you is acceptable for humans as well. ",
            ["Wizard"] = "The spirits told me that today is your birthday. In that case happy birthday @. ",
            ["Harvey"] = "Hey @, happy birthday! Make sure to come in for a checkup some time to make sure you live many more years! ",
            ["Sandy"] = "Hello there @. I heard that today was your birthday and I didn't want you feeling left out, so happy birthday!",
            ["Willy"] = "Aye @ happy birthday. Looking at you reminds me of ye days when I was just a guppy swimming out to sea. Continue to enjoy them youngin.$h",
            ["Krobus"] = "I have heard that it is tradition to give a gift to others on their birthday. In that case, happy birthday @."
        };

        /// <summary>
        /// Used to load all of the default birthday greetings.
        /// </summary>
        public void createBirthdayGreetings()
        {

            var serializer = JsonSerializer.Create();
            serializer.Formatting = Formatting.Indented;

            //English logic.
            string defaultPath = Path.Combine(HappyBirthday.ModHelper.DirectoryPath, "Content", "Dialogue", "English");
            if (!Directory.Exists(defaultPath)) Directory.CreateDirectory(defaultPath);

            string birthdayFileDict=HappyBirthday.Config.translationInfo.getjsonForTranslation("BirthdayWishes", HappyBirthday.Config.translationInfo.currentTranslation);
            string path = Path.Combine(HappyBirthday.ModHelper.DirectoryPath, "Content", "Dialogue","English", birthdayFileDict);

            //Handle normal birthday wishes.
            if (!File.Exists(path))
            {

                StreamWriter writer = new StreamWriter(path);
                serializer.Serialize(writer, defaultBirthdayWishes);
                this.birthdayWishes = defaultBirthdayWishes;
            }
            else
            {
                StreamReader reader = new StreamReader(path);
                birthdayWishes = new Dictionary<string, string>();
                birthdayWishes = (Dictionary<string, string>)serializer.Deserialize(reader, typeof(Dictionary<string, string>));
            }

            //handle spouse birthday wishes.
            string spouseBirthdayFileDict = HappyBirthday.Config.translationInfo.getjsonForTranslation("SpouseBirthdayWishes", HappyBirthday.Config.translationInfo.currentTranslation);
            string spousePath = Path.Combine(HappyBirthday.ModHelper.DirectoryPath, "Content", "Dialogue","English",spouseBirthdayFileDict);
            if (!File.Exists(path))
            {

                StreamWriter writer = new StreamWriter(spousePath);
                serializer.Serialize(writer, defaultSpouseBirthdayWishes);
                this.spouseBirthdayWishes = defaultSpouseBirthdayWishes;
            }
            else
            {
                StreamReader reader = new StreamReader(path);
                birthdayWishes = new Dictionary<string, string>();
                birthdayWishes = (Dictionary<string, string>)serializer.Deserialize(reader, typeof(Dictionary<string, string>));
            }

            //Non-english logic
            foreach(var translation in HappyBirthday.Config.translationInfo.translationCodes)
            {
                if (translation.Key == "English") continue;
                string basePath = Path.Combine(HappyBirthday.ModHelper.DirectoryPath, "Content", "Dialogue", translation.Key);
                if (!Directory.Exists(basePath)) Directory.CreateDirectory(basePath);
                string tempBirthdayFile =Path.Combine(basePath,HappyBirthday.Config.translationInfo.getjsonForTranslation("BirthdayWishes", translation.Key));
                string tempSpouseBirthdayFile =Path.Combine(basePath,HappyBirthday.Config.translationInfo.getjsonForTranslation("SpouseBirthdayWishes", translation.Key));

                
                Dictionary<string, string> tempBirthdayDict = new Dictionary<string, string>();
                foreach(var pair in defaultBirthdayWishes)
                {
                    tempBirthdayDict.Add(pair.Key, "");
                }
                StreamWriter writer = new StreamWriter(tempBirthdayFile);
                serializer.Serialize(writer, tempBirthdayDict);


                Dictionary<string, string> tempSpouseBirthdayDict = new Dictionary<string, string>();
                foreach (var pair in defaultSpouseBirthdayWishes)
                {
                    tempSpouseBirthdayDict.Add(pair.Key, "");
                }
                StreamWriter writer2 = new StreamWriter(tempSpouseBirthdayFile);
                serializer.Serialize(writer, tempSpouseBirthdayDict);
            }
        }


    }
}
