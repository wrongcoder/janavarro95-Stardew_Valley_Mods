using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Omegasis.Revitalize.Framework.World.Objects.InformationFiles
{
    /// <summary>
    /// Used to store basic information regarding object files.
    /// </summary>
    public class JsonItemInformation
    {
        public string id;
        public string name;
        public string description;
        public string categoryId;

        public int healthRestoredOnEating;
        public int staminaRestoredOnEating;

        public int fraglility;
        public int sellingPrice;

        public bool canBeSetIndoors;
        public bool canBeSetOutdoors;

        public bool ignoreBoundingBox;
        public Vector2 boundingBoxTileDimensions;

        public Vector2 drawTileOffset;

        public JsonItemInformation()
        {

        }
    }
}
