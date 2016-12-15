using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using StardewValley;
using Microsoft.Xna.Framework;
using StardewModdingAPI.Events;
using System.Xml.Serialization;
using StardewValley.Characters;
using StardewValley.Monsters;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using StardewValley.Quests;
using Newtonsoft.Json;
using System.IO;
using Revitalize.Resources;
using Revitalize.Objects;
using Revitalize.Objects.Machines;

namespace Revitalize
{
    public class Class1 :Mod
    {
        public static string key_binding="P";
        public static string key_binding2 = "L";
        public static string path;
        bool hasCleanedUp;
     

        public override void Entry(IModHelper helper)
        {
            StardewModdingAPI.Events.ControlEvents.KeyPressed += ShopCall;
            StardewModdingAPI.Events.GameEvents.UpdateTick += BedCleanUpCheck;

            hasCleanedUp = true;
            path = Helper.DirectoryPath;
            Util.acceptedTypes = new Dictionary<string, Util.del>();
            Util.addAllAcceptedTypes();
        }

        private void BedCleanUpCheck(object sender, EventArgs e)
        {
            const int range = 1;

            if (Game1.hasLoadedGame == false) return;
            //Log.Info(Game1.activeClickableMenu.GetType());
            if (Game1.player.currentLocation.name == "FarmHouse")
            {
                Vector2 playerAdj = Game1.player.mostRecentBed;

                int x = Convert.ToInt32(playerAdj.X)/Game1.tileSize;
                int y = Convert.ToInt32(playerAdj.Y)/Game1.tileSize;


                
                    if ((Game1.player.getTileY() >= y - range && Game1.player.getTileY() <= y + range) && (Game1.player.getTileX() >= x - range && Game1.player.getTileY() <= x + range))
                    {
                    if (hasCleanedUp == false)
                    {
                        Log.AsyncC("CleanUp!");
                        CleanUp.cleanUpInventory();
                        hasCleanedUp = true;
                    }
                }
                    else
                    {
                    CleanUp.restoreInventory();
                        hasCleanedUp = false;
                    }     
            }
        }
        private void ShopCall(object sender, StardewModdingAPI.Events.EventArgsKeyPressed e)
        {
            Game1.timeOfDay = 2500;
            if (Game1.activeClickableMenu != null) return;
            if (e.KeyPressed.ToString() == key_binding)
            {

                List<Item> objShopList = new List<Item>();

                objShopList.Add(new Light(3, Vector2.Zero, LightColors.DeepSkyBlue));
              //  objShopList.Add(new Spawner(3, Vector2.Zero, 9));


                List<Item> my_shop_list = new List<Item>();
              string  texturePath = "TileSheets\\furniture";
                var I = new Objects.shopObject(3, Vector2.Zero,objShopList,texturePath);
                I.name = "Shop Chair";
                my_shop_list.Add((I));

               // my_shop_list.Add((new Decoration(1120, Vector2.Zero)));
                Game1.activeClickableMenu = new StardewValley.Menus.ShopMenu(my_shop_list, 0, null);
                
                if (Game1.player == null) return;
                
            }

            
            if (e.KeyPressed.ToString() == key_binding2)
            {

             

            //    string load = Path.Combine(PathOnDisk, "this_thing.json");
            //    Game1.player=ReadFromJsonFile<StardewValley.Farmer>(load);
            }
            
            }
            

    }
}
