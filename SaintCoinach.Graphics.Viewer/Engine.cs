using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using Buffer = SharpDX.Direct3D11.Buffer;
using Device = SharpDX.Direct3D11.Device;

namespace SaintCoinach.Graphics.Viewer {
    public class Engine {
        #region Fields
        private string _Title;
        private RenderForm _Form;
        private Device _Device;
        private SwapChain _SwapChain;

        private InputService _InputService;
        private Keyboard _Keyboard;
        private Camera _Camera;

        private Texture2D _RenderTarget;
        private RenderTargetView _RenderTargetView;

        private Texture2D _DepthStencil;
        private DepthStencilView _DepthStencilView;

        private Stopwatch _RunTimer;
        private long _TotalElapsedTime;

        private ComponentContainer _CoreComponents;
        private ComponentContainer _Components;

        private ModelFactory _ModelFactory;
        private TextureFactory _TextureFactory;
        private EffectFactory _EffectFactory;
        private MaterialFactory _MaterialFactory;
        #endregion

        #region Properties
        public RenderForm Form { get { return _Form; } }
        public Device Device { get { return _Device; } }
        public InputService InputService { get { return _InputService; } }
        public Keyboard Keyboard { get { return _Keyboard; } }
        public Camera Camera { get { return _Camera; } }

        protected ComponentContainer CoreComponents { get { return _CoreComponents; } }
        public ComponentContainer Components { get { return _Components; } }

        public ModelFactory ModelFactory { get { return _ModelFactory; } }
        public TextureFactory TextureFactory { get { return _TextureFactory; } }
        public EffectFactory EffectFactory { get { return _EffectFactory; } }
        public MaterialFactory MaterialFactory { get { return _MaterialFactory; } }
        #endregion

        #region Constructor
        public Engine(string title) {
            _Title = title;

            _CoreComponents = new ComponentContainer();
            _Components = new ComponentContainer();
        }
        #endregion

        #region Run
        public void Run() {
            using (_Form = new RenderForm(_Title)) {
                CreateDevice();
                Form.ClientSizeChanged += Form_ClientSizeChanged;

                Initialize();

                Load();

                _RunTimer = new Stopwatch();
                _RunTimer.Start();

                RenderLoop.Run(Form, EngineLoop);

                Unload();
            }
        }
        void Form_ClientSizeChanged(object sender, EventArgs e) {
            var newMode = new ModeDescription(
                Form.ClientSize.Width, Form.ClientSize.Height,
                new Rational(60, 1), Format.R8G8B8A8_UNorm);
            Device.ImmediateContext.OutputMerger.ResetTargets();

            _RenderTargetView.Dispose();
            _RenderTarget.Dispose();
            _DepthStencilView.Dispose();
            _DepthStencil.Dispose();

            _SwapChain.ResizeBuffers(1, Form.ClientSize.Width, Form.ClientSize.Height, Format.Unknown, SwapChainFlags.None);

            CreateView();
        }
        private void CreateDevice() {
            var desc = new SwapChainDescription {
                BufferCount = 1,
                Flags = SwapChainFlags.None,
                IsWindowed = true,
                ModeDescription = new ModeDescription(
                    Form.ClientSize.Width, Form.ClientSize.Height,
                    new Rational(60, 1), Format.R8G8B8A8_UNorm),
                OutputHandle = Form.Handle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput | Usage.BackBuffer,
            };

            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, desc, out _Device, out _SwapChain);

            var factory = _SwapChain.GetParent<Factory>();
            factory.MakeWindowAssociation(Form.Handle, WindowAssociationFlags.IgnoreAll);

            CreateView();

            var stencilStateDesc = new DepthStencilStateDescription {
                IsDepthEnabled = true,
                IsStencilEnabled = true,
                DepthWriteMask = DepthWriteMask.All,
                DepthComparison = Comparison.Less,
                StencilReadMask = 0xFF,
                StencilWriteMask = 0xFF,

                // Stencil operation if pixel front-facing.
                FrontFace = new DepthStencilOperationDescription() {
                    FailOperation = StencilOperation.Keep,
                    DepthFailOperation = StencilOperation.Increment,
                    PassOperation = StencilOperation.Keep,
                    Comparison = Comparison.Always
                },
                // Stencil operation if pixel is back-facing.
                BackFace = new DepthStencilOperationDescription() {
                    FailOperation = StencilOperation.Keep,
                    DepthFailOperation = StencilOperation.Decrement,
                    PassOperation = StencilOperation.Keep,
                    Comparison = Comparison.Always
                }
            };
            var stencilState = new DepthStencilState(Device, stencilStateDesc);

