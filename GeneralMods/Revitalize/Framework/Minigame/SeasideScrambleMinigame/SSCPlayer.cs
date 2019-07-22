using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardustCore.Animations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using Revitalize.Framework.Utilities;

namespace Revitalize.Framework.Minigame.SeasideScrambleMinigame
{
    public class SSCPlayer
    {
        //TODO: Add movement speed variable
        //TODO: Add in health
        //TODO: Add in player HUD
        public AnimationManager characterSpriteController;
        public bool flipSprite;
        public SSCEnums.FacingDirection facingDirection;
        public Microsoft.Xna.Framework.Vector2 position;
        public bool isMoving;
        private bool movedThisFrame;
        public Color playerColor;
        public SSCEnums.PlayerID playerID;

        public const int junimoWalkingAnimationSpeed = 10;

        public StardustCore.Animations.AnimatedSprite mouseCursor;
        public Vector2 mouseSensitivity;

        public bool showMouseCursor;
        public int maxMouseSleepTime = 300;

        public SSCGuns.SSCGun gun;

        public Rectangle hitBox;

        public SSCMenus.HUD.CharacterHUD HUD;

        public int currentHealth;
        public int maxHealth;

        public bool isDead
        {
            get
            {
                return this.currentHealth <= 0;
            }
        }

        public SSCPlayer(SSCEnums.PlayerID PlayerID)
        {
            this.playerID = PlayerID;
            this.facingDirection = SSCEnums.FacingDirection.Down;
            this.characterSpriteController = new AnimationManager(SeasideScramble.self.textureUtils.getExtendedTexture("SSCPlayer", "Junimo"), new Animation(0, 0, 16, 16), new Dictionary<string, List<Animation>>{
                {"Idle_F",new List<Animation>()
                {
                    new Animation(0,0,16,16)
                } },
                {"Idle_B",new List<Animation>()
                {
                    new Animation(0,16*4,16,16)
                } },
                {"Idle_L",new List<Animation>()
                {
                    new Animation(0,16*3,16,16)
                } },
                 {"Idle_R",new List<Animation>()
                {
                    new Animation(0,16*3,16,16)
                } },
                 {"Walking_F",new List<Animation>()
                {
                    new Animation(16*0,16*0,16,16,junimoWalkingAnimationSpeed),
                    new Animation(16*1,16*0,16,16,junimoWalkingAnimationSpeed),
                    new Animation(16*2,16*0,16,16,junimoWalkingAnimationSpeed),
                    new Animation(16*3,16*0,16,16,junimoWalkingAnimationSpeed),
                    new Animation(16*4,16*0,16,16,junimoWalkingAnimationSpeed),
                    new Animation(16*5,16*0,16,16,junimoWalkingAnimationSpeed),
                    new Animation(16*6,16*0,16,16,junimoWalkingAnimationSpeed),
                    new Animation(16*7,16*0,16,16,junimoWalkingAnimationSpeed),
                } },
                {"Walking_R",new List<Animation>()
                {
                    new Animation(16*0,16*2,16,16,junimoWalkingAnimationSpeed),
                    new Animation(16*1,16*2,16,16,junimoWalkingAnimationSpeed),
                    new Animation(16*2,16*2,16,16,junimoWalkingAnimationSpeed),
                    new Animation(16*3,16*2,16,16,junimoWalkingAnimationSpeed),
                    new Animation(16*4,16*2,16,16,junimoWalkingAnimationSpeed),
                    new Animation(16*5,16*2,16,16,junimoWalkingAnimationSpeed),
                    new Animation(16*6,16*2,16,16,junimoWalkingAnimationSpeed),
                    new Animation(16*7,16*2,16,16,junimoWalkingAnimationSpeed),
                } },
                {"Walking_L",new List<Animation>()
                {
                    new Animation(16*0,16*2,16,16,junimoWalkingAnimationSpeed),
                    new Animation(16*1,16*2,16,16,junimoWalkingAnimationSpeed),
                    new Animation(16*2,16*2,16,16,junimoWalkingAnimationSpeed),
                    new Animation(16*3,16*2,16,16,junimoWalkingAnimationSpeed),
                    new Animation(16*4,16*2,16,16,junimoWalkingAnimationSpeed),
                    new Animation(16*5,16*2,16,16,junimoWalkingAnimationSpeed),
                    new Animation(16*6,16*2,16,16,junimoWalkingAnimationSpeed),
                    new Animation(16*7,16*2,16,16,junimoWalkingAnimationSpeed),
                } },
                {"Walking_B",new List<Animation>()
                {
                    new Animation(16*0,16*4,16,16,junimoWalkingAnimationSpeed),
                    new Animation(16*1,16*4,16,16,junimoWalkingAnimationSpeed),
                    new Animation(16*2,16*4,16,16,junimoWalkingAnimationSpeed),
                    new Animation(16*3,16*4,16,16,junimoWalkingAnimationSpeed),
                    new Animation(16*4,16*4,16,16,junimoWalkingAnimationSpeed),
                    new Animation(16*5,16*4,16,16,junimoWalkingAnimationSpeed),
                    new Animation(16*6,16*4,16,16,junimoWalkingAnimationSpeed),
                    new Animation(16*7,16*4,16,16,junimoWalkingAnimationSpeed),
                } },

            }, "Idle_F", 0, true);


            this.mouseCursor = new StardustCore.Animations.AnimatedSprite("P1Mouse", new Vector2(Game1.getMousePosition().X, Game1.getMousePosition().Y), new AnimationManager(SeasideScramble.self.textureUtils.getExtendedTexture("SSCUI", "Cursors"), new Animation(0, 0, 16, 16), new Dictionary<string, List<Animation>>()
            {
                {"Default",new List<Animation>()
                {
                    new Animation(0,0,16,16)

                } }

            }, "Default"), Color.White);
            if (this.playerID == SSCEnums.PlayerID.One)
            {
                this.mouseCursor.position = new Vector2(Game1.getMouseX(), Game1.getMouseY());
            }
            else
            {
                this.mouseCursor.position = this.position;
            }
            this.mouseSensitivity = new Vector2(3f, 3f);

            this.gun=new SSCGuns.SSCGun(new StardustCore.Animations.AnimatedSprite("MyFirstGun",this.position,new AnimationManager(SeasideScramble.self.textureUtils.getExtendedTexture("Guns","BasicGun"),new Animation(0,0,16,16)),Color.White), SeasideScramble.self.projectiles.getDefaultProjectile(this, this.position, Vector2.Zero, 1f, new Rectangle(0, 0, 16, 16), Color.White, 4f, 300),10,1000,3000);

            this.hitBox = new Rectangle((int)this.position.X, (int)this.position.Y, 64, 64);
            this.HUD = new SSCMenus.HUD.CharacterHUD(100, 20, 100, 100, this.playerID);

            this.maxHealth = 100;
            this.currentHealth = 100;
        }

