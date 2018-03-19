using CustomNPCFramework.Framework.Enums;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNPCFramework.Framework.Graphics.TextureGroups
{
    /// <summary>
    /// A group of a textures used to hold all of the textures associated with a single asset such as a hair style or a shirt.
    /// </summary>
    public class TextureGroup
    {
        /// <summary>
        /// The directional (Left, Right, Up, Down) textures to be used when the NPC is standing.
        /// </summary>
        public DirectionalTexture standingTexture;
        /// <summary>
        /// The directional (Left, Right, Up, Down) textures to be used when the NPC is sitting.
        /// </summary>
        public DirectionalTexture sittingTexture;
        /// <summary>
        /// The directional (Left, Right, Up, Down) textures to be used when the NPC is swimming.
        /// </summary>
        public DirectionalTexture swimmingTexture;
        /// <summary>
        /// The directional (Left, Right, Up, Down) textures to be used when the NPC is moving.
        /// </summary>
        public DirectionalTexture movingTexture;

        /// <summary>
        /// The current directional texture to be used by the npc. Can be things such as the standing, swimming, moving, or sitting texture.
        /// </summary>
        public DirectionalTexture currentTexture;

        /// <summary>
        /// Asset info loaded in from the corresponding .json file. 
        /// </summary>
        private AssetInfo info;
        /// <summary>
        /// The path to the .json file.
        /// </summary>
        private string path;
        /// <summary>
        /// The current direction of the texture group. See Direction.cs
        /// </summary>
        private Direction dir;
        /// <summary>
        /// The type of asset this is. Body, hair, eyes, shirt,etc...
        /// </summary>
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
