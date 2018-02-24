using CustomNPCFramework.Framework.ModularNPCS.ModularRenderers;
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
    public class ExtendedNPC :StardewValley.NPC
    {
        public BasicRenderer characterRenderer;

        
        public ExtendedNPC() :base()
        {
        }
        public ExtendedNPC(BasicRenderer renderer,Vector2 position,int facingDirection,string name): base(null, position, facingDirection, name, null)
        {
            this.characterRenderer = renderer;
        }

        public ExtendedNPC(BasicRenderer renderer,Texture2D portrait, Vector2 position, int facingDirection, string name) : base(null, position, facingDirection, name, null)
        {
            this.characterRenderer = renderer;
            this.Portrait = portrait;
        }

        //ERROR NEED FIXING
        public override void reloadSprite()
        {
            string name = this.name;
            string str = name == "Old Mariner" ? "Mariner" : (name == "Dwarf King" ? "DwarfKing" : (name == "Mister Qi" ? "MrQi" : (name == "???" ? "Monsters\\Shadow Guy" : this.name)));
            if (this.name.Equals(Utility.getOtherFarmerNames()[0]))
                str = Game1.player.isMale ? "maleRival" : "femaleRival";
            if (!this.IsMonster)
            {
                this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\" + str));
                if (!this.name.Contains("Dwarf"))
                    this.sprite.spriteHeight = 32;
            }
            else
                this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Monsters\\" + str));
            try
            {
                this.portrait = Game1.content.Load<Texture2D>("Portraits\\" + str);
            }
            catch (Exception ex)
            {
                this.portrait = (Texture2D)null;
            }
            int num = this.isInvisible ? 1 : 0;
            if (!Game1.newDay && (int)Game1.gameMode != 6)
                return;
            this.faceDirection(this.DefaultFacingDirection);
            this.scheduleTimeToTry = 9999999;
            this.previousEndPoint = new Point((int)this.defaultPosition.X / Game1.tileSize, (int)this.defaultPosition.Y / Game1.tileSize);
            this.Schedule = this.getSchedule(Game1.dayOfMonth);
            this.faceDirection(this.defaultFacingDirection);
            this.sprite.standAndFaceDirection(this.defaultFacingDirection);
            this.loadSeasonalDialogue();
            this.updateDialogue();
            if (this.isMarried())
                this.marriageDuties();
            bool flag = Utility.isFestivalDay(Game1.dayOfMonth, Game1.currentSeason);
            if (this.name.Equals("Robin") && Game1.player.daysUntilHouseUpgrade > 0 && !flag)
            {
                this.setTilePosition(68, 14);
                this.ignoreMultiplayerUpdates = true;
                this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
        {
          new FarmerSprite.AnimationFrame(24, 75),
          new FarmerSprite.AnimationFrame(25, 75),
          new FarmerSprite.AnimationFrame(26, 300, false, false, new AnimatedSprite.endOfAnimationBehavior(this.robinHammerSound), false),
          new FarmerSprite.AnimationFrame(27, 1000, false, false, new AnimatedSprite.endOfAnimationBehavior(this.robinVariablePause), false)
        });
                this.ignoreScheduleToday = true;
                this.CurrentDialogue.Clear();
                this.currentDialogue.Push(new StardewValley.Dialogue(Game1.player.daysUntilHouseUpgrade == 2 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.3926") : Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.3927"), this));
            }
            else if (this.name.Equals("Robin") && Game1.getFarm().isThereABuildingUnderConstruction() && !flag)
            {
                this.ignoreMultiplayerUpdates = true;
                this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
        {
          new FarmerSprite.AnimationFrame(24, 75),
          new FarmerSprite.AnimationFrame(25, 75),
          new FarmerSprite.AnimationFrame(26, 300, false, false, new AnimatedSprite.endOfAnimationBehavior(this.robinHammerSound), false),
          new FarmerSprite.AnimationFrame(27, 1000, false, false, new AnimatedSprite.endOfAnimationBehavior(this.robinVariablePause), false)
        });
                this.ignoreScheduleToday = true;
                Building underConstruction = Game1.getFarm().getBuildingUnderConstruction();
                if (underConstruction.daysUntilUpgrade > 0)
                {
                    if (!underConstruction.indoors.characters.Contains(this))
                        underConstruction.indoors.addCharacter(this);
                    if (this.currentLocation != null)
                        this.currentLocation.characters.Remove(this);
                    this.currentLocation = underConstruction.indoors;
                    this.setTilePosition(1, 5);
                }
                else
                {
                    Game1.warpCharacter(this, "Farm", new Vector2((float)(underConstruction.tileX + underConstruction.tilesWide / 2), (float)(underConstruction.tileY + underConstruction.tilesHigh / 2)), false, false);
                    this.position.X += (float)(Game1.tileSize / 4);
                    this.position.Y -= (float)(Game1.tileSize / 2);
                }
                this.CurrentDialogue.Clear();
                this.currentDialogue.Push(new StardewValley.Dialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.3926"), this));
            }
            if (this.name.Equals("Shane") || this.name.Equals("Emily"))
                this.datable = true;
            try
            {
                this.displayName = Game1.content.Load<Dictionary<string, string>>("Data\\NPCDispositions")[this.name].Split('/')[11];
            }
            catch (Exception ex)
            {
            }
        }

        //ERROR NEED FIXING
        public virtual bool checkAction(Farmer who, GameLocation l)
        {
            if (this.isInvisible)
                return false;
            if (who.isRidingHorse())
                who.Halt();
            if (this.name.Equals("Henchman") && l.name.Equals("WitchSwamp"))
            {
                if (!Game1.player.mailReceived.Contains("Henchman1"))
                {
                    Game1.player.mailReceived.Add("Henchman1");
                    this.CurrentDialogue.Push(new StardewValley.Dialogue(Game1.content.LoadString("Strings\\Characters:Henchman1"), this));
                    Game1.drawDialogue(this);
                    Game1.player.addQuest(27);
                    Game1.player.friendships.Add("Henchman", new int[6]);
                }
                else
                {
                    if (who.ActiveObject != null && who.ActiveObject.canBeGivenAsGift())
                    {
                        this.tryToReceiveActiveObject(who);
                        return true;
                    }
                    if (this.controller == null)
                    {
                        this.CurrentDialogue.Push(new StardewValley.Dialogue(Game1.content.LoadString("Strings\\Characters:Henchman2"), this));
                        Game1.drawDialogue(this);
                    }
                }
                return true;
            }
            if (Game1.NPCGiftTastes.ContainsKey(this.name) && !Game1.player.friendships.ContainsKey(this.name))
            {
                Game1.player.friendships.Add(this.name, new int[6]);
                if (this.name.Equals("Krobus"))
                {
                    this.currentDialogue.Push(new StardewValley.Dialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.3990"), this));
                    Game1.drawDialogue(this);
                    return true;
                }
            }
            if (who.checkForQuestComplete(this, -1, -1, (Item)who.ActiveObject, (string)null, -1, 5))
            {
                this.faceTowardFarmerForPeriod(6000, 3, false, who);
                return true;
            }
            if (this.name.Equals("Dwarf") && this.currentDialogue.Count <= 0 && (who.canUnderstandDwarves && l.name.Equals("Mine")))
                Game1.activeClickableMenu = (IClickableMenu)new ShopMenu(Utility.getDwarfShopStock(), 0, "Dwarf");
            if (this.name.Equals("Krobus"))
            {
                if (who.hasQuest(28))
                {
                    this.CurrentDialogue.Push(new StardewValley.Dialogue(Game1.content.LoadString("Strings\\Characters:KrobusDarkTalisman"), this));
                    Game1.drawDialogue(this);
                    who.removeQuest(28);
                    who.mailReceived.Add("krobusUnseal");
                    TemporaryAnimatedSprite t1 = new TemporaryAnimatedSprite(Projectile.projectileSheet, new Microsoft.Xna.Framework.Rectangle(0, 0, 16, 16), 3000f, 1, 0, new Vector2(31f, 17f) * (float)Game1.tileSize, false, false);
                    t1.scale = (float)Game1.pixelZoom;
                    t1.delayBeforeAnimationStart = 1;
                    t1.startSound = "debuffSpell";
                    t1.motion = new Vector2(-9f, 1f);
                    t1.rotationChange = (float)Math.PI / 64f;
                    int num1 = 1;
                    t1.light = num1 != 0;
                    double num2 = 1.0;
                    t1.lightRadius = (float)num2;
                    Color color1 = new Color(150, 0, 50);
                    t1.lightcolor = color1;
                    double num3 = 1.0;
                    t1.layerDepth = (float)num3;
                    double num4 = 3.0 / 1000.0;
                    t1.alphaFade = (float)num4;
                    GameLocation l1 = l;
                    int timer1 = 200;
                    int num5 = 1;
                    DelayedAction.addTemporarySpriteAfterDelay(t1, l1, timer1, num5 != 0);
                    TemporaryAnimatedSprite t2 = new TemporaryAnimatedSprite(Projectile.projectileSheet, new Microsoft.Xna.Framework.Rectangle(0, 0, 16, 16), 3000f, 1, 0, new Vector2(31f, 17f) * (float)Game1.tileSize, false, false);
                    t2.startSound = "debuffSpell";
                    t2.delayBeforeAnimationStart = 1;
                    double pixelZoom = (double)Game1.pixelZoom;
                    t2.scale = (float)pixelZoom;
                    Vector2 vector2 = new Vector2(-9f, 1f);
                    t2.motion = vector2;
                    double num6 = 0.0490873865783215;
                    t2.rotationChange = (float)num6;
                    int num7 = 1;
                    t2.light = num7 != 0;
                    double num8 = 1.0;
                    t2.lightRadius = (float)num8;
                    Color color2 = new Color(150, 0, 50);
                    t2.lightcolor = color2;
                    double num9 = 1.0;
                    t2.layerDepth = (float)num9;
                    double num10 = 3.0 / 1000.0;
                    t2.alphaFade = (float)num10;
                    GameLocation l2 = l;
                    int timer2 = 700;
                    int num11 = 1;
                    DelayedAction.addTemporarySpriteAfterDelay(t2, l2, timer2, num11 != 0);
                    return true;
                }
                if (this.currentDialogue.Count <= 0 && l is Sewer)
                    Game1.activeClickableMenu = (IClickableMenu)new ShopMenu((l as Sewer).getShadowShopStock(), 0, "Krobus");
            }
            if (this.name.Equals(who.spouse) && who.IsMainPlayer)
            {
                int timeOfDay = Game1.timeOfDay;
                if (this.sprite.currentAnimation == null)
                    this.faceDirection(-3);
                if (this.sprite.currentAnimation == null && who.friendships.ContainsKey(this.name) && (who.friendships[this.name][0] >= 3375 && !who.mailReceived.Contains("CF_Spouse")))
                {
                    this.CurrentDialogue.Push(new StardewValley.Dialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4001"), this));
                    Game1.player.addItemByMenuIfNecessary((Item)new Object(Vector2.Zero, 434, "Cosmic Fruit", false, false, false, false), (ItemGrabMenu.behaviorOnItemSelect)null);
                    who.mailReceived.Add("CF_Spouse");
                    return true;
                }
                if (this.sprite.currentAnimation == null && !this.hasTemporaryMessageAvailable() && (this.CurrentDialogue.Count == 0 && Game1.timeOfDay < 2200) && (this.controller == null && who.ActiveObject == null))
                {
                    this.faceGeneralDirection(who.getStandingPosition(), 0);
                    who.faceGeneralDirection(this.getStandingPosition(), 0);
                    if (this.facingDirection == 3 || this.facingDirection == 1)
                    {
                        int frame = 28;
                        bool flag = true;
                        string name = this.name;
                        // ISSUE: reference to a compiler-generated method
                        uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(name);
                        if (stringHash <= 1708213605U)
                        {
                            if (stringHash <= 587846041U)
                            {
                                if ((int)stringHash != 161540545)
                                {
                                    if ((int)stringHash == 587846041 && name == "Penny")
                                    {
                                        frame = 35;
                                        flag = true;
                                    }
                                }
                                else if (name == "Sebastian")
                                {
                                    frame = 40;
                                    flag = false;
                                }
                            }
                            else if ((int)stringHash != 1067922812)
                            {
                                if ((int)stringHash != 1281010426)
                                {
                                    if ((int)stringHash == 1708213605 && name == "Alex")
                                    {
                                        frame = 42;
                                        flag = true;
                                    }
                                }
                                else if (name == "Maru")
                                {
                                    frame = 28;
                                    flag = false;
                                }
                            }
                            else if (name == "Sam")
                            {
                                frame = 36;
                                flag = true;
                            }
                        }
                        else if (stringHash <= 2571828641U)
                        {
                            if ((int)stringHash != 1866496948)
                            {
                                if ((int)stringHash != 2010304804)
                                {
                                    if ((int)stringHash == -1723138655 && name == "Emily")
                                    {
                                        frame = 33;
                                        flag = false;
                                    }
                                }
                                else if (name == "Harvey")
                                {
                                    frame = 31;
                                    flag = false;
                                }
                            }
                            else if (name == "Shane")
                            {
                                frame = 34;
                                flag = false;
                            }
                        }
                        else if ((int)stringHash != -1562053956)
                        {
                            if ((int)stringHash != -1468719973)
                            {
                                if ((int)stringHash == -1228790996 && name == "Elliott")
                                {
                                    frame = 35;
                                    flag = false;
                                }
                            }
                            else if (name == "Leah")
                            {
                                frame = 25;
                                flag = true;
                            }
                        }
                        else if (name == "Abigail")
                        {
                            frame = 33;
                            flag = false;
                        }
                        bool flip = flag && this.facingDirection == 3 || !flag && this.facingDirection == 1;
                        if (who.getFriendshipHeartLevelForNPC(this.name) > 9)
                        {
                            this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
              {
                new FarmerSprite.AnimationFrame(frame, Game1.IsMultiplayer ? 1000 : 10, false, flip, new AnimatedSprite.endOfAnimationBehavior(this.haltMe), true)
              });
                            if (!this.hasBeenKissedToday)
                            {
                                who.changeFriendship(10, this);
                                who.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(211, 428, 7, 6), 2000f, 1, 0, new Vector2((float)this.getTileX(), (float)this.getTileY()) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 4), (float)-Game1.tileSize), false, false, 1f, 0.0f, Color.White, (float)Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                                {
                                    motion = new Vector2(0.0f, -0.5f),
                                    alphaFade = 0.01f
                                });
                                Game1.playSound("dwop");
                                who.exhausted = false;
                            }
                            this.hasBeenKissedToday = true;
                        }
                        else
                        {
                            this.faceDirection(Game1.random.NextDouble() < 0.5 ? 2 : 0);
                            this.doEmote(12, true);
                        }
                        who.CanMove = false;
                        who.FarmerSprite.pauseForSingleAnimation = false;
                        if (flag && !flip || !flag & flip)
                            who.faceDirection(3);
                        else
                            who.faceDirection(1);
                        who.FarmerSprite.animateOnce(new List<FarmerSprite.AnimationFrame>()
            {
              new FarmerSprite.AnimationFrame(101, 1000, 0, false, who.facingDirection == 3, (AnimatedSprite.endOfAnimationBehavior) null, false, 0),
              new FarmerSprite.AnimationFrame(6, 1, false, who.facingDirection == 3, new AnimatedSprite.endOfAnimationBehavior(Farmer.completelyStopAnimating), false)
            }.ToArray());
                        return true;
                    }
                }
            }
            bool flag1 = false;
            if (who.friendships.ContainsKey(this.name))
            {
                flag1 = this.checkForNewCurrentDialogue(who.friendships[this.name][0], false);
                if (!flag1)
                    flag1 = this.checkForNewCurrentDialogue(who.friendships[this.name][0], true);
            }
            if (who.IsMainPlayer && who.friendships.ContainsKey(this.name) && this.endOfRouteMessage != null | flag1)
            {
                if (!flag1 && this.setTemporaryMessages(who))
                {
                    Game1.player.checkForQuestComplete(this, -1, -1, (Item)null, (string)null, 5, -1);
                    return false;
                }
                if (this.sprite.Texture.Bounds.Height > 32)
                    this.faceTowardFarmerForPeriod(5000, 4, false, who);
                if (who.ActiveObject != null && who.ActiveObject.canBeGivenAsGift())
                {
                    this.tryToReceiveActiveObject(who);
                    Game1.stats.checkForFriendshipAchievements();
                    this.faceTowardFarmerForPeriod(3000, 4, false, who);
                    return true;
                }
                if (!this.name.Contains("King") && !who.hasPlayerTalkedToNPC(this.name) && who.friendships.ContainsKey(this.name))
                {
                    who.friendships[this.name][2] = 1;
                    who.changeFriendship(10, this);
                    Game1.stats.checkForFriendshipAchievements();
                    Game1.player.checkForQuestComplete(this, -1, -1, (Item)null, (string)null, 5, -1);
                }
                Game1.drawDialogue(this);
            }
            else if (this.CurrentDialogue.Count > 0)
            {
                if (!this.name.Contains("King") && who.ActiveObject != null && who.ActiveObject.canBeGivenAsGift())
                {
                    if (who.IsMainPlayer)
                    {
                        this.tryToReceiveActiveObject(who);
                        Game1.stats.checkForFriendshipAchievements();
                    }
                    else
                        this.faceTowardFarmerForPeriod(3000, 4, false, who);
                }
                else if (who.hasClubCard && this.name.Equals("Bouncer") && who.IsMainPlayer)
                {
                    Response[] answerChoices = new Response[2]
                    {
            new Response("Yes.", Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4018")),
            new Response("That's", Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4020"))
                    };
                    l.createQuestionDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4021"), answerChoices, "ClubCard");
                }
                else if (this.CurrentDialogue.Count >= 1 || this.endOfRouteMessage != null)
                {
                    if (this.setTemporaryMessages(who))
                    {
                        Game1.player.checkForQuestComplete(this, -1, -1, (Item)null, (string)null, 5, -1);
                        return false;
                    }
                    if (this.sprite.Texture.Bounds.Height > 32)
                        this.faceTowardFarmerForPeriod(5000, 4, false, who);
                    if (who.IsMainPlayer)
                    {
                        if (!this.name.Contains("King") && !who.hasPlayerTalkedToNPC(this.name) && who.friendships.ContainsKey(this.name))
                        {
                            who.friendships[this.name][2] = 1;
                            Game1.player.checkForQuestComplete(this, -1, -1, (Item)null, (string)null, 5, -1);
                            who.changeFriendship(20, this);
                            Game1.stats.checkForFriendshipAchievements();
                        }
                        Game1.drawDialogue(this);
                    }
                }
                else if (!this.doingEndOfRouteAnimation)
                {
                    try
                    {
                        if (who.friendships.ContainsKey(this.name))
                            this.faceTowardFarmerForPeriod(who.friendships[this.name][0] / 125 * 1000 + 1000, 4, false, who);
                    }
                    catch (Exception ex)
                    {
                    }
                    if (Game1.random.NextDouble() < 0.1)
                        this.doEmote(8, true);
                }
            }
            else if (this.name.Equals("Cat") && !(this as StardewValley.Monsters.Cat).wasPet)
            {
                (this as StardewValley.Monsters.Cat).wasPet = true;
                (this as StardewValley.Monsters.Cat).loveForMaster += 10;
                this.doEmote(20, true);
                Game1.playSound("purr");
            }
            else if (who.ActiveObject != null && who.ActiveObject.canBeGivenAsGift())
            {
                this.tryToReceiveActiveObject(who);
                Game1.stats.checkForFriendshipAchievements();
                this.faceTowardFarmerForPeriod(3000, 4, false, who);
                return true;
            }
            if (this.setTemporaryMessages(who) || !this.doingEndOfRouteAnimation && this.goingToDoEndOfRouteAnimation || this.endOfRouteMessage == null)
                return false;
            Game1.drawDialogue(this);
            return false;
        }

        //ERROR NEED FIXING
        public virtual void behaviorOnFarmerLocationEntry(GameLocation location, Farmer who)
        {
            if (this.sprite == null || this.sprite.currentAnimation != null || this.sprite.sourceRect.Height <= 32)
                return;
            this.sprite.spriteWidth = 16;
            this.sprite.spriteHeight = 16;
            this.sprite.CurrentFrame = 0;
        }

        //ERROR NEED FIXING
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

        //ERROR NEED FIXING
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

        //ERROR NEED FIXING
        public override void update(GameTime time, GameLocation location)
        {
            if (this.returningToEndPoint)
            {
                this.returnToEndPoint();
                this.MovePosition(time, Game1.viewport, location);
            }
            else if (this.temporaryController != null)
            {
                if (this.temporaryController.update(time))
                    this.temporaryController = (PathFindController)null;
                this.updateEmote(time);
            }
            else
                base.update(time, location);
            if (this.textAboveHeadTimer > 0)
            {
                if (this.textAboveHeadPreTimer > 0)
                {
                    this.textAboveHeadPreTimer = this.textAboveHeadPreTimer - time.ElapsedGameTime.Milliseconds;
                }
                else
                {
                    this.textAboveHeadTimer = this.textAboveHeadTimer - time.ElapsedGameTime.Milliseconds;
                    this.textAboveHeadAlpha = this.textAboveHeadTimer <= 500 ? Math.Max(0.0f, this.textAboveHeadAlpha - 0.04f) : Math.Min(1f, this.textAboveHeadAlpha + 0.1f);
                }
            }
            if (this.isWalkingInSquare && !this.returningToEndPoint)
                this.randomSquareMovement(time);
            if (this.Sprite != null && this.Sprite.currentAnimation != null && (!Game1.eventUp && this.Sprite.animateOnce(time)))
                this.Sprite.currentAnimation = (List<FarmerSprite.AnimationFrame>)null;
            TimeSpan timeSpan;
            if (this.movementPause > 0 && (!Game1.dialogueUp || this.controller != null))
            {
                this.freezeMotion = true;
                int movementPause = this.movementPause;
                timeSpan = time.ElapsedGameTime;
                int milliseconds = timeSpan.Milliseconds;
                this.movementPause = movementPause - milliseconds;
                if (this.movementPause <= 0)
                    this.freezeMotion = false;
            }
            if (this.shakeTimer > 0)
            {
                int shakeTimer = this.shakeTimer;
                timeSpan = time.ElapsedGameTime;
                int milliseconds = timeSpan.Milliseconds;
                this.shakeTimer = shakeTimer - milliseconds;
            }
            if (this.lastPosition.Equals(this.position))
            {
                double sinceLastMovement = (double)this.timerSinceLastMovement;
                timeSpan = time.ElapsedGameTime;
                double milliseconds = (double)timeSpan.Milliseconds;
                this.timerSinceLastMovement = (float)(sinceLastMovement + milliseconds);
            }
            else
                this.timerSinceLastMovement = 0.0f;
            if (!this.swimming)
                return;
            timeSpan = time.TotalGameTime;
            this.yOffset = (float)Math.Cos(timeSpan.TotalMilliseconds / 2000.0) * (float)Game1.pixelZoom;
            float swimTimer1 = this.swimTimer;
            double swimTimer2 = (double)this.swimTimer;
            timeSpan = time.ElapsedGameTime;
            double milliseconds1 = (double)timeSpan.Milliseconds;
            this.swimTimer = (float)(swimTimer2 - milliseconds1);
            if ((double)this.timerSinceLastMovement == 0.0)
            {
                if ((double)swimTimer1 > 400.0 && (double)this.swimTimer <= 400.0 && location.Equals((object)Game1.currentLocation))
                {
                    location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.tileSize, Game1.tileSize), (float)(150.0 - ((double)Math.Abs(this.xVelocity) + (double)Math.Abs(this.yVelocity)) * 3.0), 8, 0, new Vector2(this.position.X, (float)(this.getStandingY() - Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5, 0.01f, 0.01f, Color.White, 1f, 3f / 1000f, 0.0f, 0.0f, false));
                    Game1.playSound("slosh");
                }
                if ((double)this.swimTimer >= 0.0)
                    return;
                this.swimTimer = 800f;
                if (!location.Equals((object)Game1.currentLocation))
                    return;
                Game1.playSound("slosh");
                location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.tileSize, Game1.tileSize), (float)(150.0 - ((double)Math.Abs(this.xVelocity) + (double)Math.Abs(this.yVelocity)) * 3.0), 8, 0, new Vector2(this.position.X, (float)(this.getStandingY() - Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5, 0.01f, 0.01f, Color.White, 1f, 3f / 1000f, 0.0f, 0.0f, false));
            }
            else
            {
                if ((double)this.swimTimer >= 0.0)
                    return;
                this.swimTimer = 100f;
            }
        }

        //ERROR NEED FIXING
        public virtual void prepareToDisembarkOnNewSchedulePath()
        {
            while (this.CurrentDialogue.Count > 0 && this.CurrentDialogue.Peek().removeOnNextMove)
                this.CurrentDialogue.Pop();
            this.nextEndOfRouteMessage = (string)null;
            this.endOfRouteMessage = (string)null;
            if (this.doingEndOfRouteAnimation)
            {
                List<FarmerSprite.AnimationFrame> animation = new List<FarmerSprite.AnimationFrame>();
                for (int index = 0; index < this.routeEndOutro.Length; ++index)
                {
                    if (index == this.routeEndOutro.Length - 1)
                        animation.Add(new FarmerSprite.AnimationFrame(this.routeEndOutro[index], 100, 0, false, false, new AnimatedSprite.endOfAnimationBehavior(this.routeEndAnimationFinished), true, 0));
                    else
                        animation.Add(new FarmerSprite.AnimationFrame(this.routeEndOutro[index], 100, 0, false, false, (AnimatedSprite.endOfAnimationBehavior)null, false, 0));
                }
                if (animation.Count > 0)
                    this.sprite.setCurrentAnimation(animation);
                else
                    this.routeEndAnimationFinished((Farmer)null);
                if (this.endOfRouteBehaviorName != null)
                    this.finishRouteBehavior(this.endOfRouteBehaviorName);
            }
            else
                this.routeEndAnimationFinished((Farmer)null);
            if (!this.isMarried())
                return;
            if (this.temporaryController == null && Utility.getGameLocationOfCharacter(this) is FarmHouse)
            {
                this.temporaryController = new PathFindController((Character)this, this.getHome(), new Point(this.getHome().warps[0].X, this.getHome().warps[0].Y), 2, true)
                {
                    NPCSchedule = true
                };
                if (this.temporaryController.pathToEndPoint == null || this.temporaryController.pathToEndPoint.Count <= 0)
                {
                    this.temporaryController = (PathFindController)null;
                    this.schedule = (Dictionary<int, SchedulePathDescription>)null;
                }
                else
                    this.followSchedule = true;
            }
            else
            {
                if (!(Utility.getGameLocationOfCharacter(this) is Farm))
                    return;
                this.temporaryController = (PathFindController)null;
                this.schedule = (Dictionary<int, SchedulePathDescription>)null;
            }
        }

        //ERROR NEED FIXING
        public virtual void routeEndAnimationFinished(Farmer who)
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
            if (!this.isWalkingInSquare)
                return;
            this.returningToEndPoint = true;
            this.timeAfterSquare = Game1.timeOfDay;
        }

        ///ERROR NEED FIXING
        public virtual void doAnimationAtEndOfScheduleRoute(Character c, GameLocation l)
        {
            List<FarmerSprite.AnimationFrame> animation = new List<FarmerSprite.AnimationFrame>();
            for (int index = 0; index < this.routeEndIntro.Length; ++index)
            {
                if (index == this.routeEndIntro.Length - 1)
                    animation.Add(new FarmerSprite.AnimationFrame(this.routeEndIntro[index], 100, 0, false, false, new AnimatedSprite.endOfAnimationBehavior(this.doMiddleAnimation), true, 0));
                else
                    animation.Add(new FarmerSprite.AnimationFrame(this.routeEndIntro[index], 100, 0, false, false, (AnimatedSprite.endOfAnimationBehavior)null, false, 0));
            }
            this.doingEndOfRouteAnimation = true;
            this.freezeMotion = true;
            this.sprite.setCurrentAnimation(animation);
        }

        ///ERROR NEED FIXING
        public virtual void doMiddleAnimation(Farmer who)
        {
            List<FarmerSprite.AnimationFrame> animation = new List<FarmerSprite.AnimationFrame>();
            for (int index = 0; index < this.routeEndAnimation.Length; ++index)
                animation.Add(new FarmerSprite.AnimationFrame(this.routeEndAnimation[index], 100, 0, false, false, (AnimatedSprite.endOfAnimationBehavior)null, false, 0));
            this.sprite.setCurrentAnimation(animation);
            this.sprite.loop = true;
            if (this.endOfRouteBehaviorName == null)
                return;
            this.startRouteBehavior(this.endOfRouteBehaviorName);
        }

        //ERROR NEED FIXING
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
                if (!(behaviorName == "abigail_videogames"))
                {
                    if (!(behaviorName == "dick_fish"))
                    {
                        if (!(behaviorName == "clint_hammer"))
                            return;
                        this.extendSourceRect(16, 0, true);
                        this.sprite.spriteWidth = 32;
                        this.sprite.ignoreSourceRectUpdates = false;
                        this.sprite.CurrentFrame = 8;
                        this.sprite.currentAnimation[14] = new FarmerSprite.AnimationFrame(9, 100, 0, false, false, new AnimatedSprite.endOfAnimationBehavior(this.clintHammerSound), false, 0);
                    }
                    else
                    {
                        this.extendSourceRect(0, 32, true);
                        if (!Utility.isOnScreen(Utility.Vector2ToPoint(this.position), Game1.tileSize, this.currentLocation))
                            return;
                        Game1.playSound("slosh");
                    }
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

        //ERROR NEED FIXING
        public void finishRouteBehavior(string behaviorName)
        {
            if (!(behaviorName == "abigail_videogames"))
            {
                if (!(behaviorName == "clint_hammer") && !(behaviorName == "dick_fish"))
                    return;
                this.reloadSprite();
                this.sprite.spriteWidth = 16;
                this.sprite.spriteHeight = 32;
                this.sprite.UpdateSourceRect();
                this.Halt();
                this.movementPause = 1;
            }
            else
                Utility.getGameLocationOfCharacter(this).removeTemporarySpritesWithID(688);
        }
        //ERROR NEED FIXING
        public virtual void getHitByPlayer(Farmer who, GameLocation location)
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
                    this.CurrentDialogue.Push(new StardewValley.Dialogue(Game1.random.NextDouble() < 0.5 ? Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4293") : Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4294"), this));
                }
                location.debris.Add(new Debris(this.sprite.Texture, Game1.random.Next(3, 8), new Vector2((float)this.GetBoundingBox().Center.X, (float)this.GetBoundingBox().Center.Y)));
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
                Game1.warpCharacter(this, this.defaultMap, this.defaultPosition / (float)Game1.tileSize, true, false);
            if (this.name.Equals("Maru") || this.name.Equals("Shane"))
                this.sprite.Texture = Game1.content.Load<Texture2D>("Characters\\" + this.name);
            if (this.name.Equals("Willy") || this.name.Equals("Clint"))
            {
                this.sprite.spriteWidth = 16;
                this.sprite.spriteHeight = 32;
                this.sprite.ignoreSourceRectUpdates = false;
                this.sprite.UpdateSourceRect();
                this.isInvisible = false;
            }
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
            this.directionsToNewLocation = (SchedulePathDescription)null;
            this.faceDirection(this.DefaultFacingDirection);
            this.scheduleTimeToTry = 9999999;
            this.previousEndPoint = new Point((int)this.defaultPosition.X / Game1.tileSize, (int)this.defaultPosition.Y / Game1.tileSize);
            this.isWalkingInSquare = false;
            this.returningToEndPoint = false;
            this.lastCrossroad = Microsoft.Xna.Framework.Rectangle.Empty;
            if (this.isVillager())
                this.Schedule = this.getSchedule(dayOfMonth);
            this.endOfRouteMessage = (string)null;
            bool flag = Utility.isFestivalDay(dayOfMonth, Game1.currentSeason);
            if (this.name.Equals("Robin") && Game1.player.daysUntilHouseUpgrade > 0 && !flag)
            {
                this.ignoreMultiplayerUpdates = true;
                Game1.warpCharacter(this, "Farm", new Vector2(68f, 14f), false, false);
                this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
        {
          new FarmerSprite.AnimationFrame(24, 75),
          new FarmerSprite.AnimationFrame(25, 75),
          new FarmerSprite.AnimationFrame(26, 300, false, false, new AnimatedSprite.endOfAnimationBehavior(this.robinHammerSound), false),
          new FarmerSprite.AnimationFrame(27, 1000, false, false, new AnimatedSprite.endOfAnimationBehavior(this.robinVariablePause), false)
        });
                this.ignoreScheduleToday = true;
                this.CurrentDialogue.Clear();
                this.currentDialogue.Push(new StardewValley.Dialogue(Game1.player.daysUntilHouseUpgrade == 2 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.3926") : Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.3927"), this));
            }
            else if (this.name.Equals("Robin") && Game1.getFarm().isThereABuildingUnderConstruction() && !flag)
            {
                Building underConstruction = Game1.getFarm().getBuildingUnderConstruction();
                if (underConstruction.daysUntilUpgrade > 0)
                {
                    if (!underConstruction.indoors.characters.Contains(this))
                        underConstruction.indoors.addCharacter(this);
                    if (this.currentLocation != null)
                        this.currentLocation.characters.Remove(this);
                    this.currentLocation = underConstruction.indoors;
                    this.setTilePosition(1, 5);
                }
                else
                {
                    Game1.warpCharacter(this, "Farm", new Vector2((float)(underConstruction.tileX + underConstruction.tilesWide / 2), (float)(underConstruction.tileY + underConstruction.tilesHigh / 2)), false, false);
                    this.position.X += (float)(Game1.tileSize / 4);
                    this.position.Y -= (float)(Game1.tileSize / 2);
                }
                this.ignoreMultiplayerUpdates = true;
                this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
        {
          new FarmerSprite.AnimationFrame(24, 75),
          new FarmerSprite.AnimationFrame(25, 75),
          new FarmerSprite.AnimationFrame(26, 300, false, false, new AnimatedSprite.endOfAnimationBehavior(this.robinHammerSound), false),
          new FarmerSprite.AnimationFrame(27, 1000, false, false, new AnimatedSprite.endOfAnimationBehavior(this.robinVariablePause), false)
        });
                this.ignoreScheduleToday = true;
                this.CurrentDialogue.Clear();
                this.currentDialogue.Push(new StardewValley.Dialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.3926"), this));
            }
            if (!this.isMarried())
                return;
            this.marriageDuties();
            this.daysMarried = this.daysMarried + 1;
        }

        //ERROR NEED FIXING
        public virtual void setUpForOutdoorPatioActivity()
        {
            Game1.warpCharacter(this, "Farm", new Vector2(71f, 10f), false, false);
            this.setNewDialogue("MarriageDialogue", "patio_", -1, false, true);
            string name = this.name;
            // ISSUE: reference to a compiler-generated method
            uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(name);
            if (stringHash <= 1866496948U)
            {
                if (stringHash <= 1067922812U)
                {
                    if ((int)stringHash != 161540545)
                    {
                        if ((int)stringHash != 587846041)
                        {
                            if ((int)stringHash != 1067922812 || !(name == "Sam"))
                                return;
                            this.setTilePosition(71, 8);
                            this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
              {
                new FarmerSprite.AnimationFrame(25, 3000),
                new FarmerSprite.AnimationFrame(27, 500),
                new FarmerSprite.AnimationFrame(26, 100),
                new FarmerSprite.AnimationFrame(28, 100),
                new FarmerSprite.AnimationFrame(27, 500),
                new FarmerSprite.AnimationFrame(25, 2000),
                new FarmerSprite.AnimationFrame(27, 500),
                new FarmerSprite.AnimationFrame(26, 100),
                new FarmerSprite.AnimationFrame(29, 100),
                new FarmerSprite.AnimationFrame(30, 100),
                new FarmerSprite.AnimationFrame(32, 500),
                new FarmerSprite.AnimationFrame(31, 1000),
                new FarmerSprite.AnimationFrame(30, 100),
                new FarmerSprite.AnimationFrame(29, 100)
              });
                        }
                        else
                        {
                            if (!(name == "Penny"))
                                return;
                            this.setTilePosition(71, 8);
                            this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
              {
                new FarmerSprite.AnimationFrame(18, 6000),
                new FarmerSprite.AnimationFrame(19, 500)
              });
                        }
                    }
                    else
                    {
                        if (!(name == "Sebastian"))
                            return;
                        this.setTilePosition(71, 9);
                        this.drawOffset = new Vector2((float)(Game1.tileSize / 4), (float)(Game1.tileSize / 2 + Game1.pixelZoom * 2));
                        this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
            {
              new FarmerSprite.AnimationFrame(32, 500, Game1.tileSize, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0),
              new FarmerSprite.AnimationFrame(36, 500, Game1.tileSize, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0),
              new FarmerSprite.AnimationFrame(32, 500, Game1.tileSize, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0),
              new FarmerSprite.AnimationFrame(36, 500, Game1.tileSize, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0),
              new FarmerSprite.AnimationFrame(32, 500, Game1.tileSize, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0),
              new FarmerSprite.AnimationFrame(36, 500, Game1.tileSize, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0),
              new FarmerSprite.AnimationFrame(32, 500, Game1.tileSize, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0),
              new FarmerSprite.AnimationFrame(36, 2000, Game1.tileSize, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0),
              new FarmerSprite.AnimationFrame(33, 100, Game1.tileSize, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0),
              new FarmerSprite.AnimationFrame(34, 100, Game1.tileSize, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0),
              new FarmerSprite.AnimationFrame(35, 3000, Game1.tileSize, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0),
              new FarmerSprite.AnimationFrame(34, 100, Game1.tileSize, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0),
              new FarmerSprite.AnimationFrame(33, 100, Game1.tileSize, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0),
              new FarmerSprite.AnimationFrame(32, 1500, Game1.tileSize, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0)
            });
                    }
                }
                else if ((int)stringHash != 1281010426)
                {
                    if ((int)stringHash != 1708213605)
                    {
                        if ((int)stringHash != 1866496948 || !(name == "Shane"))
                            return;
                        this.setTilePosition(69, 9);
                        this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
            {
              new FarmerSprite.AnimationFrame(28, 4000, Game1.tileSize, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0),
              new FarmerSprite.AnimationFrame(29, 800, Game1.tileSize, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0)
            });
                    }
                    else
                    {
                        if (!(name == "Alex"))
                            return;
                        this.setTilePosition(71, 8);
                        this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
            {
              new FarmerSprite.AnimationFrame(34, 4000),
              new FarmerSprite.AnimationFrame(33, 300),
              new FarmerSprite.AnimationFrame(28, 200),
              new FarmerSprite.AnimationFrame(29, 100),
              new FarmerSprite.AnimationFrame(30, 100),
              new FarmerSprite.AnimationFrame(31, 100),
              new FarmerSprite.AnimationFrame(32, 100),
              new FarmerSprite.AnimationFrame(31, 100),
              new FarmerSprite.AnimationFrame(30, 100),
              new FarmerSprite.AnimationFrame(29, 100),
              new FarmerSprite.AnimationFrame(28, 800),
              new FarmerSprite.AnimationFrame(29, 100),
              new FarmerSprite.AnimationFrame(30, 100),
              new FarmerSprite.AnimationFrame(31, 100),
              new FarmerSprite.AnimationFrame(32, 100),
              new FarmerSprite.AnimationFrame(31, 100),
              new FarmerSprite.AnimationFrame(30, 100),
              new FarmerSprite.AnimationFrame(29, 100),
              new FarmerSprite.AnimationFrame(28, 800),
              new FarmerSprite.AnimationFrame(33, 200)
            });
                    }
                }
                else
                {
                    if (!(name == "Maru"))
                        return;
                    this.setTilePosition(70, 8);
                    this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
          {
            new FarmerSprite.AnimationFrame(16, 4000),
            new FarmerSprite.AnimationFrame(17, 200),
            new FarmerSprite.AnimationFrame(18, 200),
            new FarmerSprite.AnimationFrame(19, 200),
            new FarmerSprite.AnimationFrame(20, 200),
            new FarmerSprite.AnimationFrame(21, 200),
            new FarmerSprite.AnimationFrame(22, 200),
            new FarmerSprite.AnimationFrame(23, 200)
          });
                }
            }
            else if (stringHash <= 2571828641U)
            {
                if ((int)stringHash != 2010304804)
                {
                    if ((int)stringHash != -1860673204)
                    {
                        if ((int)stringHash != -1723138655 || !(name == "Emily"))
                            return;
                        this.setTilePosition(70, 9);
                        this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
            {
              new FarmerSprite.AnimationFrame(54, 4000, Game1.tileSize, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0)
            });
                    }
                    else
                    {
                        if (!(name == "Haley"))
                            return;
                        this.setTilePosition(70, 8);
                        this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
            {
              new FarmerSprite.AnimationFrame(30, 2000),
              new FarmerSprite.AnimationFrame(31, 200),
              new FarmerSprite.AnimationFrame(24, 2000),
              new FarmerSprite.AnimationFrame(25, 1000),
              new FarmerSprite.AnimationFrame(32, 200),
              new FarmerSprite.AnimationFrame(33, 2000),
              new FarmerSprite.AnimationFrame(32, 200),
              new FarmerSprite.AnimationFrame(25, 2000),
              new FarmerSprite.AnimationFrame(32, 200),
              new FarmerSprite.AnimationFrame(33, 2000)
            });
                    }
                }
                else
                {
                    if (!(name == "Harvey"))
                        return;
                    this.setTilePosition(71, 8);
                    this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
          {
            new FarmerSprite.AnimationFrame(42, 6000),
            new FarmerSprite.AnimationFrame(43, 1000),
            new FarmerSprite.AnimationFrame(39, 100),
            new FarmerSprite.AnimationFrame(43, 500),
            new FarmerSprite.AnimationFrame(39, 100),
            new FarmerSprite.AnimationFrame(43, 1000),
            new FarmerSprite.AnimationFrame(42, 5000),
            new FarmerSprite.AnimationFrame(43, 3000)
          });
                }
            }
            else if ((int)stringHash != -1562053956)
            {
                if ((int)stringHash != -1468719973)
                {
                    if ((int)stringHash != -1228790996 || !(name == "Elliott"))
                        return;
                    this.setTilePosition(71, 8);
                    this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
          {
            new FarmerSprite.AnimationFrame(33, 3000),
            new FarmerSprite.AnimationFrame(32, 500),
            new FarmerSprite.AnimationFrame(33, 3000),
            new FarmerSprite.AnimationFrame(32, 500),
            new FarmerSprite.AnimationFrame(33, 2000),
            new FarmerSprite.AnimationFrame(34, 1500)
          });
                }
                else
                {
                    if (!(name == "Leah"))
                        return;
                    this.setTilePosition(71, 8);
                    this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
          {
            new FarmerSprite.AnimationFrame(16, 100),
            new FarmerSprite.AnimationFrame(17, 100),
            new FarmerSprite.AnimationFrame(18, 100),
            new FarmerSprite.AnimationFrame(19, 300),
            new FarmerSprite.AnimationFrame(16, 100),
            new FarmerSprite.AnimationFrame(17, 100),
            new FarmerSprite.AnimationFrame(18, 100),
            new FarmerSprite.AnimationFrame(19, 1000),
            new FarmerSprite.AnimationFrame(16, 100),
            new FarmerSprite.AnimationFrame(17, 100),
            new FarmerSprite.AnimationFrame(18, 100),
            new FarmerSprite.AnimationFrame(19, 300),
            new FarmerSprite.AnimationFrame(16, 100),
            new FarmerSprite.AnimationFrame(17, 100),
            new FarmerSprite.AnimationFrame(18, 100),
            new FarmerSprite.AnimationFrame(19, 300),
            new FarmerSprite.AnimationFrame(16, 100),
            new FarmerSprite.AnimationFrame(17, 100),
            new FarmerSprite.AnimationFrame(18, 100),
            new FarmerSprite.AnimationFrame(19, 2000)
          });
                }
            }
            else
            {
                if (!(name == "Abigail"))
                    return;
                this.setTilePosition(71, 8);
                this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
        {
          new FarmerSprite.AnimationFrame(16, 500),
          new FarmerSprite.AnimationFrame(17, 500),
          new FarmerSprite.AnimationFrame(18, 500),
          new FarmerSprite.AnimationFrame(19, 500)
        });
            }
        }


        //ERROR NEED FIXING!!!!!!!!!!!!!!!!!!
        /// <summary>
        /// Used to draw the npc with the custom renderer.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="alpha"></param>
        public override void draw(SpriteBatch b, float alpha = 1f)
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
                b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float)(this.sprite.spriteWidth * Game1.pixelZoom / 2), (float)(this.GetBoundingBox().Height / 2)) + (this.shakeTimer > 0 ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Microsoft.Xna.Framework.Rectangle?(this.Sprite.SourceRect), this.glowingColor * this.glowingTransparency, this.rotation, new Vector2((float)(this.sprite.spriteWidth / 2), (float)((double)this.sprite.spriteHeight * 3.0 / 4.0)), Math.Max(0.2f, this.scale) * 4f, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.99f : (float)((double)this.getStandingY() / 10000.0 + 1.0 / 1000.0)));

            //This code runs if the npc is emoting.
            if (!this.IsEmoting || Game1.eventUp)
                return;
            Vector2 localPosition1 = this.getLocalPosition(Game1.viewport);
            localPosition1.Y -= (float)(Game1.tileSize / 2 + this.sprite.spriteHeight * Game1.pixelZoom);
            b.Draw(Game1.emoteSpriteSheet, localPosition1, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(this.CurrentEmoteIndex * 16 % Game1.emoteSpriteSheet.Width, this.CurrentEmoteIndex * 16 / Game1.emoteSpriteSheet.Width * 16, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)this.getStandingY() / 10000f);
        }
    }
}
