using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Revitalize.Framework.Minigame.SeasideScrambleMinigame;
using StardustCore.UIUtilities;
namespace Revitalize.Framework.Minigame.SeasideScrambleMinigame
{
    public class SeasideScramble : StardewValley.Minigames.IMinigame
    {
        public static SeasideScramble self;

        SeasideScrambleMap currentMap;
        public int currentNumberOfPlayers = 0;
        public const int maxPlayers = 4;

        public Dictionary<string, SeasideScrambleMap> SeasideScrambleMaps;
        public bool quitGame;
        public Vector2 topLeftScreenCoordinate;


        public SSCTextureUtilities textureUtils;

        public SSCPlayer player;

        //public xTile.Dimensions.Rectangle viewport;

        public SSCCamera camera;

        public IClickableMenuExtended activeMenu;
        public bool isMenuUp
        {
            get
            {
                return this.activeMenu != null;
            }
        }

        public SeasideScramble()
        {
            self = this;
            this.camera = new SSCCamera();
            //this.viewport = new xTile.Dimensions.Rectangle(StardewValley.Game1.viewport);
            this.topLeftScreenCoordinate = new Vector2((float)(this.camera.viewport.Width / 2 - 384), (float)(this.camera.viewport.Height / 2 - 384));


            this.LoadTextures();

            this.LoadMaps();
            this.loadStartingMap();
            this.quitGame = false;

            this.player = new SSCPlayer();
            this.player.setColor(Color.Red);

            this.activeMenu = new SSCMenus.TitleScreen(this.camera.viewport);
        }

        private void LoadTextures()
        {
            this.textureUtils = new SSCTextureUtilities();
            TextureManager playerManager = new TextureManager("SSCPlayer");
            playerManager.searchForTextures(ModCore.ModHelper, ModCore.Manifest, Path.Combine("Content", "Minigames", "SeasideScramble", "Graphics", "Player"));
            TextureManager mapTextureManager = new TextureManager("SSCMaps");
            mapTextureManager.searchForTextures(ModCore.ModHelper, ModCore.Manifest, Path.Combine("Content", "Minigames", "SeasideScramble", "Maps", "Backgrounds"));
            this.textureUtils.addTextureManager(playerManager);
            this.textureUtils.addTextureManager(mapTextureManager);
        }

        private void LoadMaps()
        {
            this.SeasideScrambleMaps = new Dictionary<string, SeasideScrambleMap>();
            this.SeasideScrambleMaps.Add("TestRoom", new SeasideScrambleMap(SeasideScrambleMap.LoadMap("TestRoom.tbin").Value));
        }
        private void loadStartingMap()
        {
            this.currentMap = this.SeasideScrambleMaps["TestRoom"];
        }

        /// <summary>
        /// What happens when the screen changes size.
        /// </summary>
        public void changeScreenSize()
        {
            Viewport viewport = StardewValley.Game1.graphics.GraphicsDevice.Viewport;
            double num1 = (double)(viewport.Width / 2 - 384);
            viewport = StardewValley.Game1.graphics.GraphicsDevice.Viewport;
            double num2 = (double)(viewport.Height / 2 - 384);
            this.topLeftScreenCoordinate = new Vector2((float)num1, (float)num2);
            this.camera.viewport = new xTile.Dimensions.Rectangle(StardewValley.Game1.viewport);
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Used to update Stardew Valley while this minigame runs. True means SDV updates false means the SDV pauses all update ticks.
        /// </summary>
        /// <returns></returns>
        public bool doMainGameUpdates()
        {
            return false;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Draws all game aspects to the screen.
        /// </summary>
        /// <param name="b"></param>
        public void draw(SpriteBatch b)
        {
            if (this.currentMap != null)
            {
                this.currentMap.draw(b);
            }
            b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState)null, (RasterizerState)null);
            if (this.player != null)
            {
                this.player.draw(b);
            }

            if (this.activeMenu != null)
            {
                this.activeMenu.draw(b);
            }

            b.End();
        }

        /// <summary>
        /// What happens when the left click is held.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void leftClickHeld(int x, int y)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// The id of the minigame???
        /// </summary>
        /// <returns></returns>
        public string minigameId()
        {
            return "Seaside Scramble Stardew Lite Edition";
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Does this override free mous emovements?
        /// </summary>
        /// <returns></returns>
        public bool overrideFreeMouseMovement()
        {
            return false;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// ??? Undocumended.
        /// </summary>
        /// <param name="data"></param>
        public void receiveEventPoke(int data)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// What happens when a key is pressed.
        /// </summary>
        /// <param name="k"></param>
        public void receiveKeyPress(Keys k)
        {
            //throw new NotImplementedException();
            if (k == Keys.Escape)
            {
                this.quitGame = true;
            }
            this.player.receiveKeyPress(k);
        }



        private GamePadState getGamepadState(PlayerIndex index)
        {
            return Microsoft.Xna.Framework.Input.GamePad.GetState(PlayerIndex.One);
        }

        public void receiveKeyRelease(Keys K)
        {
            this.player.receiveKeyRelease(K);
        }

        public void receiveLeftClick(int x, int y, bool playSound = true)
        {
            //throw new NotImplementedException();
        }

        public void receiveRightClick(int x, int y, bool playSound = true)
        {
            //throw new NotImplementedException();
        }

        public void releaseLeftClick(int x, int y)
        {
            //throw new NotImplementedException();
        }

        public void releaseRightClick(int x, int y)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Called every update frame.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool tick(GameTime time)
        {
            KeyboardState state = Keyboard.GetState();

            foreach (Keys k in state.GetPressedKeys())
            {
                this.receiveKeyPress(k);
            }

            if (this.quitGame)
            {
                return true;
            }
            if (this.currentMap != null)
            {
                this.currentMap.update(time);
            }
            if (this.player != null)
            {
                this.player.update(time);
                this.camera.centerOnPosition(this.player.position);
            }
            if (this.activeMenu != null)
            {
                this.activeMenu.update(time);
            }

            return false;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Called when the minigame is quit upon.
        /// </summary>
        public void unload()
        {
            //throw new NotImplementedException();
            ModCore.log("Exit the game!");
        }

        public static Vector2 GlobalToLocal(xTile.Dimensions.Rectangle viewport, Vector2 globalPosition)
        {
            return new Vector2(globalPosition.X - (float)viewport.X, globalPosition.Y - (float)viewport.Y);
        }

        public static Vector2 GlobalToLocal(Vector2 globalPosition)
        {
            return new Vector2(globalPosition.X - (float)SeasideScramble.self.camera.viewport.X, globalPosition.Y - (float)SeasideScramble.self.camera.viewport.Y);
        }
    }
}
