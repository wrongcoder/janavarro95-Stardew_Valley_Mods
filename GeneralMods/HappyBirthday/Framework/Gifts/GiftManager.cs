using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Omegasis.HappyBirthday.Framework;
using Omegasis.HappyBirthday.Framework.Configs;
using Omegasis.HappyBirthday.Framework.ContentPack;
using Omegasis.HappyBirthday.Framework.Gifts;
using StardewValley;
using static System.String;
using SObject = StardewValley.Object;

namespace Omegasis.HappyBirthday
{
    public class GiftManager
    {
        public ModConfig Config => HappyBirthdayModCore.Configs.modConfig;


        /// <summary>The next birthday gift the player will receive.</summary>
        public Item BirthdayGiftToReceive;


        public static Dictionary<string, List<GiftInformation>> NPCBirthdayGifts;
        public static Dictionary<string, List<GiftInformation>> SpouseBirthdayGifts;
        public static List<GiftInformation> DefaultBirthdayGifts;

        /// <summary>Construct an instance.</summary>
        public GiftManager()
        {
            //this.BirthdayGifts = new List<Item>();


            NPCBirthdayGifts = new Dictionary<string, List<GiftInformation>>();
            SpouseBirthdayGifts = new Dictionary<string, List<GiftInformation>>();
            DefaultBirthdayGifts = new List<GiftInformation>();


            this.registerGiftIDS();

            /*
            this.loadDefaultBirthdayGifts();
            this.loadVillagerBirthdayGifts();
            this.loadSpouseBirthdayGifts();
            */

            //this.createNPCBirthdayGifts();
            //this.createSpouseBirthdayGifts();


        }

        /// <summary>
        /// Reloads all of the birthday gifts from content packs.
        /// </summary>
        public virtual void reloadBirthdayGifts()
        {

            NPCBirthdayGifts.Clear();
            SpouseBirthdayGifts.Clear();
            DefaultBirthdayGifts.Clear();

            /*
            this.loadDefaultBirthdayGifts();
            this.loadVillagerBirthdayGifts();
            this.loadSpouseBirthdayGifts();
            */

            this.loadInGiftsFromContentPacks();
        }


        protected virtual void registerGiftIDS()
        {
            foreach (var v in GiftIDS.GetSDVObjects())
            {
                Item i = new StardewValley.Object((int)v, 1);
                string uniqueID = "StardewValley.Object." + Enum.GetName(typeof(GiftIDS.SDVObject), (int)v);
                HappyBirthdayModCore.Instance.Monitor.Log("Added gift with id: " + uniqueID);
                GiftIDS.RegisteredGifts.Add(uniqueID, i);
            }
            List<string> registeredGiftKeys = GiftIDS.RegisteredGifts.Keys.ToList();
            registeredGiftKeys.Sort();
            HappyBirthdayModCore.Instance.Helper.Data.WriteJsonFile<List<string>>(Path.Combine("ModAssets", "Gifts", "RegisteredGifts" + ".json"), GiftIDS.RegisteredGifts.Keys.ToList());
        }


