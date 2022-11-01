using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using Omegasis.Revitalize.Framework.Constants;
using Omegasis.Revitalize.Framework.World.Objects.Crafting;
using Omegasis.Revitalize.Framework.Constants.ItemIds.Items;
using Omegasis.Revitalize.Framework.Constants.CraftingIds;
using Omegasis.Revitalize.Framework.Constants.ItemIds.Objects;
using Omegasis.Revitalize.Framework.World.Objects.Interfaces;
using Omegasis.Revitalize.Framework.World.Objects.Machines.EnergyGeneration;
using Omegasis.Revitalize.Framework.World.Objects.Machines;
using Omegasis.Revitalize.Framework.World.Objects.InformationFiles;
using Omegasis.Revitalize.Framework.Utilities;
using Omegasis.StardustCore.UIUtilities;
using Omegasis.StardustCore.Animations;
using Omegasis.Revitalize.Framework.Managers;
using Omegasis.Revitalize.Framework.World.Objects.Farming;
using Omegasis.Revitalize.Framework.World.Objects.Items.Farming;
using Omegasis.Revitalize.Framework.World.Objects.Machines.Furnaces;
using Omegasis.Revitalize.Framework.World.Objects.Resources;
using Omegasis.Revitalize.Framework.Constants.ItemCategoryInformation;
using Omegasis.Revitalize.Framework.Constants.PathConstants.Data;
using System.IO;
using Omegasis.Revitalize.Framework.World.Objects.InformationFiles.Json.Crafting;
using Omegasis.Revitalize.Framework.World.Objects.Items.Utilities;
using Omegasis.Revitalize.Framework.Content.JsonContent.Objects;
using Omegasis.Revitalize.Framework.World.Objects.Machines.ResourceGeneration;

namespace Omegasis.Revitalize.Framework.World.Objects
{
    /// <summary>
    /// Deals with handling all objects for the mod.
    /// </summary>
    public class ObjectManager
    {
        /// <summary>
        /// All of the object managers id'd by a mod's or content pack's unique id.
        /// </summary>
        public static Dictionary<string, ObjectManager> ObjectPools;


        /// <summary>
        /// The name of this object manager.
        /// </summary>
        public string name;

        public ResourceManager resources;

        /// <summary>
        /// The list of registered items for this object manager.
        /// </summary>
        public Dictionary<string, Item> itemsById;

        /// <summary>
        /// Display strings for all loaded items.
        /// </summary>
        public Dictionary<string, IdToDisplayStrings> displayStrings = new Dictionary<string, IdToDisplayStrings>();

