using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Godbert {
    using SaintCoinach.Graphics.Viewer;
    public class EngineInstance {
        #region Helper class
        class ComponentInjector : IUpdateableComponent, IDrawable3DComponent, IContentComponent {
            internal List<IComponent> AddQueue = new List<IComponent>();
            internal IComponent Replacement;
            internal string SetTitle;

            private Engine _Engine;
            private ComponentContainer _InnerContainer = new ComponentContainer();

            public ComponentInjector(Engine engine) {
                _Engine = engine;
            }

            #region IUpdateableComponent Members

            public bool IsEnabled {
                get { return true; }
            }

            public void Update(EngineTime engineTime) {
                if (!string.IsNullOrWhiteSpace(SetTitle)) {
                    _Engine.Form.Text = SetTitle;
                    SetTitle = null;
                }
                if (Replacement != null) {
                    lock (AddQueue)
                        AddQueue.Clear();
                    _InnerContainer.Clear();
                    _InnerContainer.Add(Replacement);
                    Replacement = null;
                }
                IComponent[] toAdd;
                lock (AddQueue) {
                    toAdd = AddQueue.ToArray();
                    AddQueue.Clear();
                }
                foreach (var c in toAdd)
                    _InnerContainer.Add(c);
                

                _InnerContainer.Update(engineTime);
            }

            #endregion

            #region IDrawable3DComponent Members

            public bool IsVisible {
                get { return true; }
            }

            public void Draw(EngineTime time, ref SharpDX.Matrix world, ref SharpDX.Matrix view, ref SharpDX.Matrix projection) {
                _InnerContainer.Draw(time, ref world, ref view, ref projection);
            }

            #endregion

            #region IContentComponent Members
            private bool _IsLoaded;
            public bool IsLoaded {
                get { return _IsLoaded; }
                private set { _IsLoaded = value; }
            }

            public void LoadContent() {
                _InnerContainer.LoadContent();
                _IsLoaded = true;
            }

            public void UnloadContent() {
                _IsLoaded = false;
                _InnerContainer.UnloadContent();
            }

            #endregion
        }
        #endregion

        #region Fields
        private ComponentInjector _Injector;
        #endregion

        #region Properties
        public Engine Engine { get; private set; }
        #endregion

        #region Event
        public event EventHandler Stopped;
        #endregion

        #region Constructor
        public EngineInstance(string title) {
            Engine = new Engine(title);
            Engine.Components.Add(_Injector = new ComponentInjector(Engine));
        }
        #endregion

        #region Things
        public void SetTitle(string newTitle) {
            _Injector.SetTitle = newTitle;
        }
        public void AddComponent(IComponent component) {
            lock (_Injector.AddQueue)
                _Injector.AddQueue.Add(component);
        }
        public void ReplaceComponents(IComponent newComponent) {
            _Injector.Replacement = newComponent;
        }
        public void RunAsync() {
            var t = new Thread(this.Run);
            t.IsBackground = true;
            t.Name = "Renderer";
            t.Start();
        }

        private void Run() {
            try {
                Engine.Run();
            } finally {
                var h = Stopped;
                if (h != null)
                    h(this, EventArgs.Empty);
            }
        }
        #endregion
    }
}
