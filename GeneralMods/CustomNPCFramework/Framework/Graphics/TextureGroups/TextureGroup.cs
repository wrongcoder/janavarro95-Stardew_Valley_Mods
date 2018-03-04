using CustomNPCFramework.Framework.Enums;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNPCFramework.Framework.Graphics.TextureGroups
{
    public class TextureGroup
    {
        public DirectionalTexture standingTexture;
        public DirectionalTexture sittingTexture;
        public DirectionalTexture swimmingTexture;
        public DirectionalTexture movingTexture;

        public DirectionalTexture currentTexture;

        private AssetInfo info;
        private string path;
        private Direction dir;
        private AnimationType type;

        public TextureGroup(AssetInfo info, string path,Direction direction ,AnimationType animationType=AnimationType.standing)
        {
            this.standingTexture = new DirectionalTexture(info.standingAssetPaths, path, direction);
            this.sittingTexture = new DirectionalTexture(info.sittingAssetPaths, path, direction);
            this.swimmingTexture = new DirectionalTexture(info.swimmingAssetPaths, path, direction);
            this.movingTexture = new DirectionalTexture(info.movingAssetPaths, path, direction);

            this.info = info;
            this.path = path;
            this.dir = direction;
            this.type = animationType;

            if (animationType == AnimationType.standing) this.currentTexture = standingTexture;
            if (animationType == AnimationType.sitting) this.currentTexture = sittingTexture;
            if (animationType == AnimationType.swimming) this.currentTexture = swimmingTexture;
            if (animationType == AnimationType.walking) this.currentTexture = movingTexture;

        }

        public TextureGroup clone()
        {
            return new TextureGroup(this.info, this.path, this.dir, this.type);
        }


        public virtual void setLeft()
        {
            this.movingTexture.setLeft();
            this.sittingTexture.setLeft();
            this.standingTexture.setLeft();
            this.swimmingTexture.setLeft();
        }

        public virtual void setUp()
        {
            this.movingTexture.setUp();
            this.sittingTexture.setUp();
            this.standingTexture.setUp();
            this.swimmingTexture.setUp();
        }

        public virtual void setDown()
        {
            this.movingTexture.setDown();
            this.sittingTexture.setDown();
            this.standingTexture.setDown();
            this.swimmingTexture.setDown();
        }

        public virtual void setRight()
        {
            this.movingTexture.setRight();
            this.sittingTexture.setRight();
            this.standingTexture.setRight();
            this.swimmingTexture.setRight();
        }

        public virtual DirectionalTexture getTextureFromAnimation(AnimationType type)
        {
            if (type == AnimationType.standing) return this.standingTexture;
            if (type == AnimationType.walking) return this.movingTexture;
            if (type == AnimationType.swimming) return this.swimmingTexture;
            if (type == AnimationType.sitting) return this.sittingTexture;
            return null;
        }

    }
}