        /// <summary>
        /// Constructor.
        /// </summary>
        public ObjectManager()
        {
            this.initialize();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="manifest"></param>
        public ObjectManager(IManifest manifest)
        {
            this.name = manifest.UniqueID;
            this.initialize();
        }

        /// <summary>
        /// Initialize all objects used to manage this class.
        /// </summary>
        private void initialize()
        {

            this.resources = new ResourceManager();
            this.itemsById = new Dictionary<string, Item>();

            //Load in furniture again!
        }

        /// <summary>
        /// Loads in the items for the object and resource managers.
        /// </summary>
        public void loadItemsFromDisk()
        {
            this.registerStardewValleyItems();
            this.loadInDisplayStrings();

            this.resources.loadInItems(); //Should take priority over other modded content.

            this.loadInItems();
            this.loadInCraftingTables();
            this.loadInMachines();
            this.loadInAestheticsObjects();

            this.loadInResourcePlants();

            //Should load blueprints last due to the fact that they can draw references to objects.
            this.loadInBlueprints();
        }

        /// <summary>
        /// Loads in all display strings from a given .json dictionary file.
        /// </summary>
        protected virtual void loadInDisplayStrings()
        {
            List<Dictionary<string, IdToDisplayStrings>> displayStringInfo = JsonUtilities.LoadJsonFilesFromDirectories<Dictionary<string, IdToDisplayStrings>>(Constants.PathConstants.StringsPaths.DisplayStrings);
            foreach (Dictionary<string, IdToDisplayStrings> dict in displayStringInfo)
            {
                foreach (KeyValuePair<string, IdToDisplayStrings> pair in dict)
                {
                    if (this.displayStrings.ContainsKey(pair.Key))
                    {
                        continue;
                    }
                    this.displayStrings.Add(pair.Key, pair.Value);
                }
            }
        }

        /// <summary>
        /// Used to register items in the object managaer so that they can be used for the ObjectManager and for referencing things such as crafting recipes.
        /// </summary>
        protected virtual void registerStardewValleyItems()
        {
            this.addItem("StardewValley.Tools.Pickaxe", new StardewValley.Tools.Pickaxe());
            this.addItem("StardewValley.Tools.Axe", new StardewValley.Tools.Axe());
            this.addItem("StardewValley.Tools.WateringCan", new StardewValley.Tools.WateringCan());
            this.addItem("StardewValley.Tools.Hoe", new StardewValley.Tools.Hoe());
            this.addItem("StardewValley.Tools.CopperPickaxe", new StardewValley.Tools.Pickaxe() { UpgradeLevel = Tool.copper });
            this.addItem("StardewValley.Tools.CopperAxe", new StardewValley.Tools.Axe() { UpgradeLevel = Tool.copper });
            this.addItem("StardewValley.Tools.CopperWateringCan", new StardewValley.Tools.WateringCan() { UpgradeLevel = Tool.copper });
            this.addItem("StardewValley.Tools.CopperHoe", new StardewValley.Tools.Hoe() { UpgradeLevel = Tool.copper });
            this.addItem("StardewValley.Tools.SteelPickaxe", new StardewValley.Tools.Pickaxe() { UpgradeLevel = Tool.steel });
            this.addItem("StardewValley.Tools.SteelAxe", new StardewValley.Tools.Axe() { UpgradeLevel = Tool.steel });
            this.addItem("StardewValley.Tools.SteelWateringCan", new StardewValley.Tools.WateringCan() { UpgradeLevel = Tool.steel });
            this.addItem("StardewValley.Tools.SteelHoe", new StardewValley.Tools.Hoe() { UpgradeLevel = Tool.steel });
            this.addItem("StardewValley.Tools.GoldPickaxe", new StardewValley.Tools.Pickaxe() { UpgradeLevel = Tool.gold });
            this.addItem("StardewValley.Tools.GoldAxe", new StardewValley.Tools.Axe() { UpgradeLevel = Tool.gold });
            this.addItem("StardewValley.Tools.GoldWateringCan", new StardewValley.Tools.WateringCan() { UpgradeLevel = Tool.gold });
            this.addItem("StardewValley.Tools.GoldHoe", new StardewValley.Tools.Hoe() { UpgradeLevel = Tool.gold });
            this.addItem("StardewValley.Tools.IridiumPickaxe", new StardewValley.Tools.Pickaxe() { UpgradeLevel = Tool.iridium });
            this.addItem("StardewValley.Tools.IridiumAxe", new StardewValley.Tools.Axe() { UpgradeLevel = Tool.iridium });
            this.addItem("StardewValley.Tools.IridiumWateringCan", new StardewValley.Tools.WateringCan() { UpgradeLevel = Tool.iridium });
            this.addItem("StardewValley.Tools.IridiumHoe", new StardewValley.Tools.Hoe() { UpgradeLevel = Tool.iridium });
        }

        private void loadInAestheticsObjects()
        {
        }

        private void loadInResourcePlants()
        {
            ResourceBush coalBush = new ResourceBush(new BasicItemInformation("", ResourceObjectIds.CoalBush, "", CategoryNames.Resource, CategoryColors.Misc, -300, -300, 0, false, 5000, false, false, TextureManagers.Objects_Resources_ResourcePlants.createAnimationManager("CoalBush", new Animation(0, 0, 16, 32)), Color.White, false, new Vector2(1, 1), new Vector2(0, -1), null, null), this.getObject(Enums.SDVObject.Coal), 1);
            this.addItem(ResourceObjectIds.CoalBush, coalBush);

            ResourceBush copperOreBush = new ResourceBush(new BasicItemInformation("", ResourceObjectIds.CopperOreBush, "", CategoryNames.Resource, CategoryColors.Misc, -300, -300, 0, false, 2000, false, false, TextureManagers.Objects_Resources_ResourcePlants.createAnimationManager("CopperOreBush", new Animation(0, 0, 16, 32)), Color.White, false, new Vector2(1, 1), new Vector2(0, -1), null, null), this.getObject(Enums.SDVObject.CopperOre), 1);
            this.addItem(ResourceObjectIds.CopperOreBush, copperOreBush);

            ResourceBush ironOreBush = new ResourceBush(new BasicItemInformation("", ResourceObjectIds.IronOreBush, "", CategoryNames.Resource, CategoryColors.Misc, -300, -300, 0, false, 5000, false, false, TextureManagers.Objects_Resources_ResourcePlants.createAnimationManager("IronOreBush", new Animation(0, 0, 16, 32)), Color.White, false, new Vector2(1, 1), new Vector2(0, -1), null, null), this.getObject(Enums.SDVObject.IronOre), 2);
            this.addItem(ResourceObjectIds.IronOreBush, ironOreBush);

            ResourceBush goldOreBush = new ResourceBush(new BasicItemInformation("", ResourceObjectIds.GoldOreBush, "", CategoryNames.Resource, CategoryColors.Misc, -300, -300, 0, false, 10000, false, false, TextureManagers.Objects_Resources_ResourcePlants.createAnimationManager("GoldOreBush", new Animation(0, 0, 16, 32)), Color.White, false, new Vector2(1, 1), new Vector2(0, -1), null, null), this.getObject(Enums.SDVObject.GoldOre), 2);
            this.addItem(ResourceObjectIds.GoldOreBush, goldOreBush);

            ResourceBush iridiumResourceBush = new ResourceBush(new BasicItemInformation("", ResourceObjectIds.IridiumOreBush, "", CategoryNames.Resource, CategoryColors.Misc, -300, -300, 0, false, 25000, false, false, TextureManagers.Objects_Resources_ResourcePlants.createAnimationManager("IridiumOreBush", new Animation(0, 0, 16, 32)), Color.White, false, new Vector2(1, 1), new Vector2(0, -1), null, null), this.getObject(Enums.SDVObject.IridiumOre), 3);
            this.addItem(ResourceObjectIds.IridiumOreBush, iridiumResourceBush);

            ResourceBush radioactiveOreBush = new ResourceBush(new BasicItemInformation("", ResourceObjectIds.RadioactiveOreBush, "", CategoryNames.Resource, CategoryColors.Misc, -300, -300, 0, false, 50000, false, false, TextureManagers.Objects_Resources_ResourcePlants.createAnimationManager("RadioactiveOreBush", new Animation(0, 0, 16, 32)), Color.White, false, new Vector2(1, 1), new Vector2(0, -1), null, null), this.getObject(Enums.SDVObject.RadioactiveOre), 3);
            this.addItem(ResourceObjectIds.RadioactiveOreBush, radioactiveOreBush);
        }

        /// <summary>
        /// Loads in the item for the object manager.
        /// </summary>
        private void loadInItems()
        {

            AutoPlanterGardenPotAttachment autoPlanterGardenPotAttachment = new AutoPlanterGardenPotAttachment(new BasicItemInformation("", FarmingItems.AutoPlanterGardenPotAttachment, "", CategoryNames.Farming, CategoryColors.Farming, -300, -300, 0, false, 5000, false, false, TextureManagers.Items_Farming.createAnimationManager("AutoPlanterGardenPotAttachment", new Animation(0, 0, 16, 32)), Color.White, false, new Vector2(1, 1), Vector2.Zero, null, null));
            this.addItem(FarmingItems.AutoPlanterGardenPotAttachment, autoPlanterGardenPotAttachment);

            AutoHarvesterGardenPotAttachment autoHarvesterGardenPotAttachment = new AutoHarvesterGardenPotAttachment(new BasicItemInformation("", FarmingItems.AutoHarvesterGardenPotAttachment, "", CategoryNames.Farming, CategoryColors.Farming, -300, -300, 0, false, 5000, false, false, TextureManagers.Items_Farming.createAnimationManager("AutoHarvesterGardenPotAttachment", new Animation(0, 0, 16, 16)), Color.White, false, new Vector2(1, 1), Vector2.Zero, null, null));
            this.addItem(FarmingItems.AutoHarvesterGardenPotAttachment, autoHarvesterGardenPotAttachment);

            this.addItem(MiscItemIds.RadioactiveFuel, new CustomItem(new BasicItemInformation("", MiscItemIds.RadioactiveFuel, "", CategoryNames.Misc, CategoryColors.Misc, -300, -300, 0, false, 5000, false, false, TextureManagers.Items_Misc.createAnimationManager("RadioactiveFuel", new Animation(0, 0, 16, 16)), Color.White, false, new Vector2(1, 1), Vector2.Zero, null, null)));

            this.addItem(FarmingItems.RefillSilosFakeItem, new CustomItem(new BasicItemInformation("", FarmingItems.RefillSilosFakeItem, "", CategoryNames.Farming, CategoryColors.Farming, -300, -300, StardewValley.Object.fragility_Removable, false, 100, false, false, TextureManagers.Items_Farming.createAnimationManager("HayRefill", new Animation(0, 0, 16, 16)), Color.White, false, new Vector2(1, 1), Vector2.Zero, null, null)));
        }

        private void loadInBlueprints()
        {

            //Make sure that all blueprints registered here have a id reference in Blueprints.cs for easier access via code.
            foreach (JsonCraftingBlueprint jsonBlueprint in JsonUtilities.LoadJsonFilesFromDirectories<JsonCraftingBlueprint>(ObjectsDataPaths.CraftingBlueprintsPath))
            {
                this.addItem(jsonBlueprint.id, jsonBlueprint.toBlueprint());
            }

        }

        private void loadInCraftingTables()
        {
            CraftingTable WorkStationObject = new CraftingTable(new BasicItemInformation("", CraftingStations.WorkBench_Id, "", CategoryNames.Crafting, Color.Brown, -300, -300, 0, false, 500, true, true, TextureManagers.Objects_Crafting.createAnimationManager("Workbench", new Animation(0, 0, 32, 32)), Color.White, false, new Vector2(2, 2), Vector2.Zero, null, null), CraftingRecipeBooks.WorkbenchCraftingRecipies);
            CraftingTable AnvilObj = new CraftingTable(new BasicItemInformation("", CraftingStations.Anvil_Id, "", CategoryNames.Crafting, Color.Brown, -300, -300, 0, false, 2000, true, true, TextureManagers.Objects_Crafting.createAnimationManager("Anvil", new Animation(0, 0, 32, 32)), Color.White, false, new Vector2(2, 2), Vector2.Zero, null, null), CraftingRecipeBooks.AnvilCraftingRecipes);

            this.addItem(CraftingStations.WorkBench_Id, WorkStationObject);
            this.addItem(CraftingStations.Anvil_Id, AnvilObj);
        }

        private void loadInMachines()
        {

            AdvancedSolarPanel solarP1 = new AdvancedSolarPanel(new BasicItemInformation("", MachineIds.AdvancedSolarPanelV1, "", CategoryNames.Machine, Color.SteelBlue, -300, -300, 0, false, 1000, true, true, TextureManagers.Objects_Machines.createAnimationManager("SolarPanelTier1", new Animation(0, 0, 16, 16)), Color.White, false, new Vector2(1, 1), Vector2.Zero, null, null));

            this.addItem(MachineIds.AdvancedSolarPanelV1, solarP1);

            
            MiningDrill electricMiningDrill = new MiningDrill(new BasicItemInformation("", MachineIds.ElectricMiningDrill, "", CategoryNames.Machine, Color.SteelBlue, -300, -300, 0, false, 4000, true, true, TextureManagers.Objects_Machines.createAnimationManager("ElectricMiningDrill", new SerializableDictionary<string, Animation>() {
                {"Default",new Animation(new AnimationFrame(0,0,16,32))  },
                { "Working",Animation.CreateAnimationFromTextureSequence(0,0,16,32,10, 6).appendAnimation(Animation.CreateAnimationFromReverseTextureSequence(0,0,16,32,10, 6))}
            }, "Default", "Default"), Color.White, false, new Vector2(1, 1), new Vector2(0,-1),null,null), PoweredMachine.PoweredMachineTier.Electric);

            this.addItem(MachineIds.ElectricMiningDrill, electricMiningDrill);

            MiningDrill coalMiningDrill = new MiningDrill(new BasicItemInformation("", MachineIds.CoalMiningDrill, "", CategoryNames.Machine, Color.SteelBlue, -300, -300, 0, false, 4000, true, true, TextureManagers.Objects_Machines.createAnimationManager("CoalMiningDrill", new SerializableDictionary<string, Animation>() {
                {"Default",new Animation(new AnimationFrame(0,0,16,32))  },
                { "Working",Animation.CreateAnimationFromTextureSequence(0,0,16,32,10, 6).appendAnimation(Animation.CreateAnimationFromReverseTextureSequence(0,0,16,32,10, 6))}
            }, "Default", "Default"), Color.White, false, new Vector2(1, 1), new Vector2(0, -1), null, null), PoweredMachine.PoweredMachineTier.Coal);

            this.addItem(MachineIds.CoalMiningDrill, coalMiningDrill);

            MiningDrill nuclearMiningDrill = new MiningDrill(new BasicItemInformation("", MachineIds.NuclearMiningDrill, "", CategoryNames.Machine, Color.SteelBlue, -300, -300, 0, false, 4000, true, true, TextureManagers.Objects_Machines.createAnimationManager("NuclearMiningDrill", new SerializableDictionary<string, Animation>() {
                {"Default",new Animation(new AnimationFrame(0,0,16,32))  },
                { "Working",Animation.CreateAnimationFromTextureSequence(0,0,16,32,10, 6).appendAnimation(Animation.CreateAnimationFromReverseTextureSequence(0,0,16,32,10, 6))}
            }, "Default", "Default"), Color.White, false, new Vector2(1, 1), new Vector2(0, -1), null, null), PoweredMachine.PoweredMachineTier.Nuclear);

            this.addItem(MachineIds.NuclearMiningDrill, nuclearMiningDrill);

            MiningDrill magicalMiningDrill = new MiningDrill(new BasicItemInformation("", MachineIds.MagicalMiningDrill, "", CategoryNames.Machine, Color.SteelBlue, -300, -300, 0, false, 4000, true, true, TextureManagers.Objects_Machines.createAnimationManager("MagicalMiningDrill", new SerializableDictionary<string, Animation>() {
                {"Default",new Animation(new AnimationFrame(0,0,16,32))  },
                { "Working",Animation.CreateAnimationFromTextureSequence(0,0,16,32,10, 6).appendAnimation(Animation.CreateAnimationFromReverseTextureSequence(0,0,16,32,10, 6))}
            }, "Default", "Default"), Color.White, false, new Vector2(1, 1), new Vector2(0, -1), null, null), PoweredMachine.PoweredMachineTier.Magical);

            this.addItem(MachineIds.MagicalMiningDrill, magicalMiningDrill);


            Windmill windMillV1_0_0 = new Windmill(new BasicItemInformation("", MachineIds.WindmillV1, "", CategoryNames.Machine, Color.SteelBlue, -300, -300, 0, false, 500, true, true, TextureManagers.Objects_Machines.createAnimationManager("Windmill", new SerializableDictionary<string, Animation>() {

                {"Default",new Animation( new AnimationFrame(0,0,16,32)) },
                {"Working",new Animation(new List<AnimationFrame>(){
                    new AnimationFrame(0,0,16,32,20),
                    new AnimationFrame(16,0,16,32,20) },true)
                }
            }, "Default", "Working"), Color.White, false, new Vector2(1, 2), Vector2.Zero, null, null, false, null), Vector2.Zero);

            this.addItem(MachineIds.WindmillV1, windMillV1_0_0);

            this.addItem(FarmingObjects.HayMaker, new HayMaker(new BasicItemInformation("", FarmingObjects.HayMaker, "", CategoryNames.Machine, CategoryColors.Machines, -300, -300, 0, false, 2000, true, true, TextureManagers.Objects_Farming.createAnimationManager("HayMaker", new SerializableDictionary<string, Animation>()
            {
                {"Default",new Animation( new AnimationFrame(0,0,16,32)) },
                    {HayMaker.HayAnimation,new Animation(new List<AnimationFrame>(){
                       new AnimationFrame(16,0,16,32,20)}
                    ,true)},
                    {HayMaker.WheatAnimation,new Animation(new List<AnimationFrame>(){
                       new AnimationFrame(80,0,16,32,20)}
                    ,true)},
                    {HayMaker.CornAnimation,new Animation(new List<AnimationFrame>(){
                       new AnimationFrame(32,0,16,32,20)}
                    ,true)
                    },
                    {HayMaker.AmaranthAnimation,new Animation(new List<AnimationFrame>(){
                       new AnimationFrame(48,0,16,32,20)}
                    ,true)
                    },
                    {HayMaker.FiberAnimation,new Animation(new List<AnimationFrame>(){
                       new AnimationFrame(64,0,16,32,20)}
                    ,true)
                    }
            }, "Default", "Default"), Color.White, false, /* Bounding box is the number of pixels taken up */ new Vector2(1, 1),/*Shift by whitespace*/ new Vector2(0, -1), new InventoryManager(), new Illuminate.LightManager())));

            this.addItem(FarmingObjects.HayMaker_FeedShop, new HayMaker(new BasicItemInformation("", FarmingObjects.HayMaker_FeedShop, "", CategoryNames.Machine, CategoryColors.Machines, -300, -300, 0, false, 2000, true, true, TextureManagers.Objects_Farming.createAnimationManager("HayMaker", new Animation(16, 0, 16, 32)), Color.White, false, /* Bounding box is the number of pixels taken up */ new Vector2(1, 1),/*Shift by whitespace*/ new Vector2(0, -1), new InventoryManager(), new Illuminate.LightManager()),true));



            this.addItem(FarmingObjects.IrrigatedGardenPot, new IrrigatedGardenPot(new BasicItemInformation("", FarmingObjects.IrrigatedGardenPot, "", CategoryNames.Farming, CategoryColors.Farming, -300, -300, 0, false, 5000, true, true, TextureManagers.Objects_Farming.createAnimationManager("IrrigatedGardenPot", new SerializableDictionary<string, Animation>()
            {
                {IrrigatedGardenPot.DEFAULT_ANIMATION_KEY,new Animation( new AnimationFrame(0,0,16,32)) },
                {IrrigatedGardenPot.DRIPPING_ANIMATION_KEY,Animation.CreateAnimationFromTextureSequence(0,0,16,32,13, 6)},

                {IrrigatedGardenPot.DEFAULT_WITH_ENRICHER_AND_PLANTER_ATTACHMENT_ANIMATION_KEY,Animation.CreateAnimationFromTextureSequence(0,32,16,32,1, 6)},
                {IrrigatedGardenPot.DRIPPING_WITH_ENRICHER_AND_PLANTER_ATTACHMENT_ANIMATION_KEY,Animation.CreateAnimationFromTextureSequence(0,32,16,32,13, 6)},

                {IrrigatedGardenPot.DEFAULT_WITH_PLANTER_ATTACHMENT_ANIMATION_KEY,Animation.CreateAnimationFromTextureSequence(0,64,16,32,1, 6)},
                {IrrigatedGardenPot.DRIPPING_WITH_PLANTER_ATTACHMENT_ANIMATION_KEY,Animation.CreateAnimationFromTextureSequence(0,64,16,32,13, 6)},

                {IrrigatedGardenPot.DEFAULT_WITH_ENRICHER_ATTACHMENT_ANIMATION_KEY,Animation.CreateAnimationFromTextureSequence(0,96,16,32,1, 6)},
                {IrrigatedGardenPot.DRIPPING_WITH_ENRICHER_ATTACHMENT_ANIMATION_KEY,Animation.CreateAnimationFromTextureSequence(0,96,16,32,13, 6)},


                {IrrigatedGardenPot.DEFAULT_WITH_AUTO_HARVESTER_ANIMATION_KEY,Animation.CreateAnimationFromTextureSequence(0,128,16,32,1, 6)},
                {IrrigatedGardenPot.DRIPPING_WITH_AUTO_HARVESTER_ANIMATION_KEY,Animation.CreateAnimationFromTextureSequence(0,128,16,32,13, 6)},

                {IrrigatedGardenPot.DEFAULT_WITH_ALL_ATTACHMENTS_ANIMATION_KEY,Animation.CreateAnimationFromTextureSequence(0,160,16,32,1, 6)},
                {IrrigatedGardenPot.DRIPPING_WITH_ALL_ATTACHMENTS_ANIMATION_KEY,Animation.CreateAnimationFromTextureSequence(0,160,16,32,13, 6)},

                {IrrigatedGardenPot.DEFAULT_WITH_AUTO_HARVESTER_PLANTER_ANIMATION_KEY,Animation.CreateAnimationFromTextureSequence(0,192,16,32,1, 6)},
                {IrrigatedGardenPot.DRIPPING_WITH_AUTO_HARVESTER_PLANTER_ANIMATION_KEY,Animation.CreateAnimationFromTextureSequence(0,192,16,32,13, 6)},

                {IrrigatedGardenPot.DEFAULT_WITH_AUTO_HARVESTER_ENRICHER_ANIMATION_KEY,Animation.CreateAnimationFromTextureSequence(0,224,16,32,1, 6)},
                {IrrigatedGardenPot.DRIPPING_WITH_AUTO_HARVESTER_ENRICHER_ANIMATION_KEY,Animation.CreateAnimationFromTextureSequence(0,224,16,32,13, 6)},



            }, IrrigatedGardenPot.DEFAULT_ANIMATION_KEY, IrrigatedGardenPot.DRIPPING_ANIMATION_KEY), Color.White, false, new Vector2(1, 1), new Vector2(0, -1), new InventoryManager(), new Illuminate.LightManager())));


            this.addItem(FarmingObjects.AdvancedFarmingSystem, new AdvancedFarmingSystem(new BasicItemInformation("", FarmingObjects.AdvancedFarmingSystem, "", CategoryNames.Farming, CategoryColors.Farming, -300, -300, 0, false, 10000, true, true, TextureManagers.Objects_Farming.createAnimationManager("AdvancedFarmingSystem", new Animation(0, 0, 16, 32)), Color.White, false, new Vector2(1, 1), new Vector2(0, -1), null, null)));


            this.addItem(MachineIds.ElectricFurnace, new ElectricFurnace(new BasicItemInformation("", MachineIds.ElectricFurnace, "", CategoryNames.Machine, CategoryColors.Machines, -300, -300, 0, false, 2500, true, true, TextureManagers.Objects_Machines.createAnimationManager("ElectricFurnace",
                new Dictionary<string, Animation>()
                {
                    {ElectricFurnace.ELECTRIC_IDLE_ANIMATION_KEY,  new Animation(new Rectangle(0,0,16,32)) },
                    {ElectricFurnace.ELECTRIC_WORKING_ANIMATION_KEY,  new Animation(new Rectangle(16,0,16,32)) }

                }, ElectricFurnace.ELECTRIC_IDLE_ANIMATION_KEY, ElectricFurnace.ELECTRIC_IDLE_ANIMATION_KEY

                ), Color.White, false, new Vector2(1, 1), new Vector2(0, -1), null, null), PoweredMachine.PoweredMachineTier.Electric));


            this.addItem(MachineIds.NuclearFurnace, new ElectricFurnace(new BasicItemInformation("", MachineIds.ElectricFurnace, "", CategoryNames.Machine, CategoryColors.Machines, -300, -300, 0, false, 10000, true, true, TextureManagers.Objects_Machines.createAnimationManager("ElectricFurnace",
                new Dictionary<string, Animation>()
                {
                    {ElectricFurnace.NUCLEAR_IDLE_ANIMATION_KEY,  new Animation(new Rectangle(0,32,16,32)) },
                    {ElectricFurnace.NUCLEAR_WORKING_ANIMATION_KEY,  new Animation(new Rectangle(16,32,16,32)) }

                }, ElectricFurnace.NUCLEAR_IDLE_ANIMATION_KEY, ElectricFurnace.NUCLEAR_IDLE_ANIMATION_KEY

                ), Color.White, false, new Vector2(1, 1), new Vector2(0, -1), null, null), PoweredMachine.PoweredMachineTier.Nuclear));


            this.addItem(MachineIds.MagicalFurnace, new ElectricFurnace(new BasicItemInformation("", MachineIds.ElectricFurnace, "", CategoryNames.Machine, CategoryColors.Machines, -300, -300, 0, false, 50000, true, true, TextureManagers.Objects_Machines.createAnimationManager("ElectricFurnace",
                new Dictionary<string, Animation>()
                {
                    {ElectricFurnace.MAGICAL_IDLE_ANIMATION_KEY,  new Animation(new Rectangle(0,64,16,32)) },
                    {ElectricFurnace.MAGICAL_WORKING_ANIMATION_KEY,  new Animation(new Rectangle(16,64,16,32)) }

                }, ElectricFurnace.MAGICAL_IDLE_ANIMATION_KEY, ElectricFurnace.MAGICAL_IDLE_ANIMATION_KEY

                ), Color.White, false, new Vector2(1, 1), new Vector2(0, -1), null, null), PoweredMachine.PoweredMachineTier.Magical));
        }

        /// <summary>
        /// Gets a random object from the dictionary passed in.
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public Item getRandomObject(Dictionary<string, CustomObject> dictionary)
        {
            if (dictionary.Count == 0) return null;
            List<CustomObject> objs = new List<CustomObject>();
            foreach (KeyValuePair<string, CustomObject> pair in dictionary)
                objs.Add(pair.Value);
            int rand = Game1.random.Next(0, objs.Count);
            return objs[rand].getOne();
        }

        /// <summary>
        /// Adds in an item to be tracked by the mod's object manager.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="I"></param>
        public void addItem(string key, Item I)
        {
            if (this.itemsById.ContainsKey(key))
                throw new Exception("Item with the same key has already been added into the mod!");
            else
                this.itemsById.Add(key, I);
        }

        /// <summary>
        /// Gets an item from the list of modded items.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Stack"></param>
        /// <returns></returns>
        public virtual Item getItem(string Key, int Stack = 1)
        {
            return this.GetItem<Item>(Key, Stack);
        }

        /// <summary>
        /// Gets an item from the list of modded items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Key"></param>
        /// <param name="Stack"></param>
        /// <returns></returns>
        public virtual T GetItem<T>(string Key, int Stack = 1) where T : Item
        {

            if (this.itemsById.ContainsKey(Key))
            {
                Item I = this.itemsById[Key].getOne();
                I.Stack = Stack;
                return (T)I;
            }
            else
                throw new InvalidObjectManagerItemException(string.Format("Error: Trying to request an item with id {0} but there is none registered with the object manager!", Key));

        }


        public virtual T getObject<T>(string Key, int Stack = 1) where T : StardewValley.Object
        {

            if (this.itemsById.ContainsKey(Key))
            {
                Item I = this.itemsById[Key].getOne();
                I.Stack = Stack;
                return (T)I;
            }
            else
                return null;

        }

        /// <summary>
        /// Gets a StardewValley vanilla item with the given id.
        /// </summary>
        /// <param name="sdvObjectId"></param>
        /// <param name="Stack"></param>
        /// <returns></returns>
        public virtual Item getItem(Enums.SDVObject sdvObjectId, int Stack = 1)
        {
            return new StardewValley.Object((int)sdvObjectId, Stack);
        }

        public virtual StardewValley.Object getObject(Enums.SDVObject sdvId, int Stack = 1)
        {
            return (StardewValley.Object)this.getItem(sdvId, Stack);
        }

        public virtual StardewValley.Object getObject(Enums.SDVBigCraftable sdvId, int Stack = 1)
        {
            return (StardewValley.Object)this.getItem(sdvId, Stack);
        }

        /// <summary>
        /// Gets a Stardew Valley vanilla big craftable object with the given id.
        /// </summary>
        /// <param name="sdvBigCraftableId"></param>
        /// <param name="Stack"></param>
        /// <returns></returns>
        public virtual Item getItem(Enums.SDVBigCraftable sdvBigCraftableId, int Stack = 1)
        {
            StardewValley.Object obj = new StardewValley.Object(Vector2.Zero, (int)sdvBigCraftableId);
            obj.Stack = Stack;
            return obj;
        }

        /// <summary>
        /// Adds a new object manager to the master pool of managers.
        /// </summary>
        /// <param name="Manifest"></param>
        public static void addObjectManager(IManifest Manifest)
        {
            if (ObjectPools == null) ObjectPools = new Dictionary<string, ObjectManager>();
            ObjectPools.Add(Manifest.UniqueID, new ObjectManager(Manifest));
        }


        /// <summary>
        /// Cleans up all stored information.
        /// </summary>
        public void returnToTitleCleanUp()
        {

        }

    }
}
