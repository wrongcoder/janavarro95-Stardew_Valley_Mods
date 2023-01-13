using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.Constants;
using Omegasis.Revitalize.Framework.World.Objects.InformationFiles.Json;
using Omegasis.Revitalize.Framework.World.Objects.Items.Utilities;
using StardewValley;

namespace Omegasis.Revitalize.Framework.Crafting.JsonContent
{
    /// <summary>
    /// A recipe component written into a stucture to be used for json serialization/deserialization.
    /// </summary>
    public class JsonCraftingComponent
    {
        /// <summary>
        /// A reference to the item loaded in from json.
        /// </summary>
        public ItemReference item;

        /// <summary>
        /// The min stack size for this item. If this and <see cref="maxStackSize"/> are zero then <see cref="JsonCraftingComponent.item"/>'s <see cref="ItemReference.StackSize"/> is used for stack ranges instead.
        /// </summary>
        public int minStackSize;
        /// <summary>
        /// The max stack size for this item. If this and <see cref="minStackSize"/> are zero then <see cref="JsonCraftingComponent.item"/>'s <see cref="ItemReference.StackSize"/> is used for stack ranges instead.
        /// </summary>
        public int maxStackSize;

        public JsonCraftingComponent()
        {
            this.item = new ItemReference();
        }

        /// <summary>
        /// Creates a <see cref="CraftingRecipeComponent"/> from a json version loaded from disk.
        /// </summary>
        /// <returns></returns>
        public virtual CraftingRecipeComponent createCraftingRecipeComponent()
        {
            if (!string.IsNullOrEmpty(this.item.RegisteredObjectId))
            {
                return this.toCraftingRecipeComponent();
            }
            throw new InvalidJsonCraftingComponentException("A json crafting component must have one one of the following: a stardewValleyItemId, a stardewValleyBigCraftableId or a registeredObjectId set to be valid!");
        }

        /// <summary>
        /// Validates the state of the json crafting component before creating a crafting component from disk.
        /// </summary>
        public virtual void validate()
        {
            if (!string.IsNullOrEmpty(this.item.RegisteredObjectId))
            {
                if (!RevitalizeModCore.ModContentManager.objectManager.itemsById.ContainsKey(this.item.RegisteredObjectId))
                {
                    throw new InvalidJsonCraftingComponentException(string.Format("A json crafting component requests that it uses or gives an item with the registered id of {0} but no object with that id has been registered to the ModContentManager.ObjectManager's (or the Stardew Valley Item Registry) registered items list.", this.item.RegisteredObjectId));
                }
            }
            else
            {
                throw new InvalidJsonCraftingComponentException("A json crafting component must have at least one field set to be valid!");
            }
        }     

        /// <summary>
        /// Gets the item that is referenced by this crafting component.
        /// </summary>
        /// <returns></returns>
        public virtual Item getItem()
        {
            Item item = this.item.getItem();
            if (item == null)
            {
                throw new InvalidJsonCraftingComponentException("A json crafting component must have one one of the following: a stardewValleyItemId, a stardewValleyBigCraftableId or a registeredObjectId set to be valid!");
            }
            return item;
        }

        /// <summary>
        /// Converts this json information file to the actual <see cref="CraftingRecipeComponent"/> that is used in crafting.
        /// </summary>
        /// <returns></returns>
        public virtual CraftingRecipeComponent toCraftingRecipeComponent()
        {
            if (this.minStackSize != 0 && this.maxStackSize != 0)
            {
                return new CraftingRecipeComponent(this.getItem(), Game1.random.Next(this.minStackSize,this.maxStackSize));
            }
            else
            {
                return new CraftingRecipeComponent(this.getItem(), this.item.StackSize);
            }
        }
    }
}
