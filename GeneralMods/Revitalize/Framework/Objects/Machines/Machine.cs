using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using PyTK.CustomElementHandler;
using Revitalize.Framework.Crafting;
using Revitalize.Framework.Menus;
using Revitalize.Framework.Menus.Machines;
using Revitalize.Framework.Objects.InformationFiles;
using Revitalize.Framework.Utilities;
using StardewValley;
using StardustCore.Animations;
using StardustCore.UIUtilities;
using StardustCore.UIUtilities.MenuComponents.ComponentsV2.Buttons;

namespace Revitalize.Framework.Objects.Machines
{
    public class Machine:MultiTiledComponent
    {

        [JsonIgnore]
        public override string ItemInfo
        {
            get
            {
                string info = Revitalize.ModCore.Serializer.ToJSONString(this.info);
                string guidStr = this.guid.ToString();
                string pyTkData = ModCore.Serializer.ToJSONString(this.data);
                string offsetKey = this.offsetKey != null ? ModCore.Serializer.ToJSONString(this.offsetKey) : "";
                string container = this.containerObject != null ? this.containerObject.guid.ToString() : "";
                string resources = ModCore.Serializer.ToJSONString(this.producedResources);
                string energyRequired = this.energyRequiredPer10Minutes.ToString();
                string timeToProduce = this.timeToProduce.ToString();
                string updatesContainer = this.updatesContainerObjectForProduction.ToString();

                return info + "<" + guidStr + "<" + pyTkData + "<" + offsetKey + "<" + container+"<"+resources+"<"+energyRequired+"<"+timeToProduce+"<"+updatesContainer+"<"+this.craftingRecipeBook;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) return;
                string[] data = value.Split('<');
                string infoString = data[0];
                string guidString = data[1];
                string pyTKData = data[2];
                string offsetVec = data[3];
                string containerObject = data[4];
                string resourcesString = data[5];
                string energyRequired = data[6];
                string time = data[7];
                string updates = data[8];
                this.craftingRecipeBook = data[9];
                this.info = (BasicItemInformation)Revitalize.ModCore.Serializer.DeserializeFromJSONString(infoString, typeof(BasicItemInformation));
                this.data = Revitalize.ModCore.Serializer.DeserializeFromJSONString<CustomObjectData>(pyTKData);
                this.energyRequiredPer10Minutes = Convert.ToInt32(energyRequired);
                this.timeToProduce = Convert.ToInt32(time);
                this.updatesContainerObjectForProduction = Convert.ToBoolean(updates);
                if (string.IsNullOrEmpty(offsetVec)) return;
                if (string.IsNullOrEmpty(containerObject)) return;
                this.offsetKey = ModCore.Serializer.DeserializeFromJSONString<Vector2>(offsetVec);
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

                if (this.containerObject == null)
                {
                    //ModCore.log(containerObject);
                    Guid containerGuid = Guid.Parse(containerObject);
                    if (ModCore.CustomObjects.ContainsKey(containerGuid))
                    {
                        this.containerObject = (MultiTiledObject)ModCore.CustomObjects[containerGuid];
                        this.containerObject.removeComponent(this.offsetKey);
                        this.containerObject.addComponent(this.offsetKey, this);
                        //ModCore.log("Set container object from existing object!");
                    }
                    else
                    {
                        //ModCore.log("Container hasn't been synced???");
                        MultiplayerUtilities.RequestGuidObject(containerGuid);
                        MultiplayerUtilities.RequestGuidObject_Tile(this.guid);
                    }
                }
                else
                {
                    this.containerObject.updateInfo();
                }

                if (ModCore.CustomObjects.ContainsKey(oldGuid) && ModCore.CustomObjects.ContainsKey(this.guid))
                {
                    if (ModCore.CustomObjects[oldGuid] == ModCore.CustomObjects[this.guid] && oldGuid != this.guid)
                    {
                        //ModCore.CustomObjects.Remove(oldGuid);
                    }
                }

                if (string.IsNullOrEmpty(resourcesString) == false)
                {
                    this.producedResources = ModCore.Serializer.DeserializeFromJSONString<List<ResourceInformation>>(resourcesString);
                }
                else
                {
                    if (this.producedResources == null) this.producedResources = new List<ResourceInformation>();
                }
            }
        }

