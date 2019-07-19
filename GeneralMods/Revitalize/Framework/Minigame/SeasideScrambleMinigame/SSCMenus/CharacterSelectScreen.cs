using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardustCore.UIUtilities;

namespace Revitalize.Framework.Minigame.SeasideScrambleMinigame.SSCMenus
{
    public class CharacterSelectScreen: IClickableMenuExtended
    {
        StardustCore.UIUtilities.Texture2DExtended background;
        string menuTitle;

        public Dictionary<SSCEnums.PlayerID, int> playerColorIndex;

        public Dictionary<SSCEnums.PlayerID, Vector2> playerDisplayLocations;

        public List<Color> possibleColors;

        public bool closeMenu;

        public Dictionary<SSCEnums.PlayerID, int> inputDelays;
        public int maxInputDelay = 20;

        public CharacterSelectScreen(int x, int y, int width, int height) : base(x, y, width, height, false)
        {
            this.background = SeasideScramble.self.textureUtils.getExtendedTexture("SSCMaps", "TitleScreenBackground");
            this.menuTitle = "Character Selection";

            this.playerDisplayLocations = new Dictionary<SSCEnums.PlayerID, Vector2>()
            {
                {SSCEnums.PlayerID.One,new Vector2(SeasideScramble.self.camera.viewport.Width*.2f, SeasideScramble.self.camera.viewport.Height*.5f)},
                {SSCEnums.PlayerID.Two,new Vector2(SeasideScramble.self.camera.viewport.Width*.4f, SeasideScramble.self.camera.viewport.Height*.5f)},
                {SSCEnums.PlayerID.Three,new Vector2(SeasideScramble.self.camera.viewport.Width*.6f, SeasideScramble.self.camera.viewport.Height*.5f)},
                {SSCEnums.PlayerID.Four,new Vector2(SeasideScramble.self.camera.viewport.Width*.8f, SeasideScramble.self.camera.viewport.Height*.5f)},
            };

            this.possibleColors = new List<Color>()
            {
                Color.PaleVioletRed,
                Color.LightSkyBlue,
                Color.LawnGreen,
                Color.LightGoldenrodYellow,
                Color.HotPink,
                Color.Red,
                Color.Purple
            };
            this.playerColorIndex = new Dictionary<SSCEnums.PlayerID, int>()
            {
                { SSCEnums.PlayerID.One,-1},
                { SSCEnums.PlayerID.Two,-1},
                { SSCEnums.PlayerID.Three,-1},
                { SSCEnums.PlayerID.Four,-1},
            };
            this.inputDelays = new Dictionary<SSCEnums.PlayerID, int>()
            {
                { SSCEnums.PlayerID.One,0},
                { SSCEnums.PlayerID.Two,0},
                { SSCEnums.PlayerID.Three,0},
                { SSCEnums.PlayerID.Four,0},
            };
            SeasideScramble.self.camera.snapToPosition(new Vector2(0, 0));
        }

        public CharacterSelectScreen(xTile.Dimensions.Rectangle viewport) : this(0, 0, viewport.Width, viewport.Height)
        {
            
            
        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            this.xPositionOnScreen = 0;
            this.yPositionOnScreen = 0;
            this.width = newBounds.Width;
            this.height = newBounds.Height;
            //base.gameWindowSizeChanged(oldBounds, newBounds);

            //this.p1DisplayLocation = new Vector2(this.width/10,this.height/2);
            SeasideScramble.self.camera.snapToPosition(new Vector2(0, 0));

            this.playerDisplayLocations[SSCEnums.PlayerID.One] = new Vector2(SeasideScramble.self.camera.viewport.Width * .2f, SeasideScramble.self.camera.viewport.Height * .5f);
            this.playerDisplayLocations[SSCEnums.PlayerID.Two] = new Vector2(SeasideScramble.self.camera.viewport.Width * .4f, SeasideScramble.self.camera.viewport.Height * .5f);
            this.playerDisplayLocations[SSCEnums.PlayerID.Three] = new Vector2(SeasideScramble.self.camera.viewport.Width * .6f, SeasideScramble.self.camera.viewport.Height * .5f);
            this.playerDisplayLocations[SSCEnums.PlayerID.Four] = new Vector2(SeasideScramble.self.camera.viewport.Width * .8f, SeasideScramble.self.camera.viewport.Height * .5f);
        }

        public override void update(GameTime time)
        {
            GamePadState p1 = SeasideScramble.self.getGamepadState(PlayerIndex.One);
            GamePadState p2 = SeasideScramble.self.getGamepadState(PlayerIndex.Two);
            GamePadState p3 = SeasideScramble.self.getGamepadState(PlayerIndex.Three);
            GamePadState p4 = SeasideScramble.self.getGamepadState(PlayerIndex.Four);

            if (p1.IsButtonDown(Buttons.A))
            {
                this.initializeCharacter(SSCEnums.PlayerID.One);
            }
            if (p2.IsButtonDown(Buttons.A))
            {
                this.initializeCharacter(SSCEnums.PlayerID.Two);
            }
            if (p3.IsButtonDown(Buttons.A))
            {
                this.initializeCharacter(SSCEnums.PlayerID.Three);
            }
            if (p4.IsButtonDown(Buttons.A))
            {
                this.initializeCharacter(SSCEnums.PlayerID.Four);
            }

            this.inputDelays[SSCEnums.PlayerID.One]--;
            this.inputDelays[SSCEnums.PlayerID.Two]--;
            this.inputDelays[SSCEnums.PlayerID.Three]--;
            this.inputDelays[SSCEnums.PlayerID.Four]--;

            if (this.inputDelays[SSCEnums.PlayerID.One] < 0) this.inputDelays[SSCEnums.PlayerID.One] = 0;
            if (this.inputDelays[SSCEnums.PlayerID.Two] < 0) this.inputDelays[SSCEnums.PlayerID.Two] = 0;
            if (this.inputDelays[SSCEnums.PlayerID.Three] < 0) this.inputDelays[SSCEnums.PlayerID.Three] = 0;
            if (this.inputDelays[SSCEnums.PlayerID.Four] < 0) this.inputDelays[SSCEnums.PlayerID.Four] = 0;


            SeasideScramble.self.camera.snapToPosition(new Vector2(0, 0));
            ModCore.log("P1 loc" + this.playerDisplayLocations[SSCEnums.PlayerID.One]);
            ModCore.log("Viewport loc" + SeasideScramble.self.camera.Position);

        }

