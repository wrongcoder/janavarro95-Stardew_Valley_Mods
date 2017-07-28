using System;
using System.IO;
using StardewModdingAPI;
using StardewValley;

namespace Omegasis.TimeFreeze
{
    public class Class1 :Mod
    {
        string doVanillaCheck; //used to check for bathing in just the default BathHouse
        string doBathingCheck; //used to check if time passes while bathing.
        public override void Entry(IModHelper helper)
        {
            StardewModdingAPI.Events.GameEvents.UpdateTick += GameEvents_UpdateTick;
            StardewModdingAPI.Events.LocationEvents.CurrentLocationChanged += LocationEvents_CurrentLocationChanged;
            DataLoader(); //used to load/write to the config.
        }

        private void LocationEvents_CurrentLocationChanged(object sender, StardewModdingAPI.Events.EventArgsCurrentLocationChanged e)
        {
          //  Game1.showGlobalMessage(Game1.player.currentLocation.name);
        }

        private void GameEvents_UpdateTick(object sender, EventArgs e)
        {
            if (Game1.player == null || Game1.player.currentLocation == null) return;
            //if the player isn't bathing and the location is inside.

            if (doBathingCheck == "True")
            {
                if (doVanillaCheck == "True")
                {

                    if ((Game1.player.swimming == false && (Game1.player.currentLocation) as StardewValley.Locations.BathHousePool == null) && Game1.player.currentLocation.isOutdoors == false)
                    {
                        if (!doesTimePassHere())
                        {
                            Game1.gameTimeInterval = 0;
                        }
                    }
                }
                else
                {
                    if (Game1.player.swimming == false && Game1.player.currentLocation.isOutdoors == false)
                    {
                        if (!doesTimePassHere())
                        {
                            Game1.gameTimeInterval = 0;
                        }
                    }
                }
            }
            else
            {
                if (Game1.player.currentLocation.isOutdoors == false)
                {
                    if (!doesTimePassHere())
                    {
                        Game1.gameTimeInterval = 0;
                    }
                }
            }
        }


        public bool doesTimePassHere()
        {
            if (Game1.player.currentLocation.name == "Mine" || Game1.player.currentLocation.name == "SkullCave" || Game1.player.currentLocation.name=="UndergroundMine") return true;
            return false;
        }

        void MyWritter()
        {
            string mylocation = Path.Combine(Helper.DirectoryPath, "ModConfig.txt");
            string[] mystring3 = new string[6];
            if (!File.Exists(mylocation))
            {
                mystring3[0] = "Player: TimeFreeze Config";
                mystring3[1] = "====================================================================================";
                mystring3[2] = "Unfreeze time in only vanilla bathhouse: True means that time will pass when the player is bathing when in the bathhouse. False means that time will pass when the player is swimming indoors. Use this to rebalance some custom maps.";
                mystring3[3] = doVanillaCheck.ToString();
                mystring3[4] = "Does time pass while bathing? True means yes, no means that time is still frozen while bathing indoors.";
                mystring3[5] = doVanillaCheck.ToString();
                File.WriteAllLines(mylocation, mystring3);
            }
            else
            {
                mystring3[0] = "Player: TimeFreeze Config";
                mystring3[1] = "====================================================================================";
                mystring3[2] = "Unfreeze time in only vanilla bathhouse: True means that time will pass when the player is bathing when in the bathhouse. False means that time will pass when the player is swimming indoors. Use this to rebalance some custom maps.";
                mystring3[3] = "True";
                mystring3[4] = "Does time pass while bathing? True means yes, no means that time is still frozen while bathing indoors.";
                mystring3[5] = doVanillaCheck.ToString();
                File.WriteAllLines(mylocation, mystring3);
            }
        }
        void DataLoader()
        {
            string mylocation = Path.Combine(Helper.DirectoryPath, "ModConfig.txt");
            if (!File.Exists(mylocation)) 
            {
                doVanillaCheck = "True";
                doBathingCheck = "True";
                MyWritter();
            }
            else
            {
                string[] readtext = File.ReadAllLines(mylocation);
                doVanillaCheck = readtext[3];
                doBathingCheck=readtext[5];
            }
        }
    }
}
