using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Omegasis.Revitalize.Framework.Constants;
using Omegasis.Revitalize.Framework.World.Debris;
using Omegasis.Revitalize.Framework.World.Objects;
using Omegasis.Revitalize.Framework.World.Objects.Farming;
using StardewValley;
using StardewValley.Objects;

namespace Omegasis.Revitalize.Framework.World.WorldUtilities
{
    public static class WorldUtility
    {

        public static void InitializeGameWorld()
        {
            CleanUpGameWorldOnLoad();
        }

        /// <summary>
        /// Cleans up an object's references on loading a game save. Necessary due to the hack that CustomObjects are considered furniture and StardewValley.Objects...
        /// </summary>
        public static void CleanUpGameWorldOnLoad()
        {
            foreach (GameLocation location in Game1.locations)
            {
                List<CustomObject> objectsToCleanUp = new List<CustomObject>();
                foreach (StardewValley.Object obj in location.objects.Values)
                {

                    if (obj is CustomObject)
                    {
                        CustomObject customObj = (CustomObject)obj;

                        if (location.objects.ContainsKey(customObj.TileLocation))
                        {
                            //RevitalizeModCore.log("Clean up from loading: {0}", true, customObj.basicItemInformation.id);
                            objectsToCleanUp.Add(customObj);
                            customObj.removeFromGameWorld(customObj.TileLocation, location);

                        }
                    }
                }
                foreach (CustomObject obj in objectsToCleanUp)
                {
                    Furniture f = GetFurnitureEquivalentPieceAtLocation(location, obj);
                    bool furnitureContains = f != null;
                    if (location.objects.ContainsKey(obj.TileLocation) == false && furnitureContains == false)
                    {

                        //RevitalizeModCore.log("Add object back to game world since it doesn't exist in object or furniture list: {0}", false, obj.basicItemInformation.id);

                        obj.reAddToGameWorld(obj.TileLocation, location);
                        continue;
                    }

                    else if (location.objects.ContainsKey(obj.TileLocation) == false && furnitureContains == true)
                    {
                        //RevitalizeModCore.log("Add object back to game world since it doesn't exist in object list but it does exist in the furniture list: {0}", false, obj.basicItemInformation.id);
                        location.furniture.Remove(f);
                        obj.reAddToGameWorld(obj.TileLocation, location);
                        continue;

                    }

                    else if (location.objects.ContainsKey(obj.TileLocation) == true && furnitureContains == false)
                    {
                        //RevitalizeModCore.log("Object was NOT removed, but DOES NOT exist in the furniture's database: {0}", true, obj.basicItemInformation.id);
                    }
                    else
                    {
                        //RevitalizeModCore.log("Unsure what should happen here", true, obj.basicItemInformation.id);
                        StardewValley.Object overlappedObject = location.objects[obj.TileLocation];
                        if (overlappedObject is CustomObject)
                        {
                            CustomObject overlappedCustomObject = (CustomObject)overlappedObject;
                            if (overlappedCustomObject.basicItemInformation.id.Equals(obj.basicItemInformation.id))
                            {

                                //RevitalizeModCore.log("Purge due to duplication: {0}", true, obj.basicItemInformation.id);
                                continue;
                            }
                            else
                            {
                                obj.reAddToGameWorld(obj.TileLocation, location);
                            }
                        }

                    }
                }
            }
        }


        public static Furniture GetFurnitureEquivalentPieceAtLocation(this GameLocation environment, CustomObject obj)
        {
            foreach (Furniture f in environment.furniture)
            {
                if (f is CustomObject)
                {

                    CustomObject customObject = (CustomObject)f;

                    if (customObject.TileLocation.Equals(obj.TileLocation) && customObject.basicItemInformation.id.Equals(obj.basicItemInformation.id))
                    {
                        return f;
                    }

                }
            }
            return null;
        }

        public static void RemoveFurnitureAtTileLocation(this GameLocation environment, Vector2 TileLocation)
        {
            List<Furniture> furnitureToRemove = new List<Furniture>();
            foreach (Furniture f in environment.furniture)
            {
                if (f is CustomObject)
                {

                    CustomObject customObject = (CustomObject)f;

                    if (customObject.TileLocation.Equals(TileLocation))
                    {
                        furnitureToRemove.Add(f);
                    }

                }
            }

            foreach(Furniture f in furnitureToRemove)
            {
                environment.furniture.Remove(f);
            }
        }

        public static void RemoveFurnitureIntersectingTileLocation(GameLocation environment, Vector2 TileLocation)
        {
            List<Furniture> furnitureToRemove = new List<Furniture>();
            foreach (Furniture f in environment.furniture)
            {
                if (f is CustomObject)
                {

                    CustomObject customObject = (CustomObject)f;

                   bool contains= customObject.boundingBox.Value.Contains(TileLocation * 64);

                    if (contains)
                    {
                        furnitureToRemove.Add(f);
                    }

                }
            }

            foreach (Furniture f in furnitureToRemove)
            {
                environment.furniture.Remove(f);
            }
        }

        /// <summary>
        /// Creates <see cref="CustomObjectDebris"/> at the given location.
        /// </summary>
        /// <param name="Location"></param>
        /// <param name="item"></param>
        /// <param name="OriginTile"></param>
        public static void CreateItemDebrisAtTileLocation(this GameLocation Location, Item item, Vector2 OriginTile)
        {
            Location.debris.Add(new CustomObjectDebris(item, OriginTile * 64));
        }

        /// <summary>
        /// Creates <see cref="CustomObjectDebris"/> at the given location.
        /// </summary>
        /// <param name="Location"></param>
        /// <param name="item"></param>
        /// <param name="OriginTile"></param>
        /// <param name="DestinationTile"></param>
        public static void CreateItemDebrisAtTileLocation(this GameLocation Location, Item item, Vector2 OriginTile, Vector2 DestinationTile)
        {
            Location.debris.Add(new CustomObjectDebris(item, OriginTile *64, DestinationTile * 64));
        }

        /// <summary>
        /// Checks to see if minecarts have been unlocked for the world.
        /// </summary>
        /// <returns></returns>
        public static bool AreMinecartsUnlocked()
        {
            if (Game1.MasterPlayer.mailReceived.Contains("ccBoilerRoom"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
