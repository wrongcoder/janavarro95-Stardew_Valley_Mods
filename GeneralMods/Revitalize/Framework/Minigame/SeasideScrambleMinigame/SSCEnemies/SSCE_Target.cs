using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Revitalize.Framework.Minigame.SeasideScrambleMinigame.SSCProjectiles;
using StardustCore.Animations;

namespace Revitalize.Framework.Minigame.SeasideScrambleMinigame.SSCEnemies
{
    public class SSCE_Target:SSCEnemies.SSCEnemy
    {

        private bool targetHit;

        public SSCE_Target():base()
        {

        }

        public SSCE_Target(AnimatedSprite Sprite, int MoveSpeed, int MaxHealth, Vector2 HitBoxDimensions,float Scale):base(Sprite,MoveSpeed,MaxHealth,HitBoxDimensions,Scale)
        {

        }

        public override void die()
        {
            this.playDeathAnimation();
        }

        public void playDeathAnimation()
        {
            this.sprite.animation.playAnimationOnce("Die");
            this.targetHit = true;
        }

        public override void update(GameTime time)
        {
            if(this.sprite.animation.IsAnimationPlaying==false && this.targetHit)
            {
                this.shouldDie = true;
            }
        }

        public override void onCollision(SSCProjectile other)
        {
            if (other is SSCProjectiles.SSCProjectile)
            {
                this.CurrentHealth -= other.damage;
                this.die();
            }
        }


        public static void Spawn_SSCE_Target(Vector2 Position,Color Color)
        {
            SSCE_Target target = new SSCE_Target(new AnimatedSprite("TargetPractice", Position, new AnimationManager(SeasideScramble.self.textureUtils.getExtendedTexture("Enemies", "Target"), new Animation(0, 0, 16, 16), new Dictionary<string, List<Animation>>() {
                { "None",new List<Animation>(){
                    new Animation(0,0,16,16)
                } },
                {"Die",new List<Animation>()
                {
                    new Animation(0,0,16,16,20),
                    new Animation(16,0,16,16,20),
                    new Animation(32,0,16,16,20)
                }
                }

            },"None"), Color), 0, 1, new Vector2(16,16),4f);
            SeasideScramble.self.enemies.addEnemy(target);
        }
    }
}
