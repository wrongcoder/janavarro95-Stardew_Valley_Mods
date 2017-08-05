using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Omegasis.HappyBirthday.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Monsters;
using SObject = StardewValley.Object;

namespace Omegasis.HappyBirthday
{
    /// <summary>The mod entry point.</summary>
    public class HappyBirthday : Mod
    {
        /*********
        ** Properties
        *********/
        /// <summary>The key which shows the menu.</summary>
        private string KeyBinding = "O";

        /// <summary>Whether the player loaded a save.</summary>
        private bool IsGameLoaded;

        /// <summary>Whether the player has chosen a birthday.</summary>
        private bool HasChosenBirthday;

        /// <summary>The queue of villagers who haven't given a gift yet.</summary>
        private List<string> VillagerQueue;

        /// <summary>The gifts that villagers can give.</summary>
        private List<Item> PossibleBirthdayGifts;

        /// <summary>The next birthday gift the player will receive.</summary>
        private Item BirthdayGiftToReceive;

        /// <summary>Whether we've already checked for and (if applicable) set up the player's birthday today.</summary>
        private bool CheckedForBirthday;
        //private Dictionary<string, Dialogue> Dialogue;
        //private bool SeenEvent;

        /// <summary>The name of the folder containing birthday data files.</summary>
        private readonly string FolderName = "Player_Birthdays";

        /// <summary>The full path to the folder containing birthday data files.</summary>
        private string BirthdayFolderPath;

        /// <summary>The player's current birthday day.</summary>
        public int BirthdayDay;

        /// <summary>The player's current birthday season.</summary>
        public string BirthdaySeason;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            TimeEvents.AfterDayStarted += this.TimeEvents_AfterDayStarted;
            GameEvents.UpdateTick += this.GameEvents_UpdateTick;
            SaveEvents.AfterLoad += this.SaveEvents_AfterLoad;
            ControlEvents.KeyPressed += this.ControlEvents_KeyPressed;

            this.VillagerQueue = new List<string>();
            this.PossibleBirthdayGifts = new List<Item>();
            this.BirthdayFolderPath = Path.Combine(Helper.DirectoryPath, this.FolderName);

            if (!Directory.Exists(this.BirthdayFolderPath))
                Directory.CreateDirectory(this.BirthdayFolderPath);
        }


        /*********
        ** Private methods
        *********/
        /// <summary>The method invoked after a new day starts.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void TimeEvents_AfterDayStarted(object sender, EventArgs e)
        {
            if (Game1.player == null)
                return;

            if (this.HasChosenBirthday)
                this.WriteBirthday();
            this.WriteConfig();
            this.CheckedForBirthday = false;
        }

        /// <summary>The method invoked when the presses a keyboard button.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void ControlEvents_KeyPressed(object sender, EventArgsKeyPressed e)
        {
            if (Game1.player == null || Game1.player.currentLocation == null || !this.IsGameLoaded || this.HasChosenBirthday || Game1.activeClickableMenu != null)
                return;

            // show birthday selection menu
            if (e.KeyPressed.ToString() == this.KeyBinding)
                Game1.activeClickableMenu = new BirthdayMenu(this.BirthdaySeason, this.BirthdayDay, this.SetBirthday);
        }

        /// <summary>The method invoked after the player loads a save.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void SaveEvents_AfterLoad(object sender, EventArgs e)
        {
            this.IsGameLoaded = true;
            this.LoadBirthday();
            this.LoadConfig();
            //this.SeenEvent = false;
            //this.Dialogue = new Dictionary<string, Dialogue>();
        }

