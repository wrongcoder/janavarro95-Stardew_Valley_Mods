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
using Omegasis.Revitalize.Framework.Utilities.JsonContentLoading;
using Omegasis.Revitalize.Framework.World.Objects.InformationFiles;
using Omegasis.Revitalize.Framework.World.Objects.Items.Utilities;
using Omegasis.Revitalize.Framework.World.WorldUtilities;
using StardewValley;

namespace Omegasis.Revitalize.Framework.World.Objects.Machines.EnergyGeneration
{
    /// <summary>
    ///Object type that takes in a fuel type and converts it into battery packs.
    /// </summary>
    [XmlType("Mods_Revitalize.Framework.World.Objects.Machines.EnergyGeneration.BatteryGenerator")]
    public class BatteryGenerator : Machine
    {
        public enum GeneratorType
        {
            Burner,
            Nuclear
        }

        public NetEnum<GeneratorType> generatorType = new NetEnum<GeneratorType>(GeneratorType.Burner);
        public NetRef<ItemReference> itemToReceive = new NetRef<ItemReference>();

        public BatteryGenerator()
        {

        }

        public BatteryGenerator(BasicItemInformation Info, GeneratorType generatorType) : this(Info, Vector2.Zero, generatorType)
        {

        }

        public BatteryGenerator(BasicItemInformation Info, Vector2 TilePosition, GeneratorType generatorType) : base(Info, TilePosition)
        {
            this.generatorType.Value = generatorType;
        }

        protected override void initializeNetFieldsPostConstructor()
        {
            base.initializeNetFieldsPostConstructor();
            this.NetFields.AddFields(this.generatorType);
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
            //Prevent overriding and destroying the previous operation.
            if (this.itemToReceive.Value != null) return false;
            if (this.heldObject.Value != null)
            {
                Game1.player.addItemToInventory(this.heldObject.Value);
                this.heldObject.Value = null;
            }

            bool success = base.performObjectDropInAction(dropInItem, probe, who) && this.hasCorrectDropInItem(dropInItem);
            if (!success) return false;
            this.processInput(dropInItem, who, true);
            return false;
        }

        public override CraftingResult processInput(Item item, Farmer who, bool ShowRedMessage = true)
        {
            if (this.isWorking() || this.finishedProduction()) return new CraftingResult(false);
            if (!this.hasCorrectDropInItem(item))
            {
                return new CraftingResult(false);
            }

            int amountRequired = 0;
            if (this.generatorType.Value == GeneratorType.Burner)
            {
                amountRequired = 4;
            }
            if (this.generatorType.Value == GeneratorType.Nuclear)
            {
                amountRequired = 1;
            }

            //Check to make sure the player has enough, otherwise display an error!
            if (amountRequired > item.Stack)
            {
                if (ShowRedMessage)
                {
                    Game1.showRedMessage(this.getErrorString_NeedMoreInputItems(amountRequired, item));
                }
                return new CraftingResult(false);
            }

            PlayerUtilities.ReduceInventoryItemStackSize(who, item, amountRequired);
            if (who != null)
            {
                SoundUtilities.PlaySound(Enums.StardewSound.furnace);
            }

            this.MinutesUntilReady = TimeUtilities.GetMinutesFromTime(0, 1, 0);
            this.itemToReceive.Value = this.generatorType.Value== GeneratorType.Burner? new ItemReference(Enums.SDVObject.BatteryPack, 1): new ItemReference(Enums.SDVObject.BatteryPack, 5);
            this.addLight(Vector2.Zero, Illuminate.LightManager.LightIdentifier.SconceLight, this.generatorType.Value== GeneratorType.Burner? Color.DarkCyan.Invert(): Color.GreenYellow, 1f);
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

        /// <summary>
        /// Checks to see if the input item is correct or not.
        /// </summary>
        /// <param name="dropInItem"></param>
        /// <returns></returns>
        public virtual bool hasCorrectDropInItem(Item dropInItem)
        {
            bool correctDropInItem = false;
            if (this.generatorType.Value == GeneratorType.Burner)
            {
                correctDropInItem = dropInItem.parentSheetIndex == (int)Enums.SDVObject.Coal;
            }
            if (this.generatorType.Value == GeneratorType.Nuclear)
            {
                if (dropInItem is IBasicItemInformationProvider)
                {
                    correctDropInItem = (dropInItem as IBasicItemInformationProvider).Id == Constants.Ids.Items.MiscItemIds.RadioactiveFuel;
                }
            }
            return correctDropInItem;
        }

        public override Item getOne()
        {
            return new BatteryGenerator(this.basicItemInformation.Copy(), this.generatorType.Value);
        }
    }
}
