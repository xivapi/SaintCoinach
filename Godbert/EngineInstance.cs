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
            internal IEnumerable<IComponent> Replacement;
            internal string SetTitle;

            private FormEngine _Engine;
            private ComponentContainer _InnerContainer = new ComponentContainer();

            public ComponentInjector(FormEngine engine) {
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
                    foreach (var c in Replacement)
                        _InnerContainer.Add(c);
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
        public FormEngine Engine { get; private set; }
        #endregion

        #region Event
        public event EventHandler Stopped;
        #endregion

        #region Constructor
        public EngineInstance(string title) {
            Engine = new FormEngine(title);
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
            _Injector.Replacement = new IComponent[] { newComponent };
        }
        public void ReplaceComponents(IEnumerable<IComponent> newComponents) {
            _Injector.Replacement = newComponents;
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
            } catch (Exception e) {
                System.Windows.MessageBox.Show(e.ToString(), "Engine failure", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                System.Diagnostics.Debug.WriteLine(string.Format("Engine failure: {0}", e));
            } finally {
                Engine = null;
                _Injector = null;
                GC.Collect();
                var h = Stopped;
                if (h != null)
                    h(this, EventArgs.Empty);
            }
        }
        #endregion
    }
}
