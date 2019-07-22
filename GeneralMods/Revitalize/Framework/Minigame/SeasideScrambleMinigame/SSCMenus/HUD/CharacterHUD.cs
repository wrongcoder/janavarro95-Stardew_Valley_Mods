using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardustCore.Animations;
using StardustCore.UIUtilities;
using StardustCore.UIUtilities.SpriteFonts.Components;

namespace Revitalize.Framework.Minigame.SeasideScrambleMinigame.SSCMenus.HUD
{
    public class CharacterHUD: IClickableMenuExtended
    {
        public AnimatedSprite background;
        public SSCEnums.PlayerID playerID;
        public bool showHUD;

        public TexturedString str;

        public CharacterHUD()
        {

        }

        public CharacterHUD(int x, int y, int width, int height,SSCEnums.PlayerID Player) : base(x, y, width, height, false)
        {
            this.background = new AnimatedSprite("Background",new Vector2(x,y),new AnimationManager(SeasideScramble.self.textureUtils.getExtendedTexture("SSCUI", "DialogBox"),new Animation(0,0,32,32)),Color.White);

            this.playerID = Player;
            this.showHUD = false;
            this.str = SeasideScramble.self.gameFont.ParseString("012",new Vector2(100,100),Color.White,true,2f);
            this.str.setPosition(new Vector2(this.xPositionOnScreen+100, this.yPositionOnScreen+50));

        }

        public override void update(GameTime time)
        {
            if (this.showHUD == false) return;
            if (SeasideScramble.self.getPlayer(this.playerID) != null)
            {
                this.background.color = SeasideScramble.self.getPlayer(this.playerID).playerColor;
                if (this.str.getText() != "345")
                {
                    this.str.setText("345", SeasideScramble.self.gameFont, Color.White);
                }
            }
        }

        /// <summary>
        /// Draw the HUD to the screen.
        /// </summary>
        /// <param name="b"></param>
        public override void draw(SpriteBatch b)
        {
            if (this.showHUD == false) return;
            //Draw the HUD background.
            //b.Draw(this.background.texture, new Vector2(100, 100), SeasideScramble.self.camera.getXNARect(), SeasideScramble.self.players[this.playerID].playerColor, 0f, Vector2.Zero, new Vector2(4f, 2f), SpriteEffects.None, 0f);
            this.background.draw(b,this.background.position,new Vector2(8f,4f),0f);
            this.str.draw(b,new Rectangle(0,0,16,16),0f);
        }

        /// <summary>
        /// Display the HUD.
        /// </summary>
        public void displayHUD()
        {
            this.showHUD = true;
        }

        /// <summary>
        /// Hide the HUD.
        /// </summary>
        public void hideHUD()
        {
            this.showHUD = false;
        }

    }
}
