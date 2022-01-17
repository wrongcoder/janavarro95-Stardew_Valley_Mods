using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Omegasis.HappyBirthday.Framework;
using StardustCore.Events;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Menus;
using StardewValley.Monsters;
using StardustCore.Utilities;
using Omegasis.HappyBirthday.Framework.ContentPack;
using Omegasis.HappyBirthday.Framework.Utilities;
using Omegasis.HappyBirthday.Framework.Configs;
using Omegasis.HappyBirthday.Framework.Menus;

namespace Omegasis.HappyBirthday
{
    /// <summary>The mod entry point.</summary>
    public class HappyBirthday : Mod, IAssetEditor
    {
        /*********
        ** Fields
        *********/
        /// <summary>The relative path for the current player's data file.</summary>
        private string DataFilePath;

        /// <summary>The absolute path for the current player's legacy data file.</summary>
        private string LegacyDataFilePath => Path.Combine(this.Helper.DirectoryPath, "Player_Birthdays", $"HappyBirthday_{Game1.player.Name}.txt");

        /// <summary>
        /// Manages all of the configs for Happy Birthday.
        /// </summary>
        public static ConfigManager Configs;

        public static IModHelper ModHelper;

        public static IMonitor ModMonitor;

        /// <summary>Class to handle all birthday messages for this mod.</summary>
        public BirthdayMessages birthdayMessages;

        /// <summary>Class to handle all birthday gifts for this mod.</summary>
        public GiftManager giftManager;

        public static HappyBirthday Instance;

        private EventManager eventManager;

        public HappyBirthdayContentPackManager happyBirthdayContentPackManager;

        /// <summary>Handles different translations of files.</summary>
        public TranslationInfo translationInfo;

        /// <summary>
        /// Utilities for checking if it's a player's birthday, seeing if npcs have given birthday wishes already, etc.
        /// </summary>
        public BirthdayManager birthdayManager;

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            ModHelper = this.Helper;
            ModMonitor = this.Monitor;

            Instance = this;
            Configs = new ConfigManager();
            Configs.initializeConfigs();

            ModHelper.Events.GameLoop.DayStarted += this.OnDayStarted;
            ModHelper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
            ModHelper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
            ModHelper.Events.GameLoop.Saving += this.OnSaving;
            ModHelper.Events.Input.ButtonPressed += this.OnButtonPressed;
            ModHelper.Events.Display.MenuChanged += MenuUtilities.OnMenuChanged;
            ModHelper.Events.Display.RenderedActiveMenu += RenderUtilities.OnRenderedActiveMenu;
            ModHelper.Events.Display.RenderedHud += RenderUtilities.OnRenderedHud;
            ModHelper.Events.Multiplayer.ModMessageReceived += this.Multiplayer_ModMessageReceived;
            ModHelper.Events.Multiplayer.PeerDisconnected += this.Multiplayer_PeerDisconnected;
            ModHelper.Events.GameLoop.GameLaunched += this.GameLoop_GameLaunched;
            ModHelper.Events.Player.Warped += this.Player_Warped;
            ModHelper.Events.GameLoop.ReturnedToTitle += this.GameLoop_ReturnedToTitle;

            this.birthdayManager = new BirthdayManager();

            this.happyBirthdayContentPackManager = new HappyBirthdayContentPackManager();
            this.eventManager = new EventManager();
            this.translationInfo = new TranslationInfo();

        }

        private void GameLoop_ReturnedToTitle(object sender, ReturnedToTitleEventArgs e)
        {
            this.eventManager = new EventManager();
        }

