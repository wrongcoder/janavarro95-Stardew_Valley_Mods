using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Netcode;

namespace Revitalize.Framework.Utilities.Extensions
{
    public static class NetFieldsExtension
    {

        public static void AddFields(this Netcode.NetFields netFields, List<INetSerializable> netSerializables)
        {
            foreach(INetSerializable netSerializable in netSerializables)
            {
                netFields.AddField(netSerializable);
            }
        }
    }
}
