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
using StardewValley.Locations;
using Revitalize.Locations;
using Revitalize.Menus;
using Microsoft.Xna.Framework.Input;

namespace Revitalize
{
    /// <summary>
    /// TODO:
    /// Get Time lapse code working so that machines propperly work though the night since I technically remove them.
    /// Art. Lots of Art.
    /// Clean up the freaking code. Geeze it's messy.
    /// 
    /// </summary>


    public class Class1 :Mod
    {
        public static string key_binding="P";
        public static string key_binding2 = "E";
        public static string path;
        const int range = 1;

        bool mouseAction;

        bool gametick;

        bool mapWipe;
      public static  bool hasLoadedTerrainList;
        List<GameLoc> newLoc;

        public override void Entry(IModHelper helper)
        {
            StardewModdingAPI.Events.ControlEvents.KeyPressed += ShopCall;
            StardewModdingAPI.Events.ControlEvents.MouseChanged += ControlEvents_MouseChanged;
            StardewModdingAPI.Events.GameEvents.UpdateTick +=gameMenuCall;
           //  StardewModdingAPI.Events.GameEvents.UpdateTick += BedCleanUpCheck;
            StardewModdingAPI.Events.GameEvents.GameLoaded += GameEvents_GameLoaded;
            StardewModdingAPI.Events.GameEvents.OneSecondTick += MapWipe;
            StardewModdingAPI.Events.TimeEvents.DayOfMonthChanged += Util.ResetAllDailyBooleans;
            StardewModdingAPI.Events.SaveEvents.AfterLoad += SaveEvents_AfterLoad;

            StardewModdingAPI.Events.SaveEvents.BeforeSave += SaveEvents_BeforeSave;
            StardewModdingAPI.Events.SaveEvents.AfterSave += SaveEvents_AfterSave;
            StardewModdingAPI.Events.SaveEvents.AfterLoad += SaveEvents_AfterSave;

            StardewModdingAPI.Events.GameEvents.UpdateTick += GameEvents_UpdateTick;

            //StardewModdingAPI.Events.TimeEvents.DayOfMonthChanged += Util.WaterAllCropsInAllLocations;
            hasLoadedTerrainList = false;
            path = Helper.DirectoryPath;
            newLoc = new List<GameLoc>();
        }

        private void SaveEvents_AfterLoad(object sender, EventArgs e)
        {
            
        }

        private void SaveEvents_AfterSave(object sender, EventArgs e)
        {
            Serialize.createDirectories();
            Serialize.restoreAllModObjects();
        }

        private void SaveEvents_BeforeSave(object sender, EventArgs e)
        {
            Serialize.cleanUpInventory(); 
            Serialize.cleanUpWorld(); //grabs all of the items that im tracking and serializes them
        }

        private void GameEvents_UpdateTick(object sender, EventArgs e)
        {
            if (Game1.player.isMoving() == true && hasLoadedTerrainList == false)
            {
                Lists.loadAllLists();
                Util.WaterAllCropsInAllLocations();
            }

        }

    

        private void ControlEvents_MouseChanged(object sender, EventArgsMouseStateChanged e)
        {
          
            if (Game1.activeClickableMenu != null) return;
            if (Game1.eventUp == true) return;

            if (mouseAction == true) return;


            var mState = Microsoft.Xna.Framework.Input.Mouse.GetState();


            if (mState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed || mState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                if (Game1.player.ActiveObject != null)
                {
                    mouseAction = true;

                    string s = Game1.player.ActiveObject.getCategoryName();
                   // Log.AsyncC(s);
                    if (Dictionaries.interactionTypes.ContainsKey(s))
                    {
                        Dictionaries.interactFunction f;
                        Dictionaries.interactionTypes.TryGetValue(s,out f);
                        f.Invoke();
                    }
                    

                }
                else {
                    return;
                }
                //this.minutesUntilReady = 30;
            }

            if (mState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released || mState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
            {
                mouseAction = false;
                //this.minutesUntilReady = 30;
            }
        }

        private void GameEvents_GameLoaded(object sender, EventArgs e)
        {
            Dictionaries.initializeDictionaries();
            Lists.initializeAllLists();

            mapWipe = false;

        }


        public void MapWipe(object sender, EventArgs e)
        {
          
            if (mapWipe == false) return;
            if (Game1.hasLoadedGame == false) return;
            if (Game1.player == null) return;
            if (Game1.player.currentLocation == null) return;
            if (Game1.player.isMoving() == true)
            {


                foreach (var v in Game1.locations)
                {
                    GameLoc R = (new GameLoc(v.Map, v.name));

                    if (R.name == "Town" || R.name == "town")
                    {
                        Log.AsyncO("Adding Town");
                        R = new ModTown(v.Map, v.name);
                    }
                    newLoc.Add(R);
                    Log.AsyncC("DONE1");
                }
                Game1.locations.Clear();
                foreach (var v in newLoc)
                {
                    Game1.locations.Add(v);
                    Log.AsyncC("DONE2");
                }
                Log.AsyncC("DONE");
                mapWipe = false;
            }


            
            
        }

        /*
        private void BedCleanUpCheck(object sender, EventArgs e)
        {
            //Game1.options.menuButton = null;

            if (Game1.hasLoadedGame == false) return;
            if (Game1.player == null) return;
            if (Game1.player.currentLocation == null) return;
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
                        Serialize.cleanUpInventory();
                        hasCleanedUp = true;
                    }
                }
                    else
                    {
                    Serialize.restoreInventory();
                        hasCleanedUp = false;
                    }     
            }
        }
        */
       

        private void gameMenuCall(object sender, EventArgs e)
        {

            
            if (gametick == true)
            {
               // System.Threading.Thread.Sleep(1);
               
                   Game1.activeClickableMenu = new GameMenu();
            }
            gametick = false;
            
        }


        private void ShopCall(object sender, StardewModdingAPI.Events.EventArgsKeyPressed e)
        {
            Game1.player.money = 9999;
          //  Log.AsyncG(Game1.tileSize);

            //Game1.timeOfDay = 2500;
            if (Game1.activeClickableMenu != null) return;
            if (e.KeyPressed.ToString() == key_binding)
            {

                List<Item> objShopList = new List<Item>();
                List<Item> newInventory = new List<Item>();

               
           
                //  objShopList.Add(new Spawner(3, Vector2.Zero, 9));
                objShopList.Add(new Light(3, Vector2.Zero, LightColors.Aquamarine));
                objShopList.Add(new Quarry(3, Vector2.Zero,9,"copper"));
                objShopList.Add(new Decoration(3, Vector2.Zero));
                objShopList.Add(new StardewValley.Object(495, 1));
                objShopList.Add(new StardewValley.Object(496, 1));
                objShopList.Add(new StardewValley.Object(497, 1));
                objShopList.Add(new StardewValley.Object(498, 1));
                objShopList.Add(new StardewValley.Object(770, 1));
                foreach (var v in objShopList)
                {
                    newInventory.Add(v);
                 //   Log.AsyncG("GRRR");
                }
                objShopList.Add(new GiftPackage(1120, Vector2.Zero,newInventory));

                // my_shop_list.Add((new Decoration(1120, Vector2.Zero)));
                objShopList.Add(new ExtraSeeds(1, Vector2.Zero));
                objShopList.Add(new ExtraSeeds(2, Vector2.Zero));
                Game1.activeClickableMenu = new StardewValley.Menus.ShopMenu(objShopList, 0, null);
                
                if (Game1.player == null) return;
                
            }

            
            if (e.KeyPressed.ToString() == key_binding2)
            {
                gametick = true;
            }

        


        }
            

    }
}
