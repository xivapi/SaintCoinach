using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer {
    using SharpDX;

    public struct MouseState {
        public bool LeftButton { get; internal set; }
        public bool RightButton { get; internal set; }
        public bool MiddleButton { get; internal set; }

        public int MouseWheel { get; internal set; }

        public Point AbsolutePosition { get; internal set; }
        public Vector2 RelativePosition { get; internal set; }
    }
}
