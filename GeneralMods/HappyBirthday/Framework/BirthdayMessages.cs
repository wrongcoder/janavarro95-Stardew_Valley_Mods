using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Omegasis.HappyBirthday.Framework.ContentPack;
using Omegasis.HappyBirthday.Framework.Localization;
using StardewModdingAPI;
using StardewValley;
using static StardewValley.LocalizedContentManager;

namespace Omegasis.HappyBirthday
{
    public class BirthdayMessages
    {
        /// <summary>The actual birthday wishes given by an npc.</summary>
        public Dictionary<string, string> birthdayWishes;

        public Dictionary<string, string> spouseBirthdayWishes;

        public Dictionary<string, string> defaultSpouseBirthdayWishes = new Dictionary<string, string>()
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

        public Dictionary<string, Func<string, string>> spouseEnglishGeneratedMessages = new Dictionary<string, Func<string, string>>();

        /// <summary>Used to contain birthday wishes should the mod not find any available. These were written nexus mods user cerreli.</summary>
        public Dictionary<string, string> defaultBirthdayWishes = new Dictionary<string, string>()
        {

            ["Robin"] = "Hey, @, happy birthday! I'm really glad you decided to move to the valley. ",
            ["Demetrius"] = "Happy birthday, @! Make sure you take some time off today to enjoy yourself. $h",
            ["Maru"] = "Happy birthday, @. I tried to make an everlasting candle for you, but sadly that didn't work out. Maybe next year, right? $h",
            ["Sebastian"] = "Happy birthday, @. Hope things are going well down at the farm.",
            ["Linus"] = "Happy birthday, @. Thanks for visiting me even on your birthday. It makes me really happy. ",
            ["Pierre"] = "Hey @, happy birthday! Hopefully this next year will be a great one for you! ",
            ["Caroline"] = "Happy birthday, @. Thank you for all that you've done for our community. I'm sure your parents must be proud of you.$h",
            ["Abigail"] = "Happy birthday, @! Hopefully this year we can go on even more adventures together $h!",
            ["Alex"] = "Yo @, happy birthday! Here's to making this the best year yet!$h",
            ["George"] = "When you get to my age, birthdays start to come and go. Still, happy birthday, @.",
            ["Evelyn"] = "Happy birthday, @. You have grown into such a fine individual, and I'm sure your grandfather would be proud to see who you've become. ",
            ["Lewis"] = "Happy birthday, @! I really appreciate everything you've done for the town. Keep up the good work.",
            ["Clint"] = "Hey, happy birthday, @. I'm sure this year is going to be great for you.",
            ["Penny"] = "Happy birthday, @. May you enjoy all of life's blessings this year. ",
            ["Pam"] = "Happy birthday, kid. We should have a drink to celebrate another year of life for you! $h",
            ["Emily"] = "Happy birthday, @! I can see your future shining bright.$h",
            ["Haley"] = "Happy birthday, @. Hopefully this year you'll get some good presents!$h",
            ["Jas"] = "Happy birthday, @. I'm glad I decided to get to know you.",
            ["Vincent"] = "Hey, @, have you come to pl... oh, it's your birthday? Happy birthday! ",
            ["Jodi"] = "Hello there, @. Rumor has it that today is your birthday. In that case, happy birthday!$h",
            ["Kent"] = "Jodi told me that it was your birthday today, @. Happy birthday, and make sure to cherish every single day.",
            ["Sam"] = "Hey @, happy birthday! We'll have to have a birthday jam session for you sometime!$h ",
            ["Leah"] = "Hey @, happy birthday! I'm glad to see you're doing well on the farm.$h ",
            ["Shane"] = "Happy birthday, @. Keep working hard, and I'm sure this next year will be a great one for you.",
            ["Marnie"] = "Hi, @! Everyone is talking about your birthday today, and I wanted to make sure that I wished you a happy birthday as well. So, happy birthday! $h ",
            ["Elliott"] = "What a wonderful day, isn't it, @? Though I'm sure it's even lovelier to you, it being your birthday!$h#$b#I was actually in the middle of writing a poem for the occasion, but a 'happy birthday' will have to suffice for now.",
            ["Gus"] = "Hey, @, happy birthday! Hopefully you enjoy the rest of the day, and make sure you aren't a stranger at the saloon!",
            ["Dwarf"] = "Happy birthday, @. Wait, humans celebrate those, right?",
            ["Wizard"] = "Happy birthday, @. Never forget that you alone make your future.#$e#How did I know? Ah, I overheard a few of the locals conferring on what gifts to give you. I think you'll appreciate them.",
            ["Harvey"] = "Happy birthday, @. Come in for a checkup sometime, alright? I want to make sure you'll see plenty more.",
            ["Sandy"] = "Aww, sweetie, you came all the way out here to see me on your birthday?$h#$b#Well, then, the least I can do is wish you a happy one!~",
            ["Willy"] = "Ahoy, @, happy birthday. Looking at you reminds me of the days when I was just a guppy myself. $bEnjoy yours while you can, young'un.$h",
            ["Krobus"] = "Happy birthday, @. I still don't quite understand humans, but I'm glad to have met you all the same."

        };

