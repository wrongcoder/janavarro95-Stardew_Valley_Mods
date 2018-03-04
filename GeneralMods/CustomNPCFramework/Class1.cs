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
    public class Class1 : Mod
    {
        public static IModHelper ModHelper;
        public static IMonitor ModMonitor;
        public static AssetManager assetManager;
        public static NPCTracker npcTracker;
        public override void Entry(IModHelper helper)
        {
            ModHelper = this.Helper;
            ModMonitor = this.Monitor;
            assetManager = new AssetManager();
            initializeExamples();
            assetManager.loadAssets();
            StardewModdingAPI.Events.SaveEvents.AfterLoad += SaveEvents_LoadChar;

            StardewModdingAPI.Events.SaveEvents.BeforeSave += SaveEvents_BeforeSave;
            StardewModdingAPI.Events.SaveEvents.AfterSave += SaveEvents_AfterSave;

            StardewModdingAPI.Events.LocationEvents.CurrentLocationChanged += LocationEvents_CurrentLocationChanged;
            StardewModdingAPI.Events.GameEvents.UpdateTick += GameEvents_UpdateTick;
            npcTracker = new NPCTracker();
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
                ModMonitor.Log(v.sprite.spriteHeight.ToString());
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
            string path = Path.Combine(ModHelper.DirectoryPath, "Content", "Graphics", "NPCS", "Characters", "RainMan");
            assetManager.addPathCreateDirectory(new KeyValuePair<string, string>("characters", path));
            Texture2D tex = ModHelper.Content.Load<Texture2D>(Path.Combine(getShortenedDirectory(path).Remove(0, 1), "character.png"));
            ModMonitor.Log("PATH???: " + path);
            ExtendedNPC myNpc3 = new ExtendedNPC(new Framework.ModularNPCS.Sprite(Path.Combine(path,"character.png")),null, new Vector2(14, 14)*Game1.tileSize, 2, "b2");
            npcTracker.addNewNPCToLocation(Game1.getLocationFromName("BusStop"),myNpc3);
            myNpc3.SetMovingDown(true);
        }

        public void initializeExamples()
        {
            string dirPath = Path.Combine(ModHelper.DirectoryPath, "Content", "Templates");
            assetManager.addPathCreateDirectory(new KeyValuePair<string, string>("templates", dirPath));
            string filePath =Path.Combine(dirPath, "Example.json");
            if (File.Exists(filePath)) return;
            string getRelativePath = getShortenedDirectory(filePath);
            ModMonitor.Log("THIS IS THE PATH::: " + getRelativePath);
            AssetInfo info = new AssetInfo("Example", new Vector2(16, 16), false);
            info.writeToJson(filePath);
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
