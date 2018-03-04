using CustomNPCFramework.Framework.Enums;
using CustomNPCFramework.Framework.Graphics;
using CustomNPCFramework.Framework.ModularNPCS;
using CustomNPCFramework.Framework.ModularNPCS.CharacterAnimationBases;
using CustomNPCFramework.Framework.NPCS;
using CustomNPCFramework.Framework.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNPCFramework
{
    /// <summary>
    /// TODO:
    /// List all asset managers in use.
    /// Have all asset managers list what assets they are using.
    /// FIX NPC GENERATION IN ASSETPOOL.CS
    /// </summary>


    public class Class1 : Mod
    {
        public static IModHelper ModHelper;
        public static IMonitor ModMonitor;

        public static NPCTracker npcTracker;
        public static AssetPool assetPool;
        public override void Entry(IModHelper helper)
        {
            ModHelper = this.Helper;
            ModMonitor = this.Monitor;

            StardewModdingAPI.Events.SaveEvents.AfterLoad += SaveEvents_LoadChar;

            StardewModdingAPI.Events.SaveEvents.BeforeSave += SaveEvents_BeforeSave;
            StardewModdingAPI.Events.SaveEvents.AfterSave += SaveEvents_AfterSave;

            StardewModdingAPI.Events.LocationEvents.CurrentLocationChanged += LocationEvents_CurrentLocationChanged;
            StardewModdingAPI.Events.GameEvents.UpdateTick += GameEvents_UpdateTick;
            npcTracker = new NPCTracker();
            assetPool = new AssetPool();
            var assetManager = new AssetManager();
            assetPool.addAssetManager(new KeyValuePair<string, AssetManager>("testNPC", assetManager));
            initializeExamples();
            initializeAssetPool();
            assetPool.loadAllAssets();
        }

        public void initializeAssetPool()
        {
            string path = Path.Combine(ModHelper.DirectoryPath, "Content", "Graphics", "NPCS", "Characters", "RainMan");
            assetPool.getAssetManager("testNPC").addPathCreateDirectory(new KeyValuePair<string, string>("characters", path));
        }

        private void SaveEvents_AfterSave(object sender, EventArgs e)
        {
            npcTracker.afterSave();
        }

        private void SaveEvents_BeforeSave(object sender, EventArgs e)
        {
            npcTracker.cleanUpBeforeSave();
        }

        private void GameEvents_UpdateTick(object sender, EventArgs e)
        {
            if (Game1.player.currentLocation == null) return;
            foreach (var v in Game1.player.currentLocation.characters)
            {
                v.speed = 5;
                //v.MovePosition(Game1.currentGameTime, Game1.viewport, Game1.player.currentLocation);
                //ModMonitor.Log(v.sprite.spriteHeight.ToString());
            }
        }

        private void LocationEvents_CurrentLocationChanged(object sender, StardewModdingAPI.Events.EventArgsCurrentLocationChanged e)
        {
         
        }

        /// <summary>
        /// Used to spawn a custom npc just as an example. Don't keep this code.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveEvents_LoadChar(object sender, EventArgs e)
        {

            //Texture2D tex = ModHelper.Content.Load<Texture2D>(Path.Combine(getShortenedDirectory(path).Remove(0, 1), "character.png"));
            if (assetPool.getAssetManager("testNPC").getAssetByName("character") == null)
            {
                
                    ModMonitor.Log("HMMMMM", LogLevel.Error);
                
            }
            var pair= assetPool.getAssetManager("testNPC").getAssetByName("character").getPathTexturePair();
            if (pair.Value == null)
            {
                ModMonitor.Log("UGGGGGGG", LogLevel.Error);
            }
            ExtendedNPC myNpc3 = new ExtendedNPC(new Framework.ModularNPCS.Sprite(pair.Key,pair.Value),null, new Vector2(14, 14)*Game1.tileSize, 2, "b2");
            npcTracker.addNewNPCToLocation(Game1.getLocationFromName("BusStop"),myNpc3);
            myNpc3.SetMovingDown(true);
        }

        public void initializeExamples()
        {
            
            string dirPath = Path.Combine(ModHelper.DirectoryPath, "Content", "Templates");
            var aManager=assetPool.getAssetManager("testNPC");
            aManager.addPathCreateDirectory(new KeyValuePair<string, string>("templates", dirPath));
            string filePath =Path.Combine(dirPath, "Example.json");
            if (!File.Exists(filePath))
            {
                string getRelativePath = getShortenedDirectory(filePath);
                ModMonitor.Log("THIS IS THE PATH::: " + getRelativePath);
                AssetInfo info = new AssetInfo("MyExample",new NamePairings("ExampleL","ExampleR","ExampleU","ExampleD"), new Vector2(16, 16), false);
                info.writeToJson(filePath);

            }
            string filePath2 = Path.Combine(dirPath, "AdvancedExample.json");
            if (!File.Exists(filePath2))
            {

                ExtendedAssetInfo info2 = new ExtendedAssetInfo("AdvancedExample", new NamePairings("AdvancedExampleL", "AdvancedExampleR", "AdvancedExampleU", "AdvancedExampleD"), new Vector2(16, 16), false, Genders.female, new List<Seasons>()
            {
                Seasons.spring,
                Seasons.summer
            }, PartType.hair
                );
                info2.writeToJson(filePath2);
            }
        }

        public static string getShortenedDirectory(string path)
        {
            string lol = (string)path.Clone();
            string[] spliter = lol.Split(new string[] { ModHelper.DirectoryPath },StringSplitOptions.None);
            return spliter[1];
        }

        public static string getRelativeDirectory(string path)
        {
            string s = getShortenedDirectory(path);
            return s.Remove(0, 1);
        }
    }
}
