using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Buildings;

namespace Omegasis.Revitalize.Framework.World.Buildings
{
    [XmlType("Mods_Revitalize.Buildings.CustomBuilding")]
    public class CustomBuilding:Building
    {

        public CustomBuilding()
        {

        }

        public CustomBuilding(BluePrint bluePrint, Vector2 TileLocation):base(bluePrint,TileLocation)
        {

        }


        public virtual bool isInteractingWithBuilding(Vector2 tileLocation, Farmer who)
        {
            Rectangle rect = new Rectangle(this.tileX.Value, this.tileY.Value, this.tilesWide.Value, this.tilesHigh.Value);
            return rect.Contains(tileLocation);
        }
    }
}
