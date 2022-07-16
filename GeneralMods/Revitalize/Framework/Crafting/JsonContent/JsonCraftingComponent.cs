using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.Constants;

namespace Omegasis.Revitalize.Framework.Crafting.JsonContent
{
    /// <summary>
    /// A recipe component written into a stucture to be used for json serialization/deserialization.
    /// </summary>
    public class JsonCraftingComponent
    {

        public Enums.SDVObject stardewValleyItemId;
        public Enums.SDVBigCraftable stardewValleyBigCraftableId;
        public string registeredObjectId;
        public int amountNeeded;

        public JsonCraftingComponent()
        {
            this.stardewValleyItemId = Enums.SDVObject.NULL;
            this.stardewValleyBigCraftableId = Enums.SDVBigCraftable.NULL;
            this.amountNeeded = 0;
            this.registeredObjectId = "";
        }

        public JsonCraftingComponent(int ParentSheetIndex, bool IsBigCraftable, int AmountNeeded):this()
        {
            if (IsBigCraftable)
            {
                this.stardewValleyBigCraftableId = (Enums.SDVBigCraftable)ParentSheetIndex;
            }
            else
            {
                this.stardewValleyItemId = (Enums.SDVObject)ParentSheetIndex;
            }
            this.amountNeeded = AmountNeeded;
        }

        public JsonCraftingComponent(Enums.SDVObject objectId, int AmountNeeded) : this()
        {
            this.stardewValleyItemId = objectId;
            this.amountNeeded = AmountNeeded;
        }

        public JsonCraftingComponent(Enums.SDVBigCraftable objectId, int AmountNeeded) : this()
        {
            this.stardewValleyBigCraftableId = objectId;
            this.amountNeeded = AmountNeeded;
        }

        public JsonCraftingComponent(string registeredObjectId, int AmountNeeded) : this()
        {
            this.registeredObjectId = registeredObjectId;
            this.amountNeeded = AmountNeeded;
        }

        /// <summary>
        /// Creates a <see cref="CraftingRecipeComponent"/> from a json version loaded from disk.
        /// </summary>
        /// <returns></returns>
        public CraftingRecipeComponent createCraftingRecipeComponent()
        {
            this.validate();
            if (this.stardewValleyItemId > 0)
            {
                return new CraftingRecipeComponent(RevitalizeModCore.ModContentManager.objectManager.getItem(this.stardewValleyItemId, 1), this.amountNeeded);
            }
            if (this.stardewValleyBigCraftableId > 0)
            {
                return new CraftingRecipeComponent(RevitalizeModCore.ModContentManager.objectManager.getItem(this.stardewValleyBigCraftableId, 1), this.amountNeeded);
            }
            if (!string.IsNullOrEmpty(this.registeredObjectId))
            {
                return new CraftingRecipeComponent(RevitalizeModCore.ModContentManager.objectManager.getItem(this.registeredObjectId, 1), this.amountNeeded);
            }
            throw new InvalidJsonCraftingComponentException("A json crafting component must have one one of the following: a stardewValleyItemId, a stardewValleyBigCraftableId or a registeredObjectId set to be valid!");
        }

        /// <summary>
        /// Validates the state of the json crafting component before creating a crafting component from disk.
        /// </summary>
        public virtual void validate()
        {
            int numberOfValidIdFieldsSet = 0;
            if(this.stardewValleyItemId!= Enums.SDVObject.NULL)
            {
                numberOfValidIdFieldsSet++;
            }
            if (this.stardewValleyBigCraftableId != Enums.SDVBigCraftable.NULL)
            {
                numberOfValidIdFieldsSet++;
            }
            if (!string.IsNullOrEmpty(this.registeredObjectId))
            {
                numberOfValidIdFieldsSet++;
            }

            if (numberOfValidIdFieldsSet == 0)
            {
                throw new InvalidJsonCraftingComponentException("A json crafting component must have either a stardewValleyItemId, a stardewValleyBigCraftableId or a registeredObjectId set to be valid!");
            }

            if (numberOfValidIdFieldsSet > 1)
            {
                throw new InvalidJsonCraftingComponentException("A json crafting component must have one one of the following: a stardewValleyItemId, a stardewValleyBigCraftableId or a registeredObjectId set to be valid!");
            }

        }
    }
}