        private void Player_Warped(object sender, WarpedEventArgs e)
        {
            if (e.NewLocation == Game1.getLocationFromName("CommunityCenter"))
            {
                this.eventManager.startEventAtLocationIfPossible("CommunityCenterBirthday");
            }
            if (e.NewLocation == Game1.getLocationFromName("Trailer"))
            {
                this.eventManager.startEventAtLocationIfPossible("BirthdayDating:Penny");
            }
            if (e.NewLocation == Game1.getLocationFromName("Trailer_Big"))
            {
                this.eventManager.startEventAtLocationIfPossible("BirthdayDating:Penny_BigHome");
            }

            if (e.NewLocation == Game1.getLocationFromName("ScienceHouse"))
            {
                this.eventManager.startEventAtLocationIfPossible("BirthdayDating:Maru");
                this.eventManager.startEventAtLocationIfPossible("BirthdayDating:Sebastian");
            }
            if (e.NewLocation == Game1.getLocationFromName("LeahHouse"))
            {
                this.eventManager.startEventAtLocationIfPossible("BirthdayDating:Leah");
            }
            if (e.NewLocation == Game1.getLocationFromName("SeedShop"))
            {
                this.eventManager.startEventAtLocationIfPossible("BirthdayDating:Abigail");
            }
            if (e.NewLocation == Game1.getLocationFromName("Mine"))
            {
                this.eventManager.startEventAtLocationIfPossible("BirthdayDating:Abigail_Mine");
            }
            if (e.NewLocation == Game1.getLocationFromName("HaleyHouse"))
            {
                this.eventManager.startEventAtLocationIfPossible("BirthdayDating:Emily");
                this.eventManager.startEventAtLocationIfPossible("BirthdayDating:Haley");
            }
            if (e.NewLocation == Game1.getLocationFromName("HarveyRoom"))
            {
                this.eventManager.startEventAtLocationIfPossible("BirthdayDating:Harvey");
            }
            if (e.NewLocation == Game1.getLocationFromName("ElliottHouse"))
            {
                this.eventManager.startEventAtLocationIfPossible("BirthdayDating:Elliott");
            }
            if (e.NewLocation == Game1.getLocationFromName("SamHouse"))
            {
                this.eventManager.startEventAtLocationIfPossible("BirthdayDating:Sam");
            }
            if (e.NewLocation == Game1.getLocationFromName("JoshHouse"))
            {
                this.eventManager.startEventAtLocationIfPossible("BirthdayDating:Alex");
            }
            if (e.NewLocation == Game1.getLocationFromName("AnimalShop"))
            {
                this.eventManager.startEventAtLocationIfPossible("BirthdayDating:Shane");
            }

        }

        private void GameLoop_GameLaunched(object sender, GameLaunchedEventArgs e)
        {
            this.birthdayMessages = new BirthdayMessages();
            this.giftManager = new GiftManager();
            MenuUtilities.IsDailyQuestBoard = false;

        }

        /// <summary>Get whether this instance can edit the given asset.</summary>
        /// <param name="asset">Basic metadata about the asset being loaded.</param>
        public bool CanEdit<T>(IAssetInfo asset)
        {
            return asset.AssetNameEquals(@"Data\mail");
        }

