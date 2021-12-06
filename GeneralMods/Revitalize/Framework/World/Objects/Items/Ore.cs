using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Revitalize.Framework.World.Objects.InformationFiles;
using Revitalize.Framework.Objects;
using StardewValley;
using System.Xml.Serialization;

namespace Revitalize.Framework.World.Objects.Items
{
    [XmlType("Mods_Revitalize.Framework.World.Objects.Items.Ore")]
    public class Ore:CustomObject
    {
        public Ore()
        {

        }

        public Ore(BasicItemInformation BasicItemInfo): base(BasicItemInfo)
        {
            this.basicItemInfo = BasicItemInfo;

        }

        public Ore(BasicItemInformation BasicItemInfo, int StackSize) : base(BasicItemInfo, StackSize)
        {
            this.basicItemInfo = BasicItemInfo;
        }

        public Ore(BasicItemInformation BasicItemInfo, Vector2 TilePosition) : base(BasicItemInfo,TilePosition)
        {
            this.basicItemInfo = BasicItemInfo;

        }

        public Ore(BasicItemInformation BasicItemInfo, Vector2 TilePosition, int StackSize=1) : base(BasicItemInfo,TilePosition,StackSize)
        {
            this.basicItemInfo = BasicItemInfo;

        }

        public override Item getOne()
        {
            Ore component = new Ore(this.basicItemInfo.Copy(),Vector2.Zero,1);
            return component;
        }

    }
}