        public List<ResourceInformation> producedResources;
        public int energyRequiredPer10Minutes;
        public int timeToProduce;
        public bool updatesContainerObjectForProduction;

        public string craftingRecipeBook;

        [JsonIgnore]
        public bool ProducesItems
        {
            get
            {
                return this.producedResources.Count > 0;
            }
        }

        [JsonIgnore]
        public bool ConsumesEnergy
        {
            get
            {
                if (ModCore.Configs.machinesConfig.doMachinesConsumeEnergy == false) return false;
                if (this.energyRequiredPer10Minutes == 0) return false;
                if (this.EnergyManager.energyInteractionType == Enums.EnergyInteractionType.Consumes) return true;
                return false;
            }
        }

        public Machine() { }

        public Machine(CustomObjectData PyTKData, BasicItemInformation info,List<ResourceInformation> ProducedResources=null, int EnergyRequiredPer10Minutes = 0,int TimeToProduce=0,bool UpdatesContainer=false, string CraftingBook = "") : base(PyTKData, info) {
            this.producedResources = ProducedResources?? new List<ResourceInformation>();
            this.energyRequiredPer10Minutes = EnergyRequiredPer10Minutes;
            this.timeToProduce = TimeToProduce;
            this.updatesContainerObjectForProduction = UpdatesContainer;
            this.MinutesUntilReady = TimeToProduce;
            this.craftingRecipeBook = CraftingBook;
        }

        public Machine(CustomObjectData PyTKData, BasicItemInformation info, Vector2 TileLocation, List<ResourceInformation> ProducedResources=null,int EnergyRequiredPer10Minutes=0,int TimeToProduce=0,bool UpdatesContainer=false, string CraftingBook = "", MultiTiledObject obj=null) : base(PyTKData, info, TileLocation)
        {
            this.containerObject = obj;
            this.producedResources = ProducedResources ?? new List<ResourceInformation>();
            this.energyRequiredPer10Minutes = EnergyRequiredPer10Minutes;
            this.timeToProduce = TimeToProduce;
            this.updatesContainerObjectForProduction = UpdatesContainer;
            this.MinutesUntilReady = TimeToProduce;
            this.craftingRecipeBook = CraftingBook;
        }

        public Machine(CustomObjectData PyTKData, BasicItemInformation info, Vector2 TileLocation, Vector2 offsetKey,List<ResourceInformation> ProducedResources=null, int EnergyRequiredPer10Minutes = 0, int TimeToProduce=0,bool UpdatesContainer=false, string CraftingBook = "", MultiTiledObject obj = null) : base(PyTKData, info, TileLocation)
        {
            this.offsetKey = offsetKey;
            this.containerObject = obj;
            this.producedResources = ProducedResources?? new List<ResourceInformation>();
            this.timeToProduce = TimeToProduce;
            this.updatesContainerObjectForProduction = UpdatesContainer;
            this.MinutesUntilReady = TimeToProduce;
            this.craftingRecipeBook = CraftingBook;
        }

        public override void updateWhenCurrentLocation(GameTime time, GameLocation environment)
        {
            base.updateWhenCurrentLocation(time, environment);
        }


