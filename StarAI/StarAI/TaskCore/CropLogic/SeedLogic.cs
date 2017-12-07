using Microsoft.Xna.Framework;
using StarAI.PathFindingCore;
using StardewValley;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xTile.Dimensions;

namespace StarAI.TaskCore.CropLogic
{
    class SeedLogic
    {

        public static void makeAsMuchDirtAsPossible(GameLocation location)
        {
            List<TileNode> hoeDirtThings = new List<TileNode>();
            for(int i = 0; i <= location.map.Layers[0].LayerSize.Width;i++)
            {
                for (int j = 0; j <= location.map.Layers[0].LayerSize.Height;j++)
                {
                    if(canBeHoeDirt(location, new Vector2(i, j)))
                    {
                        TileNode t = new TileNode(1, Vector2.Zero, Path.Combine("Tiles", "GenericUncoloredTile.xnb"), Path.Combine("Tiles", "TileData.xnb"), StardustCore.IlluminateFramework.Colors.invertColor(StardustCore.IlluminateFramework.ColorsList.LightSkyBlue));
                        t.placementAction(Game1.currentLocation, (int)i* Game1.tileSize, (int)j * Game1.tileSize);
                        //StardustCore.Utilities.masterAdditionList.Add(new StardustCore.DataNodes.PlacementNode(t, Game1.currentLocation, (int)v.Key.X * Game1.tileSize, (int)v.Key.Y * Game1.tileSize));
                        PathFindingCore.Utilities.tileExceptionList.Add(new TileExceptionMetaData(t, "HoeDirt"));
                        hoeDirtThings.Add(t);
                    }
                }
            }


        }


        public static bool canBeHoeDirt(GameLocation location, Vector2 tileLocation)
        {
            if (location.doesTileHaveProperty((int)tileLocation.X, (int)tileLocation.Y, "Diggable", "Back") == null || location.isTileOccupied(tileLocation, "") || !location.isTilePassable(new Location((int)tileLocation.X, (int)tileLocation.Y), Game1.viewport))
                return false;
            else return true;
            //this.terrainFeatures.Add(tileLocation, (TerrainFeature)new HoeDirt(!Game1.isRaining || !this.isOutdoors ? 0 : 1));
        }

        public static Crop parseCropFromSeedIndex(int index)
        {
            return new Crop(index, 0, 0);
        }

        public static KeyValuePair<int, Crop> getSeedCropPair(int index) {

            return new KeyValuePair<int, Crop>(index, parseCropFromSeedIndex(index));
        }

        public static void buySeeds()
        {
           var retList= ShopCore.ShopLogic.getGeneralStoreSeedStock(true);
            var item = retList.ElementAt(0);

            while (Game1.player.money >= item.salePrice())
            {
                item.Stack++;
                Game1.player.money -= item.salePrice();
            }
            Game1.player.addItemToInventoryBool(item);

        }

    }
}
