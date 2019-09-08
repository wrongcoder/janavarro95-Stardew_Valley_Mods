using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using PyTK.CustomElementHandler;
using StardustCore.Animations;
using StardewValley;
using StardewValley.Objects;
using Revitalize.Framework.Utilities;
using Revitalize.Framework.Energy;

namespace Revitalize.Framework.Objects
{
    // TODO:
    //     -Multiple Lights
    //     -Events when walking over?
    //     -Inventories

    /// <summary>A custom object template.</summary>
    public class CustomObject : PySObject,IEnergyInterface
    {

        public virtual string text
        {
            get
            {
                if (this.netName.Value.Split('>') is string[] split && split.Length > 1)
                    return split[1]; //This is custom data. If the net name has a much larger name split the value and return the result.
                else
                    return ""; //Otherwise return nothing.
            }
            set
            {
                if (this.netName == null) return;
                if (this.netName.Value == null) return;
                {
                    this.netName.Value = this.netName.Value.Split('>')[0] + ">" + value; //When setting the custom dataappend it to the end of the name.
                }
            }
        }


        public override string Name
        {

            get
            {
                if (this.info != null)
                {
                    return this.netName.Value.Split('>')[0];
                    //return this.info.name;
                }
                if (this.netName == null)
                {
                    return this.name;
                }
                else
                {
                    return this.netName.Value.Split('>')[0]; //Return the value before the name because that is the true value.
                }
            }

            set
            {
                if (this.netName == null)
                {
                    return;
                }
                if (this.netName.Value == null)
                {
                    return;
                }
                if (this.netName.Value.Split('>') is string[] split && split.Length > 1)
                {
                    this.netName.Value = value + ">" + split[1]; //When setting the name if appended data is added on set the new value and add that appended data back.
                }
                else
                {
                    this.netName.Value = value; //Otherwise just set the net name.
                }
            }
        }

        public override string DisplayName
        {
            get
            {
                if (this.info != null)
                {
                    return this.info.name;
                }
                return this.netName.Value.Split('>')[0];
            }

            set
            {
                if (this.netName == null) return;
                if (this.netName.Value == null) return;

                if (this.netName.Value.Split('>') is string[] split && split.Length > 1)
                    this.netName.Value = value + ">" + split[1];
                else
                    this.netName.Value = value;
            }
        }


        public string id
        {
            get
            {

                if (this.info == null)
                {
                    //ModCore.log("Info was null when getting data.");
                    this.updateInfo();
                }

                return this.info.id;
            }
        }


        public BasicItemInformation info;
        private GameLocation _location;
        [JsonIgnore]
        public GameLocation location
        {
            get
            {
                //ModCore.log("Location Name is: " + this.info.locationName);
                if (this._location == null)
                {
                    this._location = Game1.getLocationFromName(this.info.locationName);
                    return this._location;
                }
                else
                {
                    return Game1.getLocationFromName(this.info.locationName);
                }
                return this._location;
            }
            set
            {

                this._location = value;
                if (this._location == null) this.info.locationName = "";
                else
                {
                    this.info.locationName = this._location.Name;
                }
            }
        }


        public Guid guid;

        [JsonIgnore]
        /// <summary>The animation manager.</summary>
        public AnimationManager animationManager
        {
            get
            {
                this.updateInfo();
                return this.info.animationManager;
            }
        }

        /// <summary>
        /// Accesses the energy manager for all objects.
        /// </summary>
        public EnergyManager EnergyManager
        {
            get
            {
                if (this.info == null)
                {
                    this.updateInfo();
                    return this.info.EnergyManager;
                }
                else
                {
                    return this.info.EnergyManager;
                }
            }
            set
            {
                this.info.EnergyManager = value;
            }
        }

        /// <summary>The display texture for this object.</summary>
        [JsonIgnore]
        public Texture2D displayTexture => this.animationManager.getTexture();