        /// <summary>
        /// Sets the color for the player.
        /// </summary>
        /// <param name="color"></param>
        public void setColor(Color color)
        {
            this.playerColor = color;
            this.mouseCursor.color = color;
        }

        /// <summary>
        /// Plays an animation for the character.
        /// </summary>
        /// <param name="name"></param>
        public void playAnimation(string name)
        {
            this.characterSpriteController.setAnimation(name);
        }

        /// <summary>
        /// Draws the character to the screen.
        /// </summary>
        /// <param name="b"></param>
        public void draw(Microsoft.Xna.Framework.Graphics.SpriteBatch b)
        {
            this.draw(b, this.position);
        }

        /// <summary>
        /// Draws the character to the screen.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="position"></param>
        public void draw(SpriteBatch b, Vector2 position)
        {
            this.characterSpriteController.draw(b, SeasideScramble.GlobalToLocal(SeasideScramble.self.camera.viewport, position), this.playerColor, 4f, this.flipSprite == true ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, (this.position.Y) / 10000f));
            this.gun.draw(b, SeasideScramble.GlobalToLocal(SeasideScramble.self.camera.viewport, position),2f);
        }
        public void drawMouse(SpriteBatch b)
        {
            this.mouseCursor.draw(b, 4f, 0f);
        }
        public void drawHUD(SpriteBatch b)
        {
            this.HUD.draw(b);
        }

