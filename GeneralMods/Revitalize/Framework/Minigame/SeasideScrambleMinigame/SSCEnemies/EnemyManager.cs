using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Revitalize.Framework.Minigame.SeasideScrambleMinigame.SSCEnemies
{
    public class EnemyManager
    {
        public List<SSCEnemy> enemies;
        private List<SSCEnemy> garbageCollection;

        /// <summary>
        /// Constructor.
        /// </summary>
        public EnemyManager()
        {
            this.enemies = new List<SSCEnemy>();
            this.garbageCollection = new List<SSCEnemy>();
        }

        /// <summary>
        /// Adds an enemy to the game.
        /// </summary>
        /// <param name="enemy"></param>
        public void addEnemy(SSCEnemy enemy)
        {
            this.enemies.Add(enemy);
        }
        /// <summary>
        /// Removes an enemy from the game.
        /// </summary>
        /// <param name="enemy"></param>
        public void removeEnemy(SSCEnemy enemy)
        {
            this.garbageCollection.Add(enemy);
        }

        /// <summary>
        /// Update all enemies.
        /// </summary>
        /// <param name="time"></param>
        public void update(GameTime time)
        {
            foreach(SSCEnemy enemy in this.garbageCollection)
            {
                this.enemies.Remove(enemy);
            }
            foreach(SSCEnemy enemy in this.enemies)
            {
                enemy.update(time);
                if (enemy.shouldDie) this.removeEnemy(enemy);
            }
        }

        /// <summary>
        /// Draw all enemies to the screen.
        /// </summary>
        /// <param name="b"></param>
        public void draw(SpriteBatch b)
        {
            foreach (SSCEnemy enemy in this.enemies)
            {
                enemy.draw(b);
            }
        }

    }
}
