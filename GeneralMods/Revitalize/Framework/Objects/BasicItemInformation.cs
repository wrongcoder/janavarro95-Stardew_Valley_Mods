using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PyTK.CustomElementHandler;
using Revitalize.Framework.Graphics.Animations;
using Revitalize.Framework.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize.Framework.Objects
{
    public class BasicItemInformation: CustomObjectData
    {
        public string name;
        public string description;
        public string categoryName;
        public Color categoryColor;
        public int price;
        public Vector2 TileLocation;
        public int edibility;
        public int fragility;
        public bool canBeSetIndoors;
        public bool canBeSetOutdoors;
        public bool isLamp;

        public AnimationManager animationManager;
        public Vector2 drawPosition;

        public Color drawColor;

        public bool ignoreBoundingBox;

        public InventoryManager inventory;

        public BasicItemInformation() : base()
        {
            name = "";
            description = "";
            categoryName = "";
            categoryColor = new Color(0, 0, 0);
            price = 0;
            TileLocation = Vector2.Zero;
            this.edibility = -300;
            this.canBeSetIndoors = false;
            this.canBeSetOutdoors = false;

            this.animationManager = null;
            this.drawPosition = Vector2.Zero;
            this.drawColor = Color.White;
            this.inventory = new InventoryManager();
        }

        public BasicItemInformation(string name, string description, string categoryName, Color categoryColor,int edibility,int fragility,bool isLamp,int price, Vector2 TileLocation,bool canBeSetOutdoors,bool canBeSetIndoors,string id, string data, Texture2D texture, Color color,int tileIndex, bool bigCraftable, Type type, CraftingData craftingData, AnimationManager animationManager,Color DrawColor,bool ignoreBoundingBox, InventoryManager Inventory):base(id,data,texture,color,tileIndex,bigCraftable,type,craftingData)
        {
            this.name = name;
            this.description = description;
            this.categoryName = categoryName;
            this.categoryColor = categoryColor;
            this.price = price;
            this.TileLocation = TileLocation;
            this.edibility = edibility;

            this.canBeSetOutdoors = canBeSetOutdoors;
            this.canBeSetIndoors = canBeSetIndoors;
            this.fragility = fragility;
            this.isLamp = isLamp;

            this.animationManager = animationManager;
            if (this.animationManager.IsNull)
            {
                this.animationManager = new AnimationManager(new Graphics.Texture2DExtended(), new Animation(new Rectangle(0, 0, 16, 16)), false);
                this.animationManager.getExtendedTexture().texture = this.texture;
            }
            else
            {
                this.texture = this.animationManager.getTexture();
            }

            this.drawPosition = Vector2.Zero;

            if (DrawColor == null)
            {
                this.drawColor = Color.White;
            }
            else
            {
                this.drawColor = DrawColor;
            }

            this.ignoreBoundingBox = ignoreBoundingBox;

            recreateDataString();
            if (Inventory == null)
            {
                this.inventory = new InventoryManager();
            }
            else
            {
                this.inventory = Inventory;
            }
        }

        public void recreateDataString()
        {
            this.data=this.name+"/"+this.price+"/"+this.edibility+"/"+"Crafting -9"+"/"+this.description+"/"+this.canBeSetOutdoors+"/"+this.canBeSetIndoors+"/"+this.fragility+"/"+this.isLamp+"/"+this.name;
        }



    }
}
