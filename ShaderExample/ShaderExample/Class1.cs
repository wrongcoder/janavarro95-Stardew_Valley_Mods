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
                Monitor.Log("DRAW");
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
            }
            //Location specific drawing done here


            //Game1.spriteBatch.End();

            Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
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
