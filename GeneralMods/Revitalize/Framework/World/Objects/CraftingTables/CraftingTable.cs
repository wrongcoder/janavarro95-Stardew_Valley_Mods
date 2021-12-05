using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Revitalize.Framework.World.Objects.InformationFiles;
using Revitalize.Framework.Crafting;
using StardewValley;
using System.Xml.Serialization;

namespace Revitalize.Framework.World.Objects.CraftingTables
{
    [XmlType("Mods_Revitalize.Framework.World.Objects.CraftingTables.CraftingTable")]
    public class CraftingTable : CustomObject
    {
        public string craftingBookName;


        public CraftingTable()
        {

        }

        public CraftingTable(BasicItemInformation Info,string CraftingRecipeBookName):base(Info)
        {
            this.craftingBookName = CraftingRecipeBookName;
        }

        public CraftingTable(BasicItemInformation Info,Vector2 TilePosition ,string CraftingRecipeBookName) : base(Info,TilePosition)
        {
            this.craftingBookName = CraftingRecipeBookName;
        }

        /// <summary>
        /// When the chair is right clicked ensure that all pieces associated with it are also rotated.
        /// </summary>
        /// <param name="who"></param>
        /// <returns></returns>
        public override bool rightClicked(Farmer who)
        {
            if (ModCore.CraftingManager.modCraftingRecipesByGroup.ContainsKey(this.craftingBookName))
            {
                ModCore.log("Right click the crafting table. And have the recipe book enabled.");
                ModCore.CraftingManager.modCraftingRecipesByGroup[this.craftingBookName].openCraftingMenu();
                return true;
            }
            else
            {
                ModCore.log("Right click the crafting table. BUT DO NOT have the recipe book enabled: " + this.craftingBookName);
                return true;
            }
        }


        public override Item getOne()
        {
            CraftingTable component = new CraftingTable(this.getItemInformation().Copy(),this.craftingBookName);
            return component;
        }
    }
}
