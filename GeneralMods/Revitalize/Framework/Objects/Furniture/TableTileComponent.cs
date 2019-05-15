using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using PyTK.CustomElementHandler;
using Revitalize.Framework.Objects.InformationFiles.Furniture;
using StardewValley;

namespace Revitalize.Framework.Objects.Furniture
{
    public class TableTileComponent : FurnitureTileComponent
    {
        public TableInformation furnitureInfo;

        public bool CanPlaceItemsHere
        {
            get
            {
                return this.furnitureInfo.canPlaceItemsHere;
            }
        }


        public TableTileComponent() : base()
        {

        }

        public TableTileComponent(BasicItemInformation Info, TableInformation FurnitureInfo) : base(Info)
        {
            this.furnitureInfo = FurnitureInfo;
        }

        public TableTileComponent(BasicItemInformation Info, Vector2 TileLocation, TableInformation FurnitureInfo) : base(Info, TileLocation)
        {
            this.furnitureInfo = FurnitureInfo;
        }


        public override bool clicked(Farmer who)
        {
            return base.clicked(who);
        }

        public override bool rightClicked(Farmer who)
        {
            ///Not sure.
            return true;
            //return base.rightClicked(who);
        }

        public override bool shiftRightClicked(Farmer who)
        {
            return base.shiftRightClicked(who);
        }


        public override Item getOne()
        {
            TableTileComponent component = new TableTileComponent(this.info, (TableInformation)this.furnitureInfo);
            component.containerObject = this.containerObject;
            component.offsetKey = this.offsetKey;
            return component;
        }

        public override ICustomObject recreate(Dictionary<string, string> additionalSaveData, object replacement)
        {
            //instead of using this.offsetkey.x use get additional save data function and store offset key there

            Vector2 offsetKey = new Vector2(Convert.ToInt32(additionalSaveData["offsetKeyX"]), Convert.ToInt32(additionalSaveData["offsetKeyY"]));
            TableTileComponent self = Revitalize.ModCore.Serializer.DeserializeGUID<TableTileComponent>(additionalSaveData["GUID"]);
            if (self == null)
            {
                return null;
            }

            if (!Revitalize.ModCore.ObjectGroups.ContainsKey(additionalSaveData["ParentGUID"]))
            {
                //Get new container
                TableMultiTiledObject obj = (TableMultiTiledObject)Revitalize.ModCore.Serializer.DeserializeGUID<TableMultiTiledObject>(additionalSaveData["ParentGUID"]);
                self.containerObject = obj;
                obj.addComponent(offsetKey, self);
                //Revitalize.ModCore.log("ADD IN AN OBJECT!!!!");
                Revitalize.ModCore.ObjectGroups.Add(additionalSaveData["ParentGUID"], obj);
            }
            else
            {
                self.containerObject = Revitalize.ModCore.ObjectGroups[additionalSaveData["ParentGUID"]];
                Revitalize.ModCore.ObjectGroups[additionalSaveData["GUID"]].addComponent(offsetKey, self);
                //Revitalize.ModCore.log("READD AN OBJECT!!!!");
            }

            return (ICustomObject)self;
        }

        public override Dictionary<string, string> getAdditionalSaveData()
        {
            Dictionary<string, string> saveData = base.getAdditionalSaveData();
            Revitalize.ModCore.Serializer.SerializeGUID(this.containerObject.childrenGuids[this.offsetKey].ToString(), this);

            return saveData;

        }
    }
}
