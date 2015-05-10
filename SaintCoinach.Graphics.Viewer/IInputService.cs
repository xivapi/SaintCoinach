using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer {
    using SharpDX;
    using SharpDX.Multimedia;
    using System.Windows.Forms;

    public interface IInputService {
        Point MousePosition { get; }
        IEnumerable<Keys> GetDownKeys();
        IEnumerable<MouseButtons> GetDownMouseButtons();
    }
}
