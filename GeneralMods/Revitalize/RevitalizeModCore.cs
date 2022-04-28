using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Objects;
using StardewValley.Tools;
using xTile.Dimensions;
using Omegasis.Revitalize.Framework.Utilities;
using Omegasis.Revitalize.Framework.Managers;
using Omegasis.Revitalize.Framework.Configs;
using Omegasis.Revitalize.Framework.Constants.ItemIds.Objects;
using Omegasis.Revitalize.Framework.Constants;
using Omegasis.Revitalize.Framework.Crafting;
using Omegasis.Revitalize.Framework.Environment;
using Omegasis.Revitalize.Framework.Hacks;
using Omegasis.Revitalize.Framework.Player;
using Omegasis.Revitalize.Framework.SaveData;
using Omegasis.Revitalize.Framework.Utilities.Serialization;
using Omegasis.Revitalize.Framework.Objects;
using Omegasis.Revitalize.Framework.World;
using Omegasis.Revitalize.Framework.World.WorldUtilities.Shops;

namespace Omegasis.Revitalize
{

    // TODO:
    /*
    // -Make this mod able to load content packs for easier future modding
    //
    //  -Multiple Lights On Object
    //  -Illumination Colors
    //  Furniture:
    //      -rugs 
    //      -tables
    //      -lamps
    //      -dressers/other storage containers 
    //      -fun interactables
    //          -Arcade machines
    //      -More crafting tables 
    //      -Baths (see chairs but swimming)
    //
    //  -Machines
    //      !=Energy
    //            Generators:
                  -solar
                  -burnable
                  -watermill
                  -windmill
                  -crank (costs stamina)
                  Storage:
                  -Batery Pack
             -Mini-greenhouse
                   -takes fertilizer which can do things like help crops grow or increase prodcuction yield/quality.
                   -takes crop/extended crop seeds
                   -takes sprinklers
                   -has grid (1x1, 2x2, 3x3, 4x4, 5x5) system for growing crops/placing sprinkers
                   -sprinkers auto water crops
                   -can auto harvest
                   -hover over crop to see it's info
                   -can be upgraded to grow crops from specific seasons with season stones (spring,summer, fall winter) (configurable if they are required)
                   -Add in season stone recipe

    //      -Furnace
    //      -Seed Maker
    //      -Stone Quarry
    //      -Mayo Maker
    //      -Cheese Maker
            -Yogurt Maker
                   -Fruit yogurts (artisan good)
    //      -Auto fisher
    //      -Auto Preserves
    //      -Auto Keg
    //      -Auto Cask
    //      -Calcinator (oil+stone: produces titanum?)
    //  -Materials
    //      -Tin/Bronze/Alluminum/Silver?Platinum/Etc (all but platinum: may add in at a later date)
            -titanium (d0ne)
            -Alloys!
                -Brass (done)
                -Electrum (done)
                -Steel (done)
                -Bronze (done)
            -Mythrill
            
            -Star Metal
            -Star Steel
            -Cobalt
        -Liquids
            -oil
            -water
            -coal
            -juice???
            -lava?

        -Dyes!
            -Dye custom objects certain colors!
            -Rainbow Dye -(set a custom object to any color)
            -red, green, blue, yellow, pink, etc
            -Make dye from flowers/coal/algee/minerals/gems (black), etc
                -soapstone (washes off dye)
                -Lunarite (white)
        Dye Machine
            -takes custom object and dye
            -dyes the object
            -can use water to wash off dye.
            -maybe dye stardew valley items???
            -Dyed Wool (Artisan good)

        Menus:
    //  -Crafting Menu
    //  -Item Grab Menu (Extendable) (Done!)
    //   -Yes/No Dialogue Box
    //   -Multi Choice dialogue box


    //  -Gift Boxes

    //  Magic!
    //      -Alchemy Bags
    //      -Transmutation
    //      -Effect Crystals
    //      -Spell books
    //      -Potions!
    //      -Magic Meter
    //      -Connected chests (3 digit color code) much like Project EE2 from MC
    //
    //
    //  -Food
            -multi flavored sodas

    //  -Bigger chests
    //
    //  Festivals
    //      -Firework festival
    //      -Horse Racing Festival
            -Valentines day (Maybe make this just one holiday)
                -Spring. Male to female gifts.
                -Winter. Female to male gifts. 
    //  Stargazing???
    //      -Moon Phases+DarkerNight
    //  Bigger/Better Museum?
    // 
    //  Equippables!
    //      -accessories that provide buffs/regen/friendship
    //      -braclets/rings/broaches....more crafting for these???
    //      
    //  Music???
    //      -IDK maybe add in instruments???
    //      
    //  More buildings????
    //  
    //  More Animals???
    //  
    //  Readable Books?
    //  
    //  Custom NPCs for shops???
    //
    //  Minigames:
    //      Frisbee Minigame?
    //      HorseRace Minigame/Betting?
    //  
    //  Locations:
            -Make extra bus stop sign that travels between new towns/locations.
    //      -Small Island Home?
    //      -New town inspired by FOMT;Mineral Town/The Valley HM DS
    //
    //  More crops
    //      -RF Crops
    //      -HM Crops
    //
    //  More monsters
    //  -boss fights
    //
    //  More dungeons??

    //  More NPCS?

        Accessories
        (recover hp/stamina,max hp,more friendship ,run faster, take less damage, etc)
            -Neckalces
            -Broaches
            -Earings
            -Pendants
    */

