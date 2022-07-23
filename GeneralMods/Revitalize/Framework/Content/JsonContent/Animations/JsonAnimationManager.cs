using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.Managers;
using Omegasis.StardustCore.Animations;

namespace Omegasis.Revitalize.Framework.Content.JsonContent.Animations
{
    public class JsonAnimationManager
    {
        public string modManifestId = "";
        public string textureManagerId = "";
        public string textureId = "";

        public string defaultAnimationKey = "";
        public string startingAnimationKey = "";

        public Dictionary<string, JsonAnimation> animations = new Dictionary<string, JsonAnimation>();


        public JsonAnimationManager()
        {
            this.animations = new Dictionary<string, JsonAnimation>();
            this.animations.Add("Template", new JsonAnimation());
        }

        public virtual AnimationManager toAnimationManager()
        {

            Dictionary<string, Animation> stardustCoreAnimations = new Dictionary<string, Animation>();

            foreach(KeyValuePair<string,JsonAnimation> pair in this.animations)
            {
                stardustCoreAnimations.Add(pair.Key, pair.Value.toAnimation());
            }

            return TextureManagers.CreateAnimationManager(this.modManifestId, this.textureManagerId, this.textureId, stardustCoreAnimations, this.defaultAnimationKey, this.startingAnimationKey);
        }
 
    }
}
