using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Network;

namespace NetworkNightmare.Framework
{
    public class CustomMultiplayer : StardewValley.Multiplayer
    {
        public CustomMultiplayer() : base()
        {
            
        }

        public override void processIncomingMessage(IncomingMessage msg)
        {
            base.processIncomingMessage(msg);
        }

        public static bool forceCustomMultiplayer(CustomMultiplayer custom)
        {
            if (custom == null)
            {
                NetworkNightmare.monitor.Log("WELL I GIVE UP");
            }
            // Get a PropertyInfo of specific property type(T).GetProperty(....)
            IReflectedField<Multiplayer> propertyInfo = NetworkNightmare.helper.Reflection.GetField<Multiplayer>(typeof(StardewValley.Game1), "multiplayer",false);
            propertyInfo.SetValue(custom);

            
            return true;
        }
    }
}
