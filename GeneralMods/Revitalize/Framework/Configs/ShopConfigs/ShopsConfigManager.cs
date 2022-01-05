using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize.Framework.Configs.ShopConfigs
{
    public class ShopsConfigManager
    {
        public BlacksmithShopConfig blacksmithShopsConfig;
        public DwarfShopConfig dwarfShopConfig;

        public ShopsConfigManager()
        {

            this.blacksmithShopsConfig = BlacksmithShopConfig.InitializeConfig();
            this.dwarfShopConfig = DwarfShopConfig.InitializeConfig();

        }

    }
}
