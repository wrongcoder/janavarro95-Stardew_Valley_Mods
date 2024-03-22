using System.Collections.Generic;
using System.Linq;
using Omegasis.AutoSpeed.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Buffs;
using StardewValley.GameData.Buffs;

namespace Omegasis.AutoSpeed
{
    /// <summary>The mod entry point.</summary>
    public class AutoSpeed : Mod
    {
        /*********
        **Static Fields
        *********/
        /// <summary>
        /// All of the speed that is added together for auto speed. This is used for mod authors to hook in their speed boosts before auto speed applies the default speed boost.
        /// </summary>
        public Dictionary<string, float> combinedAddedSpeed;

        /*********
        ** Fields
        *********/
        /// <summary>The mod configuration.</summary>
        private ModConfig Config;

        /// <summary>
        /// A static reference to expose public fields.
        /// </summary>
        public static AutoSpeed Instance;

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
            this.Config = helper.ReadConfig<ModConfig>();
            this.combinedAddedSpeed = new Dictionary<string, float>();
            Instance = this;
        }

        /// <summary>
        /// Returns a copy of the mods' api.
        /// </summary>
        /// <returns></returns>
        public override object GetApi()
        {
            return new AutoSpeedAPI();
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Raised after the game state is updated (≈60 times per second).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (Context.IsPlayerFree)
            {
                float addedSpeed = this.combinedAddedSpeed.Values.Sum() + this.Config.Speed;
                BuffAttributesData buffAttributesData = new StardewValley.GameData.Buffs.BuffAttributesData();
                buffAttributesData.Speed = addedSpeed;
                Game1.player.applyBuff(new Buff("Omegasis.AutoSpeed.Buff", "Omegasis.AutoSpeed.Buff", "Omegasis.AutoSpeed.Buff", Buff.ENDLESS, null, -1, new BuffEffects(buffAttributesData), false, "Omegasis: Auto Speed Buff", "Going fast with the power of mods."));
            }
        }
    }
}
