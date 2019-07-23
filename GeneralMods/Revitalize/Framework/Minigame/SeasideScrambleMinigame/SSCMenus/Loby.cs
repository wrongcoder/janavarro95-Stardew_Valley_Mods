using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Revitalize.Framework.Minigame.SeasideScrambleMinigame.SSCEnemies.Spawners;
using StardustCore.UIUtilities;
using StardustCore.UIUtilities.MenuComponents;

namespace Revitalize.Framework.Minigame.SeasideScrambleMinigame.SSCMenus
{
    public class Loby:IClickableMenuExtended
    {
        StardustCore.UIUtilities.Texture2DExtended background;
        string menuText;
        Button shootingGalleryButton;
        Button backButton;

        public Loby()
        {

        }

        public Loby(int x, int y, int width, int height) : base(x, y, width, height, false)
        {
            this.background = SeasideScramble.self.textureUtils.getExtendedTexture("SSCMaps", "TitleScreenBackground");
            this.menuText = "The Loby"+System.Environment.NewLine+System.Environment.NewLine+"Choose a game mode";
            this.shootingGalleryButton = new Button(new Rectangle(100, 300, 64*4, 32*4), SeasideScramble.self.textureUtils.getExtendedTexture("SSCUI", "ShootingGalleryButton"), new Rectangle(0, 0, 64, 32), 4f);
            this.backButton = new Button(new Rectangle(100, 100, 64, 64), SeasideScramble.self.textureUtils.getExtendedTexture("SSCUI", "BackButton"),new Rectangle(0, 0, 16, 16), 4f);
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            if (this.shootingGalleryButton.containsPoint(x, y))
            {
                foreach (SSCPlayer p in SeasideScramble.self.players.Values)
                {
                    p.HUD.displayHUD();
                    //p.statusEffects.addStatusEffect(SE_Burn.SpawnBurnEffect(new Vector2(p.HUD.xPositionOnScreen,p.HUD.yPositionOnScreen),10*1000,1000,1.00d,1));
                }

                SeasideScramble.self.entities.addSpawner(new Target_Spawner(new Vector2(SeasideScrambleMap.TileSize * -1, SeasideScrambleMap.TileSize * 4), new Vector2(1, 0), Color.White, true, 1000, 5000, true, 0.25f, 3f, true));
                SeasideScramble.self.entities.addSpawner(new Target_Spawner(new Vector2(SeasideScrambleMap.TileSize * 17, SeasideScrambleMap.TileSize * 5), new Vector2(-1, 0), Color.White, true, 1000, 5000, true, 0.25f, 3f, true));

                SeasideScramble.self.currentMap.spawnPlayersAtPositions();
                //SSCEnemies.SSCE_Target.Spawn_SSCE_Target(new Vector2(100, 100), Color.Blue);
                //SSCEnemies.SSCE_Target.Spawn_SSCE_Target(new Vector2(200, 100), Color.Red);
                //SSCEnemies.SSCE_Target.Spawn_SSCE_Target(new Vector2(300, 100), Color.Green);
                SeasideScramble.self.menuManager.closeAllMenus();
            }
        }

        public override bool readyToClose()
        {
            return false;
        }

        public override void update(GameTime time)
        {
            
        }


        private void setUpForGameplay()
        {

        }

        public override void draw(SpriteBatch b)
        {
            //Draw background.
            b.Draw(this.background.texture, new Vector2(this.xPositionOnScreen, this.yPositionOnScreen), SeasideScramble.self.camera.getXNARect(), Color.White);
            Vector2 offset = StardewValley.Game1.dialogueFont.MeasureString(this.menuText);
            b.DrawString(StardewValley.Game1.dialogueFont,this.menuText,new Vector2((this.width / 2) - (offset.X / 2), this.height *.1f), Color.White);

            this.shootingGalleryButton.draw(b);
            this.backButton.draw(b);

            if (SeasideScramble.self.getPlayer(SSCEnums.PlayerID.One) != null)
            {
                SeasideScramble.self.getPlayer(SSCEnums.PlayerID.One).drawMouse(b);
            }
            if (SeasideScramble.self.getPlayer(SSCEnums.PlayerID.Two) != null)
            {
                SeasideScramble.self.getPlayer(SSCEnums.PlayerID.Two).drawMouse(b);
            }
            if (SeasideScramble.self.getPlayer(SSCEnums.PlayerID.Three) != null)
            {
                SeasideScramble.self.getPlayer(SSCEnums.PlayerID.Three).drawMouse(b);
            }
            if (SeasideScramble.self.getPlayer(SSCEnums.PlayerID.Four) != null)
            {
                SeasideScramble.self.getPlayer(SSCEnums.PlayerID.Four).drawMouse(b);
            }
        }

    }
}