        public virtual void loadInGiftsFromContentPacks()
        {

            //Loads in all gifts across all content packs across all translations.
            foreach (HappyBirthdayContentPack contentPack in HappyBirthdayModCore.Instance.happyBirthdayContentPackManager.contentPacks.Values.SelectMany(contentPackList=>contentPackList))
            {
                HappyBirthdayModCore.Instance.Monitor.Log("Adding default gifts for content pack: " + contentPack.baseContentPack.Manifest.UniqueID);
                foreach (GiftInformation giftInfo in contentPack.getDefaultBirthdayGifts())
                {
                    DefaultBirthdayGifts.Add(giftInfo);
                }
                foreach (KeyValuePair<string,List<GiftInformation>> giftInfo in contentPack.npcBirthdayGifts)
                {
                    if (NPCBirthdayGifts.ContainsKey(giftInfo.Key))
                    {
                        HappyBirthdayModCore.Instance.Monitor.Log(string.Format("Adding npc {0} gifts for content pack: {1}", contentPack.baseContentPack.Manifest.UniqueID, giftInfo.Key));
                        NPCBirthdayGifts[giftInfo.Key].AddRange(giftInfo.Value);
                    }
                    else
                    {
                        HappyBirthdayModCore.Instance.Monitor.Log(string.Format("Adding npc {0} gifts for content pack: {1}", contentPack.baseContentPack.Manifest.UniqueID, giftInfo.Key));
                        NPCBirthdayGifts.Add(giftInfo.Key, giftInfo.Value);
                    }
                }
                foreach (KeyValuePair<string, List<GiftInformation>> giftInfo in contentPack.spouseBirthdayGifts)
                {
                    if (SpouseBirthdayGifts.ContainsKey(giftInfo.Key))
                    {
                        HappyBirthdayModCore.Instance.Monitor.Log(string.Format("Adding spouse {0} gifts for content pack: {1}", contentPack.baseContentPack.Manifest.UniqueID, giftInfo.Key));
                        SpouseBirthdayGifts[giftInfo.Key].AddRange(giftInfo.Value);
                    }
                    else
                    {
                        HappyBirthdayModCore.Instance.Monitor.Log(string.Format("Adding spouse {0} gifts for content pack: {1}", contentPack.baseContentPack.Manifest.UniqueID, giftInfo.Key));
                        SpouseBirthdayGifts.Add(giftInfo.Key, giftInfo.Value);
                    }
                }
            }

            List<string> registeredGiftKeys = GiftIDS.RegisteredGifts.Keys.ToList();
            registeredGiftKeys.Sort();
            HappyBirthdayModCore.Instance.Helper.Data.WriteJsonFile<List<string>>(Path.Combine("ModAssets", "Gifts", "RegisteredGifts" + ".json"),registeredGiftKeys );
        }

        public virtual bool registerDefaultBirthdayGift(string UniqueGiftId, int MinHeartsRequiredForGift, int MaxHeartsRequiredForGift, int MinStackAmount, int MaxStackAmount)
        {

            return this.registerDefaultBirthdayGift(new GiftInformation(UniqueGiftId, MinHeartsRequiredForGift, MaxHeartsRequiredForGift, MinStackAmount, MaxStackAmount));

        }

        public virtual bool registerDefaultBirthdayGift(GiftInformation giftInformation)
        {
            DefaultBirthdayGifts.Add(giftInformation);
            return true;
        }

        public virtual bool unregisterDefaultBirthdayGift(string UniqueGiftId, int MinHeartsRequiredForGift, int MaxHeartsRequiredForGift, int MinStackAmount, int MaxStackAmount)
        {
            return this.unregisterDefaultBirthdayGift(new GiftInformation(UniqueGiftId, MinHeartsRequiredForGift, MaxHeartsRequiredForGift, MinStackAmount, MaxStackAmount));
        }

        public virtual bool unregisterDefaultBirthdayGift(GiftInformation giftInformation)
        {
            List<GiftInformation> removalList = new();
            for(int i = 0; i < DefaultBirthdayGifts.Count; i++)
            {
                if (giftInformation.Equals(DefaultBirthdayGifts[i]))
                {
                    removalList.Add(DefaultBirthdayGifts[i]);
                }
            }
            foreach(GiftInformation info in removalList)
            {
                DefaultBirthdayGifts.Remove(info);
            }
            return removalList.Count > 0;
        }

        public virtual bool registerNpcBirthdayGift(string NPC,string UniqueGiftId, int MinHeartsRequiredForGift, int MaxHeartsRequiredForGift, int MinStackAmount, int MaxStackAmount)
        {
            return this.registerNpcBirthdayGift(NPC,new GiftInformation(UniqueGiftId, MinHeartsRequiredForGift, MaxHeartsRequiredForGift, MinStackAmount, MaxStackAmount));
        }

        public virtual bool registerNpcBirthdayGift(string NPC,GiftInformation giftInformation)
        {
            if (NPCBirthdayGifts.ContainsKey(giftInformation.objectID))
            {
                NPCBirthdayGifts[NPC].Add(giftInformation);
            }
            else
            {
                NPCBirthdayGifts.Add(NPC, new List<GiftInformation>() { giftInformation });
            }
            return true;
        }

