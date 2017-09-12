using AdditionalCropsFramework.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using StardustCore.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AdditionalCropsFramework
{ 

    /*
    Todo: + Reuse Seedbag code from revitalize in this mod. (Done!)
          + Make a way to interact with the modded seedbags (look at revitalize's code again) (Done! Fixed with planter boxes)
          ! Serialize and deserialize the modded crop objects and planter boxes and seed bags that exist in the game. Maybe have a list to keep track of all of them or just iterate through everything in the world?
          + Make it so I can plant the modded crops. (DONE! Fixed in planterboxes)
          ! Make sure crops grow overnight.
          ! Add way for crops to be watered and to ensure that there is a graphical update when the crop is being watered.
          ! Add way to harvest crop from planterbox without removing planterbox.
          ! Fix invisible planterbox so that it does get removed when planting seeds on tillable soil and keep that soil as HoeDirt instead of reverting to normal. This can also be used to just make the dirt look wet.
          ! Add in Multiple layers to the Planter Boxes: SoilLayer->CropLayer->BoxLayer is the draw order. Mainly for aesthetics. Box on top, dry dirt below, and wet dirt below that.
         * * *  */
    public class ModCore : Mod
    {
        public static IModHelper ModHelper;
        public static IMonitor ModMonitor;

     


        public static readonly List<ModularCropObject> SpringWildCrops = new List<ModularCropObject>();
        public static readonly List<ModularCropObject> SummerWildCrops = new List<ModularCropObject>();
        public static readonly List<ModularCropObject> FallWildCrops = new List<ModularCropObject>();
        public static readonly List<ModularCropObject> WinterWildCrops = new List<ModularCropObject>();
        public static SerializationManager serilaizationManager;



       public override void Entry(IModHelper helper)
        {
            ModHelper = helper;
            ModMonitor = this.Monitor;

            
            StardewModdingAPI.Events.ControlEvents.KeyPressed += ControlEvents_KeyPressed;
            StardewModdingAPI.Events.SaveEvents.AfterLoad += SaveEvents_AfterLoad;
            StardewModdingAPI.Events.SaveEvents.BeforeSave += SaveEvents_BeforeSave;
            StardewModdingAPI.Events.SaveEvents.AfterSave += SaveEvents_AfterSave;

            StardewModdingAPI.Events.GameEvents.UpdateTick += GameEvents_UpdateTick;

        }

        private void GameEvents_UpdateTick(object sender, EventArgs e)
        {
            if (Game1.activeClickableMenu != null)
            {
              //  if (Game1.activeClickableMenu.allClickableComponents == null) return;
              
                List<IClickableMenu> pages = this.Helper.Reflection.GetPrivateValue<List<IClickableMenu>>(Game1.activeClickableMenu, "pages");
                if(Game1.activeClickableMenu is GameMenu)
                {
                    StardewValley.Menus.IClickableMenu s = pages[(Game1.activeClickableMenu as GameMenu).currentTab];

                   // Log.AsyncG(s);
                   
                    
                    foreach (var v in (s as StardewValley.Menus.InventoryPage).allClickableComponents)
                    {
                        if (v.containsPoint(Game1.getMouseX(), Game1.getMouseY()))
                        {

                            if (v == null) return;
                            string fire = v.name;
                            //Log.AsyncC(v.name);

                            bool num = true;
                            foreach (var v2 in fire)
                            {
                                if (v2 != '0' && v2 != '1' && v2 != '2' && v2 != '3' && v2 != '4' && v2 != '5' && v2 != '6' && v2 != '7' && v2 != '8' && v2 != '9')
                                {
                                    num = false;
                                    break;
                                }
                                else continue;
                            }
                            if (num == true)
                            {
                                int inv = Convert.ToInt32(v.name);
                                Item I = (s as StardewValley.Menus.InventoryPage).inventory.actualInventory[inv];
                                Log.AsyncM(I.Name);
                                Log.AsyncC(I.GetType());
                            }
                        }
                      //  if (v == null) continue;
                       // Log.AsyncC(v.name);
                      //  Log.AsyncM(v.item.Name);
                       // (s as StardewValley.Menus.InventoryPage).
                    }
                    

               
                }
            }
        }

        private void SaveEvents_AfterSave(object sender, EventArgs e)
        {
            serilaizationManager.restoreAllModObjects(serilaizationManager.trackedObjectList);
        }

        private void SaveEvents_BeforeSave(object sender, EventArgs e)
        {
            serilaizationManager.cleanUpInventory();
            serilaizationManager.cleanUpWorld();
        }

        private void SaveEvents_AfterLoad(object sender, EventArgs e)
        {
           string invPath = Path.Combine(ModCore.ModHelper.DirectoryPath,Game1.player.name,"PlayerInventory");
           string worldPath = Path.Combine(ModCore.ModHelper.DirectoryPath, Game1.player.name, "ObjectsInWorld"); ;
           string trashPath = Path.Combine(ModCore.ModHelper.DirectoryPath, "ModTrashFolder");
           serilaizationManager = new SerializationManager(invPath,trashPath,worldPath);

          serilaizationManager.acceptedTypes.Add("AdditionalCropsFramework.PlanterBox", new SerializerDataNode(new SerializerDataNode.SerializingFunction(PlanterBox.Serialize), new SerializerDataNode.ParsingFunction(PlanterBox.ParseIntoInventory), new SerializerDataNode.WorldParsingFunction(PlanterBox.SerializeFromWorld))); //need serialize, deserialize, and world deserialize functions.

            serilaizationManager.restoreAllModObjects(serilaizationManager.trackedObjectList);
        }


        private void ControlEvents_KeyPressed(object sender, StardewModdingAPI.Events.EventArgsKeyPressed e)
        {
            if (e.KeyPressed == Keys.U)
            {
                List<Item> shopInventory = new List<Item>();
                shopInventory.Add((Item)new ModularSeeds(1, "SeedsGraphics", "SeedsData", "CropsGraphics", "CropsData","CropsObjectTexture","CropsObjectData"));
                shopInventory.Add((Item)new PlanterBox(0, Vector2.Zero));
                shopInventory.Add((Item)new PlanterBox(1, Vector2.Zero, "PlanterBox.png", "PlanterBox.xnb"));
                Game1.activeClickableMenu = new StardewValley.Menus.ShopMenu(shopInventory);
            }
        }

        }
    }
