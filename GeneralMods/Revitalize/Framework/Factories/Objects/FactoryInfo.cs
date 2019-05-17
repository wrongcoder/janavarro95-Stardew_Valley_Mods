using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revitalize.Framework.Objects;

namespace Revitalize.Framework.Factories.Objects.Furniture
{
    public class FactoryInfo
    {
        public BasicItemInformation info;

        public FactoryInfo()
        {

        }

        public FactoryInfo(MultiTiledObject obj)
        {
            this.info = obj.info;
        }

        public FactoryInfo(MultiTiledComponent component)
        {
            this.info = component.info;
        }

    }
}
