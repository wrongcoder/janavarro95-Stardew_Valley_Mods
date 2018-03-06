using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardustCore.UIUtilities.MenuComponents.Delegates.Functionality
{
    public class ButtonFunctionality
    {
        public DelegatePairing leftClick;
        public DelegatePairing rightClick;
        public DelegatePairing hover;

        public ButtonFunctionality(DelegatePairing LeftClick, DelegatePairing RightClick, DelegatePairing OnHover)
        {
            this.leftClick = LeftClick;
            this.rightClick = RightClick;
            this.hover = OnHover;
        }
    }
}