        public BirthdayMessages()
        {
            this.spouseEnglishGeneratedMessages.Add("Alex", this.createSpouseBirthdayDialogue);
            this.spouseEnglishGeneratedMessages.Add("Elliott", this.createSpouseBirthdayDialogue);
            this.spouseEnglishGeneratedMessages.Add("Harvey", this.createSpouseBirthdayDialogue);
            this.spouseEnglishGeneratedMessages.Add("Sam", this.createSpouseBirthdayDialogue);
            this.spouseEnglishGeneratedMessages.Add("Sebastian", this.createSpouseBirthdayDialogue);
            this.spouseEnglishGeneratedMessages.Add("Shane", this.createSpouseBirthdayDialogue);
            this.spouseEnglishGeneratedMessages.Add("Abigail", this.createSpouseBirthdayDialogue);
            this.spouseEnglishGeneratedMessages.Add("Emily", this.createSpouseBirthdayDialogue);
            this.spouseEnglishGeneratedMessages.Add("Haley", this.createSpouseBirthdayDialogue);
            this.spouseEnglishGeneratedMessages.Add("Leah", this.createSpouseBirthdayDialogue);
            this.spouseEnglishGeneratedMessages.Add("Maru", this.createSpouseBirthdayDialogue);
            this.spouseEnglishGeneratedMessages.Add("Penny", this.createSpouseBirthdayDialogue);
        }


