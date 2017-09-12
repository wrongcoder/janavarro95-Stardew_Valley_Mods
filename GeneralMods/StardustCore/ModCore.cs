using StardewModdingAPI;
using StardustCore.ModInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardustCore
{
    public class ModCore : Mod
    {
        public static IModHelper ModHelper;
        public static IMonitor ModMonitor;
        public override void Entry(IModHelper helper)
        {
            ModHelper = helper;
            ModMonitor = this.Monitor;

            StardewModdingAPI.Events.GraphicsEvents.OnPostRenderGuiEvent += Metadata.GameEvents_UpdateTick;
        }
    }
}
