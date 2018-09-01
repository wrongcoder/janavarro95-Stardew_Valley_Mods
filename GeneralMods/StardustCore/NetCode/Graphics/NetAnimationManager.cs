using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Netcode;
using StardewValley;
using StardewValley.Network;

namespace StardustCore.NetCode.Graphics
{
    public class NetAnimationManager : Netcode.NetField<Animations.AnimationManager,NetAnimationManager>
    {

        public NetAnimationManager()
        {

        }

        public NetAnimationManager(Animations.AnimationManager manager): base(manager)
        {

        }



        protected override void ReadDelta(BinaryReader reader, NetVersion version)
        {
            NetString currentAnimationName = new NetString();
            currentAnimationName.Read(reader, version);

            NetInt currentIndex = new NetInt();
            currentIndex.Read(reader, version);

            NetTexture2DExtended text = new NetTexture2DExtended();
            text.Read(reader, version);

            NetAnimation defaultAnimation = new NetAnimation();
            defaultAnimation.Read(reader, version);

            NetBool enabled = new NetBool();
            enabled.Read(reader, version);

            NetString data = new NetString();
            data.Read(reader, version);

            Value.setExtendedTexture(text.Value);
            Value.defaultDrawFrame = defaultAnimation.Value;
            Value.enabled = enabled.Value;
            //Try and prevent unnecessary parsing.
            if (Value.animations == null && !String.IsNullOrEmpty(Value.animationDataString))
            {
                Value.animations = Animations.AnimationManager.parseAnimationsFromXNB(data.Value);
            }
            if (!String.IsNullOrEmpty(data.Value))
            {
                Value.setAnimation(currentAnimationName.Value, currentIndex.Value);
            }
            else
            {
                Value.currentAnimation = defaultAnimation.Value;
            }
        }

        protected override void WriteDelta(BinaryWriter writer)
        {
           NetString currentAnimationName = new NetString(Value.currentAnimationName);
           currentAnimationName.Write(writer);
            

            NetInt currentAnimationListIndex = new NetInt(Value.currentAnimationListIndex);
            currentAnimationListIndex.Write(writer);

            NetTexture2DExtended texture = new NetTexture2DExtended(Value.getExtendedTexture());
            texture.Write(writer);

            NetAnimation defaultDrawFrame = new NetAnimation(Value.defaultDrawFrame);
            defaultDrawFrame.Write(writer);

            NetBool enabled = new NetBool(Value.enabled);
            enabled.Write(writer);

            NetString animationData = new NetString(Value.animationDataString);
            animationData.Write(writer);
            
        }
    }
}