        /// <summary>The method invoked when the game updates (roughly 60 times per second).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void GameEvents_UpdateTick(object sender, EventArgs e)
        {
            if (Game1.eventUp || Game1.isFestival() || Game1.player == null || !this.IsGameLoaded)
                return;

            if (!this.CheckedForBirthday)
            {
                this.CheckedForBirthday = true;

                // set up birthday
                if (this.IsBirthday())
                {
                    Messages.ShowStarMessage("It's your birthday today! Happy birthday!");
                    Game1.mailbox.Enqueue("birthdayMom");
                    Game1.mailbox.Enqueue("birthdayDad");

                    try
                    {
                        this.ResetVillagerQueue();
                    }
                    catch (Exception ex)
                    {
                        this.Monitor.Log(ex.ToString(), LogLevel.Error);
                    }
                    foreach (GameLocation location in Game1.locations)
                    {
                        foreach (NPC npc in location.characters)
                        {
                            if (npc is Child || npc is Horse || npc is Junimo || npc is Monster || npc is Pet)
                                continue;

                            try
                            {
                                Dialogue d = new Dialogue(Game1.content.Load<Dictionary<string, string>>("Data\\FarmerBirthdayDialogue")[npc.name], npc);
                                npc.CurrentDialogue.Push(d);
                                if (npc.CurrentDialogue.ElementAt(0) != d) npc.setNewDialogue(Game1.content.Load<Dictionary<string, string>>("Data\\FarmerBirthdayDialogue")[npc.name]);
                            }
                            catch
                            {
                                Dialogue d = new Dialogue("Happy Birthday @!", npc);
                                npc.CurrentDialogue.Push(d);
                                if (npc.CurrentDialogue.ElementAt(0) != d)
                                    npc.setNewDialogue("Happy Birthday @!");
                            }
                        }
                    }
                }

                // ask for birthday date
                if ((string.IsNullOrEmpty(this.BirthdaySeason) || this.BirthdayDay == 0) && Game1.activeClickableMenu == null)
                {
                    Game1.activeClickableMenu = new BirthdayMenu(this.BirthdaySeason, this.BirthdayDay, this.SetBirthday);
                    this.CheckedForBirthday = false;
                }
            }

            // unreachable since we exit early if Game1.eventUp
            //if (Game1.eventUp)
            //{
            //    foreach (string npcName in this.VillagerQueue)
            //    {
            //        NPC npc = Game1.getCharacterFromName(npcName);

            //        try
            //        {
            //            this.Dialogue.Add(npcName, npc.CurrentDialogue.Pop());
            //        }
            //        catch (Exception ex)
            //        {
            //            this.Monitor.Log(ex.ToString(), LogLevel.Error);
            //            this.Dialogue.Add(npcName, npc.CurrentDialogue.ElementAt(0));
            //            npc.loadSeasonalDialogue();
            //        }

            //        this.SeenEvent = true;
            //    }
            //}

            //if (!Game1.eventUp && this.SeenEvent)
            //{
            //    foreach (KeyValuePair<string, Dialogue> v in this.Dialogue)
            //    {
            //        NPC npc = Game1.getCharacterFromName(v.Key);
            //        npc.CurrentDialogue.Push(v.Value);
            //    }
            //    this.Dialogue.Clear();
            //    this.SeenEvent = false;
            //}

            // set birthday gift
            if (Game1.currentSpeaker != null)
            {
                string name = Game1.currentSpeaker.name;
                if (this.IsBirthday() && this.VillagerQueue.Contains(name))
                {
                    try
                    {
                        this.SetNextBirthdayGift(Game1.currentSpeaker.name);
                        this.VillagerQueue.Remove(Game1.currentSpeaker.name);
                    }
                    catch (Exception ex)
                    {
                        this.Monitor.Log(ex.ToString(), LogLevel.Error);
                    }
                }
            }
            if (this.BirthdayGiftToReceive != null && Game1.currentSpeaker != null)
            {
                while (this.BirthdayGiftToReceive.Name == "Error Item" || this.BirthdayGiftToReceive.Name == "Rock" || this.BirthdayGiftToReceive.Name == "???")
                    this.SetNextBirthdayGift(Game1.currentSpeaker.name);
                Game1.player.addItemByMenuIfNecessaryElseHoldUp(this.BirthdayGiftToReceive);
                this.BirthdayGiftToReceive = null;
            }

            // update settings
            if (!this.HasChosenBirthday && !string.IsNullOrEmpty(this.BirthdaySeason) && this.BirthdayDay != 0)
            {
                this.WriteConfig();
                this.WriteBirthday();
                this.HasChosenBirthday = true;
            }
        }

