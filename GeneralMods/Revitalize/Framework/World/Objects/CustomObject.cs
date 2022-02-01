using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Revitalize.Framework.Illuminate;
using Revitalize.Framework.World.Objects.InformationFiles;
using Revitalize.Framework.World.Objects.Interfaces;
using Revitalize.Framework;
using Revitalize.Framework.Objects;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Objects;
using StardewValley.Tools;
using StardustCore.Animations;
using Revitalize.Framework.Utilities;
using Netcode;
using Revitalize.Framework.Utilities.Extensions;
using Revitalize.Framework.World.WorldUtilities;

namespace Revitalize.Framework.World.Objects
{

    /*
     * 
     * NOTES: Calling this.performRemoveAction(this.tileLocation, location); is NOT the same as calling GameLocation.objects.Remove(). Perform remove action cleans up the item before being removed, but has no actual deletion/removal logic from the game world itself.
     * 
     * 
     * Current issue: When right clicking the object is picked up. This is currently not intended.
     * 
     */


    /// <summary>
    /// A base class that is to be extended by other implementations of objects.
    ///
    /// Clicking to remove and click place are bound to the samething. Need to find a way to change that.
    /// Bounding boxes work, but not for clicking to remove. Why is that?
    /// </summary>
    [XmlType("Mods_Revitalize.Framework.World.Objects.CustomObject")]
    public class CustomObject : StardewValley.Objects.Furniture, ICommonObjectInterface, ILightManagerProvider, IBasicItemInfoProvider
    {
        public bool isCurrentLocationAStructure;

        public readonly NetRef<BasicItemInformation> netBasicItemInformation = new NetRef<BasicItemInformation>();

        [XmlElement("basicItemInfo")]
        public BasicItemInformation basicItemInformation
        {
            get
            {
                return this.netBasicItemInformation.Value;
            }
            set
            {
                this.netBasicItemInformation.Value = value;
            }
        }

        public override string Name
        {
            get
            {
                if (this.basicItemInformation == null) return null;
                return this.basicItemInformation.name.Value;
            }
            set
            {
                if (this.basicItemInformation != null)
                {
                    this.basicItemInformation.name.Value = value;
                }
            }


        }
        public override string DisplayName
        {
            get
            {
                if (this.basicItemInformation == null) return null;
                return this.basicItemInformation.name.Value;
            }
            set
            {
                if (this.basicItemInformation != null)
                {
                    this.basicItemInformation.name.Value = value;
                }
            }
        }


        [XmlIgnore]
        public AnimationManager AnimationManager
        {
            get
            {
                if (this.basicItemInformation == null) return null;
                if (this.basicItemInformation.animationManager == null) return null;
                return this.basicItemInformation.animationManager;
            }
        }

        [XmlIgnore]
        public Texture2D CurrentTextureToDisplay
        {

            get
            {
                if (this.AnimationManager == null) return null;
                return this.AnimationManager.getTexture();
            }
        }

        public CustomObject()
        {
            this.basicItemInformation = new BasicItemInformation();

            this.furniture_type.Value = Furniture.other;
            this.bigCraftable.Value = true;
            this.Type = "interactive";
            this.name = this.Name;


            IReflectedField<int> placementRestriction = RevitalizeModCore.ModHelper.Reflection.GetField<int>(this, "_placementRestriction");
            placementRestriction.SetValue(2);
            this.Fragility = 0;

            this.initNetFieldsPostConstructor();
        }

        public CustomObject(BasicItemInformation BasicItemInfo)
        {
            this.basicItemInformation = BasicItemInfo;
            this.bigCraftable.Value = true;
            this.furniture_type.Value = Furniture.other;
            this.Type = "interactive";
            this.name = this.Name;

            IReflectedField<int> placementRestriction = RevitalizeModCore.ModHelper.Reflection.GetField<int>(this, "_placementRestriction");
            placementRestriction.SetValue(2);
            this.Fragility = 0;

            this.initNetFieldsPostConstructor();
        }