        [JsonIgnore]
        public virtual string ItemInfo
        {
            get
            {
                return Revitalize.ModCore.Serializer.ToJSONString(this.info) + "<" + this.guid + "<" + ModCore.Serializer.ToJSONString(this.data);
            }
            set
            {
                if (string.IsNullOrEmpty(value)) return;
                string[] data = value.Split('<');
                string infoString = data[0];
                string guidString = data[1];
                string pytkData = data[2];

                this.info = (BasicItemInformation)Revitalize.ModCore.Serializer.DeserializeFromJSONString(infoString, typeof(BasicItemInformation));
                this.data = ModCore.Serializer.DeserializeFromJSONString<CustomObjectData>(pytkData);
                Guid oldGuid = this.guid;
                this.guid = Guid.Parse(guidString);
                if (ModCore.CustomObjects.ContainsKey(this.guid))
                {
                    //ModCore.log("Update item with guid: " + this.guid);
                    ModCore.CustomObjects[this.guid] = this;
                }
                else
                {
                    //ModCore.log("Add in new guid: " + this.guid);
                    ModCore.CustomObjects.Add(this.guid, this);
                }

                if (ModCore.CustomObjects.ContainsKey(oldGuid) && ModCore.CustomObjects.ContainsKey(this.guid))
                {
                    if (ModCore.CustomObjects[oldGuid] == ModCore.CustomObjects[this.guid] && oldGuid != this.guid)
                    {
                        //ModCore.CustomObjects.Remove(oldGuid);
                    }
                }
            }
        }

        /// <summary>Empty constructor.</summary>
        public CustomObject()
        {
            this.guid = Guid.NewGuid();
            this.InitNetFields();
        }

        /// <summary>Construct an instance.</summary>
        public CustomObject(CustomObjectData PyTKData, BasicItemInformation info, int Stack = 1)
            : base(PyTKData, Vector2.Zero)
        {
            this.info = info;
            this.guid = Guid.NewGuid();
            this.initializeBasics();

            this.Stack = Stack;

        }

        /// <summary>Construct an instance.</summary>
        public CustomObject(CustomObjectData PyTKData, BasicItemInformation info, Vector2 TileLocation, int Stack = 1)
            : base(PyTKData, TileLocation)
        {
            this.info = info;
            this.guid = Guid.NewGuid();
            this.initializeBasics();

            this.Stack = Stack;
        }

        /// <summary>Sets some basic information up.</summary>
        public virtual void initializeBasics()
        {
            this.name = this.info.name;
            this.displayName = this.getDisplayNameFromStringsFile(this.id);
            this.Edibility = this.info.edibility;
            this.Category = -9; //For crafting.
            this.displayName = this.info.name;
            this.setOutdoors.Value = true;
            this.setIndoors.Value = true;
            this.isLamp.Value = false;
            this.Fragility = 0;

            this.updateDrawPosition(0, 0);

            this.bigCraftable.Value = false;

            //this.initNetFields();
            this.InitNetFields();
            this.updateInfo();
            this.Price = this.info.price;
            //if (this.info.ignoreBoundingBox)
            //    this.boundingBox.Value = new Rectangle(int.MinValue, int.MinValue, 0, 0);
        }



        public override bool isPassable()
        {
            return this.info.ignoreBoundingBox || Revitalize.ModCore.playerInfo.sittingInfo.SittingObject == this;
        }

        public override Rectangle getBoundingBox(Vector2 tileLocation)
        {
            //Revitalize.ModCore.log(System.Environment.StackTrace);
            return this.info.ignoreBoundingBox
                ? new Rectangle(int.MinValue, int.MinValue, 0, 0)
                : base.getBoundingBox(tileLocation);
        }

        public override int sellToStorePrice()
        {
            return this.Price;
        }

        public override int salePrice()
        {
            return this.Price * 2;
        }

        /// <summary>Checks for interaction with the object.</summary>
        public override bool checkForAction(Farmer who, bool justCheckingForActivity = false)
        {


            MouseState mState = Mouse.GetState();
            KeyboardState keyboardState = Game1.GetKeyboardState();

            if (mState.RightButton == ButtonState.Pressed && keyboardState.IsKeyDown(Keys.LeftShift)==false && keyboardState.IsKeyDown(Keys.RightShift)==false)
            {
                //ModCore.log("Right clicked!");
                return this.rightClicked(who);
            }

            if (mState.RightButton == ButtonState.Pressed && (keyboardState.IsKeyDown(Keys.LeftShift)==true || keyboardState.IsKeyDown(Keys.RightShift)==true))
                return this.shiftRightClicked(who);

            return base.checkForAction(who, justCheckingForActivity);

            if (justCheckingForActivity)
                return true;
            ModCore.log("Left clicked!");
            return this.clicked(who);
        }


