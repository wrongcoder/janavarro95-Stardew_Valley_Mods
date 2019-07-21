using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Revitalize.Framework.Minigame.SeasideScrambleMinigame.SSCProjectiles
{
    public class SSCProjectileManager
    {
        public List<SSCProjectile> projectiles;
        private List<SSCProjectile> garbageCollection;

        public SSCProjectileManager()
        {
            this.projectiles = new List<SSCProjectile>();
            this.garbageCollection = new List<SSCProjectile>();
        }

        public void addProjectile(SSCProjectile projectile)
        {
            this.projectiles.Add(projectile);
        }

        public void deleteProjectile(SSCProjectile projectile)
        {
            this.garbageCollection.Add(projectile);
            //this.projectiles.Remove(projectile);
        }

        public void update(GameTime Time)
        {
            foreach(SSCProjectile p in this.garbageCollection)
            {
                this.projectiles.Remove(p);
            }
            this.garbageCollection.Clear();

            foreach(SSCProjectile p in this.projectiles)
            {
                p.update(Time);

                //Do collision checking.
                foreach(SSCPlayer player in SeasideScramble.self.players.Values)
                {
                    if (p.collidesWith(player.position))
                    {
                        p.onCollision(player);
                    }
                }
            }
        }

        public void draw(SpriteBatch b)
        {
            foreach (SSCProjectile p in this.projectiles)
            {             
                p.draw(b);
            }
        }

        //~~~~~~~~~~~~~~~~~~~~//
        //   Spawning Logic   //
        //~~~~~~~~~~~~~~~~~~~~//
        #region

        public void spawnDefaultProjectile(object Owner,Vector2 Position,Vector2 Direction,float Speed,Rectangle HitBox,Color Color,float Scale,int LifeSpan=300)
        {

            SSCProjectile basic = new SSCProjectile(Owner, new StardustCore.Animations.AnimatedSprite("DefaultProjectile", Position, new StardustCore.Animations.AnimationManager(SeasideScramble.self.textureUtils.getExtendedTexture("Projectiles", "Basic"), new StardustCore.Animations.Animation(0, 0, 4, 4)), Color), HitBox, Position, Direction, Speed, LifeSpan, Scale);
            this.addProjectile(basic);
        }

        #endregion
    }
}