        public CustomObject(BasicItemInformation BasicItemInfo, int StackSize = 1)
        {
            this.basicItemInformation = BasicItemInfo;
            this.TileLocation = Vector2.Zero;
            this.Stack = StackSize;
            this.bigCraftable.Value = true;
            this.furniture_type.Value = Furniture.other;
            this.Type = "interactive";
            this.name = this.Name;
            this.Fragility = 0;

            IReflectedField<int> placementRestriction = RevitalizeModCore.ModHelper.Reflection.GetField<int>(this, "_placementRestriction");
            placementRestriction.SetValue(2);

            this.initNetFieldsPostConstructor();
        }

        public CustomObject(BasicItemInformation BasicItemInfo, Vector2 TileLocation)
        {
            this.basicItemInformation = BasicItemInfo;
            this.TileLocation = TileLocation;
            this.bigCraftable.Value = true;
            this.furniture_type.Value = Furniture.other;
            this.Type = "interactive";
            this.name = this.Name;
            this.Fragility = 0;

            IReflectedField<int> placementRestriction = RevitalizeModCore.ModHelper.Reflection.GetField<int>(this, "_placementRestriction");
            placementRestriction.SetValue(2);

            this.initNetFieldsPostConstructor();
        }
        public CustomObject(BasicItemInformation BasicItemInfo, Vector2 TileLocation, int StackSize = 1)
        {
            this.basicItemInformation = BasicItemInfo;
            this.TileLocation = TileLocation;
            this.Stack = StackSize;
            this.bigCraftable.Value = true;
            this.furniture_type.Value = Furniture.other;
            this.Type = "interactive";
            this.name = this.Name;
            this.Fragility = 0;

            IReflectedField<int> placementRestriction = RevitalizeModCore.ModHelper.Reflection.GetField<int>(this, "_placementRestriction");
            placementRestriction.SetValue(2);

            this.initNetFieldsPostConstructor();
        }

        protected virtual void initNetFieldsPostConstructor()
        {
            if (this.basicItemInformation != null)
            {
                this.NetFields.AddFields(this.netBasicItemInformation);
            }

        }

        /// <summary>
        /// What happens when the item is placed into the game world.
        /// </summary>
        /// <param name="who"></param>
        /// <returns></returns>
        public override bool performDropDownAction(Farmer who)
        {
            return false;
        }

        public override void actionOnPlayerEntry()
        {
            base.actionOnPlayerEntry();
        }

        public override void actionWhenBeingHeld(Farmer who)
        {
            base.actionWhenBeingHeld(who);
        }

        public override bool actionWhenPurchased()
        {
            return base.actionWhenPurchased();
        }

        public override void actionWhenStopBeingHeld(Farmer who)
        {
            base.actionWhenStopBeingHeld(who);
        }

        public override void ApplySprinkler(GameLocation location, Vector2 tile)
        {
            base.ApplySprinkler(location, tile);
        }

        public override bool canBeDropped()
        {
            return true;
        }

        public override bool canBeGivenAsGift()
        {
            return base.canBeGivenAsGift();
        }

        public override bool canBePlacedHere(GameLocation l, Vector2 tile)
        {

            return base.canBePlacedHere(l, tile);
        }

        public override bool canBePlacedInWater()
        {
            return base.canBePlacedInWater();
        }

        public override bool canBeShipped()
        {
            return base.canBeShipped();
        }

        public override int attachmentSlots()
        {
            return base.attachmentSlots();
        }

        public override bool canBeTrashed()
        {
            return base.canBeTrashed();
        }

        public override bool CanBuyItem(Farmer who)
        {
            return base.CanBuyItem(who);
        }

        /// <summary>
        /// Checks to see if the object is being interacted with. Seems to only happen when right clicked.
        /// </summary>
        /// <param name="who"></param>
        /// <param name="justCheckingForActivity"></param>
        /// <returns>True if something meaningful has occured. False otherwise.</returns>
        public override bool checkForAction(Farmer who, bool justCheckingForActivity = false)
        {
            if (justCheckingForActivity)
            {
                //basically on item hover.
                return true;
            }

            MouseState mState = Mouse.GetState();
            KeyboardState keyboardState = Game1.GetKeyboardState();

            if (mState.RightButton == ButtonState.Pressed && keyboardState.IsKeyDown(Keys.LeftShift) == false && keyboardState.IsKeyDown(Keys.RightShift) == false)
            {
                return this.rightClicked(who);
            }

            if (mState.RightButton == ButtonState.Pressed && (keyboardState.IsKeyDown(Keys.LeftShift) == true || keyboardState.IsKeyDown(Keys.RightShift) == true))
                return this.shiftRightClicked(who);

            if (mState.LeftButton == ButtonState.Pressed)
            {
                return true;
            }
            else
            {
                return true;
            }

            //True should be retruned when something meaningful has happened.
            //False should be returned when things like error messages have occurd.
        }

