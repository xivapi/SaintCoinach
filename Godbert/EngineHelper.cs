using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Godbert {
    using SaintCoinach.Graphics.Viewer;
    public class EngineHelper {
        public delegate IComponent ComponentFunction(Engine engine);

        #region Fields
        private List<EngineInstance> _Instances = new List<EngineInstance>();
        #endregion

        #region Things
        public EngineInstance AddToLast(string title, ComponentFunction func) {
            EngineInstance target = null;
            lock (_Instances) {
                if (_Instances.Count > 0)
                    target = _Instances[_Instances.Count - 1];
            }
            if (target == null)
                return OpenInNew(title, func);

            var component = func(target.Engine);
            target.AddComponent(component);
            target.SetTitle(target.Engine.Form.Text + ", " + title);
            return target;
        }
        public EngineInstance ReplaceInLast(string title, ComponentFunction func) {
            EngineInstance target = null;
            lock (_Instances) {
                if (_Instances.Count > 0)
                    target = _Instances[_Instances.Count - 1];
            }
            if (target == null)
                return OpenInNew(title, func);
            
            var component = func(target.Engine);
            target.ReplaceComponents(component);
            target.SetTitle(title);
            return target;
        }
        public EngineInstance OpenInNew(string title, ComponentFunction func) {
            var instance = new EngineInstance(title);
            lock (_Instances)
                _Instances.Add(instance);
            instance.Stopped += OnInstanceStopped;

            var component = func(instance.Engine);
            instance.AddComponent(component);
            instance.RunAsync();

            return instance;
        }

        public EngineInstance GetOrCreate(string title) {
            EngineInstance target = null;
            lock (_Instances) {
                if (_Instances.Count > 0)
                    target = _Instances[_Instances.Count - 1];
            }
            if (target == null)
                return OpenNew(title);
            return target;
        }
        public EngineInstance OpenNew(string title) {
            var instance = new EngineInstance(title);
            lock (_Instances)
                _Instances.Add(instance);
            instance.Stopped += OnInstanceStopped;

            instance.RunAsync();

            return instance;
        }

        void OnInstanceStopped(object sender, EventArgs e) {
            lock (_Instances)
                _Instances.Remove(sender as EngineInstance);
        }
        #endregion
    }
}
