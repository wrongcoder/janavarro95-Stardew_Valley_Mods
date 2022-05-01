using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Netcode;
using Omegasis.Revitalize.Framework.Constants;
using Omegasis.Revitalize.Framework.World.Objects.InformationFiles;
using Omegasis.Revitalize.Framework.World.Objects.SupportClasses;
using Omegasis.Revitalize.Framework.World.WorldUtilities;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Objects;

namespace Omegasis.Revitalize.Framework.World.Objects.Farming
{
    //TODO: CREATE OBJECT GRAPHIC OBJECT IN OBJECTMANAGER, AND FINISH PLANTING LOGIC.
    [XmlType("Mods_Revitalize.Framework.World.Objects.Farming.AdvancedFarmingSystem")]
    /// <summary>
    /// Plants seeds, fertilizes, and harvests crops from irrigated watering pots that have the proper attachments!
    /// 
    /// </summary>
    public class AdvancedFarmingSystem : CustomObject
    {

        //public readonly NetRef<ChestFunctionality> inventory = new NetRef<ChestFunctionality>();

        public AdvancedFarmingSystem()
        {

        }

        public AdvancedFarmingSystem(BasicItemInformation Info) : base(Info)
        {
        }

        public AdvancedFarmingSystem(BasicItemInformation Info, Vector2 TilePosition) : base(Info, TilePosition)
        {
        }

        /// <summary>
        /// When the chair is right clicked ensure that all pieces associated with it are also rotated.
        /// </summary>
        /// <param name="who"></param>
        /// <returns></returns>
        public override bool rightClicked(Farmer who)
        {
            this.doWorkOnIrrigatedWaterPots();
            SoundUtilities.PlaySound(Enums.StardewSound.Ship);
            return true;
        }

        public override void DayUpdate(GameLocation location)
        {
            base.DayUpdate(location);
        }

        public virtual void doWorkOnIrrigatedWaterPots()
        {
            Dictionary<Vector2, StardewValley.Object> connectedObjects = WorldUtilities.ObjectUtilities.GetAllConnectedObjectsStartingAtTilePosition(this.getCurrentLocation(),this.TileLocation, true);


            List<IrrigatedGardenPot> gardenPots = new List<IrrigatedGardenPot>();
            List<Chest> chests = new List<Chest>();

            foreach(KeyValuePair<Vector2,StardewValley.Object> tileToObject in connectedObjects)
            {

                //Filter out unnecessary items.
                if(tileToObject.Value is Chest)
                {
                    chests.Add((Chest)tileToObject.Value);
                }

                if(tileToObject.Value is IrrigatedGardenPot)
                {
                    gardenPots.Add((IrrigatedGardenPot)tileToObject.Value);
                }


            }

            //This will only output to chests, it is up to the player to decide what to do from there, or if Automate is installed, then Automate will take over with it's processing system.
            foreach(IrrigatedGardenPot gardenPot in gardenPots)
            {

                foreach (Chest chest in chests)
                {
                    if (gardenPot.hasAutoHarvestAttachment)
                    {
                        //find the first chest that has an inventory that can accept this crop and then break.
                        //Will probably need to adjust my code that adds items to the player's inventory to do the same with other inventories.
                    }
                    if (gardenPot.hasEnricherAttachment)
                    {
                        //find the first chest with fertilizer and use here only if crop is null, or seeds are in the first stage.
                    }
                    if (gardenPot.hasPlanterAttachment)
                    {
                        //ALSO MAKE SURE THE CROP IS NULL.
                        //find first chest with seeds and plant here.
                    }



                }

            }



        }


        public override Item getOne()
        {
            AdvancedFarmingSystem component = new AdvancedFarmingSystem(this.getItemInformation().Copy());
            return component;
        }
    }
}

