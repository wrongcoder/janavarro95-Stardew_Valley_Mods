using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Save_Anywhere_V2.Save_Utilities
{
    public class GameUtilities
    {
        public static bool passiveSave;
        public static bool should_ship;
       public static void save_game()
        {
            /*

            if (Game1.player.currentLocation.name == "Sewer")
            {
                Log.Error("There is an issue saving in the Sewer. Blame the animals for not being saved to the player's save file.");
                Log.Error("Your data has not been saved. Sorry for the issue.");
                return;
            }
            */
            
            //if a player has shipped an item, run this code.
            if (Enumerable.Count<Item>((IEnumerable<Item>)Game1.getFarm().shippingBin) > 0)
            {
                should_ship = true;
                //   Game1.endOfNightMenus.Push((IClickableMenu)new ShippingMenu(Game1.getFarm().shippingBin));
             //   Game1.showEndOfNightStuff(); //shows the nightly shipping menu.
            //    Game1.getFarm().shippingBin.Clear(); //clears out the shipping bin to prevent exploits
            }

            try
            {
                shipping_check();
                //  Game1.activeClickableMenu = new StardewValley.Menus.SaveGameMenu();
            }
            catch(Exception rrr)
            {
                Game1.showRedMessage("Can't save here. See log for error.");
                Mod_Core.thisMonitor.Log(rrr.ToString(), LogLevel.Error);
            }

           // Game1.activeClickableMenu = new StardewValley.Menus.SaveGameMenu(); //This command is what allows the player to save anywhere as it calls the saving function.

            Save_Anywhere_V2.Save_Utilities.Player_Utilities.save_player_info();
            Save_Anywhere_V2.Save_Utilities.Animal_Utilities.save_animal_info();
            Save_Anywhere_V2.Save_Utilities.NPC_Utilities.Save_NPC_Info();

            //grab the player's info
            //  player_map_name = StardewValley.Game1.player.currentLocation.name;
            //  player_tile_x = StardewValley.Game1.player.getTileX();
            //  player_tile_Y = StardewValley.Game1.player.getTileY();
            //  player_flop = false;

            //   MyWritter_Player(); //write my info to a text file


            //   MyWritter_Horse();

            //   DataLoader_Settings();  //load settings. Prevents acidental overwrite.
            //   MyWritter_Settings(); //save settings. 

            //Game1.warpFarmer(player_map_name, player_tile_x, player_tile_Y, player_flop); //refresh the player's location just incase. That will prove that they character's info was valid.

            //so this is essentially the basics of the code...
            // Log.Error("IS THIS BREAKING?");
        }

        public static void shipping_check()
        {

            if (Game1.activeClickableMenu != null) return;
            if (should_ship == true)
            {
                Game1.activeClickableMenu = new StardewValley.Menus.New_Shipping_Menu(Game1.getFarm().shippingBin);
                should_ship = false;
                Game1.getFarm().shippingBin.Clear();
                Game1.getFarm().lastItemShipped = null;
                passiveSave = true;
            }
            else
            {
                Game1.activeClickableMenu = new StardewValley.Menus.SaveGameMenu();
            }
        }
    }
}
