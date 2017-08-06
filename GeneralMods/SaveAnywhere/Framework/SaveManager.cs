using System;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Menus;
using StardewValley.Monsters;
using SFarmer = StardewValley.Farmer;

namespace Omegasis.SaveAnywhere.Framework
{
    /// <summary>Provides methods for saving and loading game data.</summary>
    internal class SaveManager
    {
        /*********
        ** Properties
        *********/
        /// <summary>The player for which to save data.</summary>
        private readonly SFarmer Player;

        /// <summary>Simplifies access to game code.</summary>
        private readonly IReflectionHelper Reflection;

        /// <summary>Writes messages to the console and log file.</summary>
        private readonly IMonitor Monitor;

        /// <summary>A callback invoked when villagers are reset during a load.</summary>
        private readonly Action OnVillagersReset;

        /// <summary>The full path to the folder in which to store data for this player.</summary>
        private readonly string SavePath;

        /// <summary>The full path to the folder in which to store animal data for this player.</summary>
        private readonly string SaveAnimalsPath;

        /// <summary>The full path to the folder in which to store villager data for this player.</summary>
        private readonly string SaveVillagersPath;

        /// <summary>Whether we should save at the next opportunity.</summary>
        private bool WaitingToSave;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="player">The player for which to save data.</param>
        /// <param name="modPath">The full path to the mod folder.</param>
        /// <param name="monitor">Writes messages to the console and log file.</param>
        /// <param name="reflection">Simplifies access to game code.</param>
        /// <param name="onVillagersReset">A callback invoked when villagers are reset during a load.</param>
        public SaveManager(SFarmer player, string modPath, IMonitor monitor, IReflectionHelper reflection, Action onVillagersReset)
        {
            // save info
            this.Player = player;
            this.Monitor = monitor;
            this.Reflection = reflection;
            this.OnVillagersReset = onVillagersReset;

            // generate paths
            this.SavePath = Path.Combine(modPath, "Save_Data", player.name);
            this.SaveAnimalsPath = Path.Combine(this.SavePath, "Animals");
            this.SaveVillagersPath = Path.Combine(this.SavePath, "NPC_Save_Info");
        }

        /// <summary>Perform any required update logic.</summary>
        public void Update()
        {
            // perform passive save
            if (this.WaitingToSave && Game1.activeClickableMenu == null)
            {
                Game1.activeClickableMenu = new NewSaveGameMenu();
                this.WaitingToSave = false;
            }
        }

        /// <summary>Clear saved data.</summary>
        public void ClearData()
        {
            Directory.Delete(this.SavePath, recursive: true);
        }

        /// <summary>Save all game data.</summary>
        public void SaveGameAndPositions()
        {
            // save game data
            Farm farm = Game1.getFarm();
            if (farm.shippingBin.Any())
            {
                Game1.activeClickableMenu = new NewShippingMenu(farm.shippingBin, this.Reflection);
                farm.shippingBin.Clear();
                farm.lastItemShipped = null;
                this.WaitingToSave = true;
            }
            else
                Game1.activeClickableMenu = new NewSaveGameMenu();

            // save custom data
            Directory.CreateDirectory(this.SaveAnimalsPath);
            Directory.CreateDirectory(this.SaveVillagersPath);
            this.SavePlayerPosition();
            this.SaveHorsePosition();
            this.SavePetPosition();
            this.SaveVillagerPositions();
        }

