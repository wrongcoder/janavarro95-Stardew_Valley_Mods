using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using Omegasis.Revitalize.Framework.Constants;
using Omegasis.Revitalize.Framework.Utilities;
using Omegasis.Revitalize.Framework.World.Objects.InformationFiles;
using Omegasis.Revitalize.Framework.World.WorldUtilities;
using StardewValley;

namespace Omegasis.Revitalize.Framework.World.Objects.Resources
{
    [XmlType("Mods_Revitalize.Framework.World.Objects.Resources.OreResourceBush")]
    public class ResourceBush:CustomObject
    {


        public NetInt daysItTakesToGrow = new NetInt();
        public NetRef<Item> itemToGrow = new NetRef<Item>();

        public NetInt daysRemainingUntilGrowth = new NetInt();

        public NetRef<Drawable> itemToDraw = new NetRef<Drawable>();
        public NetRef<Drawable> itemToDraw2 = new NetRef<Drawable>();
        public NetRef<Drawable> itemToDraw3 = new NetRef<Drawable>();


        public ResourceBush()
        {

        }

        public ResourceBush(BasicItemInformation Info, Item ItemToGrow, int DaysToGrow) : this(Info,Vector2.Zero,ItemToGrow,DaysToGrow)
        {

        }

        public ResourceBush(BasicItemInformation Info, Vector2 TilePosition, Item ItemToGrow, int DaysToGrow) : base(Info, TilePosition)
        {
            this.itemToGrow.Value = ItemToGrow;
            this.daysItTakesToGrow.Value = DaysToGrow;
            this.daysRemainingUntilGrowth.Value = DaysToGrow;
        }

        protected override void initializeNetFieldsPostConstructor()
        {
            base.initializeNetFieldsPostConstructor();
            this.NetFields.AddFields(this.daysItTakesToGrow, this.itemToGrow, this.daysRemainingUntilGrowth, this.itemToDraw, this.itemToDraw2, this.itemToDraw3);
        }

        public override void doActualDayUpdateLogic(GameLocation location)
        {
            base.doActualDayUpdateLogic(location);

            if (!this.allResourceSlotsFull())
            {
                this.daysRemainingUntilGrowth.Value--;
            }
            if (this.daysRemainingUntilGrowth.Value == 0)
            {
                this.daysRemainingUntilGrowth.Value = this.daysItTakesToGrow.Value;
                if (this.itemToDraw.Value == null)
                {
                    this.itemToDraw.Value = new Drawable(this.itemToGrow.Value.getOne());
                    return;
                }
                if (this.itemToDraw2.Value == null)
                {
                    this.itemToDraw2.Value = new Drawable(this.itemToGrow.Value.getOne());
                    return;
                }
                if (this.itemToDraw3.Value == null)
                {
                    this.itemToDraw3.Value = new Drawable(this.itemToGrow.Value.getOne());
                    return;
                }
            }

        }

        /// <summary>
        /// Returns true if no more resources can be grown on this bush.
        /// </summary>
        /// <returns></returns>
        public virtual bool allResourceSlotsFull()
        {
            return this.itemToDraw.Value != null && this.itemToDraw2.Value != null && this.itemToDraw3.Value != null;
        }

        public override bool rightClicked(Farmer who)
        {

            if (this.isReadyForHarvest())
                if (who.IsLocalPlayer)
                    this.harvest(true);

            return base.rightClicked(who);
        }


        public virtual void harvest(bool AddToPlayersInventory)
        {

            Item item = this.getItemToHarvest();

            if (AddToPlayersInventory)
            {
                SoundUtilities.PlaySound(Enums.StardewSound.coin);
                bool added = Game1.player.addItemToInventoryBool(item);
                if (added == false)
                {
                    WorldUtility.CreateItemDebrisAtTileLocation(this.getCurrentLocation(), item, this.TileLocation);
                    this.itemToDraw.Value = null;
                    this.itemToDraw2.Value = null;
                    this.itemToDraw3.Value = null;
                    return;
                }
            }
            else
            {
                WorldUtility.CreateItemDebrisAtTileLocation(this.getCurrentLocation(), item, this.TileLocation);
            }


        }

        /// <summary>
        /// Gets the item to be harvested from this ore resource bush.
        /// </summary>
        /// <param name="ActuallyHarvest">If true, clear the item off the bush and reset the bush's state.</param>
        /// <returns></returns>
        public virtual Item getItemToHarvest(bool ActuallyHarvest=true)
        {
            Item item = this.itemToGrow.Value.getOne();
            int amountToAdd = 0;

            if (this.itemToDraw.Value != null)
            {
                amountToAdd++;
            }
            if (this.itemToDraw2.Value != null)
            {
                amountToAdd++;
            }
            if (this.itemToDraw3.Value != null)
            {
                amountToAdd++;
            }
            item.Stack = amountToAdd;
            if (item.Stack == 0) return null;

            if (ActuallyHarvest)
            {
                this.itemToDraw.Value = null;
                this.itemToDraw2.Value = null;
                this.itemToDraw3.Value = null;
            }

            return item;
        }

        public override Item getOne()
        {
            return new ResourceBush(this.basicItemInformation.Copy(), this.itemToGrow.Value.getOne(), this.daysItTakesToGrow.Value);
        }

        public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1)
        {
            base.draw(spriteBatch, x, y, alpha);

            Vector2 drawPosition1 = new Vector2(x  - this.basicItemInformation.drawOffset.X, y + .25f + this.basicItemInformation.drawOffset.Y);
            Vector2 drawPosition2 = new Vector2(x - .25f - this.basicItemInformation.drawOffset.X, y + .75f + this.basicItemInformation.drawOffset.Y);
            Vector2 drawPosition3 = new Vector2(x + .25f - this.basicItemInformation.drawOffset.X, y + .75f + this.basicItemInformation.drawOffset.Y);

            if (this.itemToDraw.Value != null)
            {
                //For some reason some of the drawing logic is a bit off for displays, so we want to skip drawing items in the incorrect positions.
                if (drawPosition1.X<0 || drawPosition1.Y<0)
                {
                    return;
                }
                this.itemToDraw.Value.drawAsHeldObject(spriteBatch,drawPosition1 , alpha, -this.basicItemInformation.drawOffset.Y);
            }
            if (this.itemToDraw2.Value != null)
            {
                if (drawPosition2.X < 0 || drawPosition2.Y < 0)
                {
                    return;
                }
                //Change the depth value a bit so that it doesn't draw on top of the player as well.
                this.itemToDraw2.Value.drawAsHeldObject(spriteBatch,drawPosition2 , alpha, -this.basicItemInformation.drawOffset.Y-.75f);
            }
            if (this.itemToDraw3.Value != null)
            {
                if (drawPosition3.X < 0 || drawPosition3.Y < 0)
                {
                    return;
                }
                this.itemToDraw3.Value.drawAsHeldObject(spriteBatch,drawPosition3 , alpha, -this.basicItemInformation.drawOffset.Y-.75f);
            }
            
        }

        public override void performRemoveAction(Vector2 tileLocation, GameLocation environment)
        {
            this.harvest(true);
            base.performRemoveAction(tileLocation, environment);
        }

        public virtual bool isReadyForHarvest()
        {
            return this.itemToDraw.Value!= null || this.itemToDraw2.Value != null || this.itemToDraw3.Value != null;
        }

    }
}
