
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Locations;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using StardustCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xTile.Dimensions;

namespace StardustCore
{
    

    public class Utilities
    {

        public static List<CoreObject> masterRemovalList = new List<CoreObject>();


        public static int sellToStorePrice(CoreObject c)
        {
            return  (int)((double)c.price * (1.0 + (double)c.quality * 0.25));
        }





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



        
        public static bool placementAction(CoreObject cObj, GameLocation location, int x, int y,Serialization.SerializationManager s, StardewValley.Farmer who = null, bool playSound = true)
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
                        //ModCore.ModMonitor.Log(vector.ToString());

                        Vector2 newVec = new Vector2(vector.X, vector.Y);
                        // cObj.boundingBox.Inflate(32, 32);
                        location.objects.Add(newVec, cObj);
                    }
                    @object.initializeLightSource(vector);
                }
                if (playSound == true) Game1.playSound("woodyStep");
                else
                {
                    ModCore.ModMonitor.Log("restoring item from file");
                }
                //Log.AsyncM("Placed and object");
                cObj.locationsName = location.name;
                s.trackedObjectList.Add(cObj);
                return true;

            }
        }
        

        
        public static bool addItemToInventoryAndCleanTrackedList(CoreObject I,Serialization.SerializationManager s)
        {
            if (Game1.player.isInventoryFull() == false)
            {
                Game1.player.addItemToInventoryBool(I, false);
                s.trackedObjectList.Remove(I);
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


        public static bool addItemToOtherInventory(List<Item> inventory, Item I)
        {
            if (I == null) return false;
            if (isInventoryFull(inventory) == false)
            {
                if (inventory == null)
                {
                    return false;
                }
                if (inventory.Count == 0)
                {
                    inventory.Add(I);
                    return true;
                }
                for (int i = 0; i < inventory.Capacity; i++)
                {
                    //   Log.AsyncC("OK????");

                    foreach (var v in inventory)
                    {

                        if (inventory.Count == 0)
                        {
                            addItemToOtherInventory(inventory, I);
                            return true;
                        }
                        if (v == null) continue;
                        if (v.canStackWith(I))
                        {
                            v.addToStack(I.getStack());
                            return true;
                        }
                    }
                }

                inventory.Add(I);
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool isInventoryFull(List<Item> inventory, bool logInfo = false)
        {
            if (logInfo)
            {
                ModCore.ModMonitor.Log("size " + inventory.Count);
                ModCore.ModMonitor.Log("max " + inventory.Capacity);
            }

            if (inventory.Count == inventory.Capacity) return true;
            else return false;
        }

        public static bool isWithinRange(int tileLength,Vector2 positionToCheck)
        {
            Vector2 v = Game1.player.getTileLocation();
            if (v.X < positionToCheck.X - tileLength || v.X > positionToCheck.X + tileLength) return false;
            if (v.Y < positionToCheck.Y - tileLength || v.Y > positionToCheck.Y + tileLength) return false;

            return true;
        }

        public static bool isWithinDirectionRange(int direction,int range, Vector2 positionToCheck)
        {
            Vector2 v = Game1.player.getTileLocation();
            if (direction==3 && (v.X >= positionToCheck.X - range)) return true; //face left
            if (direction==1 && (v.X <= positionToCheck.X + range)) return true; //face right
            if (direction==0 && (v.Y <= positionToCheck.Y + range)) return true; //face up
            if (direction==2 && (v.Y >= positionToCheck.Y - range)) return true; //face down

            return true;
        }




        public static void drawGreenPlus()
        {
            try
            {
                Game1.spriteBatch.Draw(Game1.mouseCursors, new Vector2(Game1.getMouseX() + 34, Game1.getMouseY() + 34), new Microsoft.Xna.Framework.Rectangle(0, 410, 17, 17), Color.White, 0, new Vector2(0, 0), 2f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
            }
            catch(Exception e)
            {

            }
        }


        public static StardewValley.Object checkRadiusForObject(int radius, string name)
        {
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    bool f = Game1.player.currentLocation.isObjectAt((Game1.player.getTileX() + x) * Game1.tileSize, (Game1.player.getTileY() + y) * Game1.tileSize);
                    if (f == false) continue;
                    StardewValley.Object obj = Game1.player.currentLocation.getObjectAt((Game1.player.getTileX() + x) * Game1.tileSize, (Game1.player.getTileY() + y) * Game1.tileSize);
                    if (obj == null) continue;
                    if (obj.name == name)
                    {
                        return obj;
                    }
                }
            }
            return null;
        }

        public static StardewValley.Object checkCardinalForObject(string name)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == -1 && y == -1) continue; //upper left
                    if (x == -1 && y == 1) continue; //bottom left
                    if (x == 1 && y == -1) continue; //upper right
                    if (x == 1 && y == 1) continue; //bottom right
                    bool f = Game1.player.currentLocation.isObjectAt((Game1.player.getTileX() + x) * Game1.tileSize, (Game1.player.getTileY() + y) * Game1.tileSize);
                    if (f == false) continue;
                    StardewValley.Object obj = Game1.player.currentLocation.getObjectAt((Game1.player.getTileX() + x) * Game1.tileSize, (Game1.player.getTileY() + y) * Game1.tileSize);
                    if (obj == null) continue;
                    if (obj.name == name)
                    {
                        return obj;
                    }
                }
            }
            return null;
        }

        public static void faceDirectionTowardsSomething(Vector2 tileLocation)
        {

            if (tileLocation.X < Game1.player.getTileX())
            {
                Game1.player.faceDirection(3);
            }
            else if (tileLocation.X > Game1.player.getTileX())
            {
                Game1.player.faceDirection(1);
            }
            else if (tileLocation.Y < Game1.player.getTileY())
            {
                Game1.player.faceDirection(0);
            }
            else if (tileLocation.Y > Game1.player.getTileY())
            {
                Game1.player.faceDirection(2);
            }
        }

        public static bool doesLocationContainObject(GameLocation location, string name)
        {
            foreach (var v in location.objects)
            {
                if (name == v.Value.name) return true;
            }
            return false;
        }


        public static KeyValuePair<Vector2,TerrainFeature> checkRadiusForTerrainFeature(int radius, Type terrainType)
        {
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    Vector2 pos = new Vector2((Game1.player.getTileX() + x), (Game1.player.getTileY() + y));
                    bool f = Game1.player.currentLocation.isTerrainFeatureAt((int)pos.X,(int)pos.Y);
                    if (f == false) continue;
                    TerrainFeature t = Game1.player.currentLocation.terrainFeatures[pos];  //((Game1.player.getTileX() + x) * Game1.tileSize, (Game1.player.getTileY() + y) * Game1.tileSize);
                    if (t == null) continue;
                    if (t.GetType() == terrainType)
                    {
                        return new KeyValuePair<Vector2, TerrainFeature> (pos,t);
                    }
                }
            }
            return new KeyValuePair<Vector2, TerrainFeature>(new Vector2(),null);
        }

        public static KeyValuePair<Vector2, TerrainFeature> checkCardinalForTerrainFeature(Type terrainType)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == -1 && y == -1) continue; //upper left
                    if (x == -1 && y == 1) continue; //bottom left
                    if (x == 1 && y == -1) continue; //upper right
                    if (x == 1 && y == 1) continue; //bottom right
                    Vector2 pos = new Vector2((Game1.player.getTileX() + x), (Game1.player.getTileY() + y));
                    bool f = Game1.player.currentLocation.isTerrainFeatureAt((int)pos.X, (int)pos.Y);
                    if (f == false) continue;
                    TerrainFeature t = Game1.player.currentLocation.terrainFeatures[pos];  //((Game1.player.getTileX() + x) * Game1.tileSize, (Game1.player.getTileY() + y) * Game1.tileSize);
                    if (t == null) continue;
                    if (t.GetType() == terrainType)
                    {
                        return new KeyValuePair<Vector2, TerrainFeature>(pos, t);
                    }
                }
            }
            return new KeyValuePair<Vector2, TerrainFeature>(new Vector2(), null);
        }



        public static bool doesLocationContainTerrainFeature(GameLocation location, Type terrain)
        {
            foreach (var v in location.terrainFeatures)
            {
                if (terrain == v.Value.GetType()) return true;
            }
            return false;
        }

        public static Item getItemFromInventory(int index)
        {
            foreach(var v in Game1.player.items)
            {
                if (v.parentSheetIndex == index) return v;
            }
            return null;
        }
        public static Item getItemFromInventory(string name)
        {
            foreach (var v in Game1.player.items)
            {
                if (v.Name == name) return v;
            }
            return null;
        }
    }
}
