using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;

namespace Omegasis.SaveAnywhere
{
    /// <summary>A subclass of <see cref="ShippingMenu"/> that does everything except save.</summary>
    internal class NewShippingMenu : ShippingMenu
    {
        /*********
        ** Properties
        *********/
        /// <summary>The private field on the shipping menu which indicates the game has already been saved, which prevents it from saving.</summary>
        private readonly IPrivateField<bool> SavedYet;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="items">The shipping bin items.</param>
        /// <param name="reflection">Simplifies access to game code.</param>
        public NewShippingMenu(List<Item> items, IReflectionHelper reflection)
            : base(items)
        {
            this.SavedYet = reflection.GetPrivateField<bool>(this, "savedYet");
        }

        /// <summary>Updates the menu during the game's update loop.</summary>
        /// <param name="time">The game time that has passed.</param>
        public override void update(GameTime time)
        {
            this.SavedYet.SetValue(true); // prevent menu from saving
            base.update(time);
        }
    }
}
