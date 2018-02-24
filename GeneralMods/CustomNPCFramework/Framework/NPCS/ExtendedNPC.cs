using CustomNPCFramework.Framework.ModularNPCS;
using CustomNPCFramework.Framework.ModularNPCS.ModularRenderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Characters;
using StardewValley.Locations;
using StardewValley.Menus;
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
    public class ExtendedNPC :StardewValley.NPC
    {
        public BasicRenderer characterRenderer;
        public bool hasBeenKissedToday;
        public Point previousEndPoint;
        public bool returningToEndPoint;
        public bool hasSaidAfternoonDialogue;
        public int timeAfterSquare;

        public Sprite spriteInformation;

        public Portrait portraitInformation;
        public ExtendedNPC() :base()
        {
        }
        public ExtendedNPC(Sprite sprite,BasicRenderer renderer,Vector2 position,int facingDirection,string name): base(sprite.sprite, position, facingDirection, name, null)
        {
            this.characterRenderer = renderer;
            this.Portrait = (Texture2D)null;
            this.portraitInformation = null;
            this.spriteInformation = sprite;
            this.spriteInformation.setCharacterSpriteFromThis(this);
            this.swimming = false;
        }

        public ExtendedNPC(Sprite sprite,BasicRenderer renderer,Portrait portrait, Vector2 position, int facingDirection, string name) : base(sprite.sprite, position, facingDirection, name, null)
        {
            this.characterRenderer = renderer;
            this.portraitInformation = portrait;
            this.portraitInformation.setCharacterPortraitFromThis(this);
            this.spriteInformation = sprite;
            this.spriteInformation.setCharacterSpriteFromThis(this);
            this.swimming = false;
        }

        //ERROR NEED FIXING
        public override void reloadSprite()
        {
            if (this.characterRenderer == null)
            {
                this.spriteInformation.reload();
                try
                {
                    this.portraitInformation.reload();
                }
                catch (Exception ex)
                {
                    this.Portrait = (Texture2D)null;
                }
            }
            else
            {
                this.characterRenderer.reloadSprites();
                try
                {
                    this.portraitInformation.reload();
                }
                catch (Exception ex)
                {
                    this.Portrait = (Texture2D)null;
                }
            }
            int num = this.isInvisible ? 1 : 0;
            if (!Game1.newDay && (int)Game1.gameMode != 6)
                return;
            this.faceDirection(this.DefaultFacingDirection);
            this.scheduleTimeToTry = 9999999;
            this.previousEndPoint = new Point((int)this.DefaultPosition.X / Game1.tileSize, (int)this.DefaultPosition.Y / Game1.tileSize);
            this.Schedule = this.getSchedule(Game1.dayOfMonth);
            this.faceDirection(this.defaultFacingDirection);
            this.sprite.standAndFaceDirection(this.defaultFacingDirection);
            this.loadSeasonalDialogue();
            this.updateDialogue();
            if (this.isMarried())
                this.marriageDuties();
            bool flag = Utility.isFestivalDay(Game1.dayOfMonth, Game1.currentSeason);
            try
            {
                this.displayName = this.name;
            }
            catch (Exception ex)
            {
            }

        }

        public override bool checkAction(StardewValley.Farmer who, GameLocation l)
        {
            return false;
        }

        //ERROR NEED FIXING
        public override void MovePosition(GameTime time, xTile.Dimensions.Rectangle viewport, GameLocation currentLocation)
        {
            base.MovePosition(time,viewport,currentLocation);
            return;
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

        //ERROR NEED FIXING
        public override void update(GameTime time, GameLocation location)
        {
            base.update(time, location);        
        }

        public virtual void routeEndAnimationFinished(StardewValley.Farmer who)
        {
            this.doingEndOfRouteAnimation = false;
            this.freezeMotion = false;
            this.sprite.spriteHeight = 32;
            this.sprite.StopAnimation();
            this.endOfRouteMessage = (string)null;
            this.isCharging = false;
            this.speed = 2;
            this.addedSpeed = 0;
            this.goingToDoEndOfRouteAnimation = false;
            if (!this.IsWalkingInSquare)
                return;
            this.returningToEndPoint = true;
            this.timeAfterSquare = Game1.timeOfDay;
        }

        public virtual void doAnimationAtEndOfScheduleRoute(Character c, GameLocation l)
        {
        }


        public virtual void startRouteBehavior(string behaviorName)
        {
            if (behaviorName.Length > 0 && (int)behaviorName[0] == 34)
            {
                this.endOfRouteMessage = behaviorName.Replace("\"", "");
            }
            else
            {
                if (behaviorName.Contains("square_"))
                {
                    this.lastCrossroad = new Microsoft.Xna.Framework.Rectangle(this.getTileX() * Game1.tileSize, this.getTileY() * Game1.tileSize, Game1.tileSize, Game1.tileSize);
                    string[] strArray = behaviorName.Split('_');
                    this.walkInSquare(Convert.ToInt32(strArray[1]), Convert.ToInt32(strArray[2]), 6000);
                    this.squareMovementFacingPreference = strArray.Length <= 3 ? -1 : Convert.ToInt32(strArray[3]);
                }
                else
                {
                    Utility.getGameLocationOfCharacter(this).temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(167, 1714, 19, 14), 100f, 3, 999999, new Vector2(2f, 3f) * (float)Game1.tileSize + new Vector2(7f, 12f) * (float)Game1.pixelZoom, false, false, 0.0002f, 0.0f, Color.White, (float)Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                    {
                        id = 688f
                    });
                    this.doEmote(52, true);
                }
            }
        }


        public new void getHitByPlayer(StardewValley.Farmer who, GameLocation location)
        {
            this.doEmote(12, true);
            if (who == null)
            {
                if (Game1.IsMultiplayer)
                    return;
                who = Game1.player;
            }
            if (who.friendships.ContainsKey(this.name))
            {
                who.friendships[this.name][0] -= 30;
                if (who.IsMainPlayer)
                {
                    this.CurrentDialogue.Clear();
                    //this.CurrentDialogue.Push(new StardewValley.Dialogue(Game1.random.NextDouble() < 0.5 ? Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4293") : Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4294"), this));
                }
                //location.debris.Add(new Debris(this.sprite.Texture, Game1.random.Next(3, 8), new Vector2((float)this.GetBoundingBox().Center.X, (float)this.GetBoundingBox().Center.Y)));
            }
            if (this.name.Equals("Bouncer"))
                Game1.playSound("crafting");
            else
                Game1.playSound("hitEnemy");
        }

        //ERROR NEED FIXING
        public override void dayUpdate(int dayOfMonth)
        {
            if (this.currentLocation != null)
                Game1.warpCharacter(this, this.defaultMap, this.DefaultPosition / (float)Game1.tileSize, true, false);
            Game1.player.mailReceived.Remove(this.name);
            Game1.player.mailReceived.Remove(this.name + "Cooking");
            this.doingEndOfRouteAnimation = false;
            this.Halt();
            this.hasBeenKissedToday = false;
            this.faceTowardFarmer = false;
            this.faceTowardFarmerTimer = 0;
            this.drawOffset = Vector2.Zero;
            this.hasSaidAfternoonDialogue = false;
            this.ignoreScheduleToday = false;
            this.Halt();
            this.controller = (PathFindController)null;
            this.temporaryController = (PathFindController)null;
            this.DirectionsToNewLocation = (SchedulePathDescription)null;
            this.faceDirection(this.DefaultFacingDirection);
            this.scheduleTimeToTry = 9999999;
            this.previousEndPoint = new Point((int)this.DefaultPosition.X / Game1.tileSize, (int)this.DefaultPosition.Y / Game1.tileSize);
            this.IsWalkingInSquare = false;
            this.returningToEndPoint = false;
            this.lastCrossroad = Microsoft.Xna.Framework.Rectangle.Empty;
            if (this.isVillager())
                this.Schedule = this.getSchedule(dayOfMonth);
            this.endOfRouteMessage = (string)null;
            bool flag = Utility.isFestivalDay(dayOfMonth, Game1.currentSeason);
            if (!this.isMarried())
                return;
            this.marriageDuties();
            this.daysMarried = this.daysMarried + 1;
        }

        /// <summary>
        /// Does effectively nothing.
        /// </summary>
        public new void setUpForOutdoorPatioActivity()
        {
        }

        /// <summary>
        /// Used to draw the npc with the custom renderer.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="alpha"></param>
        public virtual void drawModular(SpriteBatch b, float alpha = 1f)
        {
            if (this.characterRenderer == null || this.isInvisible || !Utility.isOnScreen(this.position, 2 * Game1.tileSize))
                return;
            //Checks if the npc is swimming. If not draw it's default graphic. Do characters aside from Farmer and Penny Swim???
            if (this.swimming)
            {
                this.characterRenderer.setAnimation(AnimationKeys.swimmingKey);
                this.characterRenderer.setDirection(this.facingDirection);
                this.characterRenderer.draw(b,this,this.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize + Game1.tileSize / 4 + this.yJumpOffset * 2)) + (this.shakeTimer > 0 ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero) - new Vector2(0.0f, this.yOffset), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(this.sprite.SourceRect.X, this.sprite.SourceRect.Y, this.sprite.SourceRect.Width, this.sprite.SourceRect.Height / 2 - (int)((double)this.yOffset / (double)Game1.pixelZoom))), Color.White, this.rotation, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize * 3 / 2)) / 4f, Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.991f : (float)this.getStandingY() / 10000f));
                //Vector2 localPosition = this.getLocalPosition(Game1.viewport);
                //b.Draw(Game1.staminaRect, new Microsoft.Xna.Framework.Rectangle((int)localPosition.X + (int)this.yOffset + Game1.pixelZoom * 2, (int)localPosition.Y - 32 * Game1.pixelZoom + this.sprite.SourceRect.Height * Game1.pixelZoom + Game1.tileSize * 3 / 4 + this.yJumpOffset * 2 - (int)this.yOffset, this.sprite.SourceRect.Width * Game1.pixelZoom - (int)this.yOffset * 2 - Game1.pixelZoom * 4, Game1.pixelZoom), new Microsoft.Xna.Framework.Rectangle?(Game1.staminaRect.Bounds), Color.White * 0.75f, 0.0f, Vector2.Zero, SpriteEffects.None, (float)((double)this.getStandingY() / 10000.0 + 1.0 / 1000.0));
            }
            else
            {
                this.characterRenderer.draw(b,this, this.getLocalPosition(Game1.viewport) + new Vector2((float)(this.sprite.spriteWidth * Game1.pixelZoom / 2), (float)(this.GetBoundingBox().Height / 2)) + (this.shakeTimer > 0 ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Microsoft.Xna.Framework.Rectangle?(this.Sprite.SourceRect), Color.White * alpha, this.rotation, new Vector2((float)(this.sprite.spriteWidth / 2), (float)((double)this.sprite.spriteHeight * 3.0 / 4.0)), Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip || this.sprite.currentAnimation != null && this.sprite.currentAnimation[this.sprite.currentAnimationIndex].flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.991f : (float)this.getStandingY() / 10000f));
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
                this.characterRenderer.draw(b,this, this.getLocalPosition(Game1.viewport) + vector2 + (this.shakeTimer > 0 ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Microsoft.Xna.Framework.Rectangle?(sourceRect), Color.White * alpha, this.rotation, new Vector2((float)(sourceRect.Width / 2), (float)(sourceRect.Height / 2 + 1)), Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom + num, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.992f : (float)((double)this.getStandingY() / 10000.0 + 1.0 / 1000.0)));
            }

            //Checks if the npc is glowing.
            if (this.isGlowing)
                this.characterRenderer.draw(b,this, this.getLocalPosition(Game1.viewport) + new Vector2((float)(this.sprite.spriteWidth * Game1.pixelZoom / 2), (float)(this.GetBoundingBox().Height / 2)) + (this.shakeTimer > 0 ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Microsoft.Xna.Framework.Rectangle?(this.Sprite.SourceRect), this.glowingColor * this.glowingTransparency, this.rotation, new Vector2((float)(this.sprite.spriteWidth / 2), (float)((double)this.sprite.spriteHeight * 3.0 / 4.0)), Math.Max(0.2f, this.scale) * 4f, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.99f : (float)((double)this.getStandingY() / 10000.0 + 1.0 / 1000.0)));

            //This code runs if the npc is emoting.
            if (!this.IsEmoting || Game1.eventUp)
                return;
            Vector2 localPosition1 = this.getLocalPosition(Game1.viewport);
            localPosition1.Y -= (float)(Game1.tileSize / 2 + this.sprite.spriteHeight * Game1.pixelZoom);
            b.Draw(Game1.emoteSpriteSheet, localPosition1, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(this.CurrentEmoteIndex * 16 % Game1.emoteSpriteSheet.Width, this.CurrentEmoteIndex * 16 / Game1.emoteSpriteSheet.Width * 16, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)this.getStandingY() / 10000f);
        }


        public virtual void drawNonModularSprite(SpriteBatch b, float alpha = 1f)
        {
            if (this.sprite == null || this.isInvisible || !Utility.isOnScreen(this.position, 2 * Game1.tileSize))
                return;
            if (this.swimming)
            {
                b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize + Game1.tileSize / 4 + this.yJumpOffset * 2)) + (this.shakeTimer > 0 ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero) - new Vector2(0.0f, this.yOffset), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(this.sprite.SourceRect.X, this.sprite.SourceRect.Y, this.sprite.SourceRect.Width, this.sprite.SourceRect.Height / 2 - (int)((double)this.yOffset / (double)Game1.pixelZoom))), Color.White, this.rotation, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize * 3 / 2)) / 4f, Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.991f : (float)this.getStandingY() / 10000f));
                Vector2 localPosition = this.getLocalPosition(Game1.viewport);
                //b.Draw(Game1.staminaRect, new Microsoft.Xna.Framework.Rectangle((int)localPosition.X + (int)this.yOffset + Game1.pixelZoom * 2, (int)localPosition.Y - 32 * Game1.pixelZoom + this.sprite.SourceRect.Height * Game1.pixelZoom + Game1.tileSize * 3 / 4 + this.yJumpOffset * 2 - (int)this.yOffset, this.sprite.SourceRect.Width * Game1.pixelZoom - (int)this.yOffset * 2 - Game1.pixelZoom * 4, Game1.pixelZoom), new Microsoft.Xna.Framework.Rectangle?(Game1.staminaRect.Bounds), Color.White * 0.75f, 0.0f, Vector2.Zero, SpriteEffects.None, (float)((double)this.getStandingY() / 10000.0 + 1.0 / 1000.0));
            }
            else
                b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float)(this.sprite.spriteWidth * Game1.pixelZoom / 2), (float)(this.GetBoundingBox().Height / 2)) + (this.shakeTimer > 0 ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Microsoft.Xna.Framework.Rectangle?(this.Sprite.SourceRect), Color.White * alpha, this.rotation, new Vector2((float)(this.sprite.spriteWidth / 2), (float)((double)this.sprite.spriteHeight * 3.0 / 4.0)), Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip || this.sprite.currentAnimation != null && this.sprite.currentAnimation[this.sprite.currentAnimationIndex].flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.991f : (float)this.getStandingY() / 10000f));
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
            if (this.isGlowing)
                b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float)(this.sprite.spriteWidth * Game1.pixelZoom / 2), (float)(this.GetBoundingBox().Height / 2)) + (this.shakeTimer > 0 ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Microsoft.Xna.Framework.Rectangle?(this.Sprite.SourceRect), this.glowingColor * this.glowingTransparency, this.rotation, new Vector2((float)(this.sprite.spriteWidth / 2), (float)((double)this.sprite.spriteHeight * 3.0 / 4.0)), Math.Max(0.2f, this.scale) * 4f, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.99f : (float)((double)this.getStandingY() / 10000.0 + 1.0 / 1000.0)));
            Vector2 localPosition1 = this.getLocalPosition(Game1.viewport);
            localPosition1.Y -= (float)(Game1.tileSize / 2 + this.sprite.spriteHeight * Game1.pixelZoom);
            //b.Draw(Game1.emoteSpriteSheet, localPosition1, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(this.CurrentEmoteIndex * 16 % Game1.emoteSpriteSheet.Width, this.CurrentEmoteIndex * 16 / Game1.emoteSpriteSheet.Width * 16, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)this.getStandingY() / 10000f);
        }


        public override void draw(SpriteBatch b, float alpha = 1f)
        {
            if (this.characterRenderer == null)
            {
                this.drawNonModularSprite(b, alpha);
            }
            else
            {
                this.drawModular(b, alpha);
            }
        }
    }
}
