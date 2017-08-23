using AdditionalCropsFramework.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
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
         * *  */
    public class ModCore : Mod
    {
        public static IModHelper ModHelper;

        public static readonly List<ModularCropObject> SpringWildCrops = new List<ModularCropObject>();
        public static readonly List<ModularCropObject> SummerWildCrops = new List<ModularCropObject>();
        public static readonly List<ModularCropObject> FallWildCrops = new List<ModularCropObject>();
        public static readonly List<ModularCropObject> WinterWildCrops = new List<ModularCropObject>();


       public override void Entry(IModHelper helper)
        {
            ModHelper = helper;

            StardewModdingAPI.Events.ControlEvents.KeyPressed += ControlEvents_KeyPressed;

        }




        private void ControlEvents_KeyPressed(object sender, StardewModdingAPI.Events.EventArgsKeyPressed e)
        {
            if (e.KeyPressed == Keys.U)
            {
                List<Item> shopInventory = new List<Item>();
                shopInventory.Add((Item)new ModularSeeds(1, "SeedsGraphics", "SeedsData", "CropsGraphics", "CropsData","CropsObjectTexture","CropsObjectData"));
                shopInventory.Add((Item)new PlanterBox(0, Vector2.Zero));
                shopInventory.Add((Item)new PlanterBox(1, Vector2.Zero, "PlanterBox.png", "PlanterBox"));
                Game1.activeClickableMenu = new StardewValley.Menus.ShopMenu(shopInventory);
            }
        }

        }
    }
