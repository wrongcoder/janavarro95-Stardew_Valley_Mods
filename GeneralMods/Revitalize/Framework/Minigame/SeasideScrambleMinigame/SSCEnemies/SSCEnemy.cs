using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Revitalize.Framework.Minigame.SeasideScrambleMinigame.Interfaces;
using StardustCore.Animations;

namespace Revitalize.Framework.Minigame.SeasideScrambleMinigame.SSCEnemies
{
    public class SSCEnemy : ISSCLivingEntity
    {
        public float MovementSpeed { get; set; }
        public int CurrentHealth { get; set; }
        public int MaxHealth { get; set; }
        public Rectangle HitBox { get; set; }

        public AnimatedSprite sprite;
        public bool shouldDie;
        public float scale;

        public SSCStatusEffects.StatusEffectManager statusEffects;

        public Vector2 Position
        {
            get
            {
                return this.sprite.position;
            }
        }

        public SSCEnemy()
        {

        }

        public SSCEnemy(AnimatedSprite Sprite,int MoveSpeed, int MaxHealth,Vector2 hitBoxDimensions,float Scale)
        {
            this.sprite = Sprite;
            this.MovementSpeed = MoveSpeed;
            this.MaxHealth = MaxHealth;
            this.HitBox = new Rectangle((int)this.sprite.position.X, (int)this.sprite.position.Y, (int)(hitBoxDimensions.X * Scale),(int)(hitBoxDimensions.Y * Scale));
            this.CurrentHealth = MaxHealth;
            this.scale = Scale;
            this.statusEffects = new SSCStatusEffects.StatusEffectManager(this);
        }

        public virtual void update(GameTime time)
        {

        }
        public virtual void draw(SpriteBatch b)
        {
            this.sprite.draw(b, SeasideScramble.GlobalToLocal(SeasideScramble.self.camera.viewport, this.Position), this.scale,0f);
        }

        public virtual void draw(SpriteBatch b, Vector2 Position, float Scale)
        {
            this.sprite.draw(b,Position, Scale, 0f);
        }

        public virtual void die()
        {

        }

        public virtual void onCollision(SSCProjectiles.SSCProjectile other)
        {

        }

    }
}
