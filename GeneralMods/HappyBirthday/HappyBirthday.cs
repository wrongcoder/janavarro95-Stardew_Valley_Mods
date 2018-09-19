using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Omegasis.HappyBirthday.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Menus;
using StardewValley.Monsters;
using SObject = StardewValley.Object;

namespace Omegasis.HappyBirthday
{
    /// <summary>The mod entry point.</summary>
    public class HappyBirthday : Mod, IAssetEditor
    {
        /*********
        ** Properties
        *********/
        /// <summary>The relative path for the current player's data file.</summary>
        private string DataFilePath;

        /// <summary>The absolute path for the current player's legacy data file.</summary>
        private string LegacyDataFilePath => Path.Combine(this.Helper.DirectoryPath, "Player_Birthdays", $"HappyBirthday_{Game1.player.Name}.txt");

        /// <summary>The mod configuration.</summary>
        public static ModConfig Config;

        /// <summary>The data for the current player.</summary>
        private PlayerData PlayerData;

        /// <summary>Whether the player has chosen a birthday.</summary>
        private bool HasChosenBirthday => !string.IsNullOrEmpty(this.PlayerData.BirthdaySeason) && this.PlayerData.BirthdayDay != 0;

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

        public bool CanEdit<T>(IAssetInfo asset)
        {
            return asset.AssetNameEquals(@"Data\mail");
        }

        public void Edit<T>(IAssetData asset)
        {
            asset
            .AsDictionary<string, string>()
            .Set("birthdayMom", "Dear @,^  Happy birthday sweetheart. It's been amazing watching you grow into the kind, hard working person that I've always dreamed that you would become. I hope you continue to make many more fond memories with the ones you love. ^  Love, Mom ^ P.S. Here's a little something that I made for you. %item object 221 1 %%");

            asset
            .AsDictionary<string, string>()
            .Set("birthdayDad", "Dear @,^  Happy birthday kiddo. It's been a little quiet around here on your birthday since you aren't around, but your mother and I know that you are making both your grandpa and us proud.  We both know that living on your own can be tough but we believe in you one hundred percent, just keep following your dreams.^  Love, Dad ^ P.S. Here's some spending money to help you out on the farm. Good luck! %item money 5000 5001 %%");
        }

        public static IModHelper ModHelper;



        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            helper.Content.AssetLoaders.Add(new PossibleGifts());
            this.Config = helper.ReadConfig<ModConfig>();

            TimeEvents.AfterDayStarted += this.TimeEvents_AfterDayStarted;
            GameEvents.UpdateTick += this.GameEvents_UpdateTick;
            SaveEvents.AfterLoad += this.SaveEvents_AfterLoad;
            SaveEvents.BeforeSave += this.SaveEvents_BeforeSave;
            ControlEvents.KeyPressed += this.ControlEvents_KeyPressed;
            MenuEvents.MenuChanged += MenuEvents_MenuChanged;

            GraphicsEvents.OnPostRenderGuiEvent += GraphicsEvents_OnPostRenderGuiEvent;
            StardewModdingAPI.Events.GraphicsEvents.OnPostRenderHudEvent += GraphicsEvents_OnPostRenderHudEvent; ;
            //MultiplayerSupport.initializeMultiplayerSupport();
            ModHelper = Helper;
        }


        /// <summary>
        /// Used to properly display hovertext for all events happening on a calendar day.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GraphicsEvents_OnPostRenderHudEvent(object sender, EventArgs e)
        {
            if (Game1.activeClickableMenu == null) return;
            if (PlayerData.BirthdaySeason.ToLower() != Game1.currentSeason.ToLower()) return;
            if (Game1.activeClickableMenu is Billboard)
            {
                int index = PlayerData.BirthdayDay;
                //Game1.player.FarmerRenderer.drawMiniPortrat(Game1.spriteBatch, new Vector2(Game1.activeClickableMenu.xPositionOnScreen + 152 + (index - 1) % 7 * 32 * 4, Game1.activeClickableMenu.yPositionOnScreen + 230 + (index - 1) / 7 * 32 * 4), 1f, 4f, 2, Game1.player);

                string hoverText = "";
                foreach (var clicky in (Game1.activeClickableMenu as Billboard).calendarDays)
                {
                    if (clicky.containsPoint(Game1.getMouseX(), Game1.getMouseY()))
                    {
                        hoverText += clicky.hoverText + Environment.NewLine;
                    }
                }
                
                if (!String.IsNullOrEmpty(hoverText))
                {
                    hoverText.Remove(hoverText.Length - 2, 1);
                    var oldText = Helper.Reflection.GetField<string>(Game1.activeClickableMenu, "hoverText", true);
                    oldText.SetValue(hoverText);
                }

            }
        }