        /// <summary>What happens when the player right clicks the object.</summary>
        public virtual bool rightClicked(Farmer who)
        {
            // Game1.showRedMessage("YOOO");
            //do some stuff when the right button is down
            // rotate();
            if (this.heldObject.Value != null)
            {
                //  Game1.player.addItemByMenuIfNecessary(this.heldObject);
                // this.heldObject = null;
            }
            else
            {
                //   this.heldObject = Game1.player.ActiveObject;
                //  Game1.player.removeItemFromInventory(heldObject);
            }
            return true;
        }

        /// <summary>What happens when the player shift-right clicks this object.</summary>
        public virtual bool shiftRightClicked(Farmer who)
        {
            //ModCore.log("Shift right clicked!");
            return true;
        }

        /// <summary>What happens when the player left clicks the object.</summary>
        public override bool clicked(Farmer who)
        {
            //ModCore.log("Clicky click!");

            return this.removeAndAddToPlayersInventory();
            //return base.clicked(who);
        }

        /// <summary>What happens when a player uses a tool on this object.</summary>
        public override bool performToolAction(Tool t, GameLocation location)
        {
            if (t == null)
            {
                return true;
            }

            if (t.GetType() == typeof(StardewValley.Tools.Axe) || t.GetType() == typeof(StardewValley.Tools.Pickaxe))
            {
                Game1.createItemDebris(this, Game1.player.getStandingPosition(), Game1.player.getDirection());
                this.location = null;
                this.updateDrawPosition(0, 0);
                Game1.player.currentLocation.removeObject(this.TileLocation, false);
                this.updateDrawPosition(0, 0);
                return false;
            }

            return false;
            //return base.performToolAction(t, location);
        }

        /// <summary>Remove the object from the world and add it to the player's inventory if possible.</summary>
        public virtual bool removeAndAddToPlayersInventory()
        {
            if (Game1.player.isInventoryFull())
            {
                Game1.showRedMessage("Inventory full.");
                return false;
            }
            this.location = null;
            Game1.player.currentLocation.removeObject(this.TileLocation, false);
            Game1.player.addItemToInventory(this);
            this.updateDrawPosition(0, 0);
            return true;
        }

        /// <summary>Gets the category color for the object.</summary>
        public override Color getCategoryColor()
        {
            return this.info.categoryColor;
            //return data.categoryColor;
        }

        /// <summary>Gets the category name for the object.</summary>
        public override string getCategoryName()
        {
            return this.info.categoryName;
        }

        /// <summary>Gets the description for the object.</summary>
        public override string getDescription()
        {
            string text = this.info.description;
            SpriteFont smallFont = Game1.smallFont;
            int width = Game1.tileSize * 4 + Game1.tileSize / 4;
            return Game1.parseText(text, smallFont, width);
        }

        /// <summary>Places an object down.</summary>
        public override bool placementAction(GameLocation location, int x, int y, Farmer who = null)
        {
            if (this.info.canBeSetIndoors == false && location.IsOutdoors == false) return false;
            if (this.info.canBeSetOutdoors == false && location.IsOutdoors) return false;
            this.updateDrawPosition(x, y);
            this.location = location;
            return base.placementAction(location, x, y, who);
        }

        public override bool canBePlacedHere(GameLocation l, Vector2 tile)
        {
            if (this.info.ignoreBoundingBox && l.isObjectAtTile((int)tile.X, (int)tile.Y) == false) return true;
            return base.canBePlacedHere(l, tile);
        }

        public virtual void rotate()
        {
            if (this.info.facingDirection == Enums.Direction.Down) this.info.facingDirection = Enums.Direction.Right;
            else if (this.info.facingDirection == Enums.Direction.Right) this.info.facingDirection = Enums.Direction.Up;
            else if (this.info.facingDirection == Enums.Direction.Up) this.info.facingDirection = Enums.Direction.Left;
            else if (this.info.facingDirection == Enums.Direction.Left) this.info.facingDirection = Enums.Direction.Down;

            if (this.info.animationManager.animations.ContainsKey(this.generateRotationalAnimationKey()))
            {
                this.info.animationManager.setAnimation(this.generateRotationalAnimationKey());
            }
            else
            {
                //Revitalize.ModCore.log("Animation does not exist...." + generateRotationalAnimationKey());
            }
        }


