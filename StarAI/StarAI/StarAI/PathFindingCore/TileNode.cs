using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using Microsoft.Xna.Framework;
using StardewValley;

namespace StarAI.PathFindingCore
{
    public class TileNodeGraph
    {
        public Vector2 position;
        public List<TileNodeGraph> children = new List<TileNodeGraph>();
        public enum stateEnum { NotVisited,Seen,Visited };
        public int seenState;
        public GameLocation location;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Position"></param>
        /// <param name="Location"></param>
        public TileNodeGraph(Vector2 Position,GameLocation Location)
        {
            this.position = Position;
            this.location = Location;
            this.seenState = (int)stateEnum.NotVisited;
            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Position"></param>
        /// <param name="AdjacencyChildren"></param>
        /// <param name="Location"></param>
        public TileNodeGraph(Vector2 Position, List<TileNodeGraph> AdjacencyChildren, GameLocation Location)
        {
            this.position = Position;
            this.children = AdjacencyChildren;
            this.location = Location;
            this.seenState = (int)stateEnum.NotVisited;
        }

        /// <summary>
        /// When called it will get the 8 adjacent tiles around this node, and add them to the adjacency list for this tile node.
        /// </summary>
        public void setAdjacentTiles()
        {
            Vector2 startPosition = this.position;

            for (int x=-1; x<=1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Rectangle r = new Rectangle((int)startPosition.X + x, (int)startPosition.Y + y, Game1.tileSize, Game1.tileSize);
                    if(Game1.currentLocation.isTilePassable(r, Game1.viewport))
                    {
                        TileNodeGraph child = new TileNodeGraph(new Vector2(startPosition.X + x, startPosition.Y + y), this.location);
                        this.children.Add(child);
                    }
                }
            }
        }
    }
}
