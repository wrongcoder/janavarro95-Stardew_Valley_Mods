using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PyTK.CustomElementHandler;
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
        }

        public BasicItemInformation(string name, string description, string categoryName, Color categoryColor,int edibility,int fragility,bool isLamp,int price, Vector2 TileLocation,bool canBeSetOutdoors,bool canBeSetIndoors,string id, string data, Texture2D texture, Color color,int tileIndex, bool bigCraftable, Type type, CraftingData craftingData ):base(id,data,texture,color,tileIndex,bigCraftable,type,craftingData)
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

            recreateDataString();
        }

        public void recreateDataString()
        {
            this.data=this.name+"/"+this.price+"/"+this.edibility+"/"+"Crafting -9"+"/"+this.description+"/"+this.canBeSetOutdoors+"/"+this.canBeSetIndoors+"/"+this.fragility+"/"+this.isLamp+"/"+this.name;
        }

    }
}
