using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Netcode;
using Omegasis.Revitalize.Framework.Constants;
using Omegasis.Revitalize.Framework.Crafting;
using Omegasis.Revitalize.Framework.Illuminate;
using Omegasis.Revitalize.Framework.Player;
using Omegasis.Revitalize.Framework.Utilities;
using Omegasis.Revitalize.Framework.World.Objects.InformationFiles;
using Omegasis.Revitalize.Framework.World.Objects.Items.Utilities;
using Omegasis.Revitalize.Framework.World.WorldUtilities;
using StardewValley;

namespace Omegasis.Revitalize.Framework.World.Objects.Machines.Misc
{
    [XmlType("Mods_Revitalize.Framework.World.Objects.Machines.Misc.AdvancedCharcoalKiln")]
    public class AdvancedCharcoalKiln : Machine
    {
        public enum KilnTier
        {
            Advanced,
            Delux,
            Superior
        }

        public NetEnum<KilnTier> kilnTier = new NetEnum<KilnTier>(KilnTier.Advanced);
        public NetRef<ItemReference> itemToReceive = new NetRef<ItemReference>();

        public AdvancedCharcoalKiln()
        {

        }

        public AdvancedCharcoalKiln(BasicItemInformation Info, KilnTier KilnTier) : this(Info, Vector2.Zero, KilnTier)
        {

        }

        public AdvancedCharcoalKiln(BasicItemInformation Info, Vector2 TilePosition, KilnTier KilnTier) : base(Info, TilePosition)
        {
            this.kilnTier.Value = KilnTier;
        }

        protected override void initializeNetFieldsPostConstructor()
        {
            base.initializeNetFieldsPostConstructor();
            this.NetFields.AddFields(this.kilnTier);
        }

        public override bool minutesElapsed(int minutes, GameLocation environment)
        {
            base.minutesElapsed(minutes, environment);

            if (this.MinutesUntilReady == 0 && this.itemToReceive.Value != null)
            {
                this.heldObject.Value = (StardewValley.Object)this.itemToReceive.Value.getItem();
                this.itemToReceive.Value = null;
                this.removeLight(Vector2.Zero);
            }
            this.updateAnimation();
            return true;
        }

        /// <summary>
        /// Performed when dropping in an object into the mining drill.
        /// </summary>
        /// <param name="dropInItem"></param>
        /// <param name="probe"></param>
        /// <param name="who"></param>
        /// <returns></returns>
        public override bool performObjectDropInAction(Item dropInItem, bool probe, Farmer who)
        {
            if (probe) return false;
            //Prevent overriding and destroying the previous operation.
            if (this.itemToReceive.Value != null) return false;
            if (this.heldObject.Value != null)
            {
                Game1.player.addItemToInventory(this.heldObject.Value);
                this.heldObject.Value = null;
            }
            bool success = base.performObjectDropInAction(dropInItem, probe, who) && dropInItem.parentSheetIndex == (int)Enums.SDVObject.Wood;
            if (!success) return false;
            this.processInput(dropInItem, who, true);


            return false;
        }

        public override CraftingResult processInput(Item item, Farmer who, bool ShowRedMessage = true)
        {
            if (this.isWorking() || this.finishedProduction()) return new CraftingResult(false);
            if (item.parentSheetIndex != (int)Enums.SDVObject.Wood) return new CraftingResult(false);

            int amountRequired = 0;
            if (this.kilnTier.Value == KilnTier.Advanced)
            {
                amountRequired = 8;
            }
            if (this.kilnTier.Value == KilnTier.Delux)
            {
                amountRequired = 6;
            }
            if (this.kilnTier.Value == KilnTier.Superior)
            {
                amountRequired = 4;
            }

            //Check to make sure the player has enough, otherwise display an error!
            if (amountRequired > item.Stack && ShowRedMessage)
            {
                Game1.showRedMessage(this.getErrorString_NeedMoreInputItems(amountRequired, item));
                return new CraftingResult(false);
            }

            PlayerUtilities.ReduceInventoryItemStackSize(who, item, amountRequired);
            if (who != null)
            {
                SoundUtilities.PlaySound(Enums.StardewSound.openBox);
                SoundUtilities.PlaySoundWithDelay(Enums.StardewSound.fireball, 50);
            }

            MultiplayerUtilities.GetMultiplayer().broadcastSprites(who.currentLocation, new TemporaryAnimatedSprite(27, this.tileLocation.Value * 64f + new Vector2(-16f, -128f), Color.White, 4, flipped: false, 50f, 10, 64, (this.tileLocation.Y + 1f) * 64f / 10000f + 0.0001f)
            {
                alphaFade = 0.005f
            });

            this.MinutesUntilReady = TimeUtilities.GetMinutesFromTime(0, 0, 30);
            this.itemToReceive.Value = new ItemReference(Enums.SDVObject.Coal, 1);
            this.addLight(Vector2.Zero, Illuminate.LightManager.LightIdentifier.SconceLight, Color.DarkCyan.Invert(), 1f);

            this.updateAnimation();
            return new CraftingResult(new ItemReference(item, amountRequired), true);
        }

        public override void updateAnimation()
        {
            if (this.itemToReceive.Value != null)
            {
                this.AnimationManager.playAnimation(Machine.WORKING_ANIMATION_KEY);
            }
            else
            {
                this.AnimationManager.playDefaultAnimation();
            }
        }

        public override Item getOne()
        {
            return new AdvancedCharcoalKiln(this.basicItemInformation.Copy(), this.kilnTier.Value);
        }
    }
}
