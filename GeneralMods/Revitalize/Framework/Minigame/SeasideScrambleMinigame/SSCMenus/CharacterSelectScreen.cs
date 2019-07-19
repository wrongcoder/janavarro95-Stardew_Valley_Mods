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

        public Color p1DefaultColor = Color.PaleVioletRed;
        public Color p2DefaultColor = Color.LightSkyBlue;
        public Color p3DefaultColor = Color.LawnGreen;
        public Color p4DefaultColor = Color.LightGoldenrodYellow;

        public Vector2 p1DisplayLocation;

        public bool closeMenu;

        public CharacterSelectScreen(int x, int y, int width, int height) : base(x, y, width, height, false)
        {

            this.background = SeasideScramble.self.textureUtils.getExtendedTexture("SSCMaps", "TitleScreenBackground");
            this.menuTitle = "Character Selection";

            this.p1DisplayLocation = this.getRelativePositionToMenu(.1f, .5f);
        }

        public CharacterSelectScreen(xTile.Dimensions.Rectangle viewport) : this(0, 0, viewport.Width, viewport.Height)
        {

        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            this.xPositionOnScreen = newBounds.X;
            this.yPositionOnScreen = newBounds.Y;
            this.width = newBounds.Width;
            this.height = newBounds.Height;
            base.gameWindowSizeChanged(oldBounds, newBounds);

            this.p1DisplayLocation = this.getRelativePositionToMenu(.1f, .5f);
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
                throw new NotImplementedException("Need to support player 2!");
            }
            if (p3.IsButtonDown(Buttons.A))
            {
                throw new NotImplementedException("Need to support player 3!");
            }
            if (p4.IsButtonDown(Buttons.A))
            {
                throw new NotImplementedException("Need to support player 4!");
            }

        }

        public void initializeCharacter(SSCEnums.PlayerID player)
        {
            if (SeasideScramble.self.players.ContainsKey(player))
            {
                SeasideScramble.self.players[player] = new SSCPlayer(player);
            }
            else
            {
                SeasideScramble.self.players.Add(player, new SSCPlayer(player));
            }
            SeasideScramble.self.getPlayer(player).setColor(this.p1DefaultColor);
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            this.initializeCharacter(SSCEnums.PlayerID.One);
        }

        public override void receiveKeyPress(Keys key)
        {
            if(key== Keys.Enter)
            {
                this.setUpForGameplay();
                this.closeMenu = true;
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
            b.Draw(this.background.texture, new Vector2(this.xPositionOnScreen, this.yPositionOnScreen), SeasideScramble.self.camera.getXNARect(), Color.White);

            Vector2 menuTitlePos = Game1.dialogueFont.MeasureString(this.menuTitle);
            b.DrawString(Game1.dialogueFont, this.menuTitle, new Vector2((this.width / 2) - (menuTitlePos.X / 2), this.height / 2),Color.White);

            if (SeasideScramble.self.getPlayer(SSCEnums.PlayerID.One) != null)
            {
                SeasideScramble.self.getPlayer(SSCEnums.PlayerID.One).draw(b,this.p1DisplayLocation);
            }
        }

        public override void exitMenu(bool playSound = true)
        {
            base.exitMenu(playSound);
        }

    }
}