        public Dictionary<StardewValley.LocalizedContentManager.LanguageCode, Dictionary<string, string>> translatedStrings = new Dictionary<StardewValley.LocalizedContentManager.LanguageCode, Dictionary<string, string>>()
        {


            [StardewValley.LocalizedContentManager.LanguageCode.en] = new Dictionary<string, string>()
            {
                ["BirthdayMom"] = "Dear @,^  Happy birthday sweetheart. It's been amazing watching you grow into the kind, hard working person that I've always dreamed that you would become. I hope you continue to make many more fond memories with the ones you love. ^  Love, Mom ^ P.S. Here's a little something that I made for you. %item object 221 1 %%",
                ["BirthdayDad"] = "Dear @,^  Happy birthday kiddo. It's been a little quiet around here on your birthday since you aren't around, but your mother and I know that you are making both your grandpa and us proud.  We both know that living on your own can be tough but we believe in you one hundred percent, just keep following your dreams.^  Love, Dad ^ P.S. Here's some spending money to help you out on the farm. Good luck! %item money 5000 5001 %%",
                ["BirthdayJunimos"] = "Please come to the community center. ^ Sincerly,^      -The Junimos",
                ["BirthdayDatingPenny"] = "Dear @. ^ My mom and I decided to have a little birthday party for you. Could you come by sometime today?^Sincerly,^      -Penny",
                ["BirthdayDatingMaru"] = "Hey @. ^ My family and I decided to have a birthday party for you at our place. Could you come by our house sometime today?^Sincerly,^      -Maru",
                ["BirthdayDatingSebastian"] = "Hey @. ^ Could you come by my place later? My family and I decided to have a birthday party for you to celebrate. ^^      -Sebastian",
                ["BirthdayDatingLeah"] = "Hey @. ^ Could you come by my place later? I thought it would be nice if we had a small party for you. ^Sincerly,^      -Leah",
                ["BirthdayDatingAbigail"] = "Hey @. ^ My family and I decided to have a birthday party for you at our place. Could you come by our house sometime today?^Sincerly,^      -Abigail",
                ["BirthdayDatingAbigail_Wednesday"] = "Hey @. ^ I thought it would be fun if we had a small party for you today. Could you come by the mines later?^ Sincerly,^      -Abigail",
                ["BirthdayDatingEmily"] = "Hi @. ^ I thought it would be nice to have a birthday party for you at our place. Could you come by my house sometime today?^Sincerly,^      -Emily",
                ["BirthdayDatingHaley"] = "Hey @. ^ I thought it would be nice to have a birthday party for you at our place. Could you come by my house sometime today?^Sincerly,^      -Haley",
                ["BirthdayDatingHarvey"] = "Hey @. ^ I thought it would be nice to have a birthday party for you at my place. Could you come by my room on the second floor of the clinic sometime today?^Sincerly,^      -Harvey",
                ["BirthdayDatingElliott"] = "Hello @. ^ I thought it would be nice to have a birthday party for you at my place. Could you come by my humble home later today?^Sincerly,^      -Elloitt",
                ["BirthdayDatingSam"] = "Hey @. ^ I thought it would be fun to have a birthday party for you at our place. Could you come by my house sometime today?^^      -Sam",
                ["BirthdayDatingAlex"] = "Hey @. ^ I thought it would be fun to have a birthday party for you at our place. Could you come by my house sometime today?^^      -Alex",
                ["BirthdayDatingShane"] = "Hey @. ^Could you come by my house sometime today? I thought I'd have a little get together for you.^^      -Alex",

                ["Happy Birthday: Star Message"] = "It's your birthday today! Happy birthday!",
                ["Happy Birthday: Farmhand Birthday Message"] = "It's @'s birthday! Happy birthday to them!",
                ["Season"] = "Season",
                ["Date"] = "Date",
                ["BirthdayError_FestivalDay"]="You can't have your birthday on a festival day. Sorry!",
                ["FavoriteGift"] = "Favorite Gift",

                ["JunimoBirthdayParty_0"] = "It looks like the junimos wanted to throw you a party!",
                ["JunimoBirthdayParty_1"] = "It looks like there was some cake left over too!",

                ["DatingPennyBirthday_Pam:0"] = "Come on in kid. The party has just begun!$h",
                ["DatingPennyBirthday_Pam:1"] = "Here, pull up a seat and have a beer to celebrate!",
                ["DatingPennyBirthday_Pam:2"] = "Alright, cheers kid! Happy birthday and here is to another great year! $h",
                ["DatingPennyBirthday_Penny:0"] = "Oh, @ you are here just in time!$h",
                ["DatingPennyBirthday_Penny:1"] = "I thought it would be nice if we threw you a small party. Granted it's not much but I hope you like it. $l",
                ["DatingPennyBirthday_Penny:2"] = "Mom!$a",
                ["DatingPennyBirthday_Penny:3"] = "*sigh* Well make yourself at home. I'll get the cake out.",
                ["DatingPennyBirthday_Penny:4"] = "Happy birthday @. Here is hoping we get to spend many more birthdays together. $l",
                ["DatingPennyBirthday_Finish:0"] = "It was nice celebrating my birthday with Pam and Penny.",
                ["DatingPennyBirthday_Finish:1"] = "Looks like there was some leftover cake and beer too!",

                ["DatingMaruBirthday_Demetrius:0"] = "Welcome @, come in and make yourself at home.$h",
                ["DatingMaruBirthday_Demetrius:1"] = "I agree. I think this party is perfecty wonderful. Besides studies show that your productivity is boosted when you have fun once in a while.$h",
                ["DatingMaruBirthday_Maru:0"] = "Ohh @, you are just in time for the party.$h",
                ["DatingMaruBirthday_Maru:1"] = "I tried to build you a robot to help you out on your farm as a gift but I ran out of time. Hopefully I'll have it done by next year.",
                ["DatingMaruBirthday_Maru:2"] = "Alright, @ make a wish!",
                ["DatingMaruBirthday_Maru:3"] = "Happy birthday @. Hopefully this is just the beginning of many more years to come.$l",
                ["DatingMaruBirthday_Robin:0"] = "You know I have to agree. I think it's the thought that counts not necessarily the gift.$h",
                ["DatingMaruBirthday_Robin:1"] = "Oh I think the cake is ready!",
                ["DatingMaruBirthday_Sebastian:0"] = "You know I think just having this party is good enough. No need to go overboard.",
                ["DatingMaruBirthday_Sebastian:1"] = "Sweet, let's eat.",
                ["DatingMaruBirthday_Finish:0"] = "It was nice celebrating my birthday with Maru and her family.",
                ["DatingMaruBirthday_Finish:1"] = "It looks like there was some leftover cake too!",

                ["DatingSebastianBirthday_Demetrius:0"] = "I agree. I think this party is perfecty wonderful. Besides studies show that your productivity is boosted when you have fun once in a while.$h",
                ["DatingSebastianBirthday_Demetrius:1"] = "Alright, @ go ahead and make a wish!$h",
                ["DatingSebastianBirthday_Maru:0"] = "Happy birthday @. Honestly, I'm a bit surprised that we are having this party. Sebastian never been too keen on celebrations.",
                ["DatingSebastianBirthday_Maru:1"] = "Oh sweet we finally get to have some cake! Let me get you the first slice @!$h",
                ["DatingSebastianBirthday_Robin:0"] = "Welcome @, come in and make yourself at home.$h",
                ["DatingSebastianBirthday_Robin:1"] = "Hey now, I think that it's great that Sebastian wanted to have a party for @. $h",
                ["DatingSebastianBirthday_Robin:2"] = "Oh I think the cake is ready!",
                ["DatingSebastianBirthday_Sebastian:0"] = "Hey @ you are here just in time. Mom is just finishing the cake right now. $h",
                ["DatingSebastianBirthday_Sebastian:1"] = "Honestly... and they wonder why I don't do stuff like this more often.",
                ["DatingSebastianBirthday_Sebastian:2"] = "Happy Birthday @. I'm glad we got to spend time like this together. $h",
                ["DatingSebastianBirthday_Finish:0"] = "It was nice celebrating my birthday with Sebastian and his family.",
                ["DatingSebastianBirthday_Finish:1"] = "It looks like there was some leftover cake too!",

                ["DatingLeahBirthday_Leah:0"] = "Welcome @! Come in and make yourself at home!$h",
                ["DatingLeahBirthday_Leah:1"] = "I knew today was your birthday so I thought we would have a little celebration for you. I event got some cake for us.$h",
                ["DatingLeahBirthday_Leah:2"] = "You know I haven't celebrated a birthday with anyone in a few years, but somehow I felt like I really wanted to spend today with you.$l",
                ["DatingLeahBirthday_Leah:3"] = "I would have never though I'd change my mind about celebrations like this but I guess being with you has really opened me up again.$l",
                ["DatingLeahBirthday_Leah:4"] = "Anyways enough of this embarrasing talk. Happy birthday @. Now shall we have some cake?$h",
                ["DatingLeahBirthday_Finish:0"] = "It was nice celebrating my birthday with just Leah.",
                ["DatingLeahBirthday_Finish:1"] = "It looks like there was some leftover cake too!",

                ["DatingAbigailBirthday_Abigail:0"] = "Hey @! You are just in time for the party!$h",
                ["DatingAbigailBirthday_Abigail:1"] = "Wow this cake looks delicious mom!$h",
                ["DatingAbigailBirthday_Abigail:2"] = "Go ahead and make a wish @.",
                ["DatingAbigailBirthday_Abigail:3"] = "So @, what did you wish for? Hopefully for some awesome skills!",
                ["DatingAbigailBirthday_Abigail:4"] = "Anyways, happy birthday! I hope we get to have many more adventures to come. $l",
                ["DatingAbigailBirthday_Caroline:0"] = "Please, I'm sure your customers won't mind if you are gone for a few minutes. Anyways the cake is done.",
                ["DatingAbigailBirthday_Caroline:1"] = "*sigh* Honestly.",
                ["DatingAbigailBirthday_Caroline:2"] = "Well @ feel free to help yourself to as much as you like.$h",
                ["DatingAbigailBirthday_Pierre:0"] = "Make yourself at home @! I can't stay too long since I have to attend to the store but I thought a party would be a good idea!$h",
                ["DatingAbigailBirthday_Pierre:1"] = "Well of course it would be. It was made from the *highest* quality ingredients from the store! $h",
                ["DatingAbigailBirthday_Finish:0"] = "It was nice celebrating my birthday with Abigail and her family.",
                ["DatingAbigailBirthday_Finish:1"] = "It looks like there was some leftover cake too!",

                ["DatingAbigailBirthday_Mine_Abigail:0"] = "Hey @! You are just in time for the party. Well more like adventure!$h",
                ["DatingAbigailBirthday_Mine_Abigail:1"] = "I thought it would be fun if we spent some time together in the mine for your birthday. Nothing says an exciting birthday like some adventure right?$h",
                ["DatingAbigailBirthday_Mine_Abigail:2"] = "Don't worry I wasn't saying that we go fight monsters. I just thought a change of location could be exciting. Plus my house is closed today since my dad takes the day off on Wednesdays.",
                ["DatingAbigailBirthday_Mine_Abigail:3"] = "I'm just happy that we get to spend time together like this. I even brought some cake for us to make it a proper celebration. Go ahead and make a wish!",
                ["DatingAbigailBirthday_Mine_Abigail:4"] = "Anyways, happy birthday @! I hope we get to have many more adventures to come. $l",
                ["DatingAbigailBirthday_Mine_Finish:0"] = "It was nice celebrating my birthday with just Abigail.",
                ["DatingAbigailBirthday_Mine_Finish:1"] = "It looks like there was some leftover cake too!",

                ["DatingEmilyBirthday_Emily:0"] = "Hi, @ you are just in time for the celebration.$h",
                ["DatingEmilyBirthday_Emily:1"] = "I thought it would be nice to have a small party in your honor.",
                ["DatingEmilyBirthday_Emily:2"] = "Somehow I feel like birthdays are an important reminder for us to live in the moment and live with the harmony of nature.",
                ["DatingEmilyBirthday_Emily:3"] = "Anyways I made us some chocolate cake! It's made with all natural ingredients and sweetened with cactus syrup!$h",
                ["DatingEmilyBirthday_Emily:4"] = "Make a wish @!$h",
                ["DatingEmilyBirthday_Emily:5"] = "Happy birthday @! I feel a strong fortune energy coming from the spirits for you this year. $l",
                ["DatingEmilyBirthday_Finish:0"] = "It was nice celebrating my birthday with just Emily.",
                ["DatingEmilyBirthday_Finish:1"] = "It looks like there was some leftover cake too!",

                ["DatingHaleyBirthday_Haley:0"] = "Hey, @ you are just in time for the party!$h",
                ["DatingHaleyBirthday_Haley:1"] = "Well I mean it would be a party, but I thought it would be nice to have something with just the two of us. $l",
                ["DatingHaleyBirthday_Haley:2"] = "I think birthdays can be pretty fun when you do them right, but I wanted this to be special because... you are special you know?$l",
                ["DatingHaleyBirthday_Haley:3"] = "Anyways, I got us some cake to eat too! Make sure to make a wish alright?$h",
                ["DatingHaleyBirthday_Haley:4"] = "Happy Birthday @. I'm so glad we got to spend some time together. Let's take a picture to remember this moment. $l",
                ["DatingHaleyBirthday_Finish:0"] = "It was nice celebrating my birthday with just Haley.",
                ["DatingHaleyBirthday_Finish:1"] = "It looks like there was some leftover cake too!",

                ["DatingHarveyBirthday_Harvey:0"] = "Hi there, @ you are just in time! Let me clean up a bit.$h",
                ["DatingHarveyBirthday_Harvey:1"] = "I though it would be nice to have a small celebration for you so I cooked us something special.$l",
                ["DatingHarveyBirthday_Harvey:2"] = "Don't worry about the wine. A little moderation is needed but this is a special day!$h",
                ["DatingHarveyBirthday_Harvey:3"] = "Now cheers! Happy Birthday, here is to good health and many more years to come!$l",
                ["DatingHarveyBirthday_Finish:0"] = "It was nice celebrating my birthday with just Harvey.",
                ["DatingHarveyBirthday_Finish:1"] = "It looks like there was some leftover food too!",

                ["DatingElliottBirthday_Elliott:0"] = "Ahh @, wonderful! You are just in time! $h",
                ["DatingElliottBirthday_Elliott:1"] = "I thought it would be splendid if we had a small celebration together in honor of your birthday!$h",
                ["DatingElliottBirthday_Elliott:2"] = "I know that birthdays can be quite an ordeal and I'm sure you are quite busy today, but I do appreciate you coming out all this way to spend your special day with me. $l",
                ["DatingElliottBirthday_Elliott:3"] = "Alas, I tried writing something for you to express my affection for you but somehow words just couldn't seem to capture every little detail.$l",
                ["DatingElliottBirthday_Elliott:4"] = "Alright now how about we have some cake to mark the occassion. Go ahead and make a wish!",
                ["DatingElliottBirthday_Elliott:5"] = "Happy birthday @! Here is hoping there are many more stories to be told with your life! $l",
                ["DatingElliottBirthday_Finish:0"] = "It was nice celebrating my birthday with just Elliott.",
                ["DatingElliottBirthday_Finish:1"] = "It looks like there was some leftover cake too!",

                ["DatingSamBirthday_Sam:0"] = "Hey @ you are just in time. $h",
                ["DatingSamBirthday_Sam:1"] = "I thought it would be fun to have a small party for you since it's your birthday. Don't worry, my family is busy doing other things so we can just chill. $h",
                ["DatingSamBirthday_Sam:2"] = "I got us some pizza and some joja cola. I hope you like it! I would have made you a cake but I never really figured out how to do that sort of thing.$h",
                ["DatingSamBirthday_Sam:3"] = "Anyways happy birthday @. I'm super lucky to be with someone like you. $l",
                ["DatingSamBirthday_Finish:0"] = "It was nice celebrating my birthday with just Sam.",
                ["DatingSamBirthday_Finish:1"] = "It looks like there was some leftover food too!",


                ["DatingAlexBirthday_Alex:0"] = "Hey @ you are just in time. $h",
                ["DatingAlexBirthday_Alex:1"] = "I thought it would be fun to have a small party for you since it's your birthday. It's always a good idea to relax after working out so hard! $h",
                ["DatingAlexBirthday_Alex:2"] = "I got us some pizza and some joja cola. I hope you enjoy it! .$h",
                ["DatingAlexBirthday_Alex:3"] = "Happy birthday @. I'm super lucky to be with someone like you. $l",
                ["DatingAlexBirthday_Finish:0"] = "It was nice celebrating my birthday with just Alex.",
                ["DatingAlexBirthday_Finish:1"] = "It looks like there was some leftover food too!",


                ["DatingShaneBirthday_Shane:0"] = "Hey @ you are just in time. $h",
                ["DatingShaneBirthday_Shane:1"] = "I thought it would be good to have a party for you. $h",
                ["DatingShaneBirthday_Shane:2"] = "I got us some pizza and some joja cola. I hope you enjoy it! I would have cooked you something like a cake with eggs from the chickens but I wasn't too sure how to do it.$h",
                ["DatingShaneBirthday_Shane:3"] = "Anyways, happy birthday @. I'm glad to be with someone like you. $l",
                ["DatingShaneBirthday_Finish:0"] = "It was nice celebrating my birthday with just Shane.",
                ["DatingShaneBirthday_Finish:1"] = "It looks like there was some leftover food too!",

                ["PartyOver"] = "That was a fun party. Back to work!"

            },
        };


