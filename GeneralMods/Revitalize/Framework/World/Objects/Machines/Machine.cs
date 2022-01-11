using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Revitalize.Framework.Objects.InformationFiles;
using Revitalize.Framework.World.Objects.InformationFiles;
using Revitalize.Framework.World.Objects.Interfaces;
using Revitalize.Framework;
using Revitalize.Framework.Crafting;
using Revitalize.Framework.Menus;
using Revitalize.Framework.Objects;
using Revitalize.Framework.Utilities;
using StardewValley;
using StardustCore.Animations;
using StardustCore.UIUtilities;
using StardustCore.UIUtilities.MenuComponents.ComponentsV2.Buttons;
using Revitalize.Framework.Utilities.Extensions;

namespace Revitalize.Framework.World.Objects.Machines
{
    [XmlType("Mods_Revitalize.Framework.World.Objects.Machines.Machine")]
    public class Machine : CustomObject, IInventoryManagerProvider
    {

        public const string MachineStatusBubble_DefaultAnimationKey= "Default";
        public const string MachineStatusBubble_BlankBubbleAnimationKey = "Blank";
        public const string MachineStatusBubble_InventoryFullAnimationKey = "InventoryFull";


        [XmlIgnore]
        public List<ResourceInformation> producedResources
        {
            get
            {
                return MachineUtilities.GetResourcesProducedByThisMachine(this.basicItemInfo.id);
            }
            set
            {
                if (MachineUtilities.ResourcesForMachines == null) MachineUtilities.InitializeResourceList();
                if (MachineUtilities.ResourcesForMachines.ContainsKey(this.basicItemInfo.id)) return;
                MachineUtilities.ResourcesForMachines.Add(this.basicItemInfo.id, value);
            }
        }

        [XmlIgnore]
        public AnimationManager machineStatusBubbleBox = new AnimationManager();

        public Machine()
        {

        }


        public Machine(BasicItemInformation info, List<ResourceInformation> ProducedResources = null) : base(info)
        {
            this.producedResources = ProducedResources ?? new List<ResourceInformation>();
            this.createStatusBubble();
        }

        public Machine(BasicItemInformation info, Vector2 TileLocation, List<ResourceInformation> ProducedResources = null) : base(info, TileLocation)
        {
            this.producedResources = ProducedResources ?? new List<ResourceInformation>();
            this.createStatusBubble();
        }

        protected override void initNetFieldsPostConstructor()
        {
            base.initNetFieldsPostConstructor();
            this.NetFields.AddFields(this.machineStatusBubbleBox.getNetFields());
        }

        public virtual bool doesMachineProduceItems()
        {
            return this.producedResources.Count > 0;
        }

        public override bool minutesElapsed(int minutes, GameLocation environment)
        {

            return true;
            //return base.minutesElapsed(minutes, environment);
        }


        protected virtual void createStatusBubble()
        {
            this.machineStatusBubbleBox = new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Revitalize.HUD", "MachineStatusBubble"), new SerializableDictionary<string, Animation>()
            {
                {MachineStatusBubble_DefaultAnimationKey,new Animation(0,0,20,24)},
                {MachineStatusBubble_BlankBubbleAnimationKey,new Animation(20,0,20,24)},
                {MachineStatusBubble_InventoryFullAnimationKey,new Animation(40,0,20,24)}
            }, MachineStatusBubble_DefaultAnimationKey, MachineStatusBubble_DefaultAnimationKey, 0);
        }

        public override void updateWhenCurrentLocation(GameTime time, GameLocation environment)
        {
            base.updateWhenCurrentLocation(time, environment);

        }

        

        public override bool rightClicked(Farmer who)
        {
            if (Game1.menuUp || Game1.currentMinigame != null) return false;
            return false;
        }

        public override Item getOne()
        {
            Machine component = new Machine(this.basicItemInfo.Copy(), this.producedResources);
            return component;
        }

        /// <summary>What happens when the object is drawn at a tile location.</summary>
        public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
        {
            base.draw(spriteBatch,x, y, alpha);
            this.drawStatusBubble(spriteBatch, x, y, alpha);
        }

        public virtual void produceItem()
        {
            foreach (ResourceInformation r in this.producedResources)
            {
                if (r.shouldDropResource())
                {
                    Item i = r.getItemDrops();
                    this.GetInventoryManager().addItem(i);
                    //ModCore.log("Produced an item!");
                }
            }

        }

        protected virtual void drawStatusBubble(SpriteBatch b, int x, int y, float Alpha)
        {
            if (this.machineStatusBubbleBox == null) this.createStatusBubble();
            if (this.GetInventoryManager() == null) return;
            if (this.GetInventoryManager().isFull() && this.doesMachineProduceItems())
            {
                y--;
                float num = (float)(4.0 * Math.Round(Math.Sin(DateTime.UtcNow.TimeOfDay.TotalMilliseconds / 250.0), 2));
                this.machineStatusBubbleBox.playAnimation(MachineStatusBubble_InventoryFullAnimationKey);
                this.machineStatusBubbleBox.draw(b, this.machineStatusBubbleBox.getTexture(), Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), y * Game1.tileSize + num)), new Rectangle?(this.machineStatusBubbleBox.getCurrentAnimationFrameRectangle()), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)((y + 2) * Game1.tileSize) / 10000f) + .00001f);
            }
        }


        public virtual ref InventoryManager GetInventoryManager()
        {
            if (this.basicItemInfo == null)
            {
                return ref this.basicItemInfo.inventory;
            }
            return ref this.basicItemInfo.inventory;
        }

        public virtual void SetInventoryManager(InventoryManager Manager)
        {
            this.basicItemInfo.inventory = Manager;
        }



    }
}
