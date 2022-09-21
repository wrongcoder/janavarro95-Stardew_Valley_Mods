using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Omegasis.StardustCore.Animations;

namespace Omegasis.Revitalize.Framework.Content.JsonContent.Animations
{
    public class JsonAnimationFrame
    {

        public Rectangle animationSourceRectangle = new Rectangle();
        public int drawForXFrames = 0;

        public JsonAnimationFrame()
        {

        }

        public virtual AnimationFrame toAnimationFrame()
        {
            return new AnimationFrame(this.animationSourceRectangle, this.drawForXFrames);
        }
    }
}