        public override bool minutesElapsed(int minutes, GameLocation environment)
        {
            if (this.updatesContainerObjectForProduction)
            {
                //ModCore.log("Update container object for production!");
                //this.MinutesUntilReady -= minutes;
                int remaining = minutes;
                //ModCore.log("Minutes elapsed: " + remaining);
                List<MultiTiledObject> energySources = new List<MultiTiledObject>();
                if (this.ConsumesEnergy)
                {
                   energySources = this.EnergyGraphSearchSources(); //Only grab the network once.
                }

                if (this.ProducesItems)
                {
                    //ModCore.log("This produces items!");
                    while (remaining > 0)
                    {

                        if (this.ConsumesEnergy)
                        {
                            this.drainEnergyFromNetwork(energySources); //Continually drain from the network.
                            if (this.EnergyManager.remainingEnergy < this.energyRequiredPer10Minutes) return false;
                        }
                        else
                        {
                            //ModCore.log("Does not produce energy!");
                        }
                        remaining -= 10;
                        this.containerObject.MinutesUntilReady -= 10;

                        if (this.containerObject.MinutesUntilReady <= 0)
                        {
                            this.produceItem();
                            this.containerObject.MinutesUntilReady = this.timeToProduce;
                        }
                    }
                }
                if (this.EnergyManager.energyInteractionType == Enums.EnergyInteractionType.Produces)
                {
                    while (remaining > 0)
                    {
                        remaining -= 10;
                        this.produceEnergy();
                    }
                }
                if (this.MinutesUntilReady>0)
                {
                    this.MinutesUntilReady = Math.Max(0, this.MinutesUntilReady - minutes);

                    if(this.InventoryManager.hasItemsInBuffer && this.MinutesUntilReady == 0)
                    {
                        this.InventoryManager.dumpBufferToItems();
                    }

                }

                return false;
            }
            else
            {

                return false;
            }

            //return base.minutesElapsed(minutes, environment);
        }

        public override bool rightClicked(Farmer who)
        {
            if (this.location == null)
                this.location = Game1.player.currentLocation;
            if (Game1.menuUp || Game1.currentMinigame != null) return false;

            //ModCore.playerInfo.sittingInfo.sit(this, Vector2.Zero);
            this.createMachineMenu();
            return true;
        }

