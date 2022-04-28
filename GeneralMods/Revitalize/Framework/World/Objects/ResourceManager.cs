using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using StardewValley;
using Omegasis.Revitalize.Framework.Constants;
using Omegasis.Revitalize.Framework.Constants.ItemIds.Resources.EarthenResources;
using Omegasis.Revitalize.Framework.Utilities;
using Omegasis.Revitalize.Framework.World.Objects.InformationFiles;
using Omegasis.Revitalize.Framework.World.Objects.Items;
using Omegasis.Revitalize.Framework.World.Objects.Resources;
using Omegasis.Revitalize.Framework.World.Objects;
using Omegasis.Revitalize.Framework.World.WorldUtilities;
using Omegasis.StardustCore.UIUtilities;
using Omegasis.StardustCore.Animations;
using Omegasis.Revitalize.Framework.Managers;

namespace Omegasis.Revitalize.Framework.Objects
{
    public class ResourceManager
    {

        private string oreResourceDataPath = Path.Combine("Data", "Objects", "Resources", "Ore");

        /// <summary>
        /// A static reference to the resource manager for quicker access.
        /// </summary>
        public static ResourceManager self;

        /// <summary>
        /// A list of all of the ores held by the resource manager.
        /// </summary>
        public Dictionary<string, OreVein> oreVeins;
        public Dictionary<string, OreResourceInformation> oreResourceInformationTable;
        public Dictionary<string, Ore> ores;
        public Dictionary<string, CustomObject> resources;

        public Dictionary<string, ResourceInformation> miningDrillResources;

        /// <summary>
        /// A dictionary containing the names of all objects that can be burned with their burn times for a value.
        /// </summary>
        public Dictionary<string, int> burnableObjects;

        /// <summary>
        /// A list of all visited floors on the current visit to the mines.
        /// </summary>
        public List<int> visitedFloors;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ResourceManager()
        {
            self = this;
            this.oreVeins = new Dictionary<string, OreVein>();
            this.oreResourceInformationTable = new Dictionary<string, OreResourceInformation>();
            this.ores = new Dictionary<string, Ore>();
            this.visitedFloors = new List<int>();
            this.resources = new Dictionary<string, CustomObject>();
            this.miningDrillResources = new Dictionary<string, ResourceInformation>();
            this.burnableObjects = new Dictionary<string, int>();
        }


        //Loads in the items for the resource manager.
        public void loadInItems()
        {
            this.loadInOreItems();
            this.loadInResourceItems();
            this.loadOreVeins();
            this.loadInMiningDrillLootTable();
            this.loadInFluidDictionary();
            this.loadInBurnableObjects();
        }

        private void loadInFluidDictionary()
        {
        }

