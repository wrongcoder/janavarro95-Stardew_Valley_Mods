using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.StardustCore.Animations;

namespace Omegasis.Revitalize.Framework.Content.JsonContent.Animations
{
    public class JsonAnimation
    {

        public List<JsonAnimationFrame> animationFrames = new List<JsonAnimationFrame>();
        public bool shouldAnimationLoopWhenFinished = false;

        public JsonAnimation()
        {
            this.animationFrames = new List<JsonAnimationFrame>();
            this.animationFrames.Add(new JsonAnimationFrame());
        }

        public virtual Animation toAnimation()
        {
            List<AnimationFrame> frames = new List<AnimationFrame>();
            foreach(JsonAnimationFrame frame in this.animationFrames)
            {
                frames.Add(frame.toAnimationFrame());
            }
            return new Animation(frames, this.shouldAnimationLoopWhenFinished);
        }
    }
}
