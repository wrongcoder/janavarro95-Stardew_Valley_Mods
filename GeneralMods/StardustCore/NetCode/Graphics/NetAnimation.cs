using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Netcode;
using StardustCore.Animations;

namespace StardustCore.NetCode.Graphics
{
    public class NetAnimation : Netcode.NetField<Animations.Animation, NetAnimation>
    {

        public NetRectangle sourceRect;
        public NetInt frameDuration;
        public NetInt frameDurationUntilNextAnimation;

        public NetAnimation()
        {

        }
        public NetAnimation(Animations.Animation animation) : base(animation)
        {

        }

        public override void Set(Animation newValue)
        {
            throw new NotImplementedException();
        }

        protected override void ReadDelta(BinaryReader reader, NetVersion version)
        {
            sourceRect = new NetRectangle();
            sourceRect.Read(reader, version);
            Value.sourceRectangle = sourceRect.Value;

            frameDuration = new NetInt();
            frameDuration.Read(reader, version);
            Value.frameDuration = frameDuration.Value;

            frameDurationUntilNextAnimation = new NetInt();
            frameDurationUntilNextAnimation.Read(reader, version);
            Value.frameDuration = frameDuration.Value;
        }

        protected override void WriteDelta(BinaryWriter writer)
        {
            sourceRect = new NetRectangle(Value.sourceRectangle);
            sourceRect.Write(writer);

            frameDuration = new NetInt(Value.frameDuration);
            frameDuration.Write(writer);

            frameDurationUntilNextAnimation = new NetInt(Value.frameCountUntilNextAnimation);
            frameDurationUntilNextAnimation.Write(writer);
        }
    }
}
