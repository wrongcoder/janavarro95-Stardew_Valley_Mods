using AdditionalCropsFramework.Framework;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Locations;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using StardustCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xTile.Dimensions;

namespace AdditionalCropsFramework
{
    class Utilities
    {
        public static readonly string EntensionsFolderName = "Extensions"; 

        public static List<TerrainDataNode> trackedTerrainFeatures= new List<TerrainDataNode>();

        public static List<CoreObject> NonSolidThingsToDraw = new List<CoreObject>();


        public static void createObjectDebris(Item I, int xTileOrigin, int yTileOrigin, int xTileTarget, int yTileTarget, int groundLevel = -1, int itemQuality = 0, float velocityMultiplyer = 1f, GameLocation location = null)
        {
            Debris debris = new Debris(I, new Vector2(xTileOrigin, yTileOrigin), new Vector2(xTileTarget, yTileTarget))
            {
                itemQuality = itemQuality,
            };
       
            /*
            Debris debris = new Debris(objectIndex, new Vector2((float)(xTile * Game1.tileSize + Game1.tileSize / 2), (float)(yTile * Game1.tileSize + Game1.tileSize / 2)), new Vector2((float)Game1.player.getStandingX(), (float)Game1.player.getStandingY()))
            {
                itemQuality = itemQuality
            };
            */
            foreach (Chunk chunk in debris.Chunks)
            {
                double num1 = (double)chunk.xVelocity * (double)velocityMultiplyer;
                chunk.xVelocity = (float)num1;
                double num2 = (double)chunk.yVelocity * (double)velocityMultiplyer;
                chunk.yVelocity = (float)num2;
            }
            if (groundLevel != -1)
                debris.chunkFinalYLevel = groundLevel;
            (location == null ? Game1.currentLocation : location).debris.Add(debris);
        }

        /*
        public static void plantModdedCropHere(ModularSeeds seeds)
        {
            /*
            if (Lists.saplingNames.Contains(Game1.player.ActiveObject.name))
            {
                bool f = plantSappling();
                if (f == true) return;
            }
            
            //Log.AsyncC("HELLO");
            try
            {
                HoeDirt t;
                TerrainFeature r;
                bool plant = Game1.player.currentLocation.terrainFeatures.TryGetValue(Game1.currentCursorTile, out r);
                t = (r as HoeDirt);
                if (t is HoeDirt)
                {
                    if ((t as HoeDirt).crop == null)
                    {
                        //    Log.AsyncG("BOOP");
                       (t as HoeDirt).crop = new ModularCrop(seeds.parentSheetIndex, (int)Game1.currentCursorTile.X, (int)Game1.currentCursorTile.Y, seeds.cropDataFilePath, seeds.cropTextureFilePath, seeds.cropObjectTextureFilePath, seeds.cropObjectDataFilePath);
                        //Game1.player.reduceActiveItemByOne();
                        Game1.playSound("dirtyHit");
                        trackedTerrainFeatures.Add(new TerrainDataNode(Game1.player.currentLocation, (int)Game1.currentCursorTile.X, (int)Game1.currentCursorTile.Y, t));
                    }
                }
            }catch(Exception err)
            {
                Log.AsyncG("BUBBLES");
            }
        }
    
        public static void plantRegularCropHere()
        {
            HoeDirt t;
            TerrainFeature r;
            bool plant = Game1.player.currentLocation.terrainFeatures.TryGetValue(Game1.currentCursorTile, out r);
            t = (r as HoeDirt);
            if (t is HoeDirt)
            {
                if ((t as HoeDirt).crop == null)
                {
                    (t as HoeDirt).crop = new Crop(Game1.player.ActiveObject.parentSheetIndex, (int)Game1.currentCursorTile.X, (int)Game1.currentCursorTile.Y);
                    Game1.player.reduceActiveItemByOne();
                    Game1.playSound("dirtyHit");
                   trackedTerrainFeatures.Add(new TerrainDataNode(Game1.player.currentLocation, (int)Game1.currentCursorTile.X, (int)Game1.currentCursorTile.Y,t));
                }
            }
        }
        */


