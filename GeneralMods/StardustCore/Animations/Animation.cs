using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardustCore.Animations
{
   public class Animation
    {
       public Rectangle sourceRectangle;
       public readonly int frameDuration; 
       public int frameCountUntilNextAnimation;

        public Animation(Rectangle rec,int existForXFrames)
        {
            sourceRectangle = rec;
            frameDuration = existForXFrames;
        }

        public void tickAnimationFrame()
        {
            frameCountUntilNextAnimation--;
        }

        public void startAnimation()
        {
            frameCountUntilNextAnimation = frameDuration;
        }


    }
}
