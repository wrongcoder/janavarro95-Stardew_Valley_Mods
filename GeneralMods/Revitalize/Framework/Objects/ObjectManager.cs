using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Revitalize.Framework.Objects.CraftingTables;
using Revitalize.Framework.Objects.Extras;
using Revitalize.Framework.Objects.Furniture;
using Revitalize.Framework.Objects.Interfaces;
using Revitalize.Framework.Objects.Items.Tools;
using Revitalize.Framework.Objects.Machines;
using Revitalize.Framework.Utilities;
using StardewModdingAPI;
using StardewValley;
using StardustCore.Animations;
using StardustCore.UIUtilities;

namespace Revitalize.Framework.Objects
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

        /// <summary>
        /// All of the chairs held by this object pool.
        /// </summary>
        public Dictionary<string, ChairMultiTiledObject> chairs;
        /// <summary>
        /// All of the tables held by this object pool.
        /// </summary>
        public Dictionary<string, TableMultiTiledObject> tables;
        /// <summary>
        /// All of the lamps held by this object pool.
        /// </summary>
        public Dictionary<string, LampMultiTiledObject> lamps;
        /// <summary>
        /// All of the rugs held by this object pool.
        /// </summary>
        public Dictionary<string, RugMultiTiledObject> rugs;
        public Dictionary<string, StorageFurnitureOBJ> furnitureStorage;

        public Dictionary<string, MultiTiledObject> generic;
        /// <summary>
        /// Misc. items for this mod.
        /// </summary>
        public Dictionary<string, MultiTiledObject> miscellaneous;

        public ResourceManager resources;

        public Dictionary<string, CustomObject> ItemsByName;

        public Dictionary<string, StardewValley.Tool> Tools;

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
            this.chairs = new Dictionary<string, ChairMultiTiledObject>();
            this.tables = new Dictionary<string, TableMultiTiledObject>();
            this.lamps = new Dictionary<string, LampMultiTiledObject>();
            this.rugs = new Dictionary<string, RugMultiTiledObject>();
            this.furnitureStorage = new Dictionary<string, StorageFurnitureOBJ>();

            this.generic = new Dictionary<string, MultiTiledObject>();
            this.miscellaneous = new Dictionary<string, MultiTiledObject>();

            this.resources = new ResourceManager();
            this.ItemsByName = new Dictionary<string, CustomObject>();

            this.Tools = new Dictionary<string, Tool>();
        }

        /// <summary>
        /// Loads in the items for the object and resource managers.
        /// </summary>
        public void loadInItems()
        {
            this.resources.loadInItems(); //Must be first.
            this.loadInCraftingTables();
            this.loadInMachines();
            this.loadInTools();
        }

        private void loadInCraftingTables()
        {
            MultiTiledObject WorkbenchObj = new MultiTiledObject(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Objects.Crafting.Workbench", TextureManager.GetTexture(ModCore.Manifest, "Objects.Crafting", "Workbench"), typeof(MultiTiledObject), Color.White, true), new BasicItemInformation("Workbench", "Omegasis.Revitalize.Objects.Crafting.Workbench", "A workbench that can be used for crafting different objects.", "Crafting", Color.Brown, -300, 0, false, 500, true, true, TextureManager.GetTexture(ModCore.Manifest, "Objects.Crafting", "Workbench"), new AnimationManager(), Color.White, false, null, null));
            CraftingTableTile workbenchTile_0_0 = new CraftingTableTile(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Objects.Crafting.Workbench", TextureManager.GetTexture(ModCore.Manifest, "Objects.Crafting", "Workbench"), typeof(CraftingTableTile), Color.White, true), new BasicItemInformation("Workbench", "Omegasis.Revitalize.Objects.Crafting.Workbench", "A workbench that can be used for crafting different objects.", "Crafting", Color.Brown, -300, 0, false, 500, true, true, TextureManager.GetTexture(ModCore.Manifest, "Objects.Crafting", "Workbench"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Objects.Crafting", "Workbench"), new Animation(0, 0, 16, 16)), Color.White, false, null, null), "Workbench");
            CraftingTableTile workbenchTile_1_0 = new CraftingTableTile(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Objects.Crafting.Workbench", TextureManager.GetTexture(ModCore.Manifest, "Objects.Crafting", "Workbench"), typeof(CraftingTableTile), Color.White, true), new BasicItemInformation("Workbench", "Omegasis.Revitalize.Objects.Crafting.Workbench", "A workbench that can be used for crafting different objects.", "Crafting", Color.Brown, -300, 0, false, 500, true, true, TextureManager.GetTexture(ModCore.Manifest, "Objects.Crafting", "Workbench"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Objects.Crafting", "Workbench"), new Animation(16, 0, 16, 16)), Color.White, false, null, null), "Workbench");
            CraftingTableTile workbenchTile_0_1 = new CraftingTableTile(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Objects.Crafting.Workbench", TextureManager.GetTexture(ModCore.Manifest, "Objects.Crafting", "Workbench"), typeof(CraftingTableTile), Color.White, true), new BasicItemInformation("Workbench", "Omegasis.Revitalize.Objects.Crafting.Workbench", "A workbench that can be used for crafting different objects.", "Crafting", Color.Brown, -300, 0, false, 500, true, true, TextureManager.GetTexture(ModCore.Manifest, "Objects.Crafting", "Workbench"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Objects.Crafting", "Workbench"), new Animation(0, 16, 16, 16)), Color.White, false, null, null), "Workbench");
            CraftingTableTile workbenchTile_1_1 = new CraftingTableTile(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Objects.Crafting.Workbench", TextureManager.GetTexture(ModCore.Manifest, "Objects.Crafting", "Workbench"), typeof(CraftingTableTile), Color.White, true), new BasicItemInformation("Workbench", "Omegasis.Revitalize.Objects.Crafting.Workbench", "A workbench that can be used for crafting different objects.", "Crafting", Color.Brown, -300, 0, false, 500, true, true, TextureManager.GetTexture(ModCore.Manifest, "Objects.Crafting", "Workbench"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Objects.Crafting", "Workbench"), new Animation(16, 16, 16, 16)), Color.White, false, null, null), "Workbench");
            WorkbenchObj.addComponent(new Vector2(0, 0), workbenchTile_0_0);
            WorkbenchObj.addComponent(new Vector2(1, 0), workbenchTile_1_0);
            WorkbenchObj.addComponent(new Vector2(0, 1), workbenchTile_0_1);
            WorkbenchObj.addComponent(new Vector2(1, 1), workbenchTile_1_1);

            MultiTiledObject AnvilObj = new MultiTiledObject(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Objects.Crafting.Anvil", TextureManager.GetTexture(ModCore.Manifest, "Objects.Crafting", "Anvil"), typeof(MultiTiledObject), Color.White, true), new BasicItemInformation("Anvil", "Omegasis.Revitalize.Objects.Crafting.Anvil", "An anvil that can be used for crafting different machines and other metalic objects.", "Crafting", Color.Brown, -300, 0, false, 2000, true, true, TextureManager.GetTexture(ModCore.Manifest, "Objects.Crafting", "Anvil"), new AnimationManager(), Color.White, false, null, null));
            CraftingTableTile anvilTile_0_0 = new CraftingTableTile(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Objects.Crafting.Anvil", TextureManager.GetTexture(ModCore.Manifest, "Objects.Crafting", "Anvil"), typeof(CraftingTableTile), Color.White, true), new BasicItemInformation("Anvil", "Omegasis.Revitalize.Objects.Crafting.Anvil", "An anvil that can be used for crafting different machines and other metalic objects.", "Crafting", Color.Brown, -300, 0, false, 2000, true, true, TextureManager.GetTexture(ModCore.Manifest, "Objects.Crafting", "Anvil"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Objects.Crafting", "Anvil"), new Animation(0, 0, 16, 16)), Color.White, false, null, null), "Anvil");
            CraftingTableTile anvilTile_1_0 = new CraftingTableTile(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Objects.Crafting.Anvil", TextureManager.GetTexture(ModCore.Manifest, "Objects.Crafting", "Anvil"), typeof(CraftingTableTile), Color.White, true), new BasicItemInformation("Anvil", "Omegasis.Revitalize.Objects.Crafting.Anvil", "An anvil that can be used for crafting different machines and other metalic objects.", "Crafting", Color.Brown, -300, 0, false, 2000, true, true, TextureManager.GetTexture(ModCore.Manifest, "Objects.Crafting", "Anvil"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Objects.Crafting", "Anvil"), new Animation(16, 0, 16, 16)), Color.White, false, null, null), "Anvil");
            CraftingTableTile anvilTile_0_1 = new CraftingTableTile(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Objects.Crafting.Anvil", TextureManager.GetTexture(ModCore.Manifest, "Objects.Crafting", "Anvil"), typeof(CraftingTableTile), Color.White, true), new BasicItemInformation("Anvil", "Omegasis.Revitalize.Objects.Crafting.Anvil", "An anvil that can be used for crafting different machines and other metalic objects.", "Crafting", Color.Brown, -300, 0, false, 2000, true, true, TextureManager.GetTexture(ModCore.Manifest, "Objects.Crafting", "Anvil"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Objects.Crafting", "Anvil"), new Animation(0, 16, 16, 16)), Color.White, false, null, null), "Anvil");
            CraftingTableTile anvilTile_1_1 = new CraftingTableTile(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Objects.Crafting.Anvil", TextureManager.GetTexture(ModCore.Manifest, "Objects.Crafting", "Anvil"), typeof(CraftingTableTile), Color.White, true), new BasicItemInformation("Anvil", "Omegasis.Revitalize.Objects.Crafting.Anvil", "An anvil that can be used for crafting different machines and other metalic objects.", "Crafting", Color.Brown, -300, 0, false, 2000, true, true, TextureManager.GetTexture(ModCore.Manifest, "Objects.Crafting", "Anvil"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Objects.Crafting", "Anvil"), new Animation(16, 16, 16, 16)), Color.White, false, null, null), "Anvil");
            AnvilObj.addComponent(new Vector2(0, 0), anvilTile_0_0);
            AnvilObj.addComponent(new Vector2(1, 0), anvilTile_1_0);
            AnvilObj.addComponent(new Vector2(0, 1), anvilTile_0_1);
            AnvilObj.addComponent(new Vector2(1, 1), anvilTile_1_1);

            this.AddItem("Workbench", WorkbenchObj);
            this.AddItem("Anvil", AnvilObj);
        }

        private void loadInMachines()
        {
            MultiTiledObject trashCan = new MultiTiledObject(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Furniture.Misc.TrashCan", TextureManager.GetTexture(ModCore.Manifest, "Furniture", "TrashCan"), typeof(MultiTiledObject), Color.White, true), new BasicItemInformation("Trash Can", "Omegasis.Revitalize.Furniture.Misc.TrashCan", "A trash can where you can throw away unnecessary objects. It empties out at the beginning of each new day.", "Machine", Color.SteelBlue, -300, 0, false, 650, true, true, TextureManager.GetTexture(ModCore.Manifest, "Furniture", "TrashCan"), new AnimationManager(), Color.White, true, new InventoryManager(36), null, null));
            TrashCanTile trash1 = new TrashCanTile(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Furniture.Misc.TrashCan", TextureManager.GetTexture(ModCore.Manifest, "Furniture", "TrashCan"), typeof(TrashCanTile), Color.White, true), new BasicItemInformation("Trash Can", "Omegasis.Revitalize.Furniture.Misc.TrashCan", "A trash can where you can throw away unnecessary objects. It empties out at the beginning of each new day.", "Machine", Color.SteelBlue, -300, 0, false, 650, true, true, TextureManager.GetTexture(ModCore.Manifest, "Furniture", "TrashCan"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Furniture", "TrashCan"), new Animation(0, 0, 16, 16)), Color.White, true, new InventoryManager(36), null, null));
            TrashCanTile trash2 = new TrashCanTile(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Furniture.Misc.TrashCan", TextureManager.GetTexture(ModCore.Manifest, "Furniture", "TrashCan"), typeof(TrashCanTile), Color.White, true), new BasicItemInformation("Trash Can", "Omegasis.Revitalize.Furniture.Misc.TrashCan", "A trash can where you can throw away unnecessary objects. It empties out at the beginning of each new day.", "Machine", Color.SteelBlue, -300, 0, false, 650, true, true, TextureManager.GetTexture(ModCore.Manifest, "Furniture", "TrashCan"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Furniture", "TrashCan"), new Animation(0, 16, 16, 16)), Color.White, false, new InventoryManager(36), null, null));
            trashCan.addComponent(new Vector2(0, 0), trash1);
            trashCan.addComponent(new Vector2(0, 1), trash2);

            this.AddItem("TrashCan", trashCan);



            MultiTiledObject sandBox = new MultiTiledObject(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Objects.Machines.Sandbox", TextureManager.GetTexture(ModCore.Manifest, "Machines", "Sandbox"), typeof(MultiTiledObject), Color.White, true), new BasicItemInformation("Sandbox", "Omegasis.Revitalize.Objects.Machines.Sandbox", "A sandbox which slowly produces sand. Unfortunately you can't sit in this one.", "Machine", Color.SteelBlue, -300, 0, false, 750, true, true, TextureManager.GetTexture(ModCore.Manifest, "Machines", "Sandbox"), new AnimationManager(), Color.White, true, new InventoryManager(36), null, null));
            Machine sandBox_0_0 = new Machine(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Objects.Machines.Sandbox", TextureManager.GetTexture(ModCore.Manifest, "Machines", "Sandbox"), typeof(Machine), Color.White, true), new BasicItemInformation("Sandbox", "Omegasis.Revitalize.Objects.Machines.Sandbox", "A sandbox which slowly produces sand. Unfortunately you can't sit in this one.", "Machine", Color.SteelBlue, -300, 0, false, 750, true, true, TextureManager.GetTexture(ModCore.Manifest, "Machines", "Sandbox"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Machines", "Sandbox"),new Animation(0,0,16,16)), Color.White, false, new InventoryManager(36), null, null), new List<InformationFiles.ResourceInformation>()
            {
                //new InformationFiles.ResourceInformation(this.resources.getResource("Sand",1),1,1,1,1,1,1,0,0,0,0)

            }, 0, TimeUtilities.GetMinutesFromTime(0, 1, 0), true,"Workbench");
            Machine sandBox_1_0 = new Machine(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Objects.Machines.Sandbox", TextureManager.GetTexture(ModCore.Manifest, "Machines", "Sandbox"), typeof(Machine), Color.White, true), new BasicItemInformation("Sandbox", "Omegasis.Revitalize.Objects.Machines.Sandbox", "A sandbox which slowly produces sand. Unfortunately you can't sit in this one.", "Machine", Color.SteelBlue, -300, 0, false, 750, true, true, TextureManager.GetTexture(ModCore.Manifest, "Machines", "Sandbox"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Machines", "Sandbox"), new Animation(16, 0, 16, 16)), Color.White, false, new InventoryManager(36), null, null), new List<InformationFiles.ResourceInformation>()
            {
                //new InformationFiles.ResourceInformation(this.resources.getResource("Sand",1),1,1,1,1,1,1,0,0,0,0)

            }, 0, TimeUtilities.GetMinutesFromTime(0, 1, 0), false, "Workbench");
            Machine sandBox_0_1 = new Machine(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Objects.Machines.Sandbox", TextureManager.GetTexture(ModCore.Manifest, "Machines", "Sandbox"), typeof(Machine), Color.White, true), new BasicItemInformation("Sandbox", "Omegasis.Revitalize.Objects.Machines.Sandbox", "A sandbox which slowly produces sand. Unfortunately you can't sit in this one.", "Machine", Color.SteelBlue, -300, 0, false, 750, true, true, TextureManager.GetTexture(ModCore.Manifest, "Machines", "Sandbox"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Machines", "Sandbox"), new Animation(0, 16, 16, 16)), Color.White, false, new InventoryManager(36), null, null), new List<InformationFiles.ResourceInformation>()
            {
                //new InformationFiles.ResourceInformation(this.resources.getResource("Sand",1),1,1,1,1,1,1,0,0,0,0)

            }, 0, TimeUtilities.GetMinutesFromTime(0,1,0), false, "Workbench");
            Machine sandBox_1_1 = new Machine(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Objects.Machines.Sandbox", TextureManager.GetTexture(ModCore.Manifest, "Machines", "Sandbox"), typeof(Machine), Color.White, true), new BasicItemInformation("Sandbox", "Omegasis.Revitalize.Objects.Machines.Sandbox", "A sandbox which slowly produces sand. Unfortunately you can't sit in this one.", "Machine", Color.SteelBlue, -300, 0, false, 750, true, true, TextureManager.GetTexture(ModCore.Manifest, "Machines", "Sandbox"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Machines", "Sandbox"), new Animation(16, 16, 16, 16)), Color.White, false, new InventoryManager(36), null, null), new List<InformationFiles.ResourceInformation>()
            {
                //new InformationFiles.ResourceInformation(this.resources.getResource("Sand",1),1,1,1,1,1,1,0,0,0,0)

            }, 0, TimeUtilities.GetMinutesFromTime(0, 1, 0), false, "Workbench");

            sandBox.addComponent(new Vector2(0,0),sandBox_0_0);
            sandBox.addComponent(new Vector2(1, 0), sandBox_1_0);
            sandBox.addComponent(new Vector2(0, 1), sandBox_0_1);
            sandBox.addComponent(new Vector2(1, 1), sandBox_1_1);

            this.AddItem("SandBox", sandBox);

        }

        private void loadInTools()
        {
            PickaxeExtended bronzePick = new PickaxeExtended(new BasicItemInformation("Bronze Pickaxe", "Omegasis.Revitalize.Items.Tools.BronzePickaxe", "A sturdy pickaxe made from bronze.", "Tool", Color.SlateGray, 0, 0, false, 500, false, false, TextureManager.GetTexture(ModCore.Manifest, "Tools", "BronzePickaxe"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "BronzePickaxe"), new Animation(0, 0, 16, 16)), Color.White, true, null, null), 2, TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "BronzePickaxeWorking"));
            PickaxeExtended steelPick = new PickaxeExtended(new BasicItemInformation("Hardened Pickaxe", "Omegasis.Revitalize.Items.Tools.HardenedPickaxe", "A sturdy pickaxe made from hardened alloy.", "Tool", Color.SlateGray, 0, 0, false, 500, false, false, TextureManager.GetTexture(ModCore.Manifest, "Tools", "HardenedPickaxe"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "HardenedPickaxe"), new Animation(0, 0, 16, 16)), Color.White, true, null, null), 3, TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "HardenedPickaxeWorking"));
            PickaxeExtended titaniumPick = new PickaxeExtended(new BasicItemInformation("Titanium Pickaxe", "Omegasis.Revitalize.Items.Tools.TitaniumPickaxe", "A sturdy pickaxe made from titanium.", "Tool", Color.SlateGray, 0, 0, false, 500, false, false, TextureManager.GetTexture(ModCore.Manifest, "Tools", "TitaniumPickaxe"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "TitaniumPickaxe"), new Animation(0, 0, 16, 16)), Color.White, true, null, null), 4, TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "TitaniumPickaxeWorking"));

            AxeExtended bronzeAxe= new AxeExtended(new BasicItemInformation("Bronze Axe", "Omegasis.Revitalize.Items.Tools.BronzeAxe", "A sturdy axe made from bronze.", "Tool", Color.SlateGray, 0, 0, false, 500, false, false, TextureManager.GetTexture(ModCore.Manifest, "Tools", "BronzeAxe"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "BronzeAxe"), new Animation(0, 0, 16, 16)), Color.White, true, null, null), 2, TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "BronzeAxeWorking"));
            AxeExtended steelAxe = new AxeExtended(new BasicItemInformation("Hardened Axe", "Omegasis.Revitalize.Items.Tools.HardenedAxe", "A sturdy axe made from hardened alloy.", "Tool", Color.SlateGray, 0, 0, false, 500, false, false, TextureManager.GetTexture(ModCore.Manifest, "Tools", "HardenedAxe"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "HardenedAxe"), new Animation(0, 0, 16, 16)), Color.White, true, null, null),3,TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "HardenedAxeWorking"));
            AxeExtended titaniumAxe = new AxeExtended(new BasicItemInformation("Titanium Axe", "Omegasis.Revitalize.Items.Tools.TitaniumAxe", "A sturdy axe made from Titanium.", "Tool", Color.SlateGray, 0, 0, false, 500, false, false, TextureManager.GetTexture(ModCore.Manifest, "Tools", "TitaniumAxe"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "TitaniumAxe"), new Animation(0, 0, 16, 16)), Color.White, true, null, null), 4, TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "TitaniumAxeWorking"));

            HoeExtended bronzeHoe = new HoeExtended(new BasicItemInformation("Bronze Hoe", "Omegasis.Revitalize.Items.Tools.BronzeHoe", "A sturdy hoe made from bronze.", "Tool", Color.SlateGray, 0, 0, false, 500, false, false, TextureManager.GetTexture(ModCore.Manifest, "Tools", "BronzeHoe"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "BronzeHoe"), new Animation(0, 0, 16, 16)), Color.White, true, null, null), 2, TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "BronzeHoeWorking"));
            HoeExtended steelHoe = new HoeExtended(new BasicItemInformation("Hardened Hoe", "Omegasis.Revitalize.Items.Tools.HardenedHoe", "A sturdy hoe made from hardened alloy.", "Tool", Color.SlateGray, 0, 0, false, 500, false, false, TextureManager.GetTexture(ModCore.Manifest, "Tools", "HardenedHoe"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "HardenedHoe"), new Animation(0, 0, 16, 16)), Color.White, true, null, null), 3, TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "HardenedHoeWorking"));
            HoeExtended titaniumHoe = new HoeExtended(new BasicItemInformation("Titanium Hoe", "Omegasis.Revitalize.Items.Tools.TitaniumHoe", "A sturdy hoe made from titanium.", "Tool", Color.SlateGray, 0, 0, false, 500, false, false, TextureManager.GetTexture(ModCore.Manifest, "Tools", "TitaniumHoe"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "TitaniumHoe"), new Animation(0, 0, 16, 16)), Color.White, true, null, null), 4, TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "TitaniumHoeWorking"));

            WateringCanExtended bronzeCan = new WateringCanExtended(new BasicItemInformation("Bronze Watering Can", "Omegasis.Revitalize.Items.Tools.BronzeWateringCan", "A sturdy watering can made from bronze.", "Tool", Color.SlateGray, 0, 0, false, 500, false, false, TextureManager.GetTexture(ModCore.Manifest, "Tools", "BronzeWateringCan"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "BronzeWateringCan"), new Animation(0, 0, 16, 16)), Color.White, true, null, null), 1, TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "BronzeWateringCanWorking"),70);
            WateringCanExtended steelCan = new WateringCanExtended(new BasicItemInformation("Hardened Watering Can", "Omegasis.Revitalize.Items.Tools.HardenedWateringCan", "A sturdy watering can made from hardened alloy.", "Tool", Color.SlateGray, 0, 0, false, 500, false, false, TextureManager.GetTexture(ModCore.Manifest, "Tools", "HardenedWateringCan"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "HardenedWateringCan"), new Animation(0, 0, 16, 16)), Color.White, true, null, null), 2, TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "HardenedWateringCanWorking"), 100);
            WateringCanExtended titaniumCan = new WateringCanExtended(new BasicItemInformation("Titanium Watering Can", "Omegasis.Revitalize.Items.Tools.TitaniumWateringCan", "A sturdy watering can made from titanium.", "Tool", Color.SlateGray, 0, 0, false, 500, false, false, TextureManager.GetTexture(ModCore.Manifest, "Tools", "TitaniumWateringCan"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "TitaniumWateringCan"), new Animation(0, 0, 16, 16)), Color.White, true, null, null), 3, TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "TitaniumWateringCanWorking"), 125);

            this.Tools.Add("BronzePickaxe", bronzePick);
            this.Tools.Add("HardenedPickaxe", steelPick);
            this.Tools.Add("TitaniumPickaxe", titaniumPick);

            this.Tools.Add("BronzeAxe", bronzeAxe);
            this.Tools.Add("HardenedAxe", steelAxe);
            this.Tools.Add("TitaniumAxe", titaniumAxe);

            this.Tools.Add("BronzeHoe", bronzeHoe);
            this.Tools.Add("HardenedHoe", steelHoe);
            this.Tools.Add("TitaniumHoe", titaniumHoe);

            this.Tools.Add("BronzeWateringCan", bronzeCan);
            this.Tools.Add("HardenedWateringCan", steelCan);
            this.Tools.Add("TitaniumWateringCan", titaniumCan);
        }

        /// <summary>
        /// Gets a random object from the dictionary passed in.
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public Item getRandomObject(Dictionary<string,CustomObject> dictionary)
        {
            if (dictionary.Count == 0) return null;
            List<CustomObject> objs = new List<CustomObject>();
            foreach(KeyValuePair<string,CustomObject> pair in dictionary)
            {
                objs.Add(pair.Value);
            }
            int rand = Game1.random.Next(0,objs.Count);
            return objs[rand].getOne();
        }

        
        /// <summary>
        /// Gets an object from the dictionary that is passed in.
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public Item getObject(string objectName, Dictionary<string,CustomObject> dictionary)
        {
            if (dictionary.ContainsKey(objectName))
            {
                return dictionary[objectName].getOne();
            }
            else
            {
                throw new Exception("Object pool doesn't contain said object.");
            }
        }
        

        public Item getObject(string objectName, Dictionary<string, MultiTiledObject> dictionary)
        {
            if (dictionary.ContainsKey(objectName))
            {
                return dictionary[objectName].getOne();
            }
            else
            {
                throw new Exception("Object pool doesn't contain said object.");
            }
        }

        /// <summary>
        /// Gets a chair from the object manager.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ChairMultiTiledObject getChair(string name)
        {
            if (this.chairs.ContainsKey(name))
            {
                return (ChairMultiTiledObject)this.chairs[name].getOne();
            }
            else
            {
                throw new Exception("Object pool doesn't contain said object.");
            }
        }
        /// <summary>
        /// Gets a table from the object manager.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TableMultiTiledObject getTable(string name)
        {
            if (this.tables.ContainsKey(name))
            {
                return (TableMultiTiledObject)this.tables[name].getOne();
            }
            else
            {
                throw new Exception("Object pool doesn't contain said object.");
            }
        }

        /// <summary>
        /// Gets a lamp from the object manager.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public LampMultiTiledObject getLamp(string name)
        {
            if (this.lamps.ContainsKey(name))
            {
                return (LampMultiTiledObject)this.lamps[name].getOne();
            }
            else
            {
                throw new Exception("Object pool doesn't contain said object.");
            }
        }

        /// <summary>
        /// Gets storage furniture from the object manager.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public StorageFurnitureOBJ getStorageFuriture(string name)
        {
            if (this.furnitureStorage.ContainsKey(name))
            {
                return (StorageFurnitureOBJ)this.furnitureStorage[name].getOne();
            }
            else
            {
                throw new Exception("Object pool doesn't contain said object.");
            }
        }

        /// <summary>
        /// Adds in an item to be tracked by the mod's object manager.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="I"></param>
        public void AddItem(string key, CustomObject I)
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
        public CustomObject GetItem(string Key,int Stack=1)
        {
            if (this.ItemsByName.ContainsKey(Key))
            {
                Item I= this.ItemsByName[Key].getOne();
                I.Stack = Stack;
                return (CustomObject)I;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a tool from the list of managed tools.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public Item GetTool(string Name)
        {
            if (this.Tools.ContainsKey(Name)) return this.Tools[Name].getOne();
            else return null;
        }

        /// <summary>
        /// Adds a new object manager to the master pool of managers.
        /// </summary>
        /// <param name="Manifest"></param>
        public static void AddObjectManager(IManifest Manifest)
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

        /// <summary>
        /// Scans all mod items to try to find a match.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="T"></param>
        /// <returns></returns>
        public Item getItemByIDAndType(string ID,Type T)
        {

            foreach(var v in this.chairs)
            {
                if(v.Value.GetType()==T && v.Value.info.id == ID)
                {
                   Item I= v.Value.getOne();
                    return I;
                }
            }

            foreach(var v in this.furnitureStorage)
            {
                if (v.Value.GetType() == T && v.Value.info.id == ID)
                {
                    Item I = v.Value.getOne();
                    return I;
                }
            }

            foreach(var v in this.generic)
            {
                if (v.Value.GetType() == T && v.Value.info.id == ID)
                {
                    Item I = v.Value.getOne();
                    return I;
                }
            }

            foreach(var v in this.ItemsByName)
            {
                if (v.Value.GetType() == T && v.Value.info.id == ID)
                {
                    Item I = v.Value.getOne();
                    return I;
                }
            }
            foreach(var v in this.lamps)
            {
                if (v.Value.GetType() == T && v.Value.info.id == ID)
                {
                    Item I = v.Value.getOne();
                    return I;
                }
            }
            foreach(var v in this.miscellaneous)
            {
                if (v.Value.GetType() == T && v.Value.info.id == ID)
                {
                    Item I = v.Value.getOne();
                    return I;
                }
            }
            foreach(var v in this.rugs)
            {
                if (v.Value.GetType() == T && v.Value.info.id == ID)
                {
                    Item I = v.Value.getOne();
                    return I;
                }
            }
            foreach(var v in this.tables)
            {
                if (v.Value.GetType() == T && v.Value.info.id == ID)
                {
                    Item I = v.Value.getOne();
                    return I;
                }
            }

            foreach(var v in this.resources.ores)
            {
                if (v.Value.GetType() == T && v.Value.info.id == ID)
                {
                    Item I = v.Value.getOne();
                    return I;
                }

            }
            foreach(var v in this.resources.oreVeins)
            {
                if (v.Value.GetType() == T && v.Value.info.id == ID)
                {
                    Item I = v.Value.getOne();
                    return I;
                }
            }
            foreach(var v in this.Tools)
            {
                if (v.Value.GetType() == T && (v.Value as IItemInfo).Info.id == ID)
                {
                    Item I = v.Value.getOne();
                    return I;
                }
            }

            return null;
        }

    }
}
