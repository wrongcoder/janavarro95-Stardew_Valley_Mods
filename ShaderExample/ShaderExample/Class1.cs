using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Menus;
using StardewValley.Projectiles;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using xTile.Dimensions;

using xRectangle = xTile.Dimensions.Rectangle;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using xTile.Layers;
using StardewValley.Tools;
using StardewValley.Locations;

namespace ShaderExample
{
    class Class1 : Mod
    {
        public static Effect effect;


        public override void Entry(IModHelper helper)
        {
            //StardewModdingAPI.Events.GraphicsEvents.OnPreRenderEvent += GraphicsEvents_OnPreRenderEvent;

            //Need to make checks to see what location I am at and have custom shader functions for those events.

            StardewModdingAPI.Events.GraphicsEvents.OnPostRenderEvent += GraphicsEvents_OnPreRenderEvent;
            //StardewModdingAPI.Events.GraphicsEvents.OnPreRenderEvent += GraphicsEvents_OnPreRenderEvent;

            //StardewModdingAPI.Events.GraphicsEvents.OnPostRenderEvent += GraphicsEvents_OnPreRenderEvent1;
            effect = Helper.Content.Load<Effect>(Path.Combine("Content", "Shaders", "GreyScaleEffect.xnb"));


        }

        private void GraphicsEvents_OnPreRenderEvent(object sender, EventArgs e)
        { 
            try
            {
                Game1.spriteBatch.End();
            }
            catch(Exception err)
            {
                return;
            }
            if (Game1.activeClickableMenu != null)
            {
                Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                SetInstanceField(typeof(SpriteBatch), Game1.spriteBatch, effect, "customEffect");
                Class1.effect.CurrentTechnique.Passes[0].Apply();
                Game1.activeClickableMenu.draw(Game1.spriteBatch);
                Game1.spriteBatch.End();
            }

            if (Game1.player.currentLocation == null)
            {
                Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                return;
            }
            Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            //drawBack();
            drawMapPart1();
            Game1.spriteBatch.End();


            Game1.spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            Framework.Drawers.Characters.drawFarmer();
            Framework.Drawers.Characters.drawCharacters();

            foreach (var v in Game1.player.currentLocation.terrainFeatures)
            {
                var value = v.Values;
                var keys = v.Keys;
                int index = 0;
                foreach(var terrain in value)
                {
                    terrain.draw(Game1.spriteBatch, keys.ElementAt(index));
                    index++;
                }
            }
            Game1.spriteBatch.End();

            //Game1.spriteBatch.End();

            //Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            //drawFront();

            Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            drawMapPart2();

            drawOverlays();
            Game1.spriteBatch.End();

            if (Game1.activeClickableMenu != null)
            {
                Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                SetInstanceField(typeof(SpriteBatch), Game1.spriteBatch, effect, "customEffect");
                Class1.effect.CurrentTechnique.Passes[0].Apply();
                Game1.activeClickableMenu.draw(Game1.spriteBatch);
                Game1.spriteBatch.End();
                Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                SetInstanceField(typeof(SpriteBatch), Game1.spriteBatch, effect, "customEffect");
                Class1.effect.CurrentTechnique.Passes[0].Apply();
                if (Game1.activeClickableMenu is StardewValley.Menus.GameMenu)
                {
                    if ((Game1.activeClickableMenu as StardewValley.Menus.GameMenu).currentTab == 3) return;
                    //Draw menu tabs.
                    var tabField = GetInstanceField(typeof(StardewValley.Menus.GameMenu), Game1.activeClickableMenu, "tabs");
                    var tabs = (List<ClickableComponent>)tabField;
                    foreach (ClickableComponent tab in tabs)
                    {
                        int num = 0;
                        switch (tab.name)
                        {
                            case "catalogue":
                                num = 7;
                                break;
                            case "collections":
                                num = 5;
                                break;
                            case "coop":
                                num = 1;
                                break;
                            case "crafting":
                                num = 4;
                                break;
                            case "exit":
                                num = 7;
                                break;
                            case "inventory":
                                num = 0;
                                break;
                            case "map":
                                num = 3;
                                break;
                            case "options":
                                num = 6;
                                break;
                            case "skills":
                                num = 1;
                                break;
                            case "social":
                                num = 2;
                                break;
                        }
                        Game1.spriteBatch.Draw(Game1.mouseCursors, new Vector2((float)tab.bounds.X, (float)(tab.bounds.Y + ((Game1.activeClickableMenu as StardewValley.Menus.GameMenu).currentTab == (Game1.activeClickableMenu as StardewValley.Menus.GameMenu).getTabNumberFromName(tab.name) ? 8 : 0))), new Rectangle?(new Rectangle(num * 16, 368, 16, 16)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 0.00001f);
                        if (tab.name.Equals("skills"))
                            Game1.player.FarmerRenderer.drawMiniPortrat(Game1.spriteBatch, new Vector2((float)(tab.bounds.X + 8), (float)(tab.bounds.Y + 12 + ((Game1.activeClickableMenu as StardewValley.Menus.GameMenu).currentTab == (Game1.activeClickableMenu as StardewValley.Menus.GameMenu).getTabNumberFromName(tab.name) ? 8 : 0))), 0.00011f, 3f, 2, Game1.player);
                    }
                }

                if ((Game1.activeClickableMenu as StardewValley.Menus.GameMenu).currentTab == 2)
                {
                    

                    var pageField = GetInstanceField(typeof(StardewValley.Menus.GameMenu), Game1.activeClickableMenu, "pages");
                    var pages = (List<IClickableMenu>)pageField;
                    var socialPage = pages.ElementAt(2);
                    var v = (StardewValley.Menus.SocialPage)socialPage;
                    if (v == null)
                    {
                        Monitor.Log("WHATTT?????");
                    }
                    v = (StardewValley.Menus.SocialPage)v;


                    int numFarmers = (int)GetInstanceField(typeof(StardewValley.Menus.SocialPage), v, "numFarmers");

                    getInvokeMethod(v, "drawHorizontalPartition", new object[]{
                        Game1.spriteBatch, v.yPositionOnScreen + IClickableMenu.borderWidth + 128 + 4, true
                        });
                    getInvokeMethod(v, "drawHorizontalPartition", new object[]{
                        Game1.spriteBatch, v.yPositionOnScreen + IClickableMenu.borderWidth + 192 + 32 + 20, true
                        });
                    getInvokeMethod(v, "drawHorizontalPartition", new object[]{
                        Game1.spriteBatch, v.yPositionOnScreen + IClickableMenu.borderWidth + 320 + 36, true
                        });
                    getInvokeMethod(v, "drawHorizontalPartition", new object[]{
                        Game1.spriteBatch, v.yPositionOnScreen + IClickableMenu.borderWidth + 384 + 32 + 52, true
                        });
                    Rectangle scissorRectangle = Game1.spriteBatch.GraphicsDevice.ScissorRectangle;
                    Rectangle rectangle = scissorRectangle;
                    rectangle.Y = Math.Max(0, rowPosition(v,numFarmers - 1));
                    rectangle.Height -= rectangle.Y;
                    Game1.spriteBatch.GraphicsDevice.ScissorRectangle = rectangle;
                    try
                    {
                           
                        getInvokeMethod(v, "drawVerticalPartition", new object[]
                        {
                            Game1.spriteBatch,
                             v.xPositionOnScreen + 256 + 12,
                             true
                        });
                    }
                    finally
                    {
                        Game1.spriteBatch.GraphicsDevice.ScissorRectangle = scissorRectangle;
                    }
                    getInvokeMethod(v, "drawVerticalPartition", new object[]
                        {
                            Game1.spriteBatch,
                             v.xPositionOnScreen + 256 + 12+340,
                             true
                        });



                    int slotPosition2=(int)GetInstanceField(typeof(StardewValley.Menus.SocialPage), v, "slotPosition");




                    var sprites = (List<ClickableTextureComponent>)GetInstanceField(typeof(StardewValley.Menus.SocialPage), v, "sprites");
                    var names = (List<object>)GetInstanceField(typeof(StardewValley.Menus.SocialPage), v, "names");
                    for (int slotPosition = slotPosition2; slotPosition < slotPosition2 + 5; ++slotPosition)
                    {
                        if (slotPosition < sprites.Count)
                        {
                            if (names[slotPosition] is string)
                                getInvokeMethod(v, "drawNPCSlot", new object[]{
                                    Game1.spriteBatch, slotPosition
                                    });
                            else if (names[slotPosition] is long)
                                getInvokeMethod(v, "drawFarmerSlot", new object[]{
                                    Game1.spriteBatch, slotPosition
                                    });
                        }
                    }


                    (GetInstanceField(typeof(SocialPage),v,"upButton") as ClickableTextureComponent).draw(Game1.spriteBatch);
                    (GetInstanceField(typeof(SocialPage), v, "downButton") as ClickableTextureComponent).draw(Game1.spriteBatch);
                    Rectangle scrollBarRunner=(Rectangle)(GetInstanceField(typeof(SocialPage), v, "scrollBarRunner"));
                    IClickableMenu.drawTextureBox(Game1.spriteBatch, Game1.mouseCursors, new Rectangle(403, 383, 6, 6), scrollBarRunner.X, scrollBarRunner.Y, scrollBarRunner.Width, scrollBarRunner.Height, Color.White, 4f, true);
                    (GetInstanceField(typeof(SocialPage), v, "scrollBar") as ClickableTextureComponent).draw(Game1.spriteBatch);
                    string hoverText = (GetInstanceField(typeof(SocialPage), v, "hoverText") as string);
                    if (!hoverText.Equals(""))
                        IClickableMenu.drawHoverText(Game1.spriteBatch, hoverText, Game1.smallFont, 0, 0, -1, (string)null, -1, (string[])null, (Item)null, 0, -1, -1, -1, -1, 1f, (CraftingRecipe)null);
                }
                


                Game1.activeClickableMenu.upperRightCloseButton.draw(Game1.spriteBatch);
                Game1.activeClickableMenu.drawMouse(Game1.spriteBatch);
                Game1.spriteBatch.End();
            }
            //Location specific drawing done here


            //Game1.spriteBatch.End();

            Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            SetInstanceField(typeof(SpriteBatch), Game1.spriteBatch, effect, "customEffect");
            Class1.effect.CurrentTechnique.Passes[0].Apply();
        }

        private int rowPosition(IClickableMenu menu,int i)
        {
            int slotPosition2 = (int)GetInstanceField(typeof(StardewValley.Menus.SocialPage), menu, "slotPosition");
            int num1 = i - slotPosition2;
            int num2 = 112;
            return menu.yPositionOnScreen + IClickableMenu.borderWidth + 160 + 4 + num1 * num2;
        }



        public void drawMapPart1()
        {
            //Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            SetInstanceField(typeof(SpriteBatch), Game1.spriteBatch, effect, "customEffect");
            foreach (var layer in Game1.player.currentLocation.map.Layers)
            {
                //do back and buildings
                if (layer.Id == "Paths" || layer.Id=="AlwaysFront"|| layer.Id=="Front" ) continue;
                //if (layer.Id != "Back" || layer.Id != "Buildings") continue;
                //Framework.Drawers.Layer.drawLayer(layer,Game1.mapDisplayDevice, Game1.viewport, new xTile.Dimensions.Location(0, 0), false, Game1.pixelZoom);
                layer.Draw(Game1.mapDisplayDevice, Game1.viewport, new xTile.Dimensions.Location(0, 0), false, Game1.pixelZoom);

            }
            //Game1.spriteBatch.End();
        }

        public void getInvokeMethod(object target, string name ,object[] param)
        {
            var hello=target.GetType().GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic| BindingFlags.Instance| BindingFlags.Static);
            hello.Invoke(target, param);
        }

