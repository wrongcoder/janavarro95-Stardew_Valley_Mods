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

        private List<Item> shippingList;


       public override void Entry(IModHelper helper)
        {
            ModHelper = helper;
            ModMonitor = this.Monitor;
            StardewModdingAPI.Events.SaveEvents.AfterLoad += SaveEvents_AfterLoad;
            StardewModdingAPI.Events.SaveEvents.BeforeSave += SaveEvents_BeforeSave;
            StardewModdingAPI.Events.SaveEvents.AfterSave += SaveEvents_AfterSave;
         
            if (!Directory.Exists(Path.Combine(ModCore.ModHelper.DirectoryPath, Utilities.EntensionsFolderName)))
            {
                Directory.CreateDirectory(Path.Combine(ModCore.ModHelper.DirectoryPath, Utilities.EntensionsFolderName));
            }
           // StardewModdingAPI.Events.GameEvents.UpdateTick += GameEvents_UpdateTick;
            this.shippingList = new List<Item>();
        }

        //Unused
        private void GameEvents_UpdateTick(object sender, EventArgs e)
        {
            
            if (Game1.activeClickableMenu != null) return;
            else
            {
                if (shippingList.Count > 0) Game1.activeClickableMenu = new Menus.ShippingMenuExpanded(shippingList);
                shippingList.Clear();
            }
            if (Game1.newDay == true)
            {
                foreach(var v in Game1.getFarm().shippingBin)
                {
                    if (shippingList.Contains(v)) continue;
                    if (v is StardustCore.CoreObject)
                    {
                        Log.AsyncC(v.Name);
                        shippingList.Add(v);
                    }
                }
                foreach(var v in shippingList)
                {
                    Game1.getFarm().shippingBin.Remove(v);
                }
            }
            
        }

        public void dailyUpdates()
        {
            foreach (var v in serilaizationManager.trackedObjectList)
            {
                if (v is PlanterBox)
                {
                    (v as PlanterBox).dayUpdate();
                }
            }
        }

        private void SaveEvents_AfterSave(object sender, EventArgs e)
        {
            serilaizationManager.restoreAllModObjects(serilaizationManager.trackedObjectList);

            dailyUpdates();
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
            dailyUpdates();
        }




        }
    }
