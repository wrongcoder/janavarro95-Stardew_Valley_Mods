using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xTile.Dimensions;
using xTile.ObjectModel;
using xTile.Tiles;

namespace CustomNPCFramework.Framework.NPCS
{
    class ExtendedNPC :StardewValley.NPC
    {
        public ExtendedNPC() :base()
        {

        }
        public ExtendedNPC(StardewValley.AnimatedSprite sprite,Vector2 position,int facingDirection,string name): base(sprite, position, facingDirection, name, null)
        {
           
        }


        public override void MovePosition(GameTime time, xTile.Dimensions.Rectangle viewport, GameLocation currentLocation)
        {
            if (this.GetType() == typeof(FarmAnimal))
                this.willDestroyObjectsUnderfoot = false;
            if ((double)this.xVelocity != 0.0 || (double)this.yVelocity != 0.0)
            {
                Microsoft.Xna.Framework.Rectangle boundingBox = this.GetBoundingBox();
                boundingBox.X += (int)this.xVelocity;
                boundingBox.Y -= (int)this.yVelocity;
                if (currentLocation == null || !currentLocation.isCollidingPosition(boundingBox, viewport, false, 0, false, this))
                {
                    this.position.X += this.xVelocity;
                    this.position.Y -= this.yVelocity;
                }
                this.xVelocity = (float)(int)((double)this.xVelocity - (double)this.xVelocity / 2.0);
                this.yVelocity = (float)(int)((double)this.yVelocity - (double)this.yVelocity / 2.0);
            }
            else if (this.moveUp)
            {
                if (currentLocation == null || !currentLocation.isCollidingPosition(this.nextPosition(0), viewport, false, 0, false, this) || this.isCharging)
                {
                    this.position.Y -= (float)(this.speed + this.addedSpeed);
                    if (!this.ignoreMovementAnimation)
                    {
                        this.sprite.AnimateUp(time, (this.speed - 2 + this.addedSpeed) * -25, Utility.isOnScreen(this.getTileLocationPoint(), 1, currentLocation) ? "Cowboy_Footstep" : "");
                        this.faceDirection(0);
                    }
                }
                else if (!currentLocation.isTilePassable(this.nextPosition(0), viewport) || !this.willDestroyObjectsUnderfoot)
                    this.Halt();
                else if (this.willDestroyObjectsUnderfoot)
                {
                    Vector2 vector2 = new Vector2((float)(this.getStandingX() / Game1.tileSize), (float)(this.getStandingY() / Game1.tileSize - 1));
                    if (currentLocation.characterDestroyObjectWithinRectangle(this.nextPosition(0), true))
                    {
                        this.doEmote(12, true);
                        this.position.Y -= (float)(this.speed + this.addedSpeed);
                    }
                    else
                        this.blockedInterval = this.blockedInterval + time.ElapsedGameTime.Milliseconds;
                }
            }
            else if (this.moveRight)
            {
                if (currentLocation == null || !currentLocation.isCollidingPosition(this.nextPosition(1), viewport, false, 0, false, this) || this.isCharging)
                {
                    this.position.X += (float)(this.speed + this.addedSpeed);
                    if (!this.ignoreMovementAnimation)
                    {
                        this.sprite.AnimateRight(time, (this.speed - 2 + this.addedSpeed) * -25, Utility.isOnScreen(this.getTileLocationPoint(), 1, currentLocation) ? "Cowboy_Footstep" : "");
                        this.faceDirection(1);
                    }
                }
                else if (!currentLocation.isTilePassable(this.nextPosition(1), viewport) || !this.willDestroyObjectsUnderfoot)
                    this.Halt();
                else if (this.willDestroyObjectsUnderfoot)
                {
                    Vector2 vector2 = new Vector2((float)(this.getStandingX() / Game1.tileSize + 1), (float)(this.getStandingY() / Game1.tileSize));
                    if (currentLocation.characterDestroyObjectWithinRectangle(this.nextPosition(1), true))
                    {
                        this.doEmote(12, true);
                        this.position.X += (float)(this.speed + this.addedSpeed);
                    }
                    else
                        this.blockedInterval = this.blockedInterval + time.ElapsedGameTime.Milliseconds;
                }
            }
            else if (this.moveDown)
            {
                if (currentLocation == null || !currentLocation.isCollidingPosition(this.nextPosition(2), viewport, false, 0, false, this) || this.isCharging)
                {
                    this.position.Y += (float)(this.speed + this.addedSpeed);
                    if (!this.ignoreMovementAnimation)
                    {
                        this.sprite.AnimateDown(time, (this.speed - 2 + this.addedSpeed) * -25, Utility.isOnScreen(this.getTileLocationPoint(), 1, currentLocation) ? "Cowboy_Footstep" : "");
                        this.faceDirection(2);
                    }
                }
                else if (!currentLocation.isTilePassable(this.nextPosition(2), viewport) || !this.willDestroyObjectsUnderfoot)
                    this.Halt();
                else if (this.willDestroyObjectsUnderfoot)
                {
                    Vector2 vector2 = new Vector2((float)(this.getStandingX() / Game1.tileSize), (float)(this.getStandingY() / Game1.tileSize + 1));
                    if (currentLocation.characterDestroyObjectWithinRectangle(this.nextPosition(2), true))
                    {
                        this.doEmote(12, true);
                        this.position.Y += (float)(this.speed + this.addedSpeed);
                    }
                    else
                        this.blockedInterval = this.blockedInterval + time.ElapsedGameTime.Milliseconds;
                }
            }
            else if (this.moveLeft)
            {
                if (currentLocation == null || !currentLocation.isCollidingPosition(this.nextPosition(3), viewport, false, 0, false, this) || this.isCharging)
                {
                    this.position.X -= (float)(this.speed + this.addedSpeed);
                    if (!this.ignoreMovementAnimation)
                    {
                        this.sprite.AnimateLeft(time, (this.speed - 2 + this.addedSpeed) * -25, Utility.isOnScreen(this.getTileLocationPoint(), 1, currentLocation) ? "Cowboy_Footstep" : "");
                        this.faceDirection(3);
                    }
                }
                else if (!currentLocation.isTilePassable(this.nextPosition(3), viewport) || !this.willDestroyObjectsUnderfoot)
                    this.Halt();
                else if (this.willDestroyObjectsUnderfoot)
                {
                    Vector2 vector2 = new Vector2((float)(this.getStandingX() / Game1.tileSize - 1), (float)(this.getStandingY() / Game1.tileSize));
                    if (currentLocation.characterDestroyObjectWithinRectangle(this.nextPosition(3), true))
                    {
                        this.doEmote(12, true);
                        this.position.X -= (float)(this.speed + this.addedSpeed);
                    }
                    else
                        this.blockedInterval = this.blockedInterval + time.ElapsedGameTime.Milliseconds;
                }
            }
            if (this.blockedInterval >= 3000 && (double)this.blockedInterval <= 3750.0 && !Game1.eventUp)
            {
                this.doEmote(Game1.random.NextDouble() < 0.5 ? 8 : 40, true);
                this.blockedInterval = 3750;
            }
            else
            {
                if (this.blockedInterval < 5000)
                    return;
                this.speed = 4;
                this.isCharging = true;
                this.blockedInterval = 0;
            }
        }