    public class RevitalizeModCore : Mod, IAssetEditor
    {

        public static RevitalizeModCore Instance;

        public static IModHelper ModHelper;
        public static IMonitor ModMonitor;
        public static IManifest Manifest;

        /// <summary>
        /// Keeps track of custom objects.
        /// </summary>
        public static ObjectManager ObjectManager;

        public static PlayerInfo playerInfo;

        public static Serializer Serializer;

        public static CraftingManager CraftingManager;

        public static ConfigManager Configs;

        public static SaveDataManager SaveDataManager;

        public static MailManager MailManager;

        public override void Entry(IModHelper helper)
        {
            Instance = this;

            ModHelper = helper;
            ModMonitor = this.Monitor;
            Manifest = this.ModManifest;
            Configs = new ConfigManager();
            SaveDataManager = new SaveDataManager();
            MailManager = new MailManager();

            this.createDirectories();
            Serializer = new Serializer();
            playerInfo = new PlayerInfo();

            //Loads in textures to be used by the mod.
            TextureManagers.loadInTextures();

            //Loads in objects to be use by the mod.
            ObjectManager = new ObjectManager(Manifest);

            //Adds in event handling for the mod.
            ModHelper.Events.GameLoop.SaveLoaded += this.GameLoop_SaveLoaded;
            ModHelper.Events.GameLoop.SaveCreated += this.GameLoop_SaveCreated;

            ModHelper.Events.GameLoop.TimeChanged += this.GameLoop_TimeChanged;
            ModHelper.Events.GameLoop.UpdateTicked += this.GameLoop_UpdateTicked;
            ModHelper.Events.GameLoop.ReturnedToTitle += this.GameLoop_ReturnedToTitle;

            ModHelper.Events.Player.Warped += ObjectManager.resources.OnPlayerLocationChanged;
            ModHelper.Events.GameLoop.DayStarted += this.GameLoop_DayStarted;
            ModHelper.Events.GameLoop.DayEnding += this.GameLoop_DayEnding;

            ModHelper.Events.Input.ButtonPressed += ObjectInteractionHacks.Input_CheckForObjectInteraction;

            ModHelper.Events.Display.RenderedWorld += ObjectInteractionHacks.Render_RenderCustomObjectsHeldInMachines;
            //ModHelper.Events.Display.Rendered += MenuHacks.EndOfDay_OnMenuChanged;
            ModHelper.Events.Display.MenuChanged += ShopUtilities.OnNewMenuOpened;

            ModHelper.Events.Display.MenuChanged += MailManager.onNewMenuOpened ;
            //ModHelper.Events.GameLoop.Saved += MenuHacks.EndOfDay_CleanupForNewDay;
            ModHelper.Events.Input.ButtonPressed += ObjectInteractionHacks.ResetNormalToolsColorOnLeftClick;

            ModHelper.Events.GameLoop.GameLaunched += this.GameLoop_GameLaunched;



        }

        ///~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~///
        ///                     Initialize Mod Content                     ///
        ///~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        #region
        private void createDirectories()
        {
            Directory.CreateDirectory(Path.Combine(this.Helper.DirectoryPath, "Configs"));

            Directory.CreateDirectory(Path.Combine(this.Helper.DirectoryPath, "Content"));
            Directory.CreateDirectory(Path.Combine(this.Helper.DirectoryPath, "Content", "Graphics"));
            //Directory.CreateDirectory(Path.Combine(this.Helper.DirectoryPath, "Content", "Graphics","Furniture"));
            Directory.CreateDirectory(Path.Combine(this.Helper.DirectoryPath, "Content", "Graphics", "Furniture", "Chairs"));
            Directory.CreateDirectory(Path.Combine(this.Helper.DirectoryPath, "Content", "Graphics", "Furniture", "Lamps"));
            Directory.CreateDirectory(Path.Combine(this.Helper.DirectoryPath, "Content", "Graphics", "Furniture", "Tables"));
        }


        #endregion