        /// <summary>Set the player's birtday/</summary>
        /// <param name="season">The birthday season.</param>
        /// <param name="day">The birthday day.</param>
        private void SetBirthday(string season, int day)
        {
            this.BirthdaySeason = season;
            this.BirthdayDay = day;
        }

        /// <summary>Reset the queue of villager names.</summary>
        private void ResetVillagerQueue()
        {
            this.VillagerQueue.Clear();

            foreach (GameLocation location in Game1.locations)
            {
                foreach (NPC npc in location.characters)
                {
                    if (npc is Child || npc is Horse || npc is Junimo || npc is Monster || npc is Pet)
                        continue;
                    if (this.VillagerQueue.Contains(npc.name))
                        continue;
                    this.VillagerQueue.Add(npc.name);
                }
            }
        }

        /// <summary>Set the next birthday gift the player will receive.</summary>
        /// <param name="name">The villager's name who's giving the gift.</param>
        /// <remarks>This returns gifts based on the speaker's heart level towards the player: neutral for 0-3, good for 4-6, and best for 7-10.</remarks>
        private void SetNextBirthdayGift(string name)
        {
            Item gift;
            if (this.PossibleBirthdayGifts.Count > 0)
            {
                Random random = new Random();
                int index = random.Next(this.PossibleBirthdayGifts.Count);
                gift = this.PossibleBirthdayGifts[index];
                if (Game1.player.isInventoryFull())
                    Game1.createItemDebris(gift, Game1.player.getStandingPosition(), Game1.player.getDirection());
                else
                    this.BirthdayGiftToReceive = gift;
                return;
            }

            this.PossibleBirthdayGifts.AddRange(this.GetDefaultBirthdayGifts(name));

            Random rnd2 = new Random();
            int r2 = rnd2.Next(this.PossibleBirthdayGifts.Count);
            gift = this.PossibleBirthdayGifts.ElementAt(r2);
            if (Game1.player.isInventoryFull())
                Game1.createItemDebris(gift, Game1.player.getStandingPosition(), Game1.player.getDirection());
            else
                this.BirthdayGiftToReceive = gift;

            this.PossibleBirthdayGifts.Clear();
        }