        /// <summary>
        /// Called every frame to do update logic.
        /// </summary>
        /// <param name="Time"></param>
        public void update(GameTime Time)
        {
            this.movedThisFrame = false;
            if (this.isMoving == false)
            {
                if (this.facingDirection == SSCEnums.FacingDirection.Down)
                {
                    this.characterSpriteController.playAnimation("Idle_F");
                }
                if (this.facingDirection == SSCEnums.FacingDirection.Right)
                {
                    this.characterSpriteController.playAnimation("Idle_R");
                }
                if (this.facingDirection == SSCEnums.FacingDirection.Left)
                {
                    this.characterSpriteController.playAnimation("Idle_L");
                    this.flipSprite = true;
                    return;
                }
                if (this.facingDirection == SSCEnums.FacingDirection.Up)
                {
                    this.characterSpriteController.playAnimation("Idle_B");
                }
                this.flipSprite = false;
            }
            else
            {
                if (this.facingDirection == SSCEnums.FacingDirection.Down)
                {
                    this.characterSpriteController.playAnimation("Walking_F");
                }
                if (this.facingDirection == SSCEnums.FacingDirection.Right)
                {
                    this.characterSpriteController.playAnimation("Walking_R");
                }
                if (this.facingDirection == SSCEnums.FacingDirection.Left)
                {
                    this.characterSpriteController.playAnimation("Walking_L");
                    this.flipSprite = true;
                    return;
                }
                if (this.facingDirection == SSCEnums.FacingDirection.Up)
                {
                    this.characterSpriteController.playAnimation("Walking_B");
                }
                this.flipSprite = false;
            }

            if (this.playerID == SSCEnums.PlayerID.One)
            {
                if (SeasideScramble.self.getMouseDelta().X != 0 || SeasideScramble.self.getMouseDelta().Y != 0)
                {
                    this.mouseCursor.position = new Vector2(Game1.getMousePosition().X, Game1.getMousePosition().Y);
                    this.showMouseCursor = true;
                }
            }
            if (this.currentHealth < 0) this.currentHealth = 0;

            this.gun.update(Time);
            this.HUD.update(Time);
        }