            var blendDesc = new BlendStateDescription();
            blendDesc.RenderTarget[0].IsBlendEnabled = true;
            blendDesc.RenderTarget[0].SourceBlend = BlendOption.SourceAlpha;
            blendDesc.RenderTarget[0].DestinationBlend = BlendOption.InverseSourceAlpha;
            blendDesc.RenderTarget[0].BlendOperation = BlendOperation.Add;
            blendDesc.RenderTarget[0].SourceAlphaBlend = BlendOption.One;
            blendDesc.RenderTarget[0].DestinationAlphaBlend = BlendOption.InverseSourceAlpha;
            blendDesc.RenderTarget[0].AlphaBlendOperation = BlendOperation.Add;
            blendDesc.RenderTarget[0].RenderTargetWriteMask = ColorWriteMaskFlags.All;
            var blendState = new BlendState(Device, blendDesc);

            Device.ImmediateContext.OutputMerger.SetDepthStencilState(stencilState);
            Device.ImmediateContext.OutputMerger.SetBlendState(blendState, new Color4(0, 0, 0, 0));
            Device.ImmediateContext.Rasterizer.State = new RasterizerState(Device, new RasterizerStateDescription {
                CullMode = CullMode.None,
                FillMode = FillMode.Solid,
                IsMultisampleEnabled = true,
            });


        }
        private void CreateView() {
            var rtDesc = new Texture2DDescription {
                ArraySize = 1,
                BindFlags = BindFlags.RenderTarget,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = _SwapChain.Description.ModeDescription.Format,
                Width = _SwapChain.Description.ModeDescription.Width,
                Height = _SwapChain.Description.ModeDescription.Height,
                MipLevels = 1,
                OptionFlags = ResourceOptionFlags.None,
                SampleDescription = new SampleDescription(8, Device.CheckMultisampleQualityLevels(_SwapChain.Description.ModeDescription.Format, 8)),
                Usage = ResourceUsage.Default
            };
            _RenderTarget = Texture2D.FromSwapChain<Texture2D>(_SwapChain, 0);
            //_RenderTarget = new Texture2D(Device, rtDesc);
            _RenderTargetView = new RenderTargetView(Device, _RenderTarget);

            var dsTexDesc = new Texture2DDescription {
                Format = Format.D24_UNorm_S8_UInt,
                ArraySize = 1,
                MipLevels = 1,
                Width = _RenderTarget.Description.Width,
                Height = _RenderTarget.Description.Height,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None
            };
            _DepthStencil = new Texture2D(Device, dsTexDesc);
            _DepthStencilView = new DepthStencilView(Device, _DepthStencil);

            Device.ImmediateContext.OutputMerger.SetTargets(_DepthStencilView, _RenderTargetView);

            Device.ImmediateContext.Rasterizer.SetViewport(new Viewport(0, 0, Form.ClientSize.Width, Form.ClientSize.Height));
        }
        #endregion

        #region Initialize
        protected virtual void Initialize() {
            _InputService = new InputService(Form);
            CoreComponents.Add(_Keyboard = new Keyboard(this));
            CoreComponents.Add(_Camera = new Camera(this));

            _ModelFactory = new ModelFactory(this);
            _TextureFactory = new TextureFactory(this);
            _EffectFactory = new EffectFactory(this);
            _MaterialFactory = new MaterialFactory(this);
        }
        #endregion

        #region Content
        private void Load() {
            CoreComponents.LoadContent();
            Components.LoadContent();
            LoadContent();
        }
        protected virtual void LoadContent() { }

        private void Unload() {
            Components.UnloadContent();
            CoreComponents.UnloadContent();

            ModelFactory.UnloadAll();
            MaterialFactory.UnloadAll();
            EffectFactory.UnloadAll();
            TextureFactory.UnloadAll();

            UnloadContent();
        }
        protected virtual void UnloadContent() { }
        #endregion

        #region Loop
        private void EngineLoop() {
            var elapsed = _RunTimer.Elapsed;
            _RunTimer.Restart();
            _TotalElapsedTime += elapsed.Ticks;
            var time = new EngineTime(TimeSpan.FromTicks(_TotalElapsedTime), elapsed);

            Update(time);
            Draw(time);

            _SwapChain.Present(0, PresentFlags.None);
        }

        protected virtual void Update(EngineTime time) {
            CoreComponents.Update(time);
            Components.Update(time);
        }
        protected virtual void Draw(EngineTime time) {
            var world = Matrix.Identity;
            var view = Camera.View;
            var proj = Camera.Projection;

            Device.ImmediateContext.ClearRenderTargetView(_RenderTargetView, Color.CornflowerBlue);
            Device.ImmediateContext.ClearDepthStencilView(_DepthStencilView, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1f, 0);
            CoreComponents.Draw(time, ref world, ref view, ref proj);
            Components.Draw(time, ref world, ref view, ref proj);
        }
        #endregion
    }
}
