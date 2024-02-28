using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Netcode;

namespace Omegasis.StardustCore.Networking
{
    public static class NetFieldsExtensions
    {

        public static NetFields AddFields(this NetFields netFields, params INetSerializable[] fieldsToAdd)
        {
            foreach( INetSerializable field in fieldsToAdd )
            {
                netFields.AddField(field);
            }
            return netFields;
        }
    }
}
