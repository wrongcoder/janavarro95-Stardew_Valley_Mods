using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revitalize.Framework.Objects;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;

namespace Revitalize.Framework.Hacks
{
    public class MenuHacks
    {
        public static bool EndOfDay_HasProcessedModdedItems;
        public static bool EndOfDay_IsShowingEndOfNightMenus()
        {
            return Game1.showingEndOfNightStuff;
        }

        public static bool EndOfDay_IsEndOfDayMenuShippingMenu()
        {
            if (EndOfDay_IsShowingEndOfNightMenus())
            {
                if (Game1.activeClickableMenu.GetType() == typeof(StardewValley.Menus.ShippingMenu)) return true;
                if (Game1.endOfNightMenus.Count == 0 && Game1.activeClickableMenu==null) return false;
                if (Game1.endOfNightMenus.Peek().GetType() == typeof(StardewValley.Menus.ShippingMenu))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else return false;
        }

        public static ShippingMenu EndOfDay_GetShippingMenu()
        {
            if (EndOfDay_IsEndOfDayMenuShippingMenu())
            {
                if (Game1.activeClickableMenu.GetType() == typeof(StardewValley.Menus.ShippingMenu))
                {
                    return (ShippingMenu)Game1.activeClickableMenu;
                }
                return (ShippingMenu)Game1.endOfNightMenus.Peek();
            }
            return null;
        }

        public static void EndOfDay_HackShipping()
        {
            if (EndOfDay_GetShippingMenu() != null)
            {
                //ModCore.log("Hooked shipping menu!");
                ShippingMenu menu = EndOfDay_GetShippingMenu();

                List<int> categoryTotals = new List<int>();
                List<MoneyDial> categoryDials = new List<MoneyDial>();
                List<List<Item>> categoryItems = new List<List<Item>>();

                var CT_R=ModCore.ModHelper.Reflection.GetField<List<int>>(menu, "categoryTotals", true);
                var CD_R= ModCore.ModHelper.Reflection.GetField<List<MoneyDial>>(menu, "categoryDials", true);
                var CI_R= ModCore.ModHelper.Reflection.GetField<List<List<Item>>>(menu, "categoryItems", true);

                categoryTotals = CT_R.GetValue();
                categoryDials = CD_R.GetValue();
                categoryItems = CI_R.GetValue();

                //Recalculate other category.
                foreach (CustomObject obj in categoryItems[4])
                {
                    ModCore.log(obj.Name);
                    if (obj is StardewValley.Object)
                    {
                        ModCore.log(obj.sellToStorePrice());
                        categoryTotals[4] += obj.sellToStorePrice() * obj.Stack;
                        Game1.stats.itemsShipped += (uint)obj.Stack;
                        /*
                        if (o.Category == -75 || o.Category == -79)
                            Game1.stats.CropsShipped += (uint)o.Stack;
                        if (o.countsForShippedCollection())
                            Game1.player.shippedBasic((int)((NetFieldBase<int, NetInt>)o.parentSheetIndex), (int)((NetFieldBase<int, NetInt>)o.stack));
                            */
                    }
                }
                categoryTotals[5] = 0;
                for (int index = 0; index < 5; ++index)
                {
                    categoryTotals[5] += categoryTotals[index];
                    categoryItems[5].AddRange((IEnumerable<Item>)categoryItems[index]);
                    categoryDials[index].currentValue = categoryTotals[index];
                    categoryDials[index].previousTargetValue = categoryDials[index].currentValue;
                }
                categoryDials[5].currentValue = categoryTotals[5];
                if (Game1.IsMasterGame)
                    Game1.player.Money += categoryTotals[5];
                Game1.setRichPresence("earnings", (object)categoryTotals[5]);

            }
        }

        public static void EndOfDay_OnMenuChanged(object o, StardewModdingAPI.Events.RenderedEventArgs sender)
        {
            if (EndOfDay_IsShowingEndOfNightMenus() && EndOfDay_HasProcessedModdedItems==false)
            {
                EndOfDay_HackShipping();
                EndOfDay_HasProcessedModdedItems = true;
            }
        }

        public static void EndOfDay_CleanupForNewDay(object o, StardewModdingAPI.Events.SavedEventArgs sender)
        {
            EndOfDay_HasProcessedModdedItems = false;
        }
        


    }
}