        /// <summary>Get the default gift items.</summary>
        /// <param name="name">The villager's name.</param>
        private IEnumerable<SObject> GetDefaultBirthdayGifts(string name)
        {
            List<SObject> gifts = new List<SObject>();
            try
            {
                // read from birthday gifts file
                IDictionary<string, string> data = Game1.content.Load<Dictionary<string, string>>("Data\\PossibleBirthdayGifts");
                string text;
                data.TryGetValue(name, out text);
                if (text != null)
                {
                    string[] fields = text.Split('/');

                    // love
                    if (Game1.player.getFriendshipHeartLevelForNPC(name) >= 7)
                    {
                        string[] loveFields = fields[1].Split(' ');
                        for (int i = 0; i < loveFields.Length; i += 2)
                        {
                            try
                            {
                                gifts.AddRange(this.GetItems(Convert.ToInt32(loveFields[i]), Convert.ToInt32(loveFields[i + 1])));
                            }
                            catch { }
                        }
                    }

                    // like
                    if (Game1.player.getFriendshipHeartLevelForNPC(name) >= 4 && Game1.player.getFriendshipHeartLevelForNPC(name) <= 6)
                    {
                        string[] likeFields = fields[3].Split(' ');
                        for (int i = 0; i < likeFields.Length; i += 2)
                        {
                            try
                            {
                                gifts.AddRange(this.GetItems(Convert.ToInt32(likeFields[i]), Convert.ToInt32(likeFields[i + 1])));
                            }
                            catch { }
                        }
                    }

                    // neutral
                    if (Game1.player.getFriendshipHeartLevelForNPC(name) >= 0 && Game1.player.getFriendshipHeartLevelForNPC(name) <= 3)
                    {
                        string[] neutralFields = fields[5].Split(' ');

                        for (int i = 0; i < neutralFields.Length; i += 2)
                        {
                            try
                            {
                                gifts.AddRange(this.GetItems(Convert.ToInt32(neutralFields[i]), Convert.ToInt32(neutralFields[i + 1])));
                            }
                            catch { }
                        }
                    }
                }

                // get NPC's preferred gifts
                if (Game1.player.getFriendshipHeartLevelForNPC(name) >= 7)
                    gifts.AddRange(this.GetUniversalItems("Love", true));
                if (Game1.player.getFriendshipHeartLevelForNPC(name) >= 4 && Game1.player.getFriendshipHeartLevelForNPC(name) <= 6)
                    this.PossibleBirthdayGifts.AddRange(this.GetUniversalItems("Like", true));
                if (Game1.player.getFriendshipHeartLevelForNPC(name) >= 0 && Game1.player.getFriendshipHeartLevelForNPC(name) <= 3)
                    this.PossibleBirthdayGifts.AddRange(this.GetUniversalItems("Neutral", true));
            }
            catch
            {
                // get NPC's preferred gifts
                if (Game1.player.getFriendshipHeartLevelForNPC(name) >= 7)
                {
                    this.PossibleBirthdayGifts.AddRange(this.GetUniversalItems("Love", false));
                    this.PossibleBirthdayGifts.AddRange(this.GetLovedItems(name));
                }
                if (Game1.player.getFriendshipHeartLevelForNPC(name) >= 4 && Game1.player.getFriendshipHeartLevelForNPC(name) <= 6)
                {
                    this.PossibleBirthdayGifts.AddRange(this.GetLikedItems(name));
                    this.PossibleBirthdayGifts.AddRange(this.GetUniversalItems("Like", false));
                }
                if (Game1.player.getFriendshipHeartLevelForNPC(name) >= 0 && Game1.player.getFriendshipHeartLevelForNPC(name) <= 3)
                    this.PossibleBirthdayGifts.AddRange(this.GetUniversalItems("Neutral", false));
            }
            //TODO: Make different tiers of gifts depending on the friendship, and if it is the spouse.
            /*
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(198, 1));
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(204, 1));
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(220, 1));
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(221, 1));
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(223, 1));
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(233, 1));
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(234, 1));
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(286, 5));
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(368, 5));
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(608, 1));
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(612, 1));
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(773, 1));
                */

            return gifts;
        }

        /// <summary>Get the items loved by all villagers.</summary>
        /// <param name="group">The group to get (one of <c>Like</c>, <c>Love</c>, <c>Neutral</c>).</param>
        /// <param name="isBirthdayGiftList">Whether to get data from <c>Data\PossibleBirthdayGifts.xnb</c> instead of the game data.</param>
        private IEnumerable<SObject> GetUniversalItems(string group, bool isBirthdayGiftList)
        {
            if (!isBirthdayGiftList)
            {
                // get raw data
                string text;
                Game1.NPCGiftTastes.TryGetValue($"Universal_{group}", out text);
                if (text == null)
                    yield break;

                // parse
                string[] neutralIDs = text.Split(' ');
                foreach (string neutralID in neutralIDs)
                {
                    foreach (SObject obj in this.GetItems(Convert.ToInt32(neutralID)))
                        yield return obj;
                }
            }
            else
            {
                // get raw data
                Dictionary<string, string> data = Game1.content.Load<Dictionary<string, string>>("Data\\PossibleBirthdayGifts");
                string text;
                data.TryGetValue($"Universal_{group}_Gift", out text);
                if (text == null)
                    yield break;

                // parse
                string[] array = text.Split(' ');
                for (int i = 0; i < array.Length; i += 2)
                {
                    foreach (SObject obj in this.GetItems(Convert.ToInt32(array[i]), Convert.ToInt32(array[i + 1])))
                        yield return obj;
                }
            }
        }

