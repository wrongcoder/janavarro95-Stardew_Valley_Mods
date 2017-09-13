using AdditionalCropsFramework.Framework;
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
    /// Original Stardew Furniture Class but rewritten to be placed anywhere.
    /// </summary>
    public class PlanterBox : CoreObject
    {


        public new int price;

        public int Decoration_type;

        public int rotations;

        public int currentRotation;

        private int sourceIndexOffset;

        public Vector2 drawPosition;

        public Rectangle sourceRect;

        public Rectangle defaultSourceRect;

        public Rectangle defaultBoundingBox;

        public string description;

        public Texture2D TextureSheet;

        public new bool flipped;

        [XmlIgnore]
        public bool flaggedForPickUp;

        private bool lightGlowAdded;

        public string texturePath;
        public string dataPath;

        public bool IsSolid;

        public Crop crop;
        public ModularCrop modularCrop;
        public bool isWatered;
        public string cropInformationString;

        public string normalCropSeedName;
        public int normalCropSeedIndex;

        public string serializationName="AdditionalCropsFramework.PlanterBox";

        public override string Name
        {
            get
            {
                return this.name;
            }
        }

        public PlanterBox()
        {
            this.updateDrawPosition();
        }

        public PlanterBox(bool f)
        {
            //does nothng
        }

        /// <summary>
        /// Don't use this unless you just want to lol with some defaults.
        /// </summary>
        /// <param name="which"></param>
        /// <param name="tile"></param>
        /// <param name="isRemovable"></param>
        /// <param name="price"></param>
        /// <param name="isSolid"></param>
        public PlanterBox(int which, Vector2 tile, bool isRemovable = true, int price = 0, bool isSolid = false)
        {
            this.cropInformationString = "";
            removable = isRemovable;
            this.serializationName =Convert.ToString(GetType());
            // this.thisType = GetType();
            this.tileLocation = tile;
            this.InitializeBasics(0, tile);
            if (TextureSheet == null)
            {
                TextureSheet = ModCore.ModHelper.Content.Load<Texture2D>(Path.Combine(Utilities.EntensionsFolderName, "PlanterBox.png")); //Game1.content.Load<Texture2D>("TileSheets\\furniture");
                texturePath = "PlanterBoxGraphic";
            }
            dataPath = "";

            this.name = "Planter Box";
            this.description = "A planter box that can be used to grow many different crops in many different locations.";
            this.defaultSourceRect = new Rectangle(which * 16 % TextureSheet.Width, which * 16 / TextureSheet.Width * 16, 1, 1);

            this.defaultSourceRect.Width = 1;
            this.defaultSourceRect.Height = 1;
            this.sourceRect = new Rectangle(which * 16 % TextureSheet.Width, which * 16 / TextureSheet.Width * 16, this.defaultSourceRect.Width * 16, this.defaultSourceRect.Height * 16);
            this.defaultSourceRect = this.sourceRect;

            this.animationManager = new StardustCore.Animations.AnimationManager(this.TextureSheet, new StardustCore.Animations.Animation(this.defaultSourceRect, -1));

            this.defaultBoundingBox = new Rectangle((int)this.tileLocation.X, (int)this.tileLocation.Y, 1, 1);

            this.defaultBoundingBox.Width = 1;
            this.defaultBoundingBox.Height = 1;
            IsSolid = isSolid;
            if (isSolid == true)
            {
                this.boundingBox = new Rectangle((int)this.tileLocation.X * Game1.tileSize, (int)this.tileLocation.Y * Game1.tileSize, this.defaultBoundingBox.Width * Game1.tileSize, this.defaultBoundingBox.Height * Game1.tileSize);
            }
            else
            {
                this.boundingBox = new Rectangle(int.MinValue, (int)this.tileLocation.Y * Game1.tileSize, 0, 0); //Throw the bounding box away as far as possible.
            }
            this.defaultBoundingBox = this.boundingBox;
            this.updateDrawPosition();
            this.price = price;
            this.parentSheetIndex = which;
        }

        /// <summary>
        /// Planterbox Constructor. Does not use an animation manager.
        /// </summary>
        /// <param name="which"></param>
        /// <param name="tile"></param>
        /// <param name="ObjectTexture"></param>
        /// <param name="isRemovable"></param>
        /// <param name="price"></param>
        /// <param name="isSolid"></param>
        public PlanterBox(int which, Vector2 tile, string ObjectTexture, bool isRemovable = true, int price = 0, bool isSolid = false)
        {
      
            this.cropInformationString = "";
            removable = isRemovable;
            this.serializationName = Convert.ToString(GetType());
            // this.thisType = GetType();
            this.tileLocation = tile;
            this.InitializeBasics(0, tile);
            if (TextureSheet == null)
            {
                TextureSheet = ModCore.ModHelper.Content.Load<Texture2D>(Path.Combine(Utilities.EntensionsFolderName, ObjectTexture)); //Game1.content.Load<Texture2D>("TileSheets\\furniture");
                texturePath = ObjectTexture;
            }
            this.dataPath = "";

            this.name = "Planter Box";
            this.description = "A planter box that can be used to grow many different crops in many different locations.";

          

            this.defaultSourceRect = new Rectangle(which * 16 % TextureSheet.Width, which * 16 / TextureSheet.Width * 16, 1, 1);

            this.defaultSourceRect.Width = 1;
            this.defaultSourceRect.Height = 1;
            this.sourceRect = new Rectangle(which * 16 % TextureSheet.Width, which * 16 / TextureSheet.Width * 16, this.defaultSourceRect.Width * 16, this.defaultSourceRect.Height * 16);
            this.defaultSourceRect = this.sourceRect;

            this.defaultBoundingBox = new Rectangle((int)this.tileLocation.X, (int)this.tileLocation.Y, 1, 1);

            this.defaultBoundingBox.Width = 1;
            this.defaultBoundingBox.Height = 1;
            IsSolid = isSolid;
            if (isSolid == true)
            {
                this.boundingBox = new Rectangle((int)this.tileLocation.X * Game1.tileSize, (int)this.tileLocation.Y * Game1.tileSize, this.defaultBoundingBox.Width * Game1.tileSize, this.defaultBoundingBox.Height * Game1.tileSize);
            }
            else
            {
                this.boundingBox = new Rectangle(int.MinValue, (int)this.tileLocation.Y * Game1.tileSize, 0, 0); //Throw the bounding box away as far as possible.
            }
            this.defaultBoundingBox = this.boundingBox;
            this.updateDrawPosition();
            this.price = price;
            this.parentSheetIndex = which;
        }


        public PlanterBox(int which, Vector2 tile, string ObjectTexture, string DataPath, bool isRemovable = true, bool isSolid = false)
        {
            this.cropInformationString = "";
            this.serializationName = Convert.ToString(GetType());
            removable = isRemovable;
            // this.thisType = GetType();
            this.tileLocation = tile;
            this.InitializeBasics(0, tile);
                TextureSheet = ModCore.ModHelper.Content.Load<Texture2D>(Path.Combine(Utilities.EntensionsFolderName, ObjectTexture)); //Game1.content.Load<Texture2D>("TileSheets\\furniture");
                texturePath = ObjectTexture;
            Dictionary<int, string> dictionary;
            try
            {
                
                    dictionary = ModCore.ModHelper.Content.Load<Dictionary<int, string>>(Path.Combine(Utilities.EntensionsFolderName, DataPath));
                    dataPath = DataPath;
                

                string s = "";
                dictionary.TryGetValue(which, out s);
                string[] array = s.Split('/');
                this.name = array[0];
                this.description = array[1];
                this.defaultSourceRect = new Rectangle(which * 16 % TextureSheet.Width, which * 16 / TextureSheet.Width * 16, 1, 1);

                this.defaultSourceRect.Width = 1;
                this.defaultSourceRect.Height = 1;
                this.sourceRect = new Rectangle(which * 16 % TextureSheet.Width, which * 16 / TextureSheet.Width * 16, this.defaultSourceRect.Width * 16, this.defaultSourceRect.Height * 16);
                this.defaultSourceRect = this.sourceRect;
                try
                {                
                    this.animationManager = new StardustCore.Animations.AnimationManager(this.TextureSheet, new StardustCore.Animations.Animation(this.defaultSourceRect, -1), AnimationManager.parseAnimationsFromXNB(array[3]), "default");
                    this.animationManager.setAnimation("default", 0);
                    //Log.AsyncC("Using animation manager");
                }
                catch (Exception errr)
                {
                    this.animationManager = new StardustCore.Animations.AnimationManager(this.TextureSheet, new StardustCore.Animations.Animation(this.defaultSourceRect, -1));
                }
                this.defaultBoundingBox = new Rectangle((int)this.tileLocation.X, (int)this.tileLocation.Y, 1, 1);

                this.defaultBoundingBox.Width = 1;
                this.defaultBoundingBox.Height = 1;
                IsSolid = isSolid;
                if (isSolid == true)
                {
                    this.boundingBox = new Rectangle((int)this.tileLocation.X * Game1.tileSize, (int)this.tileLocation.Y * Game1.tileSize, this.defaultBoundingBox.Width * Game1.tileSize, this.defaultBoundingBox.Height * Game1.tileSize);
                }
                else
                {
                    this.boundingBox = new Rectangle(int.MinValue, (int)this.tileLocation.Y * Game1.tileSize, 0, 0); //Throw the bounding box away as far as possible.
                }
                this.defaultBoundingBox = this.boundingBox;
                this.updateDrawPosition();
                this.price = Convert.ToInt32(array[2]);
                this.parentSheetIndex = which;
                
            }
            catch(Exception e)
            {
              //  Log.AsyncC(e);
            }

        }


        public override string getDescription()
        {
            return this.description;
        }

        public override bool performDropDownAction(StardewValley.Farmer who)
        {
            this.resetOnPlayerEntry((who == null) ? Game1.currentLocation : who.currentLocation);
            return false;
        }

        public override void hoverAction()
        {
            base.hoverAction();
            if (!Game1.player.isInventoryFull())
            {
                Game1.mouseCursor = 2;
            }
            
        }

        public override bool checkForAction(StardewValley.Farmer who, bool justCheckingForActivity = false)
        {
            var mState = Microsoft.Xna.Framework.Input.Mouse.GetState();
            if (mState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                // Game1.showRedMessage("YOOO");
                //do some stuff when the right button is down
                // rotate();
                if (Game1.player.ActiveObject == null) return false;
                if (Game1.player.ActiveObject is ModularSeeds || Game1.player.ActiveObject.getCategoryName() == "Modular Seeds")
                {
                    this.plantModdedCrop((Game1.player.ActiveObject as ModularSeeds));
                   // Log.AsyncO("Modded seeds");
                }
               // Log.AsyncO(Game1.player.CurrentItem.getCategoryName());
                if (Game1.player.CurrentItem.getCategoryName() == "Seed" || Game1.player.CurrentItem.getCategoryName() == "seed")
                {
                   this.plantRegularCrop();
                   // Log.AsyncY("regular seeds");
                }



                Game1.playSound("coin");
                return true;
            }
            else
            {
                //Game1.showRedMessage("CRY");
            }

            if (justCheckingForActivity)
            {
                return true;
            }
            if (this.parentSheetIndex == 1402)
            {
                Game1.activeClickableMenu = new Billboard(false);
            }
            return this.clicked(who); //check for left clicked action.
        }

        public void plantModdedCrop(ModularSeeds seeds)
        {
            if (this.modularCrop != null) return;
          this.modularCrop = new ModularCrop(seeds.parentSheetIndex, (int)Game1.currentCursorTile.X, (int)Game1.currentCursorTile.Y, seeds.cropDataFilePath, seeds.cropTextureFilePath, seeds.cropObjectTextureFilePath, seeds.cropObjectDataFilePath);
         // Game1.player.reduceActiveItemByOne();
          Game1.playSound("dirtyHit");
        }

        public void plantRegularCrop()
        {
            if (this.crop != null) return;
            this.normalCropSeedName = Game1.player.ActiveObject.name;
            this.normalCropSeedIndex = Game1.player.ActiveObject.parentSheetIndex;
            this.crop = new Crop(Game1.player.ActiveObject.parentSheetIndex, (int)Game1.currentCursorTile.X, (int)Game1.currentCursorTile.Y);
            Game1.player.reduceActiveItemByOne();
            Game1.playSound("dirtyHit");
        }



        public override bool clicked(StardewValley.Farmer who)
        {
            if (Game1.player.CurrentItem != null)
            {
                if (Game1.player.getToolFromName(Game1.player.CurrentItem.Name) is StardewValley.Tools.WateringCan)
                {
                    this.isWatered = true;
                    this.animationManager.setAnimation("watered", 0);
                    return false;
                }
            }
            if (this.crop != null)
            {
                if (this.crop.fullyGrown == true)
                {
                    //this.crop.harvest();
                }
            }
            if (this.modularCrop != null)
            {
                Log.AsyncM("HELLO MOD CROP");
                if (this.modularCrop.isFullyGrown() == true)
                {
                    Log.AsyncM("FULL BLOW");
                    bool f = Utilities.harvestModularCrop(this.modularCrop, (int)this.tileLocation.X, (int)this.tileLocation.Y, 0);
                    if (f == true)
                    {
                        //this.modularCrop = null;
                        Log.AsyncO("HARVEST");
                        return false;
                    }
                    else
                    {
                        Log.AsyncC("failed to harvest crop. =/");
                    }
                }
            }
            else
            {
                Log.AsyncR("WTF???");
            }
            


            if (removable == false) return false;
            //   Game1.showRedMessage("THIS IS CLICKED!!!");
            Game1.haltAfterCheck = false;
            if (this.Decoration_type == 11 && who.ActiveObject != null && who.ActiveObject != null && this.heldObject == null)
            {
                //  Game1.showRedMessage("Why1?");
                return false;
            }
            if (this.heldObject == null && (who.ActiveObject == null || !(who.ActiveObject is PlanterBox)))
            {
                if (Game1.player.currentLocation is FarmHouse)
                {
                    //   Game1.showRedMessage("Why2?");
                    // this.heldObject = new PlanterBox(parentSheetIndex, Vector2.Zero);
                    Utilities.addItemToInventoryAndCleanTrackedList(this);
                    this.flaggedForPickUp = true;
                    this.thisLocation = null;
                    this.locationsName = "";
                    return true;
                }
                else
                {
                    // return true;

                    this.flaggedForPickUp = true;
                    if (this is TV)
                    {
                        this.heldObject = new TV(parentSheetIndex, Vector2.Zero);
                    }
                    else
                    {
                        //  this.heldObject = new PlanterBox(parentSheetIndex, Vector2.Zero);
                        Utilities.addItemToInventoryAndCleanTrackedList(this);
                        //  this.heldObject.performRemoveAction(this.tileLocation, who.currentLocation);
                        //   this.heldObject = null;
                        Game1.playSound("coin");
                        this.thisLocation = null;
                        this.locationsName = "";
                        return true;
                    }

                }
            }
            if (this.heldObject != null && who.addItemToInventoryBool(this.heldObject, false))
            {
                // Game1.showRedMessage("Why3?");
                this.heldObject.performRemoveAction(this.tileLocation, who.currentLocation);
                this.heldObject = null;
                Utilities.addItemToInventoryAndCleanTrackedList(this);
                Game1.playSound("coin");
                this.thisLocation = null;
                this.locationsName = "";
                return true;
            }



            return false;
        }

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

        public void dayUpdate()
        {
            Log.AsyncC("HELLO?!?!?");
            if (this.isWatered==true)
            {
                if (this.crop != null)
                {
                    Utilities.cropNewDay(this.crop,1, 0, (int)this.tileLocation.X, (int)this.tileLocation.Y, this.thisLocation);
                   
                }

                if (this.modularCrop != null)
                {
                   Utilities.cropNewDayModded(this.modularCrop,1, 0, (int)this.tileLocation.X, (int)this.tileLocation.Y, this.thisLocation);
                }
                Log.AsyncC("DRINK YOUR WATER");
                this.isWatered = false;
                this.animationManager.setAnimation("default", 0);
            }
            else
            {
                Log.AsyncC("No Water Here");
                this.animationManager.setAnimation("default", 0);
            }
        }

        public void resetOnPlayerEntry(GameLocation environment)
        {
            this.removeLights(environment);
            if (Game1.isDarkOut())
            {
                this.addLights(environment);
            }
        }

        public override bool performObjectDropInAction(StardewValley.Object dropIn, bool probe, StardewValley.Farmer who)
        {
            if ((this.Decoration_type == 11 || this.Decoration_type == 5) && this.heldObject == null && !dropIn.bigCraftable && (!(dropIn is PlanterBox) || ((dropIn as PlanterBox).getTilesWide() == 1 && (dropIn as PlanterBox).getTilesHigh() == 1)))
            {
                this.heldObject = (StardewValley.Object)dropIn.getOne();
                this.heldObject.tileLocation = this.tileLocation;
                this.heldObject.boundingBox.X = this.boundingBox.X;
                this.heldObject.boundingBox.Y = this.boundingBox.Y;
                //  Log.AsyncO(getDefaultBoundingBoxForType((dropIn as PlanterBox).Decoration_type));
                this.heldObject.performDropDownAction(who);
                if (!probe)
                {
                    Game1.playSound("woodyStep");
                    //   Log.AsyncC("HUH?");
                    if (who != null)
                    {
                        who.reduceActiveItemByOne();
                    }
                }
                return true;
            }
            return false;
        }

        private void addLights(GameLocation environment)
        {
            // this.lightSource.lightTexture = Game1.content.Load<Texture2D>("LooseSprites\\Lighting\\lantern");

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
                    this.lightSource = new LightSource(4, new Vector2((float)(this.boundingBox.X + Game1.tileSize / 2), (float)(this.boundingBox.Y - Game1.tileSize)), 2f, Color.Black, (int)(this.tileLocation.X * 2000f + this.tileLocation.Y));
                    Game1.currentLightSources.Add(this.lightSource);
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
        }

        public override bool minutesElapsed(int minutes, GameLocation environment)
        {
            if (Game1.isDarkOut())
            {
                this.addLights(environment);
            }
            else
            {
                this.removeLights(environment);
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
            base.performRemoveAction(tileLocation, environment);
        }
        public bool isGroundFurniture()
        {
            return this.Decoration_type != 13 && this.Decoration_type != 6 && this.Decoration_type != 13;
        }

        public override bool canBeGivenAsGift()
        {
            return false;
        }

        public override bool canBePlacedHere(GameLocation l, Vector2 tile)
        {
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
                            if ((obj.GetType()).ToString().Contains("PlanterBox"))
                            {
                                PlanterBox current = (PlanterBox)obj;
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
                return base.canBePlacedHere(l, tile);
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
                        foreach (PlanterBox current in (l as FarmHouse).PlanterBox)
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
                return base.canBePlacedHere(l, tile);
            }
        }

        public void updateDrawPosition()
        {
            this.drawPosition = new Vector2((float)this.boundingBox.X, (float)(this.boundingBox.Y - (this.sourceRect.Height * Game1.pixelZoom - this.boundingBox.Height)));
        }

        public int getTilesWide()
        {
            return this.boundingBox.Width / Game1.tileSize;
        }

        public int getTilesHigh()
        {
            return this.boundingBox.Height / Game1.tileSize;
        }

        public override bool placementAction(GameLocation location, int x, int y, StardewValley.Farmer who = null)
        {

            Point point = new Point(x / Game1.tileSize, y / Game1.tileSize);


            this.tileLocation = new Vector2((float)point.X, (float)point.Y);
            bool flag = false;

            if (this.IsSolid)
            {
                this.boundingBox = new Rectangle(x / Game1.tileSize * Game1.tileSize, y / Game1.tileSize * Game1.tileSize, this.boundingBox.Width, this.boundingBox.Height);
            }
            else
            {
                this.boundingBox = new Rectangle(int.MinValue, y / Game1.tileSize * Game1.tileSize, 0, 0);
            }
                /*
            foreach (Furniture current2 in (location as DecoratableLocation).furniture)
                {
                    if (current2.furniture_type == 11 && current2.heldObject == null && current2.getBoundingBox(current2.tileLocation).Intersects(this.boundingBox))
                    {
                        current2.performObjectDropInAction(this, false, (who == null) ? Game1.player : who);
                        bool result = true;
                        return result;
                    }
                }
                */
            using (List<StardewValley.Farmer>.Enumerator enumerator3 = location.getFarmers().GetEnumerator())
            {
                while (enumerator3.MoveNext())
                {
                    if (enumerator3.Current.GetBoundingBox().Intersects(this.boundingBox))
                    {
                        Game1.showRedMessage("Can't place on top of a person.");
                        bool result = false;
                        return result;
                    }
                }
            }
            this.updateDrawPosition();

            bool f=Utilities.placementAction(this, location, x, y, who);
            this.thisLocation = Game1.player.currentLocation;
            return f;
            //  Game1.showRedMessage("Can only be placed in House");
            //  return false;
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
            return 1;
        }

        public override int getStack()
        {
            return this.stack;
        }

        public override int addToStack(int amount)
        {
            return 1;
        }

        public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, StardewValley.Farmer f)
        {
            if (animationManager == null)
            {
                spriteBatch.Draw(this.TextureSheet, objectPosition, new Microsoft.Xna.Framework.Rectangle?(Game1.currentLocation.getSourceRectForObject(f.ActiveObject.ParentSheetIndex)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + 2) / 10000f));
            }
            else
            {
                spriteBatch.Draw(this.TextureSheet, objectPosition,this.animationManager.currentAnimation.sourceRectangle, Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + 2) / 10000f));
            }

            if (f.ActiveObject != null && f.ActiveObject.Name.Contains("="))
            {
                spriteBatch.Draw(this.TextureSheet, objectPosition + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), new Microsoft.Xna.Framework.Rectangle?(Game1.currentLocation.getSourceRectForObject(f.ActiveObject.ParentSheetIndex)), Color.White, 0f, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), (float)Game1.pixelZoom + Math.Abs(Game1.starCropShimmerPause) / 8f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + 2) / 10000f));
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
            if(animationManager==null) spriteBatch.Draw(TextureSheet, location + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), new Rectangle?(this.defaultSourceRect), Color.White * transparency, 0f, new Vector2((float)(this.defaultSourceRect.Width / 2), (float)(this.defaultSourceRect.Height / 2)), 1f * (3) * scaleSize, SpriteEffects.None, layerDepth);
            else
            {
            spriteBatch.Draw(animationManager.objectTexture, location + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), new Rectangle?(animationManager.currentAnimation.sourceRectangle), Color.White * transparency, 0f, new Vector2((float)(this.defaultSourceRect.Width / 2), (float)(this.defaultSourceRect.Height / 2)), 1f * (3) * scaleSize, SpriteEffects.None, layerDepth);
            if (Game1.player.CurrentItem != this) animationManager.tickAnimation();
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
                //The actual planter box being drawn.
                if (animationManager == null)
                {
                    spriteBatch.Draw(TextureSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), y * Game1.tileSize)), new Rectangle?(this.sourceRect), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                   // Log.AsyncG("ANIMATION IS NULL?!?!?!?!");
                }

                else
                {
                    //Log.AsyncC("Animation Manager is working!");
                    spriteBatch.Draw(animationManager.objectTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), y * Game1.tileSize)), new Rectangle?(animationManager.currentAnimation.sourceRectangle), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                    try
                    {
                        this.animationManager.tickAnimation();
                       // Log.AsyncC("Tick animation");
                    }
                    catch (Exception err)
                    {
                        ModCore.ModMonitor.Log(err.ToString());
                    }
                }
                if (this.heldObject != null)
                {
                    if (this.heldObject is PlanterBox)
                    {
                        (this.heldObject as PlanterBox).drawAtNonTileSpot(spriteBatch, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.boundingBox.Center.X - Game1.tileSize / 2), (float)(this.boundingBox.Center.Y - (this.heldObject as PlanterBox).sourceRect.Height * Game1.pixelZoom - Game1.tileSize / 4))), (float)(this.boundingBox.Bottom - 7) / 10000f, alpha);
                        return;
                    }
                    spriteBatch.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.boundingBox.Center.X - Game1.tileSize / 2), (float)(this.boundingBox.Center.Y - Game1.tileSize * 4 / 3))) + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize * 5 / 6)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White * alpha, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 4f, SpriteEffects.None, (float)this.boundingBox.Bottom / 10000f);
                    spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.boundingBox.Center.X - Game1.tileSize / 2), (float)(this.boundingBox.Center.Y - Game1.tileSize * 4 / 3))), new Rectangle?(Game1.currentLocation.getSourceRectForObject(this.heldObject.ParentSheetIndex)), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(this.boundingBox.Bottom + 1) / 10000f);
                }
                this.drawCrops(Game1.spriteBatch, (int)this.tileLocation.X, (int)this.tileLocation.Y);

            }
        }


        public void drawCrops(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
        {
            if (this.thisLocation != null)
            {
                if (this.modularCrop != null)
                {
                    this.modularCrop.draw(Game1.spriteBatch, this.tileLocation, Color.White, 0);
                   // Log.AsyncM("draw a modular crop now");
                }
               // Log.AsyncC("wait WTF");

                if (this.crop != null)
                {
                    
                    this.crop.draw(Game1.spriteBatch, this.tileLocation, Color.White, 0);
                    //Log.AsyncG("COWS GO MOO");
                }
            }
           //else Log.AsyncM("I DONT UNDERSTAND");
        }

        public void drawAtNonTileSpot(SpriteBatch spriteBatch, Vector2 location, float layerDepth, float alpha = 1f)
        {
            spriteBatch.Draw(TextureSheet, location, new Rectangle?(this.sourceRect), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth);
        }

        public override Item getOne()
        {
           
            if (this.dataPath == "") return new PlanterBox(this.parentSheetIndex, this.tileLocation);
            else return  new PlanterBox(this.parentSheetIndex, this.tileLocation,this.texturePath,this.dataPath);

            /*
            drawPosition = this.drawPosition;
            defaultBoundingBox = this.defaultBoundingBox;
            boundingBox = this.boundingBox;
            currentRotation = this.currentRotation - 1;
            rotations = this.rotations;
            rotate();
            */
        }

        public override string getCategoryName()
        {
            return "Planter Box";
            //  return base.getCategoryName();
        }

        public override Color getCategoryColor()
        {
            return Color.Purple;
        }

        public static new void Serialize(Item I)
        {
            makeCropInformationString(I);
            ModCore.serilaizationManager.WriteToJsonFile(Path.Combine(ModCore.serilaizationManager.playerInventoryPath, I.Name + ".json"), (PlanterBox)I);
        }

        public static Item ParseIntoInventory(string s)
        {
           // PlanterBox p = new PlanterBox();
           // return p;



            dynamic obj = JObject.Parse(s);

            PlanterBox d = new PlanterBox();

            d.dataPath = obj.dataPath;
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
            JArray array = obj.inventory;
            d.inventory = array.ToObject<List<Item>>();
            d.inventoryMaxSize = obj.inventoryMaxSize;
            d.itemReadyForHarvest = obj.itemReadyForHarvest;
            d.lightsOn = obj.lightsOn;
            d.lightColor = obj.lightColor;
            d.thisType = obj.thisType;
            d.removable = obj.removable;
            d.locationsName = obj.locationsName;

            d.drawColor = obj.drawColor;

            d.cropInformationString = obj.cropInformationString;

            d.IsSolid = obj.IsSolid;

            string IsWatered = obj.isWatered;
            Log.AsyncC("AM I WATERED OR NOT?!?!?: "+IsWatered);
            d.isWatered = Convert.ToBoolean(IsWatered);
            //ANIMATIONS
            var q = obj.animationManager;
            dynamic obj1 = q;
            string name =Convert.ToString( obj1.currentAnimationName);
            int frame = Convert.ToInt32(obj1.currentAnimationListIndex);
            PlanterBox dummy = new PlanterBox(d.parentSheetIndex, d.tileLocation, d.texturePath, d.dataPath, d.removable, d.IsSolid);
            d.animationManager = dummy.animationManager;          
            bool f = d.animationManager.setAnimation(name, frame);
            bool f2;
            if (f == false)
            {
                f2 = d.animationManager.setAnimation(name, 0);
                if (f2 == false) d.animationManager.currentAnimation = d.animationManager.defaultDrawFrame;
            }
            // Log.AsyncC(d.cropInformationString);

            try
            {
                string[] cropInfo = d.cropInformationString.Split('/');

                foreach (var v in cropInfo)
                {
                  //  Log.AsyncM(v);
                  
                }

                if (cropInfo[0] == "true") //modular crop
                {
                    ModularCrop c = new ModularCrop(Convert.ToInt32(cropInfo[1]), Convert.ToInt32(cropInfo[2]), Convert.ToInt32(cropInfo[3]), cropInfo[4], cropInfo[5], cropInfo[6], cropInfo[7]);
                    c.currentPhase = Convert.ToInt32(cropInfo[8]);
                    c.dayOfCurrentPhase = Convert.ToInt32(cropInfo[9]);
                    try
                    {
                        c.fullyGrown = Convert.ToBoolean(cropInfo[10]);
                    }
                    catch(Exception e)
                    {
                        
                    }
                    d.modularCrop = c;
                    Log.AsyncM("PARSED MODULAR CROP!");
                    Log.AsyncG(cropInfo[8]);
                    Log.AsyncG(cropInfo[9]);
                    Log.AsyncG(cropInfo[10]);
                }
                if (cropInfo[0] == "false") //non-modular crop
                {
                    Crop c = new Crop(Convert.ToInt32(cropInfo[1]), Convert.ToInt32(cropInfo[2]), Convert.ToInt32(cropInfo[3]));
                    c.currentPhase = Convert.ToInt32(cropInfo[4]);
                    c.dayOfCurrentPhase = Convert.ToInt32(cropInfo[5]);
                    d.crop = c;
                  //  Log.AsyncM("PARSED REGULAR CROP!");
                }
            }
            catch (Exception err)
            {
               // Log.AsyncO(err);
            }
            //ModularCrop f=  j.ToObject<ModularCrop>();
            //ModularCrop f = obj.modularCrop;

           // Log.AsyncG("THIS IS CROP: " + c.indexOfHarvest);
            //Log.AsyncG(cropString);


           // d.crop = obj.crop;
           // d.modularCrop = obj.modularCrop;


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
            makeCropInformationString(I);
            //  ModCore.serilaizationManager.WriteToJsonFile(Path.Combine(ModCore.serilaizationManager.objectsInWorldPath, (c as CoreObject).thisLocation.name, c.Name + ".json"), (PlanterBox)c);
            ModCore.serilaizationManager.WriteToJsonFile(Path.Combine(ModCore.serilaizationManager.objectsInWorldPath, I.Name + ".json"), (PlanterBox)I);
        }

        public static void makeCropInformationString(Item I)
        {
            (I as PlanterBox).cropInformationString = "";

            if ((I as PlanterBox).crop != null)
            {
                Crop c = (I as PlanterBox).crop;
                (I as PlanterBox).cropInformationString = "false" + "/" + (I as PlanterBox).normalCropSeedIndex + "/" + (I as PlanterBox).tileLocation.X + "/" + (I as PlanterBox).tileLocation.Y + "/" + c.currentPhase + "/" + c.dayOfCurrentPhase + "/" + c.fullyGrown; ;
            }

            if ((I as PlanterBox).modularCrop != null)
            {
                ModularCrop m = (I as PlanterBox).modularCrop;
                (I as PlanterBox).cropInformationString = "true" + "/" + m.seedIndex + "/" + (I as PlanterBox).tileLocation.X + "/" + (I as PlanterBox).tileLocation.Y + "/" + m.dataFileName + "/" + m.spriteSheetName + "/" + m.cropObjectTexture + "/" + m.cropObjectData + "/" + m.currentPhase + "/" + m.dayOfCurrentPhase+"/"+m.fullyGrown;
            }
        }

        

    }
}