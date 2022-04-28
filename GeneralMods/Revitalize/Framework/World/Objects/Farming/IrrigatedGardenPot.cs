using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using Omegasis.Revitalize.Framework.Utilities;
using Omegasis.Revitalize.Framework.World.Objects.InformationFiles;
using Omegasis.Revitalize.Framework.World.Objects.Interfaces;
using Omegasis.StardustCore.Animations;
using StardewValley;
using StardewValley.Objects;

namespace Omegasis.Revitalize.Framework.World.Objects.Farming
{
    [XmlType("Mods_Revitalize.Framework.World.Objects.Farming.IrrigatedGardenPot")]
    public class IrrigatedGardenPot : IndoorPot, ICustomModObject
    {

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

        public IrrigatedGardenPot() : base()
        {
        }

        public IrrigatedGardenPot(BasicItemInformation Info) : base()
        {
            this.basicItemInformation = Info;
            this.makeSoilWet();
        }

        public IrrigatedGardenPot(BasicItemInformation Info, Vector2 TilePosition) : base(TilePosition)
        {
            this.basicItemInformation = Info;
            this.makeSoilWet();
        }

        public virtual void makeSoilWet()
        {
            this.hoeDirt.Value.state.Value = 1;
        }


        public override void DayUpdate(GameLocation location)
        {
            base.DayUpdate(location);
            this.hoeDirt.Value.dayUpdate(location, base.tileLocation);
            this.makeSoilWet();
            base.showNextIndex.Value = (int)this.hoeDirt.Value.state == 1;
            if (base.heldObject.Value != null)
            {
                base.readyForHarvest.Value = true;
            }
            if (this.bush.Value != null)
            {
                this.bush.Value.dayUpdate(location);
            }
        }



        public override void drawAsProp(SpriteBatch b)
        {
            base.drawAsProp(b);
        }

        public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
        {
            this.DrawICustomModObjectInMenu(spriteBatch, location, scaleSize, transparency, layerDepth, drawStackNumber, color, drawShadow);
        }

        public override void drawAttachments(SpriteBatch b, int x, int y)
        {
            base.drawAttachments(b, x, y);
        }

        public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
        {
            this.DrawICustomModObjectWhenHeld(spriteBatch, objectPosition, f);
        }

        /// <summary>What happens when the object is drawn at a tile location.</summary>
        public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
        {
            this.DrawICustomModObject(spriteBatch, x, y, this.flipped, alpha, null);

            if ((int)this.hoeDirt.Value.fertilizer != 0)
            {
                Rectangle fertilizer_rect = this.hoeDirt.Value.GetFertilizerSourceRect(this.hoeDirt.Value.fertilizer);
                fertilizer_rect.Width = 13;
                fertilizer_rect.Height = 13;
                spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2(base.tileLocation.X * 64f + 4f, base.tileLocation.Y * 64f - 12f)), fertilizer_rect, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (base.tileLocation.Y + 0.65f) * 64f / 10000f + (float)x * 1E-05f);
            }
            if (this.hoeDirt.Value.crop != null)
            {
                this.hoeDirt.Value.crop.drawWithOffset(spriteBatch, base.tileLocation, ((int)this.hoeDirt.Value.state == 1 && (int)this.hoeDirt.Value.crop.currentPhase == 0 && !this.hoeDirt.Value.crop.raisedSeeds) ? (new Color(180, 100, 200) * 1f) : Color.White, this.hoeDirt.Value.getShakeRotation(), new Vector2(32f, 8f));
            }
            if (base.heldObject.Value != null)
            {
                base.heldObject.Value.draw(spriteBatch, x * 64, y * 64 - 48, (base.tileLocation.Y + 0.66f) * 64f / 10000f + (float)x * 1E-05f, 1f);
            }
            if (this.bush.Value != null)
            {
                this.bush.Value.draw(spriteBatch, new Vector2(x, y), -24f);
            }
        }
    }
}
