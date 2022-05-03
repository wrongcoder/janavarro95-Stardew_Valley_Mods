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
using Omegasis.Revitalize.Framework.World.Objects;
using Omegasis.Revitalize.Framework.World.Objects.InformationFiles;
using Omegasis.Revitalize.Framework.Utilities;
using Omegasis.StardustCore.UIUtilities;
using Omegasis.StardustCore.Animations;
using Omegasis.Revitalize.Framework.Managers;
using Omegasis.Revitalize.Framework.World.Objects.Farming;
using Omegasis.Revitalize.Framework.World.Objects.Items.Farming;
using Omegasis.Revitalize.Framework.World.Objects.Machines.Furnaces;

namespace Omegasis.Revitalize.Framework.Objects
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

        public Dictionary<string, Item> ItemsByName;

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
            this.ItemsByName = new Dictionary<string, Item>();

            //Load in furniture again!
        }

        /// <summary>
        /// Loads in the items for the object and resource managers.
        /// </summary>
        public void loadItemsFromDisk()
        {
            this.resources.loadInItems(); //Must be first.

            this.loadInItems();
            this.loadInCraftingTables();
            this.loadInMachines();
            this.loadInAestheticsObjects();

            //Should load blueprints last due to the fact that they can draw references to objects.
            this.loadInBlueprints();
        }

        private void loadInAestheticsObjects()
        {
        }

        private void loadInItems()
        {

            AutoPlanterGardenPotAttachment autoPlanterGardenPotAttachment = new AutoPlanterGardenPotAttachment(new BasicItemInformation("Auto Planter Attachment", FarmingItems.AutoPlanterGardenPotAttachment, "An attachment that when used on a Irrigated Garden Pot, will allow a Farming System to planr seeds automatically into the pot!", CategoryNames.Farming, CategoryColors.Farming, -300, -300, 0, false, 5000, false, false, TextureManagers.Items_Farming.createAnimationManager("AutoPlanterGardenPotAttachment", new Animation(0, 0, 16, 32)), Color.White, false, new Vector2(1, 1), Vector2.Zero, null, null));
            this.addItem(FarmingItems.AutoPlanterGardenPotAttachment, autoPlanterGardenPotAttachment);

            AutoHarvesterGardenPotAttachment autoHarvesterGardenPotAttachment = new AutoHarvesterGardenPotAttachment(new BasicItemInformation("Auto Harvester Attachment", FarmingItems.AutoHarvesterGardenPotAttachment, "An attachment that when used on a Irrigated Garden Pot, will allow a Farming System to automatically harvest crops from the pot!", CategoryNames.Farming, CategoryColors.Farming, -300, -300, 0, false, 5000, false, false, TextureManagers.Items_Farming.createAnimationManager("AutoHarvesterGardenPotAttachment", new Animation(0, 0, 16, 16)), Color.White, false, new Vector2(1, 1), Vector2.Zero, null, null));
            this.addItem(FarmingItems.AutoHarvesterGardenPotAttachment, autoHarvesterGardenPotAttachment);

            this.addItem(MiscItemIds.RadioactiveFuel, new CustomItem(new BasicItemInformation("Radioactive Fuel", MiscItemIds.RadioactiveFuel, "A radioactive fuel cell used to power various machines!", CategoryNames.Misc, CategoryColors.Misc, -300, -300, 0, false, 5000, false, false, TextureManagers.Items_Misc.createAnimationManager("RadioactiveFuel", new Animation(0, 0, 16, 16)), Color.White, false, new Vector2(1, 1), Vector2.Zero, null, null)));
        }

        private void loadInBlueprints()
        {
            Blueprint craftingBlueprint_anvilForWorkbench = new Blueprint(new BasicItemInformation("Blueprint", Blueprints.Workbench_AnvilCraftingRecipeBlueprint, "A blueprint used on how to craft an anvil at a workbench!", CategoryNames.Crafting, Color.Brown, -300, -300, 0, false, 500, false, false, TextureManagers.Items_Crafting.createAnimationManager("Blueprint", new Animation(0, 0, 32, 32)), Color.White, false, new Vector2(2, 2), Vector2.Zero, null, null), new Dictionary<string, string>()
            {
                {Constants.CraftingIds.CraftingRecipeBooks.WorkbenchCraftingRecipies,"Anvil" }

            },new Drawable(this.getItem(CraftingStations.Anvil_Id)));

            this.addItem(Blueprints.Workbench_AnvilCraftingRecipeBlueprint, craftingBlueprint_anvilForWorkbench);
        }

        private void loadInCraftingTables()
        {
            CraftingTable WorkStationObject = new CraftingTable(new BasicItemInformation("Work Station", CraftingStations.WorkStation_Id, "A workbench that can be used for crafting different objects.", CategoryNames.Crafting, Color.Brown, -300, -300, 0, false, 500, true, true, TextureManagers.Objects_Crafting.createAnimationManager("Workbench", new Animation(0, 0, 32, 32)), Color.White, false, new Vector2(2, 2), Vector2.Zero, null, null), CraftingRecipeBooks.WorkbenchCraftingRecipies);
            CraftingTable AnvilObj = new CraftingTable(new BasicItemInformation("Anvil", CraftingStations.Anvil_Id, "An anvil that can be used for crafting different machines and other metalic objects.", CategoryNames.Crafting, Color.Brown, -300, -300, 0, false, 2000, true, true, TextureManagers.Objects_Crafting.createAnimationManager("Anvil", new Animation(0, 0, 32, 32)), Color.White, false, new Vector2(2, 2), Vector2.Zero, null, null), CraftingRecipeBooks.AnvilCraftingRecipes);

            this.addItem(CraftingStations.WorkStation_Id, WorkStationObject);
            this.addItem(CraftingStations.Anvil_Id, AnvilObj);
        }

        private void loadInMachines()
        {

            AdvancedSolarPanel solarP1 = new AdvancedSolarPanel(new BasicItemInformation("Solar Panel", Machines.AdvancedSolarPanelV1, "Generates energy while the sun is up.", CategoryNames.Machine, Color.SteelBlue, -300, -300, 0, false, 1000, true, true, TextureManagers.Objects_Machines.createAnimationManager("SolarPanelTier1", new Animation(0, 0, 16, 16)), Color.White, false, new Vector2(1, 1), Vector2.Zero, null, null));
            AdvancedSolarPanel solarA1V1 = new AdvancedSolarPanel(new BasicItemInformation("Solar Array", Machines.SolarArrayV1, "A collection of solar panels that generates even more energy while the sun is up.", CategoryNames.Machine, Color.SteelBlue, -300, -300, 0, false, 1000, true, true, TextureManagers.Objects_Machines.createAnimationManager("SolarArrayTier1", new Animation(0, 0, 16, 16)), Color.White, false, new Vector2(1, 1), Vector2.Zero, null, null));

            this.addItem(Machines.AdvancedSolarPanelV1, solarP1);
            this.addItem(Machines.SolarArrayV1, solarA1V1);


            Machine miningDrillMachine_0_0 = new Machine(new BasicItemInformation("Mining Drill", Machines.MiningDrillV1, "Digs up rocks and ores. Requires energy to run.", CategoryNames.Machine, Color.SteelBlue, -300, -300, 0, false, 4000, true, true, TextureManagers.Objects_Machines.createAnimationManager("MiningDrillMachine", new SerializableDictionary<string, Animation>() {
                {"Default",new Animation(new AnimationFrame(0,0,16,16))  },
                { "Mining",new Animation(new List<AnimationFrame>(){
                    new AnimationFrame(0,0,16,32,30),
                    new AnimationFrame(16,0,16,32,30),
                    new AnimationFrame(32,0,16,32,30),
                    new AnimationFrame(48,0,16,32,30)},
                    true) }
            }, "Default", "Mining"), Color.White, false, new Vector2(1, 2), Vector2.Zero, new InventoryManager(new List<Item>(), 18, 3, 6), null), RevitalizeModCore.ObjectManager.resources.miningDrillResources.Values.ToList());

            this.addItem(Machines.MiningDrillV1, miningDrillMachine_0_0);


            Windmill windMillV1_0_0 = new Windmill(new BasicItemInformation("Windmill", Machines.WindmillV1, "Generates power from the wind.", CategoryNames.Machine, Color.SteelBlue, -300, -300, 0, false, 500, true, true, TextureManagers.Objects_Machines.createAnimationManager("Windmill", new SerializableDictionary<string, Animation>() {

                {"Default",new Animation( new AnimationFrame(0,0,16,32)) },
                {"Working",new Animation(new List<AnimationFrame>(){
                    new AnimationFrame(0,0,16,32,20),
                    new AnimationFrame(16,0,16,32,20) },true)
                }
            }, "Default", "Working"), Color.White, false, new Vector2(1, 2), Vector2.Zero, null, null, false, null), Vector2.Zero);

            this.addItem(Machines.WindmillV1, windMillV1_0_0);

            this.addItem(Machines.HayMaker, new HayMaker(new BasicItemInformation("Hay Maker", Machines.HayMaker, "Used to turn different grains and grasses into animal feed.", CategoryNames.Machine, CategoryColors.Machines, -300, -300, 0, false, 2000, true, true, TextureManagers.Objects_Farming.createAnimationManager("HayMaker", new SerializableDictionary<string, Animation>()
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


            this.addItem(FarmingObjects.IrrigatedGardenPot, new IrrigatedGardenPot(new BasicItemInformation("Irrigated Garden Pot", FarmingObjects.IrrigatedGardenPot, "A garden pot with an irrigation system attached. Waters your crops for you!", CategoryNames.Farming, CategoryColors.Farming, -300, -300, 0, false, 5000, true, true, TextureManagers.Objects_Farming.createAnimationManager("IrrigatedGardenPot", new SerializableDictionary<string, Animation>()
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


            this.addItem(FarmingObjects.AdvancedFarmingSystem, new AdvancedFarmingSystem(new BasicItemInformation("Advanced Farming System", FarmingObjects.AdvancedFarmingSystem, "An advanced farming system that interfaces irrigated gardening pots and does various tasks depending if there are enrichers, auto harvesters, or auto planter attachments on them!", CategoryNames.Farming, CategoryColors.Farming, -300, -300, 0, false, 10000, true, true, TextureManagers.Objects_Farming.createAnimationManager("AdvancedFarmingSystem", new Animation(0, 0, 16, 32)),Color.White,false,new Vector2(1,1),new Vector2(0,-1),null,null)));


            this.addItem(Machines.ElectricFurnace, new ElectricFurnace(new BasicItemInformation("Electric Furnace", Machines.ElectricFurnace, "An advanced furnace that smelts objects 25 percent faster and uses battery packs instead of coal.", CategoryNames.Machine, CategoryColors.Machines, -300, -300, 0, false, 2500, true, true, TextureManagers.Objects_Machines.createAnimationManager("ElectricFurnace",
                new Dictionary<string, Animation>()
                {
                    {ElectricFurnace.ELECTRIC_IDLE_ANIMATION_KEY,  new Animation(new Rectangle(0,0,16,32)) },
                    {ElectricFurnace.ELECTRIC_WORKING_ANIMATION_KEY,  new Animation(new Rectangle(16,0,16,32)) }

                },ElectricFurnace.ELECTRIC_IDLE_ANIMATION_KEY, ElectricFurnace.ELECTRIC_IDLE_ANIMATION_KEY

                ), Color.White, false, new Vector2(1, 1), new Vector2(0, -1), null, null), ElectricFurnace.FurnaceType.Electric));


            this.addItem(Machines.NuclearFurnace, new ElectricFurnace(new BasicItemInformation("Nuclear Furnace", Machines.ElectricFurnace, "An advanced furnace that smelts objects twice as fast as a normal furnace and uses nuclear fuel.", CategoryNames.Machine, CategoryColors.Machines, -300, -300, 0, false, 10000, true, true, TextureManagers.Objects_Machines.createAnimationManager("ElectricFurnace",
                new Dictionary<string, Animation>()
                {
                    {ElectricFurnace.NUCLEAR_IDLE_ANIMATION_KEY,  new Animation(new Rectangle(0,32,16,32)) },
                    {ElectricFurnace.NUCLEAR_WORKING_ANIMATION_KEY,  new Animation(new Rectangle(16,32,16,32)) }

                }, ElectricFurnace.NUCLEAR_IDLE_ANIMATION_KEY, ElectricFurnace.NUCLEAR_IDLE_ANIMATION_KEY

                ), Color.White, false, new Vector2(1, 1), new Vector2(0, -1), null, null), ElectricFurnace.FurnaceType.Nuclear));


            this.addItem(Machines.MagicalFurnace, new ElectricFurnace(new BasicItemInformation("Magical Furnace", Machines.ElectricFurnace, "An advanced furnace that smelts objects 75 percent faster than a normal furnace, and thanks to magical reactor technology, requires no fuel to operate.", CategoryNames.Machine, CategoryColors.Machines, -300, -300, 0, false, 50000, true, true, TextureManagers.Objects_Machines.createAnimationManager("ElectricFurnace",
                new Dictionary<string, Animation>()
                {
                    {ElectricFurnace.MAGICAL_IDLE_ANIMATION_KEY,  new Animation(new Rectangle(0,64,16,32)) },
                    {ElectricFurnace.MAGICAL_WORKING_ANIMATION_KEY,  new Animation(new Rectangle(16,64,16,32)) }

                }, ElectricFurnace.MAGICAL_IDLE_ANIMATION_KEY, ElectricFurnace.MAGICAL_IDLE_ANIMATION_KEY

                ), Color.White, false, new Vector2(1, 1), new Vector2(0, -1), null, null), ElectricFurnace.FurnaceType.Magical));
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
            {
                objs.Add(pair.Value);
            }
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
            if (this.ItemsByName.ContainsKey(key))
            {
                throw new Exception("Item with the same key has already been added into the mod!");
            }
            else
            {
                this.ItemsByName.Add(key, I);
            }
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

        public virtual T GetItem<T>(string Key, int Stack = 1) where T : Item
        {

            if (this.ItemsByName.ContainsKey(Key))
            {
                Item I = this.ItemsByName[Key].getOne();
                I.Stack = Stack;
                return (T)I;
            }
            else
            {
                return null;
            }

        }

        public virtual T getObject<T>(string Key, int Stack = 1) where T : StardewValley.Object
        {

            if (this.ItemsByName.ContainsKey(Key))
            {
                Item I = this.ItemsByName[Key].getOne();
                I.Stack = Stack;
                return (T)I;
            }
            else
            {
                return null;
            }

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
            StardewValley.Object obj= new StardewValley.Object(Vector2.Zero,(int)sdvBigCraftableId);
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
