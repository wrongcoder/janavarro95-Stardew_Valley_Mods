using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize.Resources.DataNodes
{
    class SerializerDataNode
    {
        public Dictionaries.ser serialize;
        public Dictionaries.par parse;

        public SerializerDataNode(Dictionaries.ser ser, Dictionaries.par par)
        {
            serialize = ser;
            parse = par;
        }
    }
}
