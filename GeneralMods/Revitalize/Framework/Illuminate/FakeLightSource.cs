using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Revitalize.Framework.Illuminate
{
    public class FakeLightSource
    {
        public int id;
        public Vector2 positionOffset;
        public Color color;
        public float radius;


        public FakeLightSource()
        {

        }

        public FakeLightSource(int ID, Vector2 Position, Color Color, float Raidus)
        {
            this.id = ID;
            this.positionOffset = Position;
            this.color = Color;
            this.radius = Raidus;
        }

    }
}
