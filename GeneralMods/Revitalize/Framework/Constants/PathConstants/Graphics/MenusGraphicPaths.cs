using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.Revitalize.Framework.Constants.PathConstants.Graphics
{
    /// <summary>
    /// Relative paths for all menu/hud content for the mod's assets.
    /// </summary>
    public class MenusGraphicPaths
    {
        public static string Menus = Path.Combine(RelativePaths.Graphics_Folder, "Menus");

        public static string CraftingMenu = Path.Combine(Menus, "CraftingMenu");
        public static string EnergyMenu = Path.Combine(Menus, "EnergyMenu");
        public static string InventoryMenu = Path.Combine(Menus, "InventoryMenu");

        public static string Misc = Path.Combine(Menus, "Misc");

    }
}
