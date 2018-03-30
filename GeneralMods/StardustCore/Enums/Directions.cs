using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardustCore.Enums
{
    public class Directions
    {
        public enum Direction
        {
            [Description("Up")]
            up,
            [Description("Right")]
            right,
            [Description("Down")]
            down,
            [Description("Left")]
            left
        }


        public static Direction GetUp()
        {
            return Direction.up;
        }

        public static Direction GetDown()
        {
            return Direction.down;
        }

        public static Direction GetLeft()
        {
            return Direction.left;
        }

        public static Direction GetRight()
        {
            return Direction.right;
        }

        public static int DirectionToInt(Direction dir)
        {
            return (int)dir;
        }

        public static string getString(Direction dir)
        {
            return dir.GetEnumDescription();
        }
    }
}