        public override void draw(SpriteBatch b, float alpha = 1f)
        {
            if (this.sprite == null || this.isInvisible || !Utility.isOnScreen(this.position, 2 * Game1.tileSize))
                return;
            //Checks if the npc is swimming. If not draw it's default graphic. Do characters aside from Farmer and Penny Swim???
            if (this.swimming)
            {
                b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize + Game1.tileSize / 4 + this.yJumpOffset * 2)) + (this.shakeTimer > 0 ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero) - new Vector2(0.0f, this.yOffset), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(this.sprite.SourceRect.X, this.sprite.SourceRect.Y, this.sprite.SourceRect.Width, this.sprite.SourceRect.Height / 2 - (int)((double)this.yOffset / (double)Game1.pixelZoom))), Color.White, this.rotation, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize * 3 / 2)) / 4f, Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.991f : (float)this.getStandingY() / 10000f));
                Vector2 localPosition = this.getLocalPosition(Game1.viewport);
                b.Draw(Game1.staminaRect, new Microsoft.Xna.Framework.Rectangle((int)localPosition.X + (int)this.yOffset + Game1.pixelZoom * 2, (int)localPosition.Y - 32 * Game1.pixelZoom + this.sprite.SourceRect.Height * Game1.pixelZoom + Game1.tileSize * 3 / 4 + this.yJumpOffset * 2 - (int)this.yOffset, this.sprite.SourceRect.Width * Game1.pixelZoom - (int)this.yOffset * 2 - Game1.pixelZoom * 4, Game1.pixelZoom), new Microsoft.Xna.Framework.Rectangle?(Game1.staminaRect.Bounds), Color.White * 0.75f, 0.0f, Vector2.Zero, SpriteEffects.None, (float)((double)this.getStandingY() / 10000.0 + 1.0 / 1000.0));
            }
            else
            {
                b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float)(this.sprite.spriteWidth * Game1.pixelZoom / 2), (float)(this.GetBoundingBox().Height / 2)) + (this.shakeTimer > 0 ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Microsoft.Xna.Framework.Rectangle?(this.Sprite.SourceRect), Color.White * alpha, this.rotation, new Vector2((float)(this.sprite.spriteWidth / 2), (float)((double)this.sprite.spriteHeight * 3.0 / 4.0)), Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip || this.sprite.currentAnimation != null && this.sprite.currentAnimation[this.sprite.currentAnimationIndex].flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.991f : (float)this.getStandingY() / 10000f));
            }
            //If the npc breathes then this code is ran.
            if (this.breather && this.shakeTimer <= 0 && (!this.swimming && this.sprite.CurrentFrame < 16) && !this.farmerPassesThrough)
            {
                Microsoft.Xna.Framework.Rectangle sourceRect = this.sprite.SourceRect;
                sourceRect.Y += this.sprite.spriteHeight / 2 + this.sprite.spriteHeight / 32;
                sourceRect.Height = this.sprite.spriteHeight / 4;
                sourceRect.X += this.sprite.spriteWidth / 4;
                sourceRect.Width = this.sprite.spriteWidth / 2;
                Vector2 vector2 = new Vector2((float)(this.sprite.spriteWidth * Game1.pixelZoom / 2), (float)(Game1.tileSize / 8));
                if (this.age == 2)
                {
                    sourceRect.Y += this.sprite.spriteHeight / 6 + 1;
                    sourceRect.Height /= 2;
                    vector2.Y += (float)(this.sprite.spriteHeight / 8 * Game1.pixelZoom);
                }
                else if (this.gender == 1)
                {
                    ++sourceRect.Y;
                    vector2.Y -= (float)Game1.pixelZoom;
                    sourceRect.Height /= 2;
                }
                float num = Math.Max(0.0f, (float)(Math.Ceiling(Math.Sin(Game1.currentGameTime.TotalGameTime.TotalMilliseconds / 600.0 + (double)this.DefaultPosition.X * 20.0)) / 4.0));
                b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + vector2 + (this.shakeTimer > 0 ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Microsoft.Xna.Framework.Rectangle?(sourceRect), Color.White * alpha, this.rotation, new Vector2((float)(sourceRect.Width / 2), (float)(sourceRect.Height / 2 + 1)), Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom + num, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.992f : (float)((double)this.getStandingY() / 10000.0 + 1.0 / 1000.0)));
            }

            //Checks if the npc is glowing.
            if (this.isGlowing)
                b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float)(this.sprite.spriteWidth * Game1.pixelZoom / 2), (float)(this.GetBoundingBox().Height / 2)) + (this.shakeTimer > 0 ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Microsoft.Xna.Framework.Rectangle?(this.Sprite.SourceRect), this.glowingColor * this.glowingTransparency, this.rotation, new Vector2((float)(this.sprite.spriteWidth / 2), (float)((double)this.sprite.spriteHeight * 3.0 / 4.0)), Math.Max(0.2f, this.scale) * 4f, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.99f : (float)((double)this.getStandingY() / 10000.0 + 1.0 / 1000.0)));

            //This code runs if the npc is emoting.
            if (!this.IsEmoting || Game1.eventUp)
                return;
            Vector2 localPosition1 = this.getLocalPosition(Game1.viewport);
            localPosition1.Y -= (float)(Game1.tileSize / 2 + this.sprite.spriteHeight * Game1.pixelZoom);
            b.Draw(Game1.emoteSpriteSheet, localPosition1, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(this.CurrentEmoteIndex * 16 % Game1.emoteSpriteSheet.Width, this.CurrentEmoteIndex * 16 / Game1.emoteSpriteSheet.Width * 16, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)this.getStandingY() / 10000f);
        }


        public override void updateMovement(GameLocation location, GameTime time)
        {
            this.lastPosition = this.position;
            if (this.DirectionsToNewLocation != null && !Game1.newDay)
            {
                if (this.getStandingX() < -Game1.tileSize || this.getStandingX() > location.map.DisplayWidth + Game1.tileSize || (this.getStandingY() < -Game1.tileSize || this.getStandingY() > location.map.DisplayHeight + Game1.tileSize))
                {
                    this.IsWalkingInSquare = false;
                    Game1.warpCharacter(this, this.DefaultMap, this.DefaultPosition, true, true);
                    location.characters.Remove(this);
                }
                else if (this.IsWalkingInSquare)
                {
                    this.returnToEndPoint();
                    this.MovePosition(time, Game1.viewport, location);
                }
                else
                {
                    if (!this.followSchedule)
                        return;
                    this.MovePosition(time, Game1.viewport, location);
                    Warp warp = location.isCollidingWithWarp(this.GetBoundingBox());
                    PropertyValue propertyValue = (PropertyValue)null;
                    Tile tile1 = location.map.GetLayer("Buildings").PickTile(this.nextPositionPoint(), Game1.viewport.Size);
                    if (tile1 != null)
                        tile1.Properties.TryGetValue("Action", out propertyValue);
                    string[] strArray1;
                    if (propertyValue != null)
                        strArray1 = propertyValue.ToString().Split(' ');
                    else
                        strArray1 = (string[])null;
                    string[] strArray2 = strArray1;
                    if (warp != null)
                    {
                        if (location is BusStop && warp.TargetName.Equals("Farm"))
                        {
                            Point entryLocation = ((this.isMarried() ? (GameLocation)(this.getHome() as FarmHouse) : Game1.getLocationFromName("FarmHouse")) as FarmHouse).getEntryLocation();
                            warp = new Warp(warp.X, warp.Y, "FarmHouse", entryLocation.X, entryLocation.Y, false);
                        }
                        else if (location is FarmHouse && warp.TargetName.Equals("Farm"))
                            warp = new Warp(warp.X, warp.Y, "BusStop", 0, 23, false);
                        Game1.warpCharacter(this, warp.TargetName, new Vector2((float)(warp.TargetX * Game1.tileSize), (float)(warp.TargetY * Game1.tileSize - this.Sprite.getHeight() / 2 - Game1.tileSize / 4)), false, location.IsOutdoors);
                        location.characters.Remove(this);
                    }
                    else if (strArray2 != null && strArray2.Length >= 1 && strArray2[0].Contains("Warp"))
                    {
                        Game1.warpCharacter(this, strArray2[3], new Vector2((float)Convert.ToInt32(strArray2[1]), (float)Convert.ToInt32(strArray2[2])), false, location.IsOutdoors);
                        if (Game1.currentLocation.name.Equals(location.name) && Utility.isOnScreen(this.getStandingPosition(), Game1.tileSize * 3))
                            Game1.playSound("doorClose");
                        location.characters.Remove(this);
                    }
                    else if (strArray2 != null && strArray2.Length >= 1 && strArray2[0].Contains("Door"))
                    {
                        location.openDoor(new Location(this.nextPositionPoint().X / Game1.tileSize, this.nextPositionPoint().Y / Game1.tileSize), Game1.player.currentLocation.Equals((object)location));
                    }
                    else
                    {
                        if (location.map.GetLayer("Paths") == null)
                            return;
                        Tile tile2 = location.map.GetLayer("Paths").PickTile(new Location(this.getStandingX(), this.getStandingY()), Game1.viewport.Size);
                        Microsoft.Xna.Framework.Rectangle boundingBox = this.GetBoundingBox();
                        boundingBox.Inflate(2, 2);
                        if (tile2 == null || !new Microsoft.Xna.Framework.Rectangle(this.getStandingX() - this.getStandingX() % Game1.tileSize, this.getStandingY() - this.getStandingY() % Game1.tileSize, Game1.tileSize, Game1.tileSize).Contains(boundingBox))
                            return;
                        switch (tile2.TileIndex)
                        {
                            case 0:
                                if (this.getDirection() == 3)
                                {
                                    this.SetMovingOnlyUp();
                                    break;
                                }
                                if (this.getDirection() != 2)
                                    break;
                                this.SetMovingOnlyRight();
                                break;
                            case 1:
                                if (this.getDirection() == 3)
                                {
                                    this.SetMovingOnlyDown();
                                    break;
                                }
                                if (this.getDirection() != 0)
                                    break;
                                this.SetMovingOnlyRight();
                                break;
                            case 2:
                                if (this.getDirection() == 1)
                                {
                                    this.SetMovingOnlyDown();
                                    break;
                                }
                                if (this.getDirection() != 0)
                                    break;
                                this.SetMovingOnlyLeft();
                                break;
                            case 3:
                                if (this.getDirection() == 1)
                                {
                                    this.SetMovingOnlyUp();
                                    break;
                                }
                                if (this.getDirection() != 2)
                                    break;
                                this.SetMovingOnlyLeft();
                                break;
                            case 4:
                                this.changeSchedulePathDirection();
                                this.moveCharacterOnSchedulePath();
                                break;
                            case 7:
                                this.ReachedEndPoint();
                                break;
                        }
                    }
                }
            }
            else
            {
                if (!this.IsWalkingInSquare)
                    return;
                this.randomSquareMovement(time);
                this.MovePosition(time, Game1.viewport, location);
            }
        }

    }
}