        private void loadInBurnableObjects()
        {
            this.burnableObjects.Add("Coal", TimeUtilities.GetMinutesFromTime(0, 1, 0));
            this.burnableObjects.Add("Wood", TimeUtilities.GetMinutesFromTime(0, 0, 10));
        }
        private void loadInMiningDrillLootTable()
        {
            this.miningDrillResources.Add("Bauxite", new ResourceInformation(new ObjectManagerItemReference(Ores.BauxiteOre), RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.amountOfBauxiteToMine.min, RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.amountOfBauxiteToMine.max, 1, 1, 1, RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.bauxiteMineChance, 0, 0, 0, 0));
            this.miningDrillResources.Add("Tin", new ResourceInformation(new ObjectManagerItemReference(Ores.TinOre), RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.amountOfTinToMine.min, RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.amountOfTinToMine.max, 1, 1, 1, RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.tinMineChance, 0, 0, 0, 0));
            this.miningDrillResources.Add("Lead", new ResourceInformation(new ObjectManagerItemReference(Ores.LeadOre), RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.amountOfLeadToMine.min, RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.amountOfLeadToMine.max, 1, 1, 1, RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.leadMineChance, 0, 0, 0, 0));
            this.miningDrillResources.Add("Silver", new ResourceInformation(new ObjectManagerItemReference(Ores.SilverOre), RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.amountOfSilverToMine.min, RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.amountOfSilverToMine.max, 1, 1, 1, RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.silverMineChance, 0, 0, 0, 0));
            this.miningDrillResources.Add("Titanium", new ResourceInformation(new ObjectManagerItemReference(Ores.TinOre), RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.amountOfTitaniumToMine.min, RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.amountOfTitaniumToMine.max, 1, 1, 1, RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.titaniumMineChance, 0, 0, 0, 0));
            this.miningDrillResources.Add("Prismatic", new ResourceInformation(new ObjectManagerItemReference(Gems.PrismaticNugget), RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.amountOfPrismaticNuggetsToMine.min, RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.amountOfPrismaticNuggetsToMine.max, 1, 1, 1, RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.prismaticNuggetMineChance, 0, 0, 0, 0));
            this.miningDrillResources.Add("Copper", new ResourceInformation(new StardewValleyItemReference(Enums.SDVObject.CopperOre, 1), RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.amountOfCopperToMine.min, RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.amountOfCopperToMine.max, 1, 1, 1, RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.copperMineChance, 0, 0, 0, 0));
            this.miningDrillResources.Add("Iron", new ResourceInformation(new StardewValleyItemReference(Enums.SDVObject.IronOre, 1), RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.amountOfIronToMine.min, RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.amountOfIronToMine.max, 1, 1, 1, RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.ironMineChance, 0, 0, 0, 0));
            this.miningDrillResources.Add("Gold", new ResourceInformation(new StardewValleyItemReference(Enums.SDVObject.GoldOre, 1), RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.amountOfGoldToMine.min, RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.amountOfGoldToMine.max, 1, 1, 1, RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.goldMineChance, 0, 0, 0, 0));
            this.miningDrillResources.Add("Iridium", new ResourceInformation(new StardewValleyItemReference(Enums.SDVObject.IridiumOre, 1), RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.amountOfIridiumToMine.min, RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.amountOfIridiumToMine.max, 1, 1, 1, RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.iridiumMineChance, 0, 0, 0, 0));
            this.miningDrillResources.Add("Stone", new ResourceInformation(new StardewValleyItemReference(Enums.SDVObject.Stone, 1), RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.amountOfStoneToMine.min, RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.amountOfStoneToMine.max, 1, 1, 1, RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.stoneMineChance, 0, 0, 0, 0));
            this.miningDrillResources.Add("Clay", new ResourceInformation(new StardewValleyItemReference(Enums.SDVObject.Clay, 1), RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.amountOfClayToMine.min, RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.amountOfClayToMine.max, 1, 1, 1, RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.clayMineChance, 0, 0, 0, 0));
            this.miningDrillResources.Add("Sand", new ResourceInformation(new ObjectManagerItemReference(MiscEarthenResources.Sand), RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.amountOfSandToMine.min, RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.amountOfSandToMine.max, 1, 1, 1, RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.sandMineChance, 0, 0, 0, 0));
            this.miningDrillResources.Add("Geode", new ResourceInformation(new StardewValleyItemReference(Enums.SDVObject.Geode, 1), RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.amountOfGeodesToMine.min, RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.amountOfGeodesToMine.max, 1, 1, 1, RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.geodeMineChance, 0, 0, 0, 0));
            this.miningDrillResources.Add("FrozenGeode", new ResourceInformation(new StardewValleyItemReference(Enums.SDVObject.FrozenGeode, 1), RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.amountOfFrozenGeodesToMine.min, RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.amountOfFrozenGeodesToMine.max, 1, 1, 1, RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.frozenGeodeMineChance, 0, 0, 0, 0));
            this.miningDrillResources.Add("MagmaGeode", new ResourceInformation(new StardewValleyItemReference(Enums.SDVObject.MagmaGeode, 1), RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.amountOfMagmaGeodesToMine.min, RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.amountOfMagmaGeodesToMine.max, 1, 1, 1, RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.magmaGeodeMineChance, 0, 0, 0, 0));
            this.miningDrillResources.Add("OmniGeode", new ResourceInformation(new StardewValleyItemReference(Enums.SDVObject.OmniGeode, 1), RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.amountOfOmniGeodesToMine.min, RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.amountOfOmniGeodesToMine.max, 1, 1, 1, RevitalizeModCore.Configs.objectConfigManager.miningDrillConfig.omniGeodeMineChance, 0, 0, 0, 0));
        }

        /// <summary>
        /// Loads in all of the ore veins for the game.
        /// </summary>
        protected void loadOreVeins()
        {
            foreach (var v in this.createOreVeins())
            {
                this.oreVeins.Add(v.basicItemInformation.id, v);
            }
        }