        /// <summary>
        /// Sets the mouse's position.
        /// </summary>
        /// <param name="position"></param>
        public void setMousePosition(Vector2 position)
        {
            this.mouseCursor.position = position;
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        //           Input logic            //
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

        #region

        /// <summary>
        /// Checks when the gamepad receives input.
        /// </summary>
        /// <param name="state"></param>
        public void receiveGamepadInput(GamePadState state)
        {
            if (SeasideScramble.self.menuManager.isMenuUp == false)
            {
                //Do gamepad input here!
                if (state.ThumbSticks.Left.X < 0)
                {
                    this.movePlayer(SSCEnums.FacingDirection.Left);
                }
                else if (state.ThumbSticks.Left.X > 0)
                {
                    this.movePlayer(SSCEnums.FacingDirection.Right);
                }
                if (state.ThumbSticks.Left.Y < 0)
                {
                    this.movePlayer(SSCEnums.FacingDirection.Down);
                }
                else if (state.ThumbSticks.Left.Y > 0)
                {
                    this.movePlayer(SSCEnums.FacingDirection.Up);
                }
                if (state.ThumbSticks.Left.X == 0 && state.ThumbSticks.Left.Y == 0 && this.movedThisFrame == false)
                {
                    this.isMoving = false;
                }

                if (state.ThumbSticks.Right.X != 0 || state.ThumbSticks.Right.Y != 0)
                {
                    Vector2 direction = new Vector2(state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y * -1);
                    Vector2 unit = direction.UnitVector();
                    this.shoot(unit);
                    //this.moveMouseCursor(state.ThumbSticks.Right);
                }
            }
            else
            {
                if (state.ThumbSticks.Right.X != 0 || state.ThumbSticks.Right.Y != 0)
                {
                    this.moveMouseCursor(new Vector2(state.ThumbSticks.Right.X,state.ThumbSticks.Right.Y*-1));
                    this.showMouseCursor = true;
                }
            }
        }

        /// <summary>
        /// Move the mouse cursor.
        /// </summary>
        /// <param name="direction"></param>
        private void moveMouseCursor(Vector2 direction)
        {
            if (SeasideScramble.self.camera.positionInsideViewport(this.mouseCursor.position + new Vector2(direction.X * this.mouseSensitivity.X, direction.Y * this.mouseSensitivity.Y)))
            {
                this.mouseCursor.position += new Vector2(direction.X*this.mouseSensitivity.X,direction.Y*this.mouseSensitivity.Y);
            }
            else if (SeasideScramble.self.camera.positionInsideViewport(this.mouseCursor.position + new Vector2(direction.X*this.mouseSensitivity.X, 0)))
            {
                this.mouseCursor.position += new Vector2(direction.X * this.mouseSensitivity.X, 0);
            }
            else if (SeasideScramble.self.camera.positionInsideViewport(this.mouseCursor.position + new Vector2(0, direction.Y*this.mouseSensitivity.Y)))
            {
                this.mouseCursor.position += new Vector2(0, direction.Y*this.mouseSensitivity.Y);
            }
        }

        /// <summary>
        /// Gets a normalized direction vector for the player.
        /// </summary>
        /// <returns></returns>
        private Vector2 getMouseDirection()
        {
            Vector2 dir = this.getRelativeMouseFromPlayer();
            dir = dir.UnitVector();
            return dir;
        }

        /// <summary>
        /// Gets the mouse's relative position away from the player.
        /// </summary>
        /// <returns></returns>
        private Vector2 getRelativeMouseFromPlayer()
        {
            Vector2 pos = this.mouseCursor.position - SeasideScramble.GlobalToLocal(this.position);
            return pos;
        }
        

        /// <summary>
        /// Checks when the player presses a key on the keyboard.
        /// </summary>
        /// <param name="k"></param>
        public void receiveKeyPress(Microsoft.Xna.Framework.Input.Keys k)
        {
            if (this.playerID == SSCEnums.PlayerID.One)
            {
                this.checkForMovementInput(k);
            }
        }

        /// <summary>
        /// Triggers when there isn't a key being pressed.
        /// </summary>
        /// <param name="K"></param>
        public void receiveKeyRelease(Keys K)
        {
            if (this.playerID != SSCEnums.PlayerID.One) return;
            //throw new NotImplementedException();
            if (K == Keys.A)
            {
                this.isMoving = false;
            }
            if (K == Keys.W)
            {
                this.isMoving = false;
            }
            if (K == Keys.S)
            {
                this.isMoving = false;
            }
            if (K == Keys.D)
            {
                this.isMoving = false;
            }
        }

        /// <summary>
        /// What happens when the player left clicks.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void receiveLeftClick(int x, int y)
        {
            if (SeasideScramble.self.getGamepadState(PlayerIndex.One).IsButtonDown(Buttons.A))
            {
                //Do stuff besides shooting.
                return;
            }
            Vector2 direction = this.getMouseDirection();
            this.shoot(direction);
        }

        #endregion

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        //           Movement logic         //
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        #region

        /// <summary>
        /// Checks for moving the player.
        /// </summary>
        /// <param name="direction"></param>
        public void movePlayer(SSCEnums.FacingDirection direction)
        {
            this.isMoving = true;
            this.movedThisFrame = true;
            if (direction == SSCEnums.FacingDirection.Up)
            {
                this.facingDirection = direction;
                this.position += new Vector2(0, -1);
            }
            if (direction == SSCEnums.FacingDirection.Down)
            {
                this.facingDirection = direction;
                this.position += new Vector2(0, 1);
            }
            if (direction == SSCEnums.FacingDirection.Left)
            {
                this.facingDirection = direction;
                this.position += new Vector2(-1, 0);
            }
            if (direction == SSCEnums.FacingDirection.Right)
            {
                this.facingDirection = direction;
                this.position += new Vector2(1, 0);
            }
            this.hitBox.X = (int)this.position.X;
            this.hitBox.Y = (int)this.position.Y;
            //ModCore.log(this.position);
        }

        /// <summary>
        /// Checks for player movement.
        /// </summary>
        /// <param name="K"></param>
        private void checkForMovementInput(Keys K)
        {
            if (this.playerID != SSCEnums.PlayerID.One) return;
            if (SeasideScramble.self.menuManager.isMenuUp) return;
            //Microsoft.Xna.Framework.Input.GamePadState state = this.getGamepadState(PlayerIndex.One);
            if (K == Keys.A)
            {
                this.movePlayer(SSCEnums.FacingDirection.Left);
            }
            if (K == Keys.W)
            {
                this.movePlayer(SSCEnums.FacingDirection.Up);
            }
            if (K == Keys.S)
            {
                this.movePlayer(SSCEnums.FacingDirection.Down);
            }
            if (K == Keys.D)
            {
                this.movePlayer(SSCEnums.FacingDirection.Right);
            }
        }

        #endregion

        private void shoot(Vector2 direction)
        {
            if (SeasideScramble.self.menuManager.isMenuUp) return;
            //ModCore.log("Shoot: " + direction);
            //SeasideScramble.self.projectiles.spawnDefaultProjectile(this, this.position, direction, 1f, new Rectangle(0, 0, 16, 16), Color.White, 4f, 300);

            this.gun.tryToShoot(this.position, direction);
            
        }

        public void takeDamage(int amount)
        {
            this.currentHealth -= amount;
            if (this.currentHealth < 0) {
                this.currentHealth = 0;
            }
        }

        public void heal(int amount)
        {
            this.takeDamage(amount * -1);
        }

        public void healToFull()
        {
            this.currentHealth = this.maxHealth;
        }

    }
}
