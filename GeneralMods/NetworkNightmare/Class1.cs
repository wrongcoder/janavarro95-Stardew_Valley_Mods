using NetworkNightmare.Framework;
using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkNightmare
{
    public class NetworkNightmare : Mod
    {
        public static IModHelper helper;
        public static IMonitor monitor;

        public override void Entry(IModHelper helper)
        {
            helper = this.Helper;
            monitor = this.Monitor;

            StardewModdingAPI.Events.MultiplayerEvents.BeforeMainSync += MultiplayerEvents_BeforeMainSync;

            
        }

        private void MultiplayerEvents_BeforeMainSync(object sender, EventArgs e)
        {
            CustomMultiplayer player = new CustomMultiplayer();
            CustomMultiplayer.forceCustomMultiplayer(player);
        }

        private void GameEvents_FirstUpdateTick(object sender, EventArgs e)
        {
          
        }
    }
}
