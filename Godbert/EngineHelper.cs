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
        public delegate IComponent[] MultiComponentFunction(Engine engine);

        #region Fields
        private List<EngineInstance> _Instances = new List<EngineInstance>();
        #endregion

        #region Things
        public EngineInstance AddToLast(string title, ComponentFunction func) { return AddToLast(title, (e) => new IComponent[] { func(e) }); }
        public EngineInstance AddToLast(string title, MultiComponentFunction func) {
            EngineInstance target = null;
            lock (_Instances) {
                if (_Instances.Count > 0)
                    target = _Instances[_Instances.Count - 1];
            }
            if (target == null)
                return OpenInNew(title, func);

            target.AddComponent(func);
            target.SetTitle(target.Engine.Form.Text + ", " + title);
            return target;
        }
        public EngineInstance ReplaceInLast(string title, ComponentFunction func) { return ReplaceInLast(title, (e) => new IComponent[] { func(e) }); }
        public EngineInstance ReplaceInLast(string title, MultiComponentFunction func) {
            EngineInstance target = null;
            lock (_Instances) {
                if (_Instances.Count > 0)
                    target = _Instances[_Instances.Count - 1];
            }
            if (target == null)
                return OpenInNew(title, func);
            
            target.ReplaceComponents(func);
            target.SetTitle(title);
            return target;
        }
        public EngineInstance OpenInNew(string title, ComponentFunction func) { return OpenInNew(title, (e) => new IComponent[] { func(e) }); }
        public EngineInstance OpenInNew(string title, MultiComponentFunction func) {
            var instance = new EngineInstance(title);
            lock (_Instances)
                _Instances.Add(instance);
            instance.Stopped += OnInstanceStopped;
            
            instance.AddComponent(func);
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
