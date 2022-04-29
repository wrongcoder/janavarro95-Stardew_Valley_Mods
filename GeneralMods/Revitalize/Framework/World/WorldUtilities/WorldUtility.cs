using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Omegasis.Revitalize.Framework.Constants;
using Omegasis.Revitalize.Framework.Constants.ItemIds.Objects;
using Omegasis.Revitalize.Framework.World.Objects;
using Omegasis.Revitalize.Framework.World.Objects.Machines;
using StardewValley;
using StardewValley.Objects;

namespace Omegasis.Revitalize.Framework.World.WorldUtilities
{
    public class WorldUtility
    {

        public static void InitializeGameWorld()
        {
            AddModdedMachinesToGameWorld();

            foreach(GameLocation location in Game1.locations)
            {
                List<CustomObject> objectsToCleanUp = new List<CustomObject>();
                foreach(StardewValley.Object obj in location.objects.Values)
                {

                    if (obj is CustomObject)
                    {
                        CustomObject customObj = (CustomObject)obj;

                        RevitalizeModCore.log("Clean up from loading: {0}", true, customObj.basicItemInformation.id);

                        if (location.objects.ContainsKey(customObj.TileLocation))
                        {
                            objectsToCleanUp.Add(customObj);
                            customObj.removeFromGameWorld(customObj.TileLocation, location);

                        }
                    }
                }
                foreach(CustomObject obj in objectsToCleanUp)
                {
                    Furniture f = GetFurnitureEquivalentPieceAtLocation(location, obj);
                    bool furnitureContains = f != null;
                    if (location.objects.ContainsKey(obj.TileLocation) == false && furnitureContains==false)
                    {
                        obj.reAddToGameWorld(obj.TileLocation, location);
                        return;
                    }

                    else if(location.objects.ContainsKey(obj.TileLocation)==false && furnitureContains)
                    {
                        location.furniture.Remove(f);
                        obj.reAddToGameWorld(obj.TileLocation, location);
                        return;

                    }

                    else if (location.objects.ContainsKey(obj.TileLocation) == true && furnitureContains==false)
                    {
                        RevitalizeModCore.log("Object was NOT removed, but DOES NOT exist in the furniture's database: {0}", true, obj.basicItemInformation.id);
                    }

                    else
                    {

                        StardewValley.Object overlappedObject = location.objects[obj.TileLocation];
                        if(overlappedObject is CustomObject)
                        {
                            CustomObject overlappedCustomObject = (CustomObject)overlappedObject;
                            if (overlappedCustomObject.basicItemInformation.id.Equals(obj.basicItemInformation.id))
                            {

                                RevitalizeModCore.log("Purge due to duplication: {0}", true, obj.basicItemInformation.id);
                                return; 
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


        public static Furniture GetFurnitureEquivalentPieceAtLocation(GameLocation environment, CustomObject obj)
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

        public static void RemoveFurnitureAtTileLocation(GameLocation environment, Vector2 TileLocation)
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
        /// Adds various machines and stuff to the game world.
        /// </summary>
        private static void AddModdedMachinesToGameWorld()
        {
            GameLocation cinderSapForestLocation = GameLocationUtilities.GetGameLocation(Enums.StardewLocation.Forest);
            HayMaker hayMaker = (RevitalizeModCore.ObjectManager.getObject<HayMaker>(Machines.HayMaker, 1).getOne(true) as HayMaker);
            if (RevitalizeModCore.Configs.shopsConfigManager.hayMakerShopConfig.IsHayMakerShopSetUpOutsideOfMarniesRanch &&
                cinderSapForestLocation.isObjectAtTile((int)RevitalizeModCore.Configs.shopsConfigManager.hayMakerShopConfig.HayMakerTileLocation.X, (int)RevitalizeModCore.Configs.shopsConfigManager.hayMakerShopConfig.HayMakerTileLocation.Y) == false)
            {
                hayMaker.placementActionAtTile(cinderSapForestLocation, (int)RevitalizeModCore.Configs.shopsConfigManager.hayMakerShopConfig.HayMakerTileLocation.X, (int)RevitalizeModCore.Configs.shopsConfigManager.hayMakerShopConfig.HayMakerTileLocation.Y);
            }

        }

    }
}
