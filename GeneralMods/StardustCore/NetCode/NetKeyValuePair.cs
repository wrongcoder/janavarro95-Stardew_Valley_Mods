using Netcode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardustCore.NetCode
{
    public class NetKeyValuePair<K, V, KField, VField> : NetField<KeyValuePair<K, V>, NetKeyValuePair<K, V, KField, VField>> where KField : NetField<K, KField>, new() where VField : NetField<V, VField>, new()
    {

        protected override void ReadDelta(BinaryReader reader, NetVersion version)
        {
            throw new NotImplementedException();
        }

        protected override void WriteDelta(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
