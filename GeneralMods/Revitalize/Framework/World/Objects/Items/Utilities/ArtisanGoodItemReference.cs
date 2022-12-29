using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Netcode;
using Newtonsoft.Json;
using StardewValley;
using StardewValley.Objects;
using static Omegasis.Revitalize.Framework.Constants.Enums;
using static StardewValley.Object;

namespace Omegasis.Revitalize.Framework.World.Objects.Items.Utilities
{
    /// <summary>
    /// Class that references different types of artisinal goods made in the game.
    /// </summary>
    public class ArtisanGoodItemReference : StardustCore.Networking.NetObject
    {
        [JsonIgnore]
        public readonly NetEnum<SDVPreserveType> preserveType = new NetEnum<SDVPreserveType>(SDVPreserveType.NULL);
        [JsonIgnore]
        public readonly NetString preservedRegisteredObjectId = new NetString("");
        /// <summary>
        /// The type of object that is preserved such as Wine or Jelly.
        /// </summary>
        [JsonProperty("preserveType")]
        public SDVPreserveType PreserveType
        {
            get
            {
                return this.preserveType.Value;
            }
            set
            {
                this.preserveType.Value = value;
            }
        }

        /// <summary>
        /// The id of the item that was used to preserve the item. This will usually be something like apples, or BlueJazz. <see cref="getPreservedObjectTypeRegisteredObjectId"/> for the id of the object "container" for the preserved good.
        /// </summary>
        [JsonProperty("preservedRegisteredObjectId")]
        public string PreservedRegisteredObjectId
        {
            get
            {
                return this.preservedRegisteredObjectId.Value;
            }
            set
            {
                this.preservedRegisteredObjectId.Value = value;
            }
        }

        public ArtisanGoodItemReference()
        {

        }

        public ArtisanGoodItemReference(SDVObject ObjectId, PreserveType PreserveType) : this(Revitalize.RevitalizeModCore.ModContentManager.objectManager.createVanillaObjectId(ObjectId), Enum.Parse<SDVPreserveType>(((int)PreserveType).ToString()))
        {

        }

        public ArtisanGoodItemReference(SDVObject ObjectId, SDVPreserveType PreserveType) : this(Revitalize.RevitalizeModCore.ModContentManager.objectManager.createVanillaObjectId(ObjectId), PreserveType)
        {

        }

        public ArtisanGoodItemReference(int ObjectId, PreserveType PreserveType) : this(Revitalize.RevitalizeModCore.ModContentManager.objectManager.createVanillaObjectId(ObjectId),Enum.Parse<SDVPreserveType>(((int)PreserveType).ToString()))
        {

        }

        public ArtisanGoodItemReference(string ObjectId, PreserveType PreserveType):this(ObjectId,Enum.Parse<SDVPreserveType>(((int)PreserveType).ToString()))
        {

        }

        public ArtisanGoodItemReference(string ObjectId, SDVPreserveType PreserveType)
        {
            this.PreservedRegisteredObjectId = ObjectId;
            this.PreserveType = PreserveType;
        }



        protected override void initializeNetFields()
        {
            base.initializeNetFields();
            this.NetFields.AddFields(this.preserveType, this.preservedRegisteredObjectId);
        }

        public virtual void clearItemReference()
        {
            this.PreservedRegisteredObjectId = "";
            this.PreserveType = SDVPreserveType.NULL;
        }

        public virtual bool isNotNull()
        {
            return !string.IsNullOrEmpty(this.PreservedRegisteredObjectId) && this.PreserveType != SDVPreserveType.NULL;
        }

        public virtual Item getItem()
        {
            return this.getItem(1);
        }