        /// <summary>
        /// Serializes an example ore to eb
        /// </summary>
        protected List<OreVein> createOreVeins()
        {
            //Tin
            List<OreVein> oreVeins = new List<OreVein>();
            OreVein tinOre_0_0 = new OreVein(new BasicItemInformation("Tin Ore Vein", "Revitalize.Resources.Ore.Tin", "A ore vein that is full of tin.", "Revitalize.Ore", Color.Black, -300, -300, 0, false, 350, true, true, TextureManagers.Resources_Ore.createAnimationManager( "Tin", new Animation(0, 0, 16, 16)), Color.White, false, new Vector2(1, 1), Vector2.Zero ,null, null),
                new OreResourceInformation(new ObjectManagerItemReference(Ores.TinOre), true, true, true, false, new List<IntRange>()
            {
                new IntRange(1,20)
            }, new List<IntRange>(),
            
            1, 3, 1, 10, new IntRange(1, 3), new IntRange(1, 3), new IntRange(0, 0), new List<IntRange>()
            {
                new IntRange(0,0)
            }, new List<IntRange>()
            {
                new IntRange(0,9999)
            }, 0.80d, 0.20d, 0.25d, 1d, 1d, 1, 1, 1, 1), new List<ResourceInformation>(), 4);

            //Aluminum
            OreVein bauxiteOre_0_0 = new OreVein(new BasicItemInformation("Bauxite Ore Vein", "Revitalize.Resources.Ore.Bauxite", "A ore vein that is full of bauxite ore.", "Revitalize.Ore", Color.Black, -300, -300, 0, false, 350, true, true, TextureManagers.Resources_Ore.createAnimationManager("Bauxite", new Animation(0, 0, 16, 16)), Color.White, false, new Vector2(1, 1), Vector2.Zero, null, null),
                new OreResourceInformation(new ObjectManagerItemReference(Ores.BauxiteOre), true, true, true, false, new List<IntRange>()
            {
                new IntRange(20,50)
            }, new List<IntRange>(), 1, 3, 1, 10, new IntRange(1, 3), new IntRange(1, 3), new IntRange(0, 0), new List<IntRange>()
            {
                new IntRange(0,0)
            }, new List<IntRange>()
            {
                new IntRange(0,9999)
            }, .70d, 0.16d, 0.20d, 1d, 1d, 0, 0, 0, 0), new List<ResourceInformation>(), 5);

            //Silver
            OreVein silverOre_0_0 = new OreVein(new BasicItemInformation("Silver Ore Vein", "Revitalize.Resources.Ore.Silver", "A ore vein that is full of silver ore.", "Revitalize.Ore", Color.Black, -300, -300, 0, false, 350, true, true, TextureManagers.Resources_Ore.createAnimationManager("Silver", new Animation(0, 0, 16, 16)), Color.White, false, new Vector2(1, 1), Vector2.Zero, null, null),
                new OreResourceInformation(new ObjectManagerItemReference(Ores.SilverOre), true, true, true, false, new List<IntRange>()
            {
                new IntRange(60,100)
            }, new List<IntRange>(), 1, 3, 1, 10, new IntRange(1, 3), new IntRange(1, 3), new IntRange(0, 0), new List<IntRange>()
            {
                new IntRange(0,0)
            }, new List<IntRange>()
            {
                new IntRange(0,9999)
            }, .50d, 0.10d, 0.14d, 1d, 1d, 0, 0, 0, 0), new List<ResourceInformation>(), 6);

            //Lead
            OreVein leadOre_0_0 = new OreVein(new BasicItemInformation("Lead Ore Vein", "Revitalize.Resources.Ore.Lead", "A ore vein that is full of lead ore.", "Revitalize.Ore", Color.Black, -300, -300, 0, false, 350, true, true, TextureManagers.Resources_Ore.createAnimationManager("Lead", new Animation(0, 0, 16, 16)), Color.White, false, new Vector2(1, 1), Vector2.Zero, null, null),
                new OreResourceInformation(new ObjectManagerItemReference(Ores.LeadOre), true, true, true, false, new List<IntRange>()
            {
                new IntRange(60,70),
                new IntRange(90,120)
            }, new List<IntRange>(), 1, 3, 1, 10, new IntRange(1, 3), new IntRange(1, 3), new IntRange(0, 0), new List<IntRange>()
            {
                new IntRange(0,0)
            }, new List<IntRange>()
            {
                new IntRange(0,9999)
            }, .60d, 0.13d, 0.17d, 1d, 1d, 0, 0, 0, 0), new List<ResourceInformation>(), 7);

            //Titanium
            OreVein titaniumOre_0_0 = new OreVein(new BasicItemInformation("Titanium Ore Vein", "Revitalize.Resources.Ore.Titanium", "A ore vein that is full of lead ore.", "Revitalize.Ore", Color.Black, -300, -300, 0, false, 350, true, true, TextureManagers.Resources_Ore.createAnimationManager("Titanium", new Animation(0, 0, 16, 16)), Color.White, false, new Vector2(1, 1), Vector2.Zero, null, null),
                new OreResourceInformation(new ObjectManagerItemReference(Ores.TitaniumOre), true, true, true, false, new List<IntRange>()
            {
                new IntRange(60,70),
                new IntRange(90,120)
            }, new List<IntRange>(), 1, 3, 1, 10, new IntRange(1, 3), new IntRange(1, 3), new IntRange(0, 0), new List<IntRange>()
            {
                new IntRange(0,0)
            }, new List<IntRange>()
            {
                new IntRange(0,9999)
            }, .40d, 0.05d, 0.10d, 1d, 1d, 0, 0, 0, 0), new List<ResourceInformation>(), 8);

            //Prismatic nugget ore
            OreVein prismaticOre_0_0 = new OreVein(new BasicItemInformation("Prismatic Ore Vein", "Revitalize.Resources.Ore.Prismatic", "A ore vein that is full of prismatic ore.", "Revitalize.Ore", Color.Black, -300, -300, 0, false, 350, true, true, TextureManagers.Resources_Ore.createAnimationManager("Prismatic", new Animation(0, 0, 16, 16)), Color.White, false, new Vector2(1, 1), Vector2.Zero, null, null),
                new OreResourceInformation(new ObjectManagerItemReference(Gems.PrismaticNugget), true, true, true, false, new List<IntRange>()
            {
                new IntRange(110,120)
            }, new List<IntRange>(), 1, 3, 1, 1, new IntRange(1, 1), new IntRange(1, 1), new IntRange(1, 5), new List<IntRange>()
            {
                new IntRange(1,9999)
            }, new List<IntRange>()
            {
            }, .05d, 0.01d, 0.01d, 0.10, 1d, 1, 1, 1, 1), new List<ResourceInformation>(), 10);


            oreVeins.Add(tinOre_0_0);
            oreVeins.Add(bauxiteOre_0_0);
            oreVeins.Add(silverOre_0_0);
            oreVeins.Add(leadOre_0_0);
            oreVeins.Add(titaniumOre_0_0);
            oreVeins.Add(prismaticOre_0_0);
            return oreVeins;
        }

