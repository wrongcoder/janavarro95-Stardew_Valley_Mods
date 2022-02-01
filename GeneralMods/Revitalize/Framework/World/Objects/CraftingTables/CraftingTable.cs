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
using Netcode;

namespace Revitalize.Framework.World.Objects.CraftingTables
{
    [XmlType("Mods_Revitalize.Framework.World.Objects.CraftingTables.CraftingTable")]
    public class CraftingTable : CustomObject
    {
        public readonly NetString craftingBookName = new NetString();


        public CraftingTable()
        {

        }

        public CraftingTable(BasicItemInformation Info,string CraftingRecipeBookName):base(Info)
        {
            this.craftingBookName.Value = CraftingRecipeBookName;
        }

        public CraftingTable(BasicItemInformation Info,Vector2 TilePosition ,string CraftingRecipeBookName) : base(Info,TilePosition)
        {
            this.craftingBookName.Value = CraftingRecipeBookName;
        }

        /// <summary>
        /// When the chair is right clicked ensure that all pieces associated with it are also rotated.
        /// </summary>
        /// <param name="who"></param>
        /// <returns></returns>
        public override bool rightClicked(Farmer who)
        {
            if (RevitalizeModCore.CraftingManager.modCraftingRecipesByGroup.ContainsKey(this.craftingBookName))
            {
                RevitalizeModCore.log("Right click the crafting table. And have the recipe book enabled.");
                RevitalizeModCore.CraftingManager.modCraftingRecipesByGroup[this.craftingBookName].openCraftingMenu();
                return true;
            }
            else
            {
                RevitalizeModCore.log("Right click the crafting table. BUT DO NOT have the recipe book enabled: " + this.craftingBookName);
                return true;
            }
        }

        protected override void initNetFieldsPostConstructor()
        {
            base.initNetFieldsPostConstructor();
            //this.NetFields.AddField(this.craftingBookName);
        }


        public override Item getOne()
        {
            CraftingTable component = new CraftingTable(this.getItemInformation().Copy(),this.craftingBookName);
            return component;
        }
    }
}
