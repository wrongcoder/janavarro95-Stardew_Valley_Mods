using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using StardewValley;
using SerializerUtils;
using StardewModdingAPI.Events;

namespace StarDust
{
    public class Core : Mod
    {
        public override void Entry(params object[] objects)
        {
            //GameEvents.GameLoaded += new EventHandler(SerializerUtility.Event_GameLoaded);
            //Command.RegisterCommand("include_types", "Includes types to serialize", (string[])null).CommandFired += new EventHandler<EventArgsCommand>(SerializerUtility.Command_IncludeTypes);
        }



    }
}
