using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardustCore.Animations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace Revitalize.Framework.Minigame.SeasideScrambleMinigame
{
    public class SSCPlayer
    {
        //TODO: Hint when left animations are played make sure to FLIP THE SPRITE;
        //Make game camera class!!!
        public AnimationManager characterSpriteController;
        public bool flipSprite;
        public SSCEnums.FacingDirection facingDirection;
        public Microsoft.Xna.Framework.Vector2 position;
        public bool isMoving;
        public Color playerColor;

        public const int junimoWalkingAnimationSpeed = 10;

        public SSCPlayer()
        {
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




            },"Idle_F",0,true);
        }

        public void setColor(Color color)
        {
            this.playerColor = color;
        }

        public void playAnimation(string name)
        {
            this.characterSpriteController.setAnimation(name);
        }

        public void draw(Microsoft.Xna.Framework.Graphics.SpriteBatch b)
        {
            this.characterSpriteController.draw(b, SeasideScramble.GlobalToLocal(SeasideScramble.self.camera.viewport,this.position), this.playerColor, 4f, this.flipSprite == true ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, (this.position.Y) / 10000f));
        }

        public void update(GameTime Time)
        {
            if (this.isMoving == false)
            {
                if(this.facingDirection== SSCEnums.FacingDirection.Down)
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
        }

        /// <summary>
        /// Checks for moving the player.
        /// </summary>
        /// <param name="direction"></param>
        public void movePlayer(SSCEnums.FacingDirection direction)
        {
            this.isMoving = true;
            if(direction== SSCEnums.FacingDirection.Up)
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
            ModCore.log(this.position);
        }

        public void receiveKeyPress(Microsoft.Xna.Framework.Input.Keys k)
        {
            this.checkForMovementInput(k);
        }
        public void receiveKeyRelease(Keys K)
        {
            //throw new NotImplementedException();
            if (K == Keys.A)
            {
                ModCore.log("A released for Seaside Scramble!");
                this.isMoving = false;
            }
            if (K == Keys.W)
            {
                ModCore.log("W pressed for Seaside Scramble!");
                this.isMoving = false;
            }
            if (K == Keys.S)
            {
                ModCore.log("S pressed for Seaside Scramble!");
                this.isMoving = false;
            }
            if (K == Keys.D)
            {
                ModCore.log("D pressed for Seaside Scramble!");
                this.isMoving = false;
            }
        }

        /// <summary>
        /// Checks for player movement.
        /// </summary>
        /// <param name="K"></param>
        private void checkForMovementInput(Keys K)
        {
            if (SeasideScramble.self.isMenuUp) return;
            //Microsoft.Xna.Framework.Input.GamePadState state = this.getGamepadState(PlayerIndex.One);
            if (K == Keys.A)
            {
                ModCore.log("A pressed for player");
                this.movePlayer(SSCEnums.FacingDirection.Left);
            }
            if (K == Keys.W)
            {
                ModCore.log("W pressed for player!");
                this.movePlayer(SSCEnums.FacingDirection.Up);
            }
            if (K == Keys.S)
            {
                ModCore.log("S pressed for player!");
                this.movePlayer(SSCEnums.FacingDirection.Down);
            }
            if (K == Keys.D)
            {
                ModCore.log("D pressed for player!");
                this.movePlayer(SSCEnums.FacingDirection.Right);
            }



        }




    }
}