        public override string checkForSpecialItemHoldUpMeessage()
        {
            return base.checkForSpecialItemHoldUpMeessage();
        }

        public override bool clicked(Farmer who)
        {
            if (Game1.player.isInventoryFull())
            {
                //Full inventory message.
                Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Crop.cs.588"));
                return false;
            }


            if (Game1.didPlayerJustLeftClick())
            {
                this.pickupFromGameWorld(this.TileLocation, who.currentLocation);
            }
            return true;

        }

        public override void DayUpdate(GameLocation location)
        {
            base.DayUpdate(location);
        }

        public override void farmerAdjacentAction(GameLocation location)
        {
            base.farmerAdjacentAction(location);
        }

        public override int GetBaseRadiusForSprinkler()
        {
            return base.GetBaseRadiusForSprinkler();
        }

        /// <summary>
        /// Category color.
        /// </summary>
        /// <returns></returns>
        public override Color getCategoryColor()
        {
            return this.basicItemInformation.categoryColor;
        }

        /// <summary>
        /// Category name
        /// </summary>
        /// <returns></returns>
        public override string getCategoryName()
        {
            return this.basicItemInformation.categoryName;
        }

        /// <summary>
        /// Hover box text
        /// </summary>
        /// <param name="hoveredItem"></param>
        /// <returns></returns>
        public override string getHoverBoxText(Item hoveredItem)
        {
            return base.getHoverBoxText(hoveredItem);
        }

        /// <summary>
        /// Description
        /// </summary>
        /// <returns></returns>
        public override string getDescription()
        {
            return this.basicItemInformation.description;
        }

        public override StardewValley.Object GetDeconstructorOutput(Item item)
        {
            return base.GetDeconstructorOutput(item);
        }

        public override Item getOne()
        {
            return new CustomObject(this.basicItemInformation.Copy());
        }

        public override void _GetOneFrom(Item source)
        {
            base._GetOneFrom(source);
        }

        public override int healthRecoveredOnConsumption()
        {
            return this.basicItemInformation.healthRestoredOnEating;
        }

        public override void hoverAction()
        {
            base.hoverAction();
        }

        public override void initializeLightSource(Vector2 tileLocation, bool mineShaft = false)
        {
            base.initializeLightSource(tileLocation, mineShaft);
        }

        public override bool isActionable(Farmer who)
        {
            return true;
        }

        public override bool isAnimalProduct()
        {
            return base.isAnimalProduct();
        }

        public override bool isForage(GameLocation location)
        {
            return base.isForage(location);
        }

        public override bool isPassable()
        {
            if (this.basicItemInformation.ignoreBoundingBox) return true;
            return false;
        }

        public override Rectangle getBoundingBox(Vector2 tileLocation)
        {

            Microsoft.Xna.Framework.Rectangle boundingBox = this.boundingBox.Value;
            Microsoft.Xna.Framework.Rectangle newBounds = boundingBox;
            newBounds.X = (int)tileLocation.X * Game1.tileSize;
            newBounds.Y = (int)tileLocation.Y * Game1.tileSize;

            newBounds.Width = Game1.tileSize * (int)this.basicItemInformation.boundingBoxTileDimensions.X;
            newBounds.Height = Game1.tileSize * (int)this.basicItemInformation.boundingBoxTileDimensions.Y;

            return newBounds;
        }

        public override List<Vector2> GetSprinklerTiles()
        {
            return base.GetSprinklerTiles();
        }

        public override bool isPlaceable()
        {
            return true;
        }


        public override bool IsSprinkler()
        {
            return base.IsSprinkler();
        }