        public virtual bool unregisterNPCBirthdayGift(string NPC,string UniqueGiftId, int MinHeartsRequiredForGift, int MaxHeartsRequiredForGift, int MinStackAmount, int MaxStackAmount)
        {
            return this.unregisterNPCBirthdayGift(NPC,new GiftInformation(UniqueGiftId, MinHeartsRequiredForGift, MaxHeartsRequiredForGift, MinStackAmount, MaxStackAmount));
        }

        public virtual bool unregisterNPCBirthdayGift(string NPC, GiftInformation giftInformation)
        {
            if (!NPCBirthdayGifts.ContainsKey(NPC)) return false;
            List<GiftInformation> removalList = new();
            for (int i = 0; i < NPCBirthdayGifts[NPC].Count; i++)
            {
                if (giftInformation.Equals(NPCBirthdayGifts[NPC][i]))
                {
                    removalList.Add(NPCBirthdayGifts[NPC][i]);
                }
            }
            foreach (GiftInformation info in removalList)
            {
                NPCBirthdayGifts[NPC].Remove(info);
            }
            return removalList.Count > 0;
        }

        public virtual bool registerSpouseBirthdayGift(string SpouseName,string UniqueGiftId, int MinHeartsRequiredForGift, int MaxHeartsRequiredForGift, int MinStackAmount, int MaxStackAmount)
        {
            return this.registerSpouseBirthdayGift(SpouseName, new GiftInformation(UniqueGiftId, MinHeartsRequiredForGift, MaxHeartsRequiredForGift, MinStackAmount, MaxStackAmount));
        }

        public virtual bool registerSpouseBirthdayGift(string SpouseName, GiftInformation giftInformation)
        {
            if (SpouseBirthdayGifts.ContainsKey(giftInformation.objectID))
            {
                SpouseBirthdayGifts[SpouseName].Add(giftInformation);
            }
            else
            {
                SpouseBirthdayGifts.Add(SpouseName, new List<GiftInformation>() { giftInformation });
            }
            return true;
        }


        public virtual bool unregisterSpouseBirthdayGift(string Spouse, string UniqueGiftId, int MinHeartsRequiredForGift, int MaxHeartsRequiredForGift, int MinStackAmount, int MaxStackAmount)
        {
            return this.unregisterSpouseBirthdayGift(Spouse, new GiftInformation(UniqueGiftId, MinHeartsRequiredForGift, MaxHeartsRequiredForGift, MinStackAmount, MaxStackAmount));
        }

        public virtual bool unregisterSpouseBirthdayGift(string Spouse, GiftInformation giftInformation)
        {
            if (!SpouseBirthdayGifts.ContainsKey(Spouse)) return false;
            List<GiftInformation> removalList = new();
            for (int i = 0; i < SpouseBirthdayGifts[Spouse].Count; i++)
            {
                if (giftInformation.Equals(SpouseBirthdayGifts[Spouse][i]))
                {
                    removalList.Add(SpouseBirthdayGifts[Spouse][i]);
                }
            }
            foreach (GiftInformation info in removalList)
            {
                SpouseBirthdayGifts[Spouse].Remove(info);
            }
            return removalList.Count > 0;
        }



        /// <summary>
        /// Gets the next birthday gift that would be received by the given npc.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Item getNextBirthdayGift(string name)
        {
            if (Game1.player.friendshipData.ContainsKey(name))
            {
                if (Game1.player.getSpouse() != null)
                {
                    if (Game1.player.getSpouse().Name.Equals(name))
                    {
                        //Get spouse gift here
                        Item gift = this.getSpouseBirthdayGift(name);
                        return gift;
                    }
                    else
                    {
                        Item gift = this.getNonSpouseBirthdayGift(name);
                        return gift;
                    }
                }
                else
                {
                    if (NPCBirthdayGifts.ContainsKey(name))
                    {
                        Item gift = this.getNonSpouseBirthdayGift(name);
                        return gift;
                    }
                    else
                    {
                        Item gift = this.getDefaultBirthdayGift(name);
                        return gift;

                    }
                }
            }
            else
            {
                if (NPCBirthdayGifts.ContainsKey(name))
                {

                    Item gift = this.getNonSpouseBirthdayGift(name);
                    return gift;
                }
                else
                {
                    Item gift = this.getDefaultBirthdayGift(name);
                    return gift;
                }
            }
        }

