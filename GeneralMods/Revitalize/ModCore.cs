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
using Revitalize.Framework.Graphics;
using Revitalize.Framework.Graphics.Animations;

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
            MultiTiledComponent obj = new MultiTiledComponent(new BasicItemInformation("CoreObjectTest","YAY FUN!","Omegasis.Revitalize.MultiTiledComponent",Color.White,-300,0,false,100,Vector2.Zero,true,true,"Omegasis.TEST1", "2048/0/-300/Crafting -9/Play '2048 by Platonymous' at home!/true/true/0/2048", Game1.objectSpriteSheet,Color.White,0,true,typeof(MultiTiledComponent),null,new AnimationManager(new Texture2DExtended(Game1.objectSpriteSheet),new Animation(new Rectangle(0,0,16,16))),Color.Red));
            MultiTiledComponent obj2 = new MultiTiledComponent(new BasicItemInformation("CoreObjectTest2", "SomeFun", "Omegasis.Revitalize.MultiTiledComponent", Color.White, -300, 0, false, 100, Vector2.Zero, true, true, "Omegasis.TEST1", "2048/0/-300/Crafting -9/Play '2048 by Platonymous' at home!/true/true/0/2048", Game1.objectSpriteSheet, Color.White, 0, true, typeof(MultiTiledComponent), null, new AnimationManager(new Texture2DExtended(Game1.objectSpriteSheet), new Animation(new Rectangle(0, 16, 16, 16))), Color.Red));
            MultiTiledComponent obj3 = new MultiTiledComponent(new BasicItemInformation("CoreObjectTest3", "NoFun", "Omegasis.Revitalize.MultiTiledComponent", Color.White, -300, 0, false, 100, Vector2.Zero, true, true, "Omegasis.TEST1", "2048/0/-300/Crafting -9/Play '2048 by Platonymous' at home!/true/true/0/2048", Game1.objectSpriteSheet, Color.White, 0, true, typeof(MultiTiledComponent), null, new AnimationManager(new Texture2DExtended(Game1.objectSpriteSheet), new Animation(new Rectangle(0, 32, 16, 16))), Color.Red));

            MultiTiledObject bigObject= new MultiTiledObject(new BasicItemInformation("MultiTest", "A really big object", "Omegasis.Revitalize.MultiTiledObject", Color.Blue, -300, 0, false, 100, Vector2.Zero, true, true, "Omegasis.BigTiledTest", "2048/0/-300/Crafting -9/Play '2048 by Platonymous' at home!/true/true/0/2048", Game1.objectSpriteSheet, Color.White, 0, true, typeof(MultiTiledObject), null, new AnimationManager(), Color.White));
            bigObject.addComponent(new Vector2(0, 0), obj);
            bigObject.addComponent(new Vector2(1, 0), obj2);
            bigObject.addComponent(new Vector2(2, 0), obj3);


            new InventoryItem(bigObject, 100,1).addToNPCShop("Gus");
            Game1.player.addItemToInventory(bigObject);
            
        }

        public static void log(object message)
        {
           
            ModMonitor.Log(message.ToString());
        }
    }
}
