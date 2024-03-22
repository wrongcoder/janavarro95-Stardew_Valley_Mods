using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.GameData.Objects;
using StardewValley.GameData.Shops;
using StardewValley.Menus;
using StardewValley.Triggers;

namespace RefillSilos
{
    /// <summary>
    /// A mod used to add an item to be bought from Marnie to refill all of the silos on the Player's farm automatically.
    /// </summary>
    public class RefillSilos : Mod
    {
        /// <summary>
        /// The action id to be used for triggering refilling a silo for the player.
        /// </summary>
        public const string BUY_SILO_REFILL_ACTION = "Omegasis.RefillSilos.OnRefillSiloItemObtained";

        /// <summary>
        /// Used to keep track of how much hay to actually provide when purchasing the silo refill option.
        /// </summary>
        public int numberOfHayToRefill = 0;

        public override void Entry(IModHelper helper)
        {
            TriggerActionManager.RegisterAction(BUY_SILO_REFILL_ACTION, new TriggerActionDelegate(this.onRefilSiloItemObtained));

            this.Helper.Events.Display.MenuChanged += this.updateAnimalShopForSiloRefillItem;
            this.Helper.Events.Content.AssetRequested += this.checkIfAssetCanBeEdited;
        }

        /// <summary>
        /// CHeck
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkIfAssetCanBeEdited(object? sender, AssetRequestedEventArgs e)
        {

            if (e.NameWithoutLocale.IsEquivalentTo("Data/Objects"))
            {
                e.Edit(this.addItemToObjectRegistry);
            }
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="data"></param>
        public void addItemToObjectRegistry(IAssetData data)
        {
            IDictionary<string, ObjectData> objectDictionary = data.AsDictionary<string, ObjectData>().Data;
            ObjectData objectData = new ObjectData();
            objectData.Name = "Omegasis.RefillSilo.Item";
            objectData.DisplayName = "Refill Silos";
            objectData.Texture = null;
            objectData.Description = "Refills all of the silos full of hay, as much as possible.";
            objectData.ExcludeFromFishingCollection = true;
            objectData.ExcludeFromRandomSale = true;
            objectData.ExcludeFromShippingCollection = true;
            objectData.SpriteIndex = 178;
            objectData.Price = 0;
            objectDictionary.Add("Omegasis.RefillSilo.Item", objectData);
        }

        private void updateAnimalShopForSiloRefillItem(object? sender, MenuChangedEventArgs e)
        {
            if (e.NewMenu != null && e.NewMenu is ShopMenu)
            {
                ShopMenu shopMenu = (ShopMenu)e.NewMenu;
                if (shopMenu.ShopId.Equals("AnimalShop"))
                {
                    //Reset the counter for number of hay to refill.
                    this.numberOfHayToRefill = 0;

                    Dictionary<ISalable, ItemStockInformation> stock = new Dictionary<ISalable, ItemStockInformation>();

                    int index = 0;
                    foreach (ISalable key in shopMenu.itemPriceAndStock.Keys)
                    {

                        ItemStockInformation itemStockInformation = shopMenu.itemPriceAndStock[key];
 
                        //ShopItemData hayObjectForSale = shopMenu.ShopData.Items[index];
                        //Find the hay object.
                        if (key.QualifiedItemId.Equals("(O)178"))
                        {
                            ItemStockInformation refillItemStockInformation = new ItemStockInformation();
                            refillItemStockInformation.Stock = 1;
                            refillItemStockInformation.ActionsOnPurchase = new List<string>() { BUY_SILO_REFILL_ACTION };


                            //Calculate how many pieces of hay need to be purchased.
                            foreach (GameLocation location in Game1.locations)
                            {
                                this.numberOfHayToRefill += location.GetHayCapacity() - location.piecesOfHay.Value;
                            }

                            //Cap the number of hay pieces the farmer can by by either the max number of peices they can buy, or by how much money they have, so that way the refill amount is always purchasable.
                            this.numberOfHayToRefill = Math.Min(this.numberOfHayToRefill, Game1.player.Money / itemStockInformation.Price);

                            //Get the price of hay and multiply by the number of pieces to refill for the silo.
                            refillItemStockInformation.Price = this.numberOfHayToRefill * itemStockInformation.Price;
                            refillItemStockInformation.TradeItemCount = 1;

                            //Only add the refill option if the number of hay pieces are greater than zero.
                            if (this.numberOfHayToRefill > 0)
                            {
                                //Insert the new item after the index of the hay item for better visibility.
                                Item refillItem = ItemRegistry.Create("(O)Omegasis.RefillSilo.Item");
                                refillItem.salePrice();

                                shopMenu.forSale.Insert(index+1, refillItem);
                                shopMenu.itemPriceAndStock.Add(refillItem, refillItemStockInformation);

                                break;
                            }
                            else
                            {
                                this.Monitor.Log("Refill item can't bought! Not enough hay space available.", LogLevel.Info);
                                break;
                            }
                        }
                        index++;
                    }
                }
            }
        }

        private bool onRefilSiloItemObtained(string[] args, TriggerActionContext triggerActionContext, out string error)
        {
            int initialiNumberOfPiecesOfHayToFill = this.numberOfHayToRefill;
            foreach (GameLocation location in Game1.locations)
            {
                if (location.GetHayCapacity() <= 0)
                {
                    continue;
                }
                //The return amount is always the number of hay that can't be stored.
                this.numberOfHayToRefill = location.tryToAddHay(this.numberOfHayToRefill);
                if (this.numberOfHayToRefill <= 0)
                {
                    break;
                }
            }

            Game1.hudMessages.Add(new HUDMessage(string.Format("Bought {0} pieces of hay.", initialiNumberOfPiecesOfHayToFill)));

            this.numberOfHayToRefill = 0;

            if(Game1.activeClickableMenu!= null)
            {
                if(Game1.activeClickableMenu is ShopMenu)
                {
                    ShopMenu shopMenu =(ShopMenu)Game1.activeClickableMenu;
                    //Since the item purchased is a fake item as it's just supposed to refill the silos, don't allow the player to add it to their inventory.
                    shopMenu.heldItem = null;
                }
            }

            error = "Refilling the silos was successful, but for for some reason the game throws this error message. I can confirm that everything still works as intended.";
            return true;
        }
    }
}