        public string generateRotationalAnimationKey()
        {
            return (this.info.animationManager.currentAnimationName.Split('_')[0]) + "_" + (int)this.info.facingDirection;
        }

        public string generateDefaultRotationalAnimationKey()
        {
            return ("Default" + "_" + (int)this.info.facingDirection);
        }

        /// <summary>Updates a visual draw position.</summary>
        public virtual void updateDrawPosition(int x, int y)
        {
            this.info.drawPosition = new Vector2((int)(x / Game1.tileSize), (int)(y / Game1.tileSize));
            //this.info.drawPosition = new Vector2((float)this.boundingBox.X, (float)(this.boundingBox.Y - (this.animationManager.currentAnimation.sourceRectangle.Height * Game1.pixelZoom - this.boundingBox.Height)));
        }

        /// <summary>Gets a clone of the game object.</summary>
        public override Item getOne()
        {
            return new CustomObject(this.data, this.info);
        }

        /// <summary>What happens when the object is drawn at a tile location.</summary>
        public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
        {
            this.updateInfo();
            if (x <= -1)
            {
                spriteBatch.Draw(this.info.animationManager.getTexture(), Game1.GlobalToLocal(Game1.viewport, this.info.drawPosition), new Rectangle?(this.animationManager.currentAnimation.sourceRectangle), this.info.drawColor * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, (float)(this.TileLocation.Y * Game1.tileSize) / 10000f));
            }
            else
            {
                //The actual planter box being drawn.
                if (this.animationManager == null)
                {
                    if (this.animationManager.getExtendedTexture() == null)
                        ModCore.ModMonitor.Log("Tex Extended is null???");

                    spriteBatch.Draw(this.displayTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), y * Game1.tileSize)), new Rectangle?(this.animationManager.currentAnimation.sourceRectangle), this.info.drawColor * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, (float)(this.TileLocation.Y * Game1.tileSize) / 10000f));
                    // Log.AsyncG("ANIMATION IS NULL?!?!?!?!");
                }

                else
                {
                    //Log.AsyncC("Animation Manager is working!");
                    int addedDepth = 0;
                    if (this.info.ignoreBoundingBox) addedDepth++;
                    if (Revitalize.ModCore.playerInfo.sittingInfo.SittingObject == this) addedDepth++;
                    this.animationManager.draw(spriteBatch, this.displayTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), y * Game1.tileSize)), new Rectangle?(this.animationManager.currentAnimation.sourceRectangle), this.info.drawColor * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, (float)((this.TileLocation.Y + addedDepth) * Game1.tileSize) / 10000f));
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

                // spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)((double)tileLocation.X * (double)Game1.tileSize + (((double)tileLocation.X * 11.0 + (double)tileLocation.Y * 7.0) % 10.0 - 5.0)) + (float)(Game1.tileSize / 2), (float)((double)tileLocation.Y * (double)Game1.tileSize + (((double)tileLocation.Y * 11.0 + (double)tileLocation.X * 7.0) % 10.0 - 5.0)) + (float)(Game1.tileSize / 2))), new Rectangle?(new Rectangle((int)((double)tileLocation.X * 51.0 + (double)tileLocation.Y * 77.0) % 3 * 16, 128 + this.whichForageCrop * 16, 16, 16)), Color.White, 0.0f, new Vector2(8f, 8f), (float)Game1.pixelZoom, SpriteEffects.None, (float)(((double)tileLocation.Y * (double)Game1.tileSize + (double)(Game1.tileSize / 2) + (((double)tileLocation.Y * 11.0 + (double)tileLocation.X * 7.0) % 10.0 - 5.0)) / 10000.0));
            }
        }

        /// <summary>Draw the game object at a non-tile spot. Aka like debris.</summary>
        public override void draw(SpriteBatch spriteBatch, int xNonTile, int yNonTile, float layerDepth, float alpha = 1f)
        {
            this.updateInfo();
            /*
            if (Game1.eventUp && Game1.CurrentEvent.isTileWalkedOn(xNonTile / 64, yNonTile / 64))
                return;
            if ((int)(this.ParentSheetIndex) != 590 && (int)(this.Fragility) != 2)
                spriteBatch.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(xNonTile + 32), (float)(yNonTile + 51 + 4))), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White * alpha, 0.0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 4f, SpriteEffects.None, layerDepth - 1E-06f);
            SpriteBatch spriteBatch1 = spriteBatch;
            Texture2D objectSpriteSheet = Game1.objectSpriteSheet;
            Vector2 local = Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(xNonTile + 32 + (this.shakeTimer > 0 ? Game1.random.Next(-1, 2) : 0)), (float)(yNonTile + 32 + (this.shakeTimer > 0 ? Game1.random.Next(-1, 2) : 0))));
            Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?(GameLocation.getSourceRectForObject(this.ParentSheetIndex));
            Color color = Color.White * alpha;
            double num1 = 0.0;
            Vector2 origin = new Vector2(8f, 8f);
            Vector2 scale = this.scale;
            double num2 = (double)this.scale.Y > 1.0 ? (double)this.getScale().Y : 4.0;
            int num3 = (bool)(this.flipped) ? 1 : 0;
            double num4 = (double)layerDepth;

            spriteBatch1.Draw(this.displayTexture, local, this.animationManager.defaultDrawFrame.sourceRectangle, this.info.drawColor * alpha, (float)num1, origin, (float)4f, (SpriteEffects)num3, (float)num4);
            */
            //The actual planter box being drawn.
            if (this.animationManager == null)
            {
                this.syncObject.MarkDirty();
                if (this.animationManager.getExtendedTexture() == null)
                    ModCore.ModMonitor.Log("Tex Extended is null???");

                spriteBatch.Draw(this.displayTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(xNonTile), yNonTile)), new Rectangle?(this.animationManager.currentAnimation.sourceRectangle), this.info.drawColor * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, layerDepth));
                // Log.AsyncG("ANIMATION IS NULL?!?!?!?!");
            }

            else
            {
                //Log.AsyncC("Animation Manager is working!");
                int addedDepth = 0;
                if (this.info.ignoreBoundingBox) addedDepth++;
                if (Revitalize.ModCore.playerInfo.sittingInfo.SittingObject == this) addedDepth++;
                this.animationManager.draw(spriteBatch, this.displayTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(xNonTile), yNonTile)), new Rectangle?(this.animationManager.currentAnimation.sourceRectangle), this.info.drawColor * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, layerDepth));
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

            // spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)((double)tileLocation.X * (double)Game1.tileSize + (((double)tileLocation.X * 11.0 + (double)tileLocation.Y * 7.0) % 10.0 - 5.0)) + (float)(Game1.tileSize / 2), (float)((double)tileLocation.Y * (double)Game1.tileSize + (((double)tileLocation.Y * 11.0 + (double)tileLocation.X * 7.0) % 10.0 - 5.0)) + (float)(Game1.tileSize / 2))), new Rectangle?(new Rectangle((int)((double)tileLocation.X * 51.0 + (double)tileLocation.Y * 77.0) % 3 * 16, 128 + this.whichForageCrop * 16, 16, 16)), Color.White, 0.0f, new Vector2(8f, 8f), (float)Game1.pixelZoom, SpriteEffects.None, (float)(((double)tileLocation.Y * (double)Game1.tileSize + (double)(Game1.tileSize / 2) + (((double)tileLocation.Y * 11.0 + (double)tileLocation.X * 7.0) % 10.0 - 5.0)) / 10000.0));

        }

        /// <summary>What happens when the object is drawn in a menu.</summary>
        public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber, Color c, bool drawShadow)
        {
            this.updateInfo();
            if (drawStackNumber && this.maximumStackSize() > 1 && ((double)scaleSize > 0.3 && this.Stack != int.MaxValue) && this.Stack > 1)
                Utility.drawTinyDigits(this.Stack, spriteBatch, location + new Vector2((float)(Game1.tileSize - Utility.getWidthOfTinyDigitString(this.Stack, 3f * scaleSize)) + 3f * scaleSize, (float)((double)Game1.tileSize - 18.0 * (double)scaleSize + 2.0)), 3f * scaleSize, 1f, Color.White);
            if (drawStackNumber && this.Quality > 0)
            {
                float num = this.Quality < 4 ? 0.0f : (float)((Math.Cos((double)Game1.currentGameTime.TotalGameTime.Milliseconds * Math.PI / 512.0) + 1.0) * 0.0500000007450581);
                spriteBatch.Draw(Game1.mouseCursors, location + new Vector2(12f, (float)(Game1.tileSize - 12) + num), new Microsoft.Xna.Framework.Rectangle?(this.Quality < 4 ? new Microsoft.Xna.Framework.Rectangle(338 + (this.Quality - 1) * 8, 400, 8, 8) : new Microsoft.Xna.Framework.Rectangle(346, 392, 8, 8)), Color.White * transparency, 0.0f, new Vector2(4f, 4f), (float)(3.0 * (double)scaleSize * (1.0 + (double)num)), SpriteEffects.None, layerDepth);
            }
            spriteBatch.Draw(this.displayTexture, location + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize)), new Rectangle?(this.animationManager.currentAnimation.sourceRectangle), this.info.drawColor * transparency, 0f, new Vector2((float)(this.animationManager.currentAnimation.sourceRectangle.Width / 2), (float)(this.animationManager.currentAnimation.sourceRectangle.Height)), scaleSize * 4f, SpriteEffects.None, layerDepth);
        }

        /// <summary>What happens when the object is drawn when held by a player.</summary>
        public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, StardewValley.Farmer f)
        {
            this.updateInfo();
            if (this.animationManager == null)
            {
                Revitalize.ModCore.log("Animation Manager Null");
            }
            if (this.displayTexture == null) Revitalize.ModCore.log("Display texture is null");
            if (f.ActiveObject.bigCraftable.Value)
            {
                spriteBatch.Draw(this.displayTexture, objectPosition, this.animationManager.currentAnimation.sourceRectangle, this.info.drawColor, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + 2) / 10000f));
                return;
            }

            spriteBatch.Draw(this.displayTexture, objectPosition, this.animationManager.currentAnimation.sourceRectangle, this.info.drawColor, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + 2) / 10000f));
            if (f.ActiveObject != null && f.ActiveObject.Name.Contains("="))
            {
                spriteBatch.Draw(this.displayTexture, objectPosition + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), this.animationManager.currentAnimation.sourceRectangle, Color.White, 0f, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), (float)Game1.pixelZoom + Math.Abs(Game1.starCropShimmerPause) / 8f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + 2) / 10000f));
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

        public override void drawPlacementBounds(SpriteBatch spriteBatch, GameLocation location)
        {
            //Do nothing because this shouldn't be placeable anywhere.
        }

        public void InitNetFields()
        {
        }



        public virtual void replaceAfterLoad()
        {
            if (string.IsNullOrEmpty(this.info.locationName) == false)
            {
                //ModCore.log("Replace an object!");
                this.location.removeObject(this.TileLocation, false);
                this.placementAction(this.location, (int)this.TileLocation.X * Game1.tileSize, (int)this.TileLocation.Y * Game1.tileSize);

                //ModCore.log("Do I ingnore BB? " + this.info.ignoreBoundingBox);
                //ModCore.log("This is my BB: " + this.boundingBox.Value);
            }
        }


        public string getDisplayNameFromStringsFile(string objectID)
        {
            //Load in a file that has all object names referenced here or something.
            return this.info.name;
        }

        public virtual void updateInfo()
        {
            if (this.info == null)
            {
                this.ItemInfo = this.text;
                ModCore.log("Updated item info!");
                return;
            }

            if (this.requiresUpdate())
            {
                this.text = this.ItemInfo;
                this.info.cleanAfterUpdate();
                MultiplayerUtilities.RequestUpdateSync(this.guid);
            }
        }

        public virtual void getUpdate()
        {
            this.ItemInfo = this.text;
        }

        public virtual bool requiresUpdate()
        {
            if (this.info.requiresSyncUpdate())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~//
        //     PyTk Functions      //
        //~~~~~~~~~~~~~~~~~~~~~~~~~//
        #region

        /// <summary>
        /// Rebuilds the data from saves.
        /// </summary>
        /// <param name="additionalSaveData"></param>
        /// <param name="replacement"></param>
        public override void rebuild(Dictionary<string, string> additionalSaveData, object replacement)
        {
            //CustomObjectData data = CustomObjectData.collection[additionalSaveData["id"]];
            //BasicItemInformation info = Revitalize.ModCore.Serializer.DeserializeFromJSONString<BasicItemInformation>(additionalSaveData["ItemInfo"]);

        }

        /// <summary>
        /// Prepares the data for saves.
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, string> getAdditionalSaveData()
        {
            Dictionary<string, string> serializedInfo = new Dictionary<string, string>();
            serializedInfo.Add("id", this.ItemInfo);
            serializedInfo.Add("ItemInfo", Revitalize.ModCore.Serializer.ToJSONString(this.info));
            Revitalize.ModCore.Serializer.SerializeGUID(this.guid.ToString(), this);
            return serializedInfo;
        }
        #endregion

        public override void updateWhenCurrentLocation(GameTime time, GameLocation environment)
        {
            if (this.location == null)
            {
                this.location = environment;
            }
            base.updateWhenCurrentLocation(time, environment);
        }

        /// <summary>
        /// Gets a list of neighboring tiled objects that produce or transfer energy. This should be used for machines/objects that consume or transfer energy
        /// </summary>
        /// <returns></returns>
        public List<CustomObject> GetNeighboringEnergyTransferProducers()
        {
            Vector2 tileLocation = this.TileLocation;
            List<CustomObject> customObjects = new List<CustomObject>();
            if (this.location != null)
            {
                for(int i = -1; i <= 1; i++)
                {
                    for(int j = -1; j <= 1; j++)
                    {
                        if (i == j || i== (-j)) continue;

                        Vector2 neighborTile = tileLocation + new Vector2(i, j);
                        if (this.location.isObjectAtTile((int)neighborTile.X, (int)neighborTile.Y))
                        {
                            StardewValley.Object obj=this.location.getObjectAtTile((int)neighborTile.X, (int)neighborTile.Y);
                            if (obj is CustomObject)
                            {
                                if((obj as CustomObject).EnergyManager.energyInteractionType== Enums.EnergyInteractionType.Produces || (obj as CustomObject).EnergyManager.energyInteractionType == Enums.EnergyInteractionType.Transfers)
                                {
                                    customObjects.Add((CustomObject)obj);
                                }
                            }
                            else continue;
                        }
                    }
                }
            }


            return customObjects;

        }

        /// <summary>
        /// Gets a list of neighboring tiled objects that consume or transfer energy. This should be used for machines/objects that produce or transfer energy
        /// </summary>
        /// <returns></returns>
        public List<CustomObject> GetNeighboringEnergyTransferConsumers()
        {
            Vector2 tileLocation = this.TileLocation;
            List<CustomObject> customObjects = new List<CustomObject>();
            if (this.location != null)
            {
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == j || i == (-j)) continue;

                        Vector2 neighborTile = tileLocation + new Vector2(i, j);
                        if (this.location.isObjectAtTile((int)neighborTile.X, (int)neighborTile.Y))
                        {
                            StardewValley.Object obj = this.location.getObjectAtTile((int)neighborTile.X, (int)neighborTile.Y);
                            if (obj is CustomObject)
                            {
                                if ((obj as CustomObject).EnergyManager.energyInteractionType == Enums.EnergyInteractionType.Consumes || (obj as CustomObject).EnergyManager.energyInteractionType == Enums.EnergyInteractionType.Transfers)
                                {
                                    customObjects.Add((CustomObject)obj);
                                }
                            }
                            else continue;
                        }
                    }
                }
            }


            return customObjects;
        }

        /// <summary>
        /// Gets the appropriate energy neighbors to move energy around from/to.
        /// </summary>
        /// <returns></returns>
        public List<CustomObject> getAppropriateEnergyNeighbors()
        {
            if (this.EnergyManager.consumesEnergy)
            {
                return this.GetNeighboringEnergyTransferProducers();
            }
            else if(this.EnergyManager.producesEnergy)
            {
                return this.GetNeighboringEnergyTransferConsumers();
            }
            else if (this.EnergyManager.transfersEnergy)
            {
                List<CustomObject> objs = new List<CustomObject>();
                objs.AddRange(this.GetNeighboringEnergyTransferConsumers());
                objs.AddRange(this.GetNeighboringEnergyTransferProducers());
                return objs;
            }
            return new List<CustomObject>();
        }
    }
}