        public override int maximumStackSize()
        {
            return 999;
        }

        /// <summary>
        /// When so many minutes pass,update this object.
        /// </summary>
        /// <param name="minutes"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public override bool minutesElapsed(int minutes, GameLocation environment)
        {
            return base.minutesElapsed(minutes, environment);
        }

        public override bool onExplosion(Farmer who, GameLocation location)
        {
            return base.onExplosion(who, location);
        }

        public override void onReadyForHarvest(GameLocation environment)
        {
            base.onReadyForHarvest(environment);
        }

        /*
        /// <summary>
        /// When the object is droped into (???) what happens?
        /// </summary>
        /// <param name="dropInItem"></param>
        /// <param name="probe"></param>
        /// <param name="who"></param>
        /// <returns></returns>
        public override bool performObjectDropInAction(Item dropInItem, bool probe, Farmer who)
        {
            return false;
            
        }
        */

        /// <summary>
        /// Performs cleanup that should happen related to an object's removal, but DOES NOT perform the actual removing from the game world.
        /// </summary>
        /// <param name="tileLocation"></param>
        /// <param name="environment"></param>
        public override void performRemoveAction(Vector2 tileLocation, GameLocation environment)
        {

            if (environment == null) return;

            RevitalizeModCore.log("Perform remove action for furniture!");

            this.cleanUpLights();



            this.removeLights(environment);
            if ((int)this.furniture_type == 14 || (int)this.furniture_type == 16)
            {
                base.isOn.Value = false;
                this.setFireplace(environment, playSound: false);
            }
            this.RemoveLightGlow(environment);


            this.TileLocation = Vector2.Zero;
            this.basicItemInformation.locationName.Value = "";
            this.boundingBox.Value = this.getBoundingBox(Vector2.Zero);

            this.sittingFarmers.Clear();

            //Add this back in??
            //this.removeAndAddToPlayersInventory();

            //base.performRemoveAction(tileLocation, environment) ;
            //base.performRemoveAction(tileLocation, environment);
        }


        /// <summary>
        /// Removes this from the game world, performs cleanup, and puts the object into the player's inventory. For similar logic, <see cref="CustomObject.AttemptRemoval(Action{Furniture})"/> for furniture removal logic as this is specific to the <see cref="StardewValley.Object"/> removal logic.
        /// </summary>
        /// <param name="tileLocation"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public virtual bool pickupFromGameWorld(Vector2 tileLocation, GameLocation environment)
        {

            if (Game1.player.isInventoryFull())
            {
                Game1.showRedMessage("Inventory full.");
                return false;
            }

            this.removeFromGameWorld(tileLocation, environment);
            this.performRemoveAction(tileLocation, environment);

            Farmer who = Game1.player;
            bool foundInToolbar = false;
            for (int i = 0; i < 12; i++)
            {
                if (Game1.player.items[i] == null)
                {
                    who.items[i] = this;
                    who.CurrentToolIndex = i;
                    foundInToolbar = true;
                    break;
                }
            }
            if (!foundInToolbar)
            {
                Item item = who.addItemToInventory(this, 11);
                who.addItemToInventory(item);
                who.CurrentToolIndex = 11;
            }


            return true;
        }

        /// <summary>
        /// Removes a game object from a <see cref="GameLocation"/>'s furniture and object lists.
        /// </summary>
        public virtual void removeFromGameWorld(Vector2 TileLocation, GameLocation environment)
        {

            if (environment != null) ;
            environment.objects.Remove(TileLocation);
            environment.furniture.Remove(this);
        }

        /// <summary>
        /// When a tool is used on this item.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public override bool performToolAction(Tool t, GameLocation location)
        {
            if (t == null)
            {
                RevitalizeModCore.log("Null tool used! Probably just the player's hands then.");
                this.shakeTimer = 200; //Milliseconds.
                return false;

            }
            else
            {
                RevitalizeModCore.log("Player used tool: " + t.DisplayName);
            }
            return false;
            //False is returned if we signify no meaningul tool interactions?
            //True is returned when something significant happens

        }

