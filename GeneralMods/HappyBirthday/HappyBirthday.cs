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

        /// <summary>Class to handle all birthday messages for this mod.</summary>
        public BirthdayMessages birthdayMessages;

        /// <summary>Class to handle all birthday gifts for this mod.</summary>
        public GiftManager giftManager;

        public static HappyBirthday Instance;

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

            Instance = this;
            Configs = new ConfigManager();
            Configs.initializeConfigs();

            this.Helper.Events.GameLoop.DayStarted += this.OnDayStarted;
            this.Helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
            this.Helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
            this.Helper.Events.GameLoop.Saving += this.OnSaving;
            this.Helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            this.Helper.Events.Display.MenuChanged += MenuUtilities.OnMenuChanged;
            this.Helper.Events.Display.RenderedActiveMenu += RenderUtilities.OnRenderedActiveMenu;
            this.Helper.Events.Display.RenderedHud += RenderUtilities.OnRenderedHud;
            this.Helper.Events.Multiplayer.ModMessageReceived += MultiplayerUtilities.Multiplayer_ModMessageReceived;
            this.Helper.Events.Multiplayer.PeerDisconnected += MultiplayerUtilities.Multiplayer_PeerDisconnected;
            this.Helper.Events.GameLoop.GameLaunched += this.GameLoop_GameLaunched;
            this.Helper.Events.Player.Warped += BirthdayEventUtilities.Player_Warped;
            this.Helper.Events.GameLoop.ReturnedToTitle += this.GameLoop_ReturnedToTitle;

            BirthdayEventUtilities.BirthdayEventManager = new EventManager();
            this.birthdayManager = new BirthdayManager();

            this.happyBirthdayContentPackManager = new HappyBirthdayContentPackManager();
            this.translationInfo = new TranslationInfo();

        }

        private void GameLoop_ReturnedToTitle(object sender, ReturnedToTitleEventArgs e)
        {
            BirthdayEventUtilities.BirthdayEventManager = new EventManager();
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


        /// <summary>Raised after the game begins a new day (including when the player loads a save).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            this.birthdayManager.onDayStarted(sender, e);

            BirthdayEventUtilities.ClearEventsFromFarmer();
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

            foreach (IContentPack contentPack in this.Helper.ContentPacks.GetOwned())
            {
                this.happyBirthdayContentPackManager.registerNewContentPack(contentPack);
            }


            this.DataFilePath = Path.Combine("data", $"{Game1.player.Name}_{Game1.player.UniqueMultiplayerID}.json");

            // reset state
            this.birthdayManager.setCheckedForBirthday(false);

            if (Game1.player.IsMainPlayer)
            {
                if (File.Exists(Path.Combine(this.Helper.DirectoryPath, "data", $"{Game1.player.Name}_{Game1.player.UniqueMultiplayerID}_FarmhandBirthdays.json")))
                {
                    this.birthdayManager.othersBirthdays = this.Helper.Data.ReadJsonFile<Dictionary<long, PlayerData>>(Path.Combine("data", $"{Game1.player.Name}_{Game1.player.UniqueMultiplayerID}_FarmhandBirthdays.json"));
                    this.Monitor.Log("Loaded in farmhand birthdays for this session.");
                }
                else
                {
                    this.Monitor.Log("Unable to find farmhand birthdays for this session. Does the file exist or is this single player?");
                }
                this.birthdayManager.playerBirthdayData = this.Helper.Data.ReadJsonFile<PlayerData>(this.DataFilePath) ?? new PlayerData();
            }
            else
            {
                this.Monitor.Log("Requesting birthday info from host for player: " + Game1.player.Name);
                MultiplayerUtilities.RequestFarmandBirthdayInfoFromServer();
            }

            if (this.birthdayManager.playerBirthdayData != null)
            {
                //ModMonitor.Log("Send all birthday information from " + Game1.player.Name);
                MultiplayerUtilities.SendBirthdayInfoToOtherPlayers();
            }


            MailUtilities.RemoveAllBirthdayMail();
            BirthdayEventUtilities.InitializeBirthdayEvents();
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

            BirthdayEventUtilities.UpdateEventManager();


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
