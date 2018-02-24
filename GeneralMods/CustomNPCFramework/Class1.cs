using CustomNPCFramework.Framework.Graphics;
using CustomNPCFramework.Framework.NPCS;
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
        public override void Entry(IModHelper helper)
        {
            ModHelper = this.Helper;
            ModMonitor = this.Monitor;
            assetManager = new AssetManager();
            initializeExamples();
            assetManager.loadAssets();
            StardewModdingAPI.Events.SaveEvents.AfterLoad += SaveEvents_LoadChar;
            StardewModdingAPI.Events.LocationEvents.CurrentLocationChanged += LocationEvents_CurrentLocationChanged;
            StardewModdingAPI.Events.GameEvents.UpdateTick += GameEvents_UpdateTick;
        }

        private void GameEvents_UpdateTick(object sender, EventArgs e)
        {
            if (Game1.player.currentLocation == null) return;
            foreach (var v in Game1.player.currentLocation.characters)
            {
                v.speed = 5;
                v.MovePosition(Game1.currentGameTime, Game1.viewport, Game1.player.currentLocation);
                ModMonitor.Log(v.name);
            }
        }

        private void LocationEvents_CurrentLocationChanged(object sender, StardewModdingAPI.Events.EventArgsCurrentLocationChanged e)
        {
         
        }

        private void SaveEvents_LoadChar(object sender, EventArgs e)
        {
            string path = Path.Combine(ModHelper.DirectoryPath, "Content", "Graphics", "NPCS", "Characters", "RainMan");
            assetManager.addPathCreateDirectory(new KeyValuePair<string, string>("characters", path));
            Texture2D tex=ModHelper.Content.Load<Texture2D>(Path.Combine(getShortenedDirectory(path).Remove(0,1), "character.png"));
            ExtendedNPC myNpc3 = new ExtendedNPC(new StardewValley.AnimatedSprite(tex, 0, 16, 32), new Vector2(14, 14)*Game1.tileSize, 2, "b2");
            Game1.getLocationFromName("BusStop").addCharacter(myNpc3);
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
            
            string[] spliter = path.Split(new string[] { ModHelper.DirectoryPath },StringSplitOptions.None);
            return spliter[1];
        }

        public static string getRelativeDirectory(string path)
        {
            string s = getShortenedDirectory(path);
            return s.Remove(0, 1);
        }
    }
}
