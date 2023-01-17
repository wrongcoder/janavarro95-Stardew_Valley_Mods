using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using Omegasis.Revitalize.Framework.Constants;
using Omegasis.Revitalize.Framework.Crafting;
using Omegasis.Revitalize.Framework.Crafting.JsonContent;
using Omegasis.Revitalize.Framework.Player;
using Omegasis.Revitalize.Framework.Utilities;
using Omegasis.Revitalize.Framework.Utilities.Extensions;
using Omegasis.Revitalize.Framework.Utilities.Ranges;
using Omegasis.Revitalize.Framework.World.Objects.InformationFiles;
using Omegasis.Revitalize.Framework.World.Objects.Items.Utilities;
using Omegasis.Revitalize.Framework.World.WorldUtilities;
using Omegasis.Revitalize.Framework.World.WorldUtilities.Items;
using StardewValley;
using StardewValley.Locations;

namespace Omegasis.Revitalize.Framework.World.Objects.Machines.Misc
{
    [XmlType("Mods_Revitalize.Framework.World.Objects.Machines.Misc.AdvancedGeodeCrusher")]
    public class AdvancedGeodeCrusher : PoweredMachine
    {

        public AdvancedGeodeCrusher()
        {

        }

        public AdvancedGeodeCrusher(BasicItemInformation Info, PoweredMachineTier machineTier) : this(Info, Vector2.Zero, machineTier)
        {

        }

        public AdvancedGeodeCrusher(BasicItemInformation Info, Vector2 TilePosition, PoweredMachineTier machineTier) : base(Info, TilePosition, machineTier)
        {
        }

        public override CraftingResult onSuccessfulRecipeFound(Item dropInItem, ProcessingRecipe<LootTableEntry> craftingRecipe, Farmer who = null)
        {
            CraftingResult result= base.onSuccessfulRecipeFound(dropInItem, craftingRecipe, who);
            if(result.successful)
            {
                if (who != null)
                {
                    Utility.addSmokePuff(who.currentLocation, this.TileLocation * 64f + new Vector2(4f, -48f), 200);
                    Utility.addSmokePuff(who.currentLocation, this.TileLocation * 64f + new Vector2(-16f, -56f), 300);
                    Utility.addSmokePuff(who.currentLocation, this.TileLocation * 64f + new Vector2(16f, -52f), 400);
                    Utility.addSmokePuff(who.currentLocation, this.TileLocation * 64f + new Vector2(32f, -56f), 200);
                    Utility.addSmokePuff(who.currentLocation, this.TileLocation * 64f + new Vector2(40f, -44f), 500);
                }
            }
            return result;
        }

        public override void playDropInSound()
        {
            SoundUtilities.PlaySound(Enums.StardewSound.drumkit4);
            SoundUtilities.PlaySound(Enums.StardewSound.stoneCrack);
            SoundUtilities.PlaySoundWithDelay(Enums.StardewSound.steam, 200);
        }

        public override int getElectricFuelChargeIncreaseAmount()
        {
            return 5;
        }

        public override int getNuclearFuelChargeIncreaseAmount()
        {
            return 25;
        }


        public override Item getOne()
        {
            return new AdvancedGeodeCrusher(this.basicItemInformation.Copy(), this.machineTier.Value);
        }
    }
}
