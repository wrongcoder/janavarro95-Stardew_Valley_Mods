using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModdedUtilitiesNetworking.Framework
{
    public class DataInfo 
    {
        public string type;
        public object data;

        public DataInfo()
        {
        }

        public DataInfo(string Type,object Data)
        {
            this.type = Type;
            this.data = Data;
        }


    }
}
