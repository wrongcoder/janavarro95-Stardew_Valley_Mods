using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Revitalize.Framework.Minigame.SeasideScrambleMinigame.Interfaces
{
    public interface ISSCLivingEntity
    {
        float MovementSpeed
        {
            get;
            set;
        }
        int CurrentHealth
        {
            get;
            set;
        }
        int MaxHealth
        {
            get;
            set;
        }
        Rectangle HitBox
        {
            get;
            set;
        }

    }
}
