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

       

        public SSCTextureUtilities textureUtils;

        public SSCPlayer player;

        public SeasideScramble()
        {
            self = this;

            this.textureUtils = new SSCTextureUtilities();
            TextureManager playerManager = new TextureManager("SSCPlayer");
            playerManager.searchForTextures(ModCore.ModHelper, ModCore.Manifest, Path.Combine("Content", "Minigames", "SeasideScramble", "Graphics", "Player"));
            this.textureUtils.addTextureManager(playerManager);

            this.LoadTextures();

            this.LoadMaps();
            this.loadStartingMap();
            this.quitGame = false;

            this.player = new SSCPlayer();
        }

        private void LoadTextures()
        {
            
        }

        private void LoadMaps()
        {
            this.SeasideScrambleMaps = new Dictionary<string, SeasideScrambleMap>();
            this.SeasideScrambleMaps.Add("TestRoom",new SeasideScrambleMap(SeasideScrambleMap.LoadMap("TestRoom.tbin").Value));
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
            if (this.currentMap!=null){
                this.currentMap.draw(b);
            }
            b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState)null, (RasterizerState)null);
            if (this.player != null)
            {
                this.player.draw(b);
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
            this.checkForMovementInput(k);
        }

        /// <summary>
        /// Checks for player movement.
        /// </summary>
        /// <param name="K"></param>
        private void checkForMovementInput(Keys K)
        {
            Microsoft.Xna.Framework.Input.GamePadState state = this.getGamepadState(PlayerIndex.One);
            if(K== Keys.A)
            {
                ModCore.log("A pressed for Seaside Scramble!");
                this.player.movePlayer(SSCEnums.FacingDirection.Left);
            }
            if (K == Keys.W)
            {
                ModCore.log("W pressed for Seaside Scramble!");
                this.player.movePlayer(SSCEnums.FacingDirection.Up);
            }
            if(K== Keys.S)
            {
                ModCore.log("S pressed for Seaside Scramble!");
                this.player.movePlayer(SSCEnums.FacingDirection.Down);
            }
            if(K== Keys.D)
            {
                ModCore.log("D pressed for Seaside Scramble!");
                this.player.movePlayer(SSCEnums.FacingDirection.Right);
            }
            

            if(K== Keys.Escape)
            {
                this.quitGame = true;
            }
        }

        private GamePadState getGamepadState(PlayerIndex index)
        {
           return Microsoft.Xna.Framework.Input.GamePad.GetState(PlayerIndex.One);
        }

        public void receiveKeyRelease(Keys K)
        {
            //throw new NotImplementedException();
            if (K == Keys.A)
            {
                ModCore.log("A released for Seaside Scramble!");
                this.player.isMoving = false;
            }
            if (K == Keys.W)
            {
                ModCore.log("W pressed for Seaside Scramble!");
                this.player.isMoving = false;
            }
            if (K == Keys.S)
            {
                ModCore.log("S pressed for Seaside Scramble!");
                this.player.isMoving = false;
            }
            if (K == Keys.D)
            {
                ModCore.log("D pressed for Seaside Scramble!");
                this.player.isMoving = false;
            }
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
    }
}
