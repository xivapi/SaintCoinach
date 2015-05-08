using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer {
    public abstract class Component : IUpdateableComponent, IContentComponent {

        #region IContentComponent Members

        public bool IsLoaded {
            get;
            protected set;
        }

        public virtual void LoadContent() {
            IsLoaded = true;
        }

        public virtual void UnloadContent() {
            IsLoaded = false;
        }

        #endregion

        #region IUpdateableComponent Members

        private bool _IsEnabled = true;
        public bool IsEnabled { get { return _IsEnabled; } set { _IsEnabled = value; } }

        public virtual void Update(EngineTime engineTime) {

        }

        #endregion
    }
}
