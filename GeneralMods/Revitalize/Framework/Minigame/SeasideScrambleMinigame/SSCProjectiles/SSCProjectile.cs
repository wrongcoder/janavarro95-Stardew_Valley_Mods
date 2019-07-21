using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardustCore.Animations;

namespace Revitalize.Framework.Minigame.SeasideScrambleMinigame.SSCProjectiles
{
    public class SSCProjectile
    {
        public AnimatedSprite sprite;
        public Vector2 direction;
        public float speed;
        public float scale;
        public Rectangle hitBox;
        public Vector2 position
        {
            get
            {
                return this.sprite.position;
            }
            set
            {
                this.sprite.position = value;
            }
        }
        public Color color
        {
            get
            {
                return this.sprite.color;
            }
            set
            {
                this.sprite.color = value;
            }
        }

        public int maxLifeSpan;
        public int currentLifeSpan;

        /// <summary>
        /// The object that spawned this projectile.
        /// </summary>
        public object owner;

        public Vector2 Velocity
        {
            get
            {
                return this.direction * this.speed;
            }
        }

        public SSCProjectile()
        {

        }
        public SSCProjectile(object Owner,AnimatedSprite Sprite,Rectangle HitBox ,Vector2 Position,Vector2 Direction, float Speed, int LifeSpan ,float Scale)
        {
            this.sprite = Sprite;
            this.hitBox = HitBox;
            this.direction = Direction;
            this.speed = Speed;
            this.position = Position;
            this.scale = Scale;
            this.maxLifeSpan = LifeSpan;
            this.currentLifeSpan = LifeSpan;
            this.owner = Owner;
        }

        /// <summary>
        /// Update the projectile.
        /// </summary>
        /// <param name="time"></param>
        public virtual void update(GameTime time)
        {
            this.tickLifeSpan();
            this.updateMovement();
        }

        /// <summary>
        /// Update the movement for the projectile.
        /// </summary>
        public virtual void updateMovement()
        {
            this.position += this.Velocity;
        }

        /// <summary>
        /// Tick the lifespan of the projectile.
        /// </summary>
        public virtual void tickLifeSpan()
        {
            if (this.currentLifeSpan <= 0)
            {
                this.die();
            }
            else
            {
                this.currentLifeSpan--;
            }
        }
        /// <summary>
        /// What happens when this projectile dies.
        /// </summary>
        public virtual void die()
        {
            //Make projectile manager that handles deleting this projectile.
            //Make projectile manager have a pool of projectiles????
            ModCore.log("Projectile has died.");
            SeasideScramble.self.projectiles.deleteProjectile(this);
        }

        /// <summary>
        /// Draw the projectile.
        /// </summary>
        /// <param name="b"></param>
        public virtual void draw(SpriteBatch b)
        {
            this.sprite.draw(b, SeasideScramble.GlobalToLocal(SeasideScramble.self.camera.viewport, this.position), this.scale, 0.5f);
        }

        /// <summary>
        /// Checks if the projectile collides with something.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool collidesWith(Vector2 position)
        {
            return this.hitBox.Contains(new Point((int)position.X,(int)position.Y));
        }
        public bool collidesWith(Rectangle rec)
        {
            return this.hitBox.Intersects(rec);
        }

        public virtual void collisionLogic()
        {
            //Do something I guess.
            this.die();
        }

        public virtual void onCollision(object other)
        {
            if(other is SSCPlayer)
            {
                if (this.hasOwner())
                {
                    if (this.owner == other)
                    {
                        ModCore.log("Can't get hit by own projectile.");
                        return;
                    }
                }
                ModCore.log("Big oof. Player hit by projectile.");
                this.collisionLogic();
            }
        }

        /// <summary>
        /// Checks if this projectile has an owner in the weird case I spawn some without owners.
        /// </summary>
        /// <returns></returns>
        public bool hasOwner()
        {
            return this.owner != null;
        }

    }
}