        /// <summary>
        /// When this item is used. (Left clicked)
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public override bool performUseAction(GameLocation location)
        {
            RevitalizeModCore.log("Perform use action");
            return base.performUseAction(location);
        }

        /// <summary>
        /// Places this object at a given TILE location for the game.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="TileX"></param>
        /// <param name="TileY"></param>
        /// <param name="who"></param>
        /// <returns></returns>
        public virtual bool placementActionAtTile(GameLocation location, int TileX, int TileY, Farmer who = null)
        {
          return this.placementAction(location, TileX * Game1.tileSize, TileY * Game1.tileSize, who);
        }

        public override bool placementAction(GameLocation location, int x, int y, Farmer who = null)
        {

            if (!this.isGroundFurniture())
            {
                y = this.GetModifiedWallTilePosition(location, x / 64, y / 64) * 64;
            }
            if (this.GetAdditionalFurniturePlacementStatus(location, x, y, who) != 0)
            {
                return false;
            }

            CustomObject obj = (CustomObject)this.getOne();

            obj.boundingBox.Value = this.getBoundingBox(new Vector2((float)x / (float)64, (float)y / (float)64));

            Vector2 placementTile = new Vector2(x / 64, y / 64);
            obj.health = 10;
            if (who != null)
            {
                obj.owner.Value = who.UniqueMultiplayerID;
            }
            else
            {
                obj.owner.Value = Game1.player.UniqueMultiplayerID;
            }
            obj.TileLocation = placementTile;


            location.furniture.Add(obj);
            location.objects.Add(placementTile, obj);
            if (who != null)
            {
                SoundUtilities.PlaySound(location, Enums.StardewSound.woodyStep);
            }

            string locationName = location.NameOrUniqueName;
            if (string.IsNullOrEmpty(location.NameOrUniqueName))
            {
                locationName = location.Name;
            }

            obj.basicItemInformation.locationName.Value = locationName;
            obj.updateDrawPosition();

            return true;
            //Base code throws and error so I have to do it this way.
        }

        public override int salePrice()
        {
            return this.basicItemInformation.price;
        }

        public override int sellToStorePrice(long specificPlayerID = -1)
        {
            return this.basicItemInformation.price;
            return base.sellToStorePrice(specificPlayerID); //logic for when it's regarding the player's professions and such.
        }

        public override void reloadSprite()
        {
            base.reloadSprite();
        }

        public override int staminaRecoveredOnConsumption()
        {
            return this.basicItemInformation.staminaRestoredOnEating;
        }

        public override void updateWhenCurrentLocation(GameTime time, GameLocation environment)
        {
            if (this.shakeTimer > 0)
            {
                this.basicItemInformation.shakeTimer.Value -= time.ElapsedGameTime.Milliseconds;
                this.shakeTimer -= time.ElapsedGameTime.Milliseconds;
                if (this.shakeTimer <= 0)
                {
                    this.health = 10;
                }
            }

        }



        /// <summary>What happens when the player right clicks the object.</summary>
        public virtual bool rightClicked(Farmer who)
        {
            return false;
        }

        /// <summary>What happens when the player shift-right clicks this object.</summary>
        public virtual bool shiftRightClicked(Farmer who)
        {
            return false;
        }


        public virtual void setGameLocation(string LocationName, bool IsStructure)
        {
            this.basicItemInformation.locationName.Value = LocationName;
            this.isCurrentLocationAStructure = IsStructure;
        }

        public virtual GameLocation getCurrentLocation()
        {
            if (string.IsNullOrEmpty(this.basicItemInformation.locationName.Value))
            {
                return null;
            }
            else
            {
                return Game1.getLocationFromName(this.basicItemInformation.locationName.Value, this.isCurrentLocationAStructure);
            }
        }


        public virtual void cleanUpLights()
        {
            if (this.GetLightManager() != null) this.GetLightManager().removeForCleanUp(this.getCurrentLocation());
        }

        public virtual BasicItemInformation getItemInformation()
        {
            return this.basicItemInformation;
        }

        public virtual void setItemInformation(BasicItemInformation Info)
        {
            this.basicItemInformation = Info;
        }


