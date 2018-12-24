using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize.Framework.Environment
{
    public class DarkerNightConfig
    {
        public bool Enabled;
        public float DarknessIntensity;
        public DarkerNightConfig()
        {
            this.Enabled = true;
            this.DarknessIntensity = .9f;
        }

    }
}
