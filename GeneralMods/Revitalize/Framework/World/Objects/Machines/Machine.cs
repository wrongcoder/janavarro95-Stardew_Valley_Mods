using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using Netcode;
using Omegasis.Revitalize.Framework.Utilities;
using Omegasis.Revitalize.Framework.World.Objects.InformationFiles;
using Omegasis.Revitalize.Framework.World.Objects.Interfaces;
using Omegasis.StardustCore.Animations;
using Omegasis.StardustCore.UIUtilities;

namespace Omegasis.Revitalize.Framework.World.Objects.Machines
{
    [XmlType("Mods_Revitalize.Framework.World.Objects.Machines.Machine")]
    public class Machine : CustomObject, IInventoryManagerProvider
    {

        public const string MachineStatusBubble_DefaultAnimationKey = "Default";
        public const string MachineStatusBubble_BlankBubbleAnimationKey = "Blank";
        public const string MachineStatusBubble_InventoryFullAnimationKey = "InventoryFull";


        [XmlIgnore]
        public List<ResourceInformation> producedResources
        {
            get
            {
                return MachineUtilities.GetResourcesProducedByThisMachine(this.basicItemInformation.id);
            }
            set
            {
                if (MachineUtilities.ResourcesForMachines == null) MachineUtilities.InitializeResourceList();
                if (MachineUtilities.ResourcesForMachines.ContainsKey(this.basicItemInformation.id)) return;
                MachineUtilities.ResourcesForMachines.Add(this.basicItemInformation.id, value);
            }
        }

        [XmlIgnore]
        public NetRef<AnimationManager> machineStatusBubbleBox = new NetRef<AnimationManager>();

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

        protected override void initializeNetFieldsPostConstructor()
        {
            base.initializeNetFieldsPostConstructor();
            this.NetFields.AddFields(this.machineStatusBubbleBox);
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
            this.machineStatusBubbleBox.Value = new AnimationManager(TextureManager.GetExtendedTexture(RevitalizeModCore.Manifest, "Revitalize.HUD", "MachineStatusBubble"), new SerializableDictionary<string, Animation>()
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
            Machine component = new Machine(this.basicItemInformation.Copy(), this.producedResources);
            return component;
        }

        /// <summary>What happens when the object is drawn at a tile location.</summary>
        public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
        {
            base.draw(spriteBatch, x, y, alpha);
            this.drawStatusBubble(spriteBatch, x, y, alpha);
        }

        public virtual void produceItem()
        {
            foreach (ResourceInformation r in this.producedResources)
                if (r.shouldDropResource())
                {
                    Item i = r.getItemDrops();
                    this.GetInventoryManager().addItem(i);
                    //ModCore.log("Produced an item!");
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
                this.machineStatusBubbleBox.Value.playAnimation(MachineStatusBubble_InventoryFullAnimationKey);
                this.machineStatusBubbleBox.Value.draw(b, this.machineStatusBubbleBox.Value.getTexture(), Game1.GlobalToLocal(Game1.viewport, new Vector2(x * Game1.tileSize, y * Game1.tileSize + num)), new Rectangle?(this.machineStatusBubbleBox.Value.getCurrentAnimationFrameRectangle()), Color.White, 0f, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (y + 2) * Game1.tileSize / 10000f) + .00001f);
            }
        }


        public virtual InventoryManager GetInventoryManager()
        {
            if (this.basicItemInformation == null)
                return this.basicItemInformation.inventory;
            return this.basicItemInformation.inventory;
        }

        public virtual void SetInventoryManager(InventoryManager Manager)
        {
            this.basicItemInformation.inventory = Manager;
        }



    }
}
