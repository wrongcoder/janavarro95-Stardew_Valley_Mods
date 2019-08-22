using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PyTK.CustomElementHandler;
using StardustCore.Animations;
using Revitalize.Framework.Illuminate;
using Revitalize.Framework.Utilities;
using StardewValley;
using StardustCore.UIUtilities;

namespace Revitalize.Framework.Objects
{
    public class BasicItemInformation
    {
        public string name;
        public string id;
        public string description;
        public string categoryName;
        public Color categoryColor;
        public int price;
        public int edibility;
        public int fragility;
        public bool canBeSetIndoors;
        public bool canBeSetOutdoors;
        public bool isLamp;
        public string locationName;


        public AnimationManager animationManager;
        public Vector2 drawPosition;

        public Color drawColor;

        public bool ignoreBoundingBox;

        public InventoryManager inventory;

        public LightManager lightManager;

        public Enums.Direction facingDirection;

        public int shakeTimer;

        public BasicItemInformation()
        {
            this.name = "";
            this.description = "";
            this.categoryName = "";
            this.categoryColor = new Color(0, 0, 0);
            this.price = 0;
            this.edibility = -300;
            this.canBeSetIndoors = false;
            this.canBeSetOutdoors = false;

            this.animationManager = new AnimationManager();
            this.drawPosition = Vector2.Zero;
            this.drawColor = Color.White;
            this.inventory = new InventoryManager();
            this.lightManager = new LightManager();

            this.facingDirection = Enums.Direction.Down;
            this.id = "";
            this.shakeTimer = 0;
           
        }

        public BasicItemInformation(string name, string id, string description, string categoryName, Color categoryColor,int edibility, int fragility, bool isLamp, int price, bool canBeSetOutdoors, bool canBeSetIndoors, Texture2D texture, AnimationManager animationManager, Color drawColor, bool ignoreBoundingBox, InventoryManager Inventory, LightManager Lights)
        {
            this.name = name;
            this.id = id;
            this.description = description;
            this.categoryName = categoryName;
            this.categoryColor = categoryColor;
            this.price = price;
            this.edibility = edibility;

            this.canBeSetOutdoors = canBeSetOutdoors;
            this.canBeSetIndoors = canBeSetIndoors;
            this.fragility = fragility;
            this.isLamp = isLamp;

            this.animationManager = animationManager;
            if (this.animationManager.IsNull)
            {
                this.animationManager = new AnimationManager(new Texture2DExtended(), new Animation(new Rectangle(0, 0, 16, 16)), false);
                this.animationManager.getExtendedTexture().texture = texture;
            }

            this.drawPosition = Vector2.Zero;
            this.drawColor = drawColor;
            this.ignoreBoundingBox = ignoreBoundingBox;
            this.inventory = Inventory ?? new InventoryManager();
            this.lightManager = Lights ?? new LightManager();
            this.facingDirection = Enums.Direction.Down;
            this.shakeTimer = 0;
            
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
            return new BasicItemInformation(this.name, this.id,this.description, this.categoryName, this.categoryColor, this.edibility, this.fragility, this.isLamp, this.price, this.canBeSetOutdoors, this.canBeSetIndoors, this.animationManager.getTexture(), this.animationManager, this.drawColor, this.ignoreBoundingBox, this.inventory.Copy(), this.lightManager.Copy());
        }
        
    }
}