        public static bool placementAction(CoreObject cObj, GameLocation location, int x, int y, StardewValley.Farmer who = null, bool playSound = true)
        {
            Vector2 vector = new Vector2((float)(x / Game1.tileSize), (float)(y / Game1.tileSize));
            //  cObj.health = 10;
            if (who != null)
            {
                cObj.owner = who.uniqueMultiplayerID;
            }
            else
            {
                cObj.owner = Game1.player.uniqueMultiplayerID;
            }

            if (!cObj.bigCraftable && !(cObj is Furniture))
            {
                int num = cObj.ParentSheetIndex;
                if (num <= 298)
                {
                    if (num > 94)
                    {
                        bool result;
                        switch (num)
                        {
                            case 286:
                                {
                                    using (List<TemporaryAnimatedSprite>.Enumerator enumerator = Game1.currentLocation.temporarySprites.GetEnumerator())
                                    {
                                        while (enumerator.MoveNext())
                                        {
                                            if (enumerator.Current.position.Equals(vector * (float)Game1.tileSize))
                                            {
                                                result = false;
                                                return result;
                                            }
                                        }
                                    }
                                    int num2 = Game1.random.Next();
                                    Game1.playSound("thudStep");
                                    Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(cObj.parentSheetIndex, 100f, 1, 24, vector * (float)Game1.tileSize, true, false, Game1.currentLocation, who)
                                    {
                                        shakeIntensity = 0.5f,
                                        shakeIntensityChange = 0.002f,
                                        extraInfoForEndBehavior = num2,
                                        endFunction = new TemporaryAnimatedSprite.endBehavior(Game1.currentLocation.removeTemporarySpritesWithID)
                                    });
                                    Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(598, 1279, 3, 4), 53f, 5, 9, vector * (float)Game1.tileSize + new Vector2(5f, 3f) * (float)Game1.pixelZoom, true, false, (float)(y + 7) / 10000f, 0f, Color.Yellow, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
                                    {
                                        id = (float)num2
                                    });
                                    Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(598, 1279, 3, 4), 53f, 5, 9, vector * (float)Game1.tileSize + new Vector2(5f, 3f) * (float)Game1.pixelZoom, true, true, (float)(y + 7) / 10000f, 0f, Color.Orange, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
                                    {
                                        delayBeforeAnimationStart = 100,
                                        id = (float)num2
                                    });
                                    Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(598, 1279, 3, 4), 53f, 5, 9, vector * (float)Game1.tileSize + new Vector2(5f, 3f) * (float)Game1.pixelZoom, true, false, (float)(y + 7) / 10000f, 0f, Color.White, (float)Game1.pixelZoom * 0.75f, 0f, 0f, 0f, false)
                                    {
                                        delayBeforeAnimationStart = 200,
                                        id = (float)num2
                                    });
                                    if (Game1.fuseSound != null && !Game1.fuseSound.IsPlaying)
                                    {
                                        Game1.fuseSound = Game1.soundBank.GetCue("fuse");
                                        Game1.fuseSound.Play();
                                    }
                                    return true;
                                }
                            case 287:
                                {
                                    using (List<TemporaryAnimatedSprite>.Enumerator enumerator = Game1.currentLocation.temporarySprites.GetEnumerator())
                                    {
                                        while (enumerator.MoveNext())
                                        {
                                            if (enumerator.Current.position.Equals(vector * (float)Game1.tileSize))
                                            {
                                                result = false;
                                                return result;
                                            }
                                        }
                                    }
                                    int num2 = Game1.random.Next();
                                    Game1.playSound("thudStep");
                                    Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(cObj.parentSheetIndex, 100f, 1, 24, vector * (float)Game1.tileSize, true, false, Game1.currentLocation, who)
                                    {
                                        shakeIntensity = 0.5f,
                                        shakeIntensityChange = 0.002f,
                                        extraInfoForEndBehavior = num2,
                                        endFunction = new TemporaryAnimatedSprite.endBehavior(Game1.currentLocation.removeTemporarySpritesWithID)
                                    });
                                    Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(598, 1279, 3, 4), 53f, 5, 9, vector * (float)Game1.tileSize, true, false, (float)(y + 7) / 10000f, 0f, Color.Yellow, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
                                    {
                                        id = (float)num2
                                    });
                                    Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(598, 1279, 3, 4), 53f, 5, 9, vector * (float)Game1.tileSize, true, false, (float)(y + 7) / 10000f, 0f, Color.Orange, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
                                    {
                                        delayBeforeAnimationStart = 100,
                                        id = (float)num2
                                    });
                                    Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(598, 1279, 3, 4), 53f, 5, 9, vector * (float)Game1.tileSize, true, false, (float)(y + 7) / 10000f, 0f, Color.White, (float)Game1.pixelZoom * 0.75f, 0f, 0f, 0f, false)
                                    {
                                        delayBeforeAnimationStart = 200,
                                        id = (float)num2
                                    });
                                    if (Game1.fuseSound != null && !Game1.fuseSound.IsPlaying)
                                    {
                                        Game1.fuseSound = Game1.soundBank.GetCue("fuse");
                                        Game1.fuseSound.Play();
                                    }
                                    return true;
                                }
                            case 288:
                                {
                                    using (List<TemporaryAnimatedSprite>.Enumerator enumerator = Game1.currentLocation.temporarySprites.GetEnumerator())
                                    {
                                        while (enumerator.MoveNext())
                                        {
                                            if (enumerator.Current.position.Equals(vector * (float)Game1.tileSize))
                                            {
                                                result = false;
                                                return result;
                                            }
                                        }
                                    }
                                    int num2 = Game1.random.Next();
                                    Game1.playSound("thudStep");
                                    Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(cObj.parentSheetIndex, 100f, 1, 24, vector * (float)Game1.tileSize, true, false, Game1.currentLocation, who)
                                    {
                                        shakeIntensity = 0.5f,
                                        shakeIntensityChange = 0.002f,
                                        extraInfoForEndBehavior = num2,
                                        endFunction = new TemporaryAnimatedSprite.endBehavior(Game1.currentLocation.removeTemporarySpritesWithID)
                                    });
                                    Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(598, 1279, 3, 4), 53f, 5, 9, vector * (float)Game1.tileSize + new Vector2(5f, 0f) * (float)Game1.pixelZoom, true, false, (float)(y + 7) / 10000f, 0f, Color.Yellow, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
                                    {
                                        id = (float)num2
                                    });
                                    Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(598, 1279, 3, 4), 53f, 5, 9, vector * (float)Game1.tileSize + new Vector2(5f, 0f) * (float)Game1.pixelZoom, true, true, (float)(y + 7) / 10000f, 0f, Color.Orange, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
                                    {
                                        delayBeforeAnimationStart = 100,
                                        id = (float)num2
                                    });
                                    Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(598, 1279, 3, 4), 53f, 5, 9, vector * (float)Game1.tileSize + new Vector2(5f, 0f) * (float)Game1.pixelZoom, true, false, (float)(y + 7) / 10000f, 0f, Color.White, (float)Game1.pixelZoom * 0.75f, 0f, 0f, 0f, false)
                                    {
                                        delayBeforeAnimationStart = 200,
                                        id = (float)num2
                                    });
                                    if (Game1.fuseSound != null && !Game1.fuseSound.IsPlaying)
                                    {
                                        Game1.fuseSound = Game1.soundBank.GetCue("fuse");
                                        Game1.fuseSound.Play();
                                    }
                                    return true;
                                }
                            default:
                                if (num != 297)
                                {
                                    if (num != 298)
                                    {
                                        goto IL_FD7;
                                    }
                                    if (location.objects.ContainsKey(vector))
                                    {
                                        return false;
                                    }
                                    location.objects.Add(vector, new Fence(vector, 5, false));
                                    Game1.playSound("axe");
                                    return true;
                                }
                                else
                                {
                                    if (location.objects.ContainsKey(vector) || location.terrainFeatures.ContainsKey(vector))
                                    {
                                        return false;
                                    }
                                    location.terrainFeatures.Add(vector, new Grass(1, 4));
                                    Game1.playSound("dirtyHit");
                                    return true;
                                }
                                break;
                        }
                        return result;
                    }
                    if (num != 93)
                    {
                        if (num == 94)
                        {
                            if (location.objects.ContainsKey(vector))
                            {
                                return false;
                            }
                            new Torch(vector, 1, 94).placementAction(location, x, y, who);
                            return true;
                        }
                    }
                    else
                    {
                        if (location.objects.ContainsKey(vector))
                        {
                            return false;
                        }
                        Utility.removeLightSource((int)(cObj.tileLocation.X * 2000f + cObj.tileLocation.Y));
                        Utility.removeLightSource((int)Game1.player.uniqueMultiplayerID);
                        new Torch(vector, 1).placementAction(location, x, y, (who == null) ? Game1.player : who);
                        return true;
                    }
                }
                else if (num <= 401)
                {
                    switch (num)
                    {
                        case 309:
                        case 310:
                        case 311:
                            {
                                bool flag = location.terrainFeatures.ContainsKey(vector) && location.terrainFeatures[vector] is HoeDirt && (location.terrainFeatures[vector] as HoeDirt).crop == null;
                                if (!flag && (location.objects.ContainsKey(vector) || location.terrainFeatures.ContainsKey(vector) || (!(location is Farm) && !location.name.Contains("Greenhouse"))))
                                {
                                    Game1.showRedMessage("Invalid Position");
                                    return false;
                                }
                                string text = location.doesTileHaveProperty(x, y, "NoSpawn", "Back");
                                if ((text == null || (!text.Equals("Tree") && !text.Equals("All"))) && (flag || (location.isTileLocationOpen(new Location(x * Game1.tileSize, y * Game1.tileSize)) && !location.isTileOccupied(new Vector2((float)x, (float)y), "") && location.doesTileHaveProperty(x, y, "Water", "Back") == null)))
                                {
                                    int which = 1;
                                    num = cObj.parentSheetIndex;
                                    if (num != 310)
                                    {
                                        if (num == 311)
                                        {
                                            which = 3;
                                        }
                                    }
                                    else
                                    {
                                        which = 2;
                                    }
                                    location.terrainFeatures.Remove(vector);
                                    location.terrainFeatures.Add(vector, new Tree(which, 0));
                                    Game1.playSound("dirtyHit");
                                    return true;
                                }
                                break;
                            }
                        default:
                            switch (num)
                            {
                                case 322:
                                    if (location.objects.ContainsKey(vector))
                                    {
                                        return false;
                                    }
                                    location.objects.Add(vector, new Fence(vector, 1, false));
                                    Game1.playSound("axe");
                                    return true;
                                case 323:
                                    if (location.objects.ContainsKey(vector))
                                    {
                                        return false;
                                    }
                                    location.objects.Add(vector, new Fence(vector, 2, false));
                                    Game1.playSound("stoneStep");
                                    return true;
                                case 324:
                                    if (location.objects.ContainsKey(vector))
                                    {
                                        return false;
                                    }
                                    location.objects.Add(vector, new Fence(vector, 3, false));
                                    Game1.playSound("hammer");
                                    return true;
                                case 325:
                                    if (location.objects.ContainsKey(vector))
                                    {
                                        return false;
                                    }
                                    location.objects.Add(vector, new Fence(vector, 4, true));
                                    Game1.playSound("axe");
                                    return true;
                                case 326:
                                case 327:
                                case 330:
                                case 332:
                                    break;
                                case 328:
                                    if (location.terrainFeatures.ContainsKey(vector))
                                    {
                                        return false;
                                    }
                                    location.terrainFeatures.Add(vector, new Flooring(0));
                                    Game1.playSound("axchop");
                                    return true;
                                case 329:
                                    if (location.terrainFeatures.ContainsKey(vector))
                                    {
                                        return false;
                                    }
                                    location.terrainFeatures.Add(vector, new Flooring(1));
                                    Game1.playSound("thudStep");
                                    return true;
                                case 331:
                                    if (location.terrainFeatures.ContainsKey(vector))
                                    {
                                        return false;
                                    }
                                    location.terrainFeatures.Add(vector, new Flooring(2));
                                    Game1.playSound("axchop");
                                    return true;
                                case 333:
                                    if (location.terrainFeatures.ContainsKey(vector))
                                    {
                                        return false;
                                    }
                                    location.terrainFeatures.Add(vector, new Flooring(3));
                                    Game1.playSound("thudStep");
                                    return true;
                                default:
                                    if (num == 401)
                                    {
                                        if (location.terrainFeatures.ContainsKey(vector))
                                        {
                                            return false;
                                        }
                                        location.terrainFeatures.Add(vector, new Flooring(4));
                                        Game1.playSound("thudStep");
                                        return true;
                                    }
                                    break;
                            }
                            break;
                    }
                }
                else
                {
                    switch (num)
                    {
                        case 405:
                            if (location.terrainFeatures.ContainsKey(vector))
                            {
                                return false;
                            }
                            location.terrainFeatures.Add(vector, new Flooring(6));
                            Game1.playSound("woodyStep");
                            return true;
                        case 406:
                        case 408:
                        case 410:
                            break;
                        case 407:
                            if (location.terrainFeatures.ContainsKey(vector))
                            {
                                return false;
                            }
                            location.terrainFeatures.Add(vector, new Flooring(5));
                            Game1.playSound("dirtyHit");
                            return true;
                        case 409:
                            if (location.terrainFeatures.ContainsKey(vector))
                            {
                                return false;
                            }
                            location.terrainFeatures.Add(vector, new Flooring(7));
                            Game1.playSound("stoneStep");
                            return true;
                        case 411:
                            if (location.terrainFeatures.ContainsKey(vector))
                            {
                                return false;
                            }
                            location.terrainFeatures.Add(vector, new Flooring(8));
                            Game1.playSound("stoneStep");
                            return true;
                        default:
                            if (num != 415)
                            {
                                if (num == 710)
                                {
                                    if (location.objects.ContainsKey(vector) || location.doesTileHaveProperty((int)vector.X, (int)vector.Y, "Water", "Back") == null)
                                    {
                                        return false;
                                    }
                                    new CrabPot(vector, 1).placementAction(location, x, y, who);
                                    return true;
                                }
                            }
                            else
                            {
                                if (location.terrainFeatures.ContainsKey(vector))
                                {
                                    return false;
                                }
                                location.terrainFeatures.Add(vector, new Flooring(9));
                                Game1.playSound("stoneStep");
                                return true;
                            }
                            break;
                    }
                }
            }
            else
            {
                int num = cObj.ParentSheetIndex;
                if (num <= 130)
                {
                    if (num == 71)
                    {
                        if (location is MineShaft)
                        {
                            if ((location as MineShaft).mineLevel != 120 && (location as MineShaft).recursiveTryToCreateLadderDown(vector, "hoeHit", 16))
                            {
                                return true;
                            }
                            Game1.showRedMessage("Unsuitable Location");
                        }
                        return false;
                    }
                    if (num == 130)
                    {
                        if (location.objects.ContainsKey(vector) || Game1.currentLocation is MineShaft)
                        {
                            Game1.showRedMessage("Unsuitable Location");
                            return false;
                        }
                        location.objects.Add(vector, new Chest(true)
                        {
                            shakeTimer = 50
                        });
                        Game1.playSound("axe");
                        return true;
                    }
                }
                else
                {
                    switch (num)
                    {
                        case 143:
                        case 144:
                        case 145:
                        case 146:
                        case 147:
                        case 148:
                        case 149:
                        case 150:
                        case 151:
                            if (location.objects.ContainsKey(vector))
                            {
                                return false;
                            }
                            new Torch(vector, cObj.parentSheetIndex, true)
                            {
                                shakeTimer = 25
                            }.placementAction(location, x, y, who);
                            return true;
                        default:
                            if (num == 163)
                            {
                                location.objects.Add(vector, new Cask(vector));
                                Game1.playSound("hammer");
                            }
                            break;
                    }
                }
            }
            IL_FD7:
            if (cObj.name.Equals("Tapper"))
            {
                if (location.terrainFeatures.ContainsKey(vector) && location.terrainFeatures[vector] is Tree && (location.terrainFeatures[vector] as Tree).growthStage >= 5 && !(location.terrainFeatures[vector] as Tree).stump && !location.objects.ContainsKey(vector))
                {
                    cObj.tileLocation = vector;
                    location.objects.Add(vector, cObj);
                    int treeType = (location.terrainFeatures[vector] as Tree).treeType;
                    (location.terrainFeatures[vector] as Tree).tapped = true;
                    switch (treeType)
                    {
                        case 1:
                            cObj.heldObject = new StardewValley.Object(725, 1, false, -1, 0);
                            cObj.minutesUntilReady = 13000 - Game1.timeOfDay;
                            break;
                        case 2:
                            cObj.heldObject = new StardewValley.Object(724, 1, false, -1, 0);
                            cObj.minutesUntilReady = 16000 - Game1.timeOfDay;
                            break;
                        case 3:
                            cObj.heldObject = new StardewValley.Object(726, 1, false, -1, 0);
                            cObj.minutesUntilReady = 10000 - Game1.timeOfDay;
                            break;
                        case 7:
                            cObj.heldObject = new StardewValley.Object(420, 1, false, -1, 0);
                            cObj.minutesUntilReady = 3000 - Game1.timeOfDay;
                            if (!Game1.currentSeason.Equals("fall"))
                            {
                                cObj.heldObject = new StardewValley.Object(404, 1, false, -1, 0);
                                cObj.minutesUntilReady = 6000 - Game1.timeOfDay;
                            }
                            break;
                    }
                    Game1.playSound("axe");
                    return true;
                }
                return false;
            }
            else if (cObj.name.Contains("Sapling"))
            {
                Vector2 key = default(Vector2);
                for (int i = x / Game1.tileSize - 2; i <= x / Game1.tileSize + 2; i++)
                {
                    for (int j = y / Game1.tileSize - 2; j <= y / Game1.tileSize + 2; j++)
                    {
                        key.X = (float)i;
                        key.Y = (float)j;
                        if (location.terrainFeatures.ContainsKey(key) && (location.terrainFeatures[key] is Tree || location.terrainFeatures[key] is FruitTree))
                        {
                            Game1.showRedMessage("Too close to another tree");
                            return false;
                        }
                    }
                }
                if (location.terrainFeatures.ContainsKey(vector))
                {
                    if (!(location.terrainFeatures[vector] is HoeDirt) || (location.terrainFeatures[vector] as HoeDirt).crop != null)
                    {
                        return false;
                    }
                    location.terrainFeatures.Remove(vector);
                }
                if (location is Farm && (location.doesTileHaveProperty((int)vector.X, (int)vector.Y, "Diggable", "Back") != null || location.doesTileHavePropertyNoNull((int)vector.X, (int)vector.Y, "Type", "Back").Equals("Grass")))
                {
                    Game1.playSound("dirtyHit");
                    DelayedAction.playSoundAfterDelay("coin", 100);
                    location.terrainFeatures.Add(vector, new FruitTree(cObj.parentSheetIndex));
                    return true;
                }
                Game1.showRedMessage("Can't be planted here.");
                return false;
            }
            else
            {

                //Game1.showRedMessage("STEP 1");

                if (cObj.category == -74)
                {
                    return true;
                }
                if (!cObj.performDropDownAction(who))
                {
                    CoreObject @object = (CoreObject)cObj.getOne();
                    @object.shakeTimer = 50;
                    @object.tileLocation = vector;
                    @object.performDropDownAction(who);
                    if (location.objects.ContainsKey(vector))
                    {
                        if (location.objects[vector].ParentSheetIndex != cObj.parentSheetIndex)
                        {
                            Game1.createItemDebris(location.objects[vector], vector * (float)Game1.tileSize, Game1.random.Next(4));
                            location.objects[vector] = @object;
                        }
                    }

                    else
                    {
                        //   Game1.showRedMessage("STEP 2");
                       // Log.Info(vector);

                        Vector2 newVec = new Vector2(vector.X, vector.Y);
                        // cObj.boundingBox.Inflate(32, 32);
                        location.objects.Add(newVec, cObj);
                    }
                    @object.initializeLightSource(vector);
                }
                if (playSound == true) Game1.playSound("woodyStep");
                else
                {
                  //  Log.AsyncG("restoring item from file");
                }
                //Log.AsyncM("Placed and object");
                cObj.locationsName = location.name;
                StardustCore.ModCore.SerializationManager.trackedObjectList.Add(cObj);
                return true;

            }
        }



        public static bool addItemToInventoryAndCleanTrackedList(CoreObject I)
        {
            if (Game1.player.isInventoryFull() == false)
            {
                Game1.player.addItemToInventoryBool(I, false);
                StardustCore.ModCore.SerializationManager.trackedObjectList.Remove(I);
                return true;
            }
            else
            {
                Random random = new Random(129);
                int i = random.Next();
                i = i % 4;
                Vector2 v2 = new Vector2(Game1.player.getTileX() * Game1.tileSize, Game1.player.getTileY() * Game1.tileSize);
                Game1.createItemDebris(I, v2, i);
                return false;
            }
        }


        public static Microsoft.Xna.Framework.Rectangle parseRectFromJson(string s)
        {



            s = s.Replace('{', ' ');
            s = s.Replace('}', ' ');
            s = s.Replace('^', ' ');
            s = s.Replace(':', ' ');
            string[] parsed = s.Split(' ');
            foreach (var v in parsed)
            {
                //Log.AsyncY(v);
            }
            return new Microsoft.Xna.Framework.Rectangle(Convert.ToInt32(parsed[2]), Convert.ToInt32(parsed[4]), Convert.ToInt32(parsed[6]), Convert.ToInt32(parsed[8]));
        }

       public static bool isCropFullGrown(Crop c)
        {

            if (c.currentPhase >= c.phaseDays.Count - 1)
            {
               c.currentPhase = c.phaseDays.Count - 1;
               c.dayOfCurrentPhase = 0;
                return true;
            }
            return false;
    }

        public static void cropNewDay(PlanterBox p,Crop c,int state, int fertilizer, int xTile, int yTile, GameLocation environment)
        {
            if (p.greenHouseEffect == false)
            {
                if ((c.dead || !c.seasonsToGrowIn.Contains(Game1.currentSeason)))
                {
                    c.dead = true;
                }
            }


                if (state == 1)
                {
                    c.dayOfCurrentPhase++;
                  //  Log.AsyncG("DaY OF CURRRENT PHASE BISCUITS!"+c.dayOfCurrentPhase);

                   // Log.AsyncC(c.currentPhase);
                    if (c.dayOfCurrentPhase >= c.phaseDays[c.currentPhase])
                    {
                        c.currentPhase++;
                        c.dayOfCurrentPhase = 0;
                    }

                    //c.dayOfCurrentPhase = c.fullyGrown ? c.dayOfCurrentPhase - 1 : Math.Min(c.dayOfCurrentPhase + 1, c.phaseDays.Count > 0 ? c.phaseDays[Math.Min(c.phaseDays.Count - 1, c.currentPhase)] : 0);
                    if (c.dayOfCurrentPhase >= (c.phaseDays.Count > 0 ? c.phaseDays[Math.Min(c.phaseDays.Count - 1, c.currentPhase)] : 0) && c.currentPhase < c.phaseDays.Count - 1)
                    {
                        c.currentPhase = c.currentPhase + 1;
                        c.dayOfCurrentPhase = 0;
                    }

                    while (c.currentPhase < c.phaseDays.Count - 1 && c.phaseDays.Count > 0 && c.phaseDays[c.currentPhase] <= 0)
                        c.currentPhase = c.currentPhase + 1;
                    if (c.rowInSpriteSheet == 23 && c.phaseToShow == -1 && c.currentPhase > 0)
                        c.phaseToShow = Game1.random.Next(1, 7);
                    if (c.currentPhase == c.phaseDays.Count - 1 && (c.indexOfHarvest == 276 || c.indexOfHarvest == 190 || c.indexOfHarvest == 254) && new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed + xTile * 2000 + yTile).NextDouble() < 0.01)
                    {
                        for (int index1 = xTile - 1; index1 <= xTile + 1; ++index1)
                        {
                            for (int index2 = yTile - 1; index2 <= yTile + 1; ++index2)
                            {
                                Vector2 key = new Vector2((float)index1, (float)index2);
                                if (!environment.terrainFeatures.ContainsKey(key) || !(environment.terrainFeatures[key] is HoeDirt) || ((environment.terrainFeatures[key] as HoeDirt).crop == null || (environment.terrainFeatures[key] as HoeDirt).crop.indexOfHarvest != c.indexOfHarvest))
                                    return;
                            }
                        }
                        for (int index1 = xTile - 1; index1 <= xTile + 1; ++index1)
                        {
                            for (int index2 = yTile - 1; index2 <= yTile + 1; ++index2)
                            {
                                Vector2 index3 = new Vector2((float)index1, (float)index2);
                                (environment.terrainFeatures[index3] as HoeDirt).crop = (Crop)null;
                            }
                        }
                     // (environment as Farm).resourceClumps.Add((ResourceClump)new GiantCrop(c.indexOfHarvest, new Vector2((float)(xTile - 1), (float)(yTile - 1))));
                    }
                }
                if (c.fullyGrown && c.dayOfCurrentPhase > 0 || (c.currentPhase < c.phaseDays.Count - 1 || c.rowInSpriteSheet != 23))
                    return;
                Vector2 index = new Vector2((float)xTile, (float)yTile);
                environment.objects.Remove(index);
                string season = Game1.currentSeason;
                switch (c.whichForageCrop)
                {
                    case 495:
                        season = "spring";
                        break;
                    case 496:
                        season = "summer";
                        break;
                    case 497:
                        season = "fall";
                        break;
                    case 498:
                        season = "winter";
                        break;
                }
                environment.objects.Add(index, new StardewValley.Object(index, c.getRandomWildCropForSeason(season), 1)
                {
                    isSpawnedObject = true,
                    canBeGrabbed = true
                });
                if (environment.terrainFeatures[index] == null || !(environment.terrainFeatures[index] is HoeDirt))
                    return;
                (environment.terrainFeatures[index] as HoeDirt).crop = (Crop)null;
            
        }


       

        public static void cropNewDayModded(PlanterBox p,ModularCrop c, int state, int fertilizer, int xTile, int yTile, GameLocation environment)
        {
            if (p.greenHouseEffect == false)
            {
                if ((c.dead || !c.seasonsToGrowIn.Contains(Game1.currentSeason)))
                {
                    c.dead = true;
                }
            }

                if (state == 1)
                {
                    c.dayOfCurrentPhase++;



                    //c.dayOfCurrentPhase = c.fullyGrown ? c.dayOfCurrentPhase - 1 : Math.Min(c.dayOfCurrentPhase + 1, c.phaseDays.Count > 0 ? c.phaseDays[Math.Min(c.phaseDays.Count - 1, c.currentPhase)] : 0);
                    if (c.dayOfCurrentPhase >= (c.phaseDays.Count > 0 ? c.phaseDays[Math.Min(c.phaseDays.Count - 1, c.currentPhase)] : 0) && c.currentPhase < c.phaseDays.Count - 1)
                    {
                        c.currentPhase = c.currentPhase + 1;
                        c.dayOfCurrentPhase = 0;
                    }




                    while (c.currentPhase < c.phaseDays.Count - 1 && c.phaseDays.Count > 0 && c.phaseDays[c.currentPhase] <= 0)
                        c.currentPhase = c.currentPhase + 1;
                    if (c.rowInSpriteSheet == 23 && c.phaseToShow == -1 && c.currentPhase > 0)
                        c.phaseToShow = Game1.random.Next(1, 7);
                    if (c.currentPhase == c.phaseDays.Count - 1 && (c.indexOfHarvest == 276 || c.indexOfHarvest == 190 || c.indexOfHarvest == 254) && new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed + xTile * 2000 + yTile).NextDouble() < 0.01)
                    {
                        for (int index1 = xTile - 1; index1 <= xTile + 1; ++index1)
                        {
                            for (int index2 = yTile - 1; index2 <= yTile + 1; ++index2)
                            {
                                Vector2 key = new Vector2((float)index1, (float)index2);
                                if (!environment.terrainFeatures.ContainsKey(key) || !(environment.terrainFeatures[key] is HoeDirt) || ((environment.terrainFeatures[key] as HoeDirt).crop == null || (environment.terrainFeatures[key] as HoeDirt).crop.indexOfHarvest != c.indexOfHarvest))
                                    return;
                            }
                        }
                        for (int index1 = xTile - 1; index1 <= xTile + 1; ++index1)
                        {
                            for (int index2 = yTile - 1; index2 <= yTile + 1; ++index2)
                            {
                                Vector2 index3 = new Vector2((float)index1, (float)index2);
                                (environment.terrainFeatures[index3] as HoeDirt).crop = (Crop)null;
                            }
                        }
                        // (environment as Farm).resourceClumps.Add((ResourceClump)new GiantCrop(c.indexOfHarvest, new Vector2((float)(xTile - 1), (float)(yTile - 1))));
                    }
                }
                if (c.fullyGrown && c.dayOfCurrentPhase > 0 || (c.currentPhase < c.phaseDays.Count - 1 || c.rowInSpriteSheet != 23))
                    return;
                Vector2 index = new Vector2((float)xTile, (float)yTile);
                environment.objects.Remove(index);
                string season = Game1.currentSeason;
                switch (c.whichForageCrop)
                {
                    case 495:
                        season = "spring";
                        break;
                    case 496:
                        season = "summer";
                        break;
                    case 497:
                        season = "fall";
                        break;
                    case 498:
                        season = "winter";
                        break;
                }



                if (environment.terrainFeatures[index] == null || !(environment.terrainFeatures[index] is HoeDirt))
                    return;
                (environment.terrainFeatures[index] as HoeDirt).crop = (Crop)null;
            
        }


        public static bool harvestCrop(Crop c,int xTile, int yTile, int fertilizer, JunimoHarvester junimoHarvester = null)
        {
            Item I = (Item)new StardewValley.Object(c.indexOfHarvest, 1);

            int howMuch = 3;
            if (Game1.player.addItemToInventoryBool(I, false))
            {
                Vector2 vector2 = new Vector2((float)xTile, (float)yTile);

                if (Game1.player.CurrentItem == null)
                {
                    // Game1.player.animateOnce(279 + Game1.player.facingDirection);
                   // StardustCore.Utilities.animateOnce(Game1.player, 279 + Game1.player.facingDirection, 10f, 6, null, false, false, false);
                }
                Game1.player.canMove = false;
                Game1.playSound("harvest");
                DelayedAction.playSoundAfterDelay("coin", 260);
                if (c.regrowAfterHarvest == -1)
                {
                    Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(17, new Vector2(vector2.X * (float)Game1.tileSize, vector2.Y * (float)Game1.tileSize), Color.White, 7, Game1.random.NextDouble() < 0.5, 125f, 0, -1, -1f, -1, 0));
                    Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(14, new Vector2(vector2.X * (float)Game1.tileSize, vector2.Y * (float)Game1.tileSize), Color.White, 7, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0));
                }
                Game1.player.gainExperience(2, howMuch);
                return true;
            }

            return false;


            if (c.dead)
                return junimoHarvester != null;
            if (c.forageCrop)
            {
                StardewValley.Object @object = (StardewValley.Object)null;
               // int howMuch = 3;
                if (c.whichForageCrop == 1)
                    @object = new StardewValley.Object(399, 1, false, -1, 0);
                if (Game1.player.professions.Contains(16))
                    @object.quality = 4;
                else if (Game1.random.NextDouble() < (double)Game1.player.ForagingLevel / 30.0)
                    @object.quality = 2;
                else if (Game1.random.NextDouble() < (double)Game1.player.ForagingLevel / 15.0)
                    @object.quality = 1;
                Game1.stats.ItemsForaged += (uint)@object.Stack;
                if (junimoHarvester != null)
                {
                    junimoHarvester.tryToAddItemToHut((Item)@object);
                    return true;
                }
                if (Game1.player.addItemToInventoryBool((Item)@object, false))
                {
                    Vector2 vector2 = new Vector2((float)xTile, (float)yTile);
                    Game1.player.animateOnce(279 + Game1.player.facingDirection);
                   // Game1.player.canMove = false;
                    Game1.playSound("harvest");
                    DelayedAction.playSoundAfterDelay("coin", 260);
                    if (c.regrowAfterHarvest == -1)
                    {
                        Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(17, new Vector2(vector2.X * (float)Game1.tileSize, vector2.Y * (float)Game1.tileSize), Color.White, 7, Game1.random.NextDouble() < 0.5, 125f, 0, -1, -1f, -1, 0));
                        Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(14, new Vector2(vector2.X * (float)Game1.tileSize, vector2.Y * (float)Game1.tileSize), Color.White, 7, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0));
                    }
                    Game1.player.gainExperience(2, howMuch);
                    return true;
                }
                Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Crop.cs.588"));
            }
            else if (c.currentPhase >= c.phaseDays.Count - 1 && (!c.fullyGrown || c.dayOfCurrentPhase <= 0))
            {
                int num1 = 1;
                int num2 = 0;
                int num3 = 0;
                if (c.indexOfHarvest == 0)
                    return true;
                Random random = new Random(xTile * 7 + yTile * 11 + (int)Game1.stats.DaysPlayed + (int)Game1.uniqueIDForThisGame);
                switch (fertilizer)
                {
                    case 368:
                        num3 = 1;
                        break;
                    case 369:
                        num3 = 2;
                        break;
                }
                double num4 = 0.2 * ((double)Game1.player.FarmingLevel / 10.0) + 0.2 * (double)num3 * (((double)Game1.player.FarmingLevel + 2.0) / 12.0) + 0.01;
                double num5 = Math.Min(0.75, num4 * 2.0);
                if (random.NextDouble() < num4)
                    num2 = 2;
                else if (random.NextDouble() < num5)
                    num2 = 1;
                if (c.minHarvest > 1 || c.maxHarvest > 1)
                    num1 = random.Next(c.minHarvest, Math.Min(c.minHarvest + 1, c.maxHarvest + 1 + Game1.player.FarmingLevel / c.maxHarvestIncreasePerFarmingLevel));
                if (c.chanceForExtraCrops > 0.0)
                {
                    while (random.NextDouble() < Math.Min(0.9, c.chanceForExtraCrops))
                        ++num1;
                }
                if (c.harvestMethod == 1)
                {
                    if (junimoHarvester == null)
                        DelayedAction.playSoundAfterDelay("daggerswipe", 150);
                    if (junimoHarvester != null && Utility.isOnScreen(junimoHarvester.getTileLocationPoint(), Game1.tileSize, junimoHarvester.currentLocation))
                        Game1.playSound("harvest");
                    if (junimoHarvester != null && Utility.isOnScreen(junimoHarvester.getTileLocationPoint(), Game1.tileSize, junimoHarvester.currentLocation))
                        DelayedAction.playSoundAfterDelay("coin", 260);
                    for (int index = 0; index < num1; ++index)
                    {
                        if (junimoHarvester != null)
                            junimoHarvester.tryToAddItemToHut((Item)new StardewValley.Object(c.indexOfHarvest, 1, false, -1, num2));
                        else
                            Game1.createObjectDebris(c.indexOfHarvest, xTile, yTile, -1, num2, 1f, (GameLocation)null);
                    }
                    if (c.regrowAfterHarvest == -1)
                        return true;
                    c.dayOfCurrentPhase = c.regrowAfterHarvest;
                    c.fullyGrown = true;
                }
                else
                {
                    if (junimoHarvester == null)
                    {
                        StardewValley.Farmer player = Game1.player;
                        StardewValley.Object @object;
                        if (!c.programColored)
                        {
                            @object = new StardewValley.Object(c.indexOfHarvest, 1, false, -1, num2);
                        }
                        else
                        {
                            @object = (StardewValley.Object)new ColoredObject(c.indexOfHarvest, 1, c.tintColor);
                            int num6 = num2;
                            @object.quality = num6;
                        }
                        int num7 = 0;
                        if (!player.addItemToInventoryBool((Item)@object, num7 != 0))
                        {
                            Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Crop.cs.588"));
                            goto label_86;
                        }
                    }
                    Vector2 vector2 = new Vector2((float)xTile, (float)yTile);
                    if (junimoHarvester == null)
                    {
                        Game1.player.animateOnce(279 + Game1.player.facingDirection);
                       // Game1.player.canMove = false;
                    }
                    else
                    {
                        JunimoHarvester junimoHarvester1 = junimoHarvester;
                        StardewValley.Object @object;
                        if (!c.programColored)
                        {
                            @object = new StardewValley.Object(c.indexOfHarvest, 1, false, -1, num2);
                        }
                        else
                        {
                            @object = (StardewValley.Object)new ColoredObject(c.indexOfHarvest, 1, c.tintColor);
                            int num6 = num2;
                            @object.quality = num6;
                        }
                        junimoHarvester1.tryToAddItemToHut((Item)@object);
                    }
                    if (random.NextDouble() < (double)Game1.player.LuckLevel / 1500.0 + Game1.dailyLuck / 1200.0 + 9.99999974737875E-05)
                    {
                        num1 *= 2;
                        if (junimoHarvester == null || Utility.isOnScreen(junimoHarvester.getTileLocationPoint(), Game1.tileSize, junimoHarvester.currentLocation))
                            Game1.playSound("dwoop");
                    }
                    else if (c.harvestMethod == 0)
                    {
                        if (junimoHarvester == null || Utility.isOnScreen(junimoHarvester.getTileLocationPoint(), Game1.tileSize, junimoHarvester.currentLocation))
                            Game1.playSound("harvest");
                        if (junimoHarvester == null || Utility.isOnScreen(junimoHarvester.getTileLocationPoint(), Game1.tileSize, junimoHarvester.currentLocation))
                            DelayedAction.playSoundAfterDelay("coin", 260);
                        if (c.regrowAfterHarvest == -1 && (junimoHarvester == null || junimoHarvester.currentLocation.Equals((object)Game1.currentLocation)))
                        {
                            Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(17, new Vector2(vector2.X * (float)Game1.tileSize, vector2.Y * (float)Game1.tileSize), Color.White, 7, Game1.random.NextDouble() < 0.5, 125f, 0, -1, -1f, -1, 0));
                            Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(14, new Vector2(vector2.X * (float)Game1.tileSize, vector2.Y * (float)Game1.tileSize), Color.White, 7, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0));
                        }
                    }
                    if (c.indexOfHarvest == 421)
                    {
                        c.indexOfHarvest = 431;
                        num1 = random.Next(1, 4);
                    }
                    for (int index = 0; index < num1 - 1; ++index)
                    {
                        if (junimoHarvester == null)
                            Game1.createObjectDebris(c.indexOfHarvest, xTile, yTile, -1, 0, 1f, (GameLocation)null);
                        else
                            junimoHarvester.tryToAddItemToHut((Item)new StardewValley.Object(c.indexOfHarvest, 1, false, -1, 0));
                    }
                    float num8 = (float)(16.0 * Math.Log(0.018 * (double)Convert.ToInt32(Game1.objectInformation[c.indexOfHarvest].Split('/')[1]) + 1.0, Math.E));
                    if (junimoHarvester == null)
                        Game1.player.gainExperience(0, (int)Math.Round((double)num8));
                    if (c.regrowAfterHarvest == -1)
                        return true;
                    c.dayOfCurrentPhase = c.regrowAfterHarvest;
                    c.fullyGrown = true;
                }
            }
            label_86:
            return false;
        }

        public static bool harvestModularCrop(ModularCrop c, int xTile, int yTile, int fertilizer, JunimoHarvester junimoHarvester = null)
        {
            Item I = (Item)new ModularCropObject(c.indexOfHarvest, 1, c.cropObjectTexture, c.cropObjectData);
            int howMuch = 3;
            if (Game1.player.addItemToInventoryBool(I, false))
            {
                Vector2 vector2 = new Vector2((float)xTile, (float)yTile);
                if (Game1.player.CurrentItem == null)
                {
                    // Game1.player.animateOnce(279 + Game1.player.facingDirection);
                  //  StardustCore.Utilities.animateOnce(Game1.player, 279 + Game1.player.facingDirection, 10f, 6, null, false, false, false);
                }
                Game1.player.canMove = false;
                Game1.playSound("harvest");
                DelayedAction.playSoundAfterDelay("coin", 260);
                if (c.regrowAfterHarvest == -1)
                {
                    Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(17, new Vector2(vector2.X * (float)Game1.tileSize, vector2.Y * (float)Game1.tileSize), Color.White, 7, Game1.random.NextDouble() < 0.5, 125f, 0, -1, -1f, -1, 0));
                    Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(14, new Vector2(vector2.X * (float)Game1.tileSize, vector2.Y * (float)Game1.tileSize), Color.White, 7, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0));

                }
                Game1.player.gainExperience(2, howMuch);
                return true;
            }
            return false;

            Game1.player.addItemToInventoryBool(I);

            return true;

            if (c.dead)
                return junimoHarvester != null;
            if (c.forageCrop)
            {
                ModularCropObject @object = (ModularCropObject)null;
                //int howMuch = 3;
                if (c.whichForageCrop == 1)
                   // @object = new StardewValley.Object(399, 1, false, -1, 0);
                if (Game1.player.professions.Contains(16))
                    @object.quality = 4;
                else if (Game1.random.NextDouble() < (double)Game1.player.ForagingLevel / 30.0)
                    @object.quality = 2;
                else if (Game1.random.NextDouble() < (double)Game1.player.ForagingLevel / 15.0)
                    @object.quality = 1;
                Game1.stats.ItemsForaged += (uint)@object.Stack;
                if (junimoHarvester != null)
                {
                    junimoHarvester.tryToAddItemToHut((Item)@object);
                    return true;
                }
                if (Game1.player.addItemToInventoryBool((Item)@object, false))
                {
                    Vector2 vector2 = new Vector2((float)xTile, (float)yTile);
                    Game1.player.animateOnce(279 + Game1.player.facingDirection);
                    //Game1.player.canMove = false;
                    Game1.playSound("harvest");
                    DelayedAction.playSoundAfterDelay("coin", 260);
                    if (c.regrowAfterHarvest == -1)
                    {
                        Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(17, new Vector2(vector2.X * (float)Game1.tileSize, vector2.Y * (float)Game1.tileSize), Color.White, 7, Game1.random.NextDouble() < 0.5, 125f, 0, -1, -1f, -1, 0));
                        Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(14, new Vector2(vector2.X * (float)Game1.tileSize, vector2.Y * (float)Game1.tileSize), Color.White, 7, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0));
                    }
                    Game1.player.gainExperience(2, howMuch);
                    return true;
                }
                Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Crop.cs.588"));
            }
            else if (c.currentPhase >= c.phaseDays.Count - 1 && (!c.fullyGrown || c.dayOfCurrentPhase <= 0))
            {
                int num1 = 1;
                int num2 = 0;
                int num3 = 0;
                if (c.indexOfHarvest == 0)
                    return true;
                Random random = new Random(xTile * 7 + yTile * 11 + (int)Game1.stats.DaysPlayed + (int)Game1.uniqueIDForThisGame);
                switch (fertilizer)
                {
                    case 368:
                        num3 = 1;
                        break;
                    case 369:
                        num3 = 2;
                        break;
                }
                double num4 = 0.2 * ((double)Game1.player.FarmingLevel / 10.0) + 0.2 * (double)num3 * (((double)Game1.player.FarmingLevel + 2.0) / 12.0) + 0.01;
                double num5 = Math.Min(0.75, num4 * 2.0);
                if (random.NextDouble() < num4)
                    num2 = 2;
                else if (random.NextDouble() < num5)
                    num2 = 1;
                if (c.minHarvest > 1 || c.maxHarvest > 1)
                    num1 = random.Next(c.minHarvest, Math.Min(c.minHarvest + 1, c.maxHarvest + 1 + Game1.player.FarmingLevel / c.maxHarvestIncreasePerFarmingLevel));
                if (c.chanceForExtraCrops > 0.0)
                {
                    while (random.NextDouble() < Math.Min(0.9, c.chanceForExtraCrops))
                        ++num1;
                }
                if (c.harvestMethod == 1)
                {
                    if (junimoHarvester == null)
                        DelayedAction.playSoundAfterDelay("daggerswipe", 150);
                    if (junimoHarvester != null && Utility.isOnScreen(junimoHarvester.getTileLocationPoint(), Game1.tileSize, junimoHarvester.currentLocation))
                        Game1.playSound("harvest");
                    if (junimoHarvester != null && Utility.isOnScreen(junimoHarvester.getTileLocationPoint(), Game1.tileSize, junimoHarvester.currentLocation))
                        DelayedAction.playSoundAfterDelay("coin", 260);
                    for (int index = 0; index < num1; ++index)
                    {
                        if (junimoHarvester != null)
                            junimoHarvester.tryToAddItemToHut((Item)new ModularCropObject(c.indexOfHarvest, 1, c.cropObjectTexture,c.cropObjectData));
                        else
                          Utilities.createObjectDebris((Item)new ModularCropObject(c.indexOfHarvest,1,c.cropObjectTexture,c.cropObjectData), xTile, yTile, xTile,yTile,-1, num2, 1f, (GameLocation)null);
                    }
                    if (c.regrowAfterHarvest == -1)
                        return true;
                    c.dayOfCurrentPhase = c.regrowAfterHarvest;
                    c.fullyGrown = true;
                }
                else
                {
                    if (junimoHarvester == null)
                    {
                        StardewValley.Farmer player = Game1.player;
                        ModularCropObject @object;
                        if (!c.programColored)
                        {
                           // Log.AsyncG(c.indexOfHarvest);
                            try
                            {
                                @object = new ModularCropObject(c.indexOfHarvest, 1, c.cropObjectTexture, c.cropObjectData);
                            }
                            catch(Exception lol)
                            {
                                @object = new ModularCropObject();
                              //  Log.AsyncO(lol);
                            }
                        }
                        else
                        {
                            @object = new ModularCropObject(c.indexOfHarvest, 1, c.cropObjectTexture, c.cropObjectData);
                            int num6 = num2;
                            @object.quality = num6;
                        }
                        int num7 = 0;
                        if (!player.addItemToInventoryBool((Item)@object, num7 != 0))
                        {
                            Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Crop.cs.588"));
                            goto label_86;
                        }
                    }
                    Vector2 vector2 = new Vector2((float)xTile, (float)yTile);
                    if (junimoHarvester == null)
                    {
                        Game1.player.animateOnce(279 + Game1.player.facingDirection);
                        //Game1.player.canMove = false;
                    }
                    else
                    {
                        JunimoHarvester junimoHarvester1 = junimoHarvester;
                       ModularCropObject @object;
                        if (!c.programColored)
                        {
                            @object = new ModularCropObject(c.indexOfHarvest, 1, c.cropObjectTexture, c.cropObjectData);
                        }
                        else
                        {
                            @object = new ModularCropObject(c.indexOfHarvest, 1, c.cropObjectTexture, c.cropObjectData);
                            int num6 = num2;
                            @object.quality = num6;
                        }
                        junimoHarvester1.tryToAddItemToHut((Item)@object);
                    }
                    if (random.NextDouble() < (double)Game1.player.LuckLevel / 1500.0 + Game1.dailyLuck / 1200.0 + 9.99999974737875E-05)
                    {
                        num1 *= 2;
                        if (junimoHarvester == null || Utility.isOnScreen(junimoHarvester.getTileLocationPoint(), Game1.tileSize, junimoHarvester.currentLocation))
                            Game1.playSound("dwoop");
                    }
                    else if (c.harvestMethod == 0)
                    {
                        if (junimoHarvester == null || Utility.isOnScreen(junimoHarvester.getTileLocationPoint(), Game1.tileSize, junimoHarvester.currentLocation))
                            Game1.playSound("harvest");
                        if (junimoHarvester == null || Utility.isOnScreen(junimoHarvester.getTileLocationPoint(), Game1.tileSize, junimoHarvester.currentLocation))
                            DelayedAction.playSoundAfterDelay("coin", 260);
                        if (c.regrowAfterHarvest == -1 && (junimoHarvester == null || junimoHarvester.currentLocation.Equals((object)Game1.currentLocation)))
                        {
                            Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(17, new Vector2(vector2.X * (float)Game1.tileSize, vector2.Y * (float)Game1.tileSize), Color.White, 7, Game1.random.NextDouble() < 0.5, 125f, 0, -1, -1f, -1, 0));
                            Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(14, new Vector2(vector2.X * (float)Game1.tileSize, vector2.Y * (float)Game1.tileSize), Color.White, 7, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0));
                        }
                    }
                    if (c.indexOfHarvest == 421)
                    {
                        c.indexOfHarvest = 431;
                        num1 = random.Next(1, 4);
                    }
                    for (int index = 0; index < num1 - 1; ++index)
                    {
                        if (junimoHarvester == null)
                            Utilities.createObjectDebris((Item)new ModularCropObject(c.indexOfHarvest, 1, c.cropObjectTexture, c.cropObjectData), xTile, yTile, xTile, yTile, -1, num2, 1f, (GameLocation)null);
                        else
                            junimoHarvester.tryToAddItemToHut((Item) new ModularCropObject(c.indexOfHarvest, 1, c.cropObjectTexture, c.cropObjectData));
                    }
                    float num8 = (float)(16.0 * Math.Log(0.018 * (double)Convert.ToInt32(Game1.objectInformation[c.indexOfHarvest].Split('/')[1]) + 1.0, Math.E));
                    if (junimoHarvester == null)
                        Game1.player.gainExperience(0, (int)Math.Round((double)num8));
                    if (c.regrowAfterHarvest == -1)
                        return true;
                    c.dayOfCurrentPhase = c.regrowAfterHarvest;
                    c.fullyGrown = true;
                }
            }
            label_86:
            return false;
        }
    }
}
