using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using StardewValley;
namespace NoMorePets
{
    public class Class1 :Mod
    {
        bool game_loaded;
        public override void Entry(params object[] objects)
        {
            StardewModdingAPI.Events.GameEvents.UpdateTick += GameEvents_UpdateTick;
            StardewModdingAPI.Events.PlayerEvents.LoadedGame += PlayerEvents_LoadedGame;
        }

        public void PlayerEvents_LoadedGame(object sender, StardewModdingAPI.Events.EventArgsLoadedGameChanged e)
        {
            game_loaded = true;
        }


        public void GameEvents_UpdateTick(object sender, EventArgs e)
        {
            if (game_loaded == false) return;
            List<NPC> my_npc_list = new List<NPC>();
            string pet_name = Game1.player.getPetName();
            if (Game1.player.currentLocation.name == "Farm")
            {
                foreach(NPC npc in Game1.player.currentLocation.characters)
                {
                  
                    if (npc.name == pet_name) my_npc_list.Add(npc);
                }
               foreach(var location in Game1.locations)
                {
                    if (location.name == "Farm" || location.name == "farm") StardewValley.Game1.removeCharacterFromItsLocation(pet_name);
                }
            }
        }
    }
}
