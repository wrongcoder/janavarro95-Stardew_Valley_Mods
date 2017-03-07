using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;
using StardewModdingAPI;

namespace blah
{
    public class Class1 : Mod
    {
        public override void Entry(IModHelper helper)
        {
            StardewModdingAPI.Events.LocationEvents.CurrentLocationChanged += LocationEvents_CurrentLocationChanged;
        }

        private void LocationEvents_CurrentLocationChanged(object sender, StardewModdingAPI.Events.EventArgsCurrentLocationChanged e)
        {
            Game1.showGlobalMessage("Hey I'm modding");
        }
    }
}
