
using CustomNPCFramework.Framework.Enums;
using CustomNPCFramework.Framework.ModularNPCS;
using CustomNPCFramework.Framework.ModularNPCS.CharacterAnimationBases;
using CustomNPCFramework.Framework.ModularNPCS.ModularRenderers;
using CustomNPCFramework.Framework.NPCS;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNPCFramework.Framework.Graphics
{
    /// <summary>
    /// Used to contain all of the asset managers.
    /// </summary>
    /// 

    public class NamePairings
    {
        public string leftString;
        public string rightString;
        public string upString;
        public string downString;
        public NamePairings(string LeftString,string RightString, string UpString, string DownString)
        {
            this.leftString = LeftString;
            this.rightString = RightString;
            this.upString = UpString;
            this.downString = DownString;
        }
    }

    public class AssetPool
    {

        public Dictionary<string, AssetManager> assetPool;

        public AssetPool()
        {
            this.assetPool = new Dictionary<string, AssetManager>();
        }

        public void addAssetManager(KeyValuePair<string, AssetManager> pair)
        {
            this.assetPool.Add(pair.Key, pair.Value);
        }

        public void addAssetManager(string assetManagerName, AssetManager assetManager)
        {
            this.assetPool.Add(assetManagerName, assetManager);
        }

        public AssetManager getAssetManager(string name)
        {
            assetPool.TryGetValue(name, out AssetManager asset);
            return asset;
        }

        public void removeAssetManager(string key)
        {
            assetPool.Remove(key);
        }

        public void loadAllAssets()
        {
            foreach (KeyValuePair<string, AssetManager> assetManager in this.assetPool)
            {
                assetManager.Value.loadAssets();
            }
        }

        /// <summary>
        /// Creates an extended animated sprite object given the asset name in the asset manager.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public AnimatedSpriteExtended getAnimatedSpriteFromAsset(string name)
        {
            assetPool.TryGetValue(name, out AssetManager asset);
            var assetSheet = asset.getAssetByName(name);
            return new AnimatedSpriteExtended(assetSheet.clone().getCurrentSpriteTexture(), assetSheet.index, (int)assetSheet.assetInfo.assetSize.X, (int)assetSheet.assetInfo.assetSize.Y);
        }


        
        public AnimatedSpriteCollection getSpriteCollectionFromSheet(AssetSheet assetSheet, AnimationType type)
        {    
                var left = new AnimatedSpriteExtended(assetSheet.clone().getTexture(Direction.left, type), assetSheet.index, (int)assetSheet.assetInfo.assetSize.X, (int)assetSheet.assetInfo.assetSize.Y);
                var right = new AnimatedSpriteExtended(assetSheet.clone().getTexture(Direction.right, type), assetSheet.index, (int)assetSheet.assetInfo.assetSize.X, (int)assetSheet.assetInfo.assetSize.Y);
                var up = new AnimatedSpriteExtended(assetSheet.clone().getTexture(Direction.up, type), assetSheet.index, (int)assetSheet.assetInfo.assetSize.X, (int)assetSheet.assetInfo.assetSize.Y);
                var down = new AnimatedSpriteExtended(assetSheet.clone().getTexture(Direction.down, type), assetSheet.index, (int)assetSheet.assetInfo.assetSize.X, (int)assetSheet.assetInfo.assetSize.Y);
                return new AnimatedSpriteCollection(left, right, up, down, Direction.down); 
        }
        

        /// <summary>
        /// Gets an animated sprite collection (ie a hair style facing all four directions) from a list of asset names.
        /// </summary>
        /// <param name="left">The name of the asset for the left facing sprite.</param>
        /// <param name="right">The name of the asset for the right facing sprite.</param>
        /// <param name="up">The name of the asset for the up facing sprite.</param>
        /// <param name="down">The name of the asset for the down facing sprite.</param>
        /// <param name="startingDirection"></param>
        /// <returns></returns>
        public AnimatedSpriteCollection getAnimatedSpriteCollectionFromAssets(string left, string right, string up, string down, Direction startingDirection = Direction.down)
        {
            var Left = getAnimatedSpriteFromAsset(left);
            var Right = getAnimatedSpriteFromAsset(right);
            var Up = getAnimatedSpriteFromAsset(up);
            var Down = getAnimatedSpriteFromAsset(down);
            return new AnimatedSpriteCollection(Left, Right, Up, Down, startingDirection);

        }

        public AnimatedSpriteCollection getAnimatedSpriteCollectionFromAssets(NamePairings pair, Direction startingDirection = Direction.down)
        {
            return getAnimatedSpriteCollectionFromAssets(pair.leftString, pair.rightString, pair.upString, pair.downString);
        }

        public StandardCharacterAnimation GetStandardCharacterAnimation(NamePairings BodySprites, NamePairings EyeSprites, NamePairings HairSprites, NamePairings ShirtsSprites, NamePairings PantsSprites, NamePairings ShoesSprites,List<NamePairings> AccessoriesSprites)
        {
            var body = getAnimatedSpriteCollectionFromAssets(BodySprites);
            var eyes = getAnimatedSpriteCollectionFromAssets(EyeSprites);
            var hair = getAnimatedSpriteCollectionFromAssets(HairSprites);
            var shirts = getAnimatedSpriteCollectionFromAssets(ShirtsSprites);
            var pants = getAnimatedSpriteCollectionFromAssets(PantsSprites);
            var shoes = getAnimatedSpriteCollectionFromAssets(ShoesSprites);
            List<AnimatedSpriteCollection> accessories = new List<AnimatedSpriteCollection>();
            foreach(var v in AccessoriesSprites)
            {
                accessories.Add(getAnimatedSpriteCollectionFromAssets(v));
            }
            return new StandardCharacterAnimation(body,eyes,hair,shirts,pants,shoes,accessories);
        }

        public List<AssetSheet> getListOfApplicableBodyParts(string assetManagerName,Genders gender, Seasons season, PartType type)
        {
            var parts = this.getAssetManager(assetManagerName).getListOfAssetsThatMatchThisCriteria(gender, season, type);
            return parts;
        }

        public void generateNPC(Genders gender, int minNumOfAccessories, int maxNumOfAccessories)
        {
            Seasons myseason=Seasons.spring;

            AnimationType currentType = AnimationType.standing;

            if (Game1.currentSeason == "spring") myseason = Seasons.spring;
            if (Game1.currentSeason == "summer") myseason = Seasons.summer;
            if (Game1.currentSeason == "fall") myseason = Seasons.fall;
            if (Game1.currentSeason == "winter") myseason = Seasons.winter;

            List<AssetSheet> bodyList = new List<AssetSheet>();
            List<AssetSheet> eyesList = new List<AssetSheet>();
            List<AssetSheet> hairList = new List<AssetSheet>();
            List<AssetSheet> shirtList = new List<AssetSheet>();
            List<AssetSheet> shoesList = new List<AssetSheet>();
            List<AssetSheet> pantsList = new List<AssetSheet>();
            List<AssetSheet> accessoryList = new List<AssetSheet>();

            //Get all applicable parts from this current asset manager
            foreach (var assetManager in this.assetPool)
            {
                var body = getListOfApplicableBodyParts(assetManager.Key, gender, myseason, PartType.body);
                foreach (var piece in body) bodyList.Add(piece);

                var eyes = getListOfApplicableBodyParts(assetManager.Key, gender, myseason, PartType.eyes);
                foreach (var piece in eyes) eyesList.Add(piece);

                var hair = getListOfApplicableBodyParts(assetManager.Key, gender, myseason, PartType.hair);
                foreach (var piece in hair) hairList.Add(piece);

                var shirt = getListOfApplicableBodyParts(assetManager.Key, gender, myseason, PartType.shirt);
                foreach (var piece in shirt) shirtList.Add(piece);

                var pants = getListOfApplicableBodyParts(assetManager.Key, gender, myseason, PartType.pants);
                foreach (var piece in pants) pantsList.Add(piece);

                var shoes = getListOfApplicableBodyParts(assetManager.Key, gender, myseason, PartType.shoes);
                foreach (var piece in shoes) bodyList.Add(piece);

                var accessory = getListOfApplicableBodyParts(assetManager.Key, gender, myseason, PartType.accessory);
                foreach (var piece in accessory) accessoryList.Add(piece);
            }

            Random r = new Random(Game1.random.Next());
            int amount = r.Next(minNumOfAccessories, maxNumOfAccessories);
           
            int bodyIndex = r.Next(0, bodyList.Count - 1);
            int eyesIndex = r.Next(0, eyesList.Count - 1);
            int hairIndex = r.Next(0, hairList.Count - 1);
            int shirtIndex = r.Next(0, shirtList.Count - 1);
            int pantsIndex = r.Next(0, pantsList.Count - 1);
            int shoesIndex = r.Next(0, shoesList.Count - 1);
            List<int> accIntList = new List<int>();
            for (int i = 0; i < amount; i++)
            {
                int acc = r.Next(0, accessoryList.Count - 1);
                accIntList.Add(acc);
            }

            AssetSheet bodySheet = bodyList.ElementAt(bodyIndex);
            AssetSheet eyesSheet = eyesList.ElementAt(bodyIndex);
            AssetSheet hairSheet = hairList.ElementAt(bodyIndex);
            AssetSheet shirtSheet = shirtList.ElementAt(bodyIndex);
            AssetSheet pantsSheet = pantsList.ElementAt(bodyIndex);
            AssetSheet shoesSheet = shoesList.ElementAt(bodyIndex);
            List<AssetSheet> accessorySheet = new List<AssetSheet>();

            foreach (var v in accIntList)
            {
                accessorySheet.Add(accessoryList.ElementAt(v));
            }


            AnimationType type = AnimationType.standing;
            var standingAnimation = generateCharacterAnimation(bodySheet, eyesSheet, hairSheet, shirtSheet, pantsSheet, shoesSheet, accessorySheet, type);
            type = AnimationType.walking;
            var movingAnimation = generateCharacterAnimation(bodySheet, eyesSheet, hairSheet, shirtSheet, pantsSheet, shoesSheet, accessorySheet, type);
            type = AnimationType.swimming;
            var swimmingAnimation = generateCharacterAnimation(bodySheet, eyesSheet, hairSheet, shirtSheet, pantsSheet, shoesSheet, accessorySheet, type);
            //type = AnimationType.standing;
            //var sAnimation = generateCharacterAnimation(bodySheet, eyesSheet, hairSheet, shirtSheet, pantsSheet, shoesSheet, accessorySheet, type);


            BasicRenderer render = new BasicRenderer(standingAnimation,movingAnimation,swimmingAnimation);

            ExtendedNPC npc = new ExtendedNPC(null, render, new Microsoft.Xna.Framework.Vector2(13, 15) * Game1.tileSize, 2, NPCNames.getRandomNPCName(gender));

        }

        public virtual StandardCharacterAnimation generateCharacterAnimation(AssetSheet body, AssetSheet eyes, AssetSheet hair, AssetSheet shirt, AssetSheet pants, AssetSheet shoes,List<AssetSheet> accessories, AnimationType animationType)
        {
            var bodySprite = getSpriteCollectionFromSheet(body, animationType);
            var eyesSprite = getSpriteCollectionFromSheet(eyes, animationType);
            var hairSprite = getSpriteCollectionFromSheet(hair, animationType);
            var shirtSprite = getSpriteCollectionFromSheet(shirt, animationType);
            var pantsSprite = getSpriteCollectionFromSheet(pants, animationType);
            var shoesSprite = getSpriteCollectionFromSheet(shoes, animationType);
            List<AnimatedSpriteCollection> accessoryCollection = new List<AnimatedSpriteCollection>();
            foreach (var v in accessories)
            {
                AnimatedSpriteCollection acc = getSpriteCollectionFromSheet(v, AnimationType.standing);
                accessoryCollection.Add(acc);
            }
            StandardCharacterAnimation standingAnimation = new StandardCharacterAnimation(bodySprite, eyesSprite, hairSprite, shirtSprite, pantsSprite, shoesSprite, accessoryCollection);
            return standingAnimation;
        }


        /*
        public void generateNPC(string assetManagerName,Genders gender, Seasons season, int minNumOfAccessories,int maxNumOfAccessories)
        {
           var body= this.getAssetManager(assetManagerName).getListOfAssetsThatMatchThisCriteria(gender, season, PartType.body);
           var eyes = this.getAssetManager(assetManagerName).getListOfAssetsThatMatchThisCriteria(gender, season, PartType.eyes);
           var hair = this.getAssetManager(assetManagerName).getListOfAssetsThatMatchThisCriteria(gender, season, PartType.hair);
           var shirt = this.getAssetManager(assetManagerName).getListOfAssetsThatMatchThisCriteria(gender, season, PartType.shirt);
           var pants = this.getAssetManager(assetManagerName).getListOfAssetsThatMatchThisCriteria(gender, season, PartType.pants);
           var shoes = this.getAssetManager(assetManagerName).getListOfAssetsThatMatchThisCriteria(gender, season, PartType.shoes);
           List<AssetSheet> accessories = new List<AssetSheet>();
           Random r = new Random(Game1.random.Next());
           int amount=r.Next(minNumOfAccessories, maxNumOfAccessories);
           var accessoriesList= this.getAssetManager(assetManagerName).getListOfAssetsThatMatchThisCriteria(gender, season, PartType.shoes);

            int bodyIndex = r.Next(0, body.Count - 1);
            int eyesIndex = r.Next(0, eyes.Count - 1);
            int hairIndex = r.Next(0, hair.Count - 1);
            int shirtIndex = r.Next(0, shirt.Count - 1);
            int pantsIndex = r.Next(0, pants.Count - 1);
            int shoesIndex = r.Next(0, shoes.Count - 1);
            List<int> accIntList = new List<int>();
            for(int i=0; i < amount; i++)
            {
                int acc= r.Next(0, accessoriesList.Count - 1);
                accIntList.Add(acc);
            }

            AssetSheet bodySheet = body.ElementAt(bodyIndex);
            AssetSheet eyesSheet = body.ElementAt(bodyIndex);
            AssetSheet hairSheet = body.ElementAt(bodyIndex);
            AssetSheet shirtSheet = body.ElementAt(bodyIndex);
            AssetSheet pantsSheet = body.ElementAt(bodyIndex);
            AssetSheet shoesSheet = body.ElementAt(bodyIndex);

            foreach(var v in accIntList)
            {
                accessories.Add(accessoriesList.ElementAt(v));
            }


        }
        */

    }
}