        public void drawMapPart2()
        {
            //Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            SetInstanceField(typeof(SpriteBatch), Game1.spriteBatch, effect, "customEffect");
            foreach (var layer in Game1.player.currentLocation.map.Layers)
            {
                //do front, and always front.
                if (layer.Id == "Back" || layer.Id == "Buildings" || layer.Id=="Paths") continue;
                //if (layer.Id != "Back" || layer.Id != "Buildings") continue;
                //Framework.Drawers.Layer.drawLayer(layer,Game1.mapDisplayDevice, Game1.viewport, new xTile.Dimensions.Location(0, 0), false, Game1.pixelZoom);
                layer.Draw(Game1.mapDisplayDevice, Game1.viewport, new xTile.Dimensions.Location(0, 0), false, Game1.pixelZoom);

            }
            //Game1.spriteBatch.End();
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
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                | BindingFlags.Static;
            FieldInfo field = type.GetField(fieldName, bindFlags);
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



        protected void drawOverlays()
        {
            SpriteBatch spriteBatch = Game1.spriteBatch;
           // spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState)null, (RasterizerState)null);
            SetInstanceField(typeof(SpriteBatch), Game1.spriteBatch, effect, "customEffect");
            effect.CurrentTechnique.Passes[0].Apply();
            foreach(var v in Game1.onScreenMenus)
            {
                v.draw(spriteBatch);
            }
            //if ((Game1.displayHUD || Game1.eventUp) && (Game1.currentBillboard == 0 && Game1.gameMode == (byte)3) && (!Game1.freezeControls && !Game1.panMode))
            //Game1.drawMouseCursor();
            //spriteBatch.End();
        }
    }
}
