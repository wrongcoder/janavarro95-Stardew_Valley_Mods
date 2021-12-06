using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Netcode;

namespace Revitalize.Framework.Illuminate
{
    public class NetFakeLightSource:NetField<FakeLightSource,NetFakeLightSource>
    {

        public NetFakeLightSource()
        {

        }

        public NetFakeLightSource(FakeLightSource value) : base(value)
        {

        }

        public override void Set(FakeLightSource newValue)
        {
            if (base.canShortcutSet())
            {
                base.value = newValue;
            }
            else if (newValue != base.value)
            {
                base.cleanSet(newValue);
                base.MarkDirty();
            }
        }

        protected override void ReadDelta(BinaryReader reader, NetVersion version)
        {

            if (this.value == null)
            {
                this.value = new FakeLightSource();
            }

            if (version.IsPriorityOver(base.ChangeVersion))
            {
                this.value.readFakeLightSource(reader);
                base.setInterpolationTarget(this.value);
            }
        }

        protected override void WriteDelta(BinaryWriter writer)
        {

            if (this.value == null)
            {
                this.value = new FakeLightSource();
            }
            this.value.writeFakeLightSource(writer);
        }
    }
}