        public override bool canBeRemoved(Farmer who)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DyeColor"></param>
        public virtual void dyeColor(NamedColor DyeColor)
        {
            this.basicItemInformation.dyedColor.setFields(DyeColor);
        }

        public virtual void eraseDye()
        {
            this.basicItemInformation.dyedColor.clearFields();
        }

        public override bool canStackWith(ISalable other)
        {
            if (other is CustomObject == false) return false;
            CustomObject o = (CustomObject)other;

            if (this.basicItemInformation.dyedColor != o.basicItemInformation.dyedColor) return false;
            if (this.basicItemInformation.id.Equals(o.basicItemInformation.id) == false) return false;

            if (this.maximumStackSize() > this.Stack + other.Stack) return true;

            return false;
        }



        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        //                            Rendering code                   //
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

        public override void drawAsProp(SpriteBatch b)
        {
            base.drawAsProp(b);
        }

        public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
        {
            if (this.AnimationManager == null)
            {

                RevitalizeModCore.log("Animation Manager null for item: " + this.basicItemInformation.id);
                return;
            }
            if (this.CurrentTextureToDisplay == null)
            {
                RevitalizeModCore.log("Texture null for item: " + this.basicItemInformation.id);
                RevitalizeModCore.log("Expected path should be: " + this.basicItemInformation.animationManager.objectTexture.path);


                return;
            }

            if (this.basicItemInformation == null) return;

            int scaleNerfing = Math.Max(this.AnimationManager.getCurrentAnimationFrameRectangle().Width, this.AnimationManager.getCurrentAnimationFrameRectangle().Height) / 16;

            spriteBatch.Draw(this.CurrentTextureToDisplay, location + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize)), new Rectangle?(this.AnimationManager.getCurrentAnimationFrameRectangle()), this.basicItemInformation.DrawColor * transparency, 0f, new Vector2((float)(this.AnimationManager.getCurrentAnimationFrameRectangle().Width / 2), (float)(this.AnimationManager.getCurrentAnimationFrameRectangle().Height)), (scaleSize * 4f) / scaleNerfing, SpriteEffects.None, layerDepth);