        /// <summary>
        /// Initializes a given character.
        /// </summary>
        /// <param name="player"></param>
        public void initializeCharacter(SSCEnums.PlayerID player)
        {
            if (SeasideScramble.self.players.ContainsKey(player))
            {
                return;
            }
            else
            {
                SeasideScramble.self.players.Add(player, new SSCPlayer(player));
                this.iteratePlayerColorIndex(player, 1);
            }            
            this.setPlayerColor(player);
        }

        /// <summary>
        /// Sets a given player's color.
        /// </summary>
        /// <param name="player"></param>
        private void setPlayerColor(SSCEnums.PlayerID player)
        {
            if (SeasideScramble.self.getPlayer(player) == null) return;
            SeasideScramble.self.getPlayer(player).setColor(this.possibleColors[this.playerColorIndex[player]]);
        }

        /// <summary>
        /// Iterates the player's color index to get the next possible color.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="amount"></param>
        private void iteratePlayerColorIndex(SSCEnums.PlayerID player,int amount)
        {
            if(player== SSCEnums.PlayerID.One)
            {
                if (this.inputDelays[player] != 0) return;
                this.inputDelays[player] = this.maxInputDelay;
                while (this.doesAnyOtherPlayerHaveThisColor(player) == true)
                {
                    if (this.playerColorIndex[player] >= this.possibleColors.Count)
                    {
                        this.playerColorIndex[player] = 0;
                    }
                    else if (this.playerColorIndex[player] < 0)
                    {
                        this.playerColorIndex[player] = this.possibleColors.Count-1;
                    }
                    else
                    {
                        this.playerColorIndex[player]+=amount;
                    }
                }
                this.playerColorIndex[player] += amount;
                if (this.playerColorIndex[player] >= this.possibleColors.Count)
                {
                    this.playerColorIndex[player] = 0;
                }
                else if (this.playerColorIndex[player] < 0)
                {
                    this.playerColorIndex[player] = this.possibleColors.Count - 1;
                }

            }
            this.setPlayerColor(player);
        }

        /// <summary>
        /// Checks if a given player has the same color index as another to prevent color duplicates.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        private bool doesOtherPlayerHaveThisColor(SSCEnums.PlayerID self, SSCEnums.PlayerID other)
        {
            if (SeasideScramble.self.getPlayer(other) == null) return false;
            if (this.playerColorIndex[self] == this.playerColorIndex[other]) return true;
            else return false;
        }
        /// <summary>
        /// Checks if any other player has the same color index to prevent color duplicates.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        private bool doesAnyOtherPlayerHaveThisColor(SSCEnums.PlayerID self)
        {
            for(int i = 0; i < 4; i++)
            {
                SSCEnums.PlayerID other = (SSCEnums.PlayerID)i;
                if (other == self) continue;
                if (this.doesOtherPlayerHaveThisColor(self, other) == false) continue;
                else return true;
            }
            return false;
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            this.initializeCharacter(SSCEnums.PlayerID.One);
        }

        public override void receiveKeyPress(Keys key)
        {
            if(key== Keys.Enter || key== Keys.Space)
            {    
                if (SeasideScramble.self.currentNumberOfPlayers > 0)
                {
                    this.setUpForGameplay();
                    SeasideScramble.self.menuManager.closeAllMenus();
                }
            }
            if(key== Keys.A)
            {
                this.iteratePlayerColorIndex(SSCEnums.PlayerID.One, -1);
            }
            if(key== Keys.D)
            {
                this.iteratePlayerColorIndex(SSCEnums.PlayerID.One, 1);
            }
        }

        private void setUpForGameplay()
        {

        }

        public override bool readyToClose()
        {
            if (this.closeMenu == true)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Draw everything to the screen.
        /// </summary>
        /// <param name="b"></param>
        public override void draw(SpriteBatch b)
        {
            //Draw background texture.
            b.Draw(this.background.texture, new Vector2(0,0), SeasideScramble.self.camera.getXNARect(), Color.White);

            Vector2 menuTitlePos = Game1.dialogueFont.MeasureString(this.menuTitle);
            b.DrawString(Game1.dialogueFont, this.menuTitle, new Vector2((this.width / 2) - (menuTitlePos.X / 2), this.height / 10),Color.White);

            foreach(KeyValuePair<SSCEnums.PlayerID,Vector2> pair in this.playerDisplayLocations)
            {
                //Draw player 1
                this.drawDialogueBoxBackground((int)pair.Value.X - 50, (int)pair.Value.Y - 100, 200, 200, Color.Brown);
                if (SeasideScramble.self.getPlayer(pair.Key) != null)
                {
                    SeasideScramble.self.getPlayer(pair.Key).draw(b, pair.Value);
                }
            }
            


        }

        public override void exitMenu(bool playSound = true)
        {
            base.exitMenu(playSound);
        }

    }
}
