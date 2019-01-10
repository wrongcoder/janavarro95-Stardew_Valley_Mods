using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using PyTK.CustomElementHandler;
using Revitalize.Framework.Graphics.Animations;
using Revitalize.Framework.Illuminate;
using Revitalize.Framework.Utilities;
using StardewValley;

namespace Revitalize.Framework.Objects
{

    /// <summary>
    /// In Order to make this network compatible, I MUST, MUST, MUST make it use this INetSerializable functionality.
    /// </summary>
    public class BasicItemInformation : CustomObjectData, INetSerializable
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

        public LightManager lightManager;

        public Enums.Direction facingDirection;

        public BasicItemInformation()
        {
            this.name = "";
            this.description = "";
            this.categoryName = "";
            this.categoryColor = new Color(0, 0, 0);
            this.price = 0;
            this.TileLocation = Vector2.Zero;
            this.edibility = -300;
            this.canBeSetIndoors = false;
            this.canBeSetOutdoors = false;

            this.animationManager = new AnimationManager();
            this.drawPosition = Vector2.Zero;
            this.drawColor = Color.White;
            this.inventory = new InventoryManager();
            this.lightManager = new LightManager();

            this.facingDirection = Enums.Direction.Down;
           
        }

        public BasicItemInformation(string name, string description, string categoryName, Color categoryColor,int edibility, int fragility, bool isLamp, int price, Vector2 TileLocation, bool canBeSetOutdoors, bool canBeSetIndoors, string id, string data, Texture2D texture, Color color, int tileIndex, bool bigCraftable, Type type, CraftingData craftingData, AnimationManager animationManager, Color drawColor, bool ignoreBoundingBox, InventoryManager Inventory, LightManager Lights) : base(id, data, texture, color, tileIndex, bigCraftable, type, craftingData)
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
                this.texture = this.animationManager.getTexture();

            this.drawPosition = Vector2.Zero;
            this.drawColor = drawColor;
            this.ignoreBoundingBox = ignoreBoundingBox;
            this.recreateDataString();
            this.inventory = Inventory ?? new InventoryManager();
            this.lightManager = Lights ?? new LightManager();
            this.facingDirection = Enums.Direction.Down;
        }

        public uint DirtyTick { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool Dirty => throw new NotImplementedException();

        public bool NeedsTick { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool ChildNeedsTick { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public INetSerializable Parent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public INetRoot Root => throw new NotImplementedException();

        public void MarkClean()
        {
            throw new NotImplementedException();
        }

        public void MarkDirty()
        {
            throw new NotImplementedException();
        }

        public void Read(BinaryReader reader, NetVersion version)
        {
            throw new NotImplementedException();
        }

        public void ReadFull(BinaryReader reader, NetVersion version)
        {
            throw new NotImplementedException();
        }

        public void recreateDataString()
        {
            this.data = $"{this.name}/{this.price}/{this.edibility}/Crafting -9/{this.description}/{this.canBeSetOutdoors}/{this.canBeSetIndoors}/{this.fragility}/{this.isLamp}/{this.name}";
        }

        public bool Tick()
        {
            throw new NotImplementedException();
        }

        public void Write(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public void WriteFull(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
