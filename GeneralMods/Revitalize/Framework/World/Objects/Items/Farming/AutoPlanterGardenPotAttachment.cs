using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Omegasis.Revitalize.Framework.World.Objects.InformationFiles;
using StardewValley;

namespace Omegasis.Revitalize.Framework.World.Objects.Items.Farming
{
    [XmlType("Mods_Revitalize.Framework.World.Objects.Items.Farming.AutoPlanterGardenPotAttachment")]
    public class AutoPlanterGardenPotAttachment:CustomItem
    {

        public AutoPlanterGardenPotAttachment()
        {


        }

        public AutoPlanterGardenPotAttachment(BasicItemInformation info) : base(info)
        {

        }

        public override Item getOne()
        {
            return new AutoPlanterGardenPotAttachment(this.basicItemInformation.Copy());
        }
    }
}