        /// <summary>
        /// Tries to get a default spouse birthday gift.
        /// </summary>
        /// <param name="name"></param>
        public Item getNonSpouseBirthdayGift(string name)
        {
            int heartLevel = Game1.player.getFriendshipHeartLevelForNPC(name);

            List<Item> possibleItems = new List<Item>();
            if (NPCBirthdayGifts.ContainsKey(name))
            {
                List<GiftInformation> npcPossibleGifts = NPCBirthdayGifts[name];

                foreach (GiftInformation info in npcPossibleGifts)
                {

                    if (info == null)
                    {
                        continue;
                    }

                    if (info.minRequiredHearts <= heartLevel && heartLevel <= info.maxRequiredHearts)
                    {
                        possibleItems.Add(info.getOne());
                    }
                }

                Item gift;
                int index = StardewValley.Game1.random.Next(possibleItems.Count);
                gift = possibleItems[index].getOne();
                return gift;

            }
            else
            {
                Item gift = this.getDefaultBirthdayGift(name);
                return gift;
            }

        }


        /// <summary>
        /// Tries to get a spouse birthday gift.
        /// </summary>
        /// <param name="name"></param>
        public Item getSpouseBirthdayGift(string name)
        {
            if (string.IsNullOrEmpty(HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData.favoriteBirthdayGift) == false)
            {
                if (GiftIDS.RegisteredGifts.ContainsKey(HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData.favoriteBirthdayGift))
                {
                    GiftInformation info = new GiftInformation(HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData.favoriteBirthdayGift, 0, 1, 1);
                    return info.getOne();
                }
            }

            int heartLevel = Game1.player.getFriendshipHeartLevelForNPC(name);


            List<Item> possibleItems = new List<Item>();
            if (SpouseBirthdayGifts.ContainsKey(name))
            {
                List<GiftInformation> npcPossibleGifts = SpouseBirthdayGifts[name];
                foreach (GiftInformation info in npcPossibleGifts)
                {
                    if (info.minRequiredHearts <= heartLevel && heartLevel <= info.maxRequiredHearts)
                    {
                        possibleItems.Add(info.getOne());
                    }
                }

                Item gift;
                int index = StardewValley.Game1.random.Next(possibleItems.Count);
                gift = possibleItems[index].getOne();
                return gift;
            }
            else
            {
                return this.getNonSpouseBirthdayGift(name);
            }

        }

        /// <summary>
        /// Tries to get a default birthday gift.
        /// </summary>
        /// <param name="name"></param>
        public Item getDefaultBirthdayGift(string name)
        {
            int heartLevel = Game1.player.getFriendshipHeartLevelForNPC(name);

            List<Item> possibleItems = new List<Item>();

            List<GiftInformation> npcPossibleGifts = DefaultBirthdayGifts;
            foreach (GiftInformation info in npcPossibleGifts)
            {
                if (info.minRequiredHearts <= heartLevel && heartLevel <= info.maxRequiredHearts)
                {
                    possibleItems.Add(info.getOne());
                }
            }

            Item gift;
            int index = StardewValley.Game1.random.Next(possibleItems.Count);
            gift = possibleItems[index].getOne();
            return gift;

        }

        /// <summary>
        /// Actually sets the next birthday gift to receieve or drops it on the ground for the player to pick up afterwards.
        /// </summary>
        /// <param name="gift"></param>
        public virtual void setNextBirthdayGift(Item gift)
        {
            if (Game1.player.isInventoryFull())
                Game1.createItemDebris(gift, Game1.player.getStandingPosition(), Game1.player.getDirection());
            else
                this.BirthdayGiftToReceive = gift;
        }

        /// <summary>Set the next birthday gift the player will receive.</summary>
        /// <param name="name">The villager's name who's giving the gift.</param>
        /// <remarks>This returns gifts based on the speaker's heart level towards the player: neutral for 3-4, good for 5-6, and best for 7-10.</remarks>
        public void setNextBirthdayGift(string name)
        {
            Item gift = this.getNextBirthdayGift(name);
            this.setNextBirthdayGift(gift);
        }
    }

}
