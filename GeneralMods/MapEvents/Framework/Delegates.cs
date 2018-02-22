using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEvents.Framework
{
    class Delegates
    {
        public delegate void voidDel();
        public delegate void strDel(string s);
        public delegate void paramFunction(List<object> parameters);
    }
}