            if (drawStackNumber.ShouldDrawFor(this) && this.maximumStackSize() > 1 && ((double)scaleSize > 0.3 && this.Stack != int.MaxValue) && this.Stack > 1)
                Utility.drawTinyDigits(this.Stack, spriteBatch, location + new Vector2((float)(Game1.tileSize - Utility.getWidthOfTinyDigitString(this.Stack, 3f * scaleSize)) + 3f * scaleSize, (float)((double)Game1.tileSize - 18.0 * (double)scaleSize + 2.0)), 3f * scaleSize, 1f, Color.White);
            if (drawStackNumber.ShouldDrawFor(this) && this.Quality > 0)
            {
                float num = this.Quality < 4 ? 0.0f : (float)((Math.Cos((double)Game1.currentGameTime.TotalGameTime.Milliseconds * Math.PI / 512.0) + 1.0) * 0.0500000007450581);
                spriteBatch.Draw(Game1.mouseCursors, location + new Vector2(12f, (float)(Game1.tileSize - 12) + num), new Microsoft.Xna.Framework.Rectangle?(this.Quality < 4 ? new Microsoft.Xna.Framework.Rectangle(338 + (this.Quality - 1) * 8, 400, 8, 8) : new Microsoft.Xna.Framework.Rectangle(346, 392, 8, 8)), Color.White * transparency, 0.0f, new Vector2(4f, 4f), (float)(3.0 * (double)scaleSize * (1.0 + (double)num)), SpriteEffects.None, layerDepth);
            }
        }

        public override void drawAttachments(SpriteBatch b, int x, int y)
        {
            base.drawAttachments(b, x, y);
        }

        public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
        {
            if (this.AnimationManager == null)
            {
                if (this.CurrentTextureToDisplay == null)
                {
                    RevitalizeModCore.log("Texture null for item: " + this.basicItemInformation.id);
                    return;
                }
            }

            if (f.ActiveObject.bigCraftable.Value)
            {
                spriteBatch.Draw(this.CurrentTextureToDisplay, objectPosition, this.AnimationManager.getCurrentAnimationFrameRectangle(), this.basicItemInformation.DrawColor, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + 2) / 10000f));
                return;
            }

            spriteBatch.Draw(this.CurrentTextureToDisplay, objectPosition, this.AnimationManager.getCurrentAnimationFrameRectangle(), this.basicItemInformation.DrawColor, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + 2) / 10000f));
            if (f.ActiveObject != null && f.ActiveObject.Name.Contains("="))
            {
                spriteBatch.Draw(this.CurrentTextureToDisplay, objectPosition + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), this.AnimationManager.getCurrentAnimationFrameRectangle(), Color.White, 0f, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), (float)Game1.pixelZoom + Math.Abs(Game1.starCropShimmerPause) / 8f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + 2) / 10000f));
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

        /// <summary>What happens when the object is drawn at a tile location.</summary>
        public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
        {
            if (x <= -1)
            {
                x = (int)this.TileLocation.X;
                //spriteBatch.Draw(this.basicItemInfo.animationManager.getTexture(), Game1.GlobalToLocal(Game1.viewport, this.TileLocation), new Rectangle?(this.AnimationManager.currentAnimation.sourceRectangle), this.basicItemInfo.DrawColor * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, (float)(this.TileLocation.Y * Game1.tileSize) / 10000f));
            }
            if (y <= -1)
            {
                y = (int)this.TileLocation.Y;
            }

            if (this.AnimationManager == null)
            {
                spriteBatch.Draw(this.basicItemInformation.animationManager.getTexture(), Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize) + this.basicItemInformation.shakeTimerOffset(), (y * Game1.tileSize) + this.basicItemInformation.shakeTimerOffset())), new Rectangle?(this.AnimationManager.getCurrentAnimation().getCurrentAnimationFrameRectangle()), this.basicItemInformation.DrawColor * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, (float)(y * Game1.tileSize) / 10000f));
            }
            else
            {
                this.basicItemInformation.animationManager.draw(spriteBatch, this.basicItemInformation.animationManager.getTexture(), Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize) + this.basicItemInformation.shakeTimerOffset(), (y * Game1.tileSize) + this.basicItemInformation.shakeTimerOffset())), new Rectangle?(this.AnimationManager.getCurrentAnimation().getCurrentAnimationFrameRectangle()), this.basicItemInformation.DrawColor * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, (float)((y) * Game1.tileSize) / 10000f) + .00001f);
                if (this.heldObject.Value != null) SpriteBatchUtilities.Draw(spriteBatch, this, this.heldObject.Value, alpha, 0);
            }
        }

        public override void drawPlacementBounds(SpriteBatch spriteBatch, GameLocation location)
        {
            //Need to update this????
            base.drawPlacementBounds(spriteBatch, location);
        }

        /// <summary>Draw the game object at a non-tile spot. Aka like debris.</summary>
        public override void draw(SpriteBatch spriteBatch, int xNonTile, int yNonTile, float layerDepth, float alpha = 1f)
        {

            if (this.AnimationManager == null)
            {
                spriteBatch.Draw(this.CurrentTextureToDisplay, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(xNonTile), yNonTile)), new Rectangle?(this.AnimationManager.getCurrentAnimationFrameRectangle()), this.basicItemInformation.DrawColor * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, layerDepth));
            }

            else
            {
                //Log.AsyncC("Animation Manager is working!");
                this.AnimationManager.draw(spriteBatch, this.CurrentTextureToDisplay, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(xNonTile), yNonTile)), new Rectangle?(this.AnimationManager.getCurrentAnimationFrameRectangle()), this.basicItemInformation.DrawColor * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, layerDepth));
            }
        }


        public virtual LightManager GetLightManager()
        {
            return this.basicItemInformation.lightManager;
        }

        public override void AttemptRemoval(Action<Furniture> removal_action)
        {

            if (this.getCurrentLocation().furniture.Contains(this)) ;
            {
                RevitalizeModCore.log("Furniture is contained inside of game location. Need to update removal logic.");
            }

            this.pickupFromGameWorld(this.TileLocation, this.getCurrentLocation());


            Game1.player.currentLocation.localSound("coin");

            //base.AttemptRemoval(removal_action);
        }



    }
}