        /// <summary>Edit a matched asset.</summary>
        /// <param name="asset">A helper which encapsulates metadata about an asset and enables changes to it.</param>
        public void Edit<T>(IAssetData asset)
        {
            if (asset.AssetNameEquals(@"Data\mail"))
            {
                MailUtilities.EditMailAsset(asset);
            }
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Used to check for player disconnections.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void Multiplayer_PeerDisconnected(object sender, PeerDisconnectedEventArgs e)
        {
            this.birthdayManager.removeOtherPlayerBirthdayData(e.Peer.PlayerID);
        }

        private void Multiplayer_ModMessageReceived(object sender, ModMessageReceivedEventArgs e)
        {
            if (e.FromModID == ModHelper.Multiplayer.ModID && e.Type == MultiplayerSupport.FSTRING_SendBirthdayMessageToOthers)
            {
                string message = e.ReadAs<string>();
                Game1.hudMessages.Add(new HUDMessage(message, 1));
            }

            if (e.FromModID == ModHelper.Multiplayer.ModID && e.Type == MultiplayerSupport.FSTRING_SendBirthdayInfoToOthers)
            {
                KeyValuePair<long, PlayerData> message = e.ReadAs<KeyValuePair<long, PlayerData>>();


                if (message.Key.Equals(Game1.player.UniqueMultiplayerID))
                {
                    this.birthdayManager.playerBirthdayData = message.Value;
                }
                else if (!this.birthdayManager.othersBirthdays.ContainsKey(message.Key))
                {
                    this.birthdayManager.addOtherPlayerBirthdayData(message);
                    MultiplayerSupport.SendBirthdayInfoToConnectingPlayer(e.FromPlayerID);
                    this.Monitor.Log("Got other player's birthday data from: " + Game1.getFarmer(e.FromPlayerID).Name);
                }
                else
                {
                    //Brute force update birthday info if it has already been recevived but dont send birthday info again.
                    this.birthdayManager.updateOtherPlayerBirthdayData(message);
                    this.Monitor.Log("Got other player's birthday data from: " + Game1.getFarmer(e.FromPlayerID).Name);
                }
                string p = Path.Combine("data", Game1.player.Name + "_" + Game1.player.UniqueMultiplayerID + "_" + "FarmhandBirthdays.json");
                if (File.Exists(Path.Combine(ModHelper.DirectoryPath, p)) == false)
                {
                    ModHelper.Data.WriteJsonFile(p, this.birthdayManager.othersBirthdays);
                }
            }
            if (e.FromModID == ModHelper.Multiplayer.ModID && e.Type.Equals(MultiplayerSupport.FSTRING_SendFarmhandBirthdayInfoToPlayer))
            {
                KeyValuePair<long, PlayerData> message = e.ReadAs<KeyValuePair<long, PlayerData>>();
                if (Game1.player.UniqueMultiplayerID == message.Key)
                {
                    ModMonitor.Log("Got requested farmhand birthday info");
                    this.birthdayManager.playerBirthdayData = message.Value;
                }
                else
                {
                    ModMonitor.Log("Picked up message for farmhand birthday but it was sent to the wrong player...");
                }

            }
            if (e.FromModID == ModHelper.Multiplayer.ModID && e.Type == MultiplayerSupport.FSTRING_RequestBirthdayInfoFromServer)
            {
                if (Game1.player.IsMainPlayer)
                {
                    KeyValuePair<long, string> message = e.ReadAs<KeyValuePair<long, string>>();
                    ModMonitor.Log("Got request from farmhand for birthday info" + Game1.getAllFarmhands().ToList().Find(i => i.UniqueMultiplayerID == message.Key).Name);
                    if (this.birthdayManager.othersBirthdays.ContainsKey(message.Key))
                    {
                        ModMonitor.Log("Sending requested farmhand info");
                        MultiplayerSupport.SendFarmandBirthdayInfoToPlayer(message.Key, this.birthdayManager.othersBirthdays[message.Key]);
                    }
                    else
                    {
                        ModMonitor.Log("For some reason requested birthday info was not found...");
                    }
                }
            }
        }



        /// <summary>Raised after the game begins a new day (including when the player loads a save).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            this.birthdayManager.ResetVillagerQueue();
            this.birthdayManager.setCheckedForBirthday(false);

            foreach (KeyValuePair<string, EventHelper> v in this.eventManager.events)
            {
                this.eventManager.clearEventFromFarmer(v.Key);
            }
        }

        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            // show birthday selection menu
            if (Game1.activeClickableMenu != null) return;
            if (Context.IsPlayerFree && !this.birthdayManager.hasChosenBirthday() && e.Button == Configs.modConfig.KeyBinding)
                Game1.activeClickableMenu = new BirthdayMenu(this.birthdayManager.playerBirthdayData.BirthdaySeason, this.birthdayManager.playerBirthdayData.BirthdayDay, this.birthdayManager.setBirthday);
        }

        /// <summary>Raised after the player loads a save slot and the world is initialised.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {

            foreach (IContentPack contentPack in ModHelper.ContentPacks.GetOwned())
            {
                this.happyBirthdayContentPackManager.registerNewContentPack(contentPack);
            }


            this.DataFilePath = Path.Combine("data", $"{Game1.player.Name}_{Game1.player.UniqueMultiplayerID}.json");

            // reset state
            this.birthdayManager.setCheckedForBirthday(false);

            if (Game1.player.IsMainPlayer)
            {
                if (File.Exists(Path.Combine(HappyBirthday.ModHelper.DirectoryPath, "data", $"{Game1.player.Name}_{Game1.player.UniqueMultiplayerID}_FarmhandBirthdays.json")))
                {
                    this.birthdayManager.othersBirthdays = ModHelper.Data.ReadJsonFile<Dictionary<long, PlayerData>>(Path.Combine("data", $"{Game1.player.Name}_{Game1.player.UniqueMultiplayerID}_FarmhandBirthdays.json"));
                    ModMonitor.Log("Loaded in farmhand birthdays for this session.");
                }
                else
                {
                    ModMonitor.Log("Unable to find farmhand birthdays for this session. Does the file exist or is this single player?");
                }
                this.birthdayManager.playerBirthdayData = this.Helper.Data.ReadJsonFile<PlayerData>(this.DataFilePath) ?? new PlayerData();
            }
            else
            {
                ModMonitor.Log("Requesting birthday info from host for player: " + Game1.player.Name);
                MultiplayerSupport.RequestFarmandBirthdayInfoFromServer();
            }

            if (this.birthdayManager.playerBirthdayData != null)
            {
                //ModMonitor.Log("Send all birthday information from " + Game1.player.Name);
                MultiplayerSupport.SendBirthdayInfoToOtherPlayers();
            }


            MailUtilities.RemoveAllBirthdayMail();


            EventHelper communityCenterJunimoBirthday = BirthdayEvents.CommunityCenterJunimoBirthday();
            EventHelper birthdayDating_Penny = BirthdayEvents.DatingBirthday_Penny();
            EventHelper birthdayDating_Penny_Big = BirthdayEvents.DatingBirthday_Penny_BigHome();
            EventHelper birthdayDating_Maru = BirthdayEvents.DatingBirthday_Maru();
            EventHelper birthdayDating_Sebastian = BirthdayEvents.DatingBirthday_Sebastian();
            EventHelper birthdayDating_Leah = BirthdayEvents.DatingBirthday_Leah();

            EventHelper birthdayDating_Abigail = BirthdayEvents.DatingBirthday_Abigail_Seedshop();
            EventHelper birthdayDating_Abigail_Mine = BirthdayEvents.DatingBirthday_Abigail_Mine();


            EventHelper birthdayDating_Emily = BirthdayEvents.DatingBirthday_Emily();
            EventHelper birthdayDating_Haley = BirthdayEvents.DatingBirthday_Haley();
            EventHelper birthdayDating_Harvey = BirthdayEvents.DatingBirthday_Harvey();
            EventHelper birthdayDating_Elliott = BirthdayEvents.DatingBirthday_Elliott();
            EventHelper birthdayDating_Sam = BirthdayEvents.DatingBirthday_Sam();
            EventHelper birthdayDating_Alex = BirthdayEvents.DatingBirthday_Alex();
            EventHelper birthdayDating_Shane = BirthdayEvents.DatingBirthday_Shane();

            this.eventManager.addEvent(communityCenterJunimoBirthday);
            this.eventManager.addEvent(birthdayDating_Penny);
            this.eventManager.addEvent(birthdayDating_Penny_Big);
            this.eventManager.addEvent(birthdayDating_Maru);
            this.eventManager.addEvent(birthdayDating_Sebastian);
            this.eventManager.addEvent(birthdayDating_Leah);

            this.eventManager.addEvent(birthdayDating_Abigail);
            this.eventManager.addEvent(birthdayDating_Abigail_Mine);

            this.eventManager.addEvent(birthdayDating_Emily);
            this.eventManager.addEvent(birthdayDating_Haley);
            this.eventManager.addEvent(birthdayDating_Harvey);
            this.eventManager.addEvent(birthdayDating_Elliott);
            this.eventManager.addEvent(birthdayDating_Sam);
            this.eventManager.addEvent(birthdayDating_Alex);
            this.eventManager.addEvent(birthdayDating_Shane);
        }

        /// <summary>Raised before the game begins writes data to the save file (except the initial save creation).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnSaving(object sender, SavingEventArgs e)
        {
            if (this.birthdayManager.hasChosenBirthday())
            {
                this.Helper.Data.WriteJsonFile(this.DataFilePath, this.birthdayManager.playerBirthdayData);
                if (Game1.IsMultiplayer)
                {
                    string p = Path.Combine("data", Game1.player.Name + "_" + Game1.player.UniqueMultiplayerID + "_" + "FarmhandBirthdays.json");
                    this.Helper.Data.WriteJsonFile(p, this.birthdayManager.othersBirthdays);
                }
            }

            if (Game1.player.IsMainPlayer == false)
            {

                //StardustCore.Utilities.Serialization.Serializer.JSONSerializer.Serialize(this.DataFilePath, this.PlayerData);
            }
        }

        /// <summary>Raised after the game state is updated (≈60 times per second).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {

            if (!Context.IsWorldReady || Game1.isFestival())
            {
                return;
            }

            if (Game1.eventUp)
            {
                if (this.eventManager != null)
                {
                    this.eventManager.update();
                }
                return;
            }
            else
            {
                if (this.eventManager != null)
                {
                    this.eventManager.update();
                }
            }


            //Below code sets up menus for selecting the new birthday for the player.
            if (!this.birthdayManager.hasChosenBirthday() && Game1.activeClickableMenu == null && Game1.player.Name.ToLower() != "unnamed farmhand")
            {
                if (this.birthdayManager.playerBirthdayData != null)
                {
                    Game1.activeClickableMenu = new BirthdayMenu(this.birthdayManager.playerBirthdayData.BirthdaySeason, this.birthdayManager.playerBirthdayData.BirthdayDay, this.birthdayManager.setBirthday);
                    this.birthdayManager.setCheckedForBirthday(false);
                }
                else
                {
                    this.birthdayManager.playerBirthdayData = new PlayerData();
                    Game1.activeClickableMenu = new BirthdayMenu("", 0, this.birthdayManager.setBirthday);
                    this.birthdayManager.setCheckedForBirthday(false);
                }
            }

            if (!this.birthdayManager.hasCheckedForBirthday() && Game1.activeClickableMenu == null)
            {
                this.birthdayManager.setCheckedForBirthday(true);


                //Don't constantly set the birthday menu.
                if (Game1.activeClickableMenu?.GetType() == typeof(BirthdayMenu))
                    return;

                // ask for birthday date
                if (!this.birthdayManager.hasChosenBirthday() && Game1.activeClickableMenu == null)
                {
                    Game1.activeClickableMenu = new BirthdayMenu(this.birthdayManager.playerBirthdayData.BirthdaySeason, this.birthdayManager.playerBirthdayData.BirthdayDay, this.birthdayManager.setBirthday);
                    this.birthdayManager.setCheckedForBirthday(false);
                }

                if (Game1.activeClickableMenu?.GetType() == typeof(FavoriteGiftMenu))
                    return;
                if (this.birthdayManager.hasChosenBirthday() && Game1.activeClickableMenu == null && this.birthdayManager.hasChoosenFavoriteGift() == false)
                {
                    Game1.activeClickableMenu = new FavoriteGiftMenu();
                    this.birthdayManager.setCheckedForBirthday(false);
                    return;
                }

                this.birthdayManager.setUpPlayersBirthday();
            }


        }




    }
}