        /// <summary>
        /// Creates the necessary components to display the machine menu properly.
        /// </summary>
        protected virtual void createMachineMenu()
        {
            MachineMenu machineMenu = new MachineMenu((Game1.viewport.Width / 2) - 400, 0, 800, 600);

            MachineSummaryMenu m = new Framework.Menus.Machines.MachineSummaryMenu((Game1.viewport.Width / 2) - 400, 48, 800, 600, Color.White, this.containerObject);
            machineMenu.addInMenuTab("Summary", new AnimatedButton(new StardustCore.Animations.AnimatedSprite("SummaryTab", new Vector2(), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Menus", "MenuTab"), new Animation(0, 0, 24, 24)), Color.White), new Rectangle(0, 0, 24, 24), 2f), m, true);

            if (this.InventoryManager.capacity > 0)
            {
                InventoryTransferMenu transferMenu = new InventoryTransferMenu(100, 150, 500, 600, this.InventoryManager.items, 36);
                machineMenu.addInMenuTab("Inventory", new AnimatedButton(new StardustCore.Animations.AnimatedSprite("Inventory Tab", new Vector2(), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Menus", "MenuTab"), new Animation(0, 0, 24, 24)), Color.White), new Rectangle(0, 0, 24, 24), 2f), transferMenu, false);
            }

            if (string.IsNullOrEmpty(this.craftingRecipeBook)==false)
            {
                CraftingMenuV1 craftingMenu = CraftingRecipeBook.CraftingRecipesByGroup[this.craftingRecipeBook].getCraftingMenuForMachine(100, 100, 400, 700, ref this.InventoryManager.items,ref this.InventoryManager.bufferItems,this);
                machineMenu.addInMenuTab("Crafting", new AnimatedButton(new StardustCore.Animations.AnimatedSprite("Crafting Tab", new Vector2(), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Menus", "MenuTab"), new Animation(0, 0, 24, 24)), Color.White), new Rectangle(0, 0, 24, 24), 2f), craftingMenu, false);
            }

            if (Game1.activeClickableMenu == null) Game1.activeClickableMenu = machineMenu;
        }

        public override Item getOne()
        {
            Machine component = new Machine(this.data, this.info.Copy(), this.TileLocation, this.offsetKey, this.producedResources,this.energyRequiredPer10Minutes,this.timeToProduce,this.updatesContainerObjectForProduction,this.craftingRecipeBook,this.containerObject);
            return component;
        }

        public override ICustomObject recreate(Dictionary<string, string> additionalSaveData, object replacement)
        {
            //instead of using this.offsetkey.x use get additional save data function and store offset key there

            Vector2 offsetKey = new Vector2(Convert.ToInt32(additionalSaveData["offsetKeyX"]), Convert.ToInt32(additionalSaveData["offsetKeyY"]));
            Machine self = Revitalize.ModCore.Serializer.DeserializeGUID<Machine>(additionalSaveData["GUID"]);
            if (self == null)
            {
                return null;
            }

            if (!Revitalize.ModCore.ObjectGroups.ContainsKey(additionalSaveData["ParentGUID"]))
            {
                //Get new container
                MultiTiledObject obj = (MultiTiledObject)Revitalize.ModCore.Serializer.DeserializeGUID<MultiTiledObject>(additionalSaveData["ParentGUID"]);
                self.containerObject = obj;
                obj.addComponent(offsetKey, self);
                //Revitalize.ModCore.log("ADD IN AN OBJECT!!!!");
                Revitalize.ModCore.ObjectGroups.Add(additionalSaveData["ParentGUID"], (MultiTiledObject)obj);
            }
            else
            {
                self.containerObject = Revitalize.ModCore.ObjectGroups[additionalSaveData["ParentGUID"]];
                Revitalize.ModCore.ObjectGroups[additionalSaveData["GUID"]].addComponent(offsetKey, self);
                //Revitalize.ModCore.log("READD AN OBJECT!!!!");
            }

            return (ICustomObject)self;
        }

        public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
        {
            this.updateInfo();
            if (this.info.ignoreBoundingBox == true)
            {
                x *= -1;
                y *= -1;
            }

            if (this.info == null)
            {
                Revitalize.ModCore.log("info is null");
                if (this.syncObject == null) Revitalize.ModCore.log("DEAD SYNC");
            }
            if (this.animationManager == null) Revitalize.ModCore.log("Animation Manager Null");
            if (this.displayTexture == null) Revitalize.ModCore.log("Display texture is null");

            //The actual planter box being drawn.
            if (this.animationManager == null)
            {
                if (this.animationManager.getExtendedTexture() == null)
                    ModCore.ModMonitor.Log("Tex Extended is null???");

                spriteBatch.Draw(this.displayTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), y * Game1.tileSize)), new Rectangle?(this.animationManager.currentAnimation.sourceRectangle), this.info.drawColor * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, (float)(y * Game1.tileSize) / 10000f));
                // Log.AsyncG("ANIMATION IS NULL?!?!?!?!");
            }

            else
            {
                //Log.AsyncC("Animation Manager is working!");
                float addedDepth = 0;


                if (Revitalize.ModCore.playerInfo.sittingInfo.SittingObject == this.containerObject && this.info.facingDirection == Enums.Direction.Up)
                {
                    addedDepth += (this.containerObject.Height - 1) - ((int)(this.offsetKey.Y));
                    if (this.info.ignoreBoundingBox) addedDepth += 1.5f;
                }
                else if (this.info.ignoreBoundingBox)
                {
                    addedDepth += 1.0f;
                }
                this.animationManager.draw(spriteBatch, this.displayTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), y * Game1.tileSize)), new Rectangle?(this.animationManager.currentAnimation.sourceRectangle), this.info.drawColor * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, (float)((y + addedDepth) * Game1.tileSize) / 10000f) + .00001f);
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

        public virtual void produceItem()
        {
            foreach(ResourceInformation r in this.producedResources)
            {
                if (r.shouldDropResource())
                {
                    Item i = r.getItemDrops();
                    this.InventoryManager.addItem(i);
                    //ModCore.log("Produced an item!");
                }  
            }

        }

        public virtual void produceEnergy()
        {
            if (this.EnergyManager.canReceieveEnergy)
            {
                this.EnergyManager.produceEnergy(this.energyRequiredPer10Minutes);
            }

        }


    }
}
