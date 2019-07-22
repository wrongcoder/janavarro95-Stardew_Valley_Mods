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
    public class CharacterHUD : IClickableMenuExtended
    {
        public AnimatedSprite background;
        public SSCEnums.PlayerID playerID;
        public bool showHUD;


        public AnimatedSprite heart;
        public TexturedString playerHealth;
        public int remainingHealthLerpFrames;
        public int framesToUpdate = 5;

        public AnimatedSprite gun;
        public TexturedString playerAmmo;

        public SSCPlayer Player
        {
            get
            {
                return SeasideScramble.self.getPlayer(this.playerID);
            }
        }

        public CharacterHUD()
        {

        }

        public CharacterHUD(int x, int y, int width, int height, SSCEnums.PlayerID Player) : base(x, y, width, height, false)
        {
            this.background = new AnimatedSprite("Background", new Vector2(x, y), new AnimationManager(SeasideScramble.self.textureUtils.getExtendedTexture("SSCUI", "DialogBox"), new Animation(0, 0, 32, 32)), Color.White);
            this.playerID = Player;
            this.showHUD = false;

            this.heart = new AnimatedSprite("Heart", new Vector2(x + 32, y + 10), new AnimationManager(SeasideScramble.self.textureUtils.getExtendedTexture("SSCUI", "Heart"), new Animation(0, 0, 7, 6)), Color.White);
            this.playerHealth = SeasideScramble.self.gameFont.ParseString("100", new Vector2(100, this.yPositionOnScreen + 10), Color.White, true, 2f);
            this.playerHealth.setPosition(new Vector2(this.xPositionOnScreen + 100, this.yPositionOnScreen + 10));


            this.gun = new AnimatedSprite("Gun", new Vector2(x + 32, y + 50), new AnimationManager(SeasideScramble.self.textureUtils.getExtendedTexture("Guns", "BasicGun"), new Animation(0, 0, 16, 16)), Color.White);
            this.playerAmmo = SeasideScramble.self.gameFont.ParseString("100", new Vector2(100, this.yPositionOnScreen + 50), Color.White, true, 2f);
            this.playerAmmo.setPosition(new Vector2(this.xPositionOnScreen + 100, this.yPositionOnScreen + 50));
        }

        public override void update(GameTime time)
        {
            if (this.showHUD == false) return;
            if (SeasideScramble.self.getPlayer(this.playerID) != null)
            {
                this.background.color = SeasideScramble.self.getPlayer(this.playerID).playerColor;
                this.healthDisplayLerp();
                if (this.Player.gun.remainingAmmo == SSCGuns.SSCGun.infiniteAmmo)
                {
                    this.playerAmmo.setText("999", SeasideScramble.self.gameFont, Color.White);
                }
                else
                {
                    this.playerAmmo.setText(this.Player.gun.remainingAmmo.ToString().PadLeft(3,'0'), SeasideScramble.self.gameFont, Color.White);
                }
            }
        }

        /// <summary>
        /// Has a small counting lerp for display text.
        /// </summary>
        private void healthDisplayLerp()
        {
            if (this.remainingHealthLerpFrames == 0)
            {
                this.remainingHealthLerpFrames = this.framesToUpdate;
                if (Convert.ToInt32(this.playerHealth.getText()) != this.Player.currentHealth)
                {
                    int health = Convert.ToInt32(this.playerHealth.getText());
                    health = health - 1;
                    string healthStr = health.ToString();
                    healthStr = healthStr.PadLeft(3, '0');
                    this.playerHealth.setText(healthStr, SeasideScramble.self.gameFont, Color.White);
                }
            }
            else
            {
                this.remainingHealthLerpFrames--;
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
            this.background.draw(b, this.background.position, new Vector2(8f, 4f), 0f);
            this.playerHealth.draw(b, new Rectangle(0, 0, 16, 16), 0f);
            this.playerAmmo.draw(b, new Rectangle(0, 0, 16, 16), 0f);
            this.heart.draw(b, 8f, 0f);
            this.gun.draw(b, 4f, 0f);
        }

        /// <summary>
        /// Display the HUD.
        /// </summary>
        public void displayHUD()
        {
            this.playerHealth.setText(SeasideScramble.self.getPlayer(this.playerID).currentHealth.ToString(), SeasideScramble.self.gameFont, Color.White);
            this.showHUD = true;

            SeasideScramble.self.getPlayer(this.playerID).takeDamage(100);
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
