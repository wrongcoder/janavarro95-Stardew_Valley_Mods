using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Revitalize.Framework.Minigame.SeasideScrambleMinigame.Interfaces
{
    public interface ISpawner
    {

        void update(GameTime Time);
        void draw(SpriteBatch b);
        bool enabled
        {
            get;set;
        }

    }
}
