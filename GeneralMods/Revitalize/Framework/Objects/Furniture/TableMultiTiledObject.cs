using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using PyTK.CustomElementHandler;
using StardewValley;

namespace Revitalize.Framework.Objects.Furniture
{
    public class TableMultiTiledObject:MultiTiledObject
    {
        public TableMultiTiledObject() : base()
        {

        }

        public TableMultiTiledObject(BasicItemInformation Info) : base(Info)
        {

        }

        public TableMultiTiledObject(BasicItemInformation Info, Vector2 TilePosition) : base(Info, TilePosition)
        {

        }

        public TableMultiTiledObject(BasicItemInformation Info, Vector2 TilePosition, Dictionary<Vector2, MultiTiledComponent> Objects) : base(Info, TilePosition, Objects)
        {


        }

        public override void rotate()
        {

            base.rotate();
        }

        public override Item getOne()
        {
            Dictionary<Vector2, MultiTiledComponent> objs = new Dictionary<Vector2, MultiTiledComponent>();
            foreach (var pair in this.objects)
            {
                objs.Add(pair.Key, (MultiTiledComponent)pair.Value.getOne());
            }


            return new TableMultiTiledObject(this.info, this.TileLocation, objs);
        }


        public override ICustomObject recreate(Dictionary<string, string> additionalSaveData, object replacement)
        {
            TableMultiTiledObject obj = (TableMultiTiledObject)Revitalize.ModCore.Serializer.DeserializeGUID<TableMultiTiledObject>(additionalSaveData["GUID"]);
            if (obj == null)
            {
                return null;
            }

            Dictionary<Vector2, Guid> guids = new Dictionary<Vector2, Guid>();

            foreach (KeyValuePair<Vector2, Guid> pair in obj.childrenGuids)
            {
                guids.Add(pair.Key, pair.Value);
            }

            foreach (KeyValuePair<Vector2, Guid> pair in guids)
            {
                obj.childrenGuids.Remove(pair.Key);
                //Revitalize.ModCore.log("DESERIALIZE: " + pair.Value.ToString());
                TableTileComponent component = Revitalize.ModCore.Serializer.DeserializeGUID<TableTileComponent>(pair.Value.ToString());
                component.InitNetFields();

                obj.addComponent(pair.Key, component);


            }
            obj.InitNetFields();

            if (!Revitalize.ModCore.ObjectGroups.ContainsKey(additionalSaveData["GUID"]))
            {
                Revitalize.ModCore.ObjectGroups.Add(additionalSaveData["GUID"], obj);
                return obj;
            }
            else
            {
                return Revitalize.ModCore.ObjectGroups[additionalSaveData["GUID"]];
            }


        }


        public override bool canBePlacedHere(GameLocation l, Vector2 tile)
        {
            return base.canBePlacedHere(l, tile);
        }

    }
}