        /// <summary>
        /// Used to show the farmer's portrait on the billboard menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GraphicsEvents_OnPostRenderGuiEvent(object sender, EventArgs e)
        {
            if (Game1.activeClickableMenu == null) return;
            if (PlayerData.BirthdaySeason.ToLower() != Game1.currentSeason.ToLower()) return;
            if (Game1.activeClickableMenu is Billboard)
            {
                int index = PlayerData.BirthdayDay;
                Game1.player.FarmerRenderer.drawMiniPortrat(Game1.spriteBatch, new Vector2(Game1.activeClickableMenu.xPositionOnScreen + 152 + (index - 1) % 7 * 32 * 4, Game1.activeClickableMenu.yPositionOnScreen + 230 + (index - 1) / 7 * 32 * 4), 0.5f, 4f, 2, Game1.player);

                string hoverText = "";
                foreach(var clicky in (Game1.activeClickableMenu as Billboard).calendarDays)
                {
                    if (clicky.containsPoint(Game1.getMouseX(), Game1.getMouseY()))
                    {
                        hoverText += clicky.hoverText+Environment.NewLine;
                    }
                }
                if (hoverText != "")
                {
                    var oldText=Helper.Reflection.GetField<string>(Game1.activeClickableMenu, "hoverText", true);
                    oldText.SetValue(hoverText);
                }

            }
        }

        /// <summary>
        /// Functionality to display the player's birthday on the billboard.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MenuEvents_MenuChanged(object sender, EventArgsClickableMenuChanged e)
        {
            if (Game1.activeClickableMenu == null) return;
            if(Game1.activeClickableMenu is Billboard)
            {
                Texture2D text = new Texture2D(Game1.graphics.GraphicsDevice,1,1);
                Color[] col = new Color[1];
                col[0] = new Color(0, 0, 0, 1);
                text.SetData<Color>(col);
                //players birthdy position rect=new ....
                int index = PlayerData.BirthdayDay;
                Rectangle birthdayRect = new Rectangle(Game1.activeClickableMenu.xPositionOnScreen + 152 + (index - 1) % 7 * 32 * 4, Game1.activeClickableMenu.yPositionOnScreen + 200 + (index - 1) / 7 * 32 * 4, 124, 124);
                (Game1.activeClickableMenu as Billboard).calendarDays.Add(new ClickableTextureComponent("", birthdayRect, "", Game1.player.name + "'s Birthday", text, new Rectangle(0, 0, 124, 124), 1f, false));
            }
        }


        /*********
        ** Private methods
        *********/
        /// <summary>The method invoked after a new day starts.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void TimeEvents_AfterDayStarted(object sender, EventArgs e)
        {
            this.CheckedForBirthday = false;
        }

        /// <summary>The method invoked when the presses a keyboard button.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void ControlEvents_KeyPressed(object sender, EventArgsKeyPressed e)
        {
            // show birthday selection menu
            if (Game1.activeClickableMenu != null) return;
            if (Context.IsPlayerFree && !this.HasChosenBirthday && e.KeyPressed.ToString() == this.Config.KeyBinding)
                Game1.activeClickableMenu = new BirthdayMenu(this.PlayerData.BirthdaySeason, this.PlayerData.BirthdayDay, this.SetBirthday);
        }

        /// <summary>The method invoked after the player loads a save.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void SaveEvents_AfterLoad(object sender, EventArgs e)
        {
            this.DataFilePath = Path.Combine("data", Game1.player.Name + "_" + Game1.player.UniqueMultiplayerID+".json");

            // reset state
            this.VillagerQueue = new List<string>();
            this.PossibleBirthdayGifts = new List<Item>();
            this.BirthdayGiftToReceive = null;
            this.CheckedForBirthday = false;

            // load settings
            this.MigrateLegacyData();
            this.PlayerData = this.Helper.ReadJsonFile<PlayerData>(this.DataFilePath) ?? new PlayerData();

            createBirthdayGreetings();
            //this.SeenEvent = false;
            //this.Dialogue = new Dictionary<string, Dialogue>();
        }

        /// <summary>The method invoked just before the game updates the saves.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void SaveEvents_BeforeSave(object sender, EventArgs e)
        {
            if (this.HasChosenBirthday)
                this.Helper.WriteJsonFile(this.DataFilePath, this.PlayerData);
        }