        public virtual Item getItem(int Stack=1)
        {

            if (this.PreserveType == SDVPreserveType.NULL)
            {
                throw new Exception("Preserve type can not be null for Artisan Good Item Reference!");
                return null;
            }

            StardewValley.Object obj = null;
            StardewValley.Object preservedObjectType = (RevitalizeModCore.ModContentManager.objectManager.getObject<StardewValley.Object>(this.PreservedRegisteredObjectId));

            if (this.PreserveType == SDVPreserveType.AgedRoe)
            {
                obj = new ColoredObject((int)SDVObject.AgedRoe, Stack, StardewValley.Menus.TailoringMenu.GetDyeColor(preservedObjectType) ?? Color.Orange);
                obj.Price = 60 + preservedObjectType.Price;
            }
            if (this.PreserveType == SDVPreserveType.Honey)
            {
                obj = (StardewValley.Object)new ItemReference(SDVObject.Honey, Stack).getItem();
                if (this.PreservedRegisteredObjectId == "-1")
                {
                    if (obj.Name == "Honey")
                    {
                        obj.Name = "Wild Honey";
                    }
                    obj.Name= Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12750");
                    return obj;
                }

                //Base honey sell price + flower sell price * 2;
                obj.Price = obj.Price + preservedObjectType.Price * 2;
                obj.DisplayName = preservedObjectType.DisplayName + " " + obj.DisplayName;
                obj.Name = preservedObjectType.DisplayName + " " + obj.DisplayName;
            }
            if (this.PreserveType == SDVPreserveType.Jelly)
            {
                obj = (StardewValley.Object)new ItemReference(SDVObject.Jelly, Stack).getItem();
                obj.Price = 50 + preservedObjectType.Price * 2;
            }
            if (this.PreserveType == SDVPreserveType.Juice)
            {
                obj = (StardewValley.Object)new ItemReference(SDVObject.Juice, Stack).getItem();
                obj.Price = (int)(preservedObjectType.Price * 2.25f);
            }
            if (this.PreserveType == SDVPreserveType.Pickle)
            {
                obj = (StardewValley.Object)new ItemReference(SDVObject.Pickles, Stack).getItem();
                obj.Price = 50 + (preservedObjectType.Price * 2);
            }
            if (this.PreserveType == SDVPreserveType.Roe)
            {
                obj = new ColoredObject((int)SDVObject.Roe, Stack, StardewValley.Menus.TailoringMenu.GetDyeColor(preservedObjectType) ?? Color.Orange);
                obj.Price = 30+(preservedObjectType.Price /2);
            }
            if (this.PreserveType == SDVPreserveType.Wine)
            {
                obj = (StardewValley.Object)new ItemReference(SDVObject.Wine, Stack).getItem();
                obj.Price = preservedObjectType.Price * 3;
            }

            //Note that this will break in SDV 1.6 so will need to skip the conversion from string back to int.
            obj.preservedParentSheetIndex.Value = RevitalizeModCore.ModContentManager.objectManager.convertSDVStringObjectIdToIntObjectId(this.PreservedRegisteredObjectId);
            (obj as StardewValley.Object).preserve.Value = (PreserveType?)(int)this.PreserveType;
            return obj;
        }

        /// <summary>
        /// Gets the ObjectId for the type of object that is preserved. I.E honey or wine.
        /// </summary>
        /// <returns></returns>
        public virtual string getPreservedObjectTypeRegisteredObjectId()
        {
            if (this.PreserveType == SDVPreserveType.NULL)
            {
                return null;
            }
            if (this.PreserveType == SDVPreserveType.AgedRoe)
            {
               return RevitalizeModCore.ModContentManager.objectManager.createVanillaObjectId(SDVObject.AgedRoe);
            }
            if (this.PreserveType == SDVPreserveType.Honey)
            {
                return RevitalizeModCore.ModContentManager.objectManager.createVanillaObjectId(SDVObject.Honey);
            }
            if (this.PreserveType == SDVPreserveType.Jelly)
            {
                return RevitalizeModCore.ModContentManager.objectManager.createVanillaObjectId(SDVObject.Jelly);
            }
            if (this.PreserveType == SDVPreserveType.Juice)
            {
                return RevitalizeModCore.ModContentManager.objectManager.createVanillaObjectId(SDVObject.Juice);
            }
            if (this.PreserveType == SDVPreserveType.Pickle)
            {
                return RevitalizeModCore.ModContentManager.objectManager.createVanillaObjectId(SDVObject.Pickles);
            }
            if (this.PreserveType == SDVPreserveType.Roe)
            {
                return RevitalizeModCore.ModContentManager.objectManager.createVanillaObjectId(SDVObject.Roe);
            }
            if (this.PreserveType == SDVPreserveType.Wine)
            {
                return RevitalizeModCore.ModContentManager.objectManager.createVanillaObjectId(SDVObject.Wine);
            }
            return "";
        }
    }
}
