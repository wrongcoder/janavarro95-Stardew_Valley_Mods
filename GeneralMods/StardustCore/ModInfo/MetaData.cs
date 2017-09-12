using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StardustCore.ModInfo
{
    public class Metadata
    {
        Color ModInfoColor;
        string ModName;

   
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modColor"></param>
        /// <param name="modName"></param>
        public Metadata(Color ? modColor,string modName="")
        {
            if (modColor == null) ModInfoColor = Color.Black;
            else ModInfoColor =(Color) modColor;
            ModName = modName;
        }

        public static string parseModNameFromType(Type t)
        {
            string s = t.ToString();
            string[] array = s.Split('.');
            return array[0];
        }
        public static string parseClassNameFromType(Type t)
        {
            string s = t.ToString();
            string[] array = s.Split('.');
            return array[array.Length-1];
        }


        public static void GameEvents_UpdateTick(object sender, EventArgs e)
        {
            if (Game1.activeClickableMenu != null)
            {
                //  if (Game1.activeClickableMenu.allClickableComponents == null) return;
                try {
                    List<IClickableMenu> pages = ModCore.ModHelper.Reflection.GetPrivateValue<List<IClickableMenu>>(Game1.activeClickableMenu, "pages");
                    if (Game1.activeClickableMenu is GameMenu)
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
                                    // Log.AsyncM(I.Name);
                                    string s1 = parseModNameFromType(I.GetType());
                                    string s2= parseClassNameFromType(I.GetType());
                                    Log.AsyncC(s1);
                                    Log.AsyncO(s2);

                                    try
                                    {
                                        SpriteBatch b = new SpriteBatch(Game1.graphics.GraphicsDevice);
                                        b.Begin();
                                        float boxX =Game1.getMouseX()- (Game1.viewport.Width * .20f);
                                        float boxY =Game1.getMouseY() - (Game1.viewport.Height * .05f);
                                        float boxWidth= (Game1.viewport.Width * .20f);
                                        float boxHeight = (Game1.viewport.Height*.35f);
                                        Game1.drawDialogueBox((int)boxX,(int) boxY,(int)boxWidth, (int)boxHeight, false, true, null,false);
                                        Utility.drawTextWithShadow(Game1.spriteBatch, s1, Game1.smallFont, new Vector2(boxX+(Game1.viewport.Width*.05f), Game1.getMouseY()+(int)(Game1.viewport.Height*.05)), Color.Cyan, 1, -1);
                                        b.End();
                                    }
                                    catch(Exception errr)
                                    {
                                        Log.AsyncC(errr);
                                    }

                                }
                            }
                            //  if (v == null) continue;
                            // Log.AsyncC(v.name);
                            //  Log.AsyncM(v.item.Name);
                            // (s as StardewValley.Menus.InventoryPage).
                        }



                    }
                }
                catch(Exception err)
                {

                }
            }
            
        }

    }
}