        /// <summary>
        /// Loads some strings from the StringsFromCS file for affectionate spouse words.
        /// </summary>
        /// <returns></returns>
        protected string getAffectionateSpouseWord()
        {

            List<string> words = new List<string>();
            string dict = Path.Combine("Strings", "StringsFromCSFiles");
            if (Game1.player.IsMale)
            {

                words.Add(HappyBirthday.Instance.translationInfo.loadStringFromXNBFile(dict, "NPC.cs.4507"));
                words.Add(HappyBirthday.Instance.translationInfo.loadStringFromXNBFile(dict, "NPC.cs.4509"));
                words.Add(HappyBirthday.Instance.translationInfo.loadStringFromXNBFile(dict, "NPC.cs.4511"));
                words.Add(HappyBirthday.Instance.translationInfo.loadStringFromXNBFile(dict, "NPC.cs.4514"));
                words.Add(HappyBirthday.Instance.translationInfo.loadStringFromXNBFile(dict, "NPC.cs.4515"));


            }
            else
            {
                words.Add(HappyBirthday.Instance.translationInfo.loadStringFromXNBFile(dict, "NPC.cs.4512"));
                words.Add(HappyBirthday.Instance.translationInfo.loadStringFromXNBFile(dict, "NPC.cs.4513"));

            }
            words.Add(HappyBirthday.Instance.translationInfo.loadStringFromXNBFile(dict, "NPC.cs.4508"));
            words.Add(HappyBirthday.Instance.translationInfo.loadStringFromXNBFile(dict, "NPC.cs.4510"));
            words.Add(HappyBirthday.Instance.translationInfo.loadStringFromXNBFile(dict, "NPC.cs.4516"));
            words.Add(HappyBirthday.Instance.translationInfo.loadStringFromXNBFile(dict, "NPC.cs.4517"));
            words.Add(HappyBirthday.Instance.translationInfo.loadStringFromXNBFile(dict, "NPC.cs.4518"));
            words.Add(HappyBirthday.Instance.translationInfo.loadStringFromXNBFile(dict, "NPC.cs.4519"));
            words.Add(HappyBirthday.Instance.translationInfo.loadStringFromXNBFile(dict, "NPC.cs.4522"));
            words.Add(HappyBirthday.Instance.translationInfo.loadStringFromXNBFile(dict, "NPC.cs.4523"));

            if (LocalizedContentManager.CurrentLanguageCode== LanguageCode.en)
            {
                words.Add("Pumpkin"); //Because this is cute.
            }

            string s = words[Game1.random.Next(0, words.Count - 1)];

            return s.ToLowerInvariant();
        }

