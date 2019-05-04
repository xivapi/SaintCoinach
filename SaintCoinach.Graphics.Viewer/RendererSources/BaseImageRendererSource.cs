using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer.RendererSources {
    public abstract class BaseImageRendererSource : IImageRendererSource {
        #region Fields
        protected Engine _Engine;
        #endregion

        #region IImageRendererSource Members

        public string CurrentName { get; protected set; }

        public IComponent CurrentComponent { get; protected set; }

        public BoundingBox CurrentBoundingBox { get; protected set; }

        public System.IO.FileInfo CurrentTargetFile { get; protected set; }

        public bool RenderFromOppositeSide { get; protected set; }

        public abstract bool MoveNext();

        public virtual void Reset(Engine engine) {
            _Engine = engine;
        }

        #endregion
    }
}
