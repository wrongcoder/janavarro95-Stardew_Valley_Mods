using Microsoft.Xna.Framework;
using PyTK.Types;
using PyTK.Extensions;
using Revitalize.Framework.Objects;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize
{
    public class ModCore : Mod
    {
        public static IModHelper ModHelper;
        public static IMonitor ModMonitor;

        public static Dictionary<string, CustomObject> customObjects;


        public override void Entry(IModHelper helper)
        {
            ModHelper = helper;
            ModMonitor = Monitor;

            ModHelper.Events.GameLoop.SaveLoaded += GameLoop_SaveLoaded;
        }

        private void GameLoop_SaveLoaded(object sender, StardewModdingAPI.Events.SaveLoadedEventArgs e)
        {
            CustomObject obj = new CustomObject(new BasicItemInformation("CoreObjectTest","YAY FUN!","Omegasis.Revitalize.CoreObject",Color.Violet,-300,0,false,100,Vector2.Zero,true,true,"Omegasis.bleh", "2048/0/-300/Crafting -9/Play '2048 by Platonymous' at home!/true/true/0/2048", Game1.objectSpriteSheet,Color.White,0,true,typeof(CustomObject),null,new Framework.Graphics.Animations.AnimationManager(),Color.Red));


            new InventoryItem(obj, 100,1).addToNPCShop("Gus");
            Game1.player.addItemToInventory(obj);
            
        }

        public static void log(object message)
        {
           
            ModMonitor.Log(message.ToString());
        }
    }
}
