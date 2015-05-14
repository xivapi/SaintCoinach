using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer {
    public interface IImageRendererSource {
        IComponent CurrentComponent { get; }
        BoundingBox CurrentBoundingBox { get; }
        FileInfo CurrentTargetFile { get; }
        string CurrentName { get; }
        bool RenderFromOppositeSide { get; }

        bool MoveNext();
        void Reset(Engine engine);
    }
}
