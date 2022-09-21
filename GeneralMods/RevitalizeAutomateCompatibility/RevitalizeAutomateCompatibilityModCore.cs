using System;
using Pathoschild.Stardew.Automate;
using StardewModdingAPI;

namespace Omegasis.RevitalizeAutomateCompatibility
{
    public class RevitalizeAutomateCompatibilityModCore : StardewModdingAPI.Mod
    {
        public override void Entry(IModHelper helper)
        {

            helper.Events.GameLoop.GameLaunched += this.GameLoop_GameLaunched;
        }

        private void GameLoop_GameLaunched(object sender, StardewModdingAPI.Events.GameLaunchedEventArgs e)
        {
            IAutomateAPI automate = this.Helper.ModRegistry.GetApi<IAutomateAPI>("Pathoschild.Automate");
            automate.AddFactory(new RevitalizeAutomationFactory());
        }
    }
}
