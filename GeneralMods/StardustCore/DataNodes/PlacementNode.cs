using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardustCore.DataNodes
{
    public class PlacementNode
    {
        public CoreObject cObj;
        public GameLocation location;
        public int x;
        public int y;

        public PlacementNode(CoreObject C, GameLocation Location, int X, int Y) {
            cObj = C;
            location = Location;
            x = X;
            y = Y;
        }

    }
}
