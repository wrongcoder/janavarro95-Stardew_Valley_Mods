using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardustCore.Animations;
using Revitalize.Framework.Utilities;
using StardewValley;
using StardustCore.UIUtilities;
using Newtonsoft.Json;
using Revitalize.Framework;
using Revitalize.Framework.Managers;
using Revitalize.Framework.Illuminate;
using System.Xml.Serialization;
using Netcode;
using System.Collections.Generic;

namespace Revitalize.Framework.World.Objects.InformationFiles
{
    [XmlType("Mods_Revitalize.Framework.World.Objects.InformationFiles.BasicItemInformation")]
    public class BasicItemInformation
    {
        public readonly NetString name = new NetString();

        public readonly NetString id=new NetString();

        public readonly NetString description=new NetString();

        public readonly NetString categoryName = new NetString();

        public readonly NetColor categoryColor=new NetColor();

        public readonly NetInt price=new NetInt();

        public readonly NetInt healthRestoredOnEating=new NetInt();
        public readonly NetInt staminaRestoredOnEating=new NetInt();

        public readonly NetInt fragility = new NetInt();

        public readonly NetBool canBeSetIndoors = new NetBool();

        public readonly NetBool canBeSetOutdoors = new NetBool();

        public readonly NetBool isLamp = new NetBool();

        public readonly NetString locationName = new NetString();

        public AnimationManager animationManager = new AnimationManager();

        public readonly NetVector2 drawPosition = new NetVector2();

        public readonly NetColor _drawColorBase=new NetColor();

        [XmlIgnore]
        public Color DrawColor
        {
            get
            {
                if (this.dyedColor != null)
                {

                    if (this.dyedColor.color.A != 0)
                    {
                        return this.dyedColor.getBlendedColor(this._drawColorBase);
                        //return new Color( (this._drawColor.R + this._dyedColor.color.R)/2, (this._drawColor.G + this._dyedColor.color.G)/2, (this._drawColor.B + this._dyedColor.color.B)/2, 255);
                        //return new Color(this._drawColor.R * this._dyedColor.color.R, this._drawColor.G * this._dyedColor.color.G, this._drawColor.B * this._dyedColor.color.B, 255);
                    }

                }
                return this._drawColorBase;
            }
            set
            {
                this._drawColorBase.Value = value;
            }
        }


        public readonly NetBool ignoreBoundingBox = new NetBool();

        public InventoryManager inventory = new InventoryManager();

        public LightManager lightManager = new LightManager();

        public readonly NetEnum<Enums.Direction> facingDirection = new NetEnum<Enums.Direction>();

        public readonly NetInt shakeTimer = new NetInt();

        public readonly NetBool alwaysDrawAbovePlayer = new NetBool();

        public NamedColor dyedColor;

        /// <summary>
        /// The dimensions for the game's bounding box in the number of TILES. So a Vector2(1,1) would have 1 tile width and 1 tile height.
        /// </summary>
        public readonly NetVector2 boundingBoxTileDimensions = new NetVector2();

        [JsonIgnore]
        public bool requiresUpdate;
        public BasicItemInformation()
        {
            this.name.Value = "";
            this.description.Value = "";
            this.categoryName.Value = "";
            this.categoryColor.Value = new Color(0, 0, 0);
            this.price.Value = 0;
            this.staminaRestoredOnEating.Value = -300;
            this.healthRestoredOnEating.Value = -300;
            this.canBeSetIndoors.Value = false;
            this.canBeSetOutdoors.Value = false;

            this.animationManager = new AnimationManager();
            this.drawPosition.Value = Vector2.Zero;
            this.DrawColor = Color.White;
            this.inventory = new InventoryManager();
            this.lightManager = new LightManager();

            this.facingDirection.Value = Enums.Direction.Down;
            this.id.Value = "";
            this.shakeTimer.Value = 0;
            this.alwaysDrawAbovePlayer.Value = false;

            this.ignoreBoundingBox.Value = false;
            this.boundingBoxTileDimensions.Value = new Vector2(1, 1);
            this.dyedColor = new NamedColor();
        }

        
        public BasicItemInformation(string Name, string Id, string Description, string CategoryName, Color CategoryColor, int Fragility, bool IsLamp, int Price, AnimationManager animationManager, bool IgnoreBoundingBox, Vector2 BoundingBoxTileDimensions, InventoryManager Inventory=null, LightManager Lights=null, bool AlwaysDrawAbovePlayer = false, NamedColor DyedColor = null):this(Name,Id,Description,CategoryName,CategoryColor,-300,-300,Fragility,IsLamp,Price,true,true,animationManager.getTexture(),animationManager,Color.White,IgnoreBoundingBox,BoundingBoxTileDimensions,Inventory,Lights,AlwaysDrawAbovePlayer,DyedColor)
        {

        }
        

