using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardustCore.Animations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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



            },"Idle_F",0,true);
        }

        public void playAnimation(string name)
        {
            this.characterSpriteController.setAnimation(name);
        }

        public void draw(Microsoft.Xna.Framework.Graphics.SpriteBatch b)
        {
            this.characterSpriteController.draw(b, SeasideScramble.GlobalToLocal(SeasideScramble.self.camera.viewport,this.position), Color.White, 4f, this.flipSprite == true ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, (this.position.Y) / 10000f));
        }

        public void update(GameTime Time)
        {
            if (this.isMoving == false)
            {
                if(this.facingDirection== SSCEnums.FacingDirection.Down)
                {
                    this.characterSpriteController.setAnimation("Idle_F");
                }
                if (this.facingDirection == SSCEnums.FacingDirection.Right)
                {
                    this.characterSpriteController.setAnimation("Idle_R");
                }
                if (this.facingDirection == SSCEnums.FacingDirection.Left)
                {
                    this.characterSpriteController.setAnimation("Idle_L");
                    this.flipSprite = true;
                    return;
                }
                if (this.facingDirection == SSCEnums.FacingDirection.Up)
                {
                    this.characterSpriteController.setAnimation("Idle_B");
                }
                this.flipSprite = false;
            }
            else
            {
                if (this.facingDirection == SSCEnums.FacingDirection.Down)
                {
                    this.characterSpriteController.setAnimation("Idle_F");
                }
                if (this.facingDirection == SSCEnums.FacingDirection.Right)
                {
                    this.characterSpriteController.setAnimation("Idle_R");
                }
                if (this.facingDirection == SSCEnums.FacingDirection.Left)
                {
                    this.characterSpriteController.setAnimation("Idle_L");
                    this.flipSprite = true;
                    return;
                }
                if (this.facingDirection == SSCEnums.FacingDirection.Up)
                {
                    this.characterSpriteController.setAnimation("Idle_B");
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

       
        

    }
}