        /// <summary>
        /// Loads in all of the ore items into the game.
        /// </summary>
        private void loadInOreItems()
        {
            Ore tinOre = new Ore(new BasicItemInformation("Tin Ore", Ores.TinOre, "Tin ore that can be smelted into tin ingots for further use.",CategoryNames.Ore , Color.Silver, -300, -300, 0, false, 7, false, false, TextureManagers.createOreResourceAnimationManager("TinOre"), Color.White, true, new Vector2(1, 1), Vector2.Zero, null, null), 1);
            this.ores.Add(Ores.TinOre, tinOre);

            Ore bauxiteOre = new Ore(new BasicItemInformation("Bauxite Ore", Ores.BauxiteOre, "Bauxite ore that can be smelted into aluminum ingots for further use.", CategoryNames.Ore, Color.Silver, -300, -300, 0, false, 11, false, false, TextureManagers.createOreResourceAnimationManager("BauxiteOre"), Color.White, true, new Vector2(1, 1), Vector2.Zero, null, null), 1);
            this.ores.Add(Ores.BauxiteOre, bauxiteOre);

            Ore leadOre = new Ore(new BasicItemInformation("Lead Ore", Ores.LeadOre, "Lead ore that can be smelted into lead ingots for further use.", CategoryNames.Ore, Color.Silver, -300, -300, 0, false, 15, false, false, TextureManagers.createOreResourceAnimationManager("LeadOre"), Color.White, true, new Vector2(1, 1), Vector2.Zero, null, null), 1);
            this.ores.Add(Ores.LeadOre, leadOre);

            Ore silverOre = new Ore(new BasicItemInformation("Silver Ore", Ores.SilverOre, "Silver ore that can be smelted into silver ingots for further use.", CategoryNames.Ore, Color.Silver, -300, -300, 0, false, 20, false, false, TextureManagers.createOreResourceAnimationManager("SilverOre"), Color.White, true, new Vector2(1, 1), Vector2.Zero, null, null), 1);
            this.ores.Add(Ores.SilverOre, silverOre);

            Ore titaniumOre = new Ore(new BasicItemInformation("Titanium Ore", Ores.TitaniumOre, "Titanium ore that can be smelted into titanium ingots for further use.", CategoryNames.Ore, Color.Silver, -300, -300, 0, false, 35, false, false, TextureManagers.createOreResourceAnimationManager("TitaniumOre"), Color.White, true, new Vector2(1, 1), Vector2.Zero, null, null), 1);
            this.ores.Add(Ores.TitaniumOre, titaniumOre);

            Ore prismaticOre = new Ore(new BasicItemInformation("Prismatic Nugget", Gems.PrismaticNugget, "Rare prismatic ore that can be smelted into a prismatic shard when seven are gathered.", CategoryNames.Ore, Color.Silver, -300, -300, 0, false, 200, false, false, TextureManagers.createOreResourceAnimationManager("PrismaticNugget"), Color.White, true, new Vector2(1, 1), Vector2.Zero, null, null), 1);
            this.ores.Add(Gems.PrismaticNugget, prismaticOre);

            CustomObject tinIngot = new CustomObject(new BasicItemInformation("Tin Ingot", Ingots.TinIngot, "A tin ingot that can be used for crafting purposes.", CategoryNames.Resource, Color.Silver, -300, -300, 0, false, 75, false, false, TextureManagers.createOreResourceAnimationManager("TinIngot"), Color.White, true, new Vector2(1, 1), Vector2.Zero, null, null), 1);
            RevitalizeModCore.ObjectManager.addItem(Ingots.TinIngot, tinIngot);

            CustomObject aluminumIngot = new CustomObject(new BasicItemInformation("Aluminum Ingot", Ingots.AluminumIngot, "An aluminum ingot that can be used for crafting purposes.", CategoryNames.Resource, Color.Silver, -300, -300, 0, false, 120, false, false, TextureManagers.createOreResourceAnimationManager("AluminumIngot"), Color.White, true, new Vector2(1, 1), Vector2.Zero, null, null), 1);
            RevitalizeModCore.ObjectManager.addItem(Ingots.AluminumIngot, aluminumIngot);

            CustomObject leadIngot = new CustomObject(new BasicItemInformation("Lead Ingot", Ingots.LeadIngot, "A lead ingot that can be used for crafting purposes.", CategoryNames.Resource, Color.Silver, -300, -300, 0, false, 165, false, false, TextureManagers.createOreResourceAnimationManager("LeadIngot"), Color.White, true, new Vector2(1, 1), Vector2.Zero, null, null), 1);
            RevitalizeModCore.ObjectManager.addItem(Ingots.LeadIngot, leadIngot);

            CustomObject silverIngot = new CustomObject(new BasicItemInformation("Silver Ingot", Ingots.SilverIngot, "A silver ingot that can be used for crafting purposes.", CategoryNames.Resource, Color.Silver, -300, -300, 0, false, 220, false, false, TextureManagers.createOreResourceAnimationManager("SilverIngot"), Color.White, true, new Vector2(1, 1), Vector2.Zero, null, null), 1);
            RevitalizeModCore.ObjectManager.addItem(Ingots.SilverIngot, silverIngot);

            CustomObject titaniumIngot = new CustomObject(new BasicItemInformation("Titanium Ingot", Ingots.TitaniumIngot, "A titanium ingot that can be used for crafting purposes.", CategoryNames.Resource, Color.Silver, -300, -300, 0, false, 325, false, false, TextureManagers.createOreResourceAnimationManager("TitaniumIngot"), Color.White, true, new Vector2(1, 1), Vector2.Zero, null, null), 1);
            RevitalizeModCore.ObjectManager.addItem(Ingots.TitaniumIngot, titaniumIngot);

            CustomObject brassIngot = new CustomObject(new BasicItemInformation("Brass Ingot", Ingots.BrassIngot, "A brass alloy ingot made from copper and aluminum. It can be used for crafting purposes.", CategoryNames.Resource, Color.Silver, -300, -300, 0, false, 195, false, false, TextureManagers.createOreResourceAnimationManager("BrassIngot"), Color.White, true, new Vector2(1, 1), Vector2.Zero, null, null), 1);
            RevitalizeModCore.ObjectManager.addItem(Ingots.BrassIngot, brassIngot);

            CustomObject bronzeIngot = new CustomObject(new BasicItemInformation("Bronze Ingot", Ingots.BronzeIngot, "A bronze alloy ingot made from copper and tin. It can be used for crafting purposes.", CategoryNames.Resource, Color.Silver, -300, -300, 0, false, 150, false, false, TextureManagers.createOreResourceAnimationManager("BronzeIngot"), Color.White, true, new Vector2(1, 1), Vector2.Zero, null, null), 1);
            RevitalizeModCore.ObjectManager.addItem(Ingots.BronzeIngot, bronzeIngot);

            CustomObject electrumIngot = new CustomObject(new BasicItemInformation("Electrum Ingot", Ingots.ElectrumIngot, "A electrum alloy ingot made from gold and silver. It can be used for crafting purposes for things that use electricity.", CategoryNames.Resource, Color.Silver, -300, -300, 0, false, 500, false, false, TextureManagers.createOreResourceAnimationManager("ElectrumIngot"), Color.White, true, new Vector2(1, 1), Vector2.Zero, null, null), 1);
            RevitalizeModCore.ObjectManager.addItem(Ingots.ElectrumIngot, electrumIngot);

            CustomObject steelIngot = new CustomObject(new BasicItemInformation("Steel Ingot", Ingots.SteelIngot, "A steel ingot that was made by processing iron again with more coal. It can be used for crafting purposes especially for making new machines.", CategoryNames.Resource, Color.Silver, -300, -300, 0, false, 180, false, false, TextureManagers.createOreResourceAnimationManager("SteelIngot"), Color.White, true, new Vector2(1, 1), Vector2.Zero, null, null), 1);
            RevitalizeModCore.ObjectManager.addItem(Ingots.SteelIngot, steelIngot);

            CustomObject bauxiteSand = new CustomObject(new BasicItemInformation("Bauxite Sand", OreSands.BauxiteSand, "Bauxite ore which has been crushed into sand. Smelt it to get aluminum ingots.", CategoryNames.Resource, Color.Silver, -300, -300, 0, false, 11, false, false, TextureManagers.createOreResourceAnimationManager("BauxiteSand"), Color.White, true, new Vector2(1, 1), Vector2.Zero, null, null), 1);
            CustomObject copperSand = new CustomObject(new BasicItemInformation("Copper Sand", OreSands.CopperSand, "Copper ore which has been crushed into sand. Smelt it to get copper bars.", CategoryNames.Resource, Color.Silver, -300, -300, 0, false, 5, false, false, TextureManagers.createOreResourceAnimationManager("CopperSand"), Color.White, true, new Vector2(1, 1), Vector2.Zero, null, null), 1);
            CustomObject goldSand = new CustomObject(new BasicItemInformation("Gold Sand", OreSands.GoldSand, "Gold ore which has been crushed into sand. Smelt it to get gold bars.", CategoryNames.Resource, Color.Silver, -300, -300, 0, false, 25, false, false, TextureManagers.createOreResourceAnimationManager("GoldSand"), Color.White, true, new Vector2(1, 1), Vector2.Zero, null, null), 1);
            CustomObject ironSand = new CustomObject(new BasicItemInformation("Iron Sand", OreSands.IronSand, "Iron ore which has been crushed into sand. Smelt it to get iron bars.", CategoryNames.Resource, Color.Silver, -300, -300, 0, false, 10, false, false, TextureManagers.createOreResourceAnimationManager("IronSand"), Color.White, true, new Vector2(1, 1), Vector2.Zero, null, null), 1);
            CustomObject iridiumSand = new CustomObject(new BasicItemInformation("Iridium Sand", OreSands.IridiumSand, "Iridium ore which has been crushed into sand. Smelt it to get iridium bars.", CategoryNames.Resource, Color.Silver, -300, -300, 0, false, 100, false, false, TextureManagers.createOreResourceAnimationManager("IridiumSand"), Color.White, true, new Vector2(1, 1), Vector2.Zero, null, null), 1);
            CustomObject leadSand = new CustomObject(new BasicItemInformation("Lead Sand", OreSands.LeadSand, "Lead ore which has been crushed into sand. Smelt it to get lead ingots.", CategoryNames.Resource, Color.Silver, -300, -300, 0, false, 15, false, false, TextureManagers.createOreResourceAnimationManager("LeadSand"), Color.White, true, new Vector2(1, 1), Vector2.Zero, null, null), 1);
            CustomObject silverSand = new CustomObject(new BasicItemInformation("Silver Sand", OreSands.SilverSand, "Silver ore which has been crushed into sand. Smelt it to get silver ingots.", CategoryNames.Resource, Color.Silver, -300, -300, 0, false, 20, false, false, TextureManagers.createOreResourceAnimationManager("SilverSand"), Color.White, true, new Vector2(1, 1), Vector2.Zero, null, null), 1);
            CustomObject tinSand = new CustomObject(new BasicItemInformation("Tin Sand", OreSands.TinSand, "Tin ore which has been crushed into sand. Smelt it to get tin ingots.", CategoryNames.Resource, Color.Silver, -300, -300, 0, false, 7, false, false, TextureManagers.createOreResourceAnimationManager("TinSand"), Color.White, true, new Vector2(1, 1), Vector2.Zero, null, null), 1);
            CustomObject titaniumSand = new CustomObject(new BasicItemInformation("Copper Sand", OreSands.TitaniumSand, "Titanium ore which has been crushed into sand. Smelt it to get titanium bars.", CategoryNames.Resource, Color.Silver, -300, -300, 0, false, 35, false, false, TextureManagers.createOreResourceAnimationManager("TitaniumSand"), Color.White, true, new Vector2(1, 1), Vector2.Zero, null, null), 1);

            this.resources.Add(OreSands.BauxiteSand, bauxiteSand);
            this.resources.Add(OreSands.CopperSand, copperSand);
            this.resources.Add(OreSands.GoldSand, goldSand);
            this.resources.Add(OreSands.IronSand, ironSand);
            this.resources.Add(OreSands.IridiumSand, iridiumSand);
            this.resources.Add(OreSands.LeadSand, leadSand);
            this.resources.Add(OreSands.SilverSand, silverSand);
            this.resources.Add(OreSands.TinSand, tinSand);
            this.resources.Add(OreSands.TitaniumSand, titaniumSand);
        }



