using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Network;
using StardustCore.Menus;
using StardustCore.ModInfo;
using StardustCore.Objects;
using StardustCore.Objects.Tools;
using StardustCore.Serialization;
using StardustCore.UIUtilities;
using StardustCore.UIUtilities.SpriteFonts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StardustCore
{
    public class ModCore : Mod
    {
        public static IModHelper ModHelper;
        public static IMonitor ModMonitor;
        public static IManifest Manifest;
        public static Serialization.SerializationManager SerializationManager;
        public static UIUtilities.TextureManager TextureManager;
        public static Dictionary<string, TextureManager> TextureManagers;

        

        public static Multiplayer multiplayer;
        bool serverHack;

        private Type lastMenuType;

        public ModConfig config;

        public static string ContentDirectory;
        public override void Entry(IModHelper helper)
        {
            ModHelper = Helper;
            ModMonitor = Monitor;
            Manifest = ModManifest;
            //Unused MetaData information. Works in player inventory but not in chests. Besides who really care where an object is from anyways. Also doesn't work 100% like I intended since it only gets base mod object that this runs from, not extensions?

            //  StardewModdingAPI.Events.GraphicsEvents.OnPostRenderGuiEvent += Metadata.GameEvents_UpdateTick;
            //StardewModdingAPI.Events.ControlEvents.MouseChanged += ControlEvents_MouseChanged;
            string invPath = Path.Combine(ModCore.ModHelper.DirectoryPath, "PlayerData", Game1.player.Name, "PlayerInventory");
            string worldPath = Path.Combine(ModCore.ModHelper.DirectoryPath, Game1.player.Name, "ObjectsInWorld"); ;
            string trashPath = Path.Combine(ModCore.ModHelper.DirectoryPath, "ModTrashFolder");
            string chestPath = Path.Combine(ModCore.ModHelper.DirectoryPath, "StorageContainers");
            SerializationManager = new SerializationManager(invPath, trashPath, worldPath,chestPath);

            StardewModdingAPI.Events.SaveEvents.AfterSave += SaveEvents_AfterSave;
            StardewModdingAPI.Events.SaveEvents.BeforeSave += SaveEvents_BeforeSave;
            StardewModdingAPI.Events.SaveEvents.AfterLoad += SaveEvents_AfterLoad;
            StardewModdingAPI.Events.MenuEvents.MenuChanged += MenuEvents_MenuChanged;
            StardewModdingAPI.Events.MenuEvents.MenuClosed += MenuEvents_MenuClosed;

            IlluminateFramework.Colors.initializeColors();
            ContentDirectory = "Content";
            if (!Directory.Exists(ContentDirectory)) Directory.CreateDirectory(Path.Combine(ModHelper.DirectoryPath, "Content"));
            SpriteFonts.initialize();

            SerializationManager.initializeDefaultSuportedTypes();
            TextureManagers = new Dictionary<string, TextureManager>();
            TextureManager = new TextureManager();
            TextureManager.addTexture("Test1.png", new Texture2DExtended(ModCore.ModHelper, Manifest,Path.Combine("Content", "Graphics", "MultiTest", "Test1.png")));
            TextureManagers.Add(ModManifest.UniqueID, TextureManager);
            StardewModdingAPI.Events.ControlEvents.KeyPressed += ControlEvents_KeyPressed;

            config = ModHelper.ReadConfig<ModConfig>();
            
            StardewModdingAPI.Events.GameEvents.UpdateTick += GameEvents_UpdateTick;
            serverHack = false;
            
        }

        private void MenuEvents_MenuClosed(object sender, StardewModdingAPI.Events.EventArgsClickableMenuClosed e)
        {
            if (Game1.IsMasterGame == false && config.enableMultiplayerHack)
            {
                if (this.lastMenuType == null)
                {
                    return;
                }
                else
                {
                    if (lastMenuType == typeof(StardewValley.Menus.SaveGameMenu) ||lastMenuType==typeof(StardewValley.Menus.ReadyCheckDialog))
                    {
                        SerializationManager.restoreAllModObjects(SerializationManager.trackedObjectList);
                    }
                }
            }
        }

        private void MenuEvents_MenuChanged(object sender, StardewModdingAPI.Events.EventArgsClickableMenuChanged e)
        {
            if (Game1.IsMasterGame == false && config.enableMultiplayerHack)
            {
                if (Game1.activeClickableMenu.GetType() == typeof(StardewValley.Menus.ReadyCheckDialog))
                {
                    SerializationManager.cleanUpInventory();
                    SerializationManager.cleanUpWorld();
                    SerializationManager.cleanUpStorageContainers();
                }
            }
            lastMenuType = Game1.activeClickableMenu.GetType();
        }

        /// <summary>
        /// Returns the value of the data snagged by reflection.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static object GetInstanceField(Type type, object instance, string fieldName)
        {
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            FieldInfo field = type.GetField(fieldName, bindFlags);
            /*
            FieldInfo[] meh = type.GetFields(bindFlags);
            foreach(var v in meh)
            {
                if (v.Name == null)
                {
                    continue;
                }
                Monitor.Log(v.Name);
            }
            */
            return field.GetValue(instance);
        }

        public static void SetInstanceField(Type type, object instance, object value, string fieldName)
        {
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                | BindingFlags.Static;
            FieldInfo field = type.GetField(fieldName, bindFlags);
            field.SetValue(instance, value);
            return;
        }

        private void GameEvents_UpdateTick(object sender, EventArgs e)
        {


            if (Game1.client !=null && serverHack == false && config.enableMultiplayerHack)
            {

            }
        }

        private void ControlEvents_KeyPressed(object sender, StardewModdingAPI.Events.EventArgsKeyPressed e)
        {
            if(e.KeyPressed.ToString()== config.modularMenuKey && Game1.activeClickableMenu==null)
            {
                //Game1.activeClickableMenu = new ModularGameMenu(0);
            }
        }

        private void SaveEvents_AfterLoad(object sender, EventArgs e)
        {
           
            SerializationManager.restoreAllModObjects(SerializationManager.trackedObjectList);

            List<KeyValuePair<Vector2, MultiTileComponent>> objs = new List<KeyValuePair<Vector2, MultiTileComponent>>();
            
            MultiTileComponent tile1 = new MultiTileComponent(0,"Tileobj1","A basic tile obj",new Texture2DExtended(ModCore.ModHelper,ModCore.Manifest ,Path.Combine("Content", "Graphics", "MultiTest", "Test1.png")));
            MultiTileComponent tile2 = new MultiTileComponent(0,"Tileobj2", "A basic tile obj", new Texture2DExtended(ModCore.ModHelper, ModCore.Manifest, Path.Combine("Content", "Graphics", "MultiTest", "Test2.png")));
            MultiTileComponent tile3 = new MultiTileComponent(0,"Tileobj3", "A basic tile obj", new Texture2DExtended(ModCore.ModHelper, ModCore.Manifest, Path.Combine("Content", "Graphics", "MultiTest", "Test3.png")));
            objs.Add(new KeyValuePair<Vector2, MultiTileComponent>(new Vector2(0, 0), tile1));
            objs.Add(new KeyValuePair<Vector2, MultiTileComponent>(new Vector2(1, 0), tile2));
            objs.Add(new KeyValuePair<Vector2, MultiTileComponent>(new Vector2(2, 0), tile3));

            MultiTileObject collection= new MultiTileObject("MultiTest", "Trying to get multi object testing working", Vector2.Zero, new Texture2DExtended(ModCore.ModHelper, ModCore.Manifest, Path.Combine("Content", "Graphics", "MultiTest", "Test3.png")), objs, StardustCore.IlluminateFramework.Colors.invertColor(StardustCore.IlluminateFramework.LightColorsList.Purple), "MultiTest");
            

            Game1.player.addItemToInventory(collection);
            /*
            CoreObject tile1 = new CoreObject(new Texture2DExtended(ModCore.ModHelper, Path.Combine("Content", "Graphics", "MultiTest", "Test1.png")),3, Vector2.Zero,9);
           
            tile1.description = "Hello";
            tile1.Name = "test";
            tile1.displayName = "test";
            Game1.player.addItemToInventory(tile1);
            */
            
        }

        private void SaveEvents_AfterSave(object sender, EventArgs e)
        {
            SerializationManager.restoreAllModObjects(SerializationManager.trackedObjectList);

        }

        private void SaveEvents_BeforeSave(object sender, EventArgs e)
        {
            SerializationManager.cleanUpInventory();
            SerializationManager.cleanUpWorld();
            SerializationManager.cleanUpStorageContainers();
        }

        private void ControlEvents_MouseChanged(object sender, StardewModdingAPI.Events.EventArgsMouseStateChanged e)
        {
            //??? 
            return;
            if (Game1.activeClickableMenu == null) return;
            var MouseState = Mouse.GetState();
            if (Game1.activeClickableMenu is StardewValley.Menus.ItemGrabMenu && MouseState.LeftButton == ButtonState.Released)
            {
                (Game1.activeClickableMenu as StardewValley.Menus.ItemGrabMenu).ItemsToGrabMenu.populateClickableComponentList();
                for (int index = 0; index < (Game1.activeClickableMenu as StardewValley.Menus.ItemGrabMenu).ItemsToGrabMenu.inventory.Count; ++index)
                {
                    if ((Game1.activeClickableMenu as StardewValley.Menus.ItemGrabMenu).ItemsToGrabMenu.inventory[index] != null)
                    {
                        (Game1.activeClickableMenu as StardewValley.Menus.ItemGrabMenu).ItemsToGrabMenu.inventory[index].myID += 53910;
                        (Game1.activeClickableMenu as StardewValley.Menus.ItemGrabMenu).ItemsToGrabMenu.inventory[index].upNeighborID += 53910;
                        (Game1.activeClickableMenu as StardewValley.Menus.ItemGrabMenu).ItemsToGrabMenu.inventory[index].rightNeighborID += 53910;
                        (Game1.activeClickableMenu as StardewValley.Menus.ItemGrabMenu).ItemsToGrabMenu.inventory[index].downNeighborID = -7777;
                        (Game1.activeClickableMenu as StardewValley.Menus.ItemGrabMenu).ItemsToGrabMenu.inventory[index].leftNeighborID += 53910;
                        (Game1.activeClickableMenu as StardewValley.Menus.ItemGrabMenu).ItemsToGrabMenu.inventory[index].fullyImmutable = true;
                    }
                }
                // (Game1.activeClickableMenu as ItemGrabMenu).inventory.playerInventory = false;
                // Game1.activeClickableMenu =Game1.activeClickableMenu;//new ItemGrabMenu((Game1.activeClickableMenu as ItemGrabMenu).ItemsToGrabMenu.actualInventory,true,true,null,null,null,null,false,false,true,true,true,1,null,-1,null);
            }
        }
    }
}