        private void GameLoop_GameLaunched(object sender, StardewModdingAPI.Events.GameLaunchedEventArgs e)
        {
            ObjectManager.loadItemsFromDisk();
            //Adds in recipes to the mod.
            CraftingManager = new CraftingManager();
            CraftingManager.initializeRecipeBooks();

            Serializer.SerializeTypesForXMLUsingSpaceCore();
        }

        private void GameLoop_ReturnedToTitle(object sender, StardewModdingAPI.Events.ReturnedToTitleEventArgs e)
        {
        }



        /// <summary>
        /// What happens when a new day starts.
        /// </summary>
        /// <param name="senderm"></param>
        /// <param name="e"></param>
        private void GameLoop_DayStarted(object senderm, StardewModdingAPI.Events.DayStartedEventArgs e)
        {
            ObjectManager.resources.DailyResourceSpawn(senderm, e);
            ShopUtilities.OnNewDay(senderm, e);
            MailManager.tryToAddMailToMailbox();
        }

        /// <summary>
        /// Called when the day is ending. At this point the save data should all be saved.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameLoop_DayEnding(object sender, StardewModdingAPI.Events.DayEndingEventArgs e)
        {
            SaveDataManager.save();
        }

        private void GameLoop_SaveLoaded(object sender, StardewModdingAPI.Events.SaveLoadedEventArgs e)
        {
            SaveDataManager.loadOrCreateSaveData();
            MailManager.tryToAddMailToMailbox();

            PlayerUtilities.HasCompletedSpecialOrder("ok");

            //HACKS
            Game1.player.Money = 100_000;
            Game1.player.addItemToInventoryBool(ObjectManager.getItem(CraftingStations.WorkStation_Id));
            Game1.player.addItemsByMenuIfNecessary(new List<Item>()
            {
                //new StardewValley.Object((int)Enums.SDVObject.Coal,100),
                //ObjectManager.GetItem(Ingots.SteelIngot, 20),
                //ObjectManager.GetItem(CraftingStations.Anvil,1),
                //ObjectManager.GetItem(Machines.AdvancedSolarPanelV1,1),
                //ObjectManager.GetItem(Machines.SolarArrayV1,1),
                //new StardewValley.Object(Vector2.Zero,(int)Enums.SDVBigCraftable.Furnace,false),
                //new StardewValley.Object((int)Enums.SDVObject.CopperOre,10),
                //ObjectManager.GetItem(Machines.MiningDrillV1),
                //new StardewValley.Object((int)Enums.SDVObject.IronBar,100),
                //ObjectManager.GetItem(Machines.WindmillV1),
               ObjectManager.getItem(Machines.HayMaker),
                new StardewValley.Object((int)Enums.SDVObject.Corn,10),
               // //ObjectManager.getItem(Enums.SDVObject.Stone,999),
               // ObjectManager.getItem(Enums.SDVObject.Wood,999),
               // ObjectManager.getItem(Enums.SDVObject.Clay,100),
              //  ObjectManager.getItem(Enums.SDVObject.CopperBar,100)
              ObjectManager.getItem(Revitalize.Framework.Constants.ItemIds.Items.Blueprints.Workbench_AnvilCraftingRecipeBlueprint),
              ObjectManager.getItem(FarmingObjects.IrrigatedGardenPot),
              ObjectManager.getItem(Enums.SDVBigCraftable.GardenPot)
            }) ;


            Framework.World.WorldUtilities.Utilities.InitializeGameWorld();

        }

        private void GameLoop_SaveCreated(object sender, StardewModdingAPI.Events.SaveCreatedEventArgs e)
        {
            Framework.World.WorldUtilities.Utilities.InitializeGameWorld();
        }


        private void GameLoop_UpdateTicked(object sender, StardewModdingAPI.Events.UpdateTickedEventArgs e)
        {
            DarkerNight.SetDarkerColor();
            playerInfo.update();
        }

        private void GameLoop_TimeChanged(object sender, StardewModdingAPI.Events.TimeChangedEventArgs e)
        {
            DarkerNight.CalculateDarkerNightColor();
        }



        /// <summary>
        ///Logs information to the console.
        /// </summary>
        /// <param name="message"></param>
        public static void log(object message, bool StackTrace = true)
        {
            if (StackTrace)
                ModMonitor.Log(message.ToString() + " " + getFileDebugInfo());
            else
                ModMonitor.Log(message.ToString());
        }

        public static string getFileDebugInfo()
        {
            string currentFile = new System.Diagnostics.StackTrace(true).GetFrame(2).GetFileName();
            int currentLine = new System.Diagnostics.StackTrace(true).GetFrame(2).GetFileLineNumber();
            return currentFile + " line:" + currentLine;
        }

        public bool CanEdit<T>(IAssetInfo asset)
        {
            return MailManager.canEditAsset(asset);
        }

        public void Edit<T>(IAssetData asset)
        {
            MailManager.editMailAsset(asset);
        }
    }
}