        private void loadInResourceItems()
        {
            CustomObject sand = new CustomObject(new BasicItemInformation("Sand", MiscEarthenResources.Sand, "Sand which is made from tiny rocks and can be used for smelting. Also unfun to have inside of swimwear.", CategoryNames.Resource, Color.Brown, -300, -300, 0, false, 2, false, false, TextureManagers.createMiscResourceAnimationManager("Sand"), Color.White, true, new Vector2(1, 1), Vector2.Zero, null, null));
            this.resources.Add(MiscEarthenResources.Sand, sand);

            CustomObject glass_normal = new CustomObject(new BasicItemInformation("Glass", MiscEarthenResources.Glass, "Glass smelted from sand. Used in decorations and glass objects.", CategoryNames.Resource, Color.Brown, -300, -300, 0, false, 20, false, false, TextureManagers.createMiscResourceAnimationManager("Glass"), Color.White, true, new Vector2(1, 1), Vector2.Zero, null, null));
            this.resources.Add(MiscEarthenResources.Glass, glass_normal);
        }

        public OreResourceInformation getOreResourceInfo(string id)
        {
            if (this.oreVeins.ContainsKey(id))
            {
                return (OreResourceInformation)this.oreVeins[id].resourceInfo;
            }
            else
            {
                return null;
            }
        }