        public BasicItemInformation(string name, string id, string description, string categoryName, Color categoryColor, int staminaRestoredOnEating, int healthRestoredOnEating, int fragility, bool isLamp, int price, bool canBeSetOutdoors, bool canBeSetIndoors, Texture2D texture, AnimationManager animationManager, Color drawColor, bool ignoreBoundingBox, Vector2 BoundingBoxTileDimensions, InventoryManager Inventory, LightManager Lights, bool AlwaysDrawAbovePlayer = false, NamedColor DyedColor = null)
        {
            this.name.Value = name;
            this.id.Value = id;
            this.description.Value = description;
            this.categoryName.Value = categoryName;
            this.categoryColor.Value = categoryColor;
            this.price.Value = price;
            this.staminaRestoredOnEating.Value = staminaRestoredOnEating;
            this.healthRestoredOnEating.Value = healthRestoredOnEating;

            this.canBeSetOutdoors.Value = canBeSetOutdoors;
            this.canBeSetIndoors.Value = canBeSetIndoors;
            this.fragility.Value = fragility;
            this.isLamp.Value = isLamp;

            this.animationManager = animationManager;
            if (this.animationManager.IsNull)
            {
                this.animationManager = new AnimationManager(new Texture2DExtended(), new Animation(new Rectangle(0, 0, 16, 16)), false);
                this.animationManager.getExtendedTexture().texture = texture;
            }

            this.drawPosition.Value = Vector2.Zero;
            this.DrawColor = drawColor;
            this.ignoreBoundingBox.Value = ignoreBoundingBox;
            this.boundingBoxTileDimensions.Value = BoundingBoxTileDimensions;
            this.inventory = Inventory ?? new InventoryManager();
            this.lightManager = Lights ?? new LightManager();
            this.facingDirection.Value = Enums.Direction.Down;
            this.shakeTimer.Value = 0;

            this.alwaysDrawAbovePlayer.Value = AlwaysDrawAbovePlayer;

            this.dyedColor = DyedColor ?? new NamedColor("", new Color(0, 0, 0, 0), Enums.DyeBlendMode.Blend,0.5f);
        }

        /// <summary>
        /// Gets an x offset for shaking an object. Source code used from game.
        /// </summary>
        /// <returns></returns>
        public int shakeTimerOffset()
        {
            return (this.shakeTimer > 0 ? Game1.random.Next(-1, 2) : 0);
        }

        /// <summary>
        /// Returns a copy of this object.
        /// </summary>
        /// <returns></returns>
        public BasicItemInformation Copy()
        {
            return new BasicItemInformation(this.name, this.id, this.description, this.categoryName, this.categoryColor, this.staminaRestoredOnEating, this.healthRestoredOnEating, this.fragility, this.isLamp, this.price, this.canBeSetOutdoors, this.canBeSetIndoors, this.animationManager.getTexture(), this.animationManager.Copy(), this.DrawColor, this.ignoreBoundingBox, this.boundingBoxTileDimensions, this.inventory.Copy(), this.lightManager.Copy(), this.alwaysDrawAbovePlayer, this.dyedColor);
        }


        /// <summary>
        /// Gets the name attached to the dyed color.
        /// </summary>
        /// <returns></returns>
        public string getDyedColorName()
        {
            if (this.dyedColor == null)
            {
                return "";
            }
            if (this.dyedColor.color.A == 0)
            {
                return "";
            }
            else
            {
                return this.dyedColor.name;
            }
        }

        /// <summary>
        /// Gets the netfields that should be synced across server/clients.
        /// </summary>
        /// <returns></returns>
        public virtual List<INetSerializable> getNetFields()
        {
            List<INetSerializable> fields= new List<INetSerializable>() {
                this.name,
                this.id,
                this.description,
                this.categoryName,
                this.categoryColor,
                this.price,
                this.healthRestoredOnEating,
                this.staminaRestoredOnEating,
                this.fragility,
                this.canBeSetIndoors,
                this.canBeSetOutdoors,
                this.isLamp,
                this.locationName,
                this.drawPosition,
                this._drawColorBase,
                this.ignoreBoundingBox,
                this.facingDirection,
                this.shakeTimer,
                this.alwaysDrawAbovePlayer,
                this.boundingBoxTileDimensions
            };

            fields.AddRange(this.animationManager.getNetFields());
            fields.AddRange(this.inventory.getNetFields());
            fields.AddRange(this.lightManager.getNetFields());
            fields.AddRange(this.dyedColor.getNetFields());

            return fields;
        }

    }
}
