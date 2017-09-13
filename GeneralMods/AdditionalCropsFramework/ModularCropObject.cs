
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Objects;
using StardustCore;
using StardustCore.Animations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace AdditionalCropsFramework
{
    /// <summary>
    ///  Revitalize ModularCropObject Class. This is a core class and should only be extended upon.
    /// </summary>
    /// 
    public class ModularCropObject : CoreObject
    {

        public int Decoration_type;

        public int rotations;

        public int currentRotation;

        public int sourceIndexOffset;

        protected Vector2 drawPosition;

        public Rectangle sourceRect;

        public Rectangle defaultSourceRect;

        public Rectangle defaultBoundingBox;

        public string description;

        [XmlIgnore]
        public Texture2D TextureSheet;


        public new bool flipped;

        [XmlIgnore]
        public bool flaggedForPickUp;

        public bool lightGlowAdded;

      

        public bool itemReadyForHarvest;

        public string thisType;

        public bool removable;

        public string locationsName;

        public Color drawColor;

        public bool useXML;

        public string cropType;

        public string dataFileName;
        public string texturePath;

        public override string Name
        {
            get
            {
                return this.name;
            }

        }

        public ModularCropObject()
        {
            this.updateDrawPosition();
        }

        public ModularCropObject(bool f)
        {
            //does nothng
        }

        public ModularCropObject(int which, int initalStack, string ObjectTextureSheetName, string DataFileName)
        {
            this.serializationName = this.GetType().ToString();
           // if (File.Exists(ObjectTextureSheetName)) Log.AsyncC("YAY");
            this.tileLocation = Vector2.Zero;
          
            this.stack = initalStack;
            try
            {
                TextureSheet = ModCore.ModHelper.Content.Load<Texture2D>(Path.Combine(Utilities.EntensionsFolderName, ObjectTextureSheetName));  //Game1.content.Load<Texture2D>("TileSheets\\furniture");
            }
            catch(Exception err)
            {
              //  Log.AsyncM(err);
            }
                texturePath = ObjectTextureSheetName;
            this.dataFileName = DataFileName;
            this.canBeSetDown = false;
          //  Log.AsyncG(Path.Combine(Utilities.EntensionsFolderName,DataFileName));
          //  Log.AsyncC(which);
            Dictionary<int, string> dictionary = new Dictionary<int, string>();

        
            try
            {
                dictionary = ModCore.ModHelper.Content.Load<Dictionary<int, string>>(Path.Combine(Utilities.EntensionsFolderName, DataFileName));
            }
            catch(Exception err)
            {
               // Log.AsyncC(err);
            }


            string[] array = dictionary[which].Split(new char[]
            {
                '/'
            });
            this.name = array[0];
            this.price = Convert.ToInt32(array[1]);
            this.edibility =Convert.ToInt32(array[2]);
            string[] array2 = array[3].Split(' ');
            this.cropType = array2[0];
            this.category =Convert.ToInt32(array2[1]);
            this.type = this.cropType;
            this.displayName = this.name;
            
            this.description = array[5];

            string[] dArray = this.description.Split(' ');
            string newDes = "";
            int MaxWords = 7;
            int currentCount = 0;
            foreach (var v in dArray)
            {
                if (currentCount == MaxWords)
                {
                    currentCount = 0;
                    newDes += "\n";
                }
                newDes += v+" ";
                currentCount++;
            }
            this.description = newDes;

            this.defaultSourceRect = new Rectangle(which * 16 % TextureSheet.Width, which * 16 / TextureSheet.Width * 16, 16, 16);
            this.sourceRect = this.defaultSourceRect;
            
     
        
            this.defaultBoundingBox = new Rectangle(0, 0, 16, 16);
            this.boundingBox = this.defaultBoundingBox;

            this.updateDrawPosition();
            this.parentSheetIndex = which;

            try
            {
                this.animationManager = new StardustCore.Animations.AnimationManager(this.TextureSheet, new StardustCore.Animations.Animation(this.defaultSourceRect, -1), AnimationManager.parseAnimationsFromXNB(array[3]), "default");
                this.animationManager.setAnimation("Default", 0);
                //Log.AsyncC("Using animation manager");
            }
            catch (Exception errr)
            {
                this.animationManager = new StardustCore.Animations.AnimationManager(this.TextureSheet, new StardustCore.Animations.Animation(this.defaultSourceRect, -1));
            }
        }

        public override string getDescription()
        {
            return this.description;
        }

        /*
        public override bool performDropDownAction(StardewValley.Farmer who)
        {
            this.resetOnPlayerEntry((who == null) ? Game1.currentLocation : who.currentLocation);
            return false;
        }
        */
        public override void hoverAction()
        {
            base.hoverAction();
            if (!Game1.player.isInventoryFull())
            {
                Game1.mouseCursor = 2;
            }
        }

        /*
        public override bool checkForAction(StardewValley.Farmer who, bool justCheckingForActivity = false)
        {
            /*
            var mState = Microsoft.Xna.Framework.Input.Mouse.GetState();
            if (mState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                // Game1.showRedMessage("YOOO");
                //do some stuff when the right button is down
                // rotate();
                if (this.heldObject != null)
                {
                    //  Game1.player.addItemByMenuIfNecessary(this.heldObject);
                    // this.heldObject = null;
                }
                else
                {
                    //   this.heldObject = Game1.player.ActiveObject;
                    //  Game1.player.removeItemFromInventory(heldObject);
                }
                //this.minutesUntilReady = 30;
                //  Log.AsyncC("placed item!");
            }
            else
            {
                //Game1.showRedMessage("CRY");
            }

            if (justCheckingForActivity)
            {
                return true;
            }
            return this.clicked(who);
            
        }
    */
        //DONT USE THIS BASE IT IS TERRIBLE
        /*
        public override bool clicked(StardewValley.Farmer who)
        {

            //  Game1.showRedMessage("THIS IS CLICKED!!!");
            Game1.haltAfterCheck = false;

            if (this.heldObject != null)
            {
                this.spillInventoryEverywhere();
                return false;
            }

            if (this.heldObject == null && (who.ActiveObject == null || !(who.ActiveObject is ModularCropObject)))
            {
                if (Game1.player.currentLocation is FarmHouse)
                {
                    // Game1.showRedMessage("Why2?");
                    //  this.spillInventoryEverywhere();

                   // if (this.heldObject != null) Util.addItemToInventoryElseDrop(this.heldObject.getOne());
                    this.heldObject = new ModularCropObject(parentSheetIndex, Vector2.Zero, this.inventoryMaxSize);
                   // Util.addItemToInventoryElseDrop(this.heldObject.getOne());
                    this.heldObject = null;
                    this.flaggedForPickUp = true;
                    thisLocation = null;
                    return true;
                }
                else
                {
                    // return true;

                    this.flaggedForPickUp = true;
                    if (this is TV)
                    {
                        // this.heldObject = new TV(parentSheetIndex, Vector2.Zero);
                    }
                    else
                    {

                        //    Util.addItemToInventoryElseDrop(this.heldObject);

                        var obj = new ModularCropObject(parentSheetIndex, Vector2.Zero, this.inventoryMaxSize);
                        Util.addItemToInventoryElseDrop(obj);
                        //     this.spillInventoryEverywhere();
                        if (this.heldObject != null) this.heldObject.performRemoveAction(this.tileLocation, who.currentLocation);

                        this.heldObject = null;
                        Game1.playSound("coin");
                        thisLocation = null;
                        return true;
                    }

                }
            }
            if (this.heldObject != null && who.addItemToInventoryBool(this.heldObject, false))
            {
                // Game1.showRedMessage("Why3?");
                // if(this.heldObject!=null) Game1.player.addItemByMenuIfNecessary((Item)this.heldObject);
                // this.spillInventoryEverywhere();
                var obj = new ModularCropObject(parentSheetIndex, Vector2.Zero, this.inventoryMaxSize);
                Util.addItemToInventoryElseDrop(obj);
                if (this.heldObject != null) this.heldObject.performRemoveAction(this.tileLocation, who.currentLocation);
                this.heldObject = null;
                Game1.playSound("coin");
                thisLocation = null;
                return true;
            }



            return false;
        }
        */
        /*
        public virtual bool RightClicked(StardewValley.Farmer who)
        {
            //  StardewModdingAPI.Log.AsyncC(lightColor);
            //  Game1.activeClickableMenu = new Revitalize.Menus.LightCustomizer(this);

            // Game1.showRedMessage("THIS IS CLICKED!!!");
            //var mState = Microsoft.Xna.Framework.Input.Mouse.GetState();
            /*

            Game1.haltAfterCheck = false;
            if (this.Decoration_type == 11 && who.ActiveObject != null && who.ActiveObject != null && this.heldObject == null)
            {
                //    Game1.showRedMessage("Why1?");
                return false;
            }
            if (this.heldObject == null && (who.ActiveObject == null || !(who.ActiveObject is Light)))
            {
                if (Game1.player.currentLocation is FarmHouse)
                {
                    //       
                    Game1.player.addItemByMenuIfNecessary(this);
                    removeLights(this.thisLocation);
                    this.lightsOn = false;
                    Game1.playSound("coin");
                    //        this.flaggedForPickUp = true;
                    return true;
                }
                else
                {
                    // return true;
                    // this.heldObject = new Light(parentSheetIndex, Vector2.Zero, this.lightColor, this.inventoryMaxSize);
                    Game1.player.addItemByMenuIfNecessary(this);
                    removeLights(this.thisLocation);
                    this.lightsOn = false;
                    Game1.playSound("coin");
                    return true;

                }
            }
            if (this.heldObject != null && who.addItemToInventoryBool(this.heldObject, false))
            {
                //    Game1.showRedMessage("Why3?");
                // if(this.heldObject!=null) Game1.player.addItemByMenuIfNecessary((Item)this.heldObject);
                Util.addItemToInventoryElseDrop(this);
                this.heldObject.performRemoveAction(this.tileLocation, who.currentLocation);
                this.heldObject = null;
                Game1.playSound("coin");
                removeLights(this.thisLocation);
                this.lightsOn = false;
                return true;
            }

            

            return false;
        }
        */

            /*
        public override void DayUpdate(GameLocation location)
        {
            base.DayUpdate(location);
            this.lightGlowAdded = false;
            if (!Game1.isDarkOut() || (Game1.newDay && !Game1.isRaining))
            {
                this.removeLights(location);
                return;
            }
            this.addLights(location);
        }
        */

            /*
        public virtual void resetOnPlayerEntry(GameLocation environment)
        {
            this.removeLights(environment);
            if (Game1.isDarkOut())
            {
                this.addLights(environment);
            }
        }

        public override bool performObjectDropInAction(StardewValley.Object dropIn, bool probe, StardewValley.Farmer who)
        {
            // Log.AsyncG("HEY!");
            if ((this.Decoration_type == 11 || this.Decoration_type == 5) && this.heldObject == null && !dropIn.bigCraftable && (!(dropIn is ModularCropObject) || ((dropIn as ModularCropObject).getTilesWide() == 1 && (dropIn as ModularCropObject).getTilesHigh() == 1)))
            {
                this.heldObject = (StardewValley.Object)dropIn.getOne();
                this.heldObject.tileLocation = this.tileLocation;
                this.heldObject.boundingBox.X = this.boundingBox.X;
                this.heldObject.boundingBox.Y = this.boundingBox.Y;
                // Log.AsyncO(getDefaultBoundingBoxForType((dropIn as ModularCropObject).Decoration_type));
                this.heldObject.performDropDownAction(who);
                if (!probe)
                {
                    Game1.playSound("woodyStep");
                    //  Log.AsyncC("HUH?");
                    if (who != null)
                    {
                        who.reduceActiveItemByOne();
                    }
                }
                return true;
            }
            return false;
        }

        public virtual void addLights(GameLocation environment)
        {
            if (this.Decoration_type == 7)
            {
                if (this.sourceIndexOffset == 0)
                {
                    this.sourceRect = this.defaultSourceRect;
                    this.sourceRect.X = this.sourceRect.X + this.sourceRect.Width;
                }
                this.sourceIndexOffset = 1;
                if (this.lightSource == null)
                {
                    Utility.removeLightSource((int)(this.tileLocation.X * 2000f + this.tileLocation.Y));
                    this.lightSource = new LightSource(4, new Vector2((float)(this.boundingBox.X + Game1.tileSize / 2), (float)(this.boundingBox.Y - Game1.tileSize)), 2f, lightColor, (int)(this.tileLocation.X * 2000f + this.tileLocation.Y));
                    Game1.currentLightSources.Add(this.lightSource);
                    // Log.AsyncG("LIGHT SOURCE ADDED FFFFFFF");
                    return;
                }
            }
            else if (this.Decoration_type == 13)
            {
                if (this.sourceIndexOffset == 0)
                {
                    this.sourceRect = this.defaultSourceRect;
                    this.sourceRect.X = this.sourceRect.X + this.sourceRect.Width;
                }
                this.sourceIndexOffset = 1;
                if (this.lightGlowAdded)
                {
                    environment.lightGlows.Remove(new Vector2((float)(this.boundingBox.X + Game1.tileSize / 2), (float)(this.boundingBox.Y + Game1.tileSize)));
                    this.lightGlowAdded = false;
                }
            }
            else
            {

                if (this.sourceIndexOffset == 0)
                {
                    this.sourceRect = this.defaultSourceRect;
                    this.sourceRect.X = this.sourceRect.X + this.sourceRect.Width;
                }
                this.sourceIndexOffset = 1;
                if (this.lightSource == null)
                {
                    Utility.removeLightSource((int)(this.tileLocation.X * 2000f + this.tileLocation.Y));
                    this.lightSource = new LightSource(4, new Vector2((float)(this.boundingBox.X + Game1.tileSize / 2), (float)(this.boundingBox.Y - Game1.tileSize)), 2f, lightColor, (int)(this.tileLocation.X * 2000f + this.tileLocation.Y));
                    Game1.currentLightSources.Add(this.lightSource);
                    return;
                }

            }
        }


        public virtual void addLights(GameLocation environment, Color c)
        {
            if (this.Decoration_type == 7)
            {
                if (this.sourceIndexOffset == 0)
                {
                    this.sourceRect = this.defaultSourceRect;
                    this.sourceRect.X = this.sourceRect.X + this.sourceRect.Width;
                }
                this.sourceIndexOffset = 1;
                if (this.lightSource == null)
                {
                    Utility.removeLightSource((int)(this.tileLocation.X * 2000f + this.tileLocation.Y));
                    this.lightSource = new LightSource(4, new Vector2((float)(this.boundingBox.X + Game1.tileSize / 2), (float)(this.boundingBox.Y - Game1.tileSize)), 2f, c, (int)(this.tileLocation.X * 2000f + this.tileLocation.Y));
                    // this.lightSource.lightTexture = Game1.content.Load<Texture2D>("LooseSprites\\Lighting\\BlueLight");
                    Game1.currentLightSources.Add(this.lightSource);
                    //  Log.AsyncG("LIGHT SOURCE ADDED FFFFFFF");
                    return;
                }
            }
            else if (this.Decoration_type == 13)
            {
                if (this.sourceIndexOffset == 0)
                {
                    this.sourceRect = this.defaultSourceRect;
                    this.sourceRect.X = this.sourceRect.X + this.sourceRect.Width;
                }
                this.sourceIndexOffset = 1;
                if (this.lightGlowAdded)
                {
                    environment.lightGlows.Remove(new Vector2((float)(this.boundingBox.X + Game1.tileSize / 2), (float)(this.boundingBox.Y + Game1.tileSize)));
                    this.lightGlowAdded = false;
                }
            }
            else
            {

                if (this.sourceIndexOffset == 0)
                {
                    this.sourceRect = this.defaultSourceRect;
                    this.sourceRect.X = this.sourceRect.X + this.sourceRect.Width;
                }
                this.sourceIndexOffset = 1;
                if (this.lightSource == null)
                {
                    Utility.removeLightSource((int)(this.tileLocation.X * 2000f + this.tileLocation.Y));
                    this.lightSource = new LightSource(4, new Vector2((float)(this.boundingBox.X + Game1.tileSize / 2), (float)(this.boundingBox.Y - Game1.tileSize)), 2f, c, (int)(this.tileLocation.X * 2000f + this.tileLocation.Y));
                    Game1.currentLightSources.Add(this.lightSource);
                    return;
                }

            }
        }

        public void removeLights(GameLocation environment)
        {
            if (this.Decoration_type == 7)
            {
                if (this.sourceIndexOffset == 1)
                {
                    this.sourceRect = this.defaultSourceRect;
                }
                this.sourceIndexOffset = 0;
                Utility.removeLightSource((int)(this.tileLocation.X * 2000f + this.tileLocation.Y));
                this.lightSource = null;
                return;
            }
            if (this.Decoration_type == 13)
            {
                if (this.sourceIndexOffset == 1)
                {
                    this.sourceRect = this.defaultSourceRect;
                }
                this.sourceIndexOffset = 0;
                if (Game1.isRaining)
                {
                    this.sourceRect = this.defaultSourceRect;
                    this.sourceRect.X = this.sourceRect.X + this.sourceRect.Width;
                    this.sourceIndexOffset = 1;
                    return;
                }
                if (!this.lightGlowAdded && !environment.lightGlows.Contains(new Vector2((float)(this.boundingBox.X + Game1.tileSize / 2), (float)(this.boundingBox.Y + Game1.tileSize))))
                {
                    environment.lightGlows.Add(new Vector2((float)(this.boundingBox.X + Game1.tileSize / 2), (float)(this.boundingBox.Y + Game1.tileSize)));
                }
                this.lightGlowAdded = true;
            }

            if (this.sourceIndexOffset == 1)
            {
                this.sourceRect = this.defaultSourceRect;
            }
            this.sourceIndexOffset = 0;
            Utility.removeLightSource((int)(this.tileLocation.X * 2000f + this.tileLocation.Y));
            this.lightSource = null;
            return;
        }

        public override bool minutesElapsed(int minutes, GameLocation environment)
        {
            // Log.Info("minutes passed in"+minutes);
            //  Log.Info("minues remaining" + this.minutesUntilReady);
            this.minutesUntilReady = (this.minutesUntilReady - minutes);
            if (Game1.isDarkOut())
            {
                // this.addLights(environment,this.lightColor);
                this.addLights(environment);
            }
            else
            {
                this.removeLights(environment);
            }

            if (minutesUntilReady == 0)
            {
                // Log.AsyncC(this.name + "Is ready!");
                // Log.AsyncC(Game1.player.getStandingPosition());
                // Vector2 v2 = new Vector2(this.tileLocation.X * Game1.tileSize, this.tileLocation.Y * Game1.tileSize);
                //Game1.createItemDebris((Item)this.heldObject, v2, Game1.player.getDirection());
                // minutesUntilReady = 30;
            }

            return false;
        }

        public override void performRemoveAction(Vector2 tileLocation, GameLocation environment)
        {
            this.removeLights(environment);
            if (this.Decoration_type == 13 && this.lightGlowAdded)
            {
                environment.lightGlows.Remove(new Vector2((float)(this.boundingBox.X + Game1.tileSize / 2), (float)(this.boundingBox.Y + Game1.tileSize)));
                this.lightGlowAdded = false;
            }
            this.spillInventoryEverywhere();
            base.performRemoveAction(tileLocation, environment);
        }

        public virtual void rotate()
        {
            if (this.rotations < 2)
            {
                return;
            }
            int num = (this.rotations == 4) ? 1 : 2;
            this.currentRotation += num;
            this.currentRotation %= 4;
            this.flipped = false;
            Point point = default(Point);
            int num2 = this.Decoration_type;
            switch (num2)
            {
                case 2:
                    point.Y = 1;
                    point.X = -1;
                    break;
                case 3:
                    point.X = -1;
                    point.Y = 1;
                    break;
                case 4:
                    break;
                case 5:
                    point.Y = 0;
                    point.X = -1;
                    break;
                default:
                    if (num2 == 12)
                    {
                        point.X = 0;
                        point.Y = 0;
                    }
                    break;
            }
            bool flag = this.Decoration_type == 5 || this.Decoration_type == 12 || this.parentSheetIndex == 724 || this.parentSheetIndex == 727;
            bool flag2 = this.defaultBoundingBox.Width != this.defaultBoundingBox.Height;
            if (flag && this.currentRotation == 2)
            {
                this.currentRotation = 1;
            }
            if (flag2)
            {
                int height = this.boundingBox.Height;
                switch (this.currentRotation)
                {
                    case 0:
                    case 2:
                        this.boundingBox.Height = this.defaultBoundingBox.Height;
                        this.boundingBox.Width = this.defaultBoundingBox.Width;
                        break;
                    case 1:
                    case 3:
                        this.boundingBox.Height = this.boundingBox.Width + point.X * Game1.tileSize;
                        this.boundingBox.Width = height + point.Y * Game1.tileSize;
                        break;
                }
            }
            Point point2 = default(Point);
            int num3 = this.Decoration_type;
            if (num3 == 12)
            {
                point2.X = 1;
                point2.Y = -1;
            }
            if (flag2)
            {
                switch (this.currentRotation)
                {
                    case 0:
                        this.sourceRect = this.defaultSourceRect;
                        break;
                    case 1:
                        this.sourceRect = new Rectangle(this.defaultSourceRect.X + this.defaultSourceRect.Width, this.defaultSourceRect.Y, this.defaultSourceRect.Height - 16 + point.Y * 16 + point2.X * 16, this.defaultSourceRect.Width + 16 + point.X * 16 + point2.Y * 16);
                        break;
                    case 2:
                        this.sourceRect = new Rectangle(this.defaultSourceRect.X + this.defaultSourceRect.Width + this.defaultSourceRect.Height - 16 + point.Y * 16 + point2.X * 16, this.defaultSourceRect.Y, this.defaultSourceRect.Width, this.defaultSourceRect.Height);
                        break;
                    case 3:
                        this.sourceRect = new Rectangle(this.defaultSourceRect.X + this.defaultSourceRect.Width, this.defaultSourceRect.Y, this.defaultSourceRect.Height - 16 + point.Y * 16 + point2.X * 16, this.defaultSourceRect.Width + 16 + point.X * 16 + point2.Y * 16);
                        this.flipped = true;
                        break;
                }
            }
            else
            {
                this.flipped = (this.currentRotation == 3);
                if (this.rotations == 2)
                {
                    this.sourceRect = new Rectangle(this.defaultSourceRect.X + ((this.currentRotation == 2) ? 1 : 0) * this.defaultSourceRect.Width, this.defaultSourceRect.Y, this.defaultSourceRect.Width, this.defaultSourceRect.Height);
                }
                else
                {
                    this.sourceRect = new Rectangle(this.defaultSourceRect.X + ((this.currentRotation == 3) ? 1 : this.currentRotation) * this.defaultSourceRect.Width, this.defaultSourceRect.Y, this.defaultSourceRect.Width, this.defaultSourceRect.Height);
                }
            }
            if (flag && this.currentRotation == 1)
            {
                this.currentRotation = 2;
            }
            this.updateDrawPosition();
        }

        public virtual bool isGroundFurniture()
        {
            return this.Decoration_type != 13 && this.Decoration_type != 6 && this.Decoration_type != 13;
        }
        */
        public override bool canBeGivenAsGift()
        {
            return true;
        }
        
        public override bool canBePlacedHere(GameLocation l, Vector2 tile)
        {
            return false;
            if ((l is FarmHouse))
            {
                for (int i = 0; i < this.boundingBox.Width / Game1.tileSize; i++)
                {
                    for (int j = 0; j < this.boundingBox.Height / Game1.tileSize; j++)
                    {
                        Vector2 vector = tile * (float)Game1.tileSize + new Vector2((float)i, (float)j) * (float)Game1.tileSize;
                        vector.X += (float)(Game1.tileSize / 2);
                        vector.Y += (float)(Game1.tileSize / 2);
                        foreach (KeyValuePair<Vector2, StardewValley.Object> something in l.objects)
                        {
                            StardewValley.Object obj = something.Value;
                            if ((obj.GetType()).ToString().Contains("ModularCropObject"))
                            {
                                ModularCropObject current = (ModularCropObject)obj;
                                if (current.Decoration_type == 11 && current.getBoundingBox(current.tileLocation).Contains((int)vector.X, (int)vector.Y) && current.heldObject == null && this.getTilesWide() == 1)
                                {
                                    bool result = true;
                                    return result;
                                }
                                if ((current.Decoration_type != 12 || this.Decoration_type == 12) && current.getBoundingBox(current.tileLocation).Contains((int)vector.X, (int)vector.Y))
                                {
                                    bool result = false;
                                    return result;
                                }
                            }
                        }
                    }
                }
                //return Util.canBePlacedHere(this, l, tile);
            }
            else
            {
                // Game1.showRedMessage("NOT FARMHOUSE");
                for (int i = 0; i < this.boundingBox.Width / Game1.tileSize; i++)
                {
                    for (int j = 0; j < this.boundingBox.Height / Game1.tileSize; j++)
                    {
                        Vector2 vector = tile * (float)Game1.tileSize + new Vector2((float)i, (float)j) * (float)Game1.tileSize;
                        vector.X += (float)(Game1.tileSize / 2);
                        vector.Y += (float)(Game1.tileSize / 2);
                        /*
                        foreach (ModularCropObject current in (l as FarmHouse).ModularCropObject)
                        {
                            if (current.Decoration_type == 11 && current.getBoundingBox(current.tileLocation).Contains((int)vector.X, (int)vector.Y) && current.heldObject == null && this.getTilesWide() == 1)
                            {
                                bool result = true;
                                return result;
                            }
                            if ((current.Decoration_type != 12 || this.Decoration_type == 12) && current.getBoundingBox(current.tileLocation).Contains((int)vector.X, (int)vector.Y))
                            {
                                bool result = false;
                                return result;
                            }
                        }
                       */ 
                    }
                }
              //  return Util.canBePlacedHere(this, l, tile);
            }
            
        }

        public virtual void updateDrawPosition()
        {
            this.drawPosition = new Vector2((float)this.boundingBox.X, (float)(this.boundingBox.Y - (this.sourceRect.Height * Game1.pixelZoom - this.boundingBox.Height)));
        }

        public virtual int getTilesWide()
        {
            return this.boundingBox.Width / Game1.tileSize;
        }

        public virtual int getTilesHigh()
        {
            return this.boundingBox.Height / Game1.tileSize;
        }
        /*
        public override bool placementAction(GameLocation location, int x, int y, StardewValley.Farmer who = null)
        {
          //  Log.AsyncC(x);
          //  Log.AsyncM(y);
           
            if (location is FarmHouse)
            {
                Point point = new Point(x / Game1.tileSize, y / Game1.tileSize);
                List<Rectangle> walls = FarmHouse.getWalls((location as FarmHouse).upgradeLevel);
                this.tileLocation = new Vector2((float)point.X, (float)point.Y);
                bool flag = false;
                if (this.Decoration_type == 6 || this.Decoration_type == 13 || this.parentSheetIndex == 1293)
                {
                    int num = (this.parentSheetIndex == 1293) ? 3 : 0;
                    bool flag2 = false;
                    foreach (Rectangle current in walls)
                    {
                        if ((this.Decoration_type == 6 || this.Decoration_type == 13 || num != 0) && current.Y + num == point.Y && current.Contains(point.X, point.Y - num))
                        {
                            flag2 = true;
                            break;
                        }
                    }
                    if (!flag2)
                    {
                        Game1.showRedMessage("Must be placed on wall");
                        return false;
                    }
                    flag = true;
                }
                for (int i = point.X; i < point.X + this.getTilesWide(); i++)
                {
                    for (int j = point.Y; j < point.Y + this.getTilesHigh(); j++)
                    {
                        if (location.doesTileHaveProperty(i, j, "NoFurniture", "Back") != null)
                        {
                            Game1.showRedMessage("Furniture can't be placed here");
                            return false;
                        }
                        if (!flag && Utility.pointInRectangles(walls, i, j))
                        {
                            Game1.showRedMessage("Can't place on wall");
                            return false;
                        }
                        if (location.getTileIndexAt(i, j, "Buildings") != -1)
                        {
                            return false;
                        }
                    }
                }
                this.boundingBox = new Rectangle(x, y, this.boundingBox.Width, this.boundingBox.Height);
                foreach (KeyValuePair<Vector2, StardewValley.Object> c in location.objects)
                {
                    StardewValley.Object ehh = c.Value;
                    if (((ehh.GetType()).ToString()).Contains("ModularCropObject"))
                    {
                        ModularCropObject current2 = (ModularCropObject)ehh;
                        if (current2.Decoration_type == 11 && current2.heldObject == null && current2.getBoundingBox(current2.tileLocation).Intersects(this.boundingBox))
                        {
                            current2.performObjectDropInAction(this, false, (who == null) ? Game1.player : who);
                            bool result = true;
                            return result;
                        }
                    }
                }
                foreach (StardewValley.Farmer current3 in location.getStardewValley.Farmers())
                {
                    if (current3.GetBoundingBox().Intersects(this.boundingBox))
                    {
                        Game1.showRedMessage("Can't place on top of a person.");
                        bool result = false;
                        return result;
                    }
                }
                this.updateDrawPosition();
              //  Log.AsyncO(this.boundingBox);
              //  Log.AsyncO(x);
              //  Log.AsyncY(y);
                for (int i = 0; i <= this.boundingBox.X / Game1.tileSize; i++)
                {
                    base.placementAction(location, x + 1, y, who);
                }
                for (int i = 0; i <= this.boundingBox.Y / Game1.tileSize; i++)
                {
                    base.placementAction(location, x, y + 1, who);
                }
                return true;
            }
            else
            {
                Point point = new Point(x / Game1.tileSize, y / Game1.tileSize);
                //  List<Rectangle> walls = FarmHouse.getWalls((location as FarmHouse).upgradeLevel);
                this.tileLocation = new Vector2((float)point.X, (float)point.Y);
                bool flag = false;
                if (this.Decoration_type == 6 || this.Decoration_type == 13 || this.parentSheetIndex == 1293)
                {
                    int num = (this.parentSheetIndex == 1293) ? 3 : 0;
                    bool flag2 = false;
                    /*
                    foreach (Rectangle current in walls)
                    {
                        if ((this.Decoration_type == 6 || this.Decoration_type == 13 || num != 0) && current.Y + num == point.Y && current.Contains(point.X, point.Y - num))
                        {
                            flag2 = true;
                            break;
                        }
                    }
                    
                    if (!flag2)
                    {
                        Game1.showRedMessage("Must be placed on wall");
                        return false;
                    }
                    flag = true;
                }
                for (int i = point.X; i < point.X + this.getTilesWide(); i++)
                {
                    for (int j = point.Y; j < point.Y + this.getTilesHigh(); j++)
                    {
                        if (location.doesTileHaveProperty(i, j, "NoFurniture", "Back") != null)
                        {
                            Game1.showRedMessage("Furniture can't be placed here");
                            return false;
                        }
                        /*
                        if (!flag && Utility.pointInRectangles(walls, i, j))
                        {
                            Game1.showRedMessage("Can't place on wall");
                            return false;
                        }
                        
                        if (location.getTileIndexAt(i, j, "Buildings") != -1)
                        {
                            return false;
                        }
                    }
                }
                this.boundingBox = new Rectangle(x, y, this.boundingBox.Width, this.boundingBox.Height);
                /*
                foreach (Furniture current2 in (location as FarmHouse).furniture)
                {
                    if (current2.furniture_type == 11 && current2.heldObject == null && current2.getBoundingBox(current2.tileLocation).Intersects(this.boundingBox))
                    {
                        current2.performObjectDropInAction(this, false, (who == null) ? Game1.player : who);
                        bool result = true;
                        return result;
                    }
                }
                
                foreach (StardewValley.Farmer current3 in location.getStardewValley.Farmers())
                {
                    if (current3.GetBoundingBox().Intersects(this.boundingBox))
                    {
                        Game1.showRedMessage("Can't place on top of a person.");
                        bool result = false;
                        return result;
                    }
                }
                this.updateDrawPosition();
                thisLocation = Game1.player.currentLocation;
                return base.placementAction(location, x, y, who);
            }

        }
        */
        public override bool placementAction(GameLocation location, int x, int y, StardewValley.Farmer who = null)
        {
            return false;
            if (base.placementAction(location, x, y, who) == true)
            {
               // Lists.trackedObjectList.Add(this);
            }


            if (location is FarmHouse)
            {
                Point point = new Point(x / Game1.tileSize, y / Game1.tileSize);
                List<Rectangle> walls = FarmHouse.getWalls((location as FarmHouse).upgradeLevel);
                this.tileLocation = new Vector2((float)point.X, (float)point.Y);
                bool flag = false;
                if (this.Decoration_type == 6 || this.Decoration_type == 13 || this.parentSheetIndex == 1293)
                {
                    int num = (this.parentSheetIndex == 1293) ? 3 : 0;
                    bool flag2 = false;
                    foreach (Rectangle current in walls)
                    {
                        if ((this.Decoration_type == 6 || this.Decoration_type == 13 || num != 0) && current.Y + num == point.Y && current.Contains(point.X, point.Y - num))
                        {
                            flag2 = true;
                            break;
                        }
                    }
                    if (!flag2)
                    {
                        Game1.showRedMessage("Must be placed on wall");
                        return false;
                    }
                    flag = true;
                }
                for (int i = point.X; i < point.X + this.getTilesWide(); i++)
                {
                    for (int j = point.Y; j < point.Y + this.getTilesHigh(); j++)
                    {
                        if (location.doesTileHaveProperty(i, j, "NoFurniture", "Back") != null)
                        {
                            Game1.showRedMessage("Furniture can't be placed here");
                            return false;
                        }
                        if (!flag && Utility.pointInRectangles(walls, i, j))
                        {
                            Game1.showRedMessage("Can't place on wall");
                            return false;
                        }
                        if (location.getTileIndexAt(i, j, "Buildings") != -1)
                        {
                            return false;
                        }
                    }
                }
                this.boundingBox = new Rectangle(x / Game1.tileSize, y / Game1.tileSize, this.boundingBox.Width, this.boundingBox.Height);
                foreach (KeyValuePair<Vector2, StardewValley.Object> c in location.objects)
                {
                    StardewValley.Object ehh = c.Value;
                    if (((ehh.GetType()).ToString()).Contains("Spawner"))
                    {
                        ModularCropObject current2 = (ModularCropObject)ehh;
                        if (current2.Decoration_type == 11 && current2.heldObject == null && current2.getBoundingBox(current2.tileLocation).Intersects(this.boundingBox))
                        {
                            current2.performObjectDropInAction(this, false, (who == null) ? Game1.player : who);
                            bool result = true;
                            return result;
                        }
                    }
                }
                foreach (StardewValley.Farmer current3 in location.getFarmers())
                {
                    if (current3.GetBoundingBox().Intersects(this.boundingBox))
                    {
                        Game1.showRedMessage("Can't place on top of a person.");
                        bool result = false;
                        return result;
                    }
                }
                this.updateDrawPosition();
                //  Log.AsyncO(this.boundingBox);
                //   Log.AsyncO(x);
                //   Log.AsyncY(y);
                for (int i = 0; i <= this.boundingBox.X / Game1.tileSize; i++)
                {
                    base.placementAction(location, x + 1, y, who);
                }
                for (int i = 0; i <= this.boundingBox.Y / Game1.tileSize; i++)
                {
                    base.placementAction(location, x, y + 1, who);
                }
                return true;
            }
            else
            {
                Point point = new Point(x / Game1.tileSize, y / Game1.tileSize);
                //  List<Rectangle> walls = FarmHouse.getWalls((location as FarmHouse).upgradeLevel);
                this.tileLocation = new Vector2((float)point.X, (float)point.Y);
                bool flag = false;
                if (this.Decoration_type == 6 || this.Decoration_type == 13 || this.parentSheetIndex == 1293)
                {
                    int num = (this.parentSheetIndex == 1293) ? 3 : 0;
                    bool flag2 = false;
                    /*
                    foreach (Rectangle current in walls)
                    {
                        if ((this.Decoration_type == 6 || this.Decoration_type == 13 || num != 0) && current.Y + num == point.Y && current.Contains(point.X, point.Y - num))
                        {
                            flag2 = true;
                            break;
                        }
                    }
                    */
                    if (!flag2)
                    {
                        Game1.showRedMessage("Must be placed on wall");
                        return false;
                    }
                    flag = true;
                }
                for (int i = point.X; i < point.X + this.getTilesWide(); i++)
                {
                    for (int j = point.Y; j < point.Y + this.getTilesHigh(); j++)
                    {
                        if (location.doesTileHaveProperty(i, j, "NoFurniture", "Back") != null)
                        {
                            Game1.showRedMessage("Furniture can't be placed here");
                            return false;
                        }
                        /*
                        if (!flag && Utility.pointInRectangles(walls, i, j))
                        {
                            Game1.showRedMessage("Can't place on wall");
                            return false;
                        }
                        */
                        if (location.getTileIndexAt(i, j, "Buildings") != -1)
                        {
                            return false;
                        }
                    }
                }
                this.boundingBox = new Rectangle(x / Game1.tileSize, y / Game1.tileSize, this.boundingBox.Width, this.boundingBox.Height);
                /*
                foreach (Furniture current2 in (location as FarmHouse).furniture)
                {
                    if (current2.furniture_type == 11 && current2.heldObject == null && current2.getBoundingBox(current2.tileLocation).Intersects(this.boundingBox))
                    {
                        current2.performObjectDropInAction(this, false, (who == null) ? Game1.player : who);
                        bool result = true;
                        return result;
                    }
                }
                */
                foreach (StardewValley.Farmer current3 in location.getFarmers())
                {
                    if (current3.GetBoundingBox().Intersects(this.boundingBox))
                    {
                        Game1.showRedMessage("Can't place on top of a person.");
                        bool result = false;
                        return result;
                    }
                }
                this.updateDrawPosition();
                return base.placementAction(location, x * Game1.tileSize, y * Game1.tileSize, who);
            }
        }

        public override bool isPlaceable()
        {
            return true;
        }

        public override Rectangle getBoundingBox(Vector2 tileLocation)
        {
            return this.boundingBox;
        }

        public override int salePrice()
        {
            return this.price;
        }

        public override int maximumStackSize()
        {
            return 999;
        }

        public override int getStack()
        {
            return this.stack;
        }


        private float getScaleSize()
        {
            int num = this.sourceRect.Width / 16;
            int num2 = this.sourceRect.Height / 16;
            if (num >= 5)
            {
                return 0.75f;
            }
            if (num2 >= 3)
            {
                return 1f;
            }
            if (num <= 2)
            {
                return 2f;
            }
            if (num <= 4)
            {
                return 1f;
            }
            return 0.1f;
        }

        public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, StardewValley.Farmer f)
        {
            if (f.ActiveObject.bigCraftable)
            {
                spriteBatch.Draw(Game1.bigCraftableSpriteSheet, objectPosition, new Microsoft.Xna.Framework.Rectangle?(StardewValley.Object.getSourceRectForBigCraftable(f.ActiveObject.ParentSheetIndex)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + 2) / 10000f));
                return;
            }
            spriteBatch.Draw(Game1.objectSpriteSheet, objectPosition, new Microsoft.Xna.Framework.Rectangle?(Game1.currentLocation.getSourceRectForObject(f.ActiveObject.ParentSheetIndex)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + 2) / 10000f));
            if (f.ActiveObject != null && f.ActiveObject.Name.Contains("="))
            {
                spriteBatch.Draw(Game1.objectSpriteSheet, objectPosition + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), new Microsoft.Xna.Framework.Rectangle?(Game1.currentLocation.getSourceRectForObject(f.ActiveObject.ParentSheetIndex)), Color.White, 0f, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), (float)Game1.pixelZoom + Math.Abs(Game1.starCropShimmerPause) / 8f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + 2) / 10000f));
                if (Math.Abs(Game1.starCropShimmerPause) <= 0.05f && Game1.random.NextDouble() < 0.97)
                {
                    return;
                }
                Game1.starCropShimmerPause += 0.04f;
                if (Game1.starCropShimmerPause >= 0.8f)
                {
                    Game1.starCropShimmerPause = -0.8f;
                }
            }
            //base.drawWhenHeld(spriteBatch, objectPosition, f);
        }

        public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
        {

            //spriteBatch.Draw(TextureSheet, location + new Vector2((float)(Game1.tileSize), (float)(Game1.tileSize)), new Rectangle?(this.sourceRect), Color.White * transparency, 0f, new Vector2((float)(this.sourceRect.Width / 2), (float)(this.sourceRect.Height)), 1f * 3, SpriteEffects.None, layerDepth);
            spriteBatch.Draw(TextureSheet, location + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), new Rectangle?(this.defaultSourceRect), Color.White * transparency, 0f, new Vector2((float)(this.defaultSourceRect.Width / 2), (float)(this.defaultSourceRect.Height / 2)), 1f * this.getScaleSize() * scaleSize * 1.5f, SpriteEffects.None, layerDepth);

            if (drawStackNumber && this.maximumStackSize() > 1 && ((double)scaleSize > 0.3 && this.Stack != int.MaxValue) && this.Stack > 1)
                Utility.drawTinyDigits(this.stack, spriteBatch, location + new Vector2((float)(Game1.tileSize - Utility.getWidthOfTinyDigitString(this.stack, 3f * scaleSize)) + 3f * scaleSize, (float)((double)Game1.tileSize - 18.0 * (double)scaleSize + 2.0)), 3f * scaleSize, 1f, Color.White);
            if (drawStackNumber && this.quality > 0)
            {
                float num = this.quality < 4 ? 0.0f : (float)((Math.Cos((double)Game1.currentGameTime.TotalGameTime.Milliseconds * Math.PI / 512.0) + 1.0) * 0.0500000007450581);
                spriteBatch.Draw(Game1.mouseCursors, location + new Vector2(12f, (float)(Game1.tileSize - 12) + num), new Microsoft.Xna.Framework.Rectangle?(this.quality < 4 ? new Microsoft.Xna.Framework.Rectangle(338 + (this.quality - 1) * 8, 400, 8, 8) : new Microsoft.Xna.Framework.Rectangle(346, 392, 8, 8)), Color.White * transparency, 0.0f, new Vector2(4f, 4f), (float)(3.0 * (double)scaleSize * (1.0 + (double)num)), SpriteEffects.None, layerDepth);
            }
        }

        public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
        {
            if (x == -1)
            {
                spriteBatch.Draw(TextureSheet, Game1.GlobalToLocal(Game1.viewport, this.drawPosition), new Rectangle?(this.sourceRect), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (this.Decoration_type == 12) ? 0f : ((float)(this.boundingBox.Bottom - 8) / 10000f));
            }
            else
            {
                spriteBatch.Draw(TextureSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize - (this.sourceRect.Height * Game1.pixelZoom - this.boundingBox.Height)))), new Rectangle?(this.sourceRect), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (this.Decoration_type == 12) ? 0f : ((float)(this.boundingBox.Bottom - 8) / 10000f));
            }
            if (this.heldObject != null)
            {
                if (this.heldObject is ModularCropObject)
                {
                    (this.heldObject as ModularCropObject).drawAtNonTileSpot(spriteBatch, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.boundingBox.Center.X - Game1.tileSize / 2), (float)(this.boundingBox.Center.Y - (this.heldObject as ModularCropObject).sourceRect.Height * Game1.pixelZoom - Game1.tileSize / 4))), (float)(this.boundingBox.Bottom - 7) / 10000f, alpha);
                    return;
                }
                spriteBatch.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.boundingBox.Center.X - Game1.tileSize / 2), (float)(this.boundingBox.Center.Y - Game1.tileSize * 4 / 3))) + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize * 5 / 6)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White * alpha, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 4f, SpriteEffects.None, (float)this.boundingBox.Bottom / 10000f);
                spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.boundingBox.Center.X - Game1.tileSize / 2), (float)(this.boundingBox.Center.Y - Game1.tileSize * 4 / 3))), new Rectangle?(Game1.currentLocation.getSourceRectForObject(this.heldObject.ParentSheetIndex)), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(this.boundingBox.Bottom + 1) / 10000f);
            }
        }

        public virtual void drawAtNonTileSpot(SpriteBatch spriteBatch, Vector2 location, float layerDepth, float alpha = 1f)
        {
            spriteBatch.Draw(TextureSheet, location, new Rectangle?(this.sourceRect), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth);
        }

        public override Item getOne()
        {
            ModularCropObject ModularCropObject = new ModularCropObject(this.parentSheetIndex,this.stack,this.texturePath,this.dataFileName);

            ModularCropObject.drawPosition = this.drawPosition;
            ModularCropObject.defaultBoundingBox = this.defaultBoundingBox;
            ModularCropObject.boundingBox = this.boundingBox;
            ModularCropObject.currentRotation = this.currentRotation - 1;
            ModularCropObject.rotations = this.rotations;
            //rotate();

            return ModularCropObject;
        }

        public virtual void resetTexture()
        {
            TextureSheet = Game1.content.Load<Texture2D>(this.texturePath);
        }

        public override string getCategoryName()
        {
            if (this.cropType != "") return ("Modded Crop:"+this.cropType);
            return "Modded Crop";
            //return "Modded Crop";
        }

        public override Color getCategoryColor()
        {
            return Color.DarkBlue;
            
        }


        public static new void Serialize(Item I)
        {
        
            StardustCore.ModCore.SerializationManager.WriteToJsonFile(Path.Combine(StardustCore.ModCore.SerializationManager.playerInventoryPath, I.Name + ".json"), (ModularCropObject)I);
        }

        public static Item ParseIntoInventory(string s)
        {
            // PlanterBox p = new PlanterBox();
            // return p;



            dynamic obj = JObject.Parse(s);

           ModularCropObject d = new ModularCropObject();

            d.dataFileName= obj.dataFileName;
            d.price = obj.price;
            d.Decoration_type = obj.Decoration_type;
            d.rotations = obj.rotations;
            d.currentRotation = obj.currentRotation;
            string s1 = Convert.ToString(obj.sourceRect);
            d.sourceRect = Utilities.parseRectFromJson(s1);
            string s2 = Convert.ToString(obj.defaultSourceRect);
            d.defaultSourceRect = Utilities.parseRectFromJson(s2);
            string s3 = Convert.ToString(obj.defaultBoundingBox);
            d.defaultBoundingBox = Utilities.parseRectFromJson(s3);
            d.description = obj.description;
            d.flipped = obj.flipped;
            d.flaggedForPickUp = obj.flaggedForPickUp;
            d.tileLocation = obj.tileLocation;
            d.parentSheetIndex = obj.parentSheetIndex;
            d.owner = obj.owner;
            d.name = obj.name;
            d.type = obj.type;
            d.canBeSetDown = obj.canBeSetDown;
            d.canBeGrabbed = obj.canBeGrabbed;
            d.isHoedirt = obj.isHoedirt;
            d.isSpawnedObject = obj.isSpawnedObject;
            d.questItem = obj.questItem;
            d.isOn = obj.isOn;
            d.fragility = obj.fragility;
            d.edibility = obj.edibility;
            d.stack = obj.stack;
            d.quality = obj.quality;
            d.bigCraftable = obj.bigCraftable;
            d.setOutdoors = obj.setOutdoors;
            d.setIndoors = obj.setIndoors;
            d.readyForHarvest = obj.readyForHarvest;
            d.showNextIndex = obj.showNextIndex;
            d.hasBeenPickedUpByFarmer = obj.hasBeenPickedUpByFarmer;
            d.isRecipe = obj.isRecipe;
            d.isLamp = obj.isLamp;
            d.heldObject = obj.heldObject;
            d.minutesUntilReady = obj.minutesUntilReady;
            string s4 = Convert.ToString(obj.boundingBox);
            d.boundingBox = Utilities.parseRectFromJson(s4);
            d.scale = obj.scale;
            d.lightSource = obj.lightSource;
            d.shakeTimer = obj.shakeTimer;
            d.internalSound = obj.internalSound;
            d.specialVariable = obj.specialVariable;
            d.category = obj.category;
            d.specialItem = obj.specialItem;
            d.hasBeenInInventory = obj.hasBeenInInventory;


            string t = obj.texturePath;
            d.TextureSheet = ModCore.ModHelper.Content.Load<Texture2D>(Path.Combine(Utilities.EntensionsFolderName, t));
            d.texturePath = t;
            d.itemReadyForHarvest = obj.itemReadyForHarvest;
            d.lightsOn = obj.lightsOn;
            d.lightColor = obj.lightColor;
            d.thisType = obj.thisType;
            d.removable = obj.removable;
            d.locationsName = obj.locationsName;
            d.cropType = obj.cropType;
            d.drawColor = obj.drawColor;
            d.serializationName = obj.serializationName;
            //ANIMATIONS
            var q = obj.animationManager;
            dynamic obj1 = q;

           ModularCropObject dummy = new ModularCropObject(d.parentSheetIndex, d.stack, d.texturePath, d.dataFileName);
            d.animationManager = dummy.animationManager;
            try
            {
                string name = Convert.ToString(obj1.currentAnimationName);
                int frame = Convert.ToInt32(obj1.currentAnimationListIndex);
                bool f = d.animationManager.setAnimation(name, frame);
                bool f2;
                if (f == false)
                {
                    f2 = d.animationManager.setAnimation(name, 0);
                    if (f2 == false) d.animationManager.currentAnimation = d.animationManager.defaultDrawFrame;
                }
            }
            catch(Exception err)
            {

            }

            try
            {
                return d;
            }
            catch (Exception e)
            {
                // Log.AsyncM(e);
                return null;
            }

            //return base.ParseIntoInventory();
        }

        public static void SerializeFromWorld(Item I)
        {
            //  ModCore.serilaizationManager.WriteToJsonFile(Path.Combine(ModCore.serilaizationManager.objectsInWorldPath, (c as CoreObject).thisLocation.name, c.Name + ".json"), (PlanterBox)c);
            StardustCore.ModCore.SerializationManager.WriteToJsonFile(Path.Combine(StardustCore.ModCore.SerializationManager.objectsInWorldPath, I.Name + ".json"), (ModularCropObject)I);
        }






    }
}
