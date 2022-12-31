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
    [XmlType("Mods_Revitalize.Framework.World.Objects.Items.Farming.AutoHarvesterGardenPotAttachment")]
    public class AutoHarvesterGardenPotAttachment:CustomItem
    {

        public AutoHarvesterGardenPotAttachment()
        {


        }

        public AutoHarvesterGardenPotAttachment(BasicItemInformation info) : base(info)
        {

        }

        public override Item getOne()
        {
            return new AutoHarvesterGardenPotAttachment(this.basicItemInformation.Copy());
        }
    }
}
