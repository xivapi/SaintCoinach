using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer {
    using SharpDX;

    public class ImageRenderer : Engine {
        #region Input service
        class NullInputService : IInputService {
            public static readonly NullInputService Instance = new NullInputService();

            #region IInputService Members

            public SharpDX.Point MousePosition {
                get { return SharpDX.Point.Zero; }
            }

            public IEnumerable<System.Windows.Forms.Keys> GetDownKeys() {
                yield break;
            }

            public IEnumerable<System.Windows.Forms.MouseButtons> GetDownMouseButtons() {
                yield break;
            }

            #endregion
        }
        #endregion

        #region Fields
        private IImageRendererSource _Source;
        private int _RenderWidth;
        private int _RenderHeight;
        #endregion

        #region Constructor
        public ImageRenderer(IImageRendererSource source, int renderWidth, int renderHeight) {
            _Source = source;
            _RenderWidth = renderWidth;
            _RenderHeight = renderHeight;
        }
        #endregion

        #region Run
        public void Run() {
            const int CacheClearInterval = 500;

            SetUp(_RenderWidth, _RenderHeight);
            Initialize();
            Load();

            var i = 0;
            _Source.Reset(this);
            while (_Source.MoveNext()) {
                if (i++ % CacheClearInterval == 0) {
                    Unload();
                    Load();
                    GC.Collect();
                }
                try {
                    Components.Add(_Source.CurrentComponent);
                    SetUpCamera(_Source.CurrentBoundingBox);
                    EngineLoop(EngineTime.Zero);
                } catch (Exception e) {
                    Console.Error.WriteLine("Failed to render '{0}': {1}", _Source.CurrentName, e.Message);
                } finally {
                    Components.Clear();
                }
            }

            Unload();
        }
        #endregion

        #region Viewport
        protected virtual void SetUpCamera(Graphics.BoundingBox bbox) {
            const float Yaw = MathUtil.PiOverFour / 2;    // -MathUtil.PiOverTwo;
            const float Pitch = -MathUtil.PiOverFour / 3;  // MathUtil.PiOverTwo;

            Camera.Yaw = Yaw;
            Camera.Pitch = Pitch;

            var center = (bbox.PointA.ToDx3() + bbox.PointB.ToDx3()) / 2f;

            var dx = Math.Abs(bbox.PointA.X - bbox.PointB.X);
            var dy = Math.Abs(bbox.PointA.Y - bbox.PointB.Y);
            var dz = Math.Abs(bbox.PointA.Z - bbox.PointB.Z);

            var maxAxis = Math.Max(dx, Math.Max(dy, dz)) + .1f; // Add a bit because I'm too lazy to adjust FoV for the angle

            var backDirection = -Vector3.Transform(Vector3.BackwardRH, Camera.GetRotation());
            var d = -(maxAxis / 2) / Math.Sin(Camera.FoV / 2);

            Camera.CameraPosition = center + new Vector3(backDirection.X, backDirection.Y, backDirection.Z) * (float)d;
        }
        #endregion

        public override IInputService InputService {
            get { return NullInputService.Instance; }
        }

        public override bool IsActive {
            get { return true; }
        }

        protected override SharpDX.Direct3D11.Device CreateDevice(int width, int height) {
            return new SharpDX.Direct3D11.Device(SharpDX.Direct3D.DriverType.Hardware, SharpDX.Direct3D11.DeviceCreationFlags.None, SharpDX.Direct3D.FeatureLevel.Level_11_0);
        }

        protected override SharpDX.Direct3D11.Texture2D CreateRenderTarget(int width, int height) {
            var desc = new SharpDX.Direct3D11.Texture2DDescription {
                ArraySize = 1,
                BindFlags = SharpDX.Direct3D11.BindFlags.RenderTarget,
                CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags.None,
                Format = SharpDX.DXGI.Format.R8G8B8A8_UNorm,
                Height = height,
                Width = width,
                MipLevels = 1,
                OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.Shared,
                Usage = SharpDX.Direct3D11.ResourceUsage.Default,
                SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0)
            };
            return new SharpDX.Direct3D11.Texture2D(Device, desc);
        }

        protected override void Present() {
            if (!_Source.CurrentTargetFile.Directory.Exists)
                _Source.CurrentTargetFile.Directory.Create();
            SharpDX.Direct3D11.Texture2D.ToFile(Device.ImmediateContext, RenderTarget, SharpDX.Direct3D11.ImageFileFormat.Png, _Source.CurrentTargetFile.FullName);
        }

        protected override void Draw(EngineTime time) {
            Device.ImmediateContext.ClearRenderTargetView(RenderTargetView, SharpDX.Color.Transparent);

            base.Draw(time);
        }
    }
}
