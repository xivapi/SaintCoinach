using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Reflection;

namespace SaintCoinach.Graphics.Viewer {
    using SharpDX;
    using SharpDX.D3DCompiler;
    using SharpDX.Direct3D;
    using SharpDX.Direct3D11;
    using SharpDX.DXGI;
    using SharpDX.Windows;
    using Buffer = SharpDX.Direct3D11.Buffer;
    using Device = SharpDX.Direct3D11.Device;

    public class FormEngine : Engine {
        #region Fields
        private IInputService _InputService;

        private string _Title;
        private RenderForm _Form;
        #endregion

        #region Properties
        public override IInputService InputService {
            get { return _InputService; }
        }
        public RenderForm Form { get { return _Form; } }
        public override bool IsActive {
            get { return Form.ContainsFocus; }
        }
        #endregion

        #region Constructor
        public FormEngine(string title) {
            _Title = title;
        }
        #endregion

        #region Run
        public void Run() {
            using (_Form = new RenderForm(_Title)) {
                var assembly = Assembly.GetExecutingAssembly();
                using (var iconStream = assembly.GetManifestResourceStream("SaintCoinach.Graphics.Viewer.Viewer.ico"))
                    _Form.Icon = new System.Drawing.Icon(iconStream);

                CreateDevice(Form.Handle, Form.ClientSize.Width, Form.ClientSize.Height);
                Form.ClientSizeChanged += Form_ClientSizeChanged;

                _InputService = new FormInputService(Form);

                Initialize();
                Load();

                RenderLoop.Run(Form, EngineLoop);

                Unload();
            }
        }
        void Form_ClientSizeChanged(object sender, EventArgs e) {
            Resize(Form.ClientSize.Width, Form.ClientSize.Height);
        }
        #endregion
    }
}
