using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Netcode;

namespace StardustCore.Networking
{
    public class NetObject : INetObject<NetFields>
    {
        [XmlIgnore]
        public NetFields NetFields { get; } = new NetFields();

        protected virtual void initializeNetFields()
        {

        }
    }
}