        public List<ResourceInformation> getExtraDropInformationFromOres(string id)
        {
            if (this.oreVeins.ContainsKey(id))
            {
                return this.oreVeins[id].extraDrops.ToList();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Checks to see if a resource can be spawned here.
        /// </summary>
        /// <param name="OBJ"></param>
        /// <param name="Location"></param>
        /// <param name="TilePosition"></param>
        /// <returns></returns>
        public bool canResourceBeSpawnedHere(CustomObject OBJ, GameLocation Location, Vector2 TilePosition)
        {
            return OBJ.canBePlacedHere(Location, TilePosition) && Location.isTileLocationTotallyClearAndPlaceable(TilePosition);
        }


        //~~~~~~~~~~~~~~~~~~~~~~~//
        //  World Ore Spawn Code //
        //~~~~~~~~~~~~~~~~~~~~~~~//

        #region


        /// <summary>
        /// Spawns an ore vein at the given location if possible.
        /// </summary>
        /// <param name="name"></param>
        public bool spawnOreVein(string name, GameLocation Location, Vector2 TilePosition)
        {
            if (this.oreVeins.ContainsKey(name))
            {
                OreVein spawn;
                this.oreVeins.TryGetValue(name, out spawn);
                if (spawn != null)
                {
                    spawn = (OreVein)spawn.getOne();
                    bool spawnable = this.canResourceBeSpawnedHere(spawn, Location, TilePosition);
                    if (spawnable)
                    {
                        //ModCore.log("Location is: " + Location.Name);
                        spawn.placementAction(Location, (int)TilePosition.X * Game1.tileSize, (int)TilePosition.Y * Game1.tileSize, Game1.player);
                    }
                    else
                    {
                        RevitalizeModCore.log("Can't spawn ore: " + name + "at tile location: " + TilePosition);
                    }
                    return spawnable;
                }
                RevitalizeModCore.log("Key doesn't exist. Weird.");
                return false;
            }
            else
            {
                throw new Exception("The ore dictionary doesn't contain they key for resource: " + name);
            }
        }
        /// <summary>
        /// Spawns an orevein at the tile position at the same location as the player.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="TilePosition"></param>
        /// <returns></returns>
        public bool spawnOreVein(string name, Vector2 TilePosition)
        {
            return this.spawnOreVein(name, Game1.player.currentLocation, TilePosition);
        }

        /// <summary>
        /// Spawns ore in the mine depending on a lot of given variables such as floor level and spawn chance.
        /// </summary>
        public void spawnOreInMine()
        {
            int floorLevel = GameLocationUtilities.CurrentMineLevel();
            if (this.hasVisitedFloor(floorLevel))
            {
                //Already has spawned ores for this visit.
                return;
            }
            else
            {
                this.visitedFloors.Add(floorLevel);
            }
            List<OreVein> spawnableOreVeins = new List<OreVein>();
            //Get a list of all of the ores that can spawn on this mine level.
            foreach (KeyValuePair<string, OreVein> pair in this.oreVeins)
            {
                if (pair.Value.resourceInfo.Value.canSpawnAtLocation() && (pair.Value.resourceInfo.Value as OreResourceInformation).canSpawnOnCurrentMineLevel())
                {
                    spawnableOreVeins.Add(pair.Value);
                }
            }

            foreach (OreVein ore in spawnableOreVeins)
            {
                if (ore.resourceInfo.Value.shouldSpawn())
                {
                    int amount = ore.resourceInfo.Value.getNumberOfNodesToSpawn();
                    List<Vector2> openTiles = GameLocationUtilities.GetOpenObjectTiles(Game1.player.currentLocation, (OreVein)ore.getOne());
                    amount = Math.Min(amount, openTiles.Count); //Only spawn for as many open tiles or the amount of nodes to spawn.
                    for (int i = 0; i < amount; i++)
                    {
                        int position = Game1.random.Next(openTiles.Count);
                        bool didSpawn = this.spawnOreVein(ore.getItemInformation().id, openTiles[position]);
                        if (didSpawn == false)
                        {
                            i--; //If the tile didn't spawn due to some odd reason ensure that the amount is spawned.
                            openTiles.Remove(openTiles[position]);
                        }
                        else
                        {
                            openTiles.Remove(openTiles[position]); //Remove that tile from the list of open tiles.
                        }
                    }
                }
                else
                {
                    //Ore doesn't meet spawn chance.
                }
                //ModCore.log("Spawned :" + amount + " pancake test ores!");
            }

        }

        /// <summary>
        /// Checks to see if the player has visited the given floor.
        /// </summary>
        /// <param name="Floor"></param>
        /// <returns></returns>
        public bool hasVisitedFloor(int Floor)
        {
            return this.visitedFloors.Contains(Floor);
        }

        /// <summary>
        /// Source: SDV. 
        /// </summary>
        /// <param name="tileX"></param>
        /// <param name="tileY"></param>
        /// <returns></returns>
        private bool isTileOpenForQuarryStone(int tileX, int tileY)
        {
            GameLocation loc = GameLocationUtilities.GetGameLocation( Enums.StardewLocation.Mountain);
            if (loc.doesTileHaveProperty(tileX, tileY, "Diggable", "Back") != null)
                return loc.isTileLocationTotallyClearAndPlaceable(new Vector2((float)tileX, (float)tileY));
            return false;
        }

        /// <summary>
        /// Update the quarry every day with new ores to spawn.
        /// </summary>
        private void quarryDayUpdate()
        {
            List<OreVein> spawnableOreVeins = new List<OreVein>();
            //Get a list of all of the ores that can spawn on this mine level.
            foreach (KeyValuePair<string, OreVein> pair in this.oreVeins)
            {
                if ((pair.Value.resourceInfo.Value as OreResourceInformation).spawnsInQuarry)
                {
                    spawnableOreVeins.Add(pair.Value);
                    //ModCore.log("Found an ore that spawns in the quarry");
                }
            }
            foreach (OreVein ore in spawnableOreVeins)
            {
                if ((ore.resourceInfo.Value as OreResourceInformation).shouldSpawnInQuarry())
                {
                    int amount = (ore.resourceInfo.Value as OreResourceInformation).getNumberOfNodesToSpawnQuarry();
                    List<Vector2> openTiles = this.getOpenQuarryTiles(ore);
                    amount = Math.Min(amount, openTiles.Count); //Only spawn for as many open tiles or the amount of nodes to spawn.
                    for (int i = 0; i < amount; i++)
                    {
                        int position = Game1.random.Next(openTiles.Count);
                        bool didSpawn = this.spawnOreVein(ore.getItemInformation().id, GameLocationUtilities.GetGameLocation( Enums.StardewLocation.Mountain), openTiles[position]);
                        if (didSpawn == false)
                        {
                            i--; //If the tile didn't spawn due to some odd reason ensure that the amount is spawned.
                            openTiles.Remove(openTiles[position]);
                            //amount = Math.Min(amount, openTiles.Count); //Only spawn for as many open tiles or the amount of nodes to spawn.
                        }
                        else
                        {
                            //ModCore.log("Spawned ore in the quarry!");
                            openTiles.Remove(openTiles[position]); //Remove that tile from the list of open tiles.
                        }
                    }
                }
                else
                {
                    //Ore doesn't meet spawn chance.
                }
            }

        }

        /// <summary>
        /// Spawns ore in the mountain farm every day.
        /// </summary>
        public void mountainFarmDayUpdate()
        {
            if (GameLocationUtilities.Farm_IsFarmHiltopFarm() == false)
            {
                //ModCore.log("Farm is not hiltop farm!");
                return;
            }
            GameLocation farm = Game1.getFarm();

            List<OreVein> spawnableOreVeins = new List<OreVein>();
            //Get a list of all of the ores that can spawn on this mine level.
            foreach (KeyValuePair<string, OreVein> pair in this.oreVeins)
            {
                if ((pair.Value.resourceInfo.Value as OreResourceInformation).spawnsOnFarm)
                {
                    spawnableOreVeins.Add(pair.Value);
                    //ModCore.log("Found an ore that spawns on the farm!");
                }
            }
            foreach (OreVein ore in spawnableOreVeins)
            {
                if ((ore.resourceInfo.Value as OreResourceInformation).shouldSpawnOnFarm())
                {
                    int amount = (ore.resourceInfo.Value as OreResourceInformation).getNumberOfNodesToSpawnFarm();
                    List<Vector2> openTiles = this.getFarmQuarryOpenTiles(ore);
                    if (openTiles.Count == 0)
                    {
                        //ModCore.log("No open farm tiles!");
                    }
                    amount = Math.Min(amount, openTiles.Count); //Only spawn for as many open tiles or the amount of nodes to spawn.
                    for (int i = 0; i < amount; i++)
                    {
                        int position = Game1.random.Next(openTiles.Count);
                        bool didSpawn = this.spawnOreVein(ore.getItemInformation().id, farm, openTiles[position]);
                        if (didSpawn == false)
                        {
                            i--; //If the tile didn't spawn due to some odd reason ensure that the amount is spawned.
                            openTiles.Remove(openTiles[position]);
                            //amount = Math.Min(amount, openTiles.Count); //Only spawn for as many open tiles or the amount of nodes to spawn.
                            //ModCore.log("Did not spawn ore in the farm quarry!");
                        }
                        else
                        {
                            //ModCore.log("Spawned ore in the farm quarry!");
                            openTiles.Remove(openTiles[position]); //Remove that tile from the list of open tiles.
                        }
                    }
                }
                else
                {
                    //Ore doesn't meet spawn chance.
                }
            }

        }

        /// <summary>
        /// Gets a list of all of the open quarry tiles.
        /// </summary>
        /// <returns></returns>
        private List<Vector2> getOpenQuarryTiles(CustomObject obj)
        {
            List<Vector2> tiles = new List<Vector2>();
            Microsoft.Xna.Framework.Rectangle r = new Microsoft.Xna.Framework.Rectangle(106, 13, 21, 21);
            for (int i = r.X; i <= r.X + r.Width; i++)
            {
                for (int j = r.Y; j <= r.Y + r.Height; j++)
                {
                    if (this.isTileOpenForQuarryStone(i, j) && this.canResourceBeSpawnedHere(obj, GameLocationUtilities.GetGameLocation( Enums.StardewLocation.Mountain), new Vector2(i, j)))
                    {
                        tiles.Add(new Vector2(i, j));
                    }
                }
            }
            if (tiles.Count == 0)
            {
                //ModCore.log("Quarry is full! Can't spawn more resources!");
            }
            return tiles;
        }

        /// <summary>
        /// Gets all of the open tiles in the farm quarry.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private List<Vector2> getFarmQuarryOpenTiles(CustomObject obj)
        {
            List<Vector2> tiles = new List<Vector2>();
            Microsoft.Xna.Framework.Rectangle r = new Microsoft.Xna.Framework.Rectangle(5, 37, 22, 8);
            GameLocation farm = Game1.getFarm();
            for (int i = r.X; i <= r.X + r.Width; i++)
            {
                for (int j = r.Y; j <= r.Y + r.Height; j++)
                {
                    Vector2 pos = new Vector2(i, j);
                    if (farm.doesTileHavePropertyNoNull((int)pos.X, (int)pos.Y, "Type", "Back").Equals("Dirt") && this.canResourceBeSpawnedHere(obj, farm, new Vector2(i, j)))
                    {
                        tiles.Add(pos);
                    }
                }
            }
            if (tiles.Count == 0)
            {
                //ModCore.log("Quarry is full! Can't spawn more resources!");
            }
            return tiles;
        }

        #endregion


        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        //          SMAPI Events       //
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

        #region
        /// <summary>
        /// What happens when the player warps maps.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="playerWarped"></param>
        public void OnPlayerLocationChanged(object o, EventArgs playerWarped)
        {
            this.spawnOreInMine();
            if (GameLocationUtilities.IsPlayerInMine() == false && GameLocationUtilities.IsPlayerInSkullCave() == false && GameLocationUtilities.IsPlayerInMineEnterance() == false)
            {
                this.visitedFloors.Clear();
            }
        }

        /// <summary>
        /// Triggers at the start of every new day to populate the world full of ores.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="NewDay"></param>
        public void DailyResourceSpawn(object o, EventArgs NewDay)
        {
            this.mountainFarmDayUpdate();
            this.quarryDayUpdate();
        }
        #endregion

    }
}