        /// <summary>Load all game data.</summary>
        public void LoadPositions()
        {
            if (!this.HasSaveData())
                return;

            this.LoadPlayerPosition();
            this.LoadHorsePosition();
            this.LoadPetPosition();
            bool anyVillagersMoved = this.LoadVillagerPositions();

            if (anyVillagersMoved)
                this.OnVillagersReset?.Invoke();
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Save the horse state to the save file.</summary>
        private void SaveHorsePosition()
        {
            // find horse
            Horse horse = Utility.findHorse();
            if (horse == null)
                return;

            // get horse info
            string map = horse.currentLocation.name;
            Point tile = horse.getTileLocationPoint();

            // save data
            string path = Path.Combine(this.SaveAnimalsPath, $"Horse_Save_Info_{this.Player.name}.txt");
            string[] text = new string[20];
            text[0] = "Horse: Save_Anywhere Info. Editing this might break some things.";
            text[1] = "====================================================================================";

            text[2] = "Horse Current Map Name";
            text[3] = map;

            text[4] = "Horse X Position";
            text[5] = tile.X.ToString();

            text[6] = "Horse Y Position";
            text[7] = tile.Y.ToString();

            File.WriteAllLines(path, text);
        }

        /// <summary>Reset the horse to the saved state.</summary>
        private void LoadHorsePosition()
        {
            // find horse
            Horse horse = Utility.findHorse();
            if (horse == null)
                return;

            // get file path
            string path = Path.Combine(this.SaveAnimalsPath, $"Horse_Save_Info_{this.Player.name}.txt");
            if (!File.Exists(path))
                return;

            // read saved data
            string[] text = File.ReadAllLines(path);
            string map = Convert.ToString(text[3]);
            int x = Convert.ToInt32(text[5]);
            int y = Convert.ToInt32(text[7]);

            // update horse
            Game1.warpCharacter(horse, map, new Point(x, y), false, true);
        }

        /// <summary>Save the villager states to the save file.</summary>
        private void SaveVillagerPositions()
        {
            foreach (NPC npc in Utility.getAllCharacters())
            {
                // ignore non-villagers
                if (npc is Pet || npc is Monster)
                    continue;

                // get NPC data
                string name = npc.name;
                string map = npc.currentLocation.name;
                Point tile = npc.getTileLocationPoint();

                // save data
                string path = Path.Combine(this.SaveVillagersPath, npc.name + ".txt");
                string[] text = new string[20];
                text[0] = "NPC: Save_Anywhere Info. Editing this might break some things.";
                text[1] = "====================================================================================";

                text[2] = "NPC Name";
                text[3] = name;

                text[4] = "NPC Current Map Name";
                text[5] = map;

                text[6] = "NPC X Position";
                text[7] = tile.X.ToString();

                text[8] = "NPC Y Position";
                text[9] = tile.Y.ToString();

                File.WriteAllLines(path, text);
            }
        }

        /// <summary>Reset the villagers to their saved state.</summary>
        /// <returns>Returns whether any villagers changed position.</returns>
        private bool LoadVillagerPositions()
        {
            bool anyLoaded = false;
            foreach (NPC npc in Utility.getAllCharacters())
            {
                // ignore non-villagers
                if (npc is Pet || npc is Monster)
                    continue;

                // get file path
                string path = Path.Combine(this.SaveVillagersPath, npc.name + ".txt");
                if (!File.Exists(path))
                {
                    this.Monitor.Log($"No save data for {npc.name} villager, skipping.", LogLevel.Error);
                    continue;
                }

                // read data
                string[] text = File.ReadAllLines(path);
                string map = Convert.ToString(text[5]);
                int x = Convert.ToInt32(text[7]);
                int y = Convert.ToInt32(text[9]);
                if (string.IsNullOrEmpty(map))
                    continue;

                // update NPC
                anyLoaded = true;
                Game1.warpCharacter(npc, map, new Point(x, y), false, true);
            }

            return anyLoaded;
        }

        /// <summary>Save the pet state to the save file.</summary>
        private void SavePetPosition()
        {
            if (!this.Player.hasPet())
                return;

            // find pet
            Pet pet = Utility.getAllCharacters().OfType<Pet>().FirstOrDefault();
            if (pet == null)
                return;

            // get pet info
            string map = pet.currentLocation.name;
            Point tile = pet.getTileLocationPoint();

            // save data
            string path = Path.Combine(this.SaveAnimalsPath, $"Pet_Save_Info_{this.Player.name}.txt");
            string[] text = new string[20];
            text[0] = "Pet: Save_Anywhere Info. Editing this might break some things.";
            text[1] = "====================================================================================";

            text[2] = "Pet Current Map Name";
            text[3] = map;

            text[4] = "Pet X Position";
            text[5] = tile.X.ToString();

            text[6] = "Pet Y Position";
            text[7] = tile.Y.ToString();

            File.WriteAllLines(path, text);
        }

        /// <summary>Reset the pet to the saved state.</summary>
        private void LoadPetPosition()
        {
            if (!this.Player.hasPet())
                return;

            // find pet
            Pet pet = Utility.getAllCharacters().OfType<Pet>().FirstOrDefault();
            if (pet == null)
                return;

            // get file path
            string path = Path.Combine(this.SaveAnimalsPath, $"Pet_Save_Info_{this.Player.name}.txt");
            if (!File.Exists(path))
                return;

            // read saved data
            string[] text = File.ReadAllLines(path);
            string map = Convert.ToString(text[3]);
            int x = Convert.ToInt32(text[5]);
            int y = Convert.ToInt32(text[7]);

            // update pet
            Game1.warpCharacter(pet, map, new Point(x, y), false, true);
        }

        /// <summary>Save the player state to the save file.</summary>
        private void SavePlayerPosition()
        {
            // get player info
            string map = this.Player.currentLocation.name;
            Point tile = this.Player.getTileLocationPoint();

            // save data
            string path = Path.Combine(this.SavePath, $"Player_Save_Info_{this.Player.name}.txt");
            string[] text = new string[20];

            text[0] = "Player: Save_Anywhere Info. Editing this might break some things.";
            text[1] = "====================================================================================";

            text[2] = "Player Current Game Time";
            text[3] = Game1.timeOfDay.ToString();

            text[4] = "Player Current Map Name";
            text[5] = map;

            text[6] = "Player X Position";
            text[7] = tile.X.ToString();

            text[8] = "Player Y Position";
            text[9] = tile.Y.ToString();

            File.WriteAllLines(path, text);
        }

        /// <summary>Reset the player to the saved state.</summary>
        private void LoadPlayerPosition()
        {
            // get file path
            string path = Path.Combine(this.SavePath, $"Player_Save_Info_{this.Player.name}.txt");
            if (!File.Exists(path))
                return;

            // read saved data
            string[] text = File.ReadAllLines(path);
            int time = Convert.ToInt32(text[3]);
            string map = Convert.ToString(text[5]);
            int x = Convert.ToInt32(text[7]);
            int y = Convert.ToInt32(text[9]);

            // update player
            Game1.timeOfDay = time;

            this.Player.previousLocationName = this.Player.currentLocation.name;
            Game1.locationAfterWarp = Game1.getLocationFromName(map);
            Game1.xLocationAfterWarp = x;
            Game1.yLocationAfterWarp = y;
            //Game1.facingDirectionAfterWarp = this.player_facing_direction;
            Game1.fadeScreenToBlack();
            Game1.warpFarmer(map, x, y, false);
            //this.Player.faceDirection(this.player_facing_direction);
        }

        /// <summary>Get whether any data has been saved for this player yet.</summary>
        private bool HasSaveData()
        {
            return Directory.Exists(this.SavePath);
        }
    }
}