        protected string getTimeOfDayString()
        {
            if (Game1.timeOfDay >= 600 && Game1.timeOfDay < 1200)
            {
                return "morning";
            }
            else if (Game1.timeOfDay >= 1200 && Game1.timeOfDay < 600)
            {
                return "afternoon";
            }
            else return "evening";
        }

        /// <summary>
        /// Gets the actual birthday message.
        /// </summary>
        /// <param name="SpeakerName"></param>
        /// <returns></returns>
        public string generateSpouseBirthdayDialogue(string SpeakerName)
        {
            return this.spouseEnglishGeneratedMessages[SpeakerName].Invoke(SpeakerName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SpeakerName"></param>
        /// <returns></returns>
        private string createSpouseBirthdayDialogue(string SpeakerName)
        {
            StringBuilder b = new StringBuilder();
            switch (SpeakerName)
            {
                case ("Alex"):
                    b.Append("Hey ");
                    b.Append(this.getAffectionateSpouseWord());
                    b.Append(".");
                    b.Append("I'm so glad that I married you. You make every day feel like winning a sports match. Happy birthday! $l");

                    break;
                case ("Elliott"):
                    b.Append("Good ");
                    b.Append(this.getTimeOfDayString());
                    b.Append(" ");
                    b.Append(this.getAffectionateSpouseWord());
                    b.Append(".");
                    b.Append("I was just thinking on how you have been a muse for my work. You inspire me every day I spend with you. Happy birthday! $l");

                    break;
                case ("Harvey"):
                    b.Append("Good ");
                    b.Append(this.getTimeOfDayString());
                    b.Append(" ");
                    b.Append(this.getAffectionateSpouseWord());
                    b.Append(".");
                    b.Append("I was just thinking on how invigorated I've felt since marrying you. When I look at you I feel as I'm positively glowing with joy. Happy birthday! $l");

                    break;
                case ("Sam"):
                    b.Append("Good ");
                    b.Append(this.getTimeOfDayString());
                    b.Append(" ");
                    b.Append(this.getAffectionateSpouseWord());
                    b.Append(".");
                    b.Append("You know I never saw myself settling down before I met you, but now that I have I feel like I never want to look back. Happy birthday! $l");

                    break;
                case ("Sebastian"):
                    b.Append("I was never a big celebrater of birthdays but with you, today is special. Happy birthday ");
                    b.Append(this.getAffectionateSpouseWord());
                    b.Append(".");
                    b.Append("$l");

                    break;
                case ("Shane"):
                    b.Append("I never though I'd enjoy annything but drinking on birthdays but you have shown me how to live a great life. Happy birthday ");
                    b.Append(this.getAffectionateSpouseWord());
                    b.Append(".");
                    b.Append("$l");

                    break;
                case ("Abigail"):
                    b.Append("Hey ");
                    b.Append(this.getAffectionateSpouseWord());
                    b.Append(".");
                    b.Append("You know every day feels like a great adventure with you! Happy birthday!");
                    b.Append("$l");
                    break;

                case ("Emily"):
                    b.Append("You know whenever I tried to read my fortune I never thought that I'd be as happy as I am with you! Happy birthday ");
                    b.Append(this.getAffectionateSpouseWord());
                    b.Append("!");
                    b.Append("$l");

                    break;
                case ("Haley"):
                    b.Append("You know ");
                    b.Append(this.getAffectionateSpouseWord());
                    b.Append(", ");
                    b.Append("we have come a long way since we first met but I'm glad we took this journey together. Every day feels picture perfect with you. Happy birthday!");
                    b.Append("$l");

                    break;
                case ("Leah"):
                    b.Append("Hey, ");
                    b.Append(this.getTimeOfDayString());
                    b.Append(" ");
                    b.Append(this.getAffectionateSpouseWord());
                    b.Append(".");
                    b.Append("If it wasn't for you who knows what I would be doing right now. I might sculpt wood, but being with you has helped scult my life.");
                    b.Append("Happy birthday!");
                    b.Append("$l");

                    break;
                case ("Maru"):
                    b.Append("Happy birthday");
                    b.Append(" ");
                    b.Append(this.getAffectionateSpouseWord());
                    b.Append(".");
                    b.Append("I always thought I had to look into space to find a shooting star but when I look at you I realize I already have one.");
                    b.Append("$l");
                    break;
                case ("Penny"):
                    b.Append("Happy birthday");
                    b.Append(" ");
                    b.Append(this.getAffectionateSpouseWord());
                    b.Append(". ");
                    b.Append("I always read about happy endings in books but being with you makes me realize that it continues well into the future. Here is to many more years to come.");
                    b.Append("$l");
                    break;
                default:
                    b.Append("Good ");
                    b.Append(this.getTimeOfDayString());
                    b.Append(" ");
                    b.Append(this.getAffectionateSpouseWord());
                    b.Append(".");
                    b.Append("I hope we get to spend many more years together. Happy birthday.");
                    b.Append("$h");

                    break;
            }
            return b.ToString();
        }


        public string getBirthdayMessage(string NPC)
        {
            if (Game1.player.friendshipData.ContainsKey(NPC))
            {
                if (Game1.player.getSpouse() != null) {
                    if (Game1.player.getSpouse().Name.Equals(NPC))
                    {
                        return this.getSpouseBirthdayWish(NPC);
                    }
                    else
                    {
                        return this.getBirthdayWish(NPC);
                    }
                }
                else
                {
                    return this.getBirthdayWish(NPC);
                }

            }
            else
            {
                //Potentially don't grab birthday wishes here if the npc is not know by the player?
                return this.getBirthdayWish(NPC);
            }
        }


        public virtual string getBirthdayWish(string Key)
        {
            return this.getBirthdayWish(Key, LocalizationUtilities.GetCurrentLanguageCodeString(), true);
        }

        public virtual string getBirthdayWish(string Key, string LanguageCode, bool DefaultToEnglish)
        {
            if (LanguageCode == LocalizationUtilities.GetEnglishLanguageCode() && DefaultToEnglish == true)
            {
                //Prevent infinite recursion.
                DefaultToEnglish = false;
            }
            List<HappyBirthdayContentPack> affectedContentPacks = HappyBirthday.Instance.happyBirthdayContentPackManager.getHappyBirthdayContentPacksForCurrentLanguageCode();
            List<string> potentialBirthdayWishes = new List<string>();
            foreach (HappyBirthdayContentPack contentPack in affectedContentPacks)
            {
                string birthdayWish = contentPack.getBirthdayWish(Key, false);
                if (string.IsNullOrEmpty(birthdayWish)) continue;
                potentialBirthdayWishes.Add(birthdayWish);
            }

            if (potentialBirthdayWishes.Count == 0)
            {
                if (DefaultToEnglish)
                {
                    return this.getBirthdayWish(Key, LocalizationUtilities.GetEnglishLanguageCode(), false);
                }
                return this.getDefaultBirthdayWish();
            }
            else
            {
                int randomWishIndex = Game1.random.Next(0, potentialBirthdayWishes.Count);
                return potentialBirthdayWishes[randomWishIndex];
            }
        }


        public virtual string getSpouseBirthdayWish(string Key)
        {
            return this.getSpouseBirthdayWish(Key, LocalizationUtilities.GetCurrentLanguageCodeString(), true);
        }

        public virtual string getSpouseBirthdayWish(string Key, string LanguageCode, bool DefaultToEnglish)
        {
            if (LanguageCode == LocalizationUtilities.GetEnglishLanguageCode() && DefaultToEnglish == true)
            {
                //Prevent infinite recursion.
                DefaultToEnglish = false;
            }
            List<HappyBirthdayContentPack> affectedContentPacks = HappyBirthday.Instance.happyBirthdayContentPackManager.getHappyBirthdayContentPacksForCurrentLanguageCode();
            List<string> potentialBirthdayWishes = new List<string>();
            foreach (HappyBirthdayContentPack contentPack in affectedContentPacks)
            {
                string birthdayWish = contentPack.getSpouseBirthdayWish(Key, false);
                if (string.IsNullOrEmpty(birthdayWish)) continue;
                potentialBirthdayWishes.Add(birthdayWish);
            }
            if (potentialBirthdayWishes.Count == 0)
            {
                if (DefaultToEnglish)
                {
                   return this.getSpouseBirthdayWish(Key, LocalizationUtilities.GetEnglishLanguageCode(), false);
                }
                return this.generateSpouseBirthdayDialogue(Key);
            }
            else
            {
                int randomWishIndex = Game1.random.Next(0, potentialBirthdayWishes.Count);
                return potentialBirthdayWishes[randomWishIndex];
            }
        }

        public virtual string getDefaultBirthdayWish()
        {
            return "Happy Birthday @!";

        }
    }
}