        /// <summary>The method invoked when the game updates (roughly 60 times per second).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void GameEvents_UpdateTick(object sender, EventArgs e)
        {
            if (!Context.IsWorldReady || Game1.eventUp || Game1.isFestival())
                return;
            if (!this.HasChosenBirthday && Game1.activeClickableMenu == null && Game1.player.Name.ToLower()!="unnamed farmhand")
            {
                Game1.activeClickableMenu = new BirthdayMenu(this.PlayerData.BirthdaySeason, this.PlayerData.BirthdayDay, this.SetBirthday);
                this.CheckedForBirthday = false;
            }

            if (!this.CheckedForBirthday && Game1.activeClickableMenu==null)
            {
                this.CheckedForBirthday = true;

                // set up birthday
                if (this.IsBirthday())
                {
                    Messages.ShowStarMessage("It's your birthday today! Happy birthday!");
                    //MultiplayerSupport.SendBirthdayMessageToOtherPlayers();


                    Game1.player.mailbox.Add("birthdayMom");
                    Game1.player.mailbox.Add("birthdayDad");

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
                                if (Game1.player.getFriendshipHeartLevelForNPC(npc.Name) >= Config.minimumFriendshipLevelForBirthdayWish)
                                {
                                    Dialogue d = new Dialogue(Game1.content.Load<Dictionary<string, string>>("Data\\FarmerBirthdayDialogue")[npc.Name], npc);
                                    npc.CurrentDialogue.Push(d);
                                    if (npc.CurrentDialogue.ElementAt(0) != d) npc.setNewDialogue(Game1.content.Load<Dictionary<string, string>>("Data\\FarmerBirthdayDialogue")[npc.Name]);
                                }
                            }
                            catch
                            {
                                if (Game1.player.getFriendshipHeartLevelForNPC(npc.Name) >= Config.minimumFriendshipLevelForBirthdayWish)
                                {
                                    Dialogue d = new Dialogue("Happy Birthday @!", npc);
                                    npc.CurrentDialogue.Push(d);
                                    if (npc.CurrentDialogue.ElementAt(0) != d)
                                        npc.setNewDialogue("Happy Birthday @!");
                                }
                            }
                        }
                    }
                }

                if (Game1.activeClickableMenu != null)
                {
                    if (Game1.activeClickableMenu.GetType() == typeof(BirthdayMenu)) return;
                }
                // ask for birthday date
                if (!this.HasChosenBirthday && Game1.activeClickableMenu==null)
                {
                    Game1.activeClickableMenu = new BirthdayMenu(this.PlayerData.BirthdaySeason, this.PlayerData.BirthdayDay, this.SetBirthday);
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
                string name = Game1.currentSpeaker.Name;
                if (this.IsBirthday() && this.VillagerQueue.Contains(name))
                {
                    try
                    {
                        this.SetNextBirthdayGift(Game1.currentSpeaker.Name);
                        this.VillagerQueue.Remove(Game1.currentSpeaker.Name);
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
                    this.SetNextBirthdayGift(Game1.currentSpeaker.Name);
                Game1.player.addItemByMenuIfNecessaryElseHoldUp(this.BirthdayGiftToReceive);
                this.BirthdayGiftToReceive = null;
            }
        }

        /// <summary>Set the player's birtday/</summary>
        /// <param name="season">The birthday season.</param>
        /// <param name="day">The birthday day.</param>
        private void SetBirthday(string season, int day)
        {
            this.PlayerData.BirthdaySeason = season;
            this.PlayerData.BirthdayDay = day;
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
                    if (this.VillagerQueue.Contains(npc.Name))
                        continue;
                    this.VillagerQueue.Add(npc.Name);
                }
            }
        }

        /// <summary>Set the next birthday gift the player will receive.</summary>
        /// <param name="name">The villager's name who's giving the gift.</param>
        /// <remarks>This returns gifts based on the speaker's heart level towards the player: neutral for 3-4, good for 5-6, and best for 7-10.</remarks>
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
            //Attempt to balance sapplings from being too OP as a birthday gift.
            if (gift.Name.Contains("Sapling"))
            {
                gift.Stack = 1; //A good investment?
            }
            if(gift.Name.Contains("Rare Seed"))
            {
                gift.Stack = 2; //Still a little op but less so than 5.
            }

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
                data.TryGetValue(name, out string text);
                if (text != null)
                {
                    string[] fields = text.Split('/');

                    // love
                    if (Game1.player.getFriendshipHeartLevelForNPC(name) >= Config.minLoveFriendshipLevel)
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
                    if (Game1.player.getFriendshipHeartLevelForNPC(name) >= Config.minLikeFriendshipLevel && Game1.player.getFriendshipHeartLevelForNPC(name) <= Config.maxLikeFriendshipLevel)
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
                    if (Game1.player.getFriendshipHeartLevelForNPC(name) >= Config.minNeutralFriendshipGiftLevel && Game1.player.getFriendshipHeartLevelForNPC(name) <= Config.maxNeutralFriendshipGiftLevel)
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
                if (Game1.player.getFriendshipHeartLevelForNPC(name) >= Config.minLoveFriendshipLevel)
                    gifts.AddRange(this.GetUniversalItems("Love", true));
                if (Game1.player.getFriendshipHeartLevelForNPC(name) >= Config.minLikeFriendshipLevel && Game1.player.getFriendshipHeartLevelForNPC(name) <= Config.maxLikeFriendshipLevel)
                    this.PossibleBirthdayGifts.AddRange(this.GetUniversalItems("Like", true));
                if (Game1.player.getFriendshipHeartLevelForNPC(name) >= Config.minNeutralFriendshipGiftLevel && Game1.player.getFriendshipHeartLevelForNPC(name) <= Config.maxNeutralFriendshipGiftLevel)
                    this.PossibleBirthdayGifts.AddRange(this.GetUniversalItems("Neutral", true));
            }
            catch
            {
                // get NPC's preferred gifts
                if (Game1.player.getFriendshipHeartLevelForNPC(name) >= Config.minLoveFriendshipLevel)
                {
                    this.PossibleBirthdayGifts.AddRange(this.GetUniversalItems("Love", false));
                    this.PossibleBirthdayGifts.AddRange(this.GetLovedItems(name));
                }
                if (Game1.player.getFriendshipHeartLevelForNPC(name) >= Config.minLikeFriendshipLevel && Game1.player.getFriendshipHeartLevelForNPC(name) <= Config.maxLikeFriendshipLevel)
                {
                    this.PossibleBirthdayGifts.AddRange(this.GetLikedItems(name));
                    this.PossibleBirthdayGifts.AddRange(this.GetUniversalItems("Like", false));
                }
                if (Game1.player.getFriendshipHeartLevelForNPC(name) >= Config.minNeutralFriendshipGiftLevel && Game1.player.getFriendshipHeartLevelForNPC(name) <= Config.maxNeutralFriendshipGiftLevel)
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
                Game1.NPCGiftTastes.TryGetValue($"Universal_{group}", out string text);
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
                data.TryGetValue($"Universal_{group}_Gift", out string text);
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
            Game1.NPCGiftTastes.TryGetValue(name, out string text);
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
            Game1.NPCGiftTastes.TryGetValue(name, out string text);
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
                yield return new SObject(obj.ParentSheetIndex, stack);
        }

        /// <summary>Get whether today is the player's birthday.</summary>
        private bool IsBirthday()
        {
            return
                this.PlayerData.BirthdayDay == Game1.dayOfMonth
                && this.PlayerData.BirthdaySeason == Game1.currentSeason;
        }

        /// <summary>Migrate the legacy settings for the current player.</summary>
        private void MigrateLegacyData()
        {
            // skip if no legacy data or new data already exists
            try
            {
                if (!File.Exists(this.LegacyDataFilePath) || File.Exists(this.DataFilePath))
                    if (this.PlayerData == null) this.PlayerData = new PlayerData();
                         return;
            }
            catch(Exception err)
            {
                err.ToString();
                // migrate to new file
                try
                {
                    string[] text = File.ReadAllLines(this.LegacyDataFilePath);
                    this.Helper.WriteJsonFile(this.DataFilePath, new PlayerData
                    {
                        BirthdaySeason = text[3],
                        BirthdayDay = Convert.ToInt32(text[5])
                    });

                    FileInfo file = new FileInfo(this.LegacyDataFilePath);
                    file.Delete();
                    if (!file.Directory.EnumerateFiles().Any())
                        file.Directory.Delete();
                }
                catch (Exception ex)
                {
                    this.Monitor.Log($"Error migrating data from the legacy 'Player_Birthdays' folder for the current player. Technical details:\n {ex}", LogLevel.Error);
                }
            }
            
        }

    }
}