        /// <summary>Get a villager's loved items.</summary>
        /// <param name="name">The villager's name.</param>
        private IEnumerable<SObject> GetLikedItems(string name)
        {
            // get raw data
            string text;
            Game1.NPCGiftTastes.TryGetValue(name, out text);
            if (text == null)
                yield break;

            // parse
            string[] data = text.Split('/');
            string[] likedIDs = data[3].Split(' ');
            foreach (string likedID in likedIDs)
            {
                foreach (SObject obj in this.GetItems(Convert.ToInt32(likedID)))
                    yield return obj;
            }
        }

        /// <summary>Get a villager's loved items.</summary>
        /// <param name="name">The villager's name.</param>
        private IEnumerable<SObject> GetLovedItems(string name)
        {
            // get raw data
            string text;
            Game1.NPCGiftTastes.TryGetValue(name, out text);
            if (text == null)
                yield break;

            // parse
            string[] data = text.Split('/');
            string[] lovedIDs = data[1].Split(' ');
            foreach (string lovedID in lovedIDs)
            {
                foreach (SObject obj in this.GetItems(Convert.ToInt32(lovedID)))
                    yield return obj;
            }
        }

        /// <summary>Get the items matching the given ID.</summary>
        /// <param name="id">The category or item ID.</param>
        private IEnumerable<SObject> GetItems(int id)
        {
            return id < 0
                ? ObjectUtility.GetObjectsInCategory(id)
                : new[] { new SObject(id, 1) };
        }

        /// <summary>Get the items matching the given ID.</summary>
        /// <param name="id">The category or item ID.</param>
        /// <param name="stack">The stack size.</param>
        private IEnumerable<SObject> GetItems(int id, int stack)
        {
            foreach (SObject obj in this.GetItems(id))
                yield return new SObject(obj.parentSheetIndex, stack);
        }

        /// <summary>Get whether today is the player's birthday.</summary>
        private bool IsBirthday()
        {
            return
                this.BirthdayDay == Game1.dayOfMonth
                && this.BirthdaySeason == Game1.currentSeason;
        }

        /// <summary>Load the configuration settings.</summary>
        private void LoadConfig()
        {
            string path = Path.Combine(Helper.DirectoryPath, "HappyBirthday_Config.txt");
            if (!File.Exists(path))
                this.KeyBinding = "O";
            else
            {
                string[] text = File.ReadAllLines(path);
                this.KeyBinding = Convert.ToString(text[3]);
            }
        }

        /// <summary>Save the configuration settings.</summary>
        private void WriteConfig()
        {
            string path = Path.Combine(Helper.DirectoryPath, "HappyBirthday_Config.txt");
            string[] text = new string[20];
            text[0] = "Config: HappyBirthday Info. Feel free to mess with these settings.";
            text[1] = "====================================================================================";

            text[2] = "Key binding for opening the birthday menu. Press this key to do so.";
            text[3] = this.KeyBinding;

            File.WriteAllLines(path, text);
        }

        /// <summary>Load the player's birthday from the config file.</summary>
        private void LoadBirthday()
        {
            string path = Path.Combine(this.BirthdayFolderPath, $"HappyBirthday_{Game1.player.name}.txt");
            if (File.Exists(path))
            {
                string[] text = File.ReadAllLines(path);
                this.BirthdaySeason = Convert.ToString(text[3]);
                this.BirthdayDay = Convert.ToInt32(text[5]);
            }
        }

        /// <summary>Write the player's birthday to the config file.</summary>
        private void WriteBirthday()
        {
            string path = Path.Combine(this.BirthdayFolderPath, $"HappyBirthday_{Game1.player.name}.txt");
            string[] text = new string[20];
            text[0] = "Player Info: Modifying these values could be considered cheating or an exploit. Edit at your own risk.";
            text[1] = "====================================================================================";

            text[2] = "Player's Birthday Season";
            text[3] = this.BirthdaySeason;
            text[4] = "Player's Birthday Date";
            text[5] = this.BirthdayDay.ToString();

            File.WriteAllLines(path, text);
        }
    }
}
